# App Service
* App service is a fully managed service with built in infrastructure that supports maintenance, security, patching and scaling. 
* It automatically includes a built in load balancer that comes into picture when we scale our application. 
* publish the code and just run it.
* No access to underlying servers. Security and comlieance mainteaned by microsoft. 
* Intergarte with many popular source controls like github, devops etc.
* Web Apps vs App Service: both are same. 
* App services can also run batch jobs, in web jobs you can upload exe or schedule timers.
* App services can be accessed using Http and Https both. You can make it Https only using TLS/SSL settings. 
* Even you stop app service, you have to pay for it. unlike VM, if you stop VM, pricing stops.

**Supported platforms or technologies:** 
* .Net,  Java, Node JS,  PHP, Python

**Supported application types: **

* Web Apps
* Web API
* Web jobs

**App Service Tiers:**

* Free - For trial purposes
* Basic - For Dev/Test environments
* Standard - For production workloads
* Premium - Enhanced performance and scale
* Isolated - High performance/security and isolation

Linux app services are cheaper than windows. 


**Ways of publishing code to an app service?**

* Publish through code 
* Publish through a container image 
* Use a static web application 

** How to use an app service if the run time is not available?**

We need in that case we need to use container image. 

** Choosing runtime for any app service:**

Runtime selection impacts to the selection of operating system because if the application runtime does not support in Linux or in Windows then the options are only available as per the selection of runtime. 

**App Service Editor**

We can see files uploaded to app service. 

**Plural App Services**

App services with multiple IP addresses.  

**Console**

Command line access to access file system where app service is running.

**In Bound and Out Bound IP Addresses** 


 **App Service Plan**

