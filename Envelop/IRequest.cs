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
        MultiInjection MultiInjection { get; }
    }

    /// <summary>
    /// An enum to indicate multi-injection if any
    /// </summary>
    public enum MultiInjection
    {
        /// <summary>
        /// No multi-injection
        /// </summary>
        None,
        /// <summary>
        /// Array based multi-injection
        /// </summary>
        Array,
        /// <summary>
        /// Enumerable based multi-injection
        /// </summary>
        Enumerable,
        /// <summary>
        /// List based multi-injection
        /// </summary>
        List
    }

    class Request : IRequest
    {
        public IResolver Resolver { get; set; }
        public Type ServiceType { get; set; }
        public Type Target { get; set; }
        public IRequest ParentRequest { get; set; }
        public MultiInjection MultiInjection { get; set; }
    }
}