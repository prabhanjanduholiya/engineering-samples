Entity framework core is light weight, cross platform OR/M implementation for data access. It enables developers to work with database using .Net objects.
It eleminates the need to write data access code.
Allows querying database using LINQ.

#### What are ORM Frameworks?
Basically, the ORM framework/software generates objects (as in OOP) that virtually map (like the map of a city) the tables in a database. Then you as a programmer, would use these objects to interact with the database. So the main idea, is to try and shield the programmer from having to write optimized SQL code – the ORM generated objects take care of that for you. So let’s take a look at a simple example:

#### Nuget packages 
The packages needed by an application depends on:
* The type of database system being used (SQL Server, SQLite, etc.)
* The EF Core features needed (`Microsoft.EntityFrameworkCore`, and `Microsoft.EntityFrameworkCore.Relational` if using a relational database provider. )

#### What's New in EF Core 6.0?
* ##### Support for SQL Server temporal tables

A system-versioned temporal table is a type of user table designed to keep a full history of data changes to allow easy point in time analysis. This type of temporal table is referred to as a system-versioned temporal table because the period of validity for each row is managed by the system (i.e. database engine).

Every temporal table has two explicitly defined columns, each with a datetime2 data type. These columns are referred to as period columns. These period columns are used exclusively by the system to record period of validity for each row whenever a row is modified.

* ##### Introduction to Migration Bundles
EF Core now includes a new way to apply these schema updates: migration bundles. A migration bundle is a small executable containing migrations and the code needed to apply these migrations to the database.
`dotnet ef migrations bundle`

* ##### Compiled models
  Compiled models can improve EF Core startup time for applications with large models. A large model typically means 100s to 1000s of entity types and relationships.
    - A new dbcontext optimize command is used to generate the compiled model.
    - 
  `dotnet ef dbcontext optimize`
  
  Now use a compiled model
  
  ```
  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder
        .UseModel(MyCompiledModels.BlogsContextModel.Instance)
        .UseSqlite(@"Data Source=test.db");
  ```


#### What's New in EF Core 5.0?

* EF Core 5.0 supports many-to-many relationships without explicitly mapping the join table. It recognizes this as a many-to-many relationship by convention, and automatically creates a PostTag join table in the database. Data can be queried and updated without explicitly referencing the join table, considerably simplifying code. The join table can still be customized and queried explicitly if needed.

* Split queries 
* Table-per-type (TPT) mapping 
* Flexible entity mapping:  Entity types are commonly mapped to tables or views such that EF Core will pull back the contents of the table or view when querying for that type. EF Core 5.0 adds additional mapping options, where an entity can be mapped to a SQL query (called a "defining query"), or to a table-valued function (TVF)
  ```
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Post>().ToSqlQuery(
        @"SELECT Id, Name, Category, BlogId FROM posts
          UNION ALL
          SELECT Id, Name, ""Legacy"", BlogId from legacy_posts");
    modelBuilder.Entity<Blog>().ToFunction("BlogsReturningFunction");
  }

* Shared-type entity types and property bags:
  EF Core 5.0 allows the same CLR type to be mapped to multiple different entity types; such types are known as shared-type entity types. While any CLR type can be used with this feature, .NET Dictionary offers a particularly compelling use-case which we call "property bags".
  
* Event counters: EF Core 5.0 exposes event counters which can be used to track your application's performance and spot various anomalies. Simply attach to a process running EF with the dotnet-counters tool.

  ```> dotnet counters monitor Microsoft.EntityFrameworkCore -p 49496
     [Microsoft.EntityFrameworkCore]
      Active DbContexts                                               1
      Execution Strategy Operation Failures (Count / 1 sec)           0
      Execution Strategy Operation Failures (Total)                   0
      Optimistic Concurrency Failures (Count / 1 sec)                 0
      Optimistic Concurrency Failures (Total)                         0
      Queries (Count / 1 sec)                                     1,755
      Queries (Total)                                            98,402
      Query Cache Hit Rate (%)                                      100
      SaveChanges (Count / 1 sec)                                     0
      SaveChanges (Total)                                             1


### DBContext
A DbContext instance represents a session with the database and can be used to query and save instances of your entities. DbContext is a combination of the Unit Of Work and Repository patterns.


#### DbContext lifetime
The lifetime of a DbContext begins when the instance is created and ends when the instance is disposed. A DbContext instance is designed to be used for a single unit-of-work. This means that the lifetime of a DbContext instance is usually very short.

A typical unit-of-work when using Entity Framework Core (EF Core) involves:

* Creation of a DbContext instance
* Tracking of entity instances by the context. Entities become tracked by
    - Being returned from a query
    - Being added or attached to the context
* Changes are made to the tracked entities as needed to implement the business rule
* SaveChanges or SaveChangesAsync is called. EF Core detects the changes made and writes them to the database.
* The DbContext instance is disposed

#### Connection Resiliency
Connection resiliency automatically retries failed database commands. The feature can be used with any database by supplying an "execution strategy", which encapsulates the logic necessary to detect failures and retry commands. 

As an example, the SQL Server provider includes an execution strategy that is specifically tailored to SQL Server (including SQL Azure). It is aware of the exception types that can be retried and has sensible defaults for maximum retries, delay between retries, etc.

##### How to configure Connection Resiliency in EF core

This is typically in the OnConfiguring method of your derived context:

```
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    optionsBuilder
        .UseSqlServer(
            @"Server=(localdb)\mssqllocaldb;Database=EFMiscellanous.ConnectionResiliency;Trusted_Connection=True",
            options => options.EnableRetryOnFailure());
}
```

