using Autofac;
using Autofac.Core;
using NSubstitute;
using ZenAutofac;

namespace Tests
{
    // Test module for testing RegisterModuleWithArgs
    public class TestModule : Module
    {
        public string StringValue { get; }
        public int IntValue { get; }

        public TestModule(string stringValue, int intValue)
        {
            StringValue = stringValue;
            IntValue = intValue;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(this).AsSelf();
        }
    }

    // Another test module for testing module chaining
    public class ChainedModule : Module
    {
        public string Value { get; }

        public ChainedModule() : this("default")
        {
        }

        public ChainedModule(string value)
        {
            Value = value;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(this).AsSelf();
        }
    }

    // Module that throws an exception during loading
    public class ThrowingModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            throw new InvalidOperationException("Test exception during module load");
        }
    }

    [TestFixture]
    public class DIExtensionsTests
    {
        [Test]
        public void RegisterModuleWithArgs_WhenCalled_RegistersModuleWithProvidedParameters()
        {
            //Arrange
            var builder = new ContainerBuilder();
            var expectedString = Guid.NewGuid().ToString();
            var expectedInt = new Random().Next();

            //Act
            builder.RegisterModuleWithArgs<TestModule>(
                new NamedParameter("stringValue", expectedString),
                new NamedParameter("intValue", expectedInt));

            using var container = builder.Build();

            // Assert
            var module = container.Resolve<TestModule>();
            Assert.Multiple(() =>
            {
                Assert.That(module.StringValue, Is.EqualTo(expectedString));
                Assert.That(module.IntValue, Is.EqualTo(expectedInt));
            });
        }

        [Test]
        public void RegisterModuleWithArgs_WhenCalledWithNoParameters_RegistersModuleWithDefaultConstructor()
        {
            // Arrange
            var builder = new ContainerBuilder();

            // Act - TestModule has a parameterless constructor that sets default values
            builder.RegisterModuleWithArgs<ChainedModule>();

            using var container = builder.Build();

            // Assert - Should not throw when resolving
            Assert.DoesNotThrow(() => container.Resolve<ChainedModule>());
        }

        [Test]
        public void RegisterModuleWithArgs_WhenCalledWithMultipleModules_RegistersAllModules()
        {
            // Arrange
            var builder = new ContainerBuilder();
            const string value1 = "first";
            const string value2 = "second";

            // Act
            builder.RegisterModuleWithArgs<ChainedModule>(new NamedParameter("value", value1));
            builder.RegisterModuleWithArgs<ChainedModule>(new NamedParameter("value", value2));

            using var container = builder.Build();

            // Assert - Should resolve all registered modules
            var modules = container.Resolve<IEnumerable<ChainedModule>>().ToList();
            Assert.Multiple(() =>
            {
                Assert.That(modules, Has.Count.EqualTo(2));
                Assert.That(modules.Select(m => m.Value), Is.EquivalentTo(new[] { value1, value2 }));
            });
        }

        [Test]
        public void RegisterModuleWithArgs_WhenModuleThrowsInLoad_ThrowsDependencyResolutionException()
        {
            // Arrange
            var builder = new ContainerBuilder();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(
                () =>
                {
                    builder.RegisterModuleWithArgs<ThrowingModule>();
                    builder.Build();
                });
        }

        [Test]
        public void RegisterModuleWithArgs_WhenCalledWithNullBuilder_ThrowsArgumentNullException()
        {
            // Arrange
            ContainerBuilder builder = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                builder.RegisterModuleWithArgs<TestModule>(
                    new NamedParameter("stringValue", "test"),
                    new NamedParameter("intValue", 1)));
        }

        [Test]
        public void RegisterModuleWithArgs_WhenCalledWithNullParameters_RegistersModuleWithDefaultConstructor()
        {
            // Arrange
            var builder = new ContainerBuilder();

            // Act - Should use parameterless constructor
            builder.RegisterModuleWithArgs<ChainedModule>();

            using var container = builder.Build();

            // Assert - Should not throw when resolving
            Assert.DoesNotThrow(() => container.Resolve<ChainedModule>());
        }

        [Test]
        public void WithParameters_WhenCalled_PassesParametersToRegistration()
        {
            // Arrange
            var builder = new ContainerBuilder();
            const string expectedParameter = "test_parameter";
            builder.RegisterType<ServiceWithParameter>()
                .WithParameters(new NamedParameter("data", expectedParameter));

            using var container = builder.Build();

            // Act
            var instance = container.Resolve<ServiceWithParameter>();

            // Assert
            Assert.That(instance.Data, Is.EqualTo(expectedParameter));
        }

        [Test]
        public void CreateInstance_ForServiceProvider_CreatesInstanceCorrectly()
        {
            // Arrange
            var builder = new ContainerBuilder();
            builder
                .RegisterType<SimpleService>()
                .SingleInstance();

            using var container = builder.Build();
            var serviceProvider = container.AsServiceProvider();

            // Act
            var instance = serviceProvider.CreateInstance<ServiceWithDependency>();

            // Assert
            Assert.IsNotNull(instance);
            Assert.IsNotNull(instance.Dependency);
            Assert.That(instance.Dependency, Is.EqualTo(container.Resolve<SimpleService>()));
        }

        [Test]
        public void CreateInstance_ForComponentContext_CreatesInstanceCorrectly()
        {
            // Arrange
            var builder = new ContainerBuilder();
            builder
                .RegisterType<SimpleService>()
                .SingleInstance();

            using var container = builder.Build();

            // Act
            var instance = container.CreateInstance<ServiceWithDependency>();

            // Assert
            Assert.IsNotNull(instance);
            Assert.IsNotNull(instance.Dependency);
            Assert.That(instance.Dependency, Is.EqualTo(container.Resolve<SimpleService>()));
        }

        [Test]
        public void AsServiceProvider_WhenCalled_ResolvesServices()
        {
            // Arrange
            var builder = new ContainerBuilder();
            builder.RegisterType<SimpleService>().As<IService>();
            using var container = builder.Build();
            var serviceProvider = container.AsServiceProvider();

            // Act
            var service = serviceProvider.GetService(typeof(IService));

            // Assert
            Assert.IsNotNull(service);
            Assert.IsInstanceOf<SimpleService>(service);
        }

        [Test]
        public void RegisterFromSubScope_GotDependencyFromSubScope()
        {
            // Arrange
            var builder = new ContainerBuilder();

            var externalInstance = new SimpleService();
            var subScopeInstance = new SimpleService();

            builder
                .RegisterInstance(externalInstance)
                .As<SimpleService>()
                .SingleInstance();

            builder
                .RegisterExtended<ServiceWithDependencyDisposable>()
                .FromSubScope()
                .ByFunction(
                    subContainerBuilder =>
                    {
                        subContainerBuilder
                            .RegisterType<ServiceWithDependencyDisposable>()
                            .SingleInstance();

                        subContainerBuilder
                            .RegisterInstance(subScopeInstance)
                            .As<SimpleService>()
                            .SingleInstance();
                    })
                .SingleInstance();

            using var container = builder.Build();

            // Act
            var instance = container.Resolve<ServiceWithDependencyDisposable>();

            // Assert
            Assert.That(instance.Dependency, Is.EqualTo(subScopeInstance));
        }

        [Test]
        public void RegisterFromSubScope_SubContainerDisposedWithInstance()
        {
            // Arrange
            var builder = new ContainerBuilder();

            var mockSubContainerDisposable = Substitute.For<IDisposable>();
            var mockExternalDisposable = Substitute.For<IDisposable>();

            builder
                .RegisterInstance(mockExternalDisposable)
                .As<IDisposable>()
                .SingleInstance();

            builder
                .RegisterExtended<ServiceWithDependencyDisposable>()
                .FromSubScope()
                .ByFunction(
                    subContainerBuilder =>
                    {
                        subContainerBuilder
                            .RegisterType<ServiceWithDependencyDisposable>()
                            .SingleInstance();

                        subContainerBuilder
                            .RegisterType<SimpleService>()
                            .SingleInstance();

                        subContainerBuilder
                            .RegisterInstance(mockSubContainerDisposable)
                            .As<IDisposable>()
                            .SingleInstance();
                    })
                .SingleInstance()
                .ExternallyOwned();

            using var container = builder.Build();

            // Act
            using (var _ = container.Resolve<ServiceWithDependencyDisposable>())
            {
            }

            // Assert
            mockSubContainerDisposable.Received(1).Dispose();
            mockExternalDisposable.Received(0).Dispose();
        }

        internal class ServiceWithDependencyInstaller : Module
        {
            private readonly SimpleService _service;

            public ServiceWithDependencyInstaller(SimpleService service)
            {
                _service = service ?? throw new ArgumentNullException(nameof(service));
            }

            protected override void Load(ContainerBuilder builder)
            {
                builder
                    .RegisterType<ServiceWithDependencyDisposable>()
                    .SingleInstance();

                builder
                    .RegisterInstance(_service)
                    .As<SimpleService>()
                    .SingleInstance();
            }
        }

        [Test]
        public void Register_FromSubScope_ByInstaller()
        {
            // Arrange
            var builder = new ContainerBuilder();

            var externalInstance = new SimpleService();
            var internalService = new SimpleService();

            builder
                .RegisterInstance(externalInstance)
                .As<SimpleService>()
                .SingleInstance();

            builder
                .RegisterExtended<ServiceWithDependencyDisposable>()
                .FromSubScope()
                .ByModule<ServiceWithDependencyInstaller>((scope)
                    => scope.CreateInstance<ServiceWithDependencyInstaller>(internalService))
                .SingleInstance();

            using var container = builder.Build();

            // Act
            var instance = container.Resolve<ServiceWithDependencyDisposable>();

            // Assert
            Assert.That(instance.Dependency, Is.EqualTo(internalService));
        }

        [Test]
        public void Register_FromSubScope_ByInstaller_WhenExternalDependencies()
        {
            // Arrange
            var builder = new ContainerBuilder();

            var externalInstance = new SimpleService();

            builder
                .RegisterInstance(externalInstance)
                .As<SimpleService>()
                .SingleInstance();

            builder
                .RegisterExtended<ServiceWithDependencyDisposable>()
                .FromSubScope()
                .ByModule<ServiceWithDependencyInstaller>()
                .SingleInstance();

            using var container = builder.Build();

            // Act
            var instance = container.Resolve<ServiceWithDependencyDisposable>();

            // Assert
            Assert.That(instance.Dependency, Is.EqualTo(externalInstance));
        }

        // Test Classes
        public interface IService
        {
        }

        public class SimpleService : IService
        {
        }

        public class ServiceWithParameter
        {
            public ServiceWithParameter(string data)
            {
                Data = data;
            }

            public string Data { get; }
        }

        public class ServiceWithDependency
        {
            public ServiceWithDependency(SimpleService dependency)
            {
                Dependency = dependency;
            }

            public SimpleService Dependency { get; }
        }

        public class ServiceWithDependencyDisposable : ServiceWithDependency, IDisposer
        {
            private readonly IDisposer _disposerImplementation;

            public ServiceWithDependencyDisposable(SimpleService dependency)
                : base(dependency)
            {
                _disposerImplementation = new ZenAutofac.Entities.Disposer();
            }

            public ValueTask DisposeAsync()
            {
                return _disposerImplementation.DisposeAsync();
            }

            public void AddInstanceForDisposal(IDisposable instance)
            {
                _disposerImplementation.AddInstanceForDisposal(instance);
            }

            public void AddInstanceForAsyncDisposal(IAsyncDisposable instance)
            {
                _disposerImplementation.AddInstanceForAsyncDisposal(instance);
            }

            public void Dispose()
            {
                _disposerImplementation.Dispose();
            }
        }
    }
}