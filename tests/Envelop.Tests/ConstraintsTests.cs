using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Envelop.Tests.TestDependencies;
using Xunit;

namespace Envelop.Tests
{
    public class ConstraintsTests
    {
        [Fact]
        public void BindToWhen()
        {
            var kernel = Kernel.Create();
            kernel.Bind<ISomeInterface>().To<SomeInterfaceImplementation>();
            kernel.Bind<ISomeInterface>().To<SomeInterfaceImplementation2>().When(req => typeof(IAnotherInterface).IsAssignableFrom(req.Target)); 
            kernel.Bind<IAnotherInterface>().To<AnotherInterfaceImplementation>();

            var t1 = kernel.Resolve<ISomeInterface>();
            var t2 = kernel.Resolve<IAnotherInterface>();

            Assert.NotNull(t1);
            Assert.NotNull(t2);

            Assert.True(t1 is SomeInterfaceImplementation);
            Assert.True(t2.SomeInterface is SomeInterfaceImplementation2);
        }


        [Fact]
        public void BindInterfaceToClassAsSingleton()
        {
            var kernel = Kernel.Create();
            kernel.Bind<ISomeInterface>().To<SomeInterfaceImplementation>().AsSingleton();

            var t1 = kernel.Resolve<ISomeInterface>();
            var t2 = kernel.Resolve<ISomeInterface>();
            var t3 = kernel.Resolve<ISomeInterface>();

            Assert.NotNull(t1);
            Assert.NotNull(t2);
            Assert.NotNull(t3);

            Assert.Same(t1, t2);
            Assert.Same(t2, t3);
        }

        [Fact]
        public void BindInterfaceToBuilderAsSingleton()
        {
            var kernel = Kernel.Create();
            kernel.Bind<ISomeInterface>().To(req => new SomeInterfaceImplementation()).AsSingleton();

            var t1 = kernel.Resolve<ISomeInterface>();
            var t2 = kernel.Resolve<ISomeInterface>();
            var t3 = kernel.Resolve<ISomeInterface>();

            Assert.NotNull(t1);
            Assert.NotNull(t2);
            Assert.NotNull(t3);

            Assert.Same(t1, t2);
            Assert.Same(t2, t3);
        }

        [Fact]
        public void BindInterfaceToBuilderWithDeactivation()
        {
            var value = false;

            using (var kernel = Kernel.Create())
            {

                kernel.Bind<ISomeInterface>()
                    .To(req => new SomeInterfaceImplementation())
                    .AfterDeactivation(_ => value = true);

                var t1 = kernel.Resolve<ISomeInterface>();

                Assert.NotNull(t1);
                Assert.False(value);
            }

            Assert.True(value);
        }
    }
}
