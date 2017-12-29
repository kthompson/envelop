namespace Envelop
{
    /// <summary>
    /// The Activation record that keeps track of the object created so that it can later be deactivated
    /// </summary>
    public interface IActivation
    {
        /// <summary>
        /// Gets the object created by the activation.
        /// </summary>
        /// <value>
        /// The object.
        /// </value>
        object Object { get; }

        /// <summary>
        /// Gets the scope that this activation belongs to.
        /// </summary>
        /// <value>The scope.</value>
        IScope Scope { get; }

        /// <summary>
        /// Method to Deactivate the associated Object.
        /// </summary>
        void Deactivate();
    }
}