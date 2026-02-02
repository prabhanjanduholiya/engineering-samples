# Azure Cosmos DB Fundamentals — Developer & Architect Guide

This document is a compact tutorial for developers and architects working with Azure Cosmos DB. It covers core fundamentals, components, recommended architectures, operational guidance, and interview questions with concise model answers.

---

## Quick overview
- Azure Cosmos DB is Microsoft's globally distributed, multi-model database as a service (DBaaS) with turnkey global distribution, elastic throughput (Request Units - RUs), transparent multi-master replication, and five well-known APIs (Core/SQL, MongoDB, Cassandra, Gremlin, Table).
- Use Cosmos DB when you need low-latency reads/writes at global scale, flexible JSON/graph/column store models, or strong operational SLAs (99.999% availability for single-region writes, multi-region SLAs when configured).

---

## Core concepts (both roles)
- Account: Top-level Cosmos DB account resource in Azure.
- Database: Logical container for one or more containers/collections/tables/graphs.
- Container (SQL API) / Collection (Mongo) / Table / Keyspace: Physical containers that hold items (JSON documents, rows, or vertices/edges).
- Item / Document: A JSON object stored in a container. Must include an id property (or equivalent PK depending on API).
- Partition Key: Property used to partition data across physical partitions. Chosen at container creation and immutable.
- Throughput (RUs): Request Units/sec is the currency for throughput; operations consume RUs based on cost.
- Consistency Levels: Strong, Bounded staleness, Session (default), Consistent prefix, Eventual.
- Indexing: Automatic and policy-driven; can be tuned per container for performance and cost.
- Change Feed: Ordered feed of item changes for a container — useful for event-driven patterns and ETL.

---

## Developer perspective — getting started
1) Choose API: SQL/Core (document) for native Cosmos JSON features; MongoDB API if migrating apps that use Mongo drivers; Cassandra API for wide-column; Gremlin for graph; Table for Azure Table compatibility.
2) Create account & container: pick partition key and throughput model (provisioned RUs or serverless).
3) Use SDK: official SDKs available for .NET, Java, Python, JavaScript, Go.

Example (C# minimal):

// Create client and container reference
using Microsoft.Azure.Cosmos;

var client = new CosmosClient(endpointUri, primaryKey);
var database = await client.CreateDatabaseIfNotExistsAsync("mydb");
var container = await database.Database.CreateContainerIfNotExistsAsync(
    new ContainerProperties(id: "items", partitionKeyPath: "/tenantId"), throughput: 400);

// Upsert an item
var item = new { id = Guid.NewGuid().ToString(), tenantId = "t1", name = "sample", created = DateTime.UtcNow };
await container.Container.UpsertItemAsync(item, new PartitionKey("t1"));

Developer tips:
- Model for queries: design documents around access patterns and include the partition key in queries for targeted RU-efficient reads.
- Keep item size reasonable (max 2 MB item size). Denormalize where it reduces cross-partition operations.
- Use Upsert for idempotent writes; use optimistic concurrency with ETag if needed.
- Use bulk executor or SDK bulk APIs for high-throughput writes.
- Avoid cross-partition transactions for heavy workloads; when required use stored procedures or transactional batch (within same partition key).

---

## Data modeling & partitioning (developer + architect)
- Partition key selection is critical: choose a property with high cardinality, even distribution, and used frequently in queries.
- Hot partition problems occur when a small set of partition key values get disproportionate traffic; mitigate with synthetic keys (tenantId + hashSuffix) or shard further.
- Use logical partition (all items sharing partition key) for transactional batch operations.
- Patterns:
  - Aggregate/denormalize for read-heavy workloads.
  - Reference for large one-to-many or unbounded collections.
  - Time-series: use time-based synthetic keys or rolling containers with TTL.

---

## Consistency & durability
- Five consistency levels trade off latency and throughput vs staleness guarantees. Default session provides a good developer experience for most apps.
- For strongest guarantees, use Strong (only in single-region write accounts). Bounded staleness gives a predictable staleness window across regions.
- Understand how chosen consistency interacts with multi-region writes and failover behavior.

---

## Throughput, cost, and RU planning
- Estimate RU consumption using SDK diagnostics or Azure Portal Request Unit calculator. Read/write costs depend on document size, indexed properties, and query complexity.
- Provisioned throughput vs autoscale vs serverless: choose based on predictable load. Autoscale scales RU between 10% and 100% of max RU automatically.
- Indexes increase RU cost on writes. Customize index policy to exclude large or frequently-updated properties.
- Monitor RU usage and set alerts to avoid throttling (429 responses). Implement retry with backoff and transient error handling.

---

## Key components & platform features (architect)
- Physical partitions: managed storage and compute units mapped from logical partitions.
- Global distribution: add Azure regions to replicate data globally; configure failover priorities and multi-master if needed.
- Multi-master: enable multi-region writes for low-latency writes globally; requires conflict resolution strategy (last-writer-wins or custom via conflict feed).
- Replication & failover: pair regions and configure manual or automatic failover.
- Security: Azure AD integration, role-based access control (RBAC), firewall/VNet integration, managed identities, encryption at rest, and customer-managed keys (CMK).
- Server-side programming: JavaScript stored procedures, triggers, and UDFs for transactional logic within a partition.
- Change Feed: event-driven integration for CDC, ETL, and reactive architectures.

---

## Common architectures
1) Single-region primary writes with multi-region read replicas
   - Use when writes are centralized and you want low-latency reads globally.
   - Simpler consistency handling; less complex conflict management.

