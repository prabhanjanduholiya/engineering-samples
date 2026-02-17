An architecture style is a family of architectures that share certain characteristics.

# N-tier

![image](https://github.com/user-attachments/assets/9dc1e288-58e6-454e-ae2e-6f9ed3e0054d)

[N-tier](https://learn.microsoft.com/en-us/azure/architecture/guide/architecture-styles/n-tier) is a traditional architecture for enterprise applications. Dependencies are managed by dividing the application into layers that perform logical functions, such as presentation, business logic, and data access. A layer can only call into layers that sit below it.

An N-tier application can have a closed layer architecture or an open layer architecture:

In a closed layer architecture, a layer can only call the next layer immediately down.
In an open layer architecture, a layer can call any of the layers below it.

#### When to use this architecture
N-tier architectures are typically implemented as infrastructure-as-service (IaaS) applications, with each tier running on a separate set of VMs. However, an N-tier application doesn't need to be pure IaaS. Often, it's advantageous to use managed services for some parts of the architecture, particularly caching, messaging, and data storage.

Consider an N-tier architecture for:

Simple web applications.
A good starting point when architectural requirements are not clear yet.
Migrating an on-premises application to Azure with minimal refactoring.
Unified development of on-premises and cloud applications.

#### Benefits
Portability between cloud and on-premises, and between cloud platforms.
Less learning curve for most developers.
Relatively low cost by not rearchitecting the solution
Natural evolution from the traditional application model.
Open to heterogeneous environment (Windows/Linux)

#### Challenges
Monolithic design prevents independent deployment of features.
Managing an IaaS application is more work than an application that uses only managed services.
It can be difficult to manage network security in a large system.
User and data flows typically span across multiple tiers, adding complexity to concerns like testing and observability.

# Microservices
# Web-Queue-Worker
![image](https://github.com/user-attachments/assets/a2f84373-ce26-4d52-ba6e-cb1f873d4b97)
In this style, the application has a web front end that handles HTTP requests and a back-end worker that performs CPU-intensive tasks or long-running operations. The front end communicates to the worker through an asynchronous message queue.

Web-queue-worker is suitable for relatively simple domains with some resource-intensive tasks. The use of managed services simplifies deployment and operations. But with complex domains, it can be hard to manage dependencies. 

# Event-driven architecture
[Event-Driven Architectures](https://learn.microsoft.com/en-us/azure/architecture/guide/architecture-styles/event-driven) use a publish-subscribe (pub-sub) model, where producers publish events, and consumers subscribe to them. The producers are independent from the consumers, and consumers are independent from each other.

Consider an event-driven architecture for applications that ingest and process a large volume of data with very low latency, such as IoT solutions. The style is also useful when different subsystems must perform different types of processing on the same event data.
![image](https://github.com/user-attachments/assets/1b0fbf42-b006-4ce5-b342-a45a4ab3b680)
