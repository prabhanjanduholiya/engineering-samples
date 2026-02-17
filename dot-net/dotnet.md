# Framework
###### What is Intermediate Language Code (IL Code)?
Any .Net application code is compiled into IL code. 

###### What is Common Language Runtime (CLR)?
It is an execution engine for .Net applications. Core functions are
* Code execution
* Garbage collection
* Application memory isolation
* Verification of type safety

###### What is Managed code?
Code that runs in environment of CLR. All Intermediate code are managed code. 

###### What is Unmanaged code?
Code that doesn't run under the control of CLR.

###### What is JIT Compiler?
* JIT compiler converts the IL code to machiene specific code at runtime and then excutes this.
* Machine specific code means native code

###### What are Types of JIT?
* Pre JIT
* Econo JIT
* Normal JIT

###### What is Code access security? 
CAS grants rights to program depending upon security configurations of machine. 
Let's say program tries to modify a file while settings to folder for file are read only.

###### What is Code verification?
It prevents the source code to perform illegal operation such as accessing invalid memory. 

###### What is CTS - Common type specification?
In oreder that two languages can communicate smoothly, CLR has CTS.

* Eg. VB has data type as 'Integer' while c++ has data type 'long'.
* In order to talk between these languages both data types are converted into data type of CTS. In this case it is 'System.Int32'.

###### What is CLS - Common language specification?
It is a subset of CTS. These are nothing but guidelines that a language should follow so that it can communicate with other languages in seamless manner.

###### What is Assembly?
A unit of deployment like dll or exe.

###### What is Assembly manifest?
Manifest contains meta data of assembly such as version, security, identity etc.

###### What is Global assembly cache (GAC)?
GAC is where all shared .Net assembly reside.

###### What is Reflection? 
Using reflection we can dynamically call methods, dynamically load classes etc.

###### Diffrence between `Convert.ToString()` and `.ToString()`? 
`Convert.ToString()` handles null while `.ToString()` throws `null reference error`.

### What are Parameter passing techniques? 
* Pass by value
* Pass by reference
    * Ref 
    * Out       

#### Pass by value:
* This is the default way, in this way a duplicate copy is made and sent to the called function. 
* So, if we change parameter value inside method - It won't impact outside the method.

#### Pass by reference: 
* Passing by reference uses actual address of variable so when we change te value inside called function, it will impact the original one.
    
###### By Ref
Process of `ref` is bidirectional, we must have to initialize it and once we update it - changes will be reflected outside the method call.

###### By Out
Flow of out parameter is unidirectional, we don't have to initialize it. We just get the value out of the method. 

###### By `Param` keyword
By using the params keyword, you can specify a method parameter that takes a variable number of arguments. The parameter type must be a single-dimensional array.

No additional parameters are permitted after the params keyword in a method declaration, and only one params keyword is permitted in a method declaration.

###### What is difference between `String` vs `StringBuilder`?
* String is immutable, when we modify a string new memory is allocated to new string value. 
* String builder is designed to use same object.

###### What is difference between Const vs readonly? 
* you can use a readonly keyword to declare a readonly variable. This readonly keyword shows that you can assign the variable only when you declare a variable or in a constructor of the same class in which it is declared.

## Important Access Modifiers
#### Protected Internal (Protected or Internal)
* Accessible in current assembly, doesn't matter class is drived or not.
* Outside assembly access is allowed to drived types only.

#### Private Protected (Protected and Internal)
* Accessible to drived claases in same assembly.

### `dynamic` keyword
The dynamic type indicates that use of the variable and references to its members bypass compile-time type checking. Instead, these operations are resolved at run time. The dynamic type simplifies access to COM APIs such as the Office Automation APIs, to dynamic APIs such as IronPython libraries, and to the HTML Document Object Model (DOM).

###### What is difference between `dynamic` and  `object`? 
The dynamic type differs from object in that operations that contain expressions of type dynamic are not resolved or type checked by the compiler. 

```
dyn = dyn + 3; 
obj = obj + 3; // Here compile time error occurs.

```

###### Dynamic vs Var?

