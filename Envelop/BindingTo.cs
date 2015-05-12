using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Envelop
{
    class CallSites
    {

        public static readonly Type Func = typeof (Func<>);
        public static readonly Type Resolver = typeof(IResolver);
        public static readonly Type List = typeof(List<>);
        public static readonly Type IEnumerable = typeof(IEnumerable<>);
        public static readonly Type Lazy = typeof(Lazy<>);


        public static readonly MethodInfo ResolverResolveRequest = Resolver.GetMethod("Resolve", new[] { typeof(IRequest) });
    }

    class BindingTo : IBindingTo
    {
        private readonly IBinding _binding;

        public BindingTo(IBinding binding)
        {
            _binding = binding;
        }

        public IBindingContraints To(object instance)
        {
            _binding.Activator = req => instance;

            return Constraints();
        }
        
        public IBindingContraints To<TImplementation>(Func<IResolver, TImplementation> func)
        {
            _binding.Activator = req => (object)func(req.Resolver);

            return Constraints();
        }

        public IBindingContraints To(Type targetType)
        {
            var cache = targetType.GetConstructors()
                .Select(ctor => new {Constructor = ctor, Parameters = ctor.GetParameters()})
                .ToArray();

            _binding.Activator = req =>
            {
                var resolver = req.Resolver;
                var sb = new StringBuilder();

                //Look through constructors to find a ctor we can activate
                foreach (var item in cache)
                {
                    // Create a request to see if we can resolve all parameters of this ctor
                    var requests = item.Parameters.Select(p => CreateRequest(req, targetType, p.ParameterType)).ToArray();
                    sb.AppendLine("=============================================================");
                    sb.AppendLine("Attempting to resolve parameters for: " + item.Constructor);
                    var unresolved = requests.Where(r => !resolver.CanResolve(r)).ToArray();
                    if (unresolved.Length > 0)
                    {
                        foreach (var u in unresolved)
                            sb.AppendLine("   Unresolved service type: " + u.ServiceType.FullName);

                        continue;
                    }

                    //Now lets resolve each parameter for this ctor
                    var args = requests.Select(request =>
                    {
                        if (request.InjectionMode == InjectionMode.None)
                            return resolver.Resolve(request);

                        if (request.InjectionMode == InjectionMode.Factory)
                            return CreateFactory(resolver, request);

                        if (request.InjectionMode == InjectionMode.Lazy)
                            return CreateLazy(resolver, request);

                        var enumerable = resolver.ResolveAll(request).ToArray();

                        if (request.InjectionMode == InjectionMode.List)
                            return CreateList(enumerable, request.ServiceType);

                        if (request.InjectionMode == InjectionMode.Enumerable ||
                            request.InjectionMode == InjectionMode.Array)
                            return CreateArray(enumerable, request.ServiceType);

                        throw new ActivationFailedException();
                    });

                    return item.Constructor.Invoke(args.ToArray());
                }

                // we couldnt find a ctor that we could activate
                throw new BindingNotFoundException(req, targetType, sb.ToString());
            };

            return Constraints();
        }

        private object CreateFactory(IResolver resolver, IRequest request)
        {
            var type = CallSites.Func.MakeGenericType(request.ServiceType);

            var resolverExpr = Expression.Constant(resolver, CallSites.Resolver);
            var requestExpr = Expression.Constant(request);
            var resolveCallExpr = Expression.Call(resolverExpr, CallSites.ResolverResolveRequest, requestExpr);
            var castExpr = Expression.Convert(resolveCallExpr, request.ServiceType);
            var lambda = Expression.Lambda(type, castExpr);

            return lambda.Compile();
        }

        private object CreateLazy(IResolver resolver, IRequest request)
        {
            var fact = CreateFactory(resolver, request);
            var lazyType = CallSites.Lazy.MakeGenericType(request.ServiceType);
            var lazy = Activator.CreateInstance(lazyType, fact);
            return lazy;
        }

        private static object CreateArray(object[] enumerable, Type serviceType)
        {
            var array = Array.CreateInstance(serviceType, enumerable.Length);
            enumerable.CopyTo(array, 0);
            return array;
        }

        private static object CreateList(object[] enumerable, Type serviceType)
        {
            var listType = typeof(List<>).MakeGenericType(serviceType);
            var list = (IList)Activator.CreateInstance(listType);
            foreach (var item in enumerable)
                list.Add(item);

            return list;
        }

        private Request CreateRequest(IRequest req, Type targetType, Type serviceType)
        {
            var mi = InjectionMode.None;

            if (serviceType.IsGenericType)
            {
                var genericTypeDefinition = serviceType.GetGenericTypeDefinition();
                if (genericTypeDefinition == CallSites.IEnumerable)
                {
                    mi = InjectionMode.Enumerable;
                    serviceType = serviceType.GetGenericArguments()[0];
                }
                else if (genericTypeDefinition == CallSites.List)
                {
                    mi = InjectionMode.List;
                    serviceType = serviceType.GetGenericArguments()[0];
                }
                else if (genericTypeDefinition == CallSites.Func)
                {
                    mi = InjectionMode.Factory;
                    serviceType = serviceType.GetGenericArguments()[0];
                }
                else if (genericTypeDefinition == CallSites.Lazy)
                {
                    mi = InjectionMode.Lazy;
                    serviceType = serviceType.GetGenericArguments()[0];
                }
            }
            else if (serviceType.IsArray)
            {
                mi = InjectionMode.Array;
                serviceType = serviceType.GetElementType();
            }

            return new Request
            {
                ParentRequest = req, 
                ServiceType = serviceType, 
                Resolver = req.Resolver, 
                Target = targetType, 
                InjectionMode = mi,
                CurrentScope = req.CurrentScope
            };
        }

        private BindingContraints Constraints()
        {
            return new BindingContraints(_binding);
        }
    }

    class BindingTo<T> : BindingTo, IBindingTo<T>
    {
        public BindingTo(IBinding<T> binding)
            : base(binding)
        {
        }

        public IBindingContraints To<TImplementation>() 
            where TImplementation : T
        {
            return base.To(typeof (TImplementation));
        }

        public new IBindingContraints To<TImplementation>(Func<IResolver, TImplementation> func) 
            where TImplementation : T
        {
            return base.To(func);
        }

        public IBindingContraints To<TImplementation, T1>(Func<T1, TImplementation> func) where TImplementation : T
        {
            return base.To(resolve => func(resolve.Resolve<T1>()));
        }

        public IBindingContraints To<TImplementation, T1, T2>(Func<T1, T2, TImplementation> func) where TImplementation : T
        {
            return base.To(resolve => func(resolve.Resolve<T1>(), resolve.Resolve<T2>()));
        }

        public IBindingContraints To<TImplementation, T1, T2, T3>(Func<T1, T2, T3, TImplementation> func) where TImplementation : T
        {
            return base.To(resolve => func(resolve.Resolve<T1>(), resolve.Resolve<T2>(), resolve.Resolve<T3>()));
        }

        public IBindingContraints To<TImplementation, T1, T2, T3, T4>(Func<T1, T2, T3, T4, TImplementation> func) where TImplementation : T
        {
            return base.To(resolve => func(resolve.Resolve<T1>(), resolve.Resolve<T2>(), resolve.Resolve<T3>(), resolve.Resolve<T4>()));
        }

        public IBindingContraints To<TImplementation, T1, T2, T3, T4, T5>(Func<T1, T2, T3, T4, T5, TImplementation> func) where TImplementation : T
        {
            return base.To(resolve => func(
                resolve.Resolve<T1>(),
                resolve.Resolve<T2>(),
                resolve.Resolve<T3>(),
                resolve.Resolve<T4>(),
                resolve.Resolve<T5>()));
        }

        public IBindingContraints To<TImplementation, T1, T2, T3, T4, T5, T6>(Func<T1, T2, T3, T4, T5, T6, TImplementation> func) where TImplementation : T
        {
            return base.To(resolve => func(
                resolve.Resolve<T1>(),
                resolve.Resolve<T2>(),
                resolve.Resolve<T3>(),
                resolve.Resolve<T4>(),
                resolve.Resolve<T5>(),
                resolve.Resolve<T6>()));
        }

        public IBindingContraints To<TImplementation, T1, T2, T3, T4, T5, T6, T7>(Func<T1, T2, T3, T4, T5, T6, T7, TImplementation> func) where TImplementation : T
        {
            return base.To(resolve => func(
                resolve.Resolve<T1>(),
                resolve.Resolve<T2>(),
                resolve.Resolve<T3>(),
                resolve.Resolve<T4>(),
                resolve.Resolve<T5>(),
                resolve.Resolve<T6>(),
                resolve.Resolve<T7>()));
        }

        public IBindingContraints To<TImplementation>(TImplementation instance) 
            where TImplementation : T
        {
            return base.To(instance);
        }
    }
}