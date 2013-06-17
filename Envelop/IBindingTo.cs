using System;

namespace Envelop
{
    /// <summary>
    /// <c>IBindingTo&lt;T&gt;</c> is the interface used to attach an implementation type to a binding.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBindingTo<T>
    {
        /// <summary>
        /// Maps a binding to the generic type <c>TImplementation</c>.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the class.</typeparam>
        /// <returns></returns>
        IBindingContraints<T> To<TImplementation>() where TImplementation : T;
        /// <summary>
        /// Maps a binding to the generic type <c>TImplementation</c> via a factory function.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="func">The func.</param>
        /// <returns></returns>
        IBindingContraints<T> To<TImplementation>(Func<IBuilder, TImplementation> func) where TImplementation : T;

        /// <summary>
        /// Maps a binding to the singleton.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        IBindingContraints<T> To<TImplementation>(TImplementation instance) where TImplementation : T;
    }
}