
# Object Oriented Programming

###### Programming paradigms:  

Paradigms are ways of programming, relatively unrelated to languages. A paradigm
tells you which programming structures to use, and when to use them. To date, there
have been three such paradigms. 

  * Structured programming 
  * Object oriented programming
  * Functional Programming [https://www.geeksforgeeks.org/functional-programming-paradigm/]
### Pillars of OOPS
* Abstraction
* Encapsulation
* Inheritance
* Polymorphism
  - Compile-time polymorphism
  - Run-time polymorphism

##### Abstraction: 
* Abstraction lets you to focus on what the object does instead of how it does.
* Abstarction provides the genrelized view of your class/object by providing relevant information.

##### Encapsulation
Wrapping of data into single entity is called encapsulation. It is process of hiding internal working of a class. Encapsulation is achieved by access specifiers.

##### Inheritance:
Inheritance is a feature by which class gets the behaviour of base class. 

##### Polymorphism
 An ability to provide different implementations of methods that are implemented by same name. It is of two types

|Compile time (Static) polymorphism                       |Run time polymorphism|
|---------------------------------------------------------|---------------------|
|It means linking of a method to an object at compile time|It means linking of a method to an object at run time|
|It is achieved through method or operator overloading    |Achieved by method overriding|

###### What is Operator overloading and how to achieve it?
Redefining the behaviour of built in operators is called operator overloading. 

```
public static Box Operator+(Box box1, Box box2)
{
 return new Box()
  {
    Length = box1.Length + box2.Length,
    Height = box1.Height + box2.Height
  };
}
```


###### What is difference between Abstraction and Encapsulation?
Abstraction solves the problem at design level while encapsulation solves the problem at implementation level.

###### What is difference between Inheritance and Composition?
* Inheritance can cause long tree heirchey of drived and base classes so composition is an alternative approach. 
* Inheritance follow 'Is-A' relationship while composition follows 'Has-A' relationship.

###### What is difference among `Association`, `Aggregation` and `Composition`?

Aggregation and Composition are subsets of association meaning they are specific cases of association. In both aggregation and composition object of one class "owns" object of another class. But there is a subtle difference:

* Aggregation implies a relationship where the child can exist independently of the parent. Example: Class (parent) and Student (child). Delete the Class and the Students still exist.
* Composition implies a relationship where the child cannot exist independent of the parent. Example: House (parent) and Room (child). Rooms don't exist separate to a House.
https://www.visual-paradigm.com/guide/uml-unified-modeling-language/uml-aggregation-vs-composition/

###### What is difference between Generalization and Specialization?
###### What is difference between Generalization and Inheritance?

###### What is difference between class and object?
* Class is a blueprint for an object. It defines the behaviour of an object through methods and properties.
* Object is an instance of class. It is a runtime entity in object oriented system.

###### What is difference between Value type and Refrence Types?

###### Value Type:
* A data type is value type if it holds a data value within it's own memory. It means variables of these data types directly contain their values.
* Value types are stored in stack.

###### Reference type:
* Reference types are pointers that target an address. It means a refrence type doesn't store it's value directly instead it stores the address where this is being stored.
* Reference types are stored into heap. 

###### What is difference between Boxing and Unboxing?
Coversion of value type to reerence type is boxing while conversion of reference type to value type is unboxing.

```
int i = 10;

Object obj = i; // Boxing

i = (int) o; // Unboxing
```

###### What is difference between Structure and Class? 

|  Structure                                             |Class                          |
|--------------------------------------------------------|-------------------------------|
|  Structures are value types                            | Classes are reference types   |
|  Structures can't be inherited                         | Classes support inheritance   |
|  Structures can't be null                              |                               |
|Can't be abstract||
|Can't have private constructor||
|Must have default constructor||
