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
    public class SpeedTests
    {
        [Test]
        public void InjectionSpeed()
        {
            var kernel = new Kernel();
            kernel.Bind<ISomeInterface>().To<SomeInterfaceImplementation>();
            kernel.Bind<IAnotherInterface>().To<AnotherInterfaceImplementation>();

            for (int i = 0; i < 1000000; i++)
            {
                var t1 = kernel.Resolve<IAnotherInterface>();
                Assert.NotNull(t1);
                Assert.NotNull(t1.SomeInterface);
            }
        }
    }
}
