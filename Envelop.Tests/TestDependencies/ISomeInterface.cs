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

    class SomeInterfaceFactory
    {
        private readonly Func<ISomeInterface> _factory;

        public ISomeInterface GetSomeInterface()
        {
            return _factory();
        }

        public SomeInterfaceFactory(Func<ISomeInterface> factory)
        {
            _factory = factory;
        }
    }

    class SomeInterfaceLazy
    {
        public Lazy<ISomeInterface> Lazy { get; private set; }

        public ISomeInterface GetSomeInterface()
        {
            return this.Lazy.Value;
        }

        public SomeInterfaceLazy(Lazy<ISomeInterface> factory)
        {
            this.Lazy = factory;
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

    interface IGenericInterface<T>
    {
    }

    class GenericInterface<T> : IGenericInterface<T>
    {
    }

    interface IGenericInterface<T1,T2>
    {
    }

    class GenericInterface<T1,T2> : IGenericInterface<T1,T2>
    {
    }


    interface IGenericInterface2<T>
    {
        ISomeInterface Dependency { get; }
    }

    class GenericInterface2<T> : IGenericInterface2<T>
    {
        public ISomeInterface Dependency { get; }

        public GenericInterface2(ISomeInterface dependency)
        {
            Dependency = dependency;
        }

    }

    class GenericInterfaceWacky<T2, T1> : IGenericInterface<T1, T2>
    {
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
