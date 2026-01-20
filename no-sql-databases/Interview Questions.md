# NoSQL Interview Questions — Suggested Answers

This document provides concise, practical answers to each scenario. Some questions include multiple valid approaches depending on trade-offs.

1) **Real-time chat application (millions of users)**

**Scenario:**
You are building a real-time chat application with millions of users. Messages must be delivered quickly and the system must scale horizontally.

**Question:** Which NoSQL database would you choose and why?

**Answer:**
- Primary recommendations:
  - **Redis** (Streams or Pub/Sub) for low-latency message delivery, ephemeral presence, and fast pub/sub. Use Redis Cluster for horizontal scale and persistence (AOF/RDB) for durability.
  - **Cassandra / ScyllaDB** for durable message storage with very high write throughput, linear horizontal scalability, and predictable latency. Good for chat history and fan-out workloads.
  - **DynamoDB** (or other managed key-value store) when using serverless/managed infrastructure with predictable scaling and per-request capacity control.
- Typical architecture:
  - Use Redis (or a message broker like Kafka) for real-time delivery and presence, and a durable store (Cassandra/DynamoDB) for persisted chat history.
  - Keep ephemeral state (presence, typing indicators) in an in-memory store and persist messages asynchronously to a write-optimized store.
- Why these choices:
  - Low-latency reads/writes and horizontal scale are required; Redis provides speed, Cassandra/Scylla provide durable, highly-scalable writes and predictable latencies.

2) **CAP priorities when availability is more important than strict consistency**

**Scenario:**
Your distributed system must remain available even during network partitions, and slight data inconsistency is acceptable.

**Question:** Which CAP properties would you prioritize?

**Answer:**
- Prioritize **Availability** and **Partition tolerance (AP)** over strong **Consistency**.
- Choose eventual or tunable consistency models (e.g., Cassandra, Dynamo-style) and implement conflict resolution (last-write-wins, vector clocks, or application-specific merge logic).
- Practical mitigations:
  - Use idempotent operations, client-side retries, and monotonic timestamps or vector clocks.
  - Provide session or causal guarantees when needed (session consistency in Cosmos DB).

3) **Data modeling: user profile service (reads > writes)**

**Scenario:**
You are designing a user profile service where each user has multiple addresses and preferences. Reads are more frequent than writes.

**Question:** How would you model this in NoSQL?

**Answer:**
- Embed addresses and preferences inside a single user document if the data is bounded and typically read together. This optimizes for read-heavy workloads and gives single-document atomicity for profile updates.
- Use a single document example (JSON): user document with fields: id, name, emails, addresses[], preferences object, metadata.
- If addresses can grow unbounded or have independent lifecycle/large history, store addresses in a separate collection and reference them by id (reference pattern). Use denormalization for common read paths where needed.
- Indexes: add indexes on common lookup fields (email, username) and on any fields used for filtering. Keep indexes minimal to avoid write amplification since writes are less frequent.

4) **High write throughput: millions of IoT sensor readings per second**

**Scenario:**
You are collecting millions of IoT sensor readings per second.

**Question:** Which NoSQL database is best and why?

**Answer:**
- Primary recommendations:
  - **Cassandra or ScyllaDB:** wide-column stores designed for very high write throughput, linear horizontal scaling, predictable tail latencies, and efficient time-series modeling with partition keys and bucketing.
  - **Time-series DBs** (InfluxDB, TimescaleDB) if you need specialized time-series queries, retention policies, and downsampling.
  - **DynamoDB** (managed) for serverless ingestion if you can design partition keys to avoid hot partitions and rely on provisioned or on-demand capacity.
- Why Cassandra/Scylla:
  - Append-optimized storage, tunable consistency, efficient compaction and compression, and the ability to add nodes to scale write throughput horizontally.
- Modeling tips:
  - Use partition keys that spread writes (e.g., sensorId + time-bucket), use narrow partitions, and use TTL/retention policies to expire old data.

5) **Sharding strategy for MongoDB growth**

**Scenario:**
Your MongoDB collection grows rapidly and starts affecting performance.

**Question:** How would you scale the database?

**Answer:**
- Move to a sharded cluster and choose an appropriate shard key with high cardinality and good write distribution (avoid monotonically increasing fields like timestamps or ObjectId alone).
- Consider hashed shard keys or compound keys (e.g., userId hashed or tenantId + userId) to spread load.
- Use chunk size tuning and enable the balancer; monitor chunk distribution and migrate hot chunks if necessary.
- Add replica sets per shard to provide high availability and read scaling via secondaries.
- Alternative mitigations: introduce application-level partitioning (prefix/salting), archive/roll up old data, or use time-bucketed collections for time-series workloads.

