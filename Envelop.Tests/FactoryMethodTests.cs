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
    public class FactoryMethodTests
    {
        [Test]
        public void ResolveUsingAParameterizedFactoryMethodWithOneParam()
        {
            var kernel = Kernel.Create();
            kernel.Bind<int>().To(25);
            kernel.Bind<ISomeInterface>().To((int value) => new SomeInterfaceImplementation(value));


            var t1 = kernel.Resolve<ISomeInterface>();

            Assert.NotNull(t1);

            Assert.AreEqual(t1.Value1, 25);
        }

        [Test]
        public void ResolveUsingAParameterizedFactoryMethodWithTwoParams()
        {
            var kernel = Kernel.Create();
            kernel.Bind<int>().To(25);
            kernel.Bind<object>().To(21);
            kernel.Bind<ISomeInterface>().To((int value, object v2) => new SomeInterfaceImplementation
            {
                Value1 = value,
                Value2 = v2,
            });


            var t1 = kernel.Resolve<ISomeInterface>();

            Assert.NotNull(t1);

            Assert.AreEqual(t1.Value1, 25);
            Assert.AreEqual(t1.Value2, 21);
        }

        [Test]
        public void ResolveUsingAParameterizedFactoryMethodWith3Params()
        {
            var kernel = Kernel.Create();
            kernel.Bind<int>().To(25);
            kernel.Bind<object>().To(21);
            kernel.Bind<float>().To(21.5f);
            kernel.Bind<ISomeInterface>().To((int value, object v2, float v3) => new SomeInterfaceImplementation
            {
                Value1 = value,
                Value2 = v2,
                Value3 = v3
            });


            var t1 = kernel.Resolve<ISomeInterface>();

            Assert.NotNull(t1);

            Assert.AreEqual(t1.Value1, 25);
            Assert.AreEqual(t1.Value2, 21);
            Assert.AreEqual(t1.Value3, 21.5f);
        }
    }
}
