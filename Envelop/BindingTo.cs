using System;
using System.Collections.Generic;
using System.Linq;

namespace Envelop
{
    class BindingTo<T> : IBindingTo<T>
    {
        private readonly IBuilder _builder;
        private readonly IBinding<T> _binding;

        internal BindingTo(IBuilder builder, IBinding<T> binding)
        {
            _builder = builder;
            _binding = binding;
        }

        public IBindingContraints<T> To<TImplementation>() 
            where TImplementation : T
        {
            var targetType = typeof (TImplementation);

            var cache = targetType.GetConstructors().Select(ctor => new {Constructor = ctor, Parameters = ctor.GetParameters()});

            _binding.Activator = req =>
            {
                foreach (var item in cache)
                {
                    var requests = item.Parameters.Select(p => CreateRequest(req, targetType, p.ParameterType)).ToArray();
                    if (!requests.All(r => _builder.CanResolve(r))) 
                        continue;

                    var args = requests.Select(request =>
                    {
                        if (request.MultiInjection == MultiInjection.Enumerable)
                            return _builder.ResolveAll(request);

                        if (request.MultiInjection == MultiInjection.Array)
                            return _builder.ResolveAll(request).ToArray();

                        if (request.MultiInjection == MultiInjection.List)
                            return _builder.ResolveAll(request).ToList();

                        return _builder.Resolve(request);
                    });

                    return item.Constructor.Invoke(args.ToArray());
                }

                throw new BindingNotFoundException();
            };

            return Constraints();
        }

        private Request CreateRequest(IRequest req, Type targetType, Type serviceType)
        {
            //TODO: detect array multi-injection on the request
            var mi = MultiInjection.None;

            if (serviceType.IsGenericType && serviceType.GetGenericTypeDefinition() == typeof (IEnumerable<>))
            {
                mi = MultiInjection.Enumerable;
                serviceType = serviceType.GetGenericArguments()[0];
            }

            return new Request
            {
                ParentRequest = req, 
                ServiceType = serviceType, 
                Resolver = _builder, 
                Target = targetType, 
                MultiInjection = mi
            };
        }

        public IBindingContraints<T> To<TImplementation>(Func<IBuilder, TImplementation> func) 
            where TImplementation : T
        {
            _binding.Activator = req => func(_builder);

            return Constraints();
        }

        public IBindingContraints<T> To<TImplementation>(TImplementation instance) 
            where TImplementation : T
        {
            _binding.Activator = req => instance;

            return Constraints();
        }

        private BindingContraints<T> Constraints()
        {
            return new BindingContraints<T>(_binding);
        }
    }
}