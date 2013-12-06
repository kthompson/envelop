using System;

namespace Envelop
{
    /// <summary>
    /// <c>IBindingContraints</c> is the interface used to specify any constraints based on the <c>IRequest</c>
    /// </summary>
    public interface IBindingContraints
    {
        /// <summary>
        /// Limit this binding to the specified <paramref name="predicate"/>.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        IBindingContraints When(Predicate<IRequest> predicate);
        /// <summary>
        /// Convert this binding to a singleton.
        /// </summary>
        /// <returns></returns>
        IBindingContraints AsSingleton();

        /// <summary>
        /// Performs the specified action after deactivation.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        IBindingContraints AfterDeactivation(Action<object> action);
    }
}