using Autofac;

namespace ZenAutofac.API.Builders
{
    public interface IExtendedBuilderBase
    {
        ContainerBuilder Builder { get; }
    }
}