In case of ASP.NET Core it can be configured in startup.cs, 

```
public void ConfigureServices(IServiceCollection services)
{
    services.AddDbContext<PicnicContext>(
        options => options.UseSqlServer(
            "<connection string>",
            providerOptions => providerOptions.EnableRetryOnFailure()));
}
```

##### Issues with retry and transactions 
if your code initiates a transaction using BeginTransaction() you are defining your own group of operations that need to be treated as a unit, and everything inside the transaction would need to be played back shall a failure occur. You will receive an exception like the following if you attempt to do this when using an execution strategy:

> InvalidOperationException: The configured execution strategy 'SqlServerRetryingExecutionStrategy' does not support user initiated transactions. Use the execution strategy > > returned by 'DbContext.Database.CreateExecutionStrategy()' to execute all the operations in the transaction as a retriable unit.

###### Solution is by manually creating a startegy: 

The solution is to manually invoke the execution strategy with a delegate representing everything that needs to be executed. If a transient failure occurs, the execution strategy will invoke the delegate again.

```
using (var db = new BloggingContext())
{
    var strategy = db.Database.CreateExecutionStrategy();

    strategy.Execute(
        () =>
        {
            using (var context = new BloggingContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    context.Blogs.Add(new Blog { Url = "http://blogs.msdn.com/dotnet" });
                    context.SaveChanges();

                    context.Blogs.Add(new Blog { Url = "http://blogs.msdn.com/visualstudio" });
                    context.SaveChanges();

                    transaction.Commit();
                }
            }
        });
}
```


## Creating a Model
#### What is Model?
#### How to configure a Model?
* Use fluent API to configure a model

```
using Microsoft.EntityFrameworkCore;

namespace EFModeling.EntityProperties.FluentAPI.Required
{
    internal class MyContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }

        #region Required
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blog>()
                .Property(b => b.Url)
                .IsRequired();
        }
        #endregion
    }

    public class Blog
    {
        public int BlogId { get; set; }
        public string Url { get; set; }
    }
}
```

* Use data annotations to configure a model


##### Shadow properties: 
Shadow properties are properties that aren't defined in your .NET entity class but are defined for that entity type in the EF Core model. The value and state of these properties is maintained purely in the Change Tracker. Shadow properties are useful when there's data in the database that shouldn't be exposed on the mapped entity types.

##### Indexer properties:
Indexer properties are entity type properties, which are backed by an indexer in .NET entity class. They can be accessed using the indexer on the .NET class instances. It also allows you to add additional properties to the entity type without changing the CLR class.


## Performance
Overall data access performance can be broken down into the following broad categories:
1. Pure database performance.
2. Network data transfer
3. Network roundtrips.
4. EF runtime overhead.

### Performance Diagnosis
This section discusses ways for detecting performance issues in your EF application, and once a problematic area has been identified, how to further analyze them to identify the root problem.

#### 1. Identifying slow database commands via logging

 ```
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
 {
     optionsBuilder
         .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Blogging;Trusted_Connection=True")
         .LogTo(Console.WriteLine, LogLevel.Information);
 }
```
Following log gets created.
```
info: 06/12/2020 09:12:36.117 RelationalEventId.CommandExecuted[20101] (Microsoft.EntityFrameworkCore.Database.Command)
      Executed DbCommand (4ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT [b].[Id], [b].[Name]
      FROM [Blogs] AS [b]
      WHERE [b].[Name] = N'foo'
```

###### Tagging LINQ queries to trace in logs

```
var myLocation = new Point(1, 2);
var nearestPeople = (from f in context.People.TagWith("This is my spatial query!")
                     orderby f.Location.Distance(myLocation) descending
                     select f).Take(5).ToList();
``` 

Following log gets created.

```
-- This is my spatial query!



SELECT TOP(@__p_1) [p].[Id], [p].[Location]
FROM [People] AS [p]
ORDER BY [p].[Location].STDistance(@__myLocation_0) DESC
```

#### 2. Inspecting query execution plans

Once you've pinpointed a problematic query that requires optimization, the next step is usually analyzing the query's execution plan. When databases receive a SQL statement, they typically produce a plan of how that plan is to be executed; this sometimes requires complicated decision-making based on which indexes have been defined, how much data exists in tables, etc. (incidentally, the plan itself should usually be cached at the server for optimal performance). Relational databases typically provide a way for users to see the query plan, along with calculated costing for different parts of the query; this is invaluable for improving your queries.


# Entity Framework Interview Questions
**What is Entity Framework?**

In .NET applications (desktop and web), ADO.NET is an old and powerful method for accessing databases and requires a lot of coding. Simply put, all of the work is done manually. Microsoft has therefore introduced Entity Framework to automate the database technique. 

Entity Framework (EF), a Microsoft-supported open-source ORM (Object-Relational Mapper) for .NET applications, is a set of utilities and mechanisms that works towards optimizing data-driven applications. This is considered one of the most important concepts in the MVC model. With the help of tables and columns, developers are able to streamline mapping between various objects in software applications. It makes use of objects of domain-specific classes but ignores the database tables and columns that are used to store the data. Designers can utilize EF in order to maintain and develop data-oriented applications with little coding and a high level of absorption when dealing with data, unlike traditional applications.   

The EF fits between the business entities (domain classes) and the database, as shown in the above figure. Additionally, it retrieves data from the database and converts it to business entity objects automatically, and also saves data stored in business entity properties. 

**1. Explain the advantages of the Entity Framework.**

Entity Framework has the following advantages:

