using System;
using System.Collections.Generic;

namespace Envelop
{
    class Binding<T> : IBinding<T>
    {
        public Binding()
        {
            this.Constraints = new List<IBindingConstraint>();
        }

        public Type ServiceType
        {
            get { return typeof (T); }
        }

        public object Activate(IRequest request)
        {
            var activator = Activator;
            if (activator == null)
                throw new IncompleteBindingException();

            return activator(request);
        }

        public Func<IRequest, object> Activator { get; set; }

        public List<IBindingConstraint> Constraints { get; private set; }

    }
}