# NoSQL Database Fundamentals

This document explains core NoSQL concepts for software developers: what NoSQL is, main data models, consistency and scaling trade-offs, design best practices, and common engines/use cases.

## What is NoSQL?
- NoSQL ("not only SQL") refers to database systems that depart from the traditional relational (table-based) model.
- Designed for flexible schemas, high throughput, low latency, and horizontal scalability.
- Not a single technology — a family of different data models optimized for various workloads.

## Types of NoSQL Databases
This section expands the common categories and adds other important types and characteristics.

- Document stores
  - Examples: MongoDB, Couchbase, Azure Cosmos DB (document API)
  - Data model: JSON/BSON documents (nested objects and arrays).
  - APIs: document queries, aggregation pipelines, secondary indexes.
  - Pros: flexible schema, natural mapping to object models, good for hierarchical data and fast reads.
  - Cons: document size limits, denormalization complexity for some relationships.
  - Use cases: catalogs, user profiles, content management, event-driven apps.

- Key-Value stores
  - Examples: Redis, Amazon DynamoDB (simple usage), Riak
  - Data model: simple key -> opaque value mapping; value can be blob, JSON, or structured type.
  - APIs: get/put, TTL, atomic increments, sorted sets (Redis).
  - Pros: extremely fast lookups, simple scaling, excellent for caching and session stores.
  - Cons: limited secondary query capability without additional indexing layers.
  - Use cases: caches, sessions, rate limiting, feature flags.

- Wide-Column / Column-Family stores
  - Examples: Cassandra, Scylla, HBase
  - Data model: rows keyed by a primary key with flexible column families and per-row variable columns.
  - APIs: partition-key-based queries, wide rows for time-series and event logs.
  - Pros: high write throughput, linear horizontal scalability, predictable performance at scale.
  - Cons: query model requires careful design; secondary indexes are limited.
  - Use cases: time-series, IoT telemetry, write-heavy workloads, large-scale analytics.

- Graph databases
  - Examples: Neo4j, Amazon Neptune, JanusGraph
  - Data model: nodes and relationships with properties; optimized for traversals.
  - APIs: graph query languages (Cypher, Gremlin, SPARQL).
  - Pros: expressive for connected data and deep joins/traversals.
  - Cons: scaling large graphs across clusters can be complex and may require specialized architectures.
  - Use cases: recommendations, social graphs, fraud detection, dependency analysis.

- Time-series databases
  - Examples: InfluxDB, TimescaleDB (Postgres extension), OpenTSDB
  - Data model: timestamped measurements, optimized storage and queries for time windows.
  - APIs: time-based queries, downsampling, retention policies.
  - Pros: efficient storage for high-ingest chronological data, specialized aggregations and retention.
  - Cons: not general-purpose for relational or document-like workloads.
  - Use cases: metrics, monitoring, sensor telemetry, financial ticks.

- Search / Full-Text engines
  - Examples: Elasticsearch, OpenSearch, Solr
  - Data model: inverted indexes optimized for text search and aggregations.
  - APIs: full-text queries, faceting, scoring, near-real-time indexing.
  - Pros: powerful search, analytics, and aggregations over semi-structured data.
  - Cons: eventual consistency on replication, complexity tuning relevance and scaling.
  - Use cases: site search, log analytics, ad-hoc text queries.

- Multi-model databases
  - Examples: ArangoDB, OrientDB, Azure Cosmos DB (multi-API)
  - Data model: support for two or more models (document, graph, key-value) in a single engine.
  - Pros: flexibility to use the most appropriate model without separate systems.
  - Cons: operational complexity and potential performance trade-offs.
  - Use cases: applications that need mixed workloads or gradual migrations between models.

- Specialized stores and hybrids
  - Examples: Ledger databases, object stores used as DB backends, embedded stores (LevelDB, RocksDB)
  - Note: many systems combine features (e.g., Redis modules add search/graph capabilities).

