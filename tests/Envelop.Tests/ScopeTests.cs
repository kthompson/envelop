using System;
using Envelop.Tests.TestDependencies;
using Xunit;

namespace Envelop.Tests
{
    public class ScopeTests
    {
        [Fact]
        public void DeactivationTests ()
        {
            var kernel = Kernel.Create ();
            IIsDisposable disposable;
            using (kernel) 
            {
                kernel.Bind<IIsDisposable> ().To<IsDisposable> ();

                disposable = kernel.Resolve<IIsDisposable> ();
                Assert.False (disposable.IsDisposed);
            }

            Assert.True (disposable.IsDisposed);
        }

        [Fact]
        public void ScopedDeactivationTests ()
        {
            var kernel = Kernel.Create ();
            IIsDisposable disposable;
            IIsDisposable scopedDisposable;

            using (kernel) 
            {
                kernel.Bind<IIsDisposable> ().To<IsDisposable> ();

                disposable = kernel.Resolve<IIsDisposable> ();
                Assert.NotNull (disposable);
                Assert.False (disposable.IsDisposed);

                using (var scope = kernel.CreateScope ()) 
                {
                    scopedDisposable = scope.Resolve<IIsDisposable> ();

                    Assert.NotNull (scopedDisposable);
                    Assert.False (scopedDisposable.IsDisposed);
                }

                Assert.True (scopedDisposable.IsDisposed);
                Assert.False (disposable.IsDisposed);
            }

            Assert.True (disposable.IsDisposed);
        }

        [Fact]
        public void ScopedDeactivationTests2 ()
        {
            var disposed = 0;
            var kernel = Kernel.Create ();

            using (kernel) 
            {
                kernel.Bind<ISomeInterface> ().To<SomeInterfaceImplementation> ().AfterDeactivation(_ => disposed++);

                var rootObject = kernel.Resolve<ISomeInterface> ();

                Assert.NotNull (rootObject);
                Assert.Equal (0, disposed);

                using (var scope = kernel.CreateScope ()) 
                {
                    var scopedObject = scope.Resolve<ISomeInterface> ();

                    Assert.NotNull (scopedObject);
                    Assert.Equal (0, disposed);
                }

                Assert.Equal (1, disposed);
            }

            Assert.Equal (2, disposed);
        }
    }
}