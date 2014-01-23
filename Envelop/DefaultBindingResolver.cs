using System.Collections.Generic;
using System.Linq;

namespace Envelop
{
    class DefaultBindingResolver : IBindingResolver
    {
        public IEnumerable<IBinding> Resolve(IEnumerable<IBinding> bindings, IRequest request)
        {
            return from binding in bindings
                   where binding.ServiceType == request.ServiceType
                   where binding.Constraints.All(c => c.IsMatch(request))
                   select binding;
        }
    }
}