﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Envelop
{
    /// <summary>
    /// The <c>Kernel</c> is the primary class for type resolution and dependency injection.
    /// </summary>
    /// <example>
    /// TODO: provide some examples here
    /// </example>
    public sealed class Kernel : IKernel
    {
        #region Fields

        private readonly IBindingResolver _bindingResolver;
        private readonly List<IBinding> _bindings;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Kernel"/> class.
        /// </summary>
        private Kernel()
        {
            _bindings = new List<IBinding>();
            _bindingResolver = new DefaultBindingResolver();
        }

        /// <summary>
        /// Creates an instance of the Kernel.
        /// </summary>
        /// <returns></returns>
        public static IKernel Create()
        {
            return new Kernel();
        }

        #endregion

        #region Load

        /// <summary>
        /// Loads the modules at the specified file paths.
        /// </summary>
        /// <param name="filePaths">The file paths.</param>
        public void Load(params string[] filePaths)
        {
            foreach (var filePath in filePaths)
            {
                if (File.Exists(filePath))
                {
                    this.Load(Assembly.LoadFile(filePath));
                }
                else if (Directory.Exists(filePath))
                {
                    foreach (var file in Directory.EnumerateFiles(filePath, "*.dll"))
                    {
                        this.Load(Assembly.LoadFile(file));
                    }
                }
            }
        }

        /// <summary>
        /// Loads modules from the specified assemblies.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        public void Load(params Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                var modules = assembly.GetTypes().Where(t => typeof(IModule).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface);

                //TODO: we should probably try to inject into one of the modules ctors
                foreach (var module in modules.Select(m => (IModule) Activator.CreateInstance(m)))
                    this.Load(module);
            }
        }

        /// <summary>
        /// Loads the specified modules.
        /// </summary>
        /// <param name="modules">The modules.</param>
        public void Load(params IModule[] modules)
        {
            foreach (var module in modules)
                module.OnLoad(this);
        }

        #endregion

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
        public bool CanResolve(IRequest request)
        {
            return ResolveBindings(request).Any();
        }

        #endregion

        #region Binding

        /// <summary>
        /// Binds this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IBindingTo<T> Bind<T>()
        {
            return new BindingTo<T>(CreateBinding<T>());
        }

        /// <summary>
        /// Gets the bindings.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IBinding> GetBindings()
        {
            return _bindings.ToArray();
        }

        /// <summary>
        /// Adds a binding.
        /// </summary>
        /// <param name="binding">The binding.</param>
        public void AddBinding(IBinding binding)
        {
            _bindings.Add(binding);
        }

        #endregion

        #region Helper Methods

        private Binding<T> CreateBinding<T>()
        {
            var binding = new Binding<T>();
            AddBinding(binding);
            return binding;
        }

        private IEnumerable<IBinding> ResolveBindings(IRequest req)
        {
            return _bindingResolver.Resolve(this.GetBindings(), req);
        }

        private Request CreateRequest(Type service)
        {
            return new Request { Resolver = this, ServiceType = service };
        }
        #endregion
    }
}
