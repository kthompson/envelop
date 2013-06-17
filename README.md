# Envelop: Dependency Injection

## en·vel·op
/enˈveləp/

### Verb

1. Wrap up, cover, or surround completely.
2. Make obscure; conceal.

## About Envelop

Envelop is a simple to use Dependency Injection and Inversion of Control container.

## Examples:

### Interfaces

    interface ISomeInterface
    {
    }

    class SomeInterfaceImplementation : ISomeInterface
    {
    }

    interface IAnotherInterface
    {
        ISomeInterface SomeInterface { get; }
    }

    class AnotherInterfaceImplementation : IAnotherInterface
    {
        public ISomeInterface SomeInterface { get; private set; }

        public AnotherInterfaceImplementation(ISomeInterface someInterface)
        {
            this.SomeInterface = someInterface;
        }
    }

### Fluent
	
    var kernel = new Kernel();
    
    // Automatic construction
    kernel.Bind<ISomeInterface>().To<SomeInterfaceImplementation>();
	
	// Factory based construction
    kernel.Bind<ISomeInterface>().To(_ => new SomeInterfaceImplementation());

    // Singletons
    kernel.Bind<ISomeInterface>().To(new SomeInterfaceImplementation());
    kernel.Bind<ISomeInterface>().To<SomeInterfaceImplementation>().AsSingleton();
    kernel.Bind<ISomeInterface>().To(req => new SomeInterfaceImplementation()).AsSingleton();

    // Constraints
	kernel.Bind<ISomeInterface>().To<SomeInterfaceImplementation2>().When(req => typeof(IAnotherInterface).IsAssignableFrom(req.Target));


### Injection

	var kernel = new Kernel();
	kernel.Bind<ISomeInterface>().To<SomeInterfaceImplementation>();
    kernel.Bind<IAnotherInterface>().To<AnotherInterfaceImplementation>();

    var anotherInterfaceInstance = kernel.Resolve<IAnotherInterface>();

### Multi-injection

    var kernel = new Kernel();
    kernel.Bind<ISomeInterface>().To<SomeInterfaceImplementation>();
    kernel.Bind<ISomeInterface>().To<SomeInterfaceImplementation2>();
    kernel.Bind<IMultiInterface>().To<MultiInterfaceImplementation3>();

    var t1 = kernel.Resolve<IMultiInterface>();
    var someInterfaces = kernel.ResolveAll<ISomeInterface>();