## CAP theorem (Consistency, Availability, Partition Tolerance)
- Brief history:
  - Proposed informally by Eric Brewer (2000) and formalized by Gilbert & Lynch (2002). It captures fundamental trade-offs in distributed systems.
- Formal idea (informal wording): in a distributed system that can experience network partitions, you cannot simultaneously guarantee strong consistency and full availability. Partition tolerance is mandatory for realistic distributed systems that cross machines or data centers.

Definitions:
- Consistency (C): after a successful write, all subsequent reads see that write (strongest form often referred to as linearizability). Other weaker consistency models exist (see below).
- Availability (A): every request to a non-failing node receives a response (the system remains responsive even under some failures).
- Partition Tolerance (P): the system continues to operate despite arbitrary network partitions that split nodes into disjoint groups.

Important clarifications:
- CAP applies in the presence of partitions; when there are no partitions a system may appear both consistent and available.
- Choosing C or A happens only when a network partition (or similar failure) prevents nodes from coordinating.

Consistency models (developer-facing):
- Strong consistency / Linearizability: operations appear to execute atomically at a single point in time. Good for financial or correctness-critical operations.
- Sequential consistency: operations appear in a total order but not necessarily matching real-time ordering.
- Causal consistency / Session consistency: operations that are causally related are seen in the same order; useful for per-session guarantees (read-your-writes).
- Eventual consistency: given enough time and no new writes, replicas converge to the same state. Highest availability, lowest coordination.

Quorum math (practical rule of thumb):
- For N replicas, choose write quorum W and read quorum R. If R + W > N then reads will see the latest acknowledged write (under certain assumptions).
- Example: N=3, set W=2 and R=2 -> R+W=4 > 3, so a read will intersect a write quorum and observe the latest write.
- Higher W increases write latency and reduces availability if some replicas are down; higher R increases read latency.

Leader-based vs leaderless replication:
- Leader-based (primary-secondary, consensus protocols like Raft/Paxos): one node is the leader and serializes writes; provides strong consistency when using majority commit and linearizability but may sacrifice availability if the leader is unavailable or during partitions.
- Leaderless (gossip/quorum, e.g., Dynamo-style): writes accepted on multiple replicas without a single leader, reconciliation performed later (anti-entropy, hinted handoff). This favors availability and eventual convergence.

PACELC extension:
- PACELC: "If a Partition occurs (P) choose between Availability and Consistency (A vs C); Else (E), when the system is running normally choose between Latency and Consistency (L vs C)." It highlights trade-offs even when the system is healthy (latency vs consistency).

Concrete database examples (how they expose trade-offs):
- Cassandra: typically AP with eventual consistency by default; supports tunable consistency with read/write consistency levels (ONE, QUORUM, ALL).
- Dynamo / DynamoDB: leaderless design with tunable read consistency. DynamoDB exposes eventually consistent and strongly consistent read options per request.
- MongoDB: replica sets are leader-based. WriteConcern and ReadConcern control durability and visibility; majority write concern with appropriate read preference provides stronger guarantees.
- Azure Cosmos DB: exposes multiple well-defined consistency levels (Strong, Bounded staleness, Session, Consistent Prefix, Eventual) that map to predictable behavior and latency trade-offs.

Practical developer guidance:
- Classify operations by consistency needs: strict correctness (inventory, payments) vs best-effort (analytics, feeds).
- Use strong consistency for critical operations; use eventual consistency for high-throughput, low-latency paths where stale reads are acceptable.
- Leverage tunable consistency when available to choose per-operation guarantees.
- Implement idempotent retries, optimistic concurrency control (ETags, compare-and-swap), and compensating transactions where strict transactions are not available.
- Monitor metrics that affect consistency: replication lag, write acknowledgements, and failed quorum rates.