* With its excellent prototypes, it is possible to write object-oriented programs.  
* By allowing auto-migration, it is simple to create a database or modify it.   
* It simplifies the developer's job by reducing the code length with the help of alternate commands.   
* It reduces development time, development cost, and provides auto-generated code.   

 **2. Describe some of the disadvantages of the Entity Framework.**
Entity Framework has the following disadvantages: 

If the developer does not use raw SQL codes, things can become complicated sometimes.   
* It is a slower form of the Object Relational Mapper.   
* For a big domain model, it's not ideal.  
* Some RDMS do not offer this feature.   
* EF's main drawback is its lazy loading   
* This requires a non-traditional approach to handling data that isn't available for every database.  
* Since the data migration functionality is weak, it isn't fully effective in practice. 

**3. What are the features of the Entity Framework?**

Below are some of Entity Framework's basic features:

* Cross-Platform: It is lightweight, extensible, open-source, and can be used on Windows, Linux, and Mac.
* Querying: It allows us to retrieve data from underlying databases using LINQ queries, which are then transformed into database-specific query languages.
* Modeling: EDMs (Entity Data Models) are typically created based on POCOs (Plain Old CLR Objects), which are entities with get/set properties of different types. This model is used when querying and saving entity data to the underlying database.
* Change Tracking: By using the SaveChanges method of the context, EF tracks changes to entities and their relationships and ensures the correct updates are performed on the database. Change tracking is enabled by default in EF but can be disabled by setting the AutoDetectChangesEnabled property of DbContext to false.
* Saving: Upon calling the "SaveChanges()" method, EF executes the INSERT, UPDATE, and DELETE commands to the database based on the changes made to entities. "SaveChangesAsync()" is another asynchronous method provided by EF.
* Concurrency: EF provides built-in support for Optimistic Concurrency to prevent an unknown user from overwriting data from the database.
* Transaction: EF's transaction management capabilities automate the querying and saving of data. Furthermore, you can customize the way that transactions are managed.
* Caching: First-level caching of entities is supported out of the box in the EF. Repeated queries will retrieve data from the cache rather than the database in this case.
* Built-in Conventions: EF conforms to the conventions of configuration programming and has a set of default settings that automatically configure the model.
* Configuration: By using the data annotation attribute or Fluent API, we can configure the EF model and override the default conventions.
* Migrations: EF provides migration commands that are executable on the command-line interface or NuGet Package Manager Console to incrementally update the database schema to keep it in sync with the application's data model.

**4. What are the main components of Entity Framework Architecture?**

Entity Framework Architecture consists of the following components:


* Entity Data Model (EDM): EDMs abstract logical or relational schema and expose conceptual schema of data with a three-layered model, i.e., Conceptual (C-Space), Mapping (C-S Space), and Storage (S - Space).
* LINQ to Entities (L2E): L2E is basically a query language generally used to write queries against the object model. The entities defined in the conceptual model are returned by L2E.
* Entity SQL (E-SQL): Similar to L2E, E-SQL is another query language (for EF6 only). The developer must however learn it separately since it is more difficult than L2E. Internally, E-SQL queries are translated or converted to data store-dependent SQL queries. EF is used for converting E-SQL queries to their respective datastore queries, such as T-SQL.
* Entity Client Data Provider: This layer's main task is to convert E-SQL or L2E queries into SQL queries that the database understands. In turn, the ADO.Net data provider sends and retrieves data from the database.
Net Data Provider: It uses standard ADO.NET to enable interaction with the database.
* Object Service: It is a service that facilitates access to a database, and returns data for analysis when necessary. By using it, you are able to translate data coming from entity clients into entity object structures.

**Explain different parts of the entity data model.**

The Entity Data Model consists of 3 core components that form the basis for Entity Framework. The three main components of EDM are as follows:

* Conceptual Model: It is also referred to as the Conceptual Data Definition Language Layer (C-Space). Typically, it consists of model classes (also known as entities) and their relationships.   Your database table design will not be affected by this. It makes sure that business objects and relationships are defined in XML files.
* Mapping Model: It is also referred to as the Mapping Schema Definition Language layer (C-S Space). Information about how the conceptual model is mapped to the storage model is usually included in this model. In other words, this model enables the business objects and relationships defined at the conceptual layer to be mapped to tables and relationships defined at a logical layer.
* Storage Model: It is also referred to as the Store Space Definition Language Layer (S-Space). Schematically, it represents the storage area in the backend. Therefore, the storage model is also known as a database design model that is composed of tables, keys, stored procedures, views, and related relationships.

**6. Explain what the .edmx file contains.**

First of all, a database lets you reverse engineer a model from an existing database. Entity Framework Designer is used to view and edit models stored and created in EDMX files (.edmx extensions). Using the EDMX file, you automatically generate classes that you can interact with within your application. 

EDMX files represent conceptual models, storage models, and their mappings. This file contains all the mapping information between SQL tables and objects. In addition, it also includes essential information required for rendering models graphically with ADO.NET Entity Data Designer. Furthermore, it is divided into three divisions, CSDL, MSL, and SSDL.

**What do you mean by migration? Write its type.**

Migration is a tool that was introduced in EF to update the database schema automatically when a model is modified without losing any data or other objects. Migrate Database To Latest Version is a new database initializer used by it. Entity Framework offers two types of migration:    

Automated Migration: Entity Framework 4.3 was the first to introduce automated migration so you don't have to manually migrate databases every time you alter a domain class. For example, you must also change the domain classes for each time you make a change, but with automated migration, you can simply run a command through the Package Manager Console.
Code-based Migration: When you use a code-based migration, you can configure additional aspects of the migration, like setting the default value of a column, configuring a computed column, etc.

