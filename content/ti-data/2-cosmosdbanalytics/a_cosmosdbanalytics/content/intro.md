# Day 1, Experience 2 - Leveraging Cosmos DB for near real-time analytics

## Technology overview

### Azure Cosmos DB

[Azure Cosmos DB](https://docs.microsoft.com/en-us/azure/cosmos-db) was built from the ground up with global distribution and horizontal scale at its core. It offers turnkey [global distribution](https://docs.microsoft.com/en-us/azure/cosmos-db/distribute-data-globally) across any number of Azure regions by transparently scaling and replicating your data wherever your users are. You can [elastically scale](https://docs.microsoft.com/en-us/azure/cosmos-db/request-units) your reads and writes all around the globe and pay only for what you need. Azure Cosmos DB provides native support for NoSQL and OSS APIs including MongoDB, Cassandra, Gremlin and SQL, offers multiple well-defined consistency models, guarantees single-digit-millisecond read and write latencies at the 99th percentile, and guarantees 99.999 high availability with multi-homing anywhere in the world - [all backed by industry-leading, comprehensive service level agreements (SLAs)](https://azure.microsoft.com/en-us/support/legal/sla/cosmos-db/v1_2/).

Azure Cosmos DB provides turnkey global data distribution, allowing you to easily build planet-scale, always-on, highly responsive, multi-homed applications without the hassle of complex, multiple-datacenter configurations. Designed as a [globally distributed database system](https://docs.microsoft.com/en-us/azure/cosmos-db/distribute-data-globally), Azure Cosmos DB allows you to write to and read from the local replicas of your Cosmos DB database, which is replicated across any number of Azure regions.

Azure Cosmos DB provides limitless and [elastic scalability](https://docs.microsoft.com/en-us/azure/cosmos-db/request-units) of reads and writes, enabling you to elastically scales reads and writes globally and pay only for the throughput and storage you need. Designed with transparent [horizontally-partitioning](https://docs.microsoft.com/en-us/azure/cosmos-db/partitioning-overview), and multi-master replication, Azure Cosmos DB offers unprecedented elastic scalability for your writes and reads, all around the globe!

You can build highly responsive, planet scale applications. With its novel [multi-master replication](https://docs.microsoft.com/en-us/azure/cosmos-db/multi-region-writers) protocol and latch-free and [write-optimized database engine](https://docs.microsoft.com/en-us/azure/cosmos-db/index-policy), Azure Cosmos DB guarantees less than 10ms latencies for both, reads and (indexed) writes at the 99th percentile, all around the world.

By using Azure Cosmos DB you no longer have to make the extreme [tradeoffs](https://docs.microsoft.com/en-us/azure/cosmos-db/consistency-levels-tradeoffs) between consistency, availability, latency and programmability. Azure Cosmos DB's [multi-master replication protocol](https://docs.microsoft.com/en-us/azure/cosmos-db/multi-region-writers) is carefully designed to offer five [well-defined consistency choices](https://docs.microsoft.com/en-us/azure/cosmos-db/consistency-levels) (strong, bounded staleness, consistent-prefix, session, and eventual) for an intuitive programming model with low latency and high availability for your globally distributed app.

Azure Cosmos DB allows you to model real-world data using key-value, graph, column-family, and document data models. You don’t have to deal with the hassle of managing schemas and secondary indexes as Azure Cosmos DB [automatically indexes all data at the time of ingestion](https://docs.microsoft.com/en-us/azure/cosmos-db/index-policy). You can also use your favorite API including SQL, [Apache® Cassandra](https://docs.microsoft.com/en-us/azure/cosmos-db/cassandra-introduction), Gremlin, Table Storage, and [Azure Cosmos DB for MongoDB](https://docs.microsoft.com/en-us/azure/cosmos-db/mongodb-introduction) API to access your data stored in your Cosmos DB database.

Azure Cosmos DB gives you enterprise-grade security and compliance, and is the first and only service to offer [industry-leading comprehensive SLAs](https://azure.microsoft.com/en-us/support/legal/sla/cosmos-db/) for 99.999% high availability, latency at the 99th percentile, guaranteed throughput and consistency.

### Azure Functions

[Azure Functions](https://docs.microsoft.com/en-us/azure/azure-functions/functions-overview) enables you to easily build the apps you need using simple, serverless functions that [scale](https://docs.microsoft.com/en-us/azure/azure-functions/functions-scale) to meet demand.

Azure Functions allows you to focus running great apps, instead of the infrastructure on which they run. You don’t need to worry about provisioning and maintaining servers. Azure Functions provides a fully managed compute platform with high reliability and security. With [scale](https://docs.microsoft.com/en-us/azure/azure-functions/functions-scale) on demand, your code gets the compute resources it needs, when it needs them, freeing you of capacity planning concerns.

Write code only for what truly matters to your business. Utilize innovative programming model for everything else such as [communicating with other services](https://docs.microsoft.com/en-us/azure/azure-functions/functions-scale), building [HTTP-based API](https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-http-webhook) or orchestrating complex workflows. Azure Functions naturally leads you to a microservices-friendly approach for building more scalable and stable applications.

You can create Functions in the [programming language of your choice](https://docs.microsoft.com/en-us/azure/azure-functions/supported-languages). Write code in an easy-to-use web-based interface or build and debug on your local machine with your favorite development tool. You can take advantage of built-in continuous deployment and use integrated monitoring tools to [troubleshoot issues](https://docs.microsoft.com/en-us/azure/app-service/overview-diagnostics).

### Azure Stream Analytics

As more and more data is generated from a variety of connected devices and sensors, transforming this data into actionable insights and predictions in near real-time is now an operational necessity. [Azure Stream Analytics](https://docs.microsoft.com/en-us/azure/stream-analytics/stream-analytics-introduction) seamlessly integrates with your real-time application architecture to enable powerful, real-time analytics on your data no matter what the volume.

Azure Stream Analytics enables you to develop massively parallel Complex Event Processing (CEP) pipelines with simplicity. It allows you to author powerful, real-time analytics solutions using very simple, declarative [SQL like language](https://docs.microsoft.com/en-us/stream-analytics-query/stream-analytics-query-language-reference) with embedded support for temporal logic. Extensive array of [out-of-the-box connectors](https://docs.microsoft.com/en-us/azure/stream-analytics/stream-analytics-define-outputs), advanced debugging and job monitoring capabilities help keep costs down by significantly lowering the developer skills required. Additionally, Azure Stream Analytics is highly extensible through support for custom code with [JavaScript User Defined functions](https://docs.microsoft.com/en-us/azure/stream-analytics/stream-analytics-javascript-user-defined-functions) further extending the streaming logic written in SQL.

Getting started in seconds is easy with Azure Stream Analytics as there is no infrastructure to worry about, and no servers, virtual machines, or clusters to manage. You can instantly [scale-out the processing power](https://docs.microsoft.com/en-us/azure/stream-analytics/stream-analytics-streaming-unit-consumption) from one to hundreds of streaming units for any job. You only pay for the processing used per job.

[Guaranteed event delivery](https://docs.microsoft.com/en-us/stream-analytics-query/event-delivery-guarantees-azure-stream-analytics) and an enterprise grade SLA, provide the three 9's of availability, making sure that Azure Stream Analytics is suitable for mission critical workloads. Automated checkpoints enable fault tolerant operation with fast restarts with no data loss.

Azure Stream Analytics can be used to allow you to quickly build real-time dashboards with Power BI for a live command and control view. [Real-time dashboards](https://docs.microsoft.com/en-us/azure/stream-analytics/stream-analytics-power-bi-dashboard) help transform live data into actionable and insightful visuals, and help you focus on what matters to you the most.

### Power BI

[Power BI](https://docs.microsoft.com/en-us/power-bi/) is a business analytics service that delivers insights to enable fast, informed decisions. Enabling you to transform data into stunning visuals and share them with colleagues on any device. Power BI provides a rich canvas on which to visually [explore and analyze your data](https://docs.microsoft.com/en-us/power-bi/service-basic-concepts). The ability to collaborate on and share customized [dashboards](https://docs.microsoft.com/en-us/azure/stream-analytics/stream-analytics-power-bi-dashboard) and interactive reports is part of the experience, enabling you to scale across your organization with built-in governance and security.

### Serverless computing using Azure Cosmos DB and Azure Functions

Serverless computing is all about the ability to focus on individual pieces of logic that are repeatable and stateless. These pieces require no infrastructure management and they consume resources only for the seconds, or milliseconds, they run for. At the core of the serverless computing movement are functions, which are made available in the Azure ecosystem by Azure Functions. To learn about other serverless execution environments in Azure see ‘serverless in Azure’ page.

With the native integration between [Azure Cosmos DB and Azure Functions](https://docs.microsoft.com/en-us/azure/cosmos-db/serverless-computing-database), you can create database triggers, input bindings and output bindings directly from your Azure Cosmos DB account.

Azure Functions and Azure Cosmos DB allow you can create, deploy and easily managed great low-latency event-driven serverless apps based on rich data, and serving a globally distributed user base seamlessly.

![Use an Azure Cosmos DB trigger to invoke an Azure Function.](media/cosmos-db-azure-function.png 'Cosmos DB and Azure Functions')

• For an example of event sourcing architectures based on Azure Cosmos DB in a real world use case see [https://blogs.msdn.microsoft.com/azurecat/2018/05/17/azure-cosmos-db-customer-profile-jet-com](https://blogs.msdn.microsoft.com/azurecat/2018/05/17/azure-cosmos-db-customer-profile-jet-com)

## Scenario overview

ContosoAuto is collecting vehicle telemetry and wants to use Cosmos DB to rapidly ingest and store the data in its raw form, then do some processing in near real-time. In the end, they want to create a dashboard that automatically updates with new data as it flows in after being processed. What they would like to see on the dashboard are various visualizations of detected anomalies, like engines overheating, abnormal oil pressure, and aggressive driving, using components such as a map to show anomalies related to cities, as well as various charts and graphs depicting this information in a clear way.

In this experience, you will use Azure Cosmos DB to ingest streaming vehicle telemetry data as the entry point to a near real-time analytics pipeline built on Cosmos DB, Azure Functions, Event Hubs, Azure Stream Analytics, and Power BI. To start, you will complete configuration and performance-tuning on Cosmos DB to prepare it for data ingest, and use the change feed capability of Cosmos DB to trigger Azure Functions for data processing. The function will enrich the telemetry data with location information, then send it to Event Hubs. Azure Stream Analytics extracts the enriched sensor data from Event Hubs, performs aggregations over windows of time, then sends the aggregated data to Power BI for data visualization and analysis. A vehicle telemetry data generator will be used to send vehicle telemetry data to Cosmos DB.