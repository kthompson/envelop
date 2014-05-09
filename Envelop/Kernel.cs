using System;
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
        private readonly DefaultBindingResolver _bindingResolver;
        private readonly IScope _scope;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Kernel"/> class.
        /// </summary>
        private Kernel()
        {
            _bindingResolver = new DefaultBindingResolver();
            _scope = new Scope(_bindingResolver);
        }

        /// <summary>
        /// Creates an instance of the Kernel.
        /// </summary>
        /// <returns></returns>
        public static IKernel Create(bool autoRegister = false)
        {
            var kernel = new Kernel();
            //TODO: PCL
            // if(autoRegister)
            //    kernel.AutoRegister();

            return kernel;
        }

        #endregion

        #region Automatic Registration

        private readonly object _registrationLock = new object();
        ///// <summary>
        ///// Automatically registers all interfaces and abstract classes.
        ///// </summary>
        //public void AutoRegister()
        //{
        //    var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => !IsIgnoredAssembly(a));
        //    AutoRegister(assemblies);
        //}

        /// <summary>
        /// Automatically registers all interfaces and abstract classes.
        /// </summary>
        public void AutoRegister(IEnumerable<Assembly> assemblies)
        {
            var asms = assemblies.ToList();

            lock (_registrationLock)
            {
                var types = asms.SelectMany(GetTypes).Where(type => !IsIgnoredType(type)).ToList();
                var concreteTypes = GetConcreteTypes(types).ToList();
                var abstractTypes = GetAbstractTypes(types);

                foreach (var type in concreteTypes)
                {
                    this.Bind(type).To(type);
                }

                foreach (var abstractType in abstractTypes)
                {
                    var type = abstractType;
                    var implementations = from implementation in concreteTypes
                        where type.IsAssignableFrom(implementation)
                        select implementation;

                    foreach (var implementation in implementations)
                        this.Register(type, implementation);
                }
            }
        }

        private IEnumerable<Type> GetConcreteTypes(IEnumerable<Type> types)
        {
            return types.Where(t => t.IsClass && !t.IsAbstract && !t.IsGenericTypeDefinition && t != this.GetType());
        }

        private IEnumerable<Type> GetAbstractTypes(IEnumerable<Type> types)
        {
            return types.Where(t => (t.IsAbstract || t.IsInterface) && !t.IsGenericTypeDefinition && t != this.GetType());
        }

        static Type[] GetTypes(Assembly assembly)
        {
            Type[] assemblies;

            try
            {
                assemblies = assembly.GetTypes();
            }
            catch (System.IO.FileNotFoundException)
            {
                assemblies = new Type[] { };
            }
            catch (NotSupportedException)
            {
                assemblies = new Type[] { };
            }
            catch (ReflectionTypeLoadException e)
        {
                assemblies = e.Types.Where(t => t != null).ToArray();
            }

            return assemblies;
        }

        #endregion

        #region Load

        ///// <summary>
        ///// Loads the modules at the specified file paths.
        ///// </summary>
        ///// <param name="filePaths">The file paths.</param>
        //public void Load(params string[] filePaths)
        //{
        //    foreach (var filePath in filePaths)
        //    {
        //        if (File.Exists(filePath))
        //        {
        //            this.Load(Assembly.LoadFile(filePath));
        //        }
        //        else if (Directory.Exists(filePath))
        //        {
        //            foreach (var file in Directory.EnumerateFiles(filePath, "*.dll"))
        //            {
        //                this.Load(Assembly.LoadFile(file));
        //            }
        //        }
        //    }
        //}

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
            return _scope.Resolve<T> ();
        }

        /// <summary>
        /// Resolves a given type based on service
        /// </summary>
        /// <param name="service">The service.</param>
        /// <returns></returns>
        public object Resolve(Type service)
        {
            return _scope.Resolve (service);
        }

        /// <summary>
        /// Resolves the specified req.
        /// </summary>
        /// <param name="req">The req.</param>
        /// <returns></returns>
        /// <exception cref="BindingNotFoundException"></exception>
        public object Resolve(IRequest req)
        {
            return this._scope.Resolve (req);
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
            return _scope.ResolveAll (req);
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
            return _scope.CanResolve (service);
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
            return _scope.CanResolve (request);
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
            var binding = new Binding<T>
            {
                Deactivator = o => {
                    if (o is IDisposable) 
                        ((IDisposable)o).Dispose();
                },
            };
            AddBinding(binding);
            return new BindingTo<T>(binding);
        }

        /// <summary>
        /// Binds the specified type.
        /// </summary>
        /// <param name="serviceType">The service type.</param>
        /// <returns></returns>
        public IBindingTo Bind(Type serviceType)
        {
            var binding = new Binding(serviceType)
            {
                Deactivator = o =>
                {
                    if (o is IDisposable)
                        ((IDisposable)o).Dispose();
                },
            };
            AddBinding(binding);
            return new BindingTo(binding);
        }

        /// <summary>
        /// Registers the specified service type.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="instanceType">Type of the instance.</param>
        /// <returns></returns>
        public IBindingContraints Register(Type serviceType, Type instanceType)
        {
            return this.Bind(serviceType).To(instanceType);
        }

        /// <summary>
        /// Registers the specified instance type.
        /// </summary>
        /// <param name="instanceType">Type of the instance.</param>
        /// <returns></returns>
        public IBindingContraints Register(Type instanceType)
        {
            return this.Bind(instanceType).To(instanceType);
        }

        /// <summary>
        /// Adds a binding.
        /// </summary>
        /// <param name="binding">The binding.</param>
        public void AddBinding(IBinding binding)
        {
            _bindingResolver.AddBinding(binding);
        }

        #endregion

        #region Lifecycle
        /// <summary>
        /// Creates a child scope.
        /// </summary>
        /// <returns>
        /// The scope.
        /// </returns>
        public IScope CreateScope()
        {
            return _scope.CreateScope();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _scope.Dispose();
        }

        #endregion

        #region Helper Methods

        private Request CreateRequest(Type service)
        {
            return new Request 
        {
                Resolver = this, 
                ServiceType = service,
                CurrentScope = _scope
            };
        }

        private static bool IsIgnoredAssembly(Assembly assembly)
        {
            var ignorePatterns = new[]
            {
                "Microsoft.",
                "System.",
                "System,",
                "mscorlib,",
                "JetBrains.",
                "nunit.",
                "Envelop,"
            };

            return ignorePatterns.Any(ignorePattern => assembly.FullName.StartsWith(ignorePattern, StringComparison.Ordinal));
        }

        private static bool IsIgnoredType(Type type, Func<Type, bool> registrationPredicate = null)
        {
            var ignoreChecks = new List<Func<Type, bool>>
            {
                t => t.FullName.StartsWith("System.", StringComparison.Ordinal),
                t => t.FullName.StartsWith("Microsoft.", StringComparison.Ordinal),
                t => t.IsPrimitive,
                t => t.IsGenericTypeDefinition,
                t => (t.GetConstructors(BindingFlags.Instance | BindingFlags.Public).Length == 0) && !(t.IsInterface || t.IsAbstract),
            };

            if (registrationPredicate != null)
            {
                ignoreChecks.Add(t => !registrationPredicate(t));
            }

            return ignoreChecks.Any(check => check(type));
        }
        #endregion

    }
}