**What are different types of Entity framework approaches?**

Three different approaches to implement Entity Framework are as follows:

* Code First Approach: The Code First approach primarily uses classes to create the model and its relations, which are then used to create a database. This way, developers can work in an object-oriented manner without considering the database structure. By following this model, developers first write POCO classes and then use these classes to create the database. Code First is the method used by most developers using Domain-Driven Design (DDD).
* Model First Approach: In contrast, the Model First approach uses ORM to build model classes and their relationships. Following the successful creation of the model classes and relationships, the physical database is created using these models.
* Database-First Approach: In Entity Framework, Database First approach is used to build entity models based on existing databases and reduce the amount of code required. By using this approach, domain and context classes can be created based on existing classes.

**Which according to you is considered the best approach in Entity Framework?**

It is impossible to define one approach as the optimal approach when using the Entity Framework. Project requirements and the type of project determine which development approach should be used. Database First is a good approach if there is a database present. Model First is the optimal choice if no database and model classes exist. As long as the domain classes are available, the Code First method is the best choice. 

**What do you mean by the term navigation property in the entity framework?**

A foreign key relationship in the database is represented by the navigation property supported by the Entity Framework. It is possible to specify relationships between entities in a database using this property type. Relationships are defined in a way as to remain coherent in object-oriented code.

**What are different entity states in EF?**
There are five possible states where an entity can exist:

* Added: It is a state in which an entity exists within the context but does not exist within the database. When the user invokes the SaveChanges method, DbContext usually generates an INSERT SQL query to insert the data into the database. Upon successful completion of the SaveChanges method, the entity's state changes to unchanged.
* Deleted: This state indicates that the entity is marked for deletion has not been removed from the database. Also, it indicates the existence of the entity in the database. When the user invokes the SaveChanges method, DbContext usually generates a DELETE SQL query to delete or remove the entity from the database. Upon successful completion of the delete operation, DbContext removes the entity.
* Modified: When the entity is modified, its state becomes Modified. Also, it indicates the existence of the entity in the database. When the user invokes the SaveChanges method, DbContext usually generates an UPDATE SQL query to update the entity from the database. Upon successful completion of the SaveChanges method, the entity's state changes to unchanged.
* Unchanged: Since the context retrieved the entity's property values from the database, the values have not changed. This entity is ignored by SaveChanges.
* Detached: This state indicates that the entity is not tracked by the DbContext. If an entity was created or retrieved outside the domain of the current instance of DbContext, then its entity state will be Detached.

**12. Write the importance of the T4 entity in Entity Framework.**

In Entity Framework code generation, T4 files are crucial. EDMX XML files are read by T4 code templates, which generate C# behind code. The generated C# behind code consists only of your entity and context classes.


**13. Explain CSDL, SSDL, and MSL sections in an Edmx file?**

* CSDL: This stands for Conceptual Schema Definition Language. Basically, it's a conceptual abstraction that is exposed to the application. In this file, you will find a description of the model object.
* SSDL: This stands for Storage Schema Definition Language. In this section, we define the mapping to our RDBMS data structure.
* MSL: This stands for Mapping Schema Language. SSDL and CSDL are connected by it. It bridges the gap between the CSDL and SSDL or maps the model and the storage.

**Explain the ways to increase the performance of EF.**

Entity Framework's performance is enhanced by following these steps:

Choose the right collection for data manipulation.
Do not put all DB objects into one entity model.
When the entity is no longer required, its tracking should be disabled and altered.
Use pre-generating Views to reduce response time for the first request.
Don't fetch all fields unless needed.
Whenever possible, avoid using Views and Contains.
Bind data to a grid or paging only by retrieving the number of records needed.
Optimize and debug LINQ queries.
Whenever possible, use compiled queries.

**15. Write some XML generation methods provided by the dataset object.**

DataSet objects provide the following methods for generating XML:  

* ReadXml(): This method reads an XML document into a DataSet object.
* GetXml(): This method returns a string containing an XML document.
* WriteXml(): This method writes XML data to disk.

**16. What do you mean by the migration history table in Entity Framework?**

EF6's Migration's history table (__MigrationHistory) is basically a database table that is used to store data about migrations applied to a database by Code First Migrations. A table like this is created when the first migration is applied to the database. Within a given database, this table contains meta-data describing the EF Code First models' schema versions. When you used the Microsoft SQL Server database, this table was considered a system table in EF5. 

**17. Explain how EF supports transactions.**

The SaveChanges() method in EF always wraps any operation involving inserting, updating, or deleting data into a transaction. Hence, you do not have to explicitly open the transaction scope.

**18. What do you mean by Deferred Execution in EF?**

Deferred Execution refers to the process of delaying the evaluation of an expression until its realized value is actually required. As a result, performance is greatly improved since unnecessary execution is avoided. Queries are deferred until the query variable or query object is iterated over a loop.

**19. Write difference between LINQ and Entity Framework.**

LINQ	Entity Framework
In order to operate, LINQ relies only on SQL Server Databases. 	In order to operate, the entity framework relies on several databases including SQL Server, Oracle, MYSQL, DB2, etc.  
It generates a .dbml to maintain the relationship.  	In this case, an .edmx file is generated first, then an .edmx file is maintained using three separate files- .csdl, .msl, and .ssdl. 
DataContext enables you to query data. 	ObjectContext, DbContext, and EntitySQL can all be used to query data.
Complex types are not supported.  	Complex types are supported.   
A database is not created from the model.	A database can be created from the model. 
Application is developed more quickly using SQL Server.	Applications are developed more quickly using SQL Server and other databases like MYSQL, Oracle, DB2, etc. 
It consists of a tightly coupled mechanism. 	It consists of a loosely coupled mechanism.  
Only one-to-one mappings are allowed. 	One-to-one, one-to-many & many-to-many mappings are allowed.  
It displays rapid development.	It takes longer to develop than LINQ, but it provides more capabilities. 

