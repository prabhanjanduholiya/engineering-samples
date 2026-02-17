##### Function vs Stored Procedures
* Stored procedures are pre-compiled objects which are compiled for the first time and it's compiled format is saved while functions are compiled and executed everytime.
* Functions must return a value while stored procedures may or may not.
* Functions can be called within procedures while procedures can't be called inside function.
* Procedures allows to do DML operations while functions are only allowed to `Select` operations.
* Procedures can't b used in `Select statements/JOINS`
* Try catch block can't be used inside functions.
* We can't use transactions inside function. 


## JOINS
Joins allow to combine data from two or more tables. 

* Inner Join
* Left Join
* Right Join
* Full Outer join
* Cross join
* Self Join

### FULL OUTER JOIN
Returns matching rows from both tables and row from each side if no matching found.

```
SELECT column_names
FROM table1
FULL OUTER JOIN table2 
ON table1.column_name = table2.column_name
```

### CROSS JOIN
Join unrelated tables and return cartisian product of rows in joined table. 

```
SELECT column_names
FROM table1
CROSS JOIN table2
```

###### Example of CROSS JOIN
Let's suppose we have to tables one having meal info and another having soft drink info, if we want to make combinations of these then CROSS join can be used.

```
SELECT * FROM Meals 
CROSS JOIN Drinks
```

### SELF JOIN 
> Used to query historical data and compare rows within same table.

Eg. Get all employees under each manager

# Grouping data in SQL

### GROUP BY
The GROUP BY clause allows you to arrange the rows of a query in groups. The groups are determined by the columns that you specify in the GROUP BY clause.

### Having 
The HAVING clause is often used with the GROUP BY clause to filter groups based on a specified list of conditions. The following illustrates the HAVING clause syntax:
```
SELECT
    select_list
FROM
    table_name
GROUP BY
    group_list
HAVING
    conditions;
```

In this syntax, the GROUP BY clause summarizes the rows into groups and the HAVING clause applies one or more conditions to these groups. Only groups that make the conditions evaluate to TRUE are included in the result. In other words, the groups for which the condition evaluates to  FALSE or UNKNOWN are filtered out.

## Transactions in SQL Server
> Transactions group a set of tasks in single execution unit. 

### ACID Properties

#### Atomicity: 
Ensures that all operations within the work unit are successfully completed.

#### Consistency: 
Transaction should leave database in consistent state weather it is completed or failed.

#### Isolation: 
Each transaction has well defined boundry. It is isolated from another transaction. One transaction should not effect another.

#### Durability: 
Data modification that occur as a transaction should be permanent.

Note: If inner transaction gets failed, complete transaction gets rolledback.

```
BEGIN TRANSACTION 
  
  // Statements
  
COMMIT TRANSACTION
```

## Rollback

## Save Point:

# Constraints In SQL
Constraints are the rules to mentain integrity of database. 

## Data Integrity
* Entity Integrity
* Referential Integrity
* Domain Integrity

#### Entity Integrity
* Ensures that each row in table in uniquely identifiable.
* Entity integrity is achieved by using primary key constaints.

###### Primary Key Contraints
Primary key constraints enforce uniqueness of values but doesn't allows NULL.

```
CREATE TABLE table_name
(
Id Int PRIMARY KEY
)
```

#### Referential Integrity
* Ensures the integrity of relationships.
* Refrential integrity is achieved by foreign key constarints. 
      
#### Domain Integrity
* Ensures that data in tables follow defined rules for values, range and format. 

This can be achieved by - 
* Check constraints
* Unique key constraints
* Default constraints
* Not null constraints

###### CHECK CONSTRAINTS
* Check constraints contain an expressiob that evaluates when you modify or insert a row. If expression evaluates FALSE, row will not be saved to table.
* Check constraint can't reference different row in table.
* Can't reference a column in another table.

```
CREATE TABLE table_name
(
Id Int, 
Quantity Int, 
Price Money, 
CONSTRAINT CK_Price_Quantity CHECK(Quantity > 5 AND Price > 100)
)
```

##### CHECK CONSTRAINTS With No Check
* If table already contain data then we can specify `WITH NO CHECK` for existin values.

###### UNIQUE KEY CONSTRAINTS
Unique key constraints are used to ensure that no duplicate values are inserted inside a column or group of columns. Allows NULL values to insert.

```
CREATE TABLE table_name
(
Id Int UNIQUE
)
```
###### DEFAULT CONSTRAINTS
Default constraints apply a value to a column when an insert statement specify it as NULL>

