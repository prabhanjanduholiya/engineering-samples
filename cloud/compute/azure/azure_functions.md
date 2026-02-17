
## (3) Implement Azure functions
     - Implement input and output bindings for a function
     - Implement function triggers by using data operations, timers and webhooks
     - Implement Azure durable functions
    
* What are azure function apps?
    - Functions lets you execute code in serverless invironment without having to create VM or publishing code.
    - Function Apps lets you enable to group functions as logical unit for easier management, deployment and sharing of resources.
* What are plans for deployment of azure functions?
    - Consumpsion based - Serverless
    - Premium
    - App service plan

* Which editor options are available to devlop azure functions?
    Azure fucntions can be devloped in 
    - Browser/Azure portal
    - VS code
    - Text editor

* What are templates in azure functions/How functions gets triggered? 
Triggers describe the events that involke the functions. few trigger options are as below -
  - Timer Trigger
  - Service bus Queue/Topic trigger
  - Http trigger
  - Azure queue storage trigger 

* What are authrization levels in azure functions?
  - Function (Use function level keys for access)
  - Anonymous
  - Admin (Use master keys for access)

* What is the need of functions if we can create apps?
 Functions are good if we have small amount of code, then we need not to write whole apps and deploy.
 
* What are CRON expressions? What is the format of CRON expression? 
* How app-settings are mentained in azure functions?

**Durable Functions**

Durable Functions lets you define stateful workflows by writing `orchestrator functions` and stateful entities by writing `entity functions` using the Azure Functions programming model.

###### Application patterns in durable functions

**Function chaining pattern:**
In the function chaining pattern, a sequence of functions executes in a specific order. In this pattern, the output of one function is applied to the input of another function.

```
[FunctionName("Chaining")]
public static async Task<object> Run(
    [OrchestrationTrigger] IDurableOrchestrationContext context)
{
    try
    {
        var x = await context.CallActivityAsync<object>("F1", null);
        var y = await context.CallActivityAsync<object>("F2", x);
        var z = await context.CallActivityAsync<object>("F3", y);
        return  await context.CallActivityAsync<object>("F4", z);
    }
    catch (Exception)
    {
        // Error handling or compensation goes here.
    }
}
```

**Fan out/fan in Pattern:**
In the fan out/fan in pattern, you execute multiple functions in parallel and then wait for all functions to finish. Often, some aggregation work is done on the results that are returned from the functions.

```
[FunctionName("FanOutFanIn")]
public static async Task Run(
    [OrchestrationTrigger] IDurableOrchestrationContext context)
{
    var parallelTasks = new List<Task<int>>();

    // Get a list of N work items to process in parallel.
    object[] workBatch = await context.CallActivityAsync<object[]>("F1", null);
    for (int i = 0; i < workBatch.Length; i++)
    {
        Task<int> task = context.CallActivityAsync<int>("F2", workBatch[i]);
        parallelTasks.Add(task);
    }

    await Task.WhenAll(parallelTasks);

    // Aggregate all N outputs and send the result to F3.
    int sum = parallelTasks.Sum(t => t.Result);
    await context.CallActivityAsync("F3", sum);
}
```

**Async HTTP APIs Pattern:**
The async HTTP API pattern addresses the problem of coordinating the state of long-running operations with external clients. A common way to implement this pattern is by having an HTTP endpoint trigger the long-running action. Then, redirect the client to a status endpoint that the client polls to learn when the operation is finished.

**Monitor Pattern:**
The monitor pattern refers to a flexible, recurring process in a workflow. An example is polling until specific conditions are met. You can use a regular timer trigger to address a basic scenario, such as a periodic cleanup job, but its interval is static and managing instance lifetimes becomes complex. You can use Durable Functions to create flexible recurrence intervals, manage task lifetimes, and create multiple monitor processes from a single orchestration.

When a request is received, a new orchestration instance is created for that job ID. The instance polls a status until a condition is met and the loop is exited. A durable timer controls the polling interval. Then, more work can be performed, or the orchestration can end. When nextCheck exceeds expiryTime, the monitor ends.

```
[FunctionName("MonitorJobStatus")]
public static async Task Run(
    [OrchestrationTrigger] IDurableOrchestrationContext context)
{
    int jobId = context.GetInput<int>();
    int pollingInterval = GetPollingInterval();
    DateTime expiryTime = GetExpiryTime();

    while (context.CurrentUtcDateTime < expiryTime)
    {
        var jobStatus = await context.CallActivityAsync<string>("GetJobStatus", jobId);
        if (jobStatus == "Completed")
        {
            // Perform an action when a condition is met.
            await context.CallActivityAsync("SendAlert", machineId);
            break;
        }

        // Orchestration sleeps until this time.
        var nextCheck = context.CurrentUtcDateTime.AddSeconds(pollingInterval);
        await context.CreateTimer(nextCheck, CancellationToken.None);
    }

    // Perform more work here, or let the orchestration end.
}
```

**Human interaction Pattern:**
Many automated processes involve some kind of human interaction. Involving humans in an automated process is tricky because people aren't as highly available and as responsive as cloud services. An automated process might allow for this interaction by using timeouts and compensation logic.

An approval process is an example of a business process that involves human interaction. Approval from a manager might be required for an expense report that exceeds a certain dollar amount. If the manager doesn't approve the expense report within 72 hours (maybe the manager went on vacation), an escalation process kicks in to get the approval from someone else (perhaps the manager's manager).

```
[FunctionName("ApprovalWorkflow")]
public static async Task Run(
    [OrchestrationTrigger] IDurableOrchestrationContext context)
{
    await context.CallActivityAsync("RequestApproval", null);
    using (var timeoutCts = new CancellationTokenSource())
    {
        DateTime dueTime = context.CurrentUtcDateTime.AddHours(72);
        Task durableTimeout = context.CreateTimer(dueTime, timeoutCts.Token);

        Task<bool> approvalEvent = context.WaitForExternalEvent<bool>("ApprovalEvent");
        if (approvalEvent == await Task.WhenAny(approvalEvent, durableTimeout))
        {
            timeoutCts.Cancel();
            await context.CallActivityAsync("ProcessApproval", approvalEvent.Result);
        }
        else
        {
            await context.CallActivityAsync("Escalate", null);
        }
    }
}
```

To create the durable timer, call context.CreateTimer. The notification is received by context.WaitForExternalEvent. Then, Task.WhenAny is called to decide whether to escalate (timeout happens first) or process the approval (the approval is received before timeout).

An external client can deliver the event notification to a waiting orchestrator function by using the built-in HTTP APIs:
An event can also be raised using the durable orchestration client from another function in the same function app:

```
[FunctionName("RaiseEventToOrchestration")]
public static async Task Run(
    [HttpTrigger] string instanceId,
    [DurableClient] IDurableOrchestrationClient client)
{
    bool isApproved = true;
    await client.RaiseEventAsync(instanceId, "ApprovalEvent", isApproved);
}
```

**Aggregator Pattern(stateful entities):**


**References:** 
* Udemy Course Section 1 to Section 5
* https://www.youtube.com/watch?v=BhV57ka5BlU

* What are app-Insights? 
Azure Monitor application insights is an Application Performance Management (APM) service for developers and DevOps professionals. Enable it below to automatically monitor your application. It will detect performance anomalies, and includes powerful analytics tools to help you diagnose issues and to understand what users actually do with your app. 
