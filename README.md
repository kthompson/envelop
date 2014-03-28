# Envelop: Dependency Injection

## en·vel·op
/enˈveləp/

### Verb

1. Wrap up, cover, or surround completely.
2. Make obscure; conceal.

## What is Envelop

Envelop is a simple to use Dependency Injection and Inversion of Control container.

## Examples:

### Interfaces

```c#
interface ISomeInterface
{
}

class SomeInterfaceImplementation : ISomeInterface
{
}

class SomeInterfaceImplementation2 : ISomeInterface
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

interface IMultiInterface
{
	ISomeInterface[] SomeInterfaces { get; }
}

class MultiInterfaceImplementation : IMultiInterface
{
	public ISomeInterface[] SomeInterfaces { get; private set; }

	public MultiInterfaceImplementation(ISomeInterface[] someInterfaces)
	{
		this.SomeInterfaces = someInterfaces;
	}
}

```
### Fluent

```c#
var kernel = Kernel.Create();

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
```

### Injection

```c#
var kernel = Kernel.Create();
kernel.Bind<ISomeInterface>().To<SomeInterfaceImplementation>();
kernel.Bind<IAnotherInterface>().To<AnotherInterfaceImplementation>();

var anotherInterfaceInstance = kernel.Resolve<IAnotherInterface>();
```

### Multi-injection

```c#
var kernel = Kernel.Create();
kernel.Bind<ISomeInterface>().To<SomeInterfaceImplementation>();
kernel.Bind<ISomeInterface>().To<SomeInterfaceImplementation2>();
kernel.Bind<IMultiInterface>().To<MultiInterfaceImplementation>();

var t1 = kernel.Resolve<IMultiInterface>();
var someInterfaces = kernel.ResolveAll<ISomeInterface>();
```
