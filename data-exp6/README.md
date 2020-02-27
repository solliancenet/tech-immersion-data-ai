# Data & AI Tech Immersion Workshop – Product Review Guide and Lab Instructions

## Data, Experience 6 - Delivering the Modern Data Warehouse with Azure Synapse Analytics, Azure Databricks, Azure Data Factory, and Power BI

- [Data & AI Tech Immersion Workshop – Product Review Guide and Lab Instructions](#data--ai-tech-immersion-workshop-%e2%80%93-product-review-guide-and-lab-instructions)
  - [Data, Experience 6 - Delivering the Modern Data Warehouse with Azure Synapse Analytics, Azure Databricks, Azure Data Factory, and Power BI](#data-experience-6---delivering-the-modern-data-warehouse-with-azure-synapse-analytics-azure-databricks-azure-data-factory-and-power-bi)
  - [Technology overview](#technology-overview)
  - [Scenario overview](#scenario-overview)
  - [Task 0: Verify pre requisite environment](#task-0-verify-pre-requisite-environment)
  - [Task 1: Start the vehicle telemetry generator](#task-1-start-the-vehicle-telemetry-generator)
  - [Task 2: Execute ADF Pipeline to copy data](#task-2-execute-adf-pipeline-to-copy-data)
  - [Task 3: Read streaming data from Cosmos DB using Databricks](#task-3-read-streaming-data-from-cosmos-db-using-databricks)
  - [Task 4: Perform data aggregation and summarization](#task-4-perform-data-aggregation-and-summarization)
  - [Task 5: Persisting data to Databricks Delta tables](#task-5-persisting-data-to-databricks-delta-tables)
  - [Task 6: Visualizations and dashboards with Databricks](#task-6-visualizations-and-dashboards-with-databricks)
  - [Task 7: Send summarized data to Azure Synapse Analytics](#task-7-send-summarized-data-to-azure-synapse-analytics)
  - [Task 8: Generate reports in Power BI with data from Azure Synapse Analytics](#task-8-generate-reports-in-power-bi-with-data-from-azure-synapse-analytics)
  - [Wrap-up](#wrap-up)
  - [Additional resources and more information](#additional-resources-and-more-information)

## Technology overview

A modern data warehouse lets you bring together all your data at any scale easily, and to get insights through analytical dashboards, operational reports, or advanced analytics for all your users.

![Sample solution diagram.](media/solution-diagram1.png 'Sample solution diagram')

1. Combine all your structured, unstructured and semi-structured data (logs, files, and media) using Azure Data Factory to Azure Data Lake Storage.
2. Leverage data in Azure Data Lake Storage to perform scalable analytics with Azure Databricks and achieve cleansed and transformed data.
3. Cleansed and transformed data can be moved to Azure Synapse Analytics to combine with existing structured data, creating one hub for all your data. Leverage native connectors between Azure Databricks and Azure Synapse Analytics to access and move data at scale.
4. Build operational reports and analytical dashboards on top of Azure Synapse Analytics to derive insights from the data, and use Azure Analysis Services to serve thousands of end users.
5. Run ad hoc queries directly on data within Azure Databricks.

The same technologies also enable Advanced Analytics on big data, which allows customers to transform their data into actionable insights using the best-in-class machine learning tools. This architecture allows you to combine any data at any scale, and to build and deploy custom machine learning models at scale.

![Sample solution diagram.](media/solution-diagram2.png 'Sample solution diagram')

1. Bring together all your structured, unstructured and semi-structured data (logs, files, and media) using Azure Data Factory to Azure Data Lake Storage.
2. Use Azure Databricks to clean and transform the structureless datasets and combine them with structured data from operational databases or data warehouses.
3. Use scalable machine learning/deep learning techniques, to derive deeper insights from this data using Python, R or Scala, with inbuilt notebook experiences in Azure Databricks.
4. Leverage native connectors between Azure Databricks and Azure Synapse Analytics to access and move data at scale.
5. Power users take advantage of the inbuilt capabilities of Azure Databricks to perform root cause determination and raw data analysis.
6. Run ad hoc queries directly on data within Azure Databricks.
7. Take the insights from Azure Databricks to Cosmos DB to make them accessible through web and mobile apps.

## Scenario overview

Like many organizations, ContosoAuto generates data from numerous system, each of which has its own location and format, including structured, unstructured, and semi-structured data. They would like the ability to combine and analyze these disparate datasets in order to gain actionable insights that can help them operate their business more efficiently.

In this experience, ​​you will see how Azure Data Factory (ADF), Azure Databricks, and Azure Synapse Analytics (Data Warehouse) can be used together to build a modern data warehouse. You will start by using Azure Data Factory (ADF) to automate the movement of data in various formats gathered from various sources, including Cosmos DB, into a centralized Azure Data Lake Storage Gen2 (ADLS Gen2) repository. You will then use Azure Databricks to prepare and analyze those data, and finally write the aggregations to Azure Synapse Analytics.

As part of the process, you will also use Databricks to connect to the Cosmos DB Change Feed to stream near-real-time vehicle telemetry data directly into your Data Warehouse using Spark Structured Streaming.

## Task 0: Verify pre requisite environment

Follow these steps to pause a virtual machine and SQL Data Warehouse(If you are using an automated or provided lab environment, please perform Task 0, else you can skip to Task 1).

1. Within the Azure Portal, navigate to the resource group blade and select **jumpvm**.
<img src="media/rg.jpg"/><br/>
2. On the **Overview** pane, click the **Start** button.
<img src="media/start.jpg"/><br/>
3. Wait a few moment, and you will get the notification for vitual machine start.
4. Select the **Azure Synapse SQL Pool** from the resource group page.
<img src="media/rg1.jpg"/><br/>
5. To pause the data warehouse, click the **Resume** button.
<img src="media/resume.jpg"/><br/>
6. A confirmation question appears asking if you want to continue. Click **Yes**.
<img src="media/resumedw.jpg"/><br/>
7. On the Azure Synapse SQL Pool page, notice Status is **Online**.

## Task 1: Start the vehicle telemetry generator

The data generator console application creates and sends simulated vehicle sensor telemetry for an array of vehicles, denoted by VIN (vehicle identification number), directly to Cosmos DB. For this to happen, you first need to configure it with the Cosmos DB connection string.

In this task, you will configure and run the data generator to save simulated vehicle telemetry data to a `telemetry` collection in Cosmos DB.

1. Before running the data generator, you need to edit the configuration file for the application. Open **Visual Studio Code** from the Window Start menu.

   ![Visual Studio Code is highlighted in the Start menu.](media/vscode-start-menu.png 'Start menu')

2. In Visual Studio Code, select **Open File...** from the **File** menu.

   ![Open File is highlighted in the File menu.](media/file-menu.png 'File menu')

3. In the Open File window, navigate to `C:\lab-files\data\6\TelemetryGenerator`. Select `appsettings.json` to and then select **Open**.

   ![The `appsettings.json` file is highlighted in the C:\lab-files\data\6\TelemetryGenerator folder.](media/windows-explorer-appsettings-json.png 'Windows explorer')

4. To retrieve your Cosmos DB connection string, open a web browser and navigate to the [Azure portal](https://portal.azure.com) and select **Resource groups** from the Azure services menu.

    ![Resource groups is highlighted in the Azure services list in the Azure portal.](media/azure-resource-groups.png "Resource groups")

5. Select the **tech-immersion-XXXXX** resource group (where XXXXX is the unique identifier assigned to you for this workshop).

   ![The tech-immersion resource group is selected.](media/tech-immersion-rg.png 'Resource groups')

6. Select the **Azure Cosmos DB account** from the list of resources in your resource group.

   ![The Azure Cosmos DB account is selected in the resource group.](media/tech-immersion-rg-cosmos-db.png 'Tech Immersion resource group')

7. Select **Keys** from the left-hand menu.

   ![The Keys link on the left-hand menu is highlighted.](media/cosmos-db-keys-link.png 'Keys link')

8. Copy the **Primary Connection String** value by selecting the copy button to the right of the field.

   ![The Primary Connection String key is copied.](media/cosmos-db-keys.png 'Keys')

9. Return to the `appsettings.json` file in your text editor, and paste your Cosmos DB connection string value next to `COSMOS_DB_CONNECTION_STRING`. Make sure you have double-quotes ("") around the value, as shown in the example below:

   ```json
   {
     "COSMOS_DB_CONNECTION_STRING": "AccountEndpoint=https://tech-immersion.documents.azure.com:443/;AccountKey=xVXyajzdlD3q4UXHIpMnriBhtasLztTrMrGSJgvRl8D1bUu1B7wwfGN1Q8rhBu0BHBTc2jR9iGPRtYpIV3lAkQ==;",

     "SECONDS_TO_LEAD": "0",
     "SECONDS_TO_RUN": "3600"
   }
   ```

   `SECONDS_TO_LEAD` is the amount of time to wait before sending vehicle telemetry data. Default value is `0`.

   `SECONDS_TO_RUN` is the maximum amount of time to allow the generator to run before stopping transmission of data. Ensure the value is set to `3600`, which instructs the generator to run for 60 minutes. Data will also stop transmitting when you enter <Ctrl+C> while the generator is running, or if you close the window.

   > **Note**: The telemetry generator needs to be running for the Cosmos DB Change Feed, Spark Structured Streaming, and Azure Synapse Analytics tasks below, so if it takes longer than 60 minutes to complete this lab, you may have to restart the generator.

10. Save the `appsettings.json` file.

11. Open a new File Explorer window by selecting the File Explorer icon on the Windows Start Bar.

    ![The File Explorer icon is highlighted on the Windows Start Bar.](media/windows-start-bar-file-explorer.png 'Windows Start Bar')

12. In the File Explorer window, navigate to the `C:\lab-files\data\6\TelemetryGenerator` folder, and then locate and double-click the `TransactionGenerator.exe` file to launch the console application.

    ![Screenshot of the console window.](media/telemetry-generator-console.png 'Console window')

    > **Note**: If you search for the `TransactionGenerator.exe` file in the File Explorer search box, you will need to right-click the file in the search results and then select **Open file location** from the context menu. Failure to do this will result in an error when running the application that the Cosmos DB configuration must be provided.

13. If you see a Windows Defender dialog pop up after attempting to run the executable, select **More info**.

    ![Select More Info on the Windows Defender dialog box.](media/windows-defender-more-info.png 'Windows Defender')

    Next, click **Run anyway**.

    ![Click Run Anyway.](media/windows-defender-run-anyway.png 'Windows Defender')

14. A console window will open and you should see it start to send data after a few seconds. Once you see that it is sending data to Cosmos DB, _minimize_ the window and allow it to run in the background throughout this experience.

    ![Screenshot of the console window.](media/vs-console.png 'Console window')

    > The top of the output displays information about the Cosmos DB collection you created (telemetry), the requested RU/s as well as estimated hourly and monthly cost. After every 250 records are requested to be sent, you will see output statistics.

## Task 2: Execute ADF Pipeline to copy data

In this task, you will quickly set up your ADLS Gen2 filesystem using a Databricks notebook, and then review and execute ADF pipelines to copy data from various sources, including Cosmos DB, in your ADLS Gen2 filesystem.

1. In a web browser, navigate to the [Azure portal](https://portal.azure.com), select **Resource groups** from the left-hand menu, and then select the resource group named **tech-immersion-XXXXX** resource group (where XXXXX is the unique identifier assigned to you for this workshop).

   ![The tech-immersion resource group is selected.](media/tech-immersion-rg.png 'Resource groups')

2. Prior to using ADF to move data into your ADLS Gen2 instance, you must create a filesystem in ADLS Gen2. This will be done using an Azure Databricks notebook. Select your **Azure Databricks Service** resource from the list of resources in the resource group. This will be named **XXXXX** (where XXXXX is the unique identifier assigned to you for this workshop).

   ![The Databricks resource is selected from the list of resources in the tech-immersion resource group.](media/tech-immersion-rg-databricks.png 'Tech Immersion resource group')

3. On the Azure Databricks Service blade, select **Launch Workspace**.

   ![Databricks Launch Workspace](media/tech-immersion-databricks-launch-workspace.png 'Launch Workspace')

4. In your Databricks workspace, confirm your Databricks cluster is running by selecting **Clusters** from the left-hand menu, and ensure the Status is **Running**. If it is **Terminated**, select the **Start** button for the cluster. Select **Confirm** in the dialog to start the cluster.

   > Check if the cluster is running a supported runtime version by opening the cluster. If there is a red warning sign next to the Databricks Runtime Version, edit the cluster and select a higher version.

   ![The start button for the cluster is highlighted on the Clusters page in Databricks.](media/databricks-cluster-start.png 'Clusters')

   > It will take 2-4 minutes for the cluster to start. You can move on to the next steps while the cluster is starting up.

5. Select **Workspace** from the left-hand menu, and then select **Shared**.

6. Select the drop down arrow next to Shared, and select **Import** from the context menu.

   ![Import is highlighted in the context menu for the Shared workspace in Databricks.](media/databricks-workspace-shared-import.png 'Import')

7. On the Import Notebooks dialog, select **Browse** and select the **`Tech-Immersion.dbc`** file located in the `C:\lab-files\data\6` folder on your lab VM, and then select **Import**.

   ![The Import Notebooks dialog is displayed, with the `Tech-Immersion.dbc` file listed in the import box.](media/databricks-workspace-import-notebooks.png 'Import notebooks')

8. In the shared workspace, select the **Tech-Immersion** folder, followed by the **Data** and **Experience-6** folders. Then select the notebook named **1-Environment-Setup**.

9. In the **1-Environment-Setup** notebook, follow the instructions contained in the notebook, and then return here to complete the remaining steps of this task.

10. In the Azure portal, navigate to the **tech-immersion-XXXXX** resource group (where XXXXX is the unique identifier assigned to you for this workshop) as you did in step 1 above, and then select **tech-immersion-df-XXXXX** from the list of resources.

    ![The Data Factory resource is selected from the list of resources in the tech-immersion resource group.](media/tech-immersion-rg-data-factory.png 'Tech Immersion resource group')

11. On the Data Factory blade, select the **Author & Monitor** tile to launch the Azure Data Factory management page.

    ![The Author & Monitor tile is highlighted on the Data Factory overview blade.](media/data-factory-author-and-monitor.png 'Author & Monitor')

12. On the Azure Data Factory page, select the **Author** (pencil) icon from the left-hand menu.

    ![The Author icon is highlighted on the left-hand menu of the Azure Data Factory page.](media/data-factory-home-author.png 'Data Factory Author icon')

13. On the ADF Author page, select **Pipelines** to expand the list, and then select the **CopyData** pipeline from the list.

    ![Azure Data Factory pipelines](media/data-factory-pipelines-copydata.png 'ADF pipelines')

    > The `CopyData` pipeline consists of three copy activities. Two of the activities connect to your Azure SQL Database instance to retrieve vehicle data from tables there. The third connects to Cosmos DB to retrieve batch vehicle telemetry data. Each of the copy activities writes data into files in ADLS Gen2.

14. On the pipeline toolbar, select **Add Trigger** and then **Trigger Now** to run the `CopyData` pipeline, and then select **Finish** on the Pipeline Run dialog. You will receive a notification that they `CopyData` pipeline is running.

    ![Trigger is highlighted in the Data Factory pipeline toolbar.](media/data-factory-pipeline-toolbar.png 'Data Factory pipeline toolbar')

15. To observe the pipeline run, select the **Monitor** icon from the left-hand menu, which will bring up a list of active and recent pipeline runs.

    ![Azure Data Factory pipeline runs](media/data-factory-monitor-pipeline-runs.png 'Azure Data Factory Monitor')

    > On the pipeline runs monitor page, you can see all active and recent pipeline runs. The **Status** field provide and indication of the state of the pipeline run, from In Progress to Failed or Canceled. You also have the option to filter by Status and set custom date ranges to get a specific status and time period.

16. Select the **Activity Runs** icon under Actions for the currently running pipeline to view the status of the individual activities which make up the pipeline.

    ![Data Factory activity runs](media/data-factory-monitor-activity-runs.png 'Data Factory activity runs')

    > The **Activity Runs** view allows you to monitor individual activities within your pipelines. In this view, you can see the amount of time each activity took to execute, as well as select the various icons under Actions to view the inputs, outputs, and details of each activity run. As with pipeline runs, you are provided with the Status of each activity.

## Task 3: Read streaming data from Cosmos DB using Databricks

You have now used ADF to move data from various sources, including Cosmos DB, into an ADLS Gen2 filesystem. In this task, you will use an Azure Databricks notebook to extend the use of Cosmos DB further. You will create a connection to your Cosmos DB instance, using the Azure Cosmos DB Spark Connector, and query streaming data from the Cosmos DB Change Feed.

1. Return to the Azure Databricks Workspace you opened previously, and in your Databricks workspace, select **Workspace** from the left-hand menu, and then select **Shared**.

2. In the shared workspace, select the **Tech-Immersion** folder, followed by the **Data** and **Experience-6** folders. Then select the notebook named **2-Cosmos-DB-Change-Feed**.

3. In the **2-Cosmos-DB-Change-Feed** notebook, follow the instructions to complete the remaining steps of this task.

> The notebook contains all the instructions needed to complete this task. In addition, the final cell of the notebook contains instructions on the next step, which will include a link to the notebook for the next task in this experience, or instructions to return to this document.

## Task 4: Perform data aggregation and summarization

In this task, you will using Databricks to perform data preparation, aggregation and summarization with both batch and streaming data.

1. In your Databricks workspace, select **Workspace** from the left-hand menu, then select **Shared**.

2. In the shared workspace, select the **Tech-Immersion** folder, followed by the **Data** and **Experience 6** folders. Then select the notebook named **3-Aggregation-and-Summarization**.

3. In the **3-Aggregation-and-Summarization** notebook, follow the instructions to complete the remaining steps of this task.

> The notebook contains all the instructions needed to complete this task. In addition, the final cell of the notebook contains instructions on the next step, which will include a link to the notebook for the next task in this experience, or instructions to return to this document.

## Task 5: Persisting data to Databricks Delta tables

In this task, you will see how Databricks Delta provides capabilities previous unavailable for updating records in an Hive table by using the `UPSERT` method to update existing records and insert new records.

1. In your Databricks workspace, select **Workspace** from the left-hand menu, then select **Shared**.

2. In the shared workspace, select the **Tech-Immersion** folder, followed by the **Data** and **Experience-6** folders. Then select the notebook named **4-Databricks-Delta**.

3. In the **4-Databricks-Delta** notebook, follow the instructions to complete the remaining steps of this task.

> The notebook contains all the instructions needed to complete this task. In addition, the final cell of the notebook contains instructions on the next step, which will include a link to the notebook for the next task in this experience, or instructions to return to this document.

## Task 6: Visualizations and dashboards with Databricks

In this task, you will use visualizations configured within a Databricks notebook to build a dashboard displaying your data aggregations.

1. In your Databricks workspace, select **Workspace** from the left-hand menu, then select **Shared**.

2. In the shared workspace, select the **Tech-Immersion** folder, followed by the **Data** and **Experience-6** folders. Then select the notebook named **5-Databricks-Dashboards**.

3. In the **5-Databricks-Dashboards** notebook, follow the instructions to complete the remaining steps of this task.

> The notebook contains all the instructions needed to complete this task. In addition, the final cell of the notebook contains instructions on the next step, which will include a link to the notebook for the next task in this experience, or instructions to return to this document.

## Task 7: Send summarized data to Azure Synapse Analytics

In this task, you will use the Azure Synapse Analytics connector to write aggregated data from Databricks into your Data Warehouse. You will also apply aggregations to streaming data from the Cosmos DB Change Feed, and stream the data directly into your Data Warehouse from Databricks.

1. In your Databricks workspace, select **Workspace** from the left-hand menu, then select **Shared**.

2. In the shared workspace, select the **Tech-Immersion** folder, followed by the **Data** and **Experience-6** folders. Then select the notebook named **6-Write-to-SQL-DW**.

3. In the **6-Write-to-SQL-DW** notebook, follow the instructions to complete the remaining steps of this task.

> The notebook contains all the instructions needed to complete this task. In addition, the final cell of the notebook contains instructions on the next step, which will include a link to the notebook for the next task in this experience, or instructions to return to this document.

## Task 8: Generate reports in Power BI with data from Azure Synapse Analytics

In this task, you will use Power BI Desktop to read data from Azure Synapse Analytics to create reports showing vehicle telemetry data.

1. Launch Power BI Desktop, and select **Get data** on the splash screen.

   ![The Power BI Desktop splash screen is shown with the Get data link highlighted.](media/power-bi-desktop.png 'Power BI Desktop splash screen')

2. On the Get Data dialog, select **Azure** on the left-hand side, select **Azure SQL Data Warehouse** or **Azure Synapse Analytics**  from the list of available Azure services, and then select **Connect**.

   ![The Power BI Get Data dialog is displayed, with Azure selected on the left and Azure Synapse Analytics selected on the right. The Connect button is highlighted.](media/power-bi-get-data-sql-dw.png 'Power BI Get Data')

3. On the SQL Server database dialog that appears, enter the following:

   - **Server**: Copy and paste the name of your Data Warehouse Server from the Azure portal.

     - In the Azure portal, navigate to the **tech-immersion-XXXXX** resource group (where XXXXX is the unique identifier assigned to you for this workshop), and select your Azure Synapse Analytics resource.

       ![The Azure Synapse Analytics resource is highlighted in the tech-immersion resource group.](media/resources-group-sql-dw.png 'Tech Immersion Resource Group')

     - On the Azure Synapse Analytics overview blade, copy the Server name.

       ![The Server name is highlighted on the Azure Synapse Analytics overview blade.](media/sql-dw-server-name.png 'SQL Server Data Warehouse')

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

    > **Note**: Data generated is random, so you may not see the same trend lines as represented in the image above.

14. Select a blank area on the report to deselect the line chart, and then select the **Area chart** visualization.

    ![The Area chart visualization is highlighted.](media/power-bi-area-chart-vis.png 'Area chart visualization')

15. Drag the **city** field to **Axis**, the **Make** field to **Legend**, and the **speed** field to **Values**. This will display an area chart with different colors indicating the region and the speed at which drivers travel over time within that region.

    ![Screenshot displaying where to drag the fields onto the area chart settings.](media/power-bi-area-chart-fields.png 'Area chart settings')

16. Select the down arrow next to the **speed** field under **Values**. Select **Average** from the menu to aggregate the values by average instead of the sum.

    ![The Average menu option is highlighted for the speed value.](media/power-bi-area-chart-average.png 'Average speed')

17. Your area chart should look similar to the following:

    ![The area chart on the report.](media/power-bi-area-chart.png 'Area chart')

    > **Note**: Data generated is random, so you may not see the same trend lines as represented in the image above.

18. Select a blank area on the report to deselect the area chart. Now select the **Line and stacked column chart** visualization.

    ![Line and stacked column chart visualization is highlighted.](media/power-bi-line-and-stacked-column-chart-vis.png 'Line and stacked column chart visualization')

19. Drag the **Make** field to **Shared axis** and then drag the **MpgCity** and **MpgHighway** fields into both the **Column values** and **Line values** fields.

    ![Screenshot displaying where to drag the fields onto the line and stacked column chart settings.](media/power-bi-line-and-stacked-column-chart-fields.png 'Line and stacked column chart settings')

20. Select the down arrow next to the **MgpCity** field under **Column values**. Select **Average** from the menu to aggregate the values by average instead of the sum.

    ![The Average menu option is highlighted for the MpgCity value.](media/power-bi-line-and-stacked-column-chart-average.png 'Average MpgCity')

21. Repeat the step above for **MpgHighway** under **Column values**, and then do the same for both **MpgCity** and **MpgHighway** under **Line values**.

22. Your line and stacked column chart should look similar to the following:

    ![The line and stacked column chart on the report.](media/power-bi-line-and-stacked-column-chart.png 'Line and stacked column chart')

    > **Note**: Data generated is random, so you may not see the same trend lines as represented in the image above.

23. Select **Save** on the Power BI Desktop toolbar in the upper left of the window, and then select a file location and enter a name, such as "Vehicle Telemetry", then select **Save**.

24. Your final report should look similar to the following:

    ![The report view.](media/power-bi-report.png 'Report')

## Wrap-up

In this experience, ​​you used Azure Data Factory (ADF), Azure Databricks, and Azure Synapse Analytics together to build a modern data warehouse.

You started by using Azure Data Factory (ADF) to automate the movement of data in various formats gathered from various sources, including Cosmos DB, into Azure Data Lake Storage Gen2 (ADLS Gen2). You then used Azure Databricks to prepare, analyze and visualize those data. Next, you used Spark Structured Streaming, in connection with the Azure Cosmos DB Spark Connector, to query streaming data from the Cosmos DB Change Feed, demonstrating how you can easily include near real-time data in your queries and aggregations in Databricks. You wrote aggregations of both static and streaming data into Azure Synapse Analytics.

You ended the modern data warehouse experience by using Power BI Desktop to connect to your Data Warehouse, and building a dashboard to provide visualizations of vehicle telemetry data.

## Additional resources and more information

To continue learning and expand your understanding of building modern data warehouses, use the links below.

- [Azure Modern Data Warehouse](https://azure.microsoft.com/solutions/data-warehouse/)
- [Introduction to Azure Synapse Analytics](https://www.youtube.com/watch?v=tMYOi5E14eU&t=4s) (video)
- [More information about Azure Synapse Analytics](https://azure.microsoft.com/services/sql-data-warehouse/)
- [Azure Synapse Analytics documentation](https://docs.microsoft.com/azure/sql-data-warehouse/)
- [More information about Azure Data Factory](https://azure.microsoft.com/services/data-factory/)
- [Modern data warehouse architecture](https://azure.microsoft.com/solutions/architecture/modern-data-warehouse/)
- [Azure Data Factory documentation](https://docs.microsoft.com/azure/data-factory/)
- [More information about Azure Databricks](https://azure.microsoft.com/services/databricks/)
- [Azure Databricks documentation](https://docs.microsoft.com/azure/azure-databricks/)
- [Power BI product page](https://powerbi.microsoft.com/)
- [Power BI documentation](https://docs.microsoft.com/power-bi/)
