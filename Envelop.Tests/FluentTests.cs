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
    public class FluentTests
    {
        [Test]
        public void BindInterfaceToClass()
        {
            var kernel = Kernel.Create();
            kernel.Bind<ISomeInterface>().To<SomeInterfaceImplementation>();

            var t1 = kernel.Resolve<ISomeInterface>();
            var t2 = kernel.Resolve<ISomeInterface>();
            var t3 = kernel.Resolve<ISomeInterface>();

            Assert.NotNull(t1);
            Assert.NotNull(t2);
            Assert.NotNull(t3);

            Assert.AreNotSame(t1, t2);
            Assert.AreNotSame(t2, t3);
        }

        [Test]
        public void ResolveCanUseType()
        {
            var kernel = Kernel.Create();
            kernel.Bind<ISomeInterface>().To<SomeInterfaceImplementation>();

            var t1 = kernel.Resolve(typeof(ISomeInterface));
            
            Assert.NotNull(t1);
        }

        [Test]
        public void BindInterfaceToBuilder()
        {
            var kernel = Kernel.Create();
            kernel.Bind<ISomeInterface>().To(req => new SomeInterfaceImplementation());

            var t1 = kernel.Resolve<ISomeInterface>();
            var t2 = kernel.Resolve<ISomeInterface>();
            var t3 = kernel.Resolve<ISomeInterface>();

            Assert.NotNull(t1);
            Assert.NotNull(t2);
            Assert.NotNull(t3);

            Assert.AreNotSame(t1, t2);
            Assert.AreNotSame(t2, t3);
        }

        [Test]
        public void BindInterfaceToInstance()
        {
            var kernel = Kernel.Create();
            kernel.Bind<ISomeInterface>().To(new SomeInterfaceImplementation());

            var t1 = kernel.Resolve<ISomeInterface>();
            var t2 = kernel.Resolve<ISomeInterface>();
            var t3 = kernel.Resolve<ISomeInterface>();

            Assert.NotNull(t1);
            Assert.NotNull(t2);
            Assert.NotNull(t3);

            Assert.AreSame(t1, t2);
            Assert.AreSame(t2, t3);
        }

        [Test]
        public void ResolveAllWithGenericTypeParameter()
        {
            var kernel = Kernel.Create();
            kernel.Bind<ISomeInterface>().To<SomeInterfaceImplementation>();
            kernel.Bind<ISomeInterface>().To<SomeInterfaceImplementation2>();

            var enumerable = kernel.ResolveAll<ISomeInterface>();
            Assert.NotNull(enumerable);

            var items = enumerable.ToArray();
            Assert.AreEqual(2, items.Length);

            Assert.That(items[0] is SomeInterfaceImplementation);
            Assert.That(items[1] is SomeInterfaceImplementation2);
        }

        [Test]
        public void ResolveAllWithTypeParameter()
        {
            var kernel = Kernel.Create();
            kernel.Bind<ISomeInterface>().To<SomeInterfaceImplementation>();
            kernel.Bind<ISomeInterface>().To<SomeInterfaceImplementation2>();

            var enumerable = kernel.ResolveAll(typeof(ISomeInterface));
            Assert.NotNull(enumerable);

            var items = enumerable.ToArray();
            Assert.AreEqual(2, items.Length);

            Assert.That(items[0] is SomeInterfaceImplementation);
            Assert.That(items[1] is SomeInterfaceImplementation2);
        }

        [Test]
        public void AutoRegisterBindsServices()
        {
            var kernel = Kernel.Create(true);

            var enumerable = kernel.ResolveAll<ISomeInterface>();
            Assert.NotNull(enumerable);

            var items = enumerable.ToArray();
            Assert.AreEqual(2, items.Length);

            Assert.That(items[0] is SomeInterfaceImplementation);
            Assert.That(items[1] is SomeInterfaceImplementation2);
        }
    }
}
