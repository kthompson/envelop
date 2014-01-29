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
    class ExceptionsTests
    {
        [Test, ExpectedException(typeof(IncompleteBindingException))]
        public void IncompleteBindingException()
        {
            var kernel = Kernel.Create();
            kernel.Bind<ISomeInterface>();

            kernel.Resolve<ISomeInterface>();
        }

        [Test, ExpectedException(typeof(BindingNotFoundException))]
        public void BindingNotFoundException()
        {
            var kernel = Kernel.Create();
            kernel.Resolve<ISomeInterface>();
        }

        [Test, ExpectedException(typeof(BindingNotFoundException))]
        public void BindingNotFoundDueToNoValidConstructor()
        {
            var kernel = Kernel.Create();
            //We dont define a binding for ISomeInterface which is required in the ctor of AnotherInterfaceImplementation
            kernel.Bind<IAnotherInterface>().To<AnotherInterfaceImplementation>();
            kernel.Resolve<IAnotherInterface>();
        }
    }
}
