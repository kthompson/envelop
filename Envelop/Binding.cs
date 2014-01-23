using System;
using System.Collections.Generic;

namespace Envelop
{
    class Binding : IBinding
    {
        public Binding(Type serviceType)
        {
            this.ServiceType = serviceType;
            this.Constraints = new List<IBindingConstraint>();
        }

        public object Activate(IRequest request)
        {
            var activator = Activator;
            if (activator == null)
                throw new IncompleteBindingException();

            return activator(request);
        }

        public Func<IRequest, object> Activator { get; set; }
        public Type ServiceType { get; private set; }
        public List<IBindingConstraint> Constraints { get; private set; }

        public override string ToString()
        {
            return this.ServiceType.ToString();
        }
    }

    class Binding<T> : Binding, IBinding<T>
    {
        public Binding()
            : base(typeof(T))
        {
        }
    }
}