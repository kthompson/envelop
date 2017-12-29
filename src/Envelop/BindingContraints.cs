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
            var objectLock = new object();
            var original = _binding.Activator;
            object instance = null;

            _binding.Activator = req =>
            {
                lock (objectLock)
                {
                    if (Equals(instance, null))
                        instance = original(req);

                    return instance;
                }
            };

            bool deactivated = false;
            var oldDeactivator = _binding.Deactivator;
            _binding.Deactivator = o =>
            {
                if (deactivated)
                    return;

                deactivated = true;
                if (oldDeactivator != null)
                    oldDeactivator(o);
            };

            return this;
        }

        public IBindingContraints AfterDeactivation(Action<object> action)
        {
            if (_binding.Deactivator == null)
            {
                _binding.Deactivator = action;
                return this;
            }

            var save = _binding.Deactivator;
            _binding.Deactivator = o =>
            {
                save(o);
                action(o);
            };
            return this;
        }

        public IBindingContraints BeforeDeactivation(Action<object> action)
        {
            if (_binding.Deactivator == null)
            {
                _binding.Deactivator = action;
                return this;
            }

            var save = _binding.Deactivator;
            _binding.Deactivator = o =>
            {
                action(o);
                save(o);
            };
            return this;
        }

        public IBindingContraints OnActivation(Action<object> action)
        {
            var save = _binding.Activator;
            _binding.Activator = req =>
            {
                var result = save(req);
                action(result);

                return result;
            };
            return this;
        }
    }
}