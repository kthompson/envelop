using System;

namespace Envelop
{
    class BindingContraints<T> : IBindingContraints<T>
    {
        private readonly IBinding<T> _binding;

        public BindingContraints(IBinding<T> binding)
        {
            _binding = binding;
        }

        public IBindingContraints<T> When(Predicate<IRequest> predicate)
        {
            _binding.Constraints.Add(new BindingConstraint(predicate));
            return this;
        }

        public IBindingContraints<T> AsSingleton()
        {
            var original = _binding.Activator;
            object instance = default(T);
            _binding.Activator = req =>
                {
                    if (Equals(instance, default(T)))
                        instance = original(req);

                    return instance;
                };

            return this;
        }
    }
}