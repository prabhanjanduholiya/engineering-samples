
# Software Design Principles
Software Design Principles are a set of guidelines that helps developers to make a good system design. Good system design means clean, maintainable applications.

## Common design principles
1. Separation of concerns
2. Encapsulation 
3. Dependency inversion
4. Explicit dependencies
5. Single responsibility
6. Don't repeat yourself (DRY)
7. Persistence ignorance
8. Bounded contexts
9. YAGNI ("you aren't gonna need it")
10. KISS ("Keep it simple stupid")

### 1: Separation of concerns
Separation of concerns is a design principle for separating a computer program into distinct sections, such that each section addresses a separate concern. For example the business logic of the application is a concern and the user interface is another concern. Changing the user interface should not require changes to business logic and vice versa.

###### Why
* Simplify development and maintenance of software applications.
* When concerns are well-separated, individual sections can be reused, as well as developed and updated independently.

###### How
* Break program functionality into separate modules that overlap as little as possible.

### 2: Encapsulation 
Different parts of an application should use encapsulation to insulate them from other parts of the application. 

In classes, encapsulation is achieved by limiting outside access to the class's internal state. If an outside actor wants to manipulate the state of the object, it should do so through a well-defined function (or property setter), rather than having direct access to the private state of the object. Likewise, application components and applications themselves should expose well-defined interfaces for their collaborators to use, rather than allowing their state to be modified directly.

### 3: Dependency inversion
The direction of dependency within the application should be in the direction of abstraction, not implementation details.

Dependency inversion is a key part of building loosely coupled applications, since implementation details can be written to depend on and implement higher-level abstractions, rather than the other way around. The resulting applications are more testable, modular, and maintainable as a result. The practice of dependency injection is made possible by following the dependency inversion principle.

### 4: Single responsibility - (Single responsibility at modular level)

In a monolithic application, we can apply the single responsibility principle at a high level to the layers in the application. Presentation responsibility should remain in the UI project, while data access responsibility should be kept within an infrastructure project. Business logic should be kept in the application core project, where it can be easily tested and can evolve independently from other responsibilities.

When this principle is applied to application architecture and taken to its logical endpoint, you get microservices. A given microservice should have a single responsibility. If you need to extend the behavior of a system, it's usually better to do it by adding additional microservices, rather than by adding responsibility to an existing one.

### 5: DRY - (Don't repeat yourself)
The application should avoid specifying behavior related to a particular concept in multiple places as this practice is a frequent source of errors. At some point, a change in requirements will require changing this behavior. It's likely that at least one instance of the behavior will fail to be updated, and the system will behave inconsistently.

Rather than duplicating logic, encapsulate it in a programming construct. Make this construct the single authority over this behavior, and have any other part of the application that requires this behavior use the new construct.


### 6: Bounded contexts
Bounded contexts are a central pattern in Domain-Driven Design. They provide a way of tackling complexity in large applications or organizations by breaking it up into separate conceptual modules. Each conceptual module then represents a context that is separated from other contexts (hence, bounded), and can evolve independently. 

### 7: YAGNI - ("you aren't gonna need it")
YAGNI stands for "you aren't gonna need it": don't implement something until it is necessary.

###### Why
Any work that's only used for a feature that's needed tomorrow, means losing effort from features that need to be done for the current iteration.
It leads to code bloat; the software becomes larger and more complicated.

###### How
Always implement things when you actually need them, never when you just foresee that you need them.

### 8: KISS - (Keep It Simple Stupid)
Most systems work best if they are kept simple rather than made complex.

###### Why
Less code takes less time to write, has less bugs, and is easier to modify.
Simplicity is the ultimate sophistication.
It seems that perfection is reached not when there is nothing left to add, but when there is nothing left to take away.

### 9: Explicit dependencies - (Dependencies should be injected/provided through constructors)
Methods and classes should explicitly require any collaborating objects they need in order to function correctly. 

Class constructors provide an opportunity for classes to identify the things they need in order to be in a valid state and to function properly. If you define classes that can be constructed and called, but that will only function properly if certain global or infrastructure components are in place, these classes are being dishonest with their clients. The constructor contract is telling the client that it only needs the things specified (possibly nothing if the class is just using a parameterless constructor), but then at run time it turns out the object really did need something else.

By following the explicit dependencies principle, your classes and methods are being honest with their clients about what they need in order to function. Following the principle makes your code more self-documenting and your coding contracts more user-friendly, since users will come to trust that as long as they provide what's required in the form of method or constructor parameters, the objects they're working with will behave correctly at run time.

#### 10: Persistence ignorance
**What is a Data Transfer Object (DTO)?**

* Data transfer object is a light weight class which only contains properties, and we can get and set these properties of class.
* DTO doesn’t contain any behavior and custom logic.

**Use of DTO**

DTO is for transferring data between layers.

**Why make an object that simple?**

For making a type container just for collecting data without any custom logic and due to this light weight, it can easily transfer between layers.

```
public class ClientDTO  
{  
    public String FirstName  
    public String LastName  
    public String Email  
    public String PhoneNo  
    public String MobileNo  
    public String County  
    public String City  
    public String Address  
}  
```

**What’s a POCO?**

POCO stands for Plain Old CLR Object, POCO is a business object which contains data, validation and custom or business logic but it doesn’t contain persistence logic, which  means logic which is related to data stores or database, so due to this POCO are persistent ignorant.

For example

Suppose we have “ClientPOCO” class, so it can contains properties, business logic and validation but
It doesn’t contain persistence logic such as logic related to the dat, for  example SaveClient() or GetClientById(). This means POCO class only contains properties, validation and business logic but doesn’t contain any data logic.

**Persistence Ignorance**

* Persistence ignorance means ignoring persistence logic; this means logic related to data store,
* In EF, POCO class doesn’t contains logic which is related to data stores, like saving data into data store or fetching data from data stores.

Below is the code for my BAL.ClientPOCO class which is a POCO.

```
public class ClientPOCO   
{  
    Public String FirstName  
    Public String LastName  
    Public String Email  
    Public String PhoneNo  
    Public String MobileNo  
    Public String County  
    Public String City  
    Public String Address  
    Public List < ClientPOCO > Retrieve()  
    {  
        // code which access component of Data access layer, and then data access layer interact with data stores, POCO classes not contains data access code or persistence code.   
    }  
}  
```

**Difference between POCO and DTO**
 
POCO classes have state and behavior but DTO only has state, no behavior.
Summary

So when you are working on business layers with business objects, POCO class will consider which contains properties and business logic but without any persistence logic, and POCO class is not dependent on DB structure. And DTO is just a light weight class which  contains properties only and is used to transfer data between layers or between applications.


**References:**
https://docs.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/architectural-principles
