using System;
using Autofac;
using ZenAutofac.API.Builders.Instance;

namespace ZenAutofac.Builders.Instance
{
    public class ExtendedRegistrationBuilder<TComponent> : IExtendedRegistrationBuilder<TComponent>
        where TComponent : class
    {
        public ContainerBuilder Builder { get; }

        public ExtendedRegistrationBuilder(ContainerBuilder builder)
        {
            Builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }
    }
}