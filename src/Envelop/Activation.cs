using System;

namespace Envelop
{
    class Activation : IActivation 
    {
        Action _deactivator;

        public Activation (Action deactivator)
        {
            this._deactivator = deactivator;
        }

        #region IActivation implementation

        public void Deactivate ()
        {
            this._deactivator ();
        }

        public object Object { get; set ; }

        public IScope Scope { get; set; }

        #endregion
    }
}