using System;
using System.Collections.Generic;
using System.Linq;

namespace Envelop
{

	class Scope : IScope
	{
		#region Fields

		private readonly IBindingResolver _bindingResolver;
		private readonly List<IActivation> _activations;
		private readonly List<IScope> _scopes;

		#endregion

		#region Initialization

        public Scope(IBindingResolver resolver)
		{
            _bindingResolver = resolver;
			_activations = new List<IActivation> ();
			_scopes = new List<IScope> ();
		}

		#endregion
        
		public void AddActivation (IActivation activation)
		{
			_activations.Add (activation);
		}

		public IScope CreateScope ()
		{
			var scope = new Scope (this._bindingResolver) { Parent = this };
			this._scopes.Add (scope);
			return scope;
		}

		public IScope Parent { get; set; }

		public void Dispose ()
		{
			foreach (var scope in this._scopes)
				scope.Dispose ();

			this._scopes.Clear ();

			foreach (var activation in this._activations)
				activation.Deactivate ();

			this._activations.Clear ();
		}

		#region Resolve

		/// <summary>
		/// Resolves a given type based on the generic type parameter.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
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
			if (binding == null)
                throw new BindingNotFoundException(req);

			return binding.Activate(req).Object;
		}

		#endregion

		#region ResolveAll

		/// <summary>
		/// Resolves a given type based on the generic type parameter.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
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
			return ResolveBindings(req).Select(b => b.Activate(req).Object);
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
		public bool CanResolve(IRequest request)
		{
			return ResolveBindings(request).Any();
		}

		#endregion


		#region Helper Methods

		private IEnumerable<IBinding> ResolveBindings(IRequest req)
		{
			return _bindingResolver.Resolve(req);
		}

		private Request CreateRequest(Type service)
		{
			return new Request 
			{
				Resolver = this, 
				ServiceType = service,
				CurrentScope = this,
			};
		}
		#endregion

	}
}