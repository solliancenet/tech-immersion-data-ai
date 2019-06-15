# Data & AI Tech Immersion Workshop – Product Review Guide and Lab Instructions

## Day 1, Experience 5 - Open source databases at scale

- [Data & AI Tech Immersion Workshop – Product Review Guide and Lab Instructions](#data--ai-tech-immersion-workshop-%E2%80%93-product-review-guide-and-lab-instructions)
  - [Day 1, Experience 5 - Open source databases at scale](#day-1-experience-5---open-source-databases-at-scale)
  - [Technology overview](#technology-overview)
    - [Azure Database for PostgreSQL](#azure-database-for-postgresql)
  - [Scenario overview](#scenario-overview)
  - [Experience requirements](#experience-requirements)
  - [Task 1: Connect to PostgreSQL](#task-1-connect-to-postgresql)
  - [Task 2: Create a table to store clickstream data](#task-2-create-a-table-to-store-clickstream-data)
  - [Task 3: Shard tables across nodes](#task-3-shard-tables-across-nodes)

## Technology overview

### Azure Database for PostgreSQL

Develop high-concurrency, low-latency applications with Azure Cosmos DB, a fully managed database service that supports NoSQL APIs and can scale out [multi-master](https://docs.microsoft.com/en-us/azure/cosmos-db/how-to-multi-master) workloads anywhere in the world. Ensure blazing fast performance with [industry-leading service level agreements (SLAs)](https://azure.microsoft.com/en-us/support/legal/sla/cosmos-db/v1_2/) for single-digit-millisecond reads and writes, data consistency and throughput, and 99.999% high availability. Transparent [horizontally-partitioning](https://docs.microsoft.com/en-us/azure/cosmos-db/partitioning-overview) provides elastic scaling, matching capacity with demand to controls costs and ensures your applications maintains high performance during peak traffic.

Azure Cosmos DB offers built-in, cloud-native capabilities to simplify app development and boost developer productivity, including five well-defined consistency models, [auto-indexing](https://docs.microsoft.com/en-us/azure/cosmos-db/index-policy), and multiple data models. Easily migrate existing NoSQL data with open-source APIs for [MongoDB](https://docs.microsoft.com/en-us/azure/cosmos-db/mongodb-introduction), [Cassandra](https://docs.microsoft.com/en-us/azure/cosmos-db/cassandra-introduction), Gremlin (Graph), and others. Developers can work with tools to build microservices and the languages of their choice, while enjoying seamless integration with Azure services for IoT, advanced analytics, AI and machine learning, and business intelligence.

Azure Cosmos DB enables you to innovate with IoT data to build enhanced user experiences and turn insights into action:

- Ingest and query diverse IoT data easily using Azure Cosmos DB's global presence to capture data from anywhere.
- Scale elastically to accommodate real-time fluctuations in IoT data.
- Seamlessly integrate into tools like Azure Event Hub, Azure IoT Hub and Azure Functions to ingest and stream data.

Performing real-time analytics on data of any size or type from anywhere, using a Lambda architecture, and easy integration with Azure Databricks.

- Source and serve data quickly through integration with other Azure services for real-time insights.
- Run in-depth queries over diverse data sets to understand trends and make better decisions.
- Apply Analytics, Machine Learning, and Cognitive capabilities to your NoSQL data.

By using Azure Cosmos DB you no longer have to make the extreme [tradeoffs](https://docs.microsoft.com/en-us/azure/cosmos-db/consistency-levels-tradeoffs) between consistency, availability, latency and programmability. You can choose from five well-defined consistency choices (strong, bounded staleness, consistent-prefix, session, and eventual) to better control your user’s experience through consistency, availability, latency and programmability.

Focus your time and attention on developing great apps while Azure handles management and optimization of infrastructure and databases. Deploy databases in a fraction of the time on Microsoft’s platform as a service and leverage built-in configuration options to get up and running fast. You can rest assured your apps are running on a fully managed database service built on world-class infrastructure with enterprise-grade security and compliance.

## Scenario overview

Contoso Auto is collecting vehicle telemetry and wants to use Cosmos DB to rapidly ingest and store the data in its raw form, then do some processing in near real-time. In the end, they want to create a dashboard that automatically updates with new data as it flows in after being processed. What they would like to see on the dashboard are various visualizations of detected anomalies, like engines overheating, abnormal oil pressure, and aggressive driving, using components such as a map to show anomalies related to cities, as well as various charts and graphs depicting this information in a clear way.

In this experience, you will use Azure Cosmos DB to ingest streaming vehicle telemetry data as the entry point to a near real-time analytics pipeline built on Cosmos DB, Azure Functions, Event Hubs, Azure Stream Analytics, and Power BI. To start, you will complete configuration and performance-tuning on Cosmos DB to prepare it for data ingest, and use the change feed capability of Cosmos DB to trigger Azure Functions for data processing. The function will enrich the telemetry data with location information, then send it to Event Hubs. Azure Stream Analytics extracts the enriched sensor data from Event Hubs, performs aggregations over windows of time, then sends the aggregated data to Power BI for data visualization and analysis. A vehicle telemetry data generator will be used to send vehicle telemetry data to Cosmos DB.

## Experience requirements

- Azure subscription
- [pgAdmin](https://www.pgadmin.org/download/) 4 or greater

## Task 1: Connect to PostgreSQL

1. Open the [Azure portal](https://portal.azure.com) and navigate to the resource group you created (`hands-on-lab-SUFFIX` where SUFFIX is your unique identifier).

2. Find your PostgreSQL server group and select it. (The server group name will not have a suffix. Items with names ending in, for example, "-c", "-w0", or "-w1" are not the server group.)

   ![The PostgreSQL server group is highlighted in the resource group.](media/resource-group-pg-server-group.png 'Resource group')

3. On the Overview blade, locate and copy the **Coordinator name** and **Admin username** values. Store these values in Notebook or similar text editor for later.

   ![The Coordinator name copy button and Admin username value are both highlighted.](media/postgres-coordinator-name.png 'Overview blade')

4. Select **Firewall** in the left-hand menu underneath Security. In the Firewall rules blade, select **+ Add firewall rule for current client IP address (xxx.xxx.xxx.xxx)** to add your IP to the server group's firewall.

   ![The Firewall rules blade is displayed.](media/postgres-firewall.png 'Firewall rules')

5. Select **Save** to apply the new firewall rule.

6. Launch pgAdmin. Select **Add New Server** on the home page.

   ![The pgAdmin home page is displayed with Add New Server highlighted.](media/pgadmin-home.png 'pgAdmin')

7. In the **General** tab of the Create Server dialog, enter **Lab** into the Name field.

   ![The Name field is filled out in the General tab.](media/pgadmin-create-server-general.png 'Create Server - General tab')

8. Select the **Connection** tab. Enter the following into the fields within the Connection tab:

   - **Host name/address**: paste the coordinator name value you copied earlier. For example: `<your-server-name>.postgres.database.azure.com`)
   - **Port**: 5432
   - **Maintenance database**: citus
   - **Username**: citus
   - **Password**: the administrative password (such as `Abc!1234567890`)
   - **Save password?**: check the box

   ![The previously described fields are filled in within the Connection tab.](media/pgadmin-create-server-connection.png 'Create Server - Connection tab')

9. Click the **Save** button.

10. Expand the newly added **Lab** server under the Servers tree on the pgAdmin home page. You should be able to expand the citus database.

![The pgAdmin home page is displayed and the Lab server is expanded.](media/pgadmin-home-connected.png 'pgAdmin home')

## Task 2: Create a table to store clickstream data

In this task, you will create the `events` raw table to capture every clickstream event. This table is partitioned by `event_time` since we are using it to store time series data. The script you execute to create the schema creates a partition every 5 minutes, using [pg_partman](https://www.citusdata.com/blog/2018/01/24/citus-and-pg-partman-creating-a-scalable-time-series-database-on-PostgreSQL/).

1. With the **Lab** server expanded under the Servers tree in pgAdmin, expand Databases then select **citus**. When the citus database is highlighted, select the **Query Tool** button above.

   ![The citus database is selected in pgAdmin, and the Query Tool is highlighted.](media/pgadmin-query-tool-button.png 'Query Tool')

2. Paste the following query into the Query Editor:

   ```sql
   CREATE TABLE events(
       event_id serial,
       event_time timestamptz default now(),
       customer_id bigint,
       event_type text,
       country text,
       browser text,
       device_id bigint,
       session_id bigint
   )
   PARTITION BY RANGE (event_time);

   --Create 5-minutes partitions
   SELECT partman.create_parent('public.events', 'event_time', 'native', '5 minutes');
   UPDATE partman.part_config SET infinite_time_partitions = true;

   SELECT create_distributed_table('events','customer_id');
   ```

3. Press F5 to execute the query, or select the **Execute** button on the toolbar above.

   ![The execute button is highlighted in the Query Editor.](media/pgadmin-query-editor-execute.png 'Query Editor')

4. After executing the query, verify that the new `events` table was created under the **citus** database by expanding **Schemas** -> **public** -> **Tables** in the navigation tree on the left. You may have to refresh the Schemas list by right-clicking, then selecting Refresh.

   ![The new events table is displayed in the navigation tree on the left.](media/pgadmin-events-table.png 'Events table')

## Task 3: Shard tables across nodes

In this task, you will create two rollup tables for storing aggregated data pulled from the raw events table. Later, you will create rollup functions and schedule them to run periodically.

The two tables you will create are:

- **rollup_events_5mins**: stores aggregated data in 5-minute intervals.
- **rollup_events_1hr**: stores aggregated data every 1 hour.

You will notice in the script below, as well as in the script above, that we are sharding each of the tables on `customer_id` column. The sharding logic is handled for you by the Hyperscale server group (enabled by Citus), allowing you to horizontally scale your database across multiple managed Postgres servers.

1. With the **Lab** server expanded under the Servers tree in pgAdmin, expand Databases then select **citus**. When the citus database is highlighted, select the **Query Tool** button above.

   ![The citus database is selected in pgAdmin, and the Query Tool is highlighted.](media/pgadmin-query-tool-button.png 'Query Tool')

2. Paste the following query into the Query Editor:

   ```sql
   CREATE TABLE rollup_events_5min (
        customer_id bigint,
        event_type text,
        country text,
        browser text,
        minute timestamptz,
        event_count bigint,
        device_distinct_count hll,
        session_distinct_count hll,
        top_devices_1000 jsonb
    );
    CREATE UNIQUE INDEX rollup_events_5min_unique_idx ON rollup_events_5min(customer_id,event_type,country,browser,minute);
    SELECT create_distributed_table('rollup_events_5min','customer_id');

    CREATE TABLE rollup_events_1hr (
        customer_id bigint,
        event_type text,
        country text,
        browser text,
        hour timestamptz,
        event_count bigint,
        device_distinct_count hll,
        session_distinct_count hll,
        top_devices_1000 jsonb
    );
    CREATE UNIQUE INDEX rollup_events_1hr_unique_idx ON rollup_events_1hr(customer_id,event_type,country,browser,hour);
    SELECT create_distributed_table('rollup_events_1hr','customer_id');
   ```

3. Press F5 to execute the query, or select the **Execute** button on the toolbar above.

4. After executing the query, verify that the new `rollup_events_1hr` and `rollup_events_5min` tables were created under the **citus** database by expanding **Schemas** -> **public** -> **Tables** in the navigation tree on the left. You may have to refresh the Schemas list by right-clicking, then selecting Refresh.

   ![The new rollup tables are displayed in the navigation tree on the left.](media/pgadmin-rollup-tables.png 'Rollup tables')
