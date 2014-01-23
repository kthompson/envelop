using System;

namespace Envelop
{
    /// <summary>
    /// <c>IBindingTo&lt;T&gt;</c> is the interface used to attach an implementation type to a binding.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBindingTo<T> : IBindingTo
    {
        /// <summary>
        /// Maps a binding to the generic type <c>TImplementation</c>.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the class.</typeparam>
        /// <returns></returns>
        IBindingContraints To<TImplementation>() where TImplementation : T;
        
        /// <summary>
        /// Maps a binding to the generic type <c>TImplementation</c> via a factory function.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="func">The func.</param>
        /// <returns></returns>
        IBindingContraints To<TImplementation>(Func<IResolver, TImplementation> func) where TImplementation : T;

        /// <summary>
        /// Maps a binding to the singleton.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        IBindingContraints To<TImplementation>(TImplementation instance) where TImplementation : T;
    }

    /// <summary>
    /// <c>IBindingTo</c> is the interface used to attach an implementation type to a binding.
    /// </summary>
    public interface IBindingTo
    {
        /// <summary>
        /// To the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        IBindingContraints To(object instance);

        /// <summary>
        /// Maps a binding to the result of a factory function.
        /// </summary>
        /// <param name="func">The func.</param>
        /// <returns></returns>
        IBindingContraints To(Func<IResolver, object> func);

        /// <summary>
        /// Maps a binding to the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        IBindingContraints To(Type type);
        
    }
}