6) **Handling relationships in a social network (users, friends, followers)**

**Scenario:**
You are building a social networking application with users, friends, and followers.

**Question:** How would you handle relationships in NoSQL?

**Answer:**
- For direct/few relationships (friends list small): embed lists of friend references inside user documents for fast read.
- For large or growing relationships (followers, following): store edges in an edges collection (edge documents: {from, to, type, createdAt}) and index the from/to fields for fast lookups.
- For graph-like traversals and recommendations consider a graph database (Neo4j, Amazon Neptune) or use a multi-model DB that supports graph queries.
- Use denormalization (cached counts, top followers) and fan-out-on-write or fan-out-on-read depending on read/write patterns. For large fan-out (celebrity accounts) use a hybrid approach (push to fan-out store for most users, compute timelines on demand for high-fanout users).

7) **Outdated inventory data with performance constraints**

**Scenario:**
An e-commerce application sometimes shows outdated inventory data, but performance is critical.

**Question:** Is this acceptable? How would you handle it?

**Answer:**
- Acceptability depends on business requirements. Minor staleness may be acceptable for catalog browsing but is usually unacceptable at checkout.
- Strategies:
  - Use eventual consistency for catalog reads and strong consistency for critical operations (reserve/checkout) by reading from a strongly consistent replica or using conditional writes/transactions.
  - Use caching with short TTLs and validate inventory at the point of purchase (optimistic reservation, decrement with compare-and-swap, or transactions).
  - Implement a two-step checkout: soft-reserve inventory, then confirm with a strong-consistency write.

8) **Multiple indexes cause write performance drop**

**Scenario:**
Your queries are slow, so you add multiple indexes. Write performance drops.

**Question:** Why did this happen?

**Answer:**
- Each index must be updated on writes. More indexes => higher write amplification, increased disk I/O, and greater CPU usage during inserts/updates/deletes.
- Indexes also increase storage and can cause higher memory pressure (larger working set). Balance between read performance and write cost; create only indexes that are necessary and consider covering indexes or compound indexes that serve multiple query patterns.

9) **Node failure in a NoSQL cluster**

**Scenario:**
One node in your NoSQL cluster fails.

**Question:** How does the system stay operational?

**Answer:**
- With replication configured (replica sets or multiple replicas), other replicas take over reads/writes depending on the replication model and election/leader rules.
- Leader-based systems elect a new leader (replica set) or route writes to the current leader; read availability can be preserved by routing to secondaries if stale reads are acceptable.
- Leaderless systems (Dynamo-style) continue accepting reads/writes from remaining replicas and reconcile later.
- Operational safeguards: monitor replica lag, set proper write/read consistency levels, and ensure automatic failover and re-replication are configured.

10) **Atomic updates across multiple documents**

**Scenario:**
Your application needs atomic updates across multiple documents.

**Question:** Can NoSQL support this?

**Answer:**
- Many NoSQL systems provide single-document atomicity by default. Multi-document transactions are supported in some engines (MongoDB multi-document transactions, Cosmos DB transactions, DynamoDB transactions) but have performance and scalability costs.
- Alternatives:
  - Use database-supported transactions where available and acceptable.
  - Use application-level techniques: two-phase commit, compensating transactions, idempotent operations, or event sourcing to model multi-step operations.
  - Use careful data modeling to reduce cross-document transactions (embed when possible).

11) **Adding new fields without downtime**

**Scenario:**
You need to add new fields to existing records without downtime.

**Question:** How does NoSQL help?

**Answer:**
- Schemaless document stores allow adding new fields to documents without a schema migration or downtime; new documents include the field and old documents are read with defaults.
- Best practices:
  - Use a schemaVersion field and handle missing fields in application logic with safe defaults.
  - Perform lazy migrations (update-on-read or background batch updates) if you need to normalize data.

12) **Duplicated data in NoSQL design**

**Scenario:**
Your NoSQL design has duplicated data.

**Question:** Is this a problem?

**Answer:**
- Duplication (denormalization) is common and often intentional to optimize reads. It's not inherently a problem but introduces trade-offs:
  - Pros: faster reads, fewer joins, simpler read paths.
  - Cons: more complex updates, potential for stale data, increased storage and write amplification.
- Mitigations:
  - Keep duplicated copies small, track where authoritative data is, and use background processes, change streams, or events to propagate updates to denormalized copies.
  - Use idempotent update patterns and, where necessary, transactions to update multiple copies atomically.

---

Notes:
- These answers assume general best practices; specific choices depend on non-functional requirements (latency, cost, operational expertise, managed vs self-hosted, regulatory constraints) and exact access patterns.
- For interview settings be prepared to discuss trade-offs, example data models, shard key choices, and how you would test and monitor the system under load.