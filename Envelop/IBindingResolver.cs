using System.Collections.Generic;

namespace Envelop
{
    /// <summary>
    /// interface for selecting a binding given a a set of bindings and an <c>IRequest</c> object
    /// </summary>
    public interface IBindingResolver
    {
        /// <summary>
        /// Resolves a binding based on the specified bindings and the request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        IEnumerable<IBinding> Resolve(IRequest request);
    }
}