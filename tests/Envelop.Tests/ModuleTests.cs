using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Envelop.Tests.TestDependencies;
using Xunit;

namespace Envelop.Tests
{
    public class ModuleTests
    {
        class SimpleModuleTestModule : Module
        {
            protected override void Load()
            {
                this.Bind<ISomeInterface>().To<SomeInterfaceImplementation>();
                this.Bind<ISomeInterface>().To<SomeInterfaceImplementation2>();
                this.Bind<IMultiInterface>().To<MultiInterfaceImplementation>();
            }
        }

        class SimpleModuleTestModule2 : Module
        {
            protected override void Load()
            {
                this.Bind(typeof(ISomeInterface)).To(typeof(SomeInterfaceImplementation));
                this.Register(typeof(SomeInterfaceImplementation2));
                this.Register(typeof(IMultiInterface), typeof(MultiInterfaceImplementation));
            }
        }

        [Fact]
        public void SimpleModuleTest()
        {
            var kernel = Kernel.Create();
            kernel.Load(new SimpleModuleTestModule());

            var t1 = kernel.Resolve<IMultiInterface>();

            Assert.NotNull(t1);
            Assert.NotNull(t1.SomeInterfaces);
            Assert.Equal(2, t1.SomeInterfaces.Length);
            
            Assert.True(t1.SomeInterfaces.OfType<SomeInterfaceImplementation2>().Count() == 1);
            Assert.True(t1.SomeInterfaces.OfType<SomeInterfaceImplementation>().Count() == 1);
        }

        [Fact]
        public void SimpleModuleTest2()
        {
            var kernel = Kernel.Create();
            kernel.Load(new SimpleModuleTestModule2());

            var t1 = kernel.Resolve<IMultiInterface>();

            Assert.NotNull(t1);
            Assert.NotNull(t1.SomeInterfaces);
            Assert.Single(t1.SomeInterfaces);
            Assert.True(t1.SomeInterfaces[0] is SomeInterfaceImplementation);
        }

        [Fact]
        public void SimpleModuleTest3()
        {
            var kernel = Kernel.Create();
            kernel.Load(new SimpleModuleTestModule2());

            var t1 = kernel.Resolve<SomeInterfaceImplementation2>();

            Assert.NotNull(t1);
        }

        [Fact]
        public void SimpleAssemblyModuleTest()
        {
            var kernel = Kernel.Create();
            kernel.Load(typeof(SimpleModuleTestModule).Assembly);

            var t1 = kernel.Resolve<IMultiInterface>();

            Assert.NotNull(t1);
            Assert.NotNull(t1.SomeInterfaces);
            Assert.Equal(3, t1.SomeInterfaces.Length);
            Assert.True(t1.SomeInterfaces[0] is SomeInterfaceImplementation);
            Assert.True(t1.SomeInterfaces[1] is SomeInterfaceImplementation2);
            Assert.True(t1.SomeInterfaces[2] is SomeInterfaceImplementation);
        }

        [Fact(Skip = "Removed feature for now for PCL support.")]
        public void SimpleAssemblyFileModuleTest()
        {
            var kernel = Kernel.Create();
            //kernel.Load(typeof(SimpleModuleTestModule).Assembly.Location);

            var t1 = kernel.Resolve<IMultiInterface>();

            Assert.NotNull(t1);
            Assert.NotNull(t1.SomeInterfaces);
            Assert.Equal(3, t1.SomeInterfaces.Length);
            Assert.True(t1.SomeInterfaces[0] is SomeInterfaceImplementation);
            Assert.True(t1.SomeInterfaces[1] is SomeInterfaceImplementation2);
            Assert.True(t1.SomeInterfaces[2] is SomeInterfaceImplementation);
        }
    }
}
