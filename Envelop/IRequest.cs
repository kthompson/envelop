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
    }

    /// <summary>
    /// An enum to indicate multi-injection if any
    /// </summary>
    public enum InjectionMode
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
        List,
        /// <summary>
        /// Return a factory method
        /// </summary>
        Factory,


        /// <summary>
        /// Return a Lazy&lt;T&gt;
        /// </summary>
        Lazy
    }

    class Request : IRequest
    {
        public IResolver Resolver { get; set; }
        public Type ServiceType { get; set; }
        public Type Target { get; set; }
        public IRequest ParentRequest { get; set; }
        public InjectionMode InjectionMode { get; set; }
        public IScope CurrentScope { get; set; }
    }
}