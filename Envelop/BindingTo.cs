using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Envelop
{
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

        public IBindingContraints To(Func<IResolver, object> func)
        {
            _binding.Activator = req => func(req.Resolver);

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

                //Look through constructors to find a ctor we can activate
                foreach (var item in cache)
                {
                    // Create a request to see if we can resolve all parameters of this ctor
                    var requests =
                        item.Parameters.Select(p => CreateRequest(req, targetType, p.ParameterType)).ToArray();
                    if (!requests.All(resolver.CanResolve))
                        continue;

                    //Now lets resolve each parameter for this ctor
                    var args = requests.Select(request =>
                    {
                        if (request.MultiInjection == MultiInjection.None)
                            return resolver.Resolve(request);

                        var enumerable = resolver.ResolveAll(request).ToArray();

                        if (request.MultiInjection == MultiInjection.List)
                            return CreateList(enumerable, request.ServiceType);

                        if (request.MultiInjection == MultiInjection.Enumerable ||
                            request.MultiInjection == MultiInjection.Array)
                            return CreateArray(enumerable, request.ServiceType);

                        throw new ActivationFailedException();
                    });

                    return item.Constructor.Invoke(args.ToArray());
                }

                // we couldnt find a ctor that we could activate
                throw new BindingNotFoundException(req, targetType);
            };

            return Constraints();
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
            //TODO: detect array multi-injection on the request
            var mi = MultiInjection.None;

            if (serviceType.IsGenericType)
            {
                if (serviceType.GetGenericTypeDefinition() == typeof (IEnumerable<>))
                {
                    mi = MultiInjection.Enumerable;
                    serviceType = serviceType.GetGenericArguments()[0];
                }
                else if (serviceType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    mi = MultiInjection.List;
                    serviceType = serviceType.GetGenericArguments()[0];
                }

            }
            else if (serviceType.IsArray)
            {
                mi = MultiInjection.Array;
                serviceType = serviceType.GetElementType();
            }

            return new Request
            {
                ParentRequest = req, 
                ServiceType = serviceType, 
                Resolver = req.Resolver, 
                Target = targetType, 
                MultiInjection = mi,
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

        public IBindingContraints To<TImplementation>(Func<IResolver, TImplementation> func) 
            where TImplementation : T
        {
            return base.To(resolver => func(resolver));
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