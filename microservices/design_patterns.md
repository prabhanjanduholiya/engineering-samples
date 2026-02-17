# 1. API Gateway
The API gateway is the entry point for clients. Instead of calling services directly, clients call the API gateway, which forwards the call to the appropriate services on the back end.

Advantages of using an API gateway include:
* It decouples clients from services. Services can be versioned or refactored without needing to update all of the clients.
* Services can use messaging protocols that are not web friendly, such as AMQP.
* The API Gateway can perform other cross-cutting functions such as authentication, logging, SSL termination, and load balancing.
* Out-of-the-box policies, like for throttling, caching, transformation, or validation.

##### Drawbacks of point to point communication
* It becomes chatty communication, too many backend calls
* Code duplicity for cross cutting concerns
* Challenges with non internet friendly protocols

##### Features of API Gateways
* Reverse Proxy/ Gateway routing
* Cross cutting concerns
* Request aggregation

##### Drawbacks of API Gateways
* Possible single point of failure

# 2. Backend For Frontend (BFF)
Having multiple API gateways for different kind of clients like mobile, web app etc.

# 3. Data & Transactions Management Patterns 
An important rule for microservices architecture is that each microservice must own its domain data and logic. Just as a full application owns its logic and data, 
so must each microservice own its logic and data under an autonomous lifecycle, with independent deployment per microservice.

Therefore, microservices-based applications often use a mixture of SQL and NoSQL databases, which is sometimes called the polyglot persistence approach.

##### The relationship between microservices and the Bounded Context pattern
The concept of microservice derives from the Bounded Context (BC) pattern in domain-driven design (DDD).

**DDD and Bounded Contexts**

DDD deals with large models by dividing them into multiple BCs and being explicit about their boundaries. 
Each BC must have its own model and database; likewise, each microservice owns its related data.
A microservice is therefore like a Bounded Context, but it also specifies that it’s a distributed service.

##### Data in Monolithic Archetecture
In the traditional approach, there’s a single database shared across all services, typically in a tiered architecture.
A monolithic application with typically a single relational database has two important benefits: ACID transactions and the SQL language, 
both working across all the tables and data related to your application. 
This approach provides a way to easily write a query that combines data from multiple tables.


### Distributed transactions in microservices
Often in this scenario, the success of a transaction is all or nothing — if one of the participating services fails, the entire transaction must fail.

There are two cases to consider:

* A service may experience a transient failure such as a network timeout. These errors can often be resolved simply by retrying the call. If the operation still fails after a certain number of attempts, it's considered a nontransient failure.

* A nontransient failure is any failure that's unlikely to go away by itself. Nontransient failures include normal error conditions, such as invalid input. They also include unhandled exceptions in application code or a process crashing. If this type of error occurs, the entire business transaction must be marked as a failure. It may be necessary to undo other steps in the same transaction that already succeeded.

After a nontransient failure, the current transaction might be in a partially failed state, where one or more steps already completed successfully. 
In that case, the application needs to undo the steps that succeeded, by using a `Compensating Transaction design pattern`. In some cases, this must be done by an external system.

##### Compensating Transaction design pattern
https://docs.microsoft.com/en-us/azure/architecture/patterns/compensating-transaction

![compensating-transaction-diagram](https://user-images.githubusercontent.com/31764786/154896285-adae2ea6-24bb-49ea-97f3-2e0fe2074dd3.png)

 
### Compensating a transaction pattern
https://docs.microsoft.com/en-us/azure/architecture/patterns/compensating-transaction 

### Health checks 
### Resiliency
  * Retries with exponential back off plus circuit breaker (Use 'Polly' for retries with exponential back off in case of .Net based microservice)
  * How to minimize exponential failures in microservices? 
    * Circuit Breakers (Use polly polcies)
    * Avoid long http call chains in same request/response
### Publish/Subscriber 
### Scale out with orchastrators

# 4. Anti-corruption Layer pattern
Implement a façade or adapter layer between different subsystems that don't share the same semantics. This layer translates requests that one subsystem makes to the other subsystem. 
![anti-corruption-layer](https://user-images.githubusercontent.com/31764786/154901748-f897a184-d179-4bf1-99bc-7f1930866a99.png)

###### Context and problem: 
Most applications rely on other systems for some data or functionality. For example, when a legacy application is migrated to a modern system, it may still need existing legacy resources. New features must be able to call the legacy system. This is especially true of gradual migrations, where different features of a larger application are moved to a modern system over time.

# 5. CQRS pattern (CQRS stands for Command and Query Responsibility Segregation)

A pattern that separates read and update operations for a data store. Implementing CQRS in your application can maximize its performance, scalability, and security. The flexibility created by migrating to CQRS allows a system to better evolve over time and prevents update commands from causing merge conflicts at the domain level.
 
https://docs.microsoft.com/en-us/azure/architecture/patterns/cqrs 
