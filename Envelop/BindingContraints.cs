using System;

namespace Envelop
{
    class BindingContraints : IBindingContraints
    {
        private readonly IBinding _binding;

        public BindingContraints(IBinding binding)
        {
            _binding = binding;
        }

        public IBindingContraints When(Predicate<IRequest> predicate)
        {
            _binding.Constraints.Add(new BindingConstraint(predicate));
            return this;
        }

        public IBindingContraints AsSingleton()
        {
            var original = _binding.Activator;
            object instance = null;
            _binding.Activator = req =>
            {
                if (Equals(instance, null))
                    instance = original(req);

                return instance;
            };

            return this;
        }

        public IBindingContraints AfterDeactivation(Action<object> action)
		{
			var save = _binding.Deactivator ?? (o => {});

			_binding.Deactivator = o => {
				save(o);
				action(o);
			};

			return this;
		}
    }
}