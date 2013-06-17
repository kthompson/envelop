using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Envelop
{
    /// <summary>
    /// Base class for creating bindings and performing type resolution
    /// </summary>
    public class Builder : IBuilder
    {
        #region Fields

        private readonly List<IBinding> _bindings;
        private readonly IBindingResolver _bindingResolver;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Builder"/> class.
        /// </summary>
        public Builder()
        {
            _bindings = new List<IBinding>();
            _bindingResolver = new DefaultBindingResolver();
        }

        #endregion

        #region Bind

        /// <summary>
        /// Binds this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IBindingTo<T> Bind<T>()
        {
            return new BindingTo<T>(this, CreateBinding<T>());
        }

        #endregion

        #region Resolve

        /// <summary>
        /// Resolves a given type based on the generic type parameter.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public T Resolve<T>()
        {
            IRequest req = CreateRequest(typeof(T));
            return (T)Resolve(req);
        }

        /// <summary>
        /// Resolves a given type based on service
        /// </summary>
        /// <param name="service">The service.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public object Resolve(Type service)
        {
            IRequest req = CreateRequest(service);
            return Resolve(req);
        }

        /// <summary>
        /// Resolves the specified req.
        /// </summary>
        /// <param name="req">The req.</param>
        /// <returns></returns>
        /// <exception cref="BindingNotFoundException"></exception>
        public object Resolve(IRequest req)
        {
            var binding = ResolveBindings(req).FirstOrDefault();
            if(binding == null)
                throw new BindingNotFoundException();

            return binding.Activate(req);
        }

        #endregion

        #region ResolveAll

        /// <summary>
        /// Resolves a given type based on the generic type parameter.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IEnumerable<T> ResolveAll<T>()
        {
            IRequest req = CreateRequest(typeof(T));
            return ResolveAll(req).Cast<T>();
        }

        /// <summary>
        /// Resolves all bindings based on the service.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <returns></returns>
        public IEnumerable<object> ResolveAll(Type service)
        {
            IRequest req = CreateRequest(service);
            return ResolveAll(req);
        }

        /// <summary>
        /// Resolves all.
        /// </summary>
        /// <param name="req">The req.</param>
        /// <returns></returns>
        public IEnumerable<object> ResolveAll(IRequest req)
        {
            return ResolveBindings(req).Select(b => b.Activate(req));
        }

        #endregion

        #region CanResolve

        /// <summary>
        /// Determines whether this instance can resolve the specified service.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <returns>
        ///   <c>true</c> if this instance can resolve the specified service; otherwise, <c>false</c>.
        /// </returns>
        public bool CanResolve(Type service)
        {
            return CanResolve(CreateRequest(service));
        }

        /// <summary>
        /// Determines whether this instance can resolve the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>true</c> if this instance can resolve the specified request; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool CanResolve(IRequest request)
        {
            return ResolveBindings(request).Any();
        }

        #endregion

        #region Helper Methods

        private IEnumerable<IBinding> ResolveBindings(IRequest req)
        {
            return _bindingResolver.Resolve(_bindings, req);
        }

        private Request CreateRequest(Type service)
        {
            return new Request { Resolver = this, ServiceType = service };
        }

        private Binding<T> CreateBinding<T>()
        {
            var binding = new Binding<T>();
            _bindings.Add(binding);
            return binding;
        }

        #endregion
    }
}