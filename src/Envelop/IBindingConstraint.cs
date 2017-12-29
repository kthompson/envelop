namespace Envelop
{
    /// <summary>
    /// IConstraint is an interface that allows for matching against a request. If a constraint is not matched then the binding wont be used
    /// </summary>
    public interface IBindingConstraint
    {
        /// <summary>
        /// Determines whether the specified request is a match.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>true</c> if the specified request is match; otherwise, <c>false</c>.
        /// </returns>
        bool IsMatch(IRequest request);
    }
}