**20. Write the steps to retrieve data from database using Entity Framework in MVC.**

The following steps will show you how to retrieve data from a database in MVC (Model View Controller) using Entity Framework: 

As a first step, we must create a new project.
The next step is to add an Entity Framework reference from the NuGet package manager.
Then, a new class has to be created within the model inside the table structure.
After that, we are required to add a connection string in the web.config.connection. It should be matched with the context.
The next step is to open the Global.asax.cs class and add the new namespace of EF. We must then initialize the database.
You will now need to right-click on the Controller folder and add a new controller, followed by a model reference in the section namespace.
Last but not least, right-click on the controller's name and add the sections you want to retrieve.

**21. Explain the term dbcontext and dbset.**

DbSet: An entity set is represented by a DbSet class that can be used for creating, reading, updating, and deleting operations on it. Those DbSet type properties, which map to database tables and views, must be included in the context class (derived from DbContext).   

DbContext: It is considered an essential class in EF API that bridges the gap between an entity or domain class and the database. Communication with the database is its primary responsibility. 

**22. Difference between ADO.Net and Entity Framework.**

Below are the differences between Aadonet and Entity Framework: 

A few data layer codes are created by Ado.Net that Entity Framework doesn't create.
Entity Framework, unlike ADO.Net, generates code for intermediate layers, data access layers, and mappings automatically. This results in a reduction in development time.
On a performance basis, ADO.Net is more efficient and faster than Entity Framework.

**23. Explain the role of Pluralize and Singularize in the entity framework.**

Objects in Entity Framework are primarily assigned names using Pluralize and Singularize. This feature is available when adding a .edmx file. Entity Framework automatically assigns the Singular or Plural coding conventions when using this feature. In convention names, an additional 's' is added if there is more than one record in the object. 

**24. What is the difference between Dapper and Entity Framework?**

.NET developers are allowed to work with relational data using domain-specific objects by object-relational mappers such as Entity Framework (EF) and Dapper. Performance-wise, Dapper is the King of Micro ORMs. 

Dapper: A simple micro ORM, Dapper is considered a powerful system used for data access in the .NET world. As a means to address and open-source their issues, the Stack Overflow team created Dapper. Adding this NuGet library to your .NET project allows you to perform database operations. In terms of speed, it is the king of Micro ORMs and is almost as fast as using raw ADO.NET data readers.
Entity Framework: It is a set of .NET APIs used in software development for performing data access. It is Microsoft's official tool for accessing data.
Comparison   

According to NuGet downloads and performance, Dapper is the world's most popular Micro ORM. In contrast, Entity Framework is significantly slower than Dapper.
In comparison to other ORMs, such as the Entity Framework, Dapper does not generate as much SQL, but it does an excellent job mapping from database columns to CLR properties.
Since Dapper uses RAW SQL, it can be difficult to code, especially when multiple relationships are involved, but when a lot of data is involved and performance matters, it is worth the effort.
Since Dapper uses IDbConnection, developers can execute SQL queries to the database directly rather than put data in other objects as they do in Entity Framework.

**25. Explain POCO Classes in EF.**

POCO stands for 'Plain Old CLR Objects'. Yet, it does not mean these classes are plain or old. A POCO class is defined as a class that contains no reference to the EF Framework or the .NET Framework at all. In EF applications, Poco entities are known as available domain objects. 

POCO class is just like other normal .NET classes as these classes don't depend on any framework-specific base class, unlike the standard .NET class. Persistence-ignorant objects, or POCOs, support LINQ queries, which are supported by entities derived from the Entity Object itself. Both EF 6 and EF Core support POCO entities. 

**26. In Entity Framework, what are the ways to use stored procedures?**

This figure shows how stored procedure mapping details can be used in EDMX:


**27. Explain database concurrency and the way to handle it.**

Database concurrency in EF means that multiple users can simultaneously modify the same data in one database. Concurrency controls help safeguard data consistency in situations like these.   

Optimistic locking is usually used to handle database concurrency. We must first right-click on the EDMX designer and then change the concurrency mode to Fixed in order to implement locking. With this change, if there is a concurrency issue, we will receive a positive concurrency exception error.

**28. What are different types of loading available to load related entities in EF?**

Entity Framework offers the following types of loading: 

* Eager Loading
* Lazy Loading
* Explicit Loading

**29. What do you mean by lazy loading, eager loading and explicit loading?**

* Lazy Loading: This process delays the loading of related objects until they are needed. During lazy loading, only the objects needed by the user are returned, whereas all other related objects are only returned when needed.
* Eager Loading: This process occurs when you query for an object and all of its related objects are returned as well. Aside from that, all related objects will load with the parent object automatically. When the Include method is used, eager loading can be achieved in EF6.
* Explicit Loading: Explicit loading occurs only when lazy loading is desired, even when lazy loading is disabled. We must explicitly call the relevant load method on the related entities to process explicit loading. When the Load method is used, explicit loading can be achieved in EF6.

**30. What are the pros and cons of different types of loading?**
1. Lazy Loading

Pros

