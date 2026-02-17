 # Web API 

**What is an API?**

API is some kind of interface which has a set of functions that allow programmers to access specific features or data of an application, operating system or other services.

**What is a WebAPI?**

Web API as the name suggests, is an API over the web which can be accessed using HTTP protocol.

**What is an ASP.Net Web API?**

The ASP.NET Web API is an extensible framework for building HTTP based services that can be accessed in different applications on different platforms such as web, windows, mobile etc.

* ASP.NET Web API supports different formats of response data. Built-in support for JSON, XML, BSON format.
* ASP.NET Web API can be hosted in IIS, Self-hosted or other web server that supports .NET 4.0+.

**Action Method Naming Conventions**

* Action method name can be the same as HTTP verbs like Get, Post, Put, Patch or Delete as shown in the Web API Controller example above. However, you can append any suffix with HTTP verbs for more readability.
* you can apply Http verb attributes to method.

**Web API Routing? **

**Parameter Binding in ASP.NET Web API**

By default, if the parameter type is of .NET primitive types such as int, bool, double, string, GUID, DateTime, decimal, 
or any other type that can be converted from string type, then it sets the value of a parameter from the query string.
And if the parameter type is the complex type, then Web API tries to get the value from the request body by default.

**Using `[FromUri]` and `[FromBody]` Attributes**

Use [FromUri] attribute to force Web API to get the value of complex type from the query string and [FromBody] attribute to get the value of primitive type from the request body, opposite to the default rules.

**Action return types**

The Web API action method can have following return types.

* Void
* Primitive type or Complex type
* HttpResponseMessage
* IHttpActionResult

**Web API Request/Response Data Formats**

Media type (aka MIME type) specifies the format of the data as type/subtype e.g. text/html, text/xml, application/json, image/jpeg etc.

In HTTP request, MIME type is specified in the request header using Accept and Content-Type attribute. 
* The Accept header attribute specifies the format of response data which the client expects.
* The Content-Type header attribute specifies the format of the data in the request body so that receiver can parse it into appropriate format.

**ASP.NET Web API: Media-Type Formatters**

As you have seen in the previous section that Web API handles JSON and XML formats based on Accept and Content-Type headers. But, how does it handle these different formats? The answer is: By using Media-Type formatters.

**Media type formatters are classes responsible for serializing request/response data so that Web API can understand the request data format and send data in the format which client expects.**


# `SOA` vs `REST` architecture

### SOA
SOA is way to develop service oriented applications and WCF is technology which can be used to develop service oriented applications. BUT SOA defines strict rules (known as SOA tenets) for applications. If you don't follow these rules you are building services but these services do not conform to SOA.

SOA tenets are:
* Boundaries are explicit - service doesn't share anything with other services (even database tables and data can't be shared)
* Services are autonomous - each service is independent, can be separately deployed and versioned
* Services share schema and contract, not class - services are described in WSDL, transported data are described in XSD, orchestrations (aggregation) are described in BPEL
* Services compatibility is based upon policy - WSDL contains WS-Policies to describe configuration needed for interoperability

WCF allows you to develop plenty of types of services. You can develop interoperable SOAP services which conform to SOA or which doesn't. You can develop pure .NET services with non interoperable features and you can develop REST services.

Moreover in SOA service can have different meaning than in WCF. In WCF service is collection of functionality exposed on endpoints. In SOA the service can be whole application (set of WCF like services) - difference between small and big SOA.

##### SOAP?
* SOAP stands for Simple Object Access Protocol. 
* It’s a messaging protocol for interchanging data in a decentralized and distributed environment. 
* SOAP can work with any application layer protocol, such as HTTP, SMTP, TCP, or UDP. It returns data to the receiver in XML format. 
* Security, authorization, and error-handling are built into the protocol and, unlike REST, it doesn’t assume direct point-to-point communication. Therefore it performs well in a distributed enterprise environment. 
* SOAP follows a formal and standardized approach that specifies how to encode XML files returned by the API. 
* A SOAP message is, in fact, an ordinary XML file that consists of the following parts:
	- Envelope (required) – This is the starting and ending tags of the message.
	- Header (optional) – It contains the optional attributes of the message. It allows you to extend a SOAP message in a modular and decentralized way.
	- Body (required) – It contains the XML data that the server transmits to the receiver.
	- Fault (optional) – It carries information about errors occurring during processing the message.


### REST

###### Principles of REST API:

The six principles of REST API are:

* (1) Stateless - When the request from the client is sent to the server, it contains all the required information to make the server process it.
* (2) Client-Server - Separating the functionality helps to increase user interface portability across multiple platforms as well as extended the scalability of the server components.
* (3) Uniform Interface - To obtain the uniformity throughout the application, REST has defined four interface constraints for which are:

      - Resource Identification
      - Resource Manipulation using representations
      - Self-descriptive massages
      - Hypermedia as the engine of the web application

* (4) Cacheable
* (5) Layered System - The layered system allows an application to be most stable by limiting component behavior. The layered architecture also helps to enhance security as the component at each layer cannot interact beyond each layer they are in.
* (6) Code on demand


## SOAP vs REST Services
* SOAP is a standardized protocol that sends messages using other protocols such as HTTP and SMTP. The SOAP specifications are official web standards, maintained and developed by the World Wide Web Consortium (W3C). 
* As opposed to SOAP, REST is not a protocol but an architectural style. The REST architecture lays down a set of guidelines you need to follow if you want to provide a RESTful web service

As SOAP is an official protocol, it comes with strict rules and advanced security features such as built-in ACID compliance and authorization. Higher complexity, it requires more bandwidth and resources which can lead to slower page load times.


