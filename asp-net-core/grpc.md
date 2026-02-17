# gRPC in .Net
gRPC is a language agnostic, high-performance Remote Procedure Call (RPC) framework.

The main benefits of gRPC are:
* Modern, high-performance, lightweight RPC framework.
* Contract-first API development, using Protocol Buffers by default, allowing for language agnostic implementations.
* Supports client, server, and bi-directional streaming calls.
* Reduced network usage with Protobuf binary serialization.

## Where gRPC can be used or it is best fit? 
gRPC is ideal for:

* Lightweight microservices where efficiency is critical.
* Polyglot systems where multiple languages are required for development.
* Point-to-point real-time services that need to handle streaming requests or responses.

## What are important nuget packages for gRPC? 
The gRPC client project requires the following packages:

* Grpc.Net.Client, which contains the .NET Core client.
* Google.Protobuf, which contains protobuf message APIs for C#.
* Grpc.Tools, which contains C# tooling support for protobuf files. The tooling package isn't required at runtime, so the dependency is marked with PrivateAssets="All".

## Proto files? 
gRPC uses a contract-first approach to API development. Protocol buffers (protobuf) are used as the Interface Definition Language (IDL) by default. 

The *.proto file contains:
* The definition of the gRPC service.
* The messages sent between clients and servers.

## Making gRPC calls
A gRPC call is initiated by calling a method on the client. The gRPC client will handle message serialization and addressing the gRPC call to the correct service.

gRPC has different types of methods. How the client is used to make a gRPC call depends on the type of method called. The gRPC method types are:

* Unary
* Server streaming
* Client streaming
* Bi-directional streaming

--------------------------------------------------------------------------------------------------------------------------------------------------------------------------
#### What is Remote procedure call? 
In distributed computing, a remote procedure call (RPC) is when a computer program causes a procedure (subroutine) to execute in a different address space (commonly on another computer on a shared network), which is coded as if it were a normal (local) procedure call, without the programmer explicitly coding the details for the remote interaction.

RPC use an interface description language (IDL) to let various platforms call the RPC. The IDL files can then be used to generate code to interface between the client and servers.

##### Sequence of events
* The client calls the client stub. The call is a local procedure call, with parameters pushed on to the stack in the normal way.
* The client stub packs the parameters into a message and makes a system call to send the message. Packing the parameters is called marshalling.
* The client's local operating system sends the message from the client machine to the server machine.
* The local operating system on the server machine passes the incoming packets to the server stub.
* The server stub unpacks the parameters from the message. Unpacking the parameters is called unmarshalling.
* Finally, the server stub calls the server procedure. The reply traces the same steps in the reverse direction.
