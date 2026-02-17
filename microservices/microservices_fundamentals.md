# Microservice architecture 
* Microservices are a popular architectural style for building applications that are resilient, highly scalable, independently deployable, and able to evolve quickly.
* Each service is self-contained and should implement a single business capability within a bounded context. 
* A `bounded context` is a natural division within a business and provides an explicit boundary within which a domain model exists.

#### What are Microservices?
* Microservices are small, independent, and loosely coupled. A single small team of developers can write and maintain a service.
* Each service is a separate codebase, which can be managed by a small development team.
* Services can be deployed independently. A team can update an existing service without rebuilding and redeploying the entire application.
* Services are responsible for persisting their own data or external state. This differs from the traditional model, where a separate data layer handles data persistence.
* Services communicate with each other by using well-defined APIs. Internal implementation details of each service are hidden from other services.

### Benefits of Microservices? 
* Independently deployable
* Easier to release and bug fixing
* Small, focused teams
* Smaller codebase to maintain
* Flexibility of technology
* Scalability

### Challenges of Microservices
* Complexity 
* Data integrity 
* Network congestion and latency. 

### Process for building a microservices architecture

###### STEP 1: 
Start by analyzing the business domain to understand the application's functional requirements. The output of this step is an informal description of the domain, which can be refined into a more formal set of domain models.

###### STEP 2:
Next, define the bounded contexts of the domain. Each bounded context contains a domain model that represents a particular subdomain of the larger application.

###### STEP 3:
Within a bounded context, apply tactical DDD patterns to define entities, aggregates, and domain services.

###### STEP 4:
Use the results from the previous step to identify the microservices in your application.

## Interservice communication for microservices
There are two basic messaging patterns that microservices can use to communicate with other microservices.

### Synchronous versus asynchronous messaging
#### * `Synchronous communication`:  
In this pattern, a service calls an API that another service exposes, using a protocol such as HTTP or gRPC. This option is a synchronous messaging pattern because the caller waits for a response from the receiver.

#### * `Asynchronous message passing`:
In this pattern, a service sends message without waiting for a response, and one or more services process the message asynchronously.

##### Benefits of asynchronous message passing
* Reduced coupling
* Multiple subscribers
* Failure isolation. If the consumer fails, the sender can still send messages.
* Load leveling. A queue can act as a buffer to level the workload, so that receivers can process messages at their own rate.

##### Drawbacks of asynchronous message passing
* Complexity
* Cost 
