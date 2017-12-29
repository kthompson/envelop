using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Envelop.Tests.TestDependencies;
using Xunit;

namespace Envelop.Tests
{
    public class SpeedTests
    {
        [Fact]
        public void InjectionSpeed()
        {
            var kernel = Kernel.Create();
            kernel.Bind<ISomeInterface>().To<SomeInterfaceImplementation>();
            kernel.Bind<IAnotherInterface>().To<AnotherInterfaceImplementation>();

            for (int i = 0; i < 100000; i++)
            {
                var t1 = kernel.Resolve<IAnotherInterface>();
                Assert.NotNull(t1);
                Assert.NotNull(t1.SomeInterface);
            }
        }
    }
}
