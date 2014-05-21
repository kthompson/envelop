using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Envelop.Tests.TestDependencies;
using Xunit;

namespace Envelop.Tests
{
    class ExceptionsTests
    {
        [Fact]
        public void IncompleteBindingException()
        {
            var kernel = Kernel.Create();
            kernel.Bind<ISomeInterface>();

            Assert.Throws<IncompleteBindingException>(() => kernel.Resolve<ISomeInterface>());
        }

        [Fact]
        public void BindingNotFoundException()
        {
            var kernel = Kernel.Create();

            Assert.Throws<BindingNotFoundException>(() => kernel.Resolve<ISomeInterface>());
        }

        [Fact]
        public void BindingNotFoundDueToNoValidConstructor()
        {
            var kernel = Kernel.Create();
            //We dont define a binding for ISomeInterface which is required in the ctor of AnotherInterfaceImplementation
            kernel.Bind<IAnotherInterface>().To<AnotherInterfaceImplementation>();

            Assert.Throws<BindingNotFoundException>(() => kernel.Resolve<IAnotherInterface>());
        }
    }
}
