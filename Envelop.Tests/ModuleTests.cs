using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Envelop.Tests.TestDependencies;
using NUnit.Framework;

namespace Envelop.Tests
{
    [TestFixture]
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

        [Test]
        public void SimpleModuleTest()
        {
            var kernel = Kernel.Create();
            kernel.Load(new SimpleModuleTestModule());

            var t1 = kernel.Resolve<IMultiInterface>();

            Assert.NotNull(t1);
            Assert.NotNull(t1.SomeInterfaces);
            Assert.AreEqual(2, t1.SomeInterfaces.Length);
            
            Assert.That(t1.SomeInterfaces.OfType<SomeInterfaceImplementation2>().Count() == 1);
            Assert.That(t1.SomeInterfaces.OfType<SomeInterfaceImplementation>().Count() == 1);
        }

        [Test]
        public void SimpleModuleTest2()
        {
            var kernel = Kernel.Create();
            kernel.Load(new SimpleModuleTestModule2());

            var t1 = kernel.Resolve<IMultiInterface>();

            Assert.NotNull(t1);
            Assert.NotNull(t1.SomeInterfaces);
            Assert.AreEqual(1, t1.SomeInterfaces.Length);
            Assert.That(t1.SomeInterfaces[0] is SomeInterfaceImplementation);
        }

        [Test]
        public void SimpleModuleTest3()
        {
            var kernel = Kernel.Create();
            kernel.Load(new SimpleModuleTestModule2());

            var t1 = kernel.Resolve<SomeInterfaceImplementation2>();

            Assert.NotNull(t1);
        }

        [Test]
        public void SimpleAssemblyModuleTest()
        {
            var kernel = Kernel.Create();
            kernel.Load(typeof(SimpleModuleTestModule).Assembly);

            var t1 = kernel.Resolve<IMultiInterface>();

            Assert.NotNull(t1);
            Assert.NotNull(t1.SomeInterfaces);
            Assert.AreEqual(3, t1.SomeInterfaces.Length);
            Assert.That(t1.SomeInterfaces[0] is SomeInterfaceImplementation);
            Assert.That(t1.SomeInterfaces[1] is SomeInterfaceImplementation2);
            Assert.That(t1.SomeInterfaces[2] is SomeInterfaceImplementation);
        }

        [Test]
        public void SimpleAssemblyFileModuleTest()
        {
            var kernel = Kernel.Create();
            kernel.Load(typeof(SimpleModuleTestModule).Assembly.Location);

            var t1 = kernel.Resolve<IMultiInterface>();

            Assert.NotNull(t1);
            Assert.NotNull(t1.SomeInterfaces);
            Assert.AreEqual(3, t1.SomeInterfaces.Length);
            Assert.That(t1.SomeInterfaces[0] is SomeInterfaceImplementation);
            Assert.That(t1.SomeInterfaces[1] is SomeInterfaceImplementation2);
            Assert.That(t1.SomeInterfaces[2] is SomeInterfaceImplementation);
        }
    }
}
