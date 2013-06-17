using System;

namespace Envelop
{
    /// <summary>
    /// <c>IBindingContraints&lt;T&gt;</c> is the interface used to specify any constraints based on the <c>IRequest</c>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBindingContraints<T>
    {
        /// <summary>
        /// Limit this binding to the specified <paramref name="predicate"/>.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        IBindingContraints<T> When(Predicate<IRequest> predicate);
        /// <summary>
        /// Convert this binding to a singleton.
        /// </summary>
        /// <returns></returns>
        IBindingContraints<T> AsSingleton();
    }
}