## ACID vs BASE
- Relational databases usually target ACID (strong consistency, transactions).
- Many NoSQL systems follow BASE (Basically Available, Soft state, Eventual consistency) allowing higher throughput and distributed scalability.
- Modern NoSQL engines increasingly offer transactional guarantees (multi-document or multi-key transactions) with trade-offs.

## Schema Design Principles
- Overview: in NoSQL the schema is shaped by access patterns. Think in terms of read/write patterns, data size, update frequency, and query shapes rather than normalized tables.

- Model for your queries first:
  - Start with the API or queries your application needs and model documents to satisfy those reads with minimal multi-document joins.
  - Favor read-optimized layouts where possible; denormalization (duplicating data) is common and acceptable if updates are infrequent or idempotent.

- Embedding vs referencing:
  - Embed (store related data inside the parent document) when the relationship is one-to-few and the embedded data is typically retrieved and updated together. Embedding gives fast single-document reads and atomic updates.
  - Reference (store only identifiers and join in the application or with lookups) when the relationship is one-to-many with potentially large or unbounded child sets, or when child records grow independently.
  - Use embedding for small arrays and short-lived child lists; use references for large collections, many-to-many relationships, or independent lifecycle.

- One-to-many and many-to-many patterns:
  - One-to-few: embed array of child objects.
  - One-to-many (large): store children in their own collection with a parent id reference; consider pagination and secondary indexes on the parent id.
  - Many-to-many: model as references on both sides or use a join/association collection depending on query patterns and expected growth.

- Document size and bounded arrays:
  - Mind engine-specific document size limits (e.g., MongoDB 16 MB). Avoid unbounded arrays; use bucketing or child collections for very large lists.
  - For time-series or logs, use fixed-size buckets (e.g., hourly/daily documents) or roll-up summaries to keep document sizes predictable.

- Shard/partition-key selection:
  - Pick a shard key that provides high cardinality and distributes writes/reads evenly across nodes.
  - Avoid monotonically increasing keys (timestamps, auto-increment) as a shard key — they cause hotspots.
  - Design queries so that common access patterns include the shard key when possible (to avoid scatter/gather queries).

- Denormalization trade-offs:
  - Duplication improves read latency at the cost of more complex updates. Keep duplicated copies small and accept eventual consistency unless you use transactions.
  - Use background processes or change streams to propagate updates to denormalized copies when necessary.

- Indexing and projection considerations:
  - Add indexes to support your queries; prefer compound indexes that match query predicates and sort orders.
  - Be aware write amplification: each index increases write cost and storage usage.
  - Use projections to return only fields needed by the client and reduce network and memory usage.

- Atomicity and transaction boundaries:
  - Many document stores guarantee atomicity at the single-document level. Use that guarantee to design smaller, atomic updates.
  - Use multi-document transactions sparingly (when supported) because they introduce coordination overhead and latency.

- Time-series and TTL patterns:
  - Use TTL indexes or expiration policies for ephemeral data (sessions, caches, events) to avoid manual cleanup.
  - For high-ingest time-series, use bucketing (group records by time interval) and periodically compact or roll up older data.

- Schema validation and versioning:
  - Use schema validation where available to enforce important invariants (required fields, types).
  - Add a document schemaVersion field and write migration paths so older documents can be migrated lazily or via batch jobs.

- Migrations and evolution:
  - Design changes to be backward compatible when possible. Use dual-write or read-time transformation during migration windows.
  - Test migrations on copies and provide rollback plans.

- Concurrency and update patterns:
  - Prefer idempotent updates and optimistic concurrency control (ETags, CAS) for concurrent writers.
  - For high write contention on a single document, consider splitting the workload (sharding the logical entity) or using append-only logs with compaction.

- Security, multi-tenancy, and tenant isolation:
  - Model tenant id as part of the partition/shard key or include it in compound indexes to isolate tenant data efficiently.

- Observability and testing:
  - Validate design with realistic workloads. Use explain plans and load tests to uncover hotspots, slow queries, and index usage.
  - Monitor document growth, index size, shard balancer activity, and replica lag.

