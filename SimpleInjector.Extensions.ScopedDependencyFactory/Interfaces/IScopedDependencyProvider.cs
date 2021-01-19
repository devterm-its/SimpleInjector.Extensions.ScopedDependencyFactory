using System;

namespace SimpleInjector.Extensions.ScopedDependencyFactory.Interfaces
{
    public interface IScopedDependencyProvider<out T> : IDisposable
    {
        T Instance { get; }
    }
}
