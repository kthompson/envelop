using System;

namespace Envelop
{
    class BindingConstraint : IBindingConstraint
    {
        private readonly Predicate<IRequest> _predicate;

        public BindingConstraint(Predicate<IRequest> predicate)
        {
            _predicate = predicate;
        }

        public bool IsMatch(IRequest request)
        {
            return _predicate(request);
        }
    }
}