using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Envelop.Tests.TestDependencies;
using Xunit;

namespace Envelop.Tests
{
    public class ResolutionTests
    {
        [Fact]
        public void CanResolveOpenGenericType()
        {
            // Given
            var kernel = Kernel.Create();

            // When
            kernel.Bind(typeof(IGenericInterface<>)).To(typeof(GenericInterface<>));
            var instance = kernel.Resolve<IGenericInterface<int>>();

            // Then
            Assert.NotNull(instance);
            Assert.IsType<GenericInterface<int>>(instance);
        }

        [Fact]
        public void CanResolveOpenGenericTypeWithMultipleParameters()
        {
            // Given
            var kernel = Kernel.Create();

            // When
            kernel.Bind(typeof(IGenericInterface<,>)).To(typeof(GenericInterface<,>));
            var instance = kernel.Resolve<IGenericInterface<int, long>>();

            // Then
            Assert.NotNull(instance);
            Assert.IsType<GenericInterface<int, long>>(instance);
        }

        [Fact]
        public void CanResolveOpenGenericTypeWithMultipleParametersWithDifferentGenericTypeOrder()
        {
            // Given
            var kernel = Kernel.Create();

            // When
            kernel.Bind(typeof(IGenericInterface<,>)).To(typeof(GenericInterfaceWacky<,>));
            var instance = kernel.Resolve<IGenericInterface<int, bool>>();

            // Then
            Assert.NotNull(instance);
            Assert.IsType<GenericInterfaceWacky<bool, int>>(instance);
        }


        [Fact]
        public void CanResolveOpenGenericTypeWithDependency()
        {
            // Given
            var kernel = Kernel.Create();

            // When
            kernel.Bind<ISomeInterface>().To<SomeInterfaceImplementation>();
            kernel.Bind(typeof(IGenericInterface2<>)).To(typeof(GenericInterface2<>));
            var instance = kernel.Resolve<IGenericInterface2<int>>();

            // Then
            Assert.NotNull(instance);
            Assert.IsType<GenericInterface2<int>>(instance);

            Assert.NotNull(instance.Dependency);
            Assert.IsType<SomeInterfaceImplementation>(instance.Dependency);
        }
    }
}
