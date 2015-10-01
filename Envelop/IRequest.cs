using System;

namespace Envelop
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRequest
    {
        /// <summary>
        /// Gets the builder.
        /// </summary>
        /// <value>
        /// The builder.
        /// </value>
        IResolver Resolver { get; }

        /// <summary>
        /// Gets the type of the service.
        /// </summary>
        /// <value>
        /// The type of the service.
        /// </value>
        Type ServiceType { get; }

        /// <summary>
        /// Gets the target type to be activated.
        /// </summary>
        /// <value>
        /// The target.
        /// </value>
        Type Target { get; }

        /// <summary>
        /// Gets the parent request.
        /// </summary>
        /// <value>
        /// The parent request.
        /// </value>
        IRequest ParentRequest { get; }

        /// <summary>
        /// Gets the multi injection setting.
        /// </summary>
        /// <value>
        /// The multi injection.
        /// </value>
        InjectionMode InjectionMode { get; }

        /// <summary>
        /// Gets the current scope of the request.
        /// </summary>
        /// <value>The current scope.</value>
        IScope CurrentScope { get; }

        /// <summary>
        /// Gets or sets the generic type arguments.
        /// </summary>
        /// <value>
        /// The generic type arguments.
        /// </value>
        Type[] GenericTypeArguments { get; set; }
    }
}