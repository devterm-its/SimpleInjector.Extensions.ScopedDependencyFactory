using System;

namespace SimpleInjector.Extensions.ScopedDependencyFactory.Interfaces
{
    public interface IScopedDependencyFactory<out T> : IDisposable
    {
        IScopedDependencyProvider<T> ResolveDependency();
    }
}