An app service always runs in an App Service plan. In addition, [Azure Functions](https://learn.microsoft.com/en-us/azure/azure-functions/dedicated-plan) also has the option of running in an App Service plan. An App Service plan defines a set of compute resources for a web app to run.

When you create an App Service plan in a certain region (for example, West Europe), a set of compute resources is created for that plan in that region. Whatever apps you put into this App Service plan run on these compute resources as defined by your App Service plan. Each App Service plan defines:

* Operating System (Windows, Linux)
* Region (West US, East US, and so on)
* Number of VM instances
* Size of VM instances (Small, Medium, Large)
* Pricing tier (Free, Shared, Basic, Standard, Premium, PremiumV2, PremiumV3, Isolated, IsolatedV2)

The pricing tier of an App Service plan determines what App Service features you get and how much you pay for the plan. The pricing tiers available to your App Service plan depend on the operating system selected at creation time. 

**How does my app run and scale?**

In the Free and Shared tiers, an app receives CPU minutes on a shared VM instance and cannot scale out. In other tiers, an app runs and scales as follows.

When you create an app in App Service, it's part of an App Service plan. When the app runs, it runs on all the VM instances configured in the App Service plan. If multiple apps are in the same App Service plan, they all share the same VM instances. If you have multiple deployment slots for an app, all deployment slots also run on the same VM instances. If you enable diagnostic logs, perform backups, or run [WebJobs](https://learn.microsoft.com/en-us/azure/app-service/overview-hosting-plans), they also use CPU cycles and memory on these VM instances.

In this way, the App Service plan is the scale unit of the App Service apps. If the plan is configured to run five VM instances, then all apps in the plan run on all five instances. If the plan is configured for autoscaling, then all apps in the plan are scaled out together based on the autoscale settings.

**Should I put an app in a new plan or an existing plan?**

Since you pay for the computing resources your App Service plan allocates (see [How much does my App Service plan cost?](https://learn.microsoft.com/en-us/azure/app-service/overview-hosting-plans#cost)), you can potentially save money by putting multiple apps into one App Service plan. You can continue to add apps to an existing plan as long as the plan has enough resources to handle the load. However, keep in mind that apps in the same App Service plan all share the same compute resources.

###### SKU, Size & ACU
*Acu: are called as as your computer unit. These are like indexing or masking for the performance of servers. 

###Creating New App Service
###### Deployment tab: 
Here we can enable GitHub actions to continuously deploy your application. 

###### Network tab configurations: 
What is virtual network? 

###### App Service Configuration:
###### * Application settings:
We can specify connection string, other sensitive appl settings here instead of hard coding them in code.
 
###### * General Settings
* Make HTTPS traffic only
* We can choose TLS version
* We can change runtime 
* We can platform as 32/64 bit
* We can enable/disable FTP based deployments, lets say we only want FTP deployments or we want to skip FTP deployments
* We can force specific HTTP version such as 1.1 or 2.0
* Enable web sockets

** How to debug app service application?**

In general setting we can enable feature to attach visual studio debugging.

###### * Default document settings tab: we can specify default document and priority order.
###### * Path mappings tab: 
###### * SSL Settings

* We can change/set HTTPS only or TLS version. 
* If we have any custom domain and HTTPS only enabled, we need a certificate. So we can upload/buy PFX certificate from here.  

** What is server farm?**

** What are HTTP versions?**

 
### Scaling of App Services: 

We can scale and ab service in two ways scale up and scale out. 

###### Scale Up: Switching and app service plan: 
If we switch to the another plan then there are no no interruptions because it create a new application service and switch the traffic to the new one so the existing one is not impacted. 

_** Manual scaling:**_ 
Basic tear has just manual scaling option

** Automatic scaling:**

* Only standard and premium tears has capability for the auto scaling options 
* If an app service plan has multiple apps deployed within it scaling applies to all of the applications under the same app service plan. 

** Scaling Based on Rules**

* Matric: Where we can define some rules like if the CPU usage is more than 70% and we can also set up skill down rules. 
* Based on schedule: For example we need to scale of the application for for working hours on daily basis 
* Scale up/down application automatically by alerts: By any programming we can reach an alert and based on the alert we can configure if the light is received scale of the application. 

### Deployment slots: 
* If you want to create multiple environment like Staging testing Dev environment. We can use deployment slots for it. Deployment slots represents different environment. 

* Deployment slots allow us to upload code and test it seperatly from main site. After validation we can swap the slots and promote to production.

** Deployment slot traffic splitting:** 

* Traffic can be spilitted between slots
* Some users will be routed to production and some to new slots.

_** Deployment slot swapping:**_ 
Suppose We created a new feature and that is ready in the Staging after the testing, now we want to move this feature to the production so we use the feature of swapping. Content of Staging plan get shifted to the production deployments lot while content of production deployments lot s to staging that can be used as a backup purpose. In case of any production errors we can again swipe it back to the previous version
This feature is only available for the standard and premium plans. 

### Deployment Types

Traffic splitting enable various types of deployments

* Basic - All instances update to new versions at once. No use of deployment slots.
* Rolling - Use deployment slots and split network traffic in percentage
* Blue Green - Use deployment slots. Use slots for testing purpose, Once tested. Swap slot.
 
### App Service Monitoring: 
##### Setting up alerts: 
for issues like high CPU
Enable application inside share. 
We also get capability to download the arm template share. 

##### Create Metric charts 
Pin these metric charts to dashboard
For errors, CPU usages

#### Diagnostics logging:
###### * Enable Failed request tracing:
###### * Enable Web server logging:
###### * Enable Application logging: 
* Enable application logging
* It requires storage account so choose/create storage account
* Choose/create a private container under storage account.
* Choose retention period for logs

* Configure app insights for application level logs.
* Configure process level logs 

##### Log Stream Service: 
* In case of debugging, we want to see real time logging. A console appears with live logs those are generated by application.
* We need to enable file system application logging. It automatically switches of in 12 hours
* Start streaming of logs

### How to solve or how to find find out the deployment problems? 
There are two ways: 
* By web app console: We can see the content of www root folder. We can see the underlying files directories and dll that should be available for the application. 
* Kudu app: 

# App Service Interview Questions
###### What is App Service?
Azure App Service is a service for hosting the below type of web applications​

* App Service – Web App​
* App Service – API Apps (Web API, WCF etc)​
* App Service – Mobile back ends.

###### What technologies are supported in App Service?
We can develop in our favorite language, be it .NET, .NET Core, Java, Ruby, Node.js, PHP, or Python. ​

###### What is the difference between App Service and App Service Plan?
App Service Plan: It’s a managed service that provides the infrastructure (like CPU, RAM etc) for hosting App Services

App Service: It’s a managed service which is used to deploy Web Applications inside the App Service Plan

###### What type of applications can be deployed in App Service?
Below type of web applications are supported in App Service

* App Service – Web App​
* App Service – API Apps (Web API, WCF etc)​
* App Service – Mobile back ends.

###### Is it possible to create configuration settings in App Service? If yes, what type of configuration settings can be created?
Yes, It’s possible to create the Configuration Setting as Key-Value pairs. App Service supports

* Application Settings
* Connection Strings

###### In app service. Is it possible to change the runtime stack after the app service is created?
Yes. It’s possible

###### What is the significance of always on property in app service?
When Always On is turned on in the configuration, it prevents your app from being idled out due to inactivity

###### What is the significance of ARR affinity setting in Azure app service?
ARR affinity – when it’s turned off, the Load Balancer will decides to which server the request should be forwarded. When ARR Affinity is turned on, then the request from a client will always reach the same server for that entire session. For compatibility reasons, ARR Affinity is turned on by default.

###### Is it possible to Configure default documents or default page in Azure app service
Yes, It can be done in the Default Documents tab of the Configuration blade of the App Service

###### Is it possible to do a remote debugging of applications hosted inside app service
Yes. It’s possible. We need to perform the below steps.

* Turn on Remote Debugging in the General Settings tab of the Configuration blade of App Service.
* We also need to deploy the application in Debug mode.

###### How do you achieve zero downtime deployment in Azure app service.
In Azure app service. Zero downtime deployment can be achieved using a feature called Deployment Slots.

Deployment slots is available in App Service which is hosted in Standard pricing tier or above.

###### Is it really possible to do a blue green deployment in Azure app service
Yes. It is possible to implement blue green deployments in azure app service using the feature called deployment slots

###### What is deployment slot of Azure app service? Can you please explain the significance of deployment slots?
Deployment slots are live apps with their own host names. Deploying your application to a non-production slot has the following benefits:

You can validate app changes in a staging deployment slot before swapping it with the production slot. Deploying an app to a slot first and swapping it into production makes sure that all instances of the slot are warmed up before being swapped into production. This eliminates downtime when you deploy your app.

###### How do you ensure that the connection strings of database are different for different environments within Azure app service
For all Application Settings and Connection Strings, there is a special attribute setting called Deployment Slot Setting . If you turn this on, then that particular App Setting will become specific to that deployment slot.

###### Are there any limitations or challenges that you have faced with Deployment slots in App Service?
As all the Deployment slots (including the Production Slots) share the same infrastructure provdied by the App Service Plan, it is not recommended to perform load testing on staging slots.

###### What are the different types of IP addresses that are available with Azure app service.
App service supports 2 types of IP addresses.

* Inbound IP address – It is used to configure A record for DNS configuration.
* Outbound IP addresses – App Service has multiple Outbound IP Addresses. These are used when app service makes external calls to any other service like databases, web APIs etc. In case if your SQL Database only allows access from IP addresses that are whitelisting in SQL Server, then we need to white list all the outbound IP addresses of app service.

###### What is an Azure App Service Plan
App Service plan is a managed Service from Azure. It helps in hosting web applications. App Service plan provides us

* Azure Compuet units.
* RAM
* Storage
And other features. These feature depend on the Pricing tiers.

###### What are various Pricing Tiers.
App Service plan has multiple pricing tiers.

* Free
* Shared
* Basic
* Standard
* Premium
In all these 3 pricing tiers, we also have various performance levels,

for example,
* In Basic – B1, B2 and B3.
* In Standard – S, S2 and S3
* In Premium – P1, P2 and P3

###### What are different types of Operating Systems that can be used in an App Service Plan?
* Linux
* Windows

###### Is it possible to choose an Operating System version while creating the App Service Plan?
NO. It’s not possible to choose OS version. If you want to choose a specific version of OS, then you should consider Virtual Machines.

###### Is it possible to Change the Operating System from Windows to Linux after the App Service Plan is created?
No. It’s not possible.

###### In which Pricing tier Deployment Slots could be created?
Standard Or Higher .

###### In which tier Scale-out (Load Balancer) is possible?​
Basic or Above

###### In which tier Auto Scaling is possible?​
Standard or Higher

###### Is it possible to create Windows and Linux App Service Plans in the same Resource Group?​
Yes

###### Is it possible to log into App Service Plan?
App Service Plan is a Managed Service. So, It’s not possible to Log-in to App Service Plan.

###### How Many App Services can be created in one App Service Plan?
Technically, it is possible to add infinite number of App Services in one App Service Plan (except in Free or Shared Plans)

###### What is Scaling?
The Process of increasing the Capacity of the Servers is called Scaling.

###### What are different types of Scaling?
* Vertical Scaling
* Horizantal Scaling

###### What is Verical Scaling and how do you implement it in App Service Plan?
Vertical scaling is a process of increasing the capacity of an individual server. For example. In app Service plan, if the current pricing tier is Standard S1. Then you want to increase the capacity from by S1 to S2. Then we need to do scale up.

And when you want to decrease the capacity from S2 to S1. Then we need to do scale down.

###### What is Horizantal Scaling and how do you implement it in App Service Plan?
In horizontal scaling we can add multiple servers (along with Load Balancer) to increase the capacity. App Service Plan supports horizontal scaling ONLY in Basic or higher pricing tier.

###### What is scale-out and Scale-in?
When you want to increase the number of servers from 1 to 5, then that process is called as scale out. The Process of decreasing number of servers from 5 to 4 or even less than that upto 1, then that process is called as scale in.

App Service plan allows us to scale out and scale in in two different ways.

* Manual Scale
* Auto-Scale

###### What is Auto-scale?
* Azure App Service allows us to do Auto-scale in Standard tier or higher. Auto-Scale can be implemented using multiple metrics.
* An example of auto scale rules is: If the average CPU percentage is greater than 70% for the last 5 minutes, then we can automatically add a new instance. And if the average CPU percentage of all the servers is less than 40%, then we can decrease one more one server from the cluster.
