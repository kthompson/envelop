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

        public IActivation Activate(IRequest request)
        {
            var activator = Activator;
            if (activator == null)
                throw new IncompleteBindingException();

            var obj = activator (request);
            var deactivator = this.Deactivator;

            Action deactivation = () => {
                if(deactivator != null)
                    deactivator(obj);
            };
            var activation = new Activation (deactivation) 
            { 
                Object = obj,
                Scope = request.CurrentScope,
            };

            request.CurrentScope.AddActivation (activation);

            return activation;
        }

        public Action<object> Deactivator { get; set; }
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