When the relationships are not too high, use Eager Loading. So you can reduce further queries on the server by using Eager Loading.
If you know that related entities will be used everywhere with the main entity, use Eager Loading.
Cons

Adding the extra lines of code to implement lazy load makes the code more complicated.
It can affect a website's search engine ranking sometimes because the unloaded content is not properly indexed.
2. Eager Loading

Pros

Upon executing the code, the system initializes or loads the resource.
Additionally, related entities that are referenced by a resource must be pre-loaded.
It is advantageous when resources need to be loaded in the background.
It saves you time by avoiding the need to execute extra SQL queries.
Cons

Since everything must be loaded to begin running, starting the application takes a longer time.
Choosing the right tool

When you know you will use related entities with your main entity everywhere, use Eager Loading.
You should use Lazy Loading whenever you have one-to-many collections.
Use lazy loading only if you are sure you won't need related entities right away.
When you are unsure about whether or not an entity will be used, use explicit loading after you have turned off Lazy Loading.

**31. Write different types of inheritance supported by Entity Framework.**

In Entity Framework, inheritance is primarily divided into three types: 

Table per Hierarchy (TPH): The TPH inheritance representation shows one table per inheritance hierarchy class. A discriminator column also aids in distinguishing between inheritance classes. This is Entity Framework's default inheritance mapping technique.
Table per Type (TPT): In this inheritance method, each domain class has its own table.
Table per Concrete Class (TPC): TPC demonstrates a single table per concrete class, but does not include the abstract class. Because of this, if an abstract class is inherited by many concrete classes, then the tables in all those concrete classes will have the same properties as that of an abstract class.

**32. Explain Complex Type in Entity Framework.**

Complex types are defined as the non-scalar properties of entity types that assist in organizing scalar properties within entities. In addition to scalar properties, complex types may also have other complex type properties. Instances of complex types are complex objects. 

**33. What do you mean by Micro ORM?**

Rather than creating database schemas, modifying database schemas, tracking changes, etc., Micro ORMs focus on working with database tables. EF 6.x and EF Core provide a full set of capabilities and features, making them ORMs. 

**34. Explain EF Data access Architecture.**

There are two types of Data Access Architecture supported by the ADO.NET Framework: 

Disconnected data access: Disconnected data access is possible with the Data Adapter object. Datasets work independently of databases, and the data can be edited.
Connected data access: A Data Reader object of a Data Provider allows you to access linked data. Data can be accessed quickly, but editing is not permitted.

**35. What do you mean by SQL injection attack?**

SQL injection is a method that hackers use to access sensitive information from an organization's database. This application-layer attack is the result of inappropriate coding in our applications, allowing hackers to inject SQL statements into your SQL code.   

The most common cause of SQL Injection is that user input fields allow SQL statements to pass through and directly query the database. ADO.NET Data Services queries are commonly affected by SQL Injection issues. 

**36. What is the best way to handle SQL injection attacks in Entity Framework?**

The injection-proof nature of Entity Framework lies in the fact that it generates parameterized SQL commands that help prevent our database from SQL injections.  

By inserting some malicious inputs into queries and parameter names, one can generate a SQL injection attack in Entity SQL syntax. It is best to never combine user inputs with Entity SQL commands text to prevent or avoid this problem.

**37. Explain the ObjectSet in EF.**

ObjectSet is generally considered as a specific type of data set that is commonly used to read, update, create, and remove operations from existing entities. Only the ObjectContext instance can be used to create it. No Entity SQL method is supported by it. 

**38. Write the namespace that is used to include .NET Data provider for SQL server in .NET code.**

NET Data Provider for SQL Server is included in .NET code by using the namespace System.Data.SqlClient. 

**39. Explain EDM and write the process to create it.**

In Entity Framework, EDM refers to the 'Entity Data Model'. It is considered as an entity-relationship prototype that assigns some basic prototypes for the data using various modeling procedures. Moreover, it is defined as a set of principles pertaining to the formation of data, regardless of how it is collected. Shortly, it's just a simple link or connection created between the database and the prototype. The steps for creating an Entity Data Model are as follows:   

Right-click on a project in the Solution Explorer.
Select the Add>New Item option from the menu.
Select the ADO.Net Entity Data Model arrangement or template.
Please enter a name and click the 'Add' button.

**40. What do you mean by DbEntityEntry Class in EF?**

An important class, DbEntityEntry helps you retrieve a variety of information about an entity. DbContext offers the Entry method for retrieving an instance of DBEntityEntry of a specific entity. 

Example: 

DbEntityEntry studentEntry = dbcontext.Entry(entity); 
You can access the entity state, as well as the current and original values of all properties of an entity using the DbEntityEntry. EntityState can be set using the DbEntityEntry, as shown below.  

context.Entry(student).State = System.Data.Entity.EntityState.Modified; 
There are mainly two types of migration in Entity Framework:

Automated Migration
Code-based Migration

**9) What are the processes used to load related entities in the Entity Framework?**

We can use any of the following processes to load related entities in the Entity Framework:


Lazy Loading: This process delays the loading of related objects until there is a requirement of them. Lazy loading only returns objects needed by the user, and all other related objects are only returned when required in the process.

Eager Loading: This process mainly takes place when we query for an object. Eager loading returns all the related objects. Additionally, all the related objects are automatically loaded with the parent object.

Explicit Loading: This process only occurs when we want to use lazy loading, even when we have already disabled lazy loading. To process explicit loading, we are required to call the relevant load method on the related entities explicitly.

**10) What are the different types of inheritance supported in Entity Framework?**

There are mainly three types of inheritance supported in Entity Framework, such as:

