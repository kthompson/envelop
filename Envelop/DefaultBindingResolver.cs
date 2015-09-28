using System;
using System.Collections.Generic;
using System.Linq;

namespace Envelop
{
    class DefaultBindingResolver : IBindingResolver
    {
        private readonly Dictionary<Type, List<IBinding>> _bindings;

        public DefaultBindingResolver()
        {
            this._bindings = new Dictionary<Type, List<IBinding>>();
        }

        public void AddBinding(IBinding binding)
        {
            // always put bindings at the beginning so most recent bindings will be returned first
            List<IBinding> bindings;
            if (_bindings.TryGetValue(binding.ServiceType, out bindings))
            {
                bindings.Insert(0, binding);
            }
            else
            {
                _bindings[binding.ServiceType] = new List<IBinding> {binding};
            }
        }

        public IEnumerable<IBinding> Resolve(IRequest request)
        {
            List<IBinding> bindings;
            if (_bindings.TryGetValue(request.ServiceType, out bindings))
            {
                return from binding in bindings
                    where binding.Constraints.All(c => c.IsMatch(request))
                    select binding;
            }

            return Enumerable.Empty<IBinding>();
        }
    }
}