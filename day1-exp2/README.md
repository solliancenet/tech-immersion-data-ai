# Day 1, Experience 2 - Leveraging Cosmos DB for near real-time analytics

ContosoAuto is collecting vehicle telemetry and wants to use Cosmos DB to rapidly ingest and store the data in its raw form, then do some processing in near real-time. In the end, they want to create a dashboard that automatically updates with new data as it flows in after being processed. What they would like to see on the dashboard are various visualizations of detected anomalies, like engines overheating, abnormal oil pressure, and aggressive driving, using components such as a map to show anomalies related to cities, as well as various charts and graphs depicting this information in a clear way.

In this experience, you will use Azure Cosmos DB to ingest streaming vehicle telemetry data as the entry point to a near real-time analytics pipeline built on Cosmos DB, Azure Functions, Event Hubs, Azure Stream Analytics, and Power BI. To start, you will complete configuration and performance-tuning on Cosmos DB to prepare it for data ingest, and use the change feed capability of Cosmos DB to trigger Azure Functions for data processing. The function will enrich the telemetry data with location information, then send it to Event Hubs. Azure Stream Analytics extracts the enriched sensor data from Event Hubs, performs aggregations over windows of time, then sends the aggregated data to Power BI for data visualization and analysis. A vehicle telemetry data generator will be used to send vehicle telemetry data to Cosmos DB.

## Exercise 1: Configure Cosmos DB

In this exercise, you will create a new Cosmos DB database and collection, set the throughput units, and obtain the connection details.

1.  To start, open a new web browser window and navigate to <https://portal.azure.com>. Log in with the credentials provided to you for this lab.

2.  After logging into the Azure portal, select **Resource groups** from the left-hand menu. Then select the resource group named **tech-immersion**.

    ![The tech-immersion resource group is selected.](media/tech-immersion-rg.png 'Resource groups')

3.  Select the **Azure Cosmos DB account** from the list of resources in your resource group.

    ![The Azure Cosmos DB account is selected in the resource group.](media/tech-immersion-rg-cosmos-db.png 'tech-immersion resource group')

4.  Within the Cosmos DB account blade, select **Data Explorer** on the left-hand menu.

    ![The Data Explorer link located in the left-hand menu is highlighted.](media/cosmos-db-data-explorer-link.png 'Data Explorer link')

5.  Select **New Collection** in the top toolbar.

    ![The New Collection link in the top toolbar is highlighted.](media/cosmos-db-new-collection-link.png 'New Collection link')

6.  In the **Add Collection** blade, configure the following:

    - **Database id:** Select **Create new**, then enter "ContosoAuto" for the id.
    - **Provision database throughput:** Unchecked.
    - **Collection id:** Enter "telemetry".
    - **Partition key:** Enter "/vin".
    - **Throughput:** Enter 15000.

    > The /vin partition was selected because the data will most likely include this value, and it allows us to partition by location from which the transaction originated. This field also contains a wide range of values, which is preferable for partitions.

    ![The Add Collection form is filled out with the previously mentioned settings entered into the appropriate fields.](media/cosmos-db-new-collection.png 'Add Collection')

7.  Select **OK** on the bottom of the form when you are finished entering the values.

8.  Select **Firewall and virtual networks** from the left-hand menu, then select Allow access from **All networks**. Select **Save**. This will allow the vehicle telemetry generator application to send data to your Cosmos DB collection. Select **Save**.

    ![The All networks option is selected within the Firewall and virtual networks blade.](media/cosmos-db-firewall.png 'Firewall and virtual networks')

9.  Select **Keys** from the left-hand menu.

    ![The Keys link on the left-hand menu is highlighted.](media/cosmos-db-keys-link.png 'Keys link')

10. Copy the **Primary Connection String** value by selecting the copy button to the right of the field. **SAVE THIS VALUE** in Notepad or similar text editor for later.

    ![The Primary Connection String key is copied.](media/cosmos-db-keys.png 'Keys')
