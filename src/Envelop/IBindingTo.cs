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
        IBindingContraints To<TImplementation>() 
            where TImplementation : T;

        /// <summary>
        /// Maps a binding to the generic type <c>TImplementation</c> via a factory function.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="func">The func.</param>
        /// <returns></returns>
        IBindingContraints To<TImplementation>(Func<IResolver, TImplementation> func) 
            where TImplementation : T;

        /// <summary>
        /// Maps a binding to the generic type <c>TImplementation</c> via a parameterized factory function.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <param name="func">The func.</param>
        /// <returns></returns>
        IBindingContraints To<TImplementation, T1>(Func<T1, TImplementation> func)
            where TImplementation : T;

        /// <summary>
        /// Maps a binding to the generic type <c>TImplementation</c> via a parameterized factory function.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <param name="func">The func.</param>
        /// <returns></returns>
        IBindingContraints To<TImplementation, T1, T2>(Func<T1, T2, TImplementation> func)
            where TImplementation : T;

        /// <summary>
        /// Maps a binding to the generic type <c>TImplementation</c> via a parameterized factory function.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <typeparam name="T3">The type of the third argument.</typeparam>
        /// <param name="func">The func.</param>
        /// <returns></returns>
        IBindingContraints To<TImplementation, T1, T2, T3>(Func<T1, T2, T3, TImplementation> func)
            where TImplementation : T;

        /// <summary>
        /// Maps a binding to the generic type <c>TImplementation</c> via a parameterized factory function.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <typeparam name="T3">The type of the third argument.</typeparam>
        /// <typeparam name="T4">The type of the fourth argument.</typeparam>
        /// <typeparam name="T5">The type of the fifth argument.</typeparam>
        /// <param name="func">The func.</param>
        /// <returns></returns>
        IBindingContraints To<TImplementation, T1, T2, T3, T4, T5>(Func<T1, T2, T3, T4, T5, TImplementation> func)
            where TImplementation : T;

        /// <summary>
        /// Maps a binding to the generic type <c>TImplementation</c> via a parameterized factory function.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <typeparam name="T3">The type of the third argument.</typeparam>
        /// <typeparam name="T4">The type of the fourth argument.</typeparam>
        /// <typeparam name="T5">The type of the fifth argument.</typeparam>
        /// <typeparam name="T6">The type of the sixth argument.</typeparam>
        /// <param name="func">The func.</param>
        /// <returns></returns>
        IBindingContraints To<TImplementation, T1, T2, T3, T4, T5, T6>(Func<T1, T2, T3, T4, T5, T6, TImplementation> func)
            where TImplementation : T;


        /// <summary>
        /// Maps a binding to the generic type <c>TImplementation</c> via a parameterized factory function.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <typeparam name="T3">The type of the third argument.</typeparam>
        /// <typeparam name="T4">The type of the fourth argument.</typeparam>
        /// <typeparam name="T5">The type of the fifth argument.</typeparam>
        /// <typeparam name="T6">The type of the sixth argument.</typeparam>
        /// <typeparam name="T7">The type of the sixth argument.</typeparam>
        /// <param name="func">The func.</param>
        /// <returns></returns>
        IBindingContraints To<TImplementation, T1, T2, T3, T4, T5, T6, T7>(Func<T1, T2, T3, T4, T5, T6, T7, TImplementation> func)
            where TImplementation : T;



        /// <summary>
        /// Maps a binding to the generic type <c>TImplementation</c> via a parameterized factory function.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <typeparam name="T3">The type of the third argument.</typeparam>
        /// <typeparam name="T4">The type of the fourth argument.</typeparam>
        /// <param name="func">The func.</param>
        /// <returns></returns>
        IBindingContraints To<TImplementation, T1, T2, T3, T4>(Func<T1, T2, T3, T4, TImplementation> func)
            where TImplementation : T;

        /// <summary>
        /// Maps a binding to the singleton.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        IBindingContraints To<TImplementation>(TImplementation instance) 
            where TImplementation : T;

        /// <summary>
        /// Maps a binding to the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        IBindingContraints To(Type type);
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
        IBindingContraints To<TImplementation>(Func<IResolver, TImplementation> func);

        /// <summary>
        /// Maps a binding to the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        IBindingContraints To(Type type);
        
    }
}