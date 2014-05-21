using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Envelop.Tests.TestDependencies;
using Xunit;

namespace Envelop.Tests
{
    public class FluentTests
    {
        [Fact]
        public void NewBindShouldOverrideExistingBind()
        {
            var kernel = Kernel.Create();
            kernel.Bind<ISomeInterface>().To<SomeInterfaceImplementation>();

            var t1 = kernel.Resolve<ISomeInterface>();
            var t2 = kernel.Resolve<ISomeInterface>();

            Assert.NotNull(t1);
            Assert.NotNull(t2);

            Assert.NotSame(t1, t2);

            Assert.True(t1 is SomeInterfaceImplementation);
            Assert.True(t2 is SomeInterfaceImplementation);

            //override existing bind
            kernel.Bind<ISomeInterface>().To<SomeInterfaceImplementation>().AsSingleton();

            var t3 = kernel.Resolve<ISomeInterface>(); 
            var t4 = kernel.Resolve<ISomeInterface>();
            
            Assert.NotNull(t3); 
            Assert.NotNull(t4);

            Assert.NotSame(t1, t3);
            Assert.NotSame(t1, t4);
            Assert.NotSame(t2, t3);
            Assert.NotSame(t2, t4);

            Assert.True(t3 is SomeInterfaceImplementation);
            Assert.True(t4 is SomeInterfaceImplementation);

            Assert.Same(t3, t4);
        }

        [Fact]
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

            Assert.NotSame(t1, t2);
            Assert.NotSame(t2, t3);
            Assert.NotSame(t1, t3);

            Assert.True(t1 is SomeInterfaceImplementation);
            Assert.True(t2 is SomeInterfaceImplementation);
            Assert.True(t3 is SomeInterfaceImplementation);
        }

        [Fact]
        public void ResolveCanUseTypeParameter()
        {
            var kernel = Kernel.Create();
            kernel.Bind<ISomeInterface>().To(typeof(SomeInterfaceImplementation));

            var t1 = kernel.Resolve(typeof(ISomeInterface));

            Assert.NotNull(t1);
            Assert.True(t1 is SomeInterfaceImplementation);
        }

        [Fact]
        public void ResolveCanUseType()
        {
            var kernel = Kernel.Create();
            kernel.Bind<ISomeInterface>().To<SomeInterfaceImplementation>();

            var t1 = kernel.Resolve(typeof(ISomeInterface));

            Assert.NotNull(t1);
            Assert.True(t1 is SomeInterfaceImplementation);
        }

        [Fact]
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

            Assert.NotSame(t1, t2);
            Assert.NotSame(t2, t3);

            Assert.True(t1 is SomeInterfaceImplementation);
            Assert.True(t2 is SomeInterfaceImplementation);
            Assert.True(t3 is SomeInterfaceImplementation);
        }

        [Fact]
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

            Assert.Same(t1, t2);
            Assert.Same(t2, t3);

            Assert.True(t1 is SomeInterfaceImplementation);
            Assert.True(t2 is SomeInterfaceImplementation);
            Assert.True(t3 is SomeInterfaceImplementation);
        }

        [Fact]
        public void ResolveAllWithGenericTypeParameter()
        {
            var kernel = Kernel.Create();
            kernel.Bind<ISomeInterface>().To<SomeInterfaceImplementation>();
            kernel.Bind<ISomeInterface>().To<SomeInterfaceImplementation2>();

            var enumerable = kernel.ResolveAll<ISomeInterface>();
            Assert.NotNull(enumerable);

            var items = enumerable.ToArray();
            Assert.Equal(2, items.Length);

            Assert.True(items.OfType<SomeInterfaceImplementation2>().Count() == 1);
            Assert.True(items.OfType<SomeInterfaceImplementation>().Count() == 1);
        }

        [Fact]
        public void ResolveAllWithTypeParameter()
        {
            var kernel = Kernel.Create();
            kernel.Bind<ISomeInterface>().To<SomeInterfaceImplementation>();
            kernel.Bind<ISomeInterface>().To<SomeInterfaceImplementation2>();

            var enumerable = kernel.ResolveAll(typeof(ISomeInterface));
            Assert.NotNull(enumerable);

            var items = enumerable.ToArray();
            Assert.Equal(2, items.Length);

            Assert.True(items.OfType<SomeInterfaceImplementation2>().Count() == 1);
            Assert.True(items.OfType<SomeInterfaceImplementation>().Count() == 1);
        }

        [Fact]
        public void AutoRegisterBindsServices()
        {
            var kernel = Kernel.Create(true);

            var enumerable = kernel.ResolveAll<ISomeInterface>();
            Assert.NotNull(enumerable);

            var items = enumerable.ToArray();
            Assert.Equal(2, items.Length);

            Assert.True(items.OfType<SomeInterfaceImplementation2>().Count() == 1);
            Assert.True(items.OfType<SomeInterfaceImplementation>().Count() == 1);
        }
    }
}
