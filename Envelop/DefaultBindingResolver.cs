using System.Collections.Generic;
using System.Linq;

namespace Envelop
{
    class DefaultBindingResolver : IBindingResolver
    {
		private readonly List<IBinding> _bindings;

		public DefaultBindingResolver ()
		{
			this._bindings = new List<IBinding>();
		}

		public void AddBinding(IBinding binding)
		{
			this._bindings.Add (binding);
		}

        public IEnumerable<IBinding> Resolve(IRequest request)
        {
            return from binding in this._bindings
                   where binding.ServiceType == request.ServiceType
                   where binding.Constraints.All(c => c.IsMatch(request))
                   select binding;
        }
    }
}