2) Multi-region writes (multi-master)
   - Low-latency writes worldwide. Use when active-active updates are required.
   - Plan conflict resolution: LWW for simple cases; otherwise implement application reconciliation using the conflict feed.

3) CQRS + Event-driven using Change Feed
   - Use change feed to push updates to downstream systems (search index, caches, data lake), or implement event-sourcing.
   - Combine with Functions or custom workers to process feed reliably.

4) Microservices with per-service containers
   - Each bounded context gets its own Cosmos container with tuned partitioning and throughput, allowing independent scaling.

5) Time-series & TTL containers
   - Use TTL to automatically expire old telemetry; use partitioning that supports append patterns to avoid hotspots.

---

## Operational best practices (architect)
- Monitor using Azure Monitor metrics (Total Request Units, Throttled Requests, Latency, Storage), Diagnostic logs, and SDK client diagnostics.
- Automate failover tests and runbooks for region outages.
- Use dedicated throughput for steady workloads and autoscale for spiky loads.
- Plan partition key and capacity for growth; re-creating a container to change partition key is a costly migration.
- Test indexing policies and query plans; use Query Metrics and RequestDiagnostics to optimize RU consumption.

---

## Security
- Use Azure AD roles or resource keys for access control. Prefer managed identities when possible.
- Restrict network with IP firewall rules and Private Endpoints (VNet integration).
- Use role-based access and least privilege for management plane and data plane.
- Enable encryption at rest and consider customer-managed keys for extra control.

---
 
## What types of databases are supported in Cosmos Db?
* Core Sql database
* Azure cosmos db for Mongo Db
* Cassendra database
* Azure table
* Gremlin (Graph)

## Interview questions & model answers (concise)
1) What is Azure Cosmos DB and when would you use it?
Answer: A globally distributed multi-model DBaaS with guaranteed low latency and elastic throughput; use it for globally distributed apps needing predictable performance and multi-model data.

2) How do you choose a partition key?
Answer: Pick a property with high cardinality and even traffic distribution that is present in most queries; avoid monotonically increasing values.

3) What is an RU and how do you estimate needs?
Answer: Request Unit is the currency for throughput. Estimate by measuring typical read/write costs with representative item size and query patterns and add headroom for peaks.

4) Explain Cosmos DB consistency levels.
Answer: Strong (linearizability in single-region writes), Bounded staleness (bounded lag), Session (per-session monotonic), Consistent prefix (preserves order), Eventual (lowest latency).

5) When to use multi-master (multi-region writes)?
Answer: Use when you need write locality and low-latency writes worldwide; be prepared to handle conflicts or use simple LWW conflict resolution.

6) How to avoid RU throttling?
Answer: Monitor RU consumption, provision correct throughput or autoscale, tune queries and indexing, use targeted (partition key) reads, and implement retries with exponential backoff.

7) What is change feed and use-cases?
Answer: An ordered, reliable stream of changes for a container; use for ETL, search indexing, cache invalidation, materialized views, and event-driven workflows.

8) How are stored procedures different in Cosmos DB?
Answer: They are written in JavaScript, execute transactionally within a single logical partition, and are useful for multi-item atomic operations within that partition.

9) How to design for high write throughput?
Answer: Choose a partition key that spreads writes, use bulk API, limit indexing on hot write paths, provision sufficient RU, and use multi-region writes if needed.

10) How does indexing affect cost?
Answer: Indexes accelerate reads but increase RU cost for writes. Use custom index policies to exclude large or volatile properties and use composite indexes for common queries.

11) How to handle multi-tenant data?
Answer: Options: container-per-tenant (isolation + cost), shared container with tenantId partition key (density + single endpoint), or hybrid. Prefer tenantId as partition key for per-tenant routing when isolation and scale demand it.

12) What monitoring signals are most important?
Answer: Total RUs, 429 throttles, latency, storage consumption, partition distribution, and regional replication lag.

13) How to migrate when you choose a bad partition key?
Answer: Export and re-ingest into a new container with the correct key (using change feed + backfill or bulk import); plan migration windows and traffic cutover.


---

## Quick checklist for developers
- Pick API early based on existing drivers and features.
- Model data for queries and include partition key in frequent predicates.
- Instrument and measure RU costs for representative workloads.
- Use SDK bulk and retry policies; guard against 429 responses.
- Prefer server-side stored procs/transactional batch only for operations within a single partition.

## Quick checklist for architects
- Design partition strategy and capacity plan before production.
- Decide region topology and failover approach; evaluate multi-master vs single-master trade-offs.
- Secure account with VNet/Private Endpoint and RBAC, and consider CMK for encryption needs.
- Automate monitoring, scaling, and failover tests; maintain runbooks.

---

## Further reading and tools
- Official docs: https://docs.microsoft.com/azure/cosmos-db/
- Cosmos DB capacity planner and RU calculator in Azure portal
- SDK docs: .NET / Java / Python / Node.js

---

Revision: developer + architect tutorial added covering fundamentals, components, architectures, operations, and interview Q&A.
