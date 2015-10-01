using System;

namespace Envelop
{
    class Request : IRequest
    {
        public IResolver Resolver { get; set; }
        public Type ServiceType { get; set; }
        public Type Target { get; set; }
        public IRequest ParentRequest { get; set; }
        public InjectionMode InjectionMode { get; set; }
        public IScope CurrentScope { get; set; }
        public Type[] GenericTypeArguments { get; set; }
    }
}