Table per Hierarchy (TPH): This type of inheritance depicts a single table for the entire inheritance hierarchy class. Besides, the table consists of a discriminator column that helps distinguish different inheritance classes. It is the default inheritance mapping technique in Entity Framework.

Table per Type (TPT): This type of inheritance depicts a separate or specific table for all and each domain class.

Table per Concrete Class (TPC): This type of inheritance depicts a single table for a single concrete class, but it does not include the abstract class. Therefore, if an abstract class is inherited in various concrete classes, then every concrete class table will have the same properties as the abstract class.

**11) What are the primary parts of the Entity Data Model?**

There are mainly three parts of the Entity Data Model, such as:

Storage Model
Conceptual Model
Mapping
**12) What is meant by a model in context to Entity Framework?**

A model is nothing but a class mainly used to represent the data. In context to EF, a model represents the data from a table inside the existing database.


Example: The following codes display the basic customer model:

public class Customer  
{  
  public int ID { get; set; }  
  public string Name { get; set; }  
  public DateTime JoinDate { get; set; }  
}  
**13) How will you differentiate LINQ from Entity Framework?**

We can differentiate LINQ from Entity Framework with the help of the following table:

LINQ	Entity Framework
It only operates with the help of the SQL Server Database.	It has various databases, such as SQL Server, MYSQL, Oracle, DB2, etc.
It supports one to one mapping between entity classes and the relational tables.	It supports one to one, one to many, and many to many mapping types between the entity classes and the relational tables.
It maintains a relation by creating a .dbml file.	It first creates the .edmx file. After that, it maintains a relation using three types of files: .ssdl, .msl and .csdl.
It does not support complex types.	It supports complex types.
It cannot generate a database by using the model.	It can generate a database using the model.
It enables users to query the data with DataContext.	It allows users to query the data with DbContext, ObjectContext, and EntitySQL.
It contains a tightly coupled mechanism.	It contains a loosely coupled mechanism.
It is mainly used for faster application developments with SQL Server.	It is primarily used for faster application developments using SQL Server and other databases like MYSQL, Oracle, DB2, etc.

**14) How will you define the Conceptual Model?**

The conceptual model is usually defined as the model class that consists of relationships. This type of model remains independent of the database structure.

**15) How will you explain the Storage Model?**

The storage model is usually explained as the database design model that consists of database tables, stored procs, views, and keys with relationships.

**16) What do you understand by the EDM? Also, list the process for creating EDM.**

EDM in EF is a short form of 'Entity Data Model'. It is defined as the entity-relationship prototype that helps assign basic prototypes for the data utilizing various modeling procedures. Additionally, it is referred to as a collection of core principles that define data that disregards its aggregated form. In short, it is nothing but a simple connection formed between the database and the prototype.

The following are the steps that help create an Entity Data Model:

First, we need to right-click on the project's name given in the solution explorer tab.
Next, we are required to click on 'Add a new item from the menu'.
After that, we are required to select the ADO.NET Entity Data Model arrangement or any template.
Finally, we must specify a name for the model and click on the 'ADD' button.
That is how we can create EDM in Entity Framework.

**17) Enlist all the steps required to retrieve data from the MVC database with the help of Entity Framework.**

The following are the steps required to retrieve data from the database in MVC with the help of Entity Framework:

First of all, we need to create a new project.
Next, we need to add Entity Framework reference from the NuGet package manager.
Then, we need to create a new class within the model inside the table structure.
After that, we are required to add a connection string in the web.config.connection. It should be matched with the context.
Next, we need to open the Global.asax.cs class, and implement the new namespace of EF. Then, we need to initialize the database.
We are now required to right-click on the Controller folder and add a new controller along with the model reference in section namespace.
Finally, we need to right-click on the Controller name and add the sections we want to retrieve.
That is how we can view or retrieve the data from the database in MVC using EF.

**18) What is meant by dbcontext and dbset?**

DbContext is referred to as a class in the Entity Framework API that helps create a communication between the database and the domain/entity class. In simple terms, it is a class mainly used for communicating with the database.

DbSet is also referred to as a class that helps represent an entity set for different operations, such as creating, updating, reading, and deleting. The context class in DbContext must include the DbSet type properties for all such entities that help join database tables and views.

**19) How will you define POCO classes concerning the Entity Framework?**

The term POCO is a short form of 'Plain Old CLR Objects'. However, it does not mean that the classes used here are plain or old. POCO classes are usually defined as the classes that do not include any reference specific to the EF or .NET framework. The POCO entities are referred to as the available domain objects in the Entity Framework application.

Unlike standard .NET class, POCO class of any object is independent of a framework-specific base class. POCO classes support various LINQ queries that are supported by the derived entities of the Entity Object.

**20) What are the different types of approaches used in Entity Framework?**

There are mainly three types of approaches used in Entity Framework, such as:

* Model First Approach
* Code First Approach
* Database First Approach

**21) How will you define the Code First approach and the Model First approach? Also, enlist their advantages.**

Code First Approach: In Entity Framework, the Code First approach is mainly used to create a model and its relationships using classes, which further help create a database. This enables developers to work in an object-oriented manner without thinking about the structure of the database. In this approach, the developers first write POCO classes and then create the database with the help of these POCO classes. Most developers following Domain-Driven Design (DDD) technique use the Code First approach.

Advantages of Code First Approach:

It allows developers to decide the database structure according to business objects, making it more beneficial for smaller applications.
It enables developers to decide which classes should be serialized. It also allows us to specify the collection to eager load.
Model First Approach: On the other side, the Model First approach is used to create model classes and their relationships using ORM. Once the model classes and relationships are created successfully, the physical database is created with the help of these models.

Advantages of Model First Approach:

It offers flexibility for designing the Entity Models separately and provides options to improve them in further stages.
It does not use many databases because we can create model classes by drawing them using the EDMX designer.

**22) Define the database first approach.**

In terms of the Entity Framework, the Database First approach is the method of generating an entity model from the available database. The primary motive of this approach is to lower the number of codes to be written. This approach mainly helps create domain and context classes based on the existing classes.

**23) Which, according to you, is the best approach in the Entity Framework?**

There is no particular approach that can be referred to as the best approach in Entity Framework. The selection of the development approach primarily depends on the project requirements and the project's types. If there is the database's existence, then it is good to use the Database First approach. If there is no database and the model classes, then the Model First approach is the best selection. If there is the availability of the domain classes, the Code First approach is the most suitable choice.

**24) Define the .edmx file in the context of Entity Framework.**

In the context of Entity Framework, a .edmx file is a simple XML file that may contain storage models, conceptual models, and the mapping between them. It contains all the mapping information of how object maps with SQL tables. Besides, it can include information required by ADO.NET Entity Data Designer to render a model graphically.

**25) What is meant by database concurrency and how to handle it?**

EF's database concurrency is defined as the scenario when multiple users access and modify the same data in the same database simultaneously. The systems that protect the consistency of data in such situations are known as concurrency controls.

The database concurrency is usually handled by implementing optimistic locking. To implement the locking, we must first right-click on the EDMX designer and then set the concurrency mode to Fixed. After making this change, we will get a positive concurrency exception error if there is a concurrency issue.

**26) What are the different types of properties supported in Entity Framework?**

There are mainly three types of properties supported in Entity Framework, such as:

Navigational Property
Complex Property
Scalar Property

**27) How will you define Mapping in Entity Framework?**

The mapping in Entity Framework is defined as the information that explains how the conceptual models are mapped to the storage models.

**28) What do you understand by LINQ to Entities?**

LINQ to Entities (L2E) is defined as one of the popular query languages in Entity Framework. L2E mainly helps write queries against the objects to retrieve entities based on the conceptual model's definitions.


**29) What do you understand by the Entity SQL?**

Entity SQL is an alternate query language that is similar to a LINQ to Entities. However, it is more complicated than LINQ to Entities. Developers who want to use this language will have to learn it separately.

**30) What is the role of Pluralize and Singularize in Entity Framework?**

In Entity Framework, Pluralize and Singularize are primarily responsible for assigning meaningful naming conventions to objects. We can access this feature while adding a .edmx file. While using this feature, the Entity Framework will assign to Singular or Plural coding conventions. An extra 's' is added while giving convention names if there is more than one record within the object.

**31) What are the methods used for executing plain SQL in Entity Framework?**

The following are the methods used for executing plain SQL in Entity Framework:

SqlQuery()
Database.SqlQuery()
Database.ExecuteSqlCommand()

**32) What do you understand by the SQL injection attack?**

SQL injection attacks involve attackers stealing confidential information from an organization's database. This type of attack is usually done by adding SQL commands to the existing applications' coding based on SQL statements. This is an application-layer attack, meaning front-end attack, as attackers use and modify existing code, which is improperly coded.

SQL injection attack is usually inserted using the fields that are available for user input. It occurs as these fields allow SQL statements to be pass through it and query the database directly. The SQL injection problem is a common problem with ADO.NET data service queries. Besides, Entity Framework is an injection safe because it creates parameterized SQL commands every time to secure the database from SQL injection. Also, the developers should never combine user inputs with Entity SQL command text. This will add extra layers of security.

**33) Define the navigation property in context to Entity Framework.**

In Entity Framework, the navigation property is used to represent a foreign key relationship in the database. This type of property allows users to assign specific relationships between the entities within the database. A relationship is defined in such a way that they remain coherent in the object-oriented language.

**34) What do you know about ComplexType in Entity Framework?**

In Entity Framework, ComplexType is a non-scalar property of entity types. This type of property helps users to assign scalar relationships between entities.

**35) Enlist a few O/RMs names that can be used with .NET based applications.**

The O/RMs listed below can be used with .NET based applications:

* Entity Framework 6.x
* Entity Framework Core
* N Hibernate
* Dapper

**36) How will you explain Micro O/RMs?**

A Micro ORM is designed to focus on the essential task of working with database tables rather than creating database schemas, modifying database schemas, tracking changes, etc. Entity Framework 6.x and Entity Framework core are called O/RMs because they provide a complete set of features and functionalities.

**37) What is meant by a Dapper?**

Dapper is a simple micro ORM that helps map the native query's output to a domain or any c# class. It is a powerful system used for data access in the .NET world. Dapper was developed by the StackOverflow team and released as an open-source project. It consists of a NuGet library that can be easily inserted into any.NET project based on database operations.

**38) What can we do to improve the performance of the Entity Framework?**

We can use the following ways to improve the performance of the Entity Framework:

We can use compiled queries whenever required.
We must avoid the use of Views and Contains.
We can disable and alter tracking for the entity when it is not needed.
We can Debug and Optimize the LINQ query.
If not obligatory, we must try to evade fetching all the fields.
We should retrieve only the desired number of records when binding data to the grid.

**39) What is meant by the Object Set in Entity Framework?**

Object set is a specific type of entity set that can be used to read, update, create, and remove operations from any existing entity. It can only be created by using Object Context instance. It does not support any kind of Entity SQL method.