- Quick checklist when designing a schema:
  1. What are the most common read and write operations?
  2. Can those be satisfied by a single document read or require joins?
  3. Will any document grow without bound? If so, use references or bucketing.
  4. What shard key provides even distribution and supports common queries?
  5. Which fields must be indexed for performance and which increase write cost?
  6. Are updates frequent on duplicated fields? Plan for update propagation.
  7. Is a single-document atomic update sufficient or are transactions required?

## Indexing and Querying
- Create indexes on fields used in filters and sort operations.
- Mind index cardinality and write amplification: many indexes increase write cost.
- Use explain/trace plans to validate query performance.

## Transactions and Concurrency
- If the application requires multi-document strong transactions, verify the DB supports them and understand performance impact.
- For eventual consistency, design using idempotent operations and conflict resolution strategies (timestamps, vector clocks, CRDTs).

## Scaling, Replication, and Sharding

This section explains practical strategies and trade-offs for scaling NoSQL systems: read and write scaling, replication topologies, sharding strategies, rebalancing, and operational best practices.

- Goals of scaling:
  - Read scaling: serve more read throughput by adding replicas or caching.
  - Write scaling: distribute write load across nodes/partitions to increase ingestion throughput.
  - Fault tolerance: tolerate node failures without losing availability or data durability.

- Replication basics:
  - Types of replication:
    - Master/primary-secondary (leader-based): a single primary accepts writes; secondaries replicate. Simple to reason about, common in MongoDB replica sets.
    - Multi-leader / leaderless: multiple nodes accept writes (Dynamo-style). Improves availability but requires conflict resolution.
    - Consensus-based replication: uses protocols like Raft or Paxos to elect a leader and replicate using majority commit for strong guarantees (etcd, Consul).
  - Replication goals: durability, high availability, and read scaling. Choose topology based on your consistency and availability needs.

- Read scaling techniques:
  - Add read replicas that asynchronously replicate from primaries and serve reads. Watch for replica lag — reads may be stale.
  - Use caching layers (Redis, CDN) for hot data to reduce DB read load.
  - Route traffic: send stale-tolerant reads to secondaries and critical reads to primaries (or use strong read settings).

- Write scaling techniques:
  - Vertical scaling (bigger machines) has limits; prefer horizontal scaling for large write volumes.
  - Partition data (sharding) by a partition key so writes for different keys go to different nodes.
  - For write-heavy single-object workloads, consider splitting the object (shard the logical entity) or use append-only patterns to reduce contention.

- Sharding fundamentals:
  - Sharding (partitioning) splits data across nodes so each node stores a subset of the dataset, enabling write and storage scale-out.
  - Partitioning strategies:
    - Range-based: contiguous key ranges assigned to shards. Good for ordered scans but vulnerable to hotspotting on increasing keys.
    - Hash-based: apply a hash to the key and distribute evenly. Good for uniform distribution but poor for range scans.
    - Composite/compound keys: combine multiple fields (tenantId + timestamp) to achieve distribution and support targeted queries.
    - Consistent hashing & virtual nodes: used in Dynamo/Cassandra to map token ranges to nodes and make rebalancing smoother.
  - Choosing a shard key:
    - High cardinality and high entropy across your dataset.
    - Avoid monotonic keys (timestamps, auto-increment ids) unless you combine them with another attribute.
    - Consider query patterns: making common queries include shard key avoids scatter-gather across shards.

- Rebalancing and resharding:
  - Rebalancing moves partitions between nodes when adding/removing capacity. It can be online but has performance costs.
  - Strategies:
    - Online chunk migration (MongoDB): move small chunks of data while keeping the cluster available.
    - Virtual node/token approaches (Cassandra): add nodes and let consistent hashing reassign token ranges with minimal movement.
  - Operational concerns: throttled migrations, temporary increased I/O, and careful capacity planning to avoid overloading cluster during rebalancing.

