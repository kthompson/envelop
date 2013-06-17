using System;
using System.Collections.Generic;

namespace Envelop
{
    /// <summary>
    /// <c>IBuilder</c> interface for exposing basic binding and type resolution functions
    /// </summary>
    public interface IBuilder : IResolver
    {
        /// <summary>
        /// Fluent interface for defining bindings.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <returns></returns>
        /// <example>
        /// TODO: insert some basic examples
        /// </example>
        IBindingTo<TInterface> Bind<TInterface>();
    }

    /// <summary>
    /// Generic type resolving interface
    /// </summary>
    public interface IResolver
    {
        /// <summary>
        /// Resolves a given type based on the generic type parameter.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Resolve<T>();

        /// <summary>
        /// Resolves a given type based on type
        /// </summary>
        /// <param name="service">The service.</param>
        /// <returns></returns>
        object Resolve(Type service);

        /// <summary>
        /// Resolves the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        object Resolve(IRequest request);

        /// <summary>
        /// Resolves all bindings based on the generic type parameter.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<T> ResolveAll<T>();

        /// <summary>
        /// Resolves all bindings based on the service.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <returns></returns>
        IEnumerable<object> ResolveAll(Type service);

        /// <summary>
        /// Resolves all bindings that meet the request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        IEnumerable<object> ResolveAll(IRequest request);

        /// <summary>
        /// Determines whether this instance can resolve the specified service.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <returns>
        ///   <c>true</c> if this instance can resolve the specified service; otherwise, <c>false</c>.
        /// </returns>
        bool CanResolve(Type service);

        /// <summary>
        /// Determines whether this instance can resolve the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>true</c> if this instance can resolve the specified request; otherwise, <c>false</c>.
        /// </returns>
        bool CanResolve(IRequest request);
    }
}