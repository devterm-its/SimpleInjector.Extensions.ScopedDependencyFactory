using SimpleInjector.Extensions.ScopedDependencyFactory.Interfaces;

namespace SimpleInjector.Extensions.ScopedDependencyFactory.Implementations
{
    class ScopedDependencyProvider<T> : IScopedDependencyProvider<T>
    {
        private readonly Scope _scope;
        private readonly object _scopeLock = new object();

        public ScopedDependencyProvider(Scope scope)
        {
            _scope = scope;
            Instance = (T)_scope.GetInstance(typeof(T));
        }

        public T Instance { get; }

        public void Dispose()
        {
            lock (_scopeLock)
                _scope?.Dispose();
        }
    }
}
