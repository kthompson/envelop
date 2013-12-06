using System;
using NUnit.Framework;
using Envelop.Tests.TestDependencies;

namespace Envelop.Tests
{
    [TestFixture]
    public class ScopeTests
    {
        [Test]
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

        [Test]
        public void ScopedDeactivationTests ()
        {
            var kernel = Kernel.Create ();
            IIsDisposable disposable;
            IIsDisposable scopedDisposable;

            using (kernel) 
            {
                kernel.Bind<IIsDisposable> ().To<IsDisposable> ();

                disposable = kernel.Resolve<IIsDisposable> ();
                Assert.IsNotNull (disposable);
                Assert.IsFalse (disposable.IsDisposed);

                using (var scope = kernel.CreateScope ()) 
                {
                    scopedDisposable = scope.Resolve<IIsDisposable> ();

                    Assert.IsNotNull (scopedDisposable);
                    Assert.IsFalse (scopedDisposable.IsDisposed);
                }

                Assert.IsTrue (scopedDisposable.IsDisposed);
                Assert.IsFalse (disposable.IsDisposed);
            }

            Assert.IsTrue (disposable.IsDisposed);
        }

        [Test]
        public void ScopedDeactivationTests2 ()
        {
            var disposed = 0;
            var kernel = Kernel.Create ();
            ISomeInterface rootObject;
            ISomeInterface scopedObject;

            using (kernel) 
            {
                kernel.Bind<ISomeInterface> ().To<SomeInterfaceImplementation> ().AfterDeactivation(_ => disposed++);

                rootObject = kernel.Resolve<ISomeInterface> ();	

                Assert.IsNotNull (rootObject);
                Assert.AreEqual (0, disposed);

                using (var scope = kernel.CreateScope ()) 
                {
                    scopedObject = scope.Resolve<ISomeInterface> ();

                    Assert.IsNotNull (scopedObject);
                    Assert.AreEqual (0, disposed);
                }

                Assert.AreEqual (1, disposed);
            }

            Assert.AreEqual (2, disposed);
        }
    }
}