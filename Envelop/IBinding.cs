using System;
using System.Collections.Generic;

namespace Envelop
{
    /// <summary>
    /// A generic of a binding.
    /// </summary>
    public interface IBinding
    {
        /// <summary>
        /// Gets the type of the service.
        /// </summary>
        /// <value>
        /// The type of the service.
        /// </value>
        Type ServiceType { get; }

        /// <summary>
        /// Gets the constraints.
        /// </summary>
        /// <value>
        /// The constraints.
        /// </value>
        List<IBindingConstraint> Constraints { get; }

        /// <summary>
        /// Builds this instance.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        object Activate(IRequest request);

        /// <summary>
        /// Gets or sets the builder.
        /// </summary>
        /// <value>
        /// The builder.
        /// </value>
        Func<IRequest, object> Activator { get; set; }
    }

    /// <summary>
    /// A type specific version of IBinding <c ref=""></c>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBinding<T> : IBinding
    {
    }
}