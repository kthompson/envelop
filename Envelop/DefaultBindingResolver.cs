using System;
using System.Collections.Generic;
using System.Linq;

namespace Envelop
{
    class DefaultBindingResolver : IBindingResolver
    {
        private readonly Type _bindingType = typeof(IBinding<>);

        public IEnumerable<IBinding> Resolve(IEnumerable<IBinding> bindings, IRequest request)
        {
            var serviceBindingType = _bindingType.MakeGenericType(request.ServiceType);
            return from binding in bindings
                   where serviceBindingType.IsInstanceOfType(binding)
                   where binding.Constraints.All(c => c.IsMatch(request))
                   select binding;
        }
    }
}