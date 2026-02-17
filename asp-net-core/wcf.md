
**ASP.NET Web API vs WCF**

###### WebAPI

* Supports only HTTP protocol.
* Maps http verbs to methods
* Uses routing and controller concept similar to ASP.NET MVC.
* Does not support Reliable Messaging and transaction.
* Ideal for building RESTful services.

###### WCF

* Supports HTTP, TCP, UDP and custom transport protocol.
* Uses attributes based programming model.
* Uses Service, Operation and Data contracts.
* Supports Reliable Messaging and Transactions.
* Supports RESTful services but with limitations.

# Windows communication Foundation

## WCF interview Questions

##### What is service?
A unit of functionality exposed to client is called service.

##### What is client?
Consumer of any service.

### WCF Endpoint:
Endpoint = Address + Binding + Contract

##### What is an Address?
An URL which specifies the location of hosted service. 

##### What is WCF Binding?
Binding specifies how to access the service. It defines the communication protocol, message encoding technique, configured timeouts and message size. 

##### What are message encoding techniques in WCF? 
* XML 1.0
* MTOM
* Binary
* Custom

##### What is Contract?
Specifies what kind of data can be transported from one end to another. 

##### What is WCF and why it is required instead of web service?
* Web services does't support duplex communication
* WCF supports inbuilt logging through configuration but web services require custom logging. 
* net.pipe and MSMQ are not supported
* WCF has better reliabilty and security

##### What are transport schemes supported by WCF?
* Http/Https
* Net.Tcp
* net.Pipe
* MSMQ
* ServiceBus

### What are WCF contracts?
An agreement between cliant and service. 

##### Service Contract: 
Which service is exposed to client. 

##### Operation Contract:
Which methods are exposed to outside world/client. 

##### Data Contract:
Classes used for communication b/w client and service. 

##### Fault Contract: 
* Which errors are raised from service. 
* Service handles their errors and rethrows/propogates fault contracts to clients. 

##### Message Contract:
* WCF uses SOAP message for communication. It converts data contract to SOAP message. 
* Message contracts are used when you want more control over SOAP messages. 
* SOAP envlope contains SOAP header and SOAP body. 

```
[MessageContract]
public class Shape
{
  [MessageHeader]
  public string Id {get;set;}
  
  [MessageBodyMember]
  public double Area {get;set;}
}
```

##### How to host WCF Service?
* IIS Hosting 
    - Through .svc file
    - By creating host in configuration file. 
         
* Self Hosting 
* WAS Hosting 

##### When to use Net.TCP binding in WCF?
* Can be accessed on intranet only. 
* This binding use binary encoding of SOAP messages. 

##### When to use Net.PIPE binding in WCF?
* When client and service reside at same machine.
* Performance is very high
* Binary encoded messages sent over named pipes.

##### When to use MSMSQ binding in WCF?
* When you want to execute operation in queued manner.
* These bindings are one-way, they doesn't return response. 

##### What can be achieved through binding configuration?
##### What is message ordering in WCF?
##### What is reliable session in WCF?
Reliable session provide message level guarntee for WCF commuinication b/w client and server or guarantees to report failure if message delivery fails.

##### What are default endpoints? 
If service doesn't provide endpoints but provide one base address then WCF itself creates endpoints relative to the base address and the contract name.

##### How to enable meta-data publishing in WCF?
By defining service metadata tag to the service behaviour. 

```
<behaviours>
    <ServiceBehaviour name="xyz">
        <ServiceMetaData httpGetEnabled="true"/>
    </ServiceBehaviour>
</behaviours>
```

##### What are supported bindings for metadata exchange in WCF? 
* MexHttpBinding
* MexNamedPipeBinding
* MexTcpBinding

##### How to generate proxy of WCF service? 
SVCUtil.exe

##### What is service locator, How to use service locator for creating proxy? 
Service locator is common design pattern that allows decoupling of clients from it's services. 

##### How to implement operation overloading in WCF? 
By using the name property of operation contract attribute. 

##### What are known types? 

##### What are service known types?

##### What are Instance context mode? 
Controls over WCF instance creation. 
* Singelton
* PerCall
* PerSession

```
[ServiceBehaviour(InstanceContextMode=InstanceContextMode.Single)]
public class MyService : IService
```
##### What are release instance mode? 
Controls when service intance is recycled. 
* None
* BeforeCall
* AfterCall
* BeforeAndAfterCall

##### What is throttling behaviour of service? 
Throttling behaviour for service controls
* MaxConcurrentCalls
* MaxConcurrentInstances
* MaxConcurrentSessions

```
<behaviours>
    <ServiceBehaviour name="xyz">
        <behaviour>
          <servicethrottling
          MaxConcurrentCalls = 500
          MaxConcurrentInstances = 100
          MaxConcurrentSessions = 10
          />
        <behaviour>
    </ServiceBehaviour>
</behaviours>
```

##### What are session modes?
* Session.Allowed
* Session.Required
* Session.NotAllowed

##### How to use session modes? 

### Message exchange patterns in WCF
* Request-Reply
* One-way
* Duplex/callback

##### How to use one way MEP? 
`[OperationContract(IsOneWay=true)]`

##### How to use Duplex MEP? 
```
public interface IDuplexCallBack
{
   [OperationContract()]
   void ShowProgress();
}
```

```
[ServiceContract(CallbackContract= typeOf(IDuplexCallBack))]
public interface IMyDuplexService
{
  [OperationContract()]
  bool Operation();
}
```

##### Which bindings support duplex MEP? 
`WsDualHttpBinding`

##### What are concurrency modes? 
* Single
* Multiple
* Renentrant


##### How to use principle permission attribute? 
##### How to implement logging in WCF? 
##### How to read message contract? 
##### How to define service behaviour in WCF? 
##### What is host factory and how to use it?
##### What is channel factory?, when it should be used to create proxies in WCF?
* Channel factory allows you to create the proxy of service only based on service contract. 
* You need not to have complete address but you must have direct access to service contract/data contract

### WCF Security: 
#### Security levels in WCF?
##### * Transport Level Security
* This security happens at channel level. Transport level security is achieved by protocols like HTTPS, TCP. 
* No code changes are required to achieve transpot level security. 
###### *How to implement transport level security
* Create WCF service
* Enable transport level security in web.config
* Enable Https support

```
<Bindings>
  <WsHttpBinding>
    <Binding>
      <Security Mode="Transport"/> 
    </Binding>
  </WsHttpBinding>
</Bindings>
```
##### * Message Level Security
* Message level security is implemented within message data itself. 
* Easiest way to implement message level security is by encrypting data using some algothim 
###### *Message level security should be used when there is any intermediate system between client server communication. Eg. One service calling another service.

##### When to use WSHttpBinding? 
* SOAP messages are encrypted by default. 
* Achives message level security.
* This binding supports WS-* standards.

##### What are WS.* standards? 
* WS-Addressing
* WS-Security
* WS- Reliable messaging

##### Impersonation?

### WCF Transactions? 
#### ACID properties: 
##### *Atomic:
Either all or none operations will make changes to system. If any of them fails, all must be rolled back. 

##### *Consistency:

##### *Isolation:
##### *Duarbility:

##### In how many ways transactions can be executed in WCF? 
* A client starts some transaction perform some work and propogates to server to do some operations in it.
* Client doesn'y start any transaction  but servere wants to do operation within transaction.  
##### How to start transactions in WCF? 
##### What are transaction flow options? 
* Mandatory - (Client must send transaction to server)
* Allowed
* Not Allowed - (Server can't be involked within transaction from client side)

##### What are the security modes in WCF?


