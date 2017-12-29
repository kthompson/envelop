using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Envelop.Tests.TestDependencies;
using Xunit;

namespace Envelop.Tests
{
    public class InjectionTests
    {
        [Fact]
        public void InjectionTest()
        {
            var kernel = Kernel.Create();
            kernel.Bind<ISomeInterface>().To<SomeInterfaceImplementation>();
            kernel.Bind<IAnotherInterface>().To<AnotherInterfaceImplementation>();

            var t1 = kernel.Resolve<IAnotherInterface>();

            Assert.NotNull(t1);
            Assert.NotNull(t1.SomeInterface);
        }

        [Fact]
        public void MultiInjectionArrayTest()
        {
            var kernel = Kernel.Create();
            kernel.Bind<ISomeInterface>().To<SomeInterfaceImplementation>();
            kernel.Bind<ISomeInterface>().To<SomeInterfaceImplementation2>();
            kernel.Bind<IMultiInterface>().To<MultiInterfaceImplementation>();

            var t1 = kernel.Resolve<IMultiInterface>();

            Assert.NotNull(t1);
            Assert.NotNull(t1.SomeInterfaces);
            Assert.Equal(2, t1.SomeInterfaces.Length);

            Assert.True(t1.SomeInterfaces.OfType<SomeInterfaceImplementation2>().Count() == 1);
            Assert.True(t1.SomeInterfaces.OfType<SomeInterfaceImplementation>().Count() == 1);
        }

        [Fact]
        public void MultiInjectionIEnumerableTest()
        {
            var kernel = Kernel.Create();
            kernel.Bind<ISomeInterface>().To<SomeInterfaceImplementation>();
            kernel.Bind<ISomeInterface>().To<SomeInterfaceImplementation2>();
            kernel.Bind<IMultiInterface>().To<MultiInterfaceImplementation2>();

            var t1 = kernel.Resolve<IMultiInterface>();

            Assert.NotNull(t1);
            Assert.NotNull(t1.SomeInterfaces);
            Assert.Equal(2, t1.SomeInterfaces.Length);
            Assert.True(t1.SomeInterfaces.OfType<SomeInterfaceImplementation2>().Count() == 1);
            Assert.True(t1.SomeInterfaces.OfType<SomeInterfaceImplementation>().Count() == 1);
        }

        [Fact]
        public void MultiInjectionListTest()
        {
            var kernel = Kernel.Create();
            kernel.Bind<ISomeInterface>().To<SomeInterfaceImplementation>();
            kernel.Bind<ISomeInterface>().To<SomeInterfaceImplementation2>();
            kernel.Bind<IMultiInterface>().To<MultiInterfaceImplementation3>();

            var t1 = kernel.Resolve<IMultiInterface>();

            Assert.NotNull(t1);
            Assert.NotNull(t1.SomeInterfaces);
            Assert.Equal(2, t1.SomeInterfaces.Length);

            Assert.True(t1.SomeInterfaces.OfType<SomeInterfaceImplementation2>().Count() == 1);
            Assert.True(t1.SomeInterfaces.OfType<SomeInterfaceImplementation>().Count() == 1);
        }
    }
}
