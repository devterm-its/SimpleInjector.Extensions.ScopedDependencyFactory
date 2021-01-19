using FluentAssertions;
using SimpleInjector.Extensions.ScopedDependencyFactory.Implementations;
using SimpleInjector.Lifestyles;
using System;
using Xunit;

namespace SimpleInjector.Extensions.ScopedDependencyFactory.Test.Implementations
{
    public class AsyncScopedDependencyFactoryShould
    {
        [Fact]
        public void AllowScopedRegistration()
        {
            var container = new Container();
            container.Options.DefaultLifestyle = new AsyncScopedLifestyle();
            container.Register<TestClass>();

            var scopedDependencyFactory = new AsyncScopedDependencyFactory<TestClass>(container);
            scopedDependencyFactory.Should().NotBeNull();
        }

        [Fact]
        public void RejectTransientLifestyle()
        {
            var container = new Container();
            container.Options.DefaultLifestyle = Lifestyle.Transient;
            container.Register<TestClass>();

            Action act = () => new AsyncScopedDependencyFactory<TestClass>(container);
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void RejectSingletonLifestyle()
        {
            var container = new Container();
            container.Options.DefaultLifestyle = Lifestyle.Singleton;
            container.Register<TestClass>();

            Action act = () => new AsyncScopedDependencyFactory<TestClass>(container);
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void RejectMissingType()
        {
            var container = new Container();

            Action act = () => new AsyncScopedDependencyFactory<TestClass>(container);
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void ProvideTheSameInstanceInTheSameScope()
        {
            var container = new Container();
            container.Options.DefaultLifestyle = new AsyncScopedLifestyle();
            container.Register<TestClass>();

            var scopedDependencyFactory = new AsyncScopedDependencyFactory<TestClass>(container);
            using (var provider = scopedDependencyFactory.ResolveDependency())
            {
                var instance1 = provider.Instance;
                var instance2 = provider.Instance;
                instance1.Should().Be(instance2);
            }
        }

        [Fact]
        public void ProvideDifferentInstancesInDifferentProviders()
        {
            var container = new Container();
            container.Options.DefaultLifestyle = new AsyncScopedLifestyle();
            container.Register<TestClass>();

            var scopedDependencyFactory = new AsyncScopedDependencyFactory<TestClass>(container);
            using (var provider1 = scopedDependencyFactory.ResolveDependency())
            using (var provider2 = scopedDependencyFactory.ResolveDependency())
            {
                provider1.Instance.Should().NotBeNull();
                provider2.Instance.Should().NotBeNull();
                provider1.Instance.Should().NotBe(provider2.Instance);
            }
        }

        [Fact]
        public void DisposeInstanceWhenDisposingProvider()
        {
            var container = new Container();
            container.Options.DefaultLifestyle = new AsyncScopedLifestyle();
            container.Register<TestClass>();

            var scopedDependencyFactory = new AsyncScopedDependencyFactory<TestClass>(container);
            var provider = scopedDependencyFactory.ResolveDependency();
            var instance = provider.Instance;

            instance.IsDisposed.Should().BeFalse();
            provider.Dispose();
            instance.IsDisposed.Should().BeTrue();
        }

        [Fact]
        public void DisposeAllUndisposedProviders()
        {
            var container = new Container();
            container.Options.DefaultLifestyle = new AsyncScopedLifestyle();
            container.Register<TestClass>();

            var scopedDependencyFactory = new AsyncScopedDependencyFactory<TestClass>(container);
            var provider = scopedDependencyFactory.ResolveDependency();
            var instance = provider.Instance;

            instance.IsDisposed.Should().BeFalse();
            scopedDependencyFactory.Dispose();
            instance.IsDisposed.Should().BeTrue();
        }

        [Fact]
        public void ThrowAfterDisposal()
        {
            var container = new Container();
            container.Options.DefaultLifestyle = new AsyncScopedLifestyle();
            container.Register<TestClass>();

            var scopedDependencyFactory = new AsyncScopedDependencyFactory<TestClass>(container);
            scopedDependencyFactory.Dispose();

            Action act = () => scopedDependencyFactory.ResolveDependency();

            act.Should().Throw<ObjectDisposedException>();
        }
    }

    class TestClass : IDisposable
    {
        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            IsDisposed = true;
        }
    }
}
