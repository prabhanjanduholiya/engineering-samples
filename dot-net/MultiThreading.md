# MultiThreading
### Concurrent vs Parallel vs Asynchronous
https://blog.christian-schou.dk/concurrency-vs-parallelism-vs-asynchronous/

All of them are applications of multi-threading, their definitions overlap. 

* **Synchronous execution:** doing things one after the other.
* **Concurrent:** doing multiple things at the same time.
* **Parallel:** doing multiple copies of something at the same time.
* **Asynchronous:** not having to wait for one task to finish before starting another.

#### Asynchronous programming

* If you have any `I/O-bound needs` (such as requesting data from a network, accessing a database, or reading and writing to a file system), you'll want to utilize asynchronous programming. 
* You could also have `CPU-bound code`, such as performing an expensive calculation, which is also a good scenario for writing async code.
* The execution thread should not wait for an I/O-bound or CPU-bound task to finish.

**Using asynchronous programming has several benefits:**
* Avoiding thread pool starvation by “pausing” execution and releasing the thread back to thread pool during asynchronous activities
* Keeping the UI responsive
* Possible performance gains from concurrency

###### Asynchronous Programming Patterns:
##### Asynchronous programming model (APM):
* Also known as an IAsyncResult pattern, it’s implemented by using two methods: BeginOperationName and EndOperationName.
* After calling BeginOperationName, an application can continue executing instructions on the calling thread while the asynchronous operation takes place on a different thread. For each call to BeginOperationName, the application should also call EndOperationName to get the results of the operation.

```
public class MyClass { 
    public IAsyncResult BeginRead(byte [] buffer, int offset, int count, AsyncCallback callback, object state) {...};
    public int EndRead(IAsyncResult asyncResult);
} 
```

##### Event-based asynchronous pattern (EAP):
* This pattern is implemented by writing an OperationNameAsync method and an OperationNameCompleted event:
* The asynchronous operation will be started with the async method, which will trigger the Completed event for making the result available when the async operation is completed. A class that uses EAP may also contain an OperationNameAsyncCancel method to cancel an ongoing asynchronous operation.

```
public class MyClass { 
    public void ReadAsync(byte [] buffer, int offset, int count) {...};
    public event ReadCompletedEventHandler ReadCompleted;
} 
```

##### Task-based asynchronous pattern (TAP): 
* We have only an OperationNameAsync method that returns a Task or a generic `Task<T>` object:
* Task and `Task<T>` classes model asynchronous operations in TAP. It’s important to understand Task and `Task<T>` classes for understanding TAP, which is important for understanding and using async/await keywords, so let’s talk about these two classes in more detail.
  
```
public class MyClass { 
    public Task<int> ReadAsync(byte [] buffer, int offset, int count) {...};
} 
```

#### Task
The `Task` and `Task<T>` classes are the core of asynchronous programming in .NET. They facilitate all kinds of interactions with the asynchronous operation they represent, such as:
* Adding continuation tasks
* Blocking the current thread to wait until the task is completed
* Signaling cancellation (via CancellationTokens)
  
#### Async - Await

**async keyword **

The async keyword is added to the method signature to enable the usage of the await keyword in the method. It also instructs the compiler to create a state machine to handle asynchronicity, but that’s out of the scope of this article.

The return type of an async method is always `Task` or `Task<T>`. It’s checked by the compiler, so there’s not much room for making mistakes here.
  
**await keyword**

The await keyword is used to asynchronously wait for a Task or `Task<T>` to complete. It pauses the execution of the current method until the asynchronous task that’s being awaited completes. The difference from calling .Result or .Wait() is that the await keyword sends the current thread back to the thread pool, instead of keeping it in a blocked state.

Under the hood, it:

* Creates a new Task or `Task<T>` object for the remainder of the async method
* Assigns this new task as a continuation to the awaited task,
* Assigns the context requirement for the continuation task
That last bit is also the part that causes deadlocks in some situations. We’re going to talk about it later, but first let’s see the async and await keywords in action.  

**Avoiding Deadlocks**
Executing async operations synchronously by blocking the execution thread brings the risk of creating a deadlock.

  ##### Exception Handling
  
  **References: **
https://www.sitepoint.com/asynchronous-programming-using-async-await-in-c/



### Threads & Tasks:
* What is multithreading.
* How to achieve fault tolerance in multi-threaded environment.
* Difference between Thread and Tasks.
* Multi-threading vs async programming?
* async await life cycle
* does async create a new thread
* Manual reset vs auto reset
* Thread vs async and await.
* async await vs task
* How deadlock occurs in async and await and how to resolve those.
* aggregated exception in c#
* task connected and attached with child
* Use of configure await false in await async
* What is deadlock and how to prevent
* Can we achieve parallel programming through async and await
* Deadlock and race condition
* How async await works internally
* Slimlock.
* Continue With vs Await
* Difference between await and Task.ContinueWith
* Difference b/w Task.WaitAll and Task.WhenAll
* Diff between task and thread
* What is Appdomain in C#? when to use?
* What is multithreading.
* How to achieve fault tolerance in multi-threaded environment.
* Difference between Thread and Tasks.
* Multi-threading vs async programming?
* async await life cycle
* does async create a new thread
* Manual reset vs auto reset
* Thread vs async and await.
* async await vs task
* How deadlock occurs in async and await and how to resolve those.
* aggregated exception in c
* task connected and attached with child
* Use of configure await false in await async
* What is deadlock and how to prevent
* Can we achieve parallel programming through async and await
* Deadlock and race condition
* How async await works internally
* Slimlock.
* Continue With vs Await
* Difference between await and Task.ContinueWith
* Difference b/w Task.WaitAll and Task.WhenAll
* Diff between task and thread
* What is Appdomain in C? when to use?
* What is a Task? - Task encorporates th state of async operation.


