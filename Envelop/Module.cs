using System;
using System.Collections.Generic;

namespace Envelop
{
    /// <summary>
    /// <c>Module</c> is a container for a set of bindings to be applied to the <c>Kernel</c>.
    /// </summary>
    public abstract class Module : IModule
    {
        /// <summary>
        /// Gets the attached kernel.
        /// </summary>
        /// <value>
        /// The kernel.
        /// </value>
        protected IKernel Kernel { get; private set; }

        /// <summary>
        /// The name of the module
        /// </summary>
        public virtual string Name
        {
            get { return GetType().FullName; }
        }

        /// <summary>
        /// Called to load the module bindings into the Kernel
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        public void OnLoad(IKernel kernel)
        {
            if(kernel == null)
                throw new ArgumentNullException("kernel");

            this.Kernel = kernel;
            this.Load();
        }

        /// <summary>
        /// Loads the bindings to the attached Kernel.
        /// </summary>
        protected abstract void Load();

        /// <summary>
        /// Fluent interface for defining bindings.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <returns></returns>
        /// <example>
        /// TODO: insert some basic examples
        ///   </example>
        public IBindingTo<TInterface> Bind<TInterface>()
        {
            return this.Kernel.Bind<TInterface>();
        }

        /// <summary>
        /// Interface for defining bindings.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        /// <example>
        /// TODO: insert some basic examples
        ///   </example>
        public IBindingTo Bind(Type type)
        {
            return this.Kernel.Bind(type);
        }

        /// <summary>
        /// Registers the specified service type.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="instanceType">Type of the instance.</param>
        /// <returns></returns>
        public IBindingContraints Register(Type serviceType, Type instanceType)
        {
            return this.Kernel.Register(serviceType, instanceType);
        }

        /// <summary>
        /// Registers the specified instance type.
        /// </summary>
        /// <param name="instanceType">Type of the instance.</param>
        /// <returns></returns>
        public IBindingContraints Register(Type instanceType)
        {
            return this.Kernel.Register(instanceType);
        }

        /// <summary>
        /// Gets the bindings.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IBinding> GetBindings()
        {
            return this.Kernel.GetBindings();
        }

        /// <summary>
        /// Adds a binding.
        /// </summary>
        /// <param name="binding">The binding.</param>
        public void AddBinding(IBinding binding)
        {
            this.Kernel.AddBinding(binding);
        }
    }
}