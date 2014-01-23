using System;
using System.Collections.Generic;

namespace Envelop
{
    /// <summary>
    /// <c>IBuilder</c> interface for exposing basic binding and type resolution functions
    /// </summary>
    public interface IBuilder
    {
        /// <summary>
        /// Fluent interface for defining bindings.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <returns></returns>
        /// <example>
        /// TODO: insert some basic examples
        /// </example>
        IBindingTo<TInterface> Bind<TInterface>();

        /// <summary>
        /// Interface for defining bindings.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        /// <example>
        /// TODO: insert some basic examples
        /// </example>
        IBindingTo Bind(Type type);

        /// <summary>
        /// Gets the registered bindings.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IBinding> GetBindings();

        /// <summary>
        /// Adds the specified binding.
        /// </summary>
        /// <param name="binding">The binding.</param>
        void AddBinding(IBinding binding);
    }
}