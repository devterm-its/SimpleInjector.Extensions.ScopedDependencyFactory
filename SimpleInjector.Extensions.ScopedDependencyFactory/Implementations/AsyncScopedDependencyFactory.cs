using System;
using System.Collections.Generic;
using SimpleInjector.Extensions.ScopedDependencyFactory.Interfaces;
using SimpleInjector.Lifestyles;

namespace SimpleInjector.Extensions.ScopedDependencyFactory.Implementations
{
    public class AsyncScopedDependencyFactory<T> : IScopedDependencyFactory<T>
    {
        private List<IScopedDependencyProvider<T>> _providers;
        private readonly Container _container;
        private readonly object _providerLock = new object();

        public AsyncScopedDependencyFactory(Container container)
        {
            var registration = container.GetRegistration(typeof(T));
            if (registration == null)
            {
                throw new ArgumentException($"Cannot create {nameof(AsyncScopedDependencyFactory<T>)}: Type {typeof(T)} is not registered.");
            }

            if (!(registration.Lifestyle is AsyncScopedLifestyle))
            {
                throw new ArgumentException($"Cannot create {nameof(AsyncScopedDependencyFactory<T>)}: Type {typeof(T)} is not registerd with an Async Scoped Lifestyle");
            }
            _container = container;
            _providers = new List<IScopedDependencyProvider<T>>();
        }

        public IScopedDependencyProvider<T> ResolveDependency()
        {
            lock (_providerLock)
            {
                if (_providers == null)
                    throw new ObjectDisposedException($"{typeof(T)} Provider");
            }

            var provider = new ScopedDependencyProvider<T>(AsyncScopedLifestyle.BeginScope(_container));
            lock (_providerLock)
                _providers.Add(provider);

            return provider;
        }

        public void Dispose()
        {
            lock (_providerLock)
            {
                if (_providers != null)
                {
                    foreach (var provider in _providers)
                    {
                        provider?.Dispose();
                    }

                    _providers = null;
                }
            }
        }
    }
}
