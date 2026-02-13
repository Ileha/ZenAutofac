using Autofac;
using Autofac.Features.AttributeFilters;
using NSubstitute;
using ZenAutofac;
using ZenAutofac.Entities;
using ZenAutofac.Interfaces;

namespace Tests;

[TestFixture]
public class DIExtensionsKeyFilterTests
{
    [Test]
    public void Register2KeyedServices_ResolveFromService()
    {
        //Arrange
        var builder = new ContainerBuilder();

        var serviceAData = Guid.NewGuid();
        var serviceBData = Guid.NewGuid();

        var serviceA = Substitute.For<IService>();
        serviceA.GetData().Returns(serviceAData);

        var serviceB = Substitute.For<IService>();
        serviceB.GetData().Returns(serviceBData);

        builder
            .RegisterInstance(serviceA)
            .Keyed<IService>(0)
            .SingleInstance();

        builder
            .RegisterInstance(serviceB)
            .Keyed<IService>(1)
            .SingleInstance();

        builder
            .RegisterType<ServiceHolder>()
            .WithAttributeFiltering()
            .SingleInstance();

        //Act
        using var container = builder.Build();

        using var holder = container.Resolve<ServiceHolder>();

        // Assert
        Assert.That(holder.A, Is.EqualTo(serviceA));
        Assert.That(holder.B, Is.EqualTo(serviceB));
    }

    [Test]
    public void Register2KeyedServices_ResolveFromServiceSubScope()
    {
        //Arrange
        var builder = new ContainerBuilder();

        var serviceAData = Guid.NewGuid();
        var serviceBData = Guid.NewGuid();

        var serviceA = Substitute.For<IService>();
        serviceA.GetData().Returns(serviceAData);

        var serviceB = Substitute.For<IService>();
        serviceB.GetData().Returns(serviceBData);

        builder
            .RegisterInstance(serviceA)
            .Keyed<IService>(0)
            .SingleInstance();

        builder
            .RegisterInstance(serviceB)
            .Keyed<IService>(1)
            .SingleInstance();

        builder
            .RegisterExtended<ServiceHolder>()
            .FromSubScope()
            .ByFunction(subContainerBuilder =>
            {
                subContainerBuilder
                    .RegisterType<ServiceHolder>()
                    .SingleInstance()
                    .WithAttributeFiltering();
            })
            .SingleInstance();

        //Act
        using var container = builder.Build();

        var holder = container.Resolve<ServiceHolder>();

        // Assert
        Assert.That(holder.A, Is.EqualTo(serviceA));
        Assert.That(holder.B, Is.EqualTo(serviceB));
    }

    [Test]
    public void Register2KeyedServices_ResolveFromFactory()
    {
        //Arrange
        var builder = new ContainerBuilder();

        var serviceAData = Guid.NewGuid();
        var serviceBData = Guid.NewGuid();

        var serviceA = Substitute.For<IService>();
        serviceA.GetData().Returns(serviceAData);

        var serviceB = Substitute.For<IService>();
        serviceB.GetData().Returns(serviceBData);

        builder
            .RegisterInstance(serviceA)
            .Keyed<IService>(0)
            .SingleInstance();

        builder
            .RegisterInstance(serviceB)
            .Keyed<IService>(1)
            .SingleInstance();

        builder
            .RegisterIFactoryExtended<ServiceHolder>()
            .FromNewInstance()
            .SingleInstance();

        //Act
        using var container = builder.Build();

        using var holder = container.Resolve<IFactory<ServiceHolder>>().Create();

        // Assert
        Assert.That(holder.A, Is.EqualTo(serviceA));
        Assert.That(holder.B, Is.EqualTo(serviceB));
    }

    [Test]
    public void Register2KeyedServices_ResolveFromFunctionFactory()
    {
        //Arrange
        var builder = new ContainerBuilder();

        var serviceAData = Guid.NewGuid();
        var serviceBData = Guid.NewGuid();

        var serviceA = Substitute.For<IService>();
        serviceA.GetData().Returns(serviceAData);

        var serviceB = Substitute.For<IService>();
        serviceB.GetData().Returns(serviceBData);

        builder
            .RegisterInstance(serviceA)
            .Keyed<IService>(0)
            .SingleInstance();

        builder
            .RegisterInstance(serviceB)
            .Keyed<IService>(1)
            .SingleInstance();

        builder
            .RegisterIFactoryExtended<ServiceHolder>()
            .FromFunction(scope => scope.CreateInstance<ServiceHolder>())
            .SingleInstance();

        //Act
        using var container = builder.Build();

        using var holder = container.Resolve<IFactory<ServiceHolder>>().Create();

        // Assert
        Assert.That(holder.A, Is.EqualTo(serviceA));
        Assert.That(holder.B, Is.EqualTo(serviceB));
    }

    [Test]
    public void Register2KeyedServices_ResolveFromSubScopeFactory()
    {
        //Arrange
        var builder = new ContainerBuilder();

        var serviceAData = Guid.NewGuid();
        var serviceBData = Guid.NewGuid();

        var serviceA = Substitute.For<IService>();
        serviceA.GetData().Returns(serviceAData);

        var serviceB = Substitute.For<IService>();
        serviceB.GetData().Returns(serviceBData);

        builder
            .RegisterInstance(serviceA)
            .Keyed<IService>(0)
            .SingleInstance();

        builder
            .RegisterInstance(serviceB)
            .Keyed<IService>(1)
            .SingleInstance();

        builder
            .RegisterIFactoryExtended<ServiceHolder>()
            .FromSubScope()
            .ByFunction(subContainerBuilder =>
            {
                subContainerBuilder
                    .RegisterType<ServiceHolder>()
                    .SingleInstance()
                    .WithAttributeFiltering();
            })
            .SingleInstance();

        //Act
        using var container = builder.Build();

        using var holder = container.Resolve<IFactory<ServiceHolder>>().Create();

        // Assert
        Assert.That(holder.A, Is.EqualTo(serviceA));
        Assert.That(holder.B, Is.EqualTo(serviceB));
    }

    public class ServiceHolder : Disposer
    {
        public IService A { get; }
        public IService B { get; }

        public ServiceHolder(
            [KeyFilter(0)]
            IService a,
            [KeyFilter(1)]
            IService b)
        {
            A = a;
            B = b;
        }
    }

    public interface IService
    {
        Guid GetData();
    }
}