```
CREATE TABLE table_name
(
Id Int, 
Address varchar(100) NULL DEFAULT ('NO ADDRESS')
)
```

###### NOT NULL CONSTRAINTS

```
CREATE TABLE table_name
(
ID Int NOT NULL,
Name VarChar(50) NULL
)
```

###### UNIQUE CONSRAINTS

###### How to DROP constraints? 

```
ALTER TABLE table_name DROP CONTRAINT constraint_name
```

## Cursors in SQL 
A database cursor is an object that enables traversal over the rows of a result set. It allows you to process individual row returned by a query.

#### Cursor Life cycle/Steps
![SQL-Server-Cursor](https://user-images.githubusercontent.com/31764786/143002468-71488acd-112f-4d93-bde2-d70789322ca5.png)

```
DECLARE 
    @product_name VARCHAR(MAX), 
    @list_price   DECIMAL;

DECLARE cursor_product CURSOR
FOR SELECT 
        product_name, 
        list_price
    FROM 
        production.products;

OPEN cursor_product;

FETCH NEXT FROM cursor_product INTO 
    @product_name, 
    @list_price;

WHILE @@FETCH_STATUS = 0
    BEGIN
        PRINT @product_name + CAST(@list_price AS varchar);
        FETCH NEXT FROM cursor_product INTO 
            @product_name, 
            @list_price;
    END;

CLOSE cursor_product;

DEALLOCATE cursor_product;

```

#### What are the limitations of a SQL Cursor
* A cursor is a memory resident set of pointers -- meaning it occupies memory from your system that may be available for other processes.
* Cursors can be faster than a while loop but they do have more overhead.
* Another factor affecting cursor speed is the number of rows and columns brought into the cursor. Time how long it takes to open your cursor and fetch statements.
* Too many columns being dragged around in memory, which are never referenced in the subsequent cursor operations, can slow things down.
* The cursors are slower because they update tables row by row. 

#### Where to use cursors
* Mostly for database administration tasks like backups, integrity checks, rebuilding indexes
* For one-time tasks when you’re sure that possible poor performance won’t impact the overall system performance
* Calling a stored procedure a few times using different parameters. In that case, you would get parameters from cursor variables and make calls inside the loop
* Calling a stored procedure or another query inside the cursor (or loop) impacts performance a lot, because, in each step of the cursor loop, you’ll run the query/procedure from the start. If you decide to do that, you should be aware of possible consequences.
* The previous hint brings us to the last bullet when you should use cursors. If you’re completely aware of how they work and you’re pretty sure it won’t impact performance, go for it

# INDEXES in SQL Server
Indexes are required to improve performance of queries.

* If we specify a primary key on a table, it will create clustered index in the table.

### Types of Indexes

##### Clustered Index
* It determines physical order of data in a table. For this reason, a table can have just one clustered index. Data is arranged in a table as per the clustered index.
* Single table can have only one clustered index
* These are faster than non-clustered indexes

##### Non-clustered Index
* Index and data are stored seperatly, the index will have pointers to the data in the table.
* As indexes are stored seprately so they can have multiple non-clustered index.
* Look is required in another table, so these are slower in perfromance
* Non-clustured indexes need extra space for storage


###### Composite Index
When an index is composed of multiple columns. It can be clustered or non clustered.

```
CREATE CLUSTERED INDEX index_name
ON table_name(col1,col2)
```

###### Advantage of having an index
* Read speed: Faster SELECT when that column is in WHERE clause

###### Disadvantages of having an index
* Space: Additional disk/memory space needed
* Write speed: Slower INSERT / UPDATE / DELETE


**How many indexes needs to be created and on which columns?**

* One index on a table is not a big deal. You automatically have an index on columns (or combinations of columns) that are primary keys or declared as unique.
* There is some overhead to an index. The index itself occupies space on disk and memory (when used). So, if space or memory are issues then too many indexes could be a problem. 
* A small number of indexes on each table are reasonable. These should be designed with the typical query load in mind. If you index every column in every table, then data modifications would slow down. 

* As a minimum I would normally recommend having at least 1 index per table, this would be automatically created on your tables primary key, for example an IDENTITY column. Then foreign keys would normally benefit from an index, this will need to be created manually.** Other columns that are frequently included in WHERE clauses should be indexed, especially if they contain lots of unique values.**


**How indexes slow down the performance?**

* When data is inserted/updated/deleted, then the index needs to be maintained as well as the original data. This slows down updates and locks the tables (or parts of the tables), which can affect query processing.

## Temp Table
## Table Variable
## Common Table Expressions (CTE)
* The CTE is preferred to use as an alternative to a Subquery/View.
* It is temporary resultset. It may be used to store result of sub-query for further use. 
* This can also create recursive query or can be used to retrieve historical data. 
* Performance is high compared to table variable

```
;With CTE1(Address, Name, Age)
AS
(
SELECT Address, Name, Age from T1
)
```

#### Advantages of CTE
* CTE improves the code readability.
* CTE provides recursive programming.
* CTE makes code maintainability easier.
* Though it provides similar functionality as a view, it will not store the definition in metadata

#### Cannot use with CTE
* The clauses like ORDER BY, INTO, OPTION clause with query hints, FOR XML, FOR BROWSE, cannot be used in the CTE query definition.
 
* "SELECT DISTINCT", GROUP BY, PIVOT, HAVING, Scalar aggregation, TOP, LEFT, RIGHT, OUTER JOIN, and Subqueries are not allowed in the CTE query definition of a recursive member.
 
* A CTE can be self-referencing and previously defined CTEs in the same WITH clause. Forward referencing is not allowed.
 
* Specifying more than one "WITH" clause in a CTE is not allowed. For example, if a CTE query definition contains a subquery then that subquery cannot contain a nested WITH clause to define other CTE. 

https://learnsql.com/blog/sql-cte-best-practices/ 

## Table Variable vs Temp Tables vs CTE

* Table variables are created in memory while temp tables are created in temp db.
* Table variables can be passed as parameters to function and stored procedures.
* Table variables can't be involved in transactions while temp tables can be involved.
* Temp tables are disposed when session connection ends.

```
DECLARE @TempStudents Table
(
RollNo INT,
// OTHER PARAMETERS
)

INSERT INTO @TempStudents
(
1, 
// Data
)
```
 
```
CREATE TABLE #TempStudents
(
RollNo INT,
// OTHER COLUMNS
)
GO
```


## Views
A view is a virtual table whose contents are defined by a query. Like a table, a view consists of a set of named columns and rows of data. Unless indexed, a view does not exist as a stored set of data values in a database. The rows and columns of data come from tables referenced in the query defining the view and are produced dynamically when the view is referenced.

A view acts as a filter on the underlying tables referenced in the view. The query that defines the view can be from one or more tables or from other views in the current or other databases. 

### Uses of views
* Views are generally used to focus, simplify, and customize the perception each user has of the database. 
* Views can be used as security mechanisms by letting users access data through the view, without granting the users permissions to directly access the underlying base tables of the view. 
* Views can be used to provide a backward compatible interface to emulate a table that used to exist but whose schema has changed. 
* Views can also be used when you copy data to and from SQL Server to improve performance and to partition data.

### Types of views beyond standard views

##### Indexed Views:
An indexed view is a view that has been materialized. This means the view definition has been computed and the resulting data stored just like a table. You index a view by creating a unique clustered index on it. Indexed views can dramatically improve the performance of some types of queries. Indexed views work best for queries that aggregate many rows. They are not well-suited for underlying data sets that are frequently updated.


###### Benefits: 
* Reduces complexity of database
* Implement row, column level security. (We can control, what data needs to be shared)
* Can be used to present aggregate data

###### Limitations:
* We can't pass parameters to view, this is required if we want to apply filters.
* Views can't be used on temp tables.
* Order by clause can't be used. 

##### Partitioned Views:
A partitioned view joins horizontally partitioned data from a set of member tables across one or more servers. This makes the data appear as if from one table. A view that joins member tables on the same instance of SQL Server is a local partitioned view.

##### System Views:
System views expose catalog metadata. You can use system views to return information about the instance of SQL Server or the objects defined in the instance. For example, you can query the sys.databases catalog view to return information about the user-defined databases available in the instance. 

#### Create view:
```
CREATE VIEW view_name
AS 
select_query_using_joins
```

#### Modify view:
`ALTER VIEW view_name`

#### Delete view:
`DROP VIEW view_name`

#### Updatable Views
* We can INSERT/UPDATE/DELETE rows from views, similar to tables. 
* If view is generated from multiple tables using JOINS then updating view may result into data corruption

```
UPDATE VIEW view_name
Set column1 = value
```

## Sub Query
A sub-query is a query within a query. It is also called an inner query or a nested query. A sub-query is usually added in a where clause of the SQL statement.
```
Select Name,Age, employeeID    
From employee    
Where employeeID in    
(   
   Select employeeID from salary where salary >=1000 /******Sub Query******/   
) 
```

# Frequently Asked Sql Queries

### Set-up Db Script

```
USE [learn.sql]
GO

/****** Object:  Table [dbo].[TblEmployee]    Script Date: 17-03-2022 13:52:50 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TblEmployee]') AND type in (N'U'))
DROP TABLE [dbo].[TblEmployee]
GO

/****** Object:  Table [dbo].[TblEmployee]    Script Date: 17-03-2022 13:52:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TblEmployee](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Salary] [int] NOT NULL,
	[Manager_Id] [int] NULL,
 CONSTRAINT [PK_TblEmployee] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


USE [learn.sql]
GO

INSERT INTO [dbo].[TblEmployee] ([Name],[Salary],[Manager_Id]) VALUES ('Prabhanjan'           ,50000           ,null)

INSERT INTO [dbo].[TblEmployee] ([Name],[Salary],[Manager_Id]) VALUES ('Duholiya'           ,11000           ,1)

INSERT INTO [dbo].[TblEmployee] ([Name],[Salary],[Manager_Id]) VALUES ('Mark'           ,9000           ,1)

INSERT INTO [dbo].[TblEmployee] ([Name],[Salary],[Manager_Id]) VALUES ('JK-Tim'           ,10000           ,2)

INSERT INTO [dbo].[TblEmployee] ([Name],[Salary],[Manager_Id]) VALUES ('Nile'           ,20000           ,1)

INSERT INTO [dbo].[TblEmployee] ([Name],[Salary],[Manager_Id]) VALUES ('Kaile'           ,10000           ,2)

INSERT INTO [dbo].[TblEmployee] ([Name],[Salary],[Manager_Id]) VALUES ('Hamre'           ,28000           ,3)

INSERT INTO [dbo].[TblEmployee] ([Name],[Salary],[Manager_Id]) VALUES ('Singh'           ,7000           ,4)

INSERT INTO [dbo].[TblEmployee] ([Name],[Salary],[Manager_Id]) VALUES ('Sloel'           ,21000           ,3)

INSERT INTO [dbo].[TblEmployee] ([Name],[Salary],[Manager_Id]) VALUES ('Den'           ,40000           ,1)

INSERT INTO [dbo].[TblEmployee] ([Name],[Salary],[Manager_Id]) VALUES ('John'           ,18000           ,4)

INSERT INTO [dbo].[TblEmployee] ([Name],[Salary],[Manager_Id]) VALUES ('Kim'           ,19000           ,4)

INSERT INTO [dbo].[TblEmployee] ([Name],[Salary],[Manager_Id]) VALUES ('Haris'           ,41000           ,2)

INSERT INTO [dbo].[TblEmployee] ([Name],[Salary],[Manager_Id]) VALUES ('Nikle'           ,20000           ,1)

INSERT INTO [dbo].[TblEmployee] ([Name],[Salary],[Manager_Id]) VALUES ('Pikols'           ,42000           ,2)

INSERT INTO [dbo].[TblEmployee] ([Name],[Salary],[Manager_Id]) VALUES ('Hamsol'           ,28000           ,2)

INSERT INTO [dbo].[TblEmployee] ([Name],[Salary],[Manager_Id]) VALUES ('RJ'           ,27000           ,4)

INSERT INTO [dbo].[TblEmployee] ([Name],[Salary],[Manager_Id]) VALUES ('Ricko'           ,21000           ,3)

INSERT INTO [dbo].[TblEmployee] ([Name],[Salary],[Manager_Id]) VALUES ('Hamsol'           ,28000           ,2)

INSERT INTO [dbo].[TblEmployee] ([Name],[Salary],[Manager_Id]) VALUES ('RJ'           ,27000           ,4)

INSERT INTO [dbo].[TblEmployee] ([Name],[Salary],[Manager_Id]) VALUES ('Ricko',21000,3)

GO

```

## Get the manager of each employee
```
--Get the manager of each employee

SELECT 
emp.Id as 'Employee Id',
emp.Name as 'Employee Name', 
mgr.Id as 'Manager Id',
mgr.Name as 'Manager Name' 
FROM [learn.sql].[dbo].[TblEmployee] as emp
JOIN [learn.sql].[dbo].[TblEmployee] as mgr 
ON emp.Manager_Id = mgr.id

```

----------------------------------------------------------------------------------  
  # SQL Server:
* Scenario based questions on Table Variables and temp tables.
* View and types of View
* transaction in sql server
* Nested Transaction Tricky question
* Transaction Save Point 
* Delete, Truncate and Drop
* Functions and Store Procedures
* Write Proc taking Top 50 and Skipping 10 in SQL.
  '''
SELECT FruitName, Price
FROM SampleFruits
ORDER BY Price
OFFSET 5 ROWS FETCH NEXT 6 ROWS ONLY
  '''
* scenario based sql queries
* CTE, temptable and table variable
* CTE in sql – questions on retrieving hierarchical data in sql
* Clustered and non – clustered indexes
*
* If there is an legacy application and there was initially hardcoded sql query used 
  and it became complex day by day with requirement change as per new clients. 
  So what would be the design approach you will follow to mitigate such problems which could arrive in future while designing new application.
* Can we use triggers for DML operations.
* As table variable has limited in-memory size and if the data exceeds in this variable during query execution then how sql identify this.
* transaction isolation levels. 
* what is phantom read?
* SQL Bulk copy
* pessimistic and Optimistic Locking 
* types of lock in sqlserver
* How to optimize SQL Query
* Ranking related query , diff b/w rank, dense_rank, row_number
  https://www.c-sharpcorner.com/article/rank-denserank-and-rownumber-functions-in-sql-server/
  
* Can we create a data type in sql
* Can we create a constraint in sql
* Merge Join
# SQL Interview Questions
## ORDER BY

#### * What is the default ordering mode in `Order By` clause?
* ASC

#### * What is `OFFSET` and `FETCH`?
`OFFSET` and `FETCH` allows to limit number of rows to be returned. These are the options of `ORDER BY` clasue. 

* `OFFSET` clause specifies number of rows to skip before starting to return rows from query.
* `FETCH` clause specifies the number of rows to be returned after the `OFFSET`
* `FETCH` clause is optional.

```
ORDER BY column_list [ASC/DESC]

OFFSET offset_rows_count {Row | Rows}

FETCH {FIRST | NEXT} fetch_row_count {Row | Rows} Only
```

##### Locking in SQL Server
##### What is difference between `UNION` and `UNION ALL`?
 `UNION` returns distinct values. 

##### What is difference between `DELETE` and `TRUNCATE`?

##### What is difference between `CHAR` and `VARCHAR`? 
##### Write SQL query to get the second highest salary among all Employees

```
SELECT TOP 1 SALARY  
FROM (  
      SELECT DISTINCT TOP 2 SALARY  
      FROM tbl_Employees  
      ORDER BY SALARY DESC  
      ) RESULT  
ORDER BY SALARY
```

##### How can we retrieve alternate records from a table?
* To get Even number records:
```
SELECT *

FROM (SELECT rownum, ID, Name

FROM Employee)

WHERE MOD(rownum,2)=0
```
* To get Odd number records:

```
SELECT *

FROM (SELECT rownum, ID, Name

FROM Employee)

WHERE MOD(rownum,2)=1
```
* What is the use of Execution plan in SQL 
https://www.sqlshack.com/execution-plans-in-sql-server/

* Write a SQL Query to find Max salary and Department name from each department?
* Query to find records in Table A not in Table B without using NOT IN.
* Write SQL Query to find employees that have same name and email
* Write a SQL Query to get the Quarter from date.
* Write Query to find employees with duplicate email.
* Query to find all Employee whose name contains the word "Rich" ignore case.
* What is the difference between DELETE and TRUNCATE in SQL?
* What is the difference between DDL and DML commands in SQL?
* Query to get Employee Name, Manager ID & number of employees in the department.
* Write SQL query to delete duplicate rows in a table?

```
DELETE FROM [SampleDB].[dbo].[Employee]
    WHERE ID NOT IN
    (
        SELECT MAX(ID) AS MaxRecordID
        FROM [SampleDB].[dbo].[Employee]
        GROUP BY [FirstName], 
                 [LastName], 
                 [Country]
    );
```
* What are ACID properties in a SQL transaction?
* What is the main difference between RANK and DENSE_RANK functions in Oracle?
* What is the use of WITH clause in SQL?
* Write a Query to get Unique names of products without using DISTINCT keyword.
* SQL query to maximum Zipcode from a table without using MAX or MIN function
* Write a query to print a comma separated list of student names in a grade.
* What is the difference between Correlated and Un-correlated Sub query?
* Print First name, Manager ID and Level of employees in Organization Structure?
* Write a query to create an empty table from an existing table?
* What is the difference between Primary key and Unique key in SQL?
* What is the difference between INNER join and OUTER join in SQL?
* Why do we use Escape characters in SQL queries?
* What is the difference between Left OUTER Join and Right OUTER Join?

[SQL Tricky Interview Questions Preparation Course _ Udemy.pdf](https://github.com/prabhanjanduholiya/wiki/files/7586167/SQL.Tricky.Interview.Questions.Preparation.Course._.Udemy.pdf)