### REST
REST was created to address the problems of SOAP. Therefore it has a more flexible architecture. It consists of only loose guidelines and lets developers implement the recommendations in their own way. It allows different messaging formats, such as HTML, JSON, XML, and plain text, while SOAP only allows XML.

#### Six architectural constraints of REST

###### Uniform interface – 
Requests from different clients should look the same, for example, the same resource shouldn’t have more than one URI.
###### Client-server separation – 
The client and the server should act independently. They should interact with each other only through requests and responses.
###### Statelessness – 
There shouldn’t be any server-side sessions. Each request should contain all the information the server needs to know.
###### Cacheable resources – 
Server responses should contain information about whether the data they send is cacheable or not. Cacheable resources should arrive with a version number so that the client can avoid requesting the same data more than once.
###### Layered system – 
There might be several layers of servers between the client and the server that returns the response. This shouldn’t affect either the request or the response.
Code on demand [optional] – When it’s necessary, the response can contain executable code (e.g., JavaScript within an HTML response) that the client can execute.

REST inherits HTTP operations, meaning you can make simple API calls using the well-known HTTP verbs like GET, POST, PUT, and DELETE.

**SOAP vs. REST comparison table**
Although REST is very popular these days, SOAP still has its place in the world of web services. To help you choose between them, here’s a comparison table of SOAP and REST, that highlights the main differences between the two API styles:

| 	|SOAP|	REST|
|---|----|------|
|Meaning|	Simple Object Access Protocol	|Representational State Transfer|
|Design	|Standardized protocol with pre-defined rules to follow.|	Architectural style with loose guidelines and recommendations.|
|Approach|	Function-driven (data available as services, e.g.: “getUser”)|	Data-driven (data available as resources, e.g. “user”).|
|Statefulness|	Stateless by default, but it’s possible to make a SOAP API stateful.	|Stateless (no server-side sessions).|
|Caching|	API calls cannot be cached.	|API calls can be cached.|
|Security|	WS-Security with SSL support. Built-in ACID compliance.|	Supports HTTPS and SSL.|
|Performance|	Requires more bandwidth and computing power.	|Requires fewer resources.|
|Message format	|Only XML.|	Plain text, HTML, XML, JSON, YAML, and others.|
|Transfer protocol(s)|	HTTP, SMTP, UDP, and others.	|Only HTTP|
|Recommended for|	Enterprise apps, high-security apps, distributed environment, financial services, payment gateways, telecommunication services.|	Public APIs for web services, mobile services, social networks.|
|Advantages|	High security, standardized, extensibility.	| Scalability, better performance, browser-friendliness, flexibility.|
|Disadvantages|	Poorer performance, more complexity, less flexibility.|	Less security, not suitable for distributed environments.|




**How to decide on SOAP or REST**

Both SOAP and REST web services are platform-independent, which means that the client and server machines can use different technologies and programming languages.

To decide whether to use SOAP vs REST for your web service, you’ll need to take the following factors into consideration:

Coupling: for loosely coupled applications, choose REST while for tightly coupled applications, go with SOAP. Most modern web and mobile applications provide loosely coupled web services.
State: if you need to process stateful operations and have complex API calls in which subsequent messages rely on each other, opt for SOAP. However, if your operations are stateless, REST is almost always the better option, especially because it also allows you to store data in the cache of the user’s browser (not suitable for high-security data).
Security: if you need to comply with security law or you transfer highly sensitive data, go with SOAP.
Knowledge of your team: as SOAP has a higher learning curve, you’ll need developers who have the sufficient knowledge. In most places, it’s easier to find developers who have experience with creating REST APIs.
Overall, if you don’t have a reason to use SOAP, such as security or creating a tightly coupled enterprise application, REST will most likely be the better choice. It’s not just easier to code, test, and maintain, but the data transfer also requires less bandwidth, so you can provide a faster web service.
Reference-> https://raygun.com/blog/soap-vs-rest-vs-json/


#### Status Codes
###### 1xx Informational
###### 2xx Success
 * 200 OK
 * 201 Created
 * 202 Accepted
 * 204 No Content

###### 3xx Redirection
* 301 Moved Permanently
* 304 Not Modified
* 307 Temporary Redirect
* 308 Permanent Redirect (experimental)

###### 4xx Client Error
 * 400 Bad Request
 * 401 Unauthorized
 * 403 Forbidden
 * 404 Not Found
 * 409 Conflict
 * 408 Request Timeout
 * 429 Too Many Requests

###### 5xx Server Error
 * 500 Internal Server Error
 * 501 Not Implemented
 * 502 Bad Gateway
 * 503 Service Unavailable
 * 504 Gateway Timeout


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


# [Status Codes:](https://restfulapi.net/http-status-codes/) 
##### 1xx: Informational – Communicates transfer protocol-level information.
##### 2xx: Success – Indicates that the client’s request was accepted successfully.
* 200 (OK)
* 201 (Created)
* 202 (Accepted)
* 204 (No Content)
* 
##### 3xx: Redirection – Indicates that the client must take some additional action in order to complete their request.
* 301 (Moved Permanently)
* 

##### 4xx: Client Error – This category of error status codes points the finger at clients.
* 400 Bad Request
* 401 Unauthorized
* 403 Forbidden
* 408 Request Timeout
* 409 Conflict
* 449 Retry With (Microsoft)
* 
##### 5xx: Server Error – The server takes responsibility for these error status codes.
* 500 Internal Server Error
* 503 Service Unavailable
* 504 Gateway Timeout
* 
