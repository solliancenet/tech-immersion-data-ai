## Wrap-up

Thank you for participating in the Leveraging Cosmos DB for near real-time analytics experience! There are many aspects of Cosmos DB that make it suitable for ingesting and serving real-time data at a global scale, some of which we have covered here today. Of course, there are other services that work alongside Cosmos DB to complete the processing pipeline.

To recap, you experienced:

- How to configure and send real-time data to Cosmos DB.
- Processing data as it is saved to Cosmos DB through the use of Azure functions, with the convenience of the Cosmos DB trigger to reduce code and automatically handle kicking off the processing logic as data arrives.
- Ingesting processed data with Event Hubs and querying and reshaping that data with Azure Stream Analytics, then sending it to Power BI for reporting.
- Rapidly creating a real-time dashboard in Power BI with interesting visualizations to view and explore vehicle anomaly data.

## Additional resources and more information

- [Introduction to Azure Cosmos DB](https://docs.microsoft.com/en-us/azure/cosmos-db/introduction)
- [Overview of the Cosmos DB change feed](https://docs.microsoft.com/en-us/azure/cosmos-db/change-feed)
- [High availability with Azure Cosmos DB](https://docs.microsoft.com/en-us/azure/cosmos-db/high-availability)
- [Scaling throughput in Azure Cosmos DB](https://docs.microsoft.com/azure/cosmos-db/scaling-throughput)
- [Partitioning and horizontal scaling](https://docs.microsoft.com/azure/cosmos-db/partition-data) in Azure Cosmos DB, plus [guide for scaling throughput](https://docs.microsoft.com/azure/cosmos-db/scaling-throughput)
- [About Event Hubs](https://docs.microsoft.com/azure/event-hubs/event-hubs-about)
- [What is Azure Stream Analytics?](https://docs.microsoft.com/en-us/azure/stream-analytics/stream-analytics-introduction)
- [Intro to Stream Analytics windowing functions](https://docs.microsoft.com/en-us/azure/stream-analytics/stream-analytics-window-functions)
- [Trigger Azure Functions from Azure Cosmos DB](https://docs.microsoft.com/en-us/azure/cosmos-db/change-feed-functions)