- Handling cross-shard operations:
  - Cross-shard joins and transactions are expensive. Avoid them by designing queries that target a single shard.
  - If transactions are required across shards, use database-supported distributed transactions or implement application-level two-phase commit/compensating transactions. Expect higher latency and complexity.

- Conflict resolution and eventual convergence:
  - For leaderless or multi-leader systems, implement conflict resolution: last-write-wins (timestamp), vector clocks, application merge logic, or CRDTs for automatic merge.
  - Use anti-entropy mechanisms (Merkle trees, repair processes) to reconcile divergent replicas.

- Operational best practices:
  - Monitor health: replication lag, GC/pause metrics, disk I/O, CPU, and network saturation.
  - Capacity planning: size nodes based on working set (memory for indexes), storage, and expected rebalancing overhead.
  - Throttle data migrations and background compactions to avoid performance cliffs.
  - Test failure scenarios and recovery procedures (node failure, network partition, region outage).

- Examples by database type:
  - MongoDB (replica sets + sharded clusters): use replica sets for HA and sharded clusters for horizontal scale. Pick shard key carefully and use chunk balancing.
  - Cassandra/Scylla: ring architecture with consistent hashing and tunable consistency per request (QUORUM, ONE). Add nodes to scale seamlessly and rely on hinted handoff and repair for convergence.
  - DynamoDB: serverless, partitions based on primary key throughput; design keys to avoid hot partitions and use adaptive capacity.
  - Redis (cluster mode): hash slots partition keys across nodes; use replicas for read scaling and failover.

- Short tutorial: adding capacity safely (generalized steps)
  1. Analyze the workload and verify that scaling is necessary (CPU, IO, latency, queue growth).
  2. Ensure backups and snapshots are recent and tested.
  3. Add nodes to the cluster per vendor instructions (requires metadata updates and possibly token/slot assignments).
  4. Start controlled rebalancing with throttling so migrations do not saturate I/O.
  5. Monitor replication lag, request latencies, and error rates. Pause or slow migration if errors rise.
  6. Verify data distribution is even and query latencies meet objectives.
  7. Decommission any retired nodes after verifying no data loss and cluster stability.

- Troubleshooting hotspots and imbalanced load:
  - If a shard gets disproportionate load, verify shard key distribution and consider resharding with a new key or use key salting/hashed prefixes to spread traffic.
  - For read hotspots, add read replicas or cache hot keys.
  - For write hotspots on single keys, introduce additional sharding inside the logical object (fan-out writes across subkeys) or use append-only logs and background compaction.

- Summary
  - Scaling NoSQL systems requires planning for both data distribution (sharding) and redundancy (replication).
  - Choose replication and sharding strategies aligned with your consistency, latency, and operational constraints.
  - Test rebalancing and failure modes before production and continuously monitor cluster health.

## Operational Considerations
- Backups and point-in-time recovery vary by engine — plan backups and restore testing.
- Monitor latency, throughput, replica lag, disk usage, and compaction metrics.
- Capacity plan for memory and disk: many NoSQL systems rely on memory for performance (e.g., Redis, indexes in MongoDB).

## Common NoSQL Engines and Typical Use Cases
- MongoDB / Azure Cosmos DB (document): content management, catalogs, user profiles.
- DynamoDB / Redis (key-value): caching, session stores, simple lookup tables.
- Cassandra / Scylla (wide-column): high-write ingestion, IoT, time-series.
- Neo4j (graph): social networks, recommendations, dependency graphs.

## Choosing When to Use NoSQL
- Use NoSQL when you need flexible schema, massive horizontal scale, low-latency reads/writes, or specialized data models (graph).
- Prefer relational databases when you need complex transactions, strong consistency, and complex joins across normalized data.

## Summary
NoSQL databases offer a variety of models and trade-offs. Successful use requires designing around access patterns, understanding consistency guarantees, and planning for operational concerns like replication, sharding, backups, and monitoring.
