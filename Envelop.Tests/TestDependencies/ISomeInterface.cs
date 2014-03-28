using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Envelop.Tests.TestDependencies
{
    interface ISomeInterface
    {
        object Value1 { get; set; }
        object Value2 { get; set; }
        object Value3 { get; set; }
        object Value4 { get; set; }
        object Value5 { get; set; }
    }

    class SomeInterfaceImplementation : ISomeInterface
    {
        public object Value1 { get; set; }
        public object Value2 { get; set; }
        public object Value3 { get; set; }
        public object Value4 { get; set; }
        public object Value5 { get; set; }

        public SomeInterfaceImplementation()
        {
        }

        public SomeInterfaceImplementation(int value)
        {
            this.Value1 = value;
        }
    }

    class SomeInterfaceImplementation2 : ISomeInterface
    {
        public object Value1 { get; set; }
        public object Value2 { get; set; }
        public object Value3 { get; set; }
        public object Value4 { get; set; }
        public object Value5 { get; set; }
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
