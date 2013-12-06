using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envelop.Tests.TestDependencies
{
    interface ISomeInterface
    {
    }

    class SomeInterfaceImplementation : ISomeInterface
    {

    }

    class SomeInterfaceImplementation2 : ISomeInterface
    {

    }

    interface IAnotherInterface
    {
        ISomeInterface SomeInterface { get; }
    }

    class AnotherInterfaceImplementation : IAnotherInterface
    {
        public ISomeInterface SomeInterface { get; private set; }

        public AnotherInterfaceImplementation(ISomeInterface someInterface)
        {
            this.SomeInterface = someInterface;
        }
    }
    
    interface IMultiInterface
    {
        ISomeInterface[] SomeInterfaces { get; }
    }

    class MultiInterfaceImplementation : IMultiInterface
    {
        public ISomeInterface[] SomeInterfaces { get; private set; }

        public MultiInterfaceImplementation(ISomeInterface[] someInterfaces)
        {
            this.SomeInterfaces = someInterfaces;
        }
    }

    class MultiInterfaceImplementation2 : IMultiInterface
    {
        public ISomeInterface[] SomeInterfaces { get; private set; }

        public MultiInterfaceImplementation2(IEnumerable<ISomeInterface> someInterfaces)
        {
            this.SomeInterfaces = someInterfaces.ToArray();
        }
    }

    class MultiInterfaceImplementation3 : IMultiInterface
    {
        public ISomeInterface[] SomeInterfaces { get; private set; }

        public MultiInterfaceImplementation3(List<ISomeInterface> someInterfaces)
        {
            this.SomeInterfaces = someInterfaces.ToArray();
        }
    }

    interface IIsDisposable : IDisposable 
    {
        bool IsDisposed { get; }
    }

    class IsDisposable : IIsDisposable 
    {
        #region IDisposable implementation
        public void Dispose ()
        {
            this.IsDisposed = true;
        }
        #endregion

        #region IIsDisposable implementation
        public bool IsDisposed {
            get ;
            private set;
        }
        #endregion
    }
}
