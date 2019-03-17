# Day 1, Experience 4 - Delivering the Modern Data Warehouse with Azure Data Factory, Azure SQL Data Warehouse, Azure Databricks, and Power BI

Like many organizations, ContosoAuto generates data from numerous system, each of which has its own location and format, including structured, unstructured, and semi-structured data. They would like the ability to combine and analyze these disparate datasets in order to gain actionable insights that can help them operate their business more efficiently.

In this experience, ​​you will see how Azure Data Factory (ADF), Azure Databricks, and Azure SQL Data Warehouse (SQL DW) can be used together to build a modern data warehouse. You will start by using Azure Data Factory (ADF) to automate the movement of data in various formats gathered from various sources, including Cosmos DB, into a centralized repository, Azure Data Lake Storage Gen2 (ADLS Gen2) in this case. You will then use Azure Databricks to prepare and analyze those data, and finally write the aggregations to Azure SQL Data Warehouse (SQL DW).

As part of the process, you will also use Databricks to connect to the Cosmos DB Change Feed to stream near-real-time vehicle telemetry data directly into your SQL DW using Spark Structured Streaming.

- [Day 1, Experience 4 - Delivering the Modern Data Warehouse with Azure Data Factory, Azure SQL Data Warehouse, Azure Databricks, and Power BI](#day-1-experience-4---delivering-the-modern-data-warehouse-with-azure-data-factory-azure-sql-data-warehouse-azure-databricks-and-power-bi)
  - [Task 1: Execute ADF Pipeline to copy data](#task-1-execute-adf-pipeline-to-copy-data)
  - [Task 2: Read streaming data from Cosmos DB using Databricks](#task-2-read-streaming-data-from-cosmos-db-using-databricks)
  - [Task 3: Perform data aggregation and summarization](#task-3-perform-data-aggregation-and-summarization)
  - [Task 4: Persisting data to Databricks Delta tables](#task-4-persisting-data-to-databricks-delta-tables)
  - [Task 5: Visualizations and dashboards with Databricks](#task-5-visualizations-and-dashboards-with-databricks)
  - [Task 6: Send summarized data to Azure SQL DW](#task-6-send-summarized-data-to-azure-sql-dw)
  - [Task 7: Generate reports in Power BI with data from Azure SQL DW](#task-7-generate-reports-in-power-bi-with-data-from-azure-sql-dw)

## Task 1: Execute ADF Pipeline to copy data

In this task, you will quickly set up your ADLS Gen2 filesystem using a Databricks notebook, and then review and execute ADF pipelines to copy data from various sources, including Cosmos DB, in your ADLS Gen2 filesystem.

1. In a web browser, navigate to the [Azure portal](https://portal.azure.com), select **Resource groups** from the left-hand menu, and then select the resource group named **tech-immersion**.

   ![The tech-immersion resource group is selected.](media/tech-immersion-rg.png 'Resource groups')

2. Prior to using ADF to move data into your ADLS Gen2 instance, you must create a filesystem in ADLS Gen2. This will be done using an Azure Databricks notebook. Select **tech-immersion-databricks** from the list of resources.

   ![The Databricks resource is selected from the list of resources in the tech-immersion resource group.](media/tech-immersion-rg-databricks.png 'Tech Immersion resource group')

3. On the Azure Databricks Service blade, select **Launch Workspace**.

   ![Databricks Launch Workspace](media/tech-immersion-databricks-launch-workspace.png 'Launch Workspace')

4. In your Databricks workspace, select **Workspace** from the left-hand menu, and then select **Shared**.

5. In the shared workspace, select the **Tech-Immersion** folder, followed by the **Day 1** and **Experience 4** folders. Then select the notebook named **1-Environment-Setup**.

   ![In the shared workspace, the 1-Environment-Setup notebook is selected under the Tech-Immersion/Day-1/Experience-4 folder.](media/databricks-workspace-day1-exp4-notebook1.png 'Notebooks in the shared workspace')

6. In the **1-Environment-Setup** notebook, follow the instructions contained in the notebook, and then return here to complete the remaining steps of this task.

7. In the Azure portal, navigate to the **tech-immersion** resource group as you did in step 1 above, and then select **tech-immersion-data-factory** from the list of resources.

   ![The Data Factory resource is selected from the list of resources in the tech-immersion resource group.](media/tech-immersion-rg-data-factory.png 'Tech Immersion resource group')

8. On the Data Factory blade, select the **Author & Monitor** tile to launch the Azure Data Factory management page.

   ![The Author & Monitor tile is highlighted on the Data Factory overview blade.](media/data-factory-author-and-monitor.png 'Author & Monitor')

9. On the Azure Data Factory page, select the **Author** (pencil) icon from the left-hand menu.

   ![The Author icon is highlighted on the left-hand menu of the Azure Data Factory page.](media/data-factory-home-author.png 'Data Factory Author icon')

10. On the ADF Author page, select **Pipelines** to expand the list, and then select the **CopyData** pipeline from the list.

    ![Azure Data Factory pipelines](media/data-factory-pipelines-copydata.png 'ADF pipelines')

    > The `CopyData` pipeline consists of three copy activities. Two connect to your SQL Server 2019 instance to retrieve vehicle data from tables there. The third connects to Cosmos DB to retrieve batch vehicle telemetry data. Each of the copy activities writes data into files in ADLS Gen2.

11. On the pipeline toolbar, select **Trigger** to run the `CopyData` pipeline, and then select **Finish** on the Pipeline Run dialog. You will receive a notification that they `CopyData` pipeline is running.

    ![Trigger is highlighted in the Data Factory pipeline toolbar.](media/data-factory-pipeline-toolbar.png 'Data Factory pipeline toolbar')

12. To observe the pipeline run, select the **Monitor** icon from the left-hand menu, which will bring up a list of active and recent pipeline runs.

    ![Azure Data Factory pipeline runs](media/data-factory-monitor-pipeline-runs.png 'Azure Data Factory Monitor')

    > On the pipeline runs monitor page, you can see all active and recent pipeline runs. The **Status** field provide and indication of the state of the pipeline run, from In Progress to Failed or Canceled. You also have the option to filter by Status and set custom date ranges to get a specific status and time period.

13. Select the **Activity Runs** icon under Actions for the currently running pipeline to view the status of the individual activities which make up the pipeline.

    ![Data Factory activity runs](media/data-factory-monitor-activity-runs.png 'Data Factory activity runs')

    > The **Activity Runs** view allows you to monitor individual activities within your pipelines. In this view, you can see the amount of time each activity took to execute, as well as select the various icons under Actions to view the inputs, outputs, and details of each activity run. As with pipeline runs, you are provided with the Status of each activity.

## Task 2: Read streaming data from Cosmos DB using Databricks

You have now used ADF to move data from various sources, including Cosmos DB, into an ADLS Gen2 filesystem. In this task, you will use an Azure Databricks notebook to extend the use of Cosmos DB further. You will create a connection to your Cosmos DB instance, using the Azure Cosmos DB Spark Connector, and query streaming data from the Cosmos DB Change Feed.

1. In the [Azure portal](https://portal.azure.com), select **Resource groups**, select **Resource groups** from the left-hand menu, and then select the resource group named **tech-immersion**.

   ![The tech-immersion resource group is selected.](media/tech-immersion-rg.png 'Resource groups')

2. Select **tech-immersion-databricks** from the list of resources.

   ![The Databricks resource is selected from the list of resources in the tech-immersion resource group.](media/tech-immersion-rg-databricks.png 'Tech Immersion resource group')

3. On the Azure Databricks Service blade, select **Launch Workspace**.

   ![Databricks Launch Workspace](media/tech-immersion-databricks-launch-workspace.png 'Launch Workspace')

4. In your Databricks workspace, select **Workspace** from the left-hand menu, and then select **Shared**.

5. In the shared workspace, select the **Tech-Immersion** folder, followed by the **Day 1** and **Experience 4** folders. Then select the notebook named **2-Cosmos-DB-Change-Feed**.

   ![In the shared workspace, the 2-Cosmos-DB-Change-Feed notebook is selected under the Tech-Immersion/Day-1/Experience-4 folder.](media/databricks-workspace-day1-exp4-notebook2.png 'Notebooks in the shared workspace')

6. In the **2-Cosmos-DB-Change-Feed** notebook, follow the instructions to complete the remaining steps of this task.

> The notebook contains all the instructions needed to complete this task. In addition, the final cell of the notebook contains instructions on the next step, which will include a link to the notebook for the next task in this experience, or instructions to return to this document.

## Task 3: Perform data aggregation and summarization

In this task, you will using Databricks to perform data preparation, aggregation and summarization with both batch and streaming data.

1. In your Databricks workspace, select **Workspace** from the left-hand menu, then select **Shared**.

2. In the shared workspace, select the **Tech-Immersion** folder, followed by the **Day 1** and **Experience 4** folders. Then select the notebook named **3-Aggregation-and-Summarization**.

   ![In the shared workspace, the 3-Aggregation-and-Summarization notebook is selected under the Tech-Immersion/Day-1/Experience-4 folder.](media/databricks-workspace-day1-exp4-notebook3.png 'Notebooks in the shared workspace')

3. In the **3-Aggregation-and-Summarization** notebook, follow the instructions to complete the remaining steps of this task.

> The notebook contains all the instructions needed to complete this task. In addition, the final cell of the notebook contains instructions on the next step, which will include a link to the notebook for the next task in this experience, or instructions to return to this document.

## Task 4: Persisting data to Databricks Delta tables

In this task, you will see how Databricks Delta provides capabilities previous unavailable for updating records in an Hive table by using the UPSERT method to update existing records and insert new records.

1. In your Databricks workspace, select **Workspace** from the left-hand menu, then select **Shared**.

2. In the shared workspace, select the **Tech-Immersion** folder, followed by the **Day 1** and **Experience 4** folders. Then select the notebook named **4-Databricks-Delta**.

   ![In the shared workspace, the 4-Databricks-Delta notebook is selected under the Tech-Immersion/Day-1/Experience-4 folder.](media/databricks-workspace-day1-exp4-notebook4.png 'Notebooks in the shared workspace')

3. In the **4-Databricks-Delta** notebook, follow the instructions to complete the remaining steps of this task.

> The notebook contains all the instructions needed to complete this task. In addition, the final cell of the notebook contains instructions on the next step, which will include a link to the notebook for the next task in this experience, or instructions to return to this document.

## Task 5: Visualizations and dashboards with Databricks

In this task, you will use visualizations configured within a Databricks notebook to build a dashboard displaying your data aggregations.

1. In your Databricks workspace, select **Workspace** from the left-hand menu, then select **Shared**.

2. In the shared workspace, select the **Tech-Immersion** folder, followed by the **Day 1** and **Experience 4** folders. Then select the notebook named **5-Databricks-Dashboards**.

   ![In the shared workspace, the 5-Databricks-Dashboards notebook is selected under the Tech-Immersion/Day-1/Experience-4 folder.](media/databricks-workspace-day1-exp4-notebook5.png 'Notebooks in the shared workspace')

3. In the **5-Databricks-Dashboards** notebook, follow the instructions to complete the remaining steps of this task.

> The notebook contains all the instructions needed to complete this task. In addition, the final cell of the notebook contains instructions on the next step, which will include a link to the notebook for the next task in this experience, or instructions to return to this document.

## Task 6: Send summarized data to Azure SQL DW

In this task, you will use the Azure SQL Data Warehouse connector to write aggregated data from Databricks into your SQL DW. You will also apply aggregations to streaming data from the Cosmos DB Change Feed, and stream the data directly into your Azure SQL DW from Databricks.

1. In your Databricks workspace, select **Workspace** from the left-hand menu, then select **Shared**.

2. In the shared workspace, select the **Tech-Immersion** folder, followed by the **Day 1** and **Experience 4** folders. Then select the notebook named **6-Write-to-SQL-DW**.

   ![In the shared workspace, the 6-Write-to-SQL-DW notebook is selected under the Tech-Immersion/Day-1/Experience-4 folder.](media/databricks-workspace-day1-exp4-notebook6.png 'Notebooks in the shared workspace')

3. In the **6-Write-to-SQL-DW** notebook, follow the instructions to complete the remaining steps of this task.

> The notebook contains all the instructions needed to complete this task. In addition, the final cell of the notebook contains instructions on the next step, which will include a link to the notebook for the next task in this experience, or instructions to return to this document.

## Task 7: Generate reports in Power BI with data from Azure SQL DW

In this task, you will use Power BI Desktop to read data from Azure SQL DW to create reports showing vehicle telemetry data.

1. Launch Power BI Desktop, and select **Get data** on the splash screen.

   ![The Power BI Desktop splash screen is shown with the Get data link highlighted.](media/power-bi-desktop.png 'Power BI Desktop splash screen')

2. On the Get Data dialog, select **Azure** on the left-hand side, select **Azure SQL Data Warehouse** from the list of available Azure services, and then select **Connect**.

   ![The Power BI Get Data dialog is displayed, with Azure selected on the left and Azure SQL Data Warehouse selected on the right. The Connect button is highlighted.](media/power-bi-get-data-sql-dw.png 'Power BI Get Data')

3. On the SQL Server database dialog that appears, enter the following:

   - **Server**: Copy and paste the name of your SQL DW Server from the Azure portal.

     - In the Azure portal, navigate to the tech-immersion resource group, and select your SQL data warehouse resource.

       ![The SQL data warehouse resource is highlighted in the tech-immersion resource group.](media/resources-group-sql-dw.png 'Tech Immersion Resource Group')

     - On the SQL DW overview blade, copy the Server name.

       ![The Server name is highlighted on the SQL DW overview blade.](media/sql-dw-server-name.png 'SQL Server Data Warehouse')

   - **Database**: Enter tech-immersion-sql-dw.
   - **Data Connectivity mode**: Select DirectQuery.

   ![The Power BI SQL Server database connection dialog is displayed. The tech-immersion-sql-dw server name is entered into the Server box, and tech-immersion-sql-dw is entered into the Database field. DirectQuery is selected for the Data Connectivity mode.](media/power-bi-sql-server-database.png 'Power BI SQL Server database connection')

4. On the next dialog, select **Database** on the left-hand side, enter **ti-admin** as the User name and **Password.1!!** as the Password, and then select **Connect**.

   ![In the SQL Server Database dialog, Database is selected and the credentials for the ti-admin account are entered into the user name and password fields.](media/power-bi-sql-dw-credentials.png 'SQL DW Login')

5. After signing in, select the **StreamData** and **VehicleTelemetry** tables on the Navigator dialog, and then select **Load**.

   ![StreamData and VehicleTelemetry are checked on the Navigator dialog.](media/power-bi-table-navigator.png 'Power BI Table Navigator')

6. After a few seconds, you will see a blank report appear, with a menu of Visualizations and Fields on the right-hand side. Under **Fields**, expand **StreamData**.

   ![StreamData is highlighted under Fields in Power BI](media/power-bi-fields-stream-data.png 'Power BI Fields')

7. Next, select the **Map** visualization by clicking on it in the Visualizations section on the right.

   ![The Map visualization is highlighted.](media/power-bi-map-vis.png 'Visualizations')

8. Drag the **City** field to **Location**, and **Count** to **Size**. This will place points of different sizes over cities on the map, depending on how many telemetry entries there are.

   ![Screenshot displaying where to drag the fields onto the map settings.](media/power-bi-map-fields.png 'Map settings')

9. Your map should look similar to the following:

   ![The map is shown on the report.](media/power-bi-map.png 'Map')

10. Select a blank area on the report to deselect the map and then select the **Line chart** visualization.

    ![The Line chart visualization is highlighted.](media/power-bi-line-chart-vis.png 'Visualization')

11. Drag the **speed** field to **Axis** and then drag the **enginetemperature** field to **Values**. This will allow you to visualize the relationship between speed and engine temperatures.

    ![Screenshot displaying where to drag the fields onto the line chart settings.](media/power-bi-line-chart-fields.png 'Line chart settings')

12. Next, select the down arrow next to the **enginetemperature** field under **Values**. Select **Average** from the menu to aggregate the values by average instead of the sum.

    ![The Average menu option is highlighted for the enginetemperature value.](media/power-bi-line-chart-average.png 'Average engine temperature')

13. Your line chart should look similar to the following:

    ![The line chart is shown on the report.](media/power-bi-line-chart.png 'Line chart')

14. Select a blank area on the report to deselect the line chart, and then select the **Area chart** visualization.

    ![The Area chart visualization is highlighted.](media/power-bi-area-chart-vis.png 'Area chart visualization')

15. Drag the **city** field to **Axis**, the **Make** field to **Legend**, and the **speed** field to **Values**. This will display an area chart with different colors indicating the region and the speed at which drivers travel over time within that region.

    ![Screenshot displaying where to drag the fields onto the area chart settings.](media/power-bi-area-chart-fields.png 'Area chart settings')

16. Select the down arrow next to the **speed** field under **Values**. Select **Average** from the menu to aggregate the values by average instead of the sum.

    ![The Average menu option is highlighted for the speed value.](media/power-bi-area-chart-average.png 'Average speed')

17. Your area chart should look similar to the following:

    ![The area chart on the report.](media/power-bi-area-chart.png 'Area chart')

18. Select a blank area on the report to deselect the area chart. Now select the **Line and stacked column chart** visualization.

    ![Line and stacked column chart visualization is highlighted.](media/power-bi-line-and-stacked-column-chart-vis.png 'Line and stacked column chart visualization')

19. Drag the **Make** field to **Shared axis** and then drag the **MpgCity** and **MpgHighway** fields into both the **Column values** and **Line values** fields.

    ![Screenshot displaying where to drag the fields onto the line and stacked column chart settings.](media/power-bi-line-and-stacked-column-chart-fields.png 'Line and stacked column chart settings')

20. Select the down arrow next to the **MgpCity** field under **Column values**. Select **Average** from the menu to aggregate the values by average instead of the sum.

    ![The Average menu option is highlighted for the MpgCity value.](media/power-bi-line-and-stacked-column-chart-average.png 'Average MpgCity')

21. Repeat the step above for **MpgHighway** under **Column values**, and then do the same for both **MpgCity** and **MpgHighway** under **Line values**.

22. Your line and stacked column chart should look similar to the following:

    ![The line and stacked column chart on the report.](media/power-bi-line-and-stacked-column-chart.png 'Line and stacked column chart')

23. Select **Save** on the Power BI Desktop toolbar in the upper left of the window, and then select a file location and enter a name, such as "Vehicle Telemetry", then select **Save**.

24. Your final report should look similar to the following:

    ![The report view.](media/power-bi-report.png 'Report')
