using SimpleInjector.Extensions.ScopedDependencyFactory.Implementations;
using SimpleInjector.Extensions.ScopedDependencyFactory.Interfaces;

namespace SimpleInjector
{
    public static class Extension
    {
        public static void AddAsyncScopedFactories(this Container container)
        {
            container.Register(typeof(IScopedDependencyFactory<>), typeof(AsyncScopedDependencyFactory<>), Lifestyle.Singleton);
        }
    }
}
