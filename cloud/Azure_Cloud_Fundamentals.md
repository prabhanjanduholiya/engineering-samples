# Cloud Computing
Cloud computing is like renting resources. Resources like storage space, CPU etc. You pay for the resources as per your usage. 

The company offering these resources are called cloud providers. 

## Benefits of cloud computing
 ### 1- Cost effective
 - No direct infra cost, no need to purchase costly hardwares that may not be used fully. 
 - Ability to pay/stop paying for additional resources as and when needed or no longer needed.
 - Allows better cost prediction

 ### 2- Scalable
 Azure resources can be scaled in to ways as following: 
 - ##### Virtual scaling (Scale up) 
   Adding more CPU or memory to existing server. 

 - ##### Horizontal scaling (Scale out)   
   Adding more servers to function.

 ### 3- Elastic 
 As workload changes due to a spike or drop in demand, cloud computing systems can compensate by adding and removing resources.

 ### 4- Secure 
 - Physical security
 - Digital security 


 ### 5- Reliable
 Cloud providers offer data backup, disaster recovery and data replication. 
 
 Redundancy is often built in cloud architecture so if one component fails backup component takes place. This is called fault tolerance. 

 ### 6- Global 

## Cloud computing services
Goal of cloud services is to make running business easier and more efficient irrespective of business is small or big. 

Most common services offered by any cloud provider are compute power and storage.

 - Compute power
 - Storage , such as files and database
 - Networking services, such as providing secure connections
 - Analytics services, such as visualising and monitoring data and performance stats.

### Compute power  
When you build your solutions, you can choose that how you want to get your work done as per your own need. You need not to worry about managing infrastructure or hardwares. Your cloud provider will take care of it. 

Options available for compute power are : 

##### Virtual machienes:
 VM is an emulation of computer. The difference is you need not to buy any hardware or installation of OS. 
 Your cloud provider runs your virtual machiene on a physical server in one of their data centers. 
 VM's are best option for compute power if you want more control and responsibility of maintenence. 

 ##### Containers:
 Containers provide consistent, isolated environments for execution of an app.
 They are similar to VM's but they don't have guest OS instead, application with allo of it's dependencies is packaged into a container then standard runtime is used to execute the app.

 Containers start in seconds because there is no OS boot time. You just need to launch the app.
 
 ##### Serverless computing:
 - Serverless computing allows to execute your code without maintaining a server
 - Your application is broken into functions that runs by triggers.
 - Serverless computing is ideal for automated tasks.
 - You only pay for the processing time used by the each function. While VM's and containers are charged even if they are idle. 
 - These are light weight, fast and independently testable and manageble.

### Storage :
Depending on type and need of an application cloud storage options provide variety of types like file, relational databases etc.
As per need you can scale up your database size or sometimes it is also offered automatically.


# Azure
### Pricing models for azure
 - Pay as you go
 - Consumption based
 
### What are pricing models for azure?
#### How cloud computing is cost effective? 
#### Tell something about scalability of Azure. 
#### Tell something about elasticity of cloud computing
#### Tell something about reliablity of cloud computing
#### Tell something about security of cloud computing
### What are cloud computing services?
### What are compute power services 

 

### What about economies of scale?
- Economies of scale is ability to do things more effeciently or at lower cost per unit when operating at large scale.
- Big cloud providers like Microsoft, Amzon, Google has big business leveraging the benefits of economies of scale so these providers can pass these savings to clients.
- Cloud providers can deal with government and definitly that benefit passes to client.


**Regions:**

How to select a region? 
* Closer to audience
* Availability of services, not all services are available
* Basis availability zones (An availability zone means a region has more than one physical data centers that helps in data redundancy within region)
* Pricing: Sometimes prices differ based on regions.

**SLA (Service level agreements)** 

* Uptime percentage of services
* SLA differs basis pricing tiers of resources.

**Resource Groups:**

Logical group for all services.

**Management Groups, Subscriptions, Resource Groups: **


<img width="433" height="281" alt="image" src="https://github.com/user-attachments/assets/4152adbd-16c8-479f-9719-8af95026510a" />

Subscriptions - are associated based on associated account or cost centers.

Management Groups -  are place to manage all subscriptions. If an organization has multiple subscriptions then they will be managed by management groups.

**Pricing Models in Azure:**

* Per resource
* Consumption Plans
* Upfront cost models (Reservation models)

**Budgets Azure:**

* Set budgets 
* Create alerts