### `yeild` keyword
* What is `yeild` keyword?
* What are Usages of `yeild` keyword?
* What is difference between `yeild return` and `yeild break`?
* How to do Exception handling with yeild keywords? 
  - `Yeild return` can't be located in `try catch` block while it can be in `try finally` block.
  - `yeild break` can't be located in `try finally` block while it can be in `try catch` block.
* How "yeild" works internally?

* When you use `yeild` keyword in a statement, it indicates that the 'method', 'get accessor' or 'the operator' in which it appears is an iterator.
* Being an iterator it allows to do stateful iteration over the collection.
* By using yeild keyword, we don't need an extra class or collection that holds the state for an enumeration.

###### Usages of `yeild` keyword
###### `yeild return` : 
We can use yeild return to return values. 

Example: 

###### `yeild break` : 
You can use yeild break to end the iteration.

Example:  

###### Exception handling: 
* `Yeild return` can't be located in `try catch` block while it can be in `try finally` block.
* `yeild break` can't be located in `try finally` block while it can be in `try catch` block.

###### Using `yeild` in get acessor: 
###### How "yeild" works internally?

### `static` keyword
### Static class: 
* A static class can be used as convenient class for the methods that work on input parameters. They don't need/use any instance member.
* A static class is loaded by CLR when any program that refrences it gets loaded. 
* Creating static class is same as creating non-static class with private constructor and static members. Benefit of static class is compiler can check to make sure no instance member is added.
* We can't specify when the class should be loaded. 
* static class can't be instansiated. static class is sealed, it can't be inherited.
* static class can't contain non static constructor.
* We can access the members by it's class name.

### Static constructor:
* static CTOR is used to initialize static data members.
* static CTOR can't be parametrized.
* static constructor is called just one time and remain alive for lifetime of application.
* Non static classes can also have static constructor. 

#### Let's suppose base class has one static and one non-static CTOR and child class also has one static and one non-static CTOR. What will be the order of execution?  
See code example in `CodeToLearn` Repo.
#### When to use singleton class when to use static?
A Singleton can implement interfaces, inherit from other classes and allow inheritance. While a static class cannot inherit their instance members. So Singleton is more flexible than static classes and can maintain state.

C# What is difference between Singleton Pattern and Static class vb.net asp.net
A Singleton can be initialized lazily or asynchronously and loaded automatically by the .NET Framework CLR (common language runtime) when the program or namespace containing the class is loaded. While a static class is generally initialized when it is first loaded and it will lead to potential class loader issues.

###### What is difference between `Continue` vs `Break`?
* Continue -> Jumps out of current iteration
* Break -> Jumps out of loop

## Extension menthods 
Extension methods enable you to add methods to existing types without modifying existing class or driving new class from it. 

#### Creating extension methods: 

* Create a static method in static class. 
* 'this' keyword should be used to 1st parameter of method.
* Type of first parameter should be the type you want to extend

``` 
public static MyExtension
{
    public static int GetData(this MyType t)
    {
      // Any logic
    }
}
```

## Nullable Types
* Nullable types allows us to assign null value to value types as by default null values can't be assigned to value types. 
* These are instances of `System.Nullable<T>` struct.

##### Can we have reference type as nullable?
These are only applied to structs not to refernece types.

```
public struct Nullable<T> where T: struct
{
    public bool HasValue { get; }
    
    public T Value { get; }
}
```

## `SingleOrDefault()` vs `FirstOrDefault()` 
SingleOrDefault asserts that only one item exists. It will throw error if No or more than one item exists.

### `Sealed` keyword (Sealed classes and Sealed Method):

##  What is difference between `New` and `Override` keywords?
[Refer test MethodHidingVsOverridingTest in CodeToLearn Repo]()

##### What are Virtual Methods?
Virtual method tells the compiler that this method can be overriden in drived class.

##### 'Override' keyword: 
This is used to override the method implementation of base class into drived class.

``` Base b = new Base();  // Base class method will be called```

``` Base b = new Child(); // Child class method will be called```

``` Child c = new Child(); // Child class method will be called```

##### 'New' keyword:
It hides the defination of base class method and gives new implementation in child class.

``` Base b = new Base();  // Base class method will be called```

``` Base b = new Child(); // Base class method will be called```

``` Child c = new Child(); // Child class method will be called```

`In case of new, if child instance is assigned in base type variable - still it will execute base class method.`



