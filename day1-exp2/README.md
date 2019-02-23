# Day 1, Experience 2 - Leveraging Cosmos DB for near real-time analytics

ContosoAuto is collecting vehicle telemetry and wants to use Cosmos DB to rapidly ingest and store the data in its raw form, then do some processing in near real-time. In the end, they want to create a dashboard that automatically updates with new data as it flows in after being processed. What they would like to see on the dashboard are various visualizations of detected anomalies, like engines overheating, abnormal oil pressure, and aggressive driving, using components such as a map to show anomalies related to cities, as well as various charts and graphs depicting this information in a clear way.

In this experience, you will use Azure Cosmos DB to ingest streaming vehicle telemetry data as the entry point to a near real-time analytics pipeline built on Cosmos DB, Azure Functions, Event Hubs, Azure Stream Analytics, and Power BI. To start, you will complete configuration and performance-tuning on Cosmos DB to prepare it for data ingest, and use the change feed capability of Cosmos DB to trigger Azure Functions for data processing. The function will enrich the telemetry data with location information, then send it to Event Hubs. Azure Stream Analytics extracts the enriched sensor data from Event Hubs, performs aggregations over windows of time, then sends the aggregated data to Power BI for data visualization and analysis. A vehicle telemetry data generator will be used to send vehicle telemetry data to Cosmos DB.

- [Day 1, Experience 2 - Leveraging Cosmos DB for near real-time analytics](#day-1-experience-2---leveraging-cosmos-db-for-near-real-time-analytics)
  - [Exercise 1: Configure Cosmos DB](#exercise-1-configure-cosmos-db)
  - [Exercise 2: Configure Event Hubs](#exercise-2-configure-event-hubs)
  - [Exercise 3: Configure Stream Analytics](#exercise-3-configure-stream-analytics)
  - [Exercise 4: Configure Azure Function App](#exercise-4-configure-azure-function-app)
  - [Exercise 5: Publish Function App and run data generator](#exercise-5-publish-function-app-and-run-data-generator)
  - [Exercise 6: Create Power BI dashboard](#exercise-6-create-power-bi-dashboard)

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

## Exercise 2: Configure Event Hubs

In this exercise, you will create and configure a new event hub within the provided Event Hubs namespace. This will be used to capture vehicle telemetry after it has been processed and enriched by the Azure function you will create later on.

1.  Navigate to the [Azure portal](https://portal.azure.com).

2.  Select **Resource groups** from the left-hand menu. Then select the resource group named **tech-immersion**.

    ![The tech-immersion resource group is selected.](media/tech-immersion-rg.png 'Resource groups')

3.  Select the **Event Hubs Namespace** from the list of resources in your resource group.

    ![The Event Hubs Namespace is selected in the resource group.](media/tech-immersion-rg-event-hubs.png 'tech-immersion resource group')

4.  Within the Event Hubs Namespace blade, select **Event Hubs** within the left-hand menu.

    ![The Event Hubs link is selected in the left-hand menu.](media/event-hubs-link.png 'Event Hubs link')

5.  Select **+ Event Hub** in the top toolbar to create a new event hub in the namespace.

    ![The new Event Hub link is highlighted in the top toolbar.](media/event-hubs-new-event-hub-link.png 'New event hub link')

6.  In the **Create Event Hub** blade, configure the following:

    - **Name:** Enter "telemetry".
    - **Partition Count:** Select 2.
    - **Message Retention**: Select 1.
    - **Capture:** Select Off.

    ![The Create Event Hub form is filled out with the previously mentioned settings entered into the appropriate fields.](media/event-hubs-create-event-hub.png 'Create Event Hub')

7.  Select **Create** on the bottom of the form when you are finished entering the values.

8.  Select your newly created **telemetry** event hub from the list after it is created.

    ![The newly created telemetry event hub is selected.](media/event-hubs-select.png 'Event hubs')

9.  Select **Shared access policies** from the left-hand menu.

    ![The Shared access policies link is selected in the left-hand menu.](media/event-hubs-shared-access-policies-link.png 'Shared access policies link')

10. Select **+ Add** in the top toolbar to create a new shared access policy.

    ![The Add button is highlighted.](media/event-hubs-shared-access-policies-add-link.png 'Add')

11. In the **Add SAS Policy** blade, configure the following:

    - **Name:** Enter "Read".
    - **Managed:** Unchecked.
    - **Send:** Unchecked.
    - **Listen:** Checked.

    ![The Add SAS Policy form is filled out with the previously mentioned settings entered into the appropriate fields.](media/event-hubs-add-sas-policy-read.png 'Add SAS Policy')

    > It is a best practice to create separate policies for reading, writing, and managing events. This follows the principle of least privilege to prevent services and applications from performing unauthorized operations.

12. Select **Create** on the bottom of the form when you are finished entering the values.

13. Select **+ Add** in the top toolbar to create a new shared access policy.

    ![The Add button is highlighted.](media/event-hubs-shared-access-policies-add-link.png 'Add')

14. In the **Add SAS Policy** blade, configure the following:

    - **Name:** Enter "Write".
    - **Managed:** Unchecked.
    - **Send:** Checked.
    - **Listen:** Unchecked.

    ![The Add SAS Policy form is filled out with the previously mentioned settings entered into the appropriate fields.](media/event-hubs-add-sas-policy-write.png 'Add SAS Policy')

15. Select **Create** on the bottom of the form when you are finished entering the values.

16. Select your **Read** policy from the list. Copy the **Connection string - primary key** value by selecting the Copy button to the right of the field. **SAVE THIS VALUE** in Notepad or similar text editor for later.

    ![The Read policy is selected and its blade displayed. The Copy button next to the Connection string - primary key field is highlighted.](media/event-hubs-read-policy-key.png 'SAS Policy: Read')

17. Now select your **Write** policy from the list. Copy the **Connection string - primary key** value by selecting the Copy button to the right of the field. **SAVE THIS VALUE** in Notepad or similar text editor for later.

    ![The Write policy is selected and its blade displayed. The Copy button next to the Connection string - primary key field is highlighted.](media/event-hubs-write-policy-key.png 'SAS Policy: Write')

## Exercise 3: Configure Stream Analytics

In this exercise, you will create and configure a new event hub within the provided Event Hubs namespace. This will be used to capture vehicle telemetry after it has been processed and enriched by the Azure function you will create later on.

1.  Navigate to the [Azure portal](https://portal.azure.com).

2.  Select **Resource groups** from the left-hand menu. Then select the resource group named **tech-immersion**.

    ![The tech-immersion resource group is selected.](media/tech-immersion-rg.png 'Resource groups')

3.  Select the **Stream Analytics job** from the list of resources in your resource group.

    ![The Stream Analytics job is selected in the resource group.](media/tech-immersion-rg-stream-analytics.png 'tech-immersion resource group')

4.  Within the Stream Analytics job blade, select **Inputs** within the left-hand menu.

    ![The Inputs link is selected in the left-hand menu.](media/inputs-link.png 'Inputs link')

5.  Select **+ Add stream input** in the top toolbar, then select **Event Hub** to create a new Event Hub input.

    ![The Add stream input button and Event Hub menu item are highlighted.](media/stream-analytics-add-input-link.png 'Add stream input - Event Hub')

6.  In the **New Input** blade, configure the following:

    - **Name:** Enter "eventhub".
    - **Select Event Hub from your subscriptions:** Selected.
    - **Subscription:** Make sure the subscription you are using for this lab is selected.
    - **Event Hub namespace:** Select the Event Hub namespace you are using for this lab.
    - **Event Hub name:** Select **Use existing**, then select **telemetry**, which you created earlier.
    - **Event Hub policy name:** Select **Read**.
    - Leave all other values at their defaults.

    ![The New Input form is filled out with the previously mentioned settings entered into the appropriate fields.](media/stream-analytics-new-input.png 'New Input')

7.  Select **Save** on the bottom of the form when you are finished entering the values.

8.  Within the Stream Analytics job blade, select **Outputs** within the left-hand menu.

    ![The Outputs link is selected in the left-hand menu.](media/outputs-link.png 'Outputs link')

9.  Select **+ Add** in the top toolbar, then select **Power BI** to create a new Power BI output.

    ![The Add button and Power BI menu item are highlighted.](media/stream-analytics-add-output-link.png 'Add output - Power BI')

10. In the **New Output** blade, select the **Authorize** button to authorize a connection from Stream Analytics to your Power BI account. If you do not have a Power BI account, select the **Sign up** link below the button.

    ![The Authorize button is highlighted in the New Output blade.](media/stream-analytics-new-output-authorize.png 'New Output')

11. When prompted, sign in to your Power BI account.

    ![The Power BI sign in form is displayed.](media/power-bi-sign-in.png 'Power BI Sign In')

12. After successfully signing in to your Power BI account, the New Output blade will update to show you are currently authorized.

    ![The New Output blade has been updated to show user is authorized to Power BI.](media/stream-analytics-new-output-authorized.png 'Authorized')

13. In the **New Output** blade, configure the following:

    - **Output alias:** Enter "powerBIAlerts".
    - **Group workspace:** Select My Workspace.
    - **Dataset name:** Enter "VehicleAnomalies".
    - **Table name:** Enter "Alerts".

    ![The New Output form is filled out with the previously mentioned settings entered into the appropriate fields.](media/stream-analytics-new-output.png 'New Output')

14. Select **Save** on the bottom of the form when you are finished entering the values.

15. Within the Stream Analytics job blade, select **Query** within the left-hand menu.

    ![The Query link is selected in the left-hand menu.](media/query-link.png 'Query link')

16. Clear the edit **Query** window and paste the following in its place:

    ```sql
    WITH
    Averages AS (
    select
        AVG(engineTemperature) averageEngineTemperature,
        AVG(speed) averageSpeed
    FROM
        eventhub TIMESTAMP BY [timestamp]
    GROUP BY
        TumblingWindow(Duration(second, 2))
    ),
    Anomalies AS (
    select
        t.vin,
        t.[timestamp],
        t.city,
        t.region,
        t.outsideTemperature,
        t.engineTemperature,
        a.averageEngineTemperature,
        t.speed,
        a.averageSpeed,
        t.fuel,
        t.engineoil,
        t.tirepressure,
        t.odometer,
        t.accelerator_pedal_position,
        t.parking_brake_status,
        t.headlamp_status,
        t.brake_pedal_status,
        t.transmission_gear_position,
        t.ignition_status,
        t.windshield_wiper_status,
        t.abs,
        (case when a.averageEngineTemperature >= 405 OR a.averageEngineTemperature <= 15 then 1 else 0 end) as enginetempanomaly,
        (case when t.engineoil <= 1 then 1 else 0 end) as oilanomaly,
        (case when (t.transmission_gear_position = 'first' OR
            t.transmission_gear_position = 'second' OR
            t.transmission_gear_position = 'third') AND
            t.brake_pedal_status = 1 AND
            t.accelerator_pedal_position >= 90 AND
            a.averageSpeed >= 55 then 1 else 0 end) as aggressivedriving
    from eventhub t TIMESTAMP BY [timestamp]
    INNER JOIN Averages a ON DATEDIFF(second, t, a) BETWEEN 0 And 2
    )
    SELECT
        *
    INTO
        powerBIAlerts
    FROM
        Anomalies
    where aggressivedriving = 1 OR enginetempanomaly = 1 OR oilanomaly = 1
    ```

    ![The query above has been inserted into the Query window.](media/stream-analytics-query.png 'Query window')

    The query averages the engine temperature and speed over a two second duration. Then it selects all telemetry data, including the average values from the previous step, and specifies the following anomalies as new fields:

    a. **enginetempanomaly**: When the average engine temperature is \>= 405 or \<= 15.

    b. **oilanomaly**: When the engine oil \<= 1.

    c. **aggressivedriving**: When the transmission gear position is in first, second, or third, and the brake pedal status is 1, the accelerator pedal position \>= 90, and the average speed is \>= 55.

    Finally, the query outputs all fields from the anomalies step into the `powerBIAlerts` output where aggressivedriving = 1 or enginetempanomaly = 1 or oilanomaly = 1.

17. Select **Save** in the top toolbar when you are finished updating the query.

18. Within the Stream Analytics job blade, select **Overview** within the left-hand menu. On top of the Overview blade, select **Start**.

    ![The Start button is highlighted on top of the Overview blade.](media/stream-analytics-overview-start-button.png 'Overview')

19. In the Start job blade that appears, select **Now** for the job output start time, then select **Start**. This will start the Stream Analytics job so it will be ready to start processing and sending your events to Power BI later on.

    ![The Now and Start buttons are highlighted within the Start job blade.](media/stream-analytics-start-job.png 'Start job')

## Exercise 4: Configure Azure Function App

In this exercise, you will configure the Function App with the Azure Cosmos DB and Event Hubs connection strings.

1.  Navigate to the [Azure portal](https://portal.azure.com).

2.  Select **Resource groups** from the left-hand menu. Then select the resource group named **tech-immersion**.

    ![The tech-immersion resource group is selected.](media/tech-immersion-rg.png 'Resource groups')

3.  Select the **App Service** (Azure Function App) from the list of resources in your resource group.

    ![The App Service Function App is selected in the resource group.](media/tech-immersion-rg-function-app.png 'tech-immersion resource group')

4.  Within the Function App Overview blade, scroll down and select **Application settings**.

    ![The Function App Overview blade is displayed with the Application Settings link highlighted.](media/function-app-app-settings-link.png 'Function App overview')

5.  Select **Add new setting** at the bottom of the Application settings section.

    ![The Add new setting link is highlighted on the bottom of the Application settings section.](media/function-app-app-settings-new-link.png 'Application settings')

6.  Enter **CosmosDbConnectionString** into the **Name** field, then paste your Cosmos DB connection string into the **Value** field. If you cannot locate your connection string, refer to Exercise 1, step 10.

    ![The CosmosDbConnectionString name and value pair has been added and is highlighted.](media/function-app-app-settings-cosmos-db.png 'Application settings')

7.  Select **Add new setting** underneath the new application setting you just added to add a new one.

8.  Enter **EventHubsConnectionString** into the **Name** field, then paste your Event Hubs connection string into the **Value** field. If you cannot locate your connection string, refer to Exercise 2, step 17.

    ![The EventHubsConnectionString name and value pair has been added and is highlighted.](media/function-app-app-settings-event-hubs.png 'Application settings')

9.  Scroll to the top of the page and select **Save** in the top toolbar to apply your changes.

    ![The Save button is highlighted on top of the Application settings blade.](media/function-app-app-settings-save.png 'Application settings')

## Exercise 5: Publish Function App and run data generator

In this exercise, you will open the lab solution in Visual Studio, publish the Function App, and configure and run the data generator. The data generator saves simulated vehicle telemetry data to Cosmos DB, which triggers the Azure function to run and process the data, sending it to Event Hubs, prompting your Stream Analytics job to aggregate and analyze the enriched data and send it to Power BI. The final step will be to create the Power BI report in the exercise that follows.

1.  Open Windows Explorer and navigate to `C:\lab-files`. Double-click on **TechImmersion.sln** to open the solution in Visual Studio. If you are prompted by Visual Studio to log in, log in with your Azure credentials you are using for this lab.

    ![The TechImmersion.sln file is highlighted in the C:\tech-immersion folder.](media/vs-solution.png 'Windows explorer')

    The Visual Studio solution contains the following projects:

    - **TechImmersion.CarEventProcessor**: Azure Function App project from which you will publish the Azure function that processes Cosmos DB documents as they arrive, and sends them to Event Hubs.
    - **TechImmersion.Common**: Common library that contains models and structs used by the other projects within the solution.
    - **TransactionGenerator**: Console app that generates simulated vehicle telemetry and writes it to Cosmos DB.

2.  Select the **Build** menu item, then select **Build Solution**. You should see a message in the output window on the bottom of the Visual Studio window that the build successfully completed. One of the operations that completes during this process is to download and install all NuGet packages.

    ![The Build menu item and Build Solution sub-menu item are highlighted.](media/vs-build-solution.png 'Build Solution')

3.  You will see the projects listed within the Solution Explorer in Visual Studio. Right-click the **TechImmersion.CarEventProcessor** solution, then select **Publish...** in the context menu.

    ![The TechImmersion.CarEventProcessor project and the Publish menu item are highlighted.](media/vs-publish-link.png 'Solution Explorer')

4.  Select **Select Existing** underneath Azure App Service since you will be publishing this to an existing Function App. Click **Publish** on the bottom of the dialog window. If you are prompted to log into your Azure Account, log in with the Azure account you are using for this lab.

    ![The Select Existing radio button and Publish button are highlighted.](media/vs-publish-target.png 'Pick a publish target')

5.  In the App Service dialog that follows, make sure your Azure **Subscription** for this lab is selected, then find and expand the **tech-immersion** resource group. Select your Function App, then click **OK** on the bottom of the dialog window

    ![The Function App and OK button are highlighted.](media/vs-publish-app-service.png 'App Service')

6.  The Function App will start publishing in a moment. You can watch the output window for the publish status. When it is done publishing, you should see a "Publish completed" message on the bottom of the output window.

    ![The Publish Succeeded and Publish Completed messages are highlighted in the output window.](media/vs-publish-output.png 'Publish output')

7.  Expand the **TransactionGenerator** project within the Solution Explorer, then double-click on **appsettings.json** to open it.

    ![The appsettings.json file is highlighted in Solution Explorer.](media/vs-appsettings-link.png 'Solution Explorer')

8.  Paste your Cosmos DB connection string value next to `COSMOS_DB_CONNECTION_STRING`. Make sure you have quotes ("") around the value, as shown.

    ![The Cosmos DB connection string is highlighted within the appsettings.json file.](media/vs-appsettings.png 'appsettings.json')

    `SECONDS_TO_LEAD` is the amount of time to wait before sending vehicle telemetry data. Default value is `0`.

    `SECONDS_TO_RUN` is the maximum amount of time to allow the generator to run before stopping transmission of data. The default value is `600`. Data will also stop transmitting when you enter Ctrl+C while the generator is running, or if you close the window.

9.  Now you are ready to run the transaction generator. Select the **Debug** menu item, then select **Start Debugging**, or press _F-5_ on your keyboard.

    ![The Debug menu item and Start Debugging sub-menu item are selected](media/vs-debug.png 'Debug')

10. A new console window will open, and you should see it start to send data after a few seconds. Once you see that it is sending data to Cosmos DB, _minimize_ the window and keep it running in the background.

    ![Screenshot of the console window.](media/vs-console.png 'Console window')

    The top of the output displays information about the Cosmos DB collection you created (telemetry), the requested RU/s as well as estimated hourly and monthly cost. After every 1,000 records are requested to be sent, you will see output statistics.

## Exercise 6: Create Power BI dashboard

In this exercise, you will use Power BI to create a report showing captured vehicle anomaly data. Then you will pin that report to a live dashboard for near real-time updates.

1.  Open your web browser and navigate to <https://powerbi.microsoft.com/>. Select **Sign in** on the upper-right.

    ![The Power BI home page is shown with the Sign in link highlighted.](media/pbi-signin.png 'Power BI home page')

2.  Enter your Power BI credentials you used when creating the Power BI output for Stream Analytics.

3.  After signing in, select **My Workspace** on the left-hand menu.

    ![The My Workspace link is selected on the left-hand menu.](media/pbi-my-workspace-link.png 'My Workspace')

4.  Select the **Datasets** tab on top of the workspace. Locate the dataset named **VehicleAnomalies**, then select the **Create Report** action button to the right of the name. If you do not see the dataset, you may need to wait a few minutes and refresh the page.

    ![The Datasets tab is selected in My Workspace and the VehicleAnomalies dataset is highlighted.](media/pbi-my-workspace.png 'Datasets')

5.  You should see a new blank report for VehicleAnomalies with the field list on the far right.

    ![A new blank report is displayed with the field list on the right.](media/pbi-blank-report.png 'Blank report')

6.  Select the **Map** visualization within the Visualizations section on the right.

    ![The Map visualization is highlighted.](media/pbi-map-vis.png 'Visualizations')

7.  Drag the **city** field to **Location**, and **aggressivedriving** to **Size**. This will place points of different sizes over cities on the map, depending on how many aggressive driving records there are.

    ![Screenshot displaying where to drag the fields onto the map settings.](media/pbi-map-fields.png 'Map settings')

8.  Your map should look similar to the following:

    ![The map is shown on the report.](media/pbi-map.png 'Map')

9.  Select a blank area on the report to deselect the map. Now select the **Treemap** visualization.

    ![The Treemap visualization is highlighted.](media/pbi-treemap-vis.png 'Visualization')

10. Drag the **enginetemperature** field to **Values**, then drag the **transmission_gear_position** field to **Group**. This will group the engine temperature values by the transmission gear position on the treemap so you can see which gears are associated with the hottest or coolest engine temperatures. The treemap sizes the groups according to the values, with the largest appearing on the upper-left and the lowest on the lower-right.


    ![Screenshot displaying where to drag the fields onto the treemap settings.](media/pbi-treemap-fields.png "Treemap settings")

11. Select the down arrow next to the **enginetemperature** field under **Values**. Select **Average** from the menu to aggregate the values by average instead of the sum.


    ![The Average menu option is highlighted for the enginetemperature value.](media/pbi-treemap-average.png "Average engine temperature")

12. Your treemap should look similar to the following:

    ![The treemap is shown on the report.](media/pbi-treemap.png 'Treemap')

13. Select a blank area on the report to deselect the treemap. Now select the **Area chart** visualization.

    ![The Area chart visualization is highlighted.](media/pbi-areachart-vis.png 'Area chart visualization')

14. Drag the **region** field to **Legend**, the **speed** field to **Values**, and the **timestamp** field to **Axis**. This will display an area chart with different colors indicating the region and the speed at which drivers travel over time within that region.

    ![Screenshot displaying where to drag the fields onto the area chart settings.](media/pbi-areachart-fields.png 'Area chart settings')

15. Select the down arrow next to the **speed** field under **Values**. Select **Average** from the menu to aggregate the values by average instead of the sum.

    ![The Average menu option is highlighted for the speed value.](media/pbi-areachart-average.png 'Average speed')

16. Your area chart should look similar to the following:

    ![The area chart on the report.](media/pbi-areachart.png 'Area chart')

17. Select a blank area on the report to deselect the area chart. Now select the **Multi-row card** visualization.

    ![The multi-card visualization is highlighted.](media/pbi-card-vis.png 'Multi-row card visualization')

18. Drag the **aggressivedriving** field, **enginetempanomaly**, and **oilanomaly** fields to **Fields**.

    ![Screenshot displaying where to drag the fields onto the multi-row card settings.](media/pbi-card-fields.png 'Multi-row card settings')

19. Select the **Format** tab in the multi-row card settings, then expand **Data labels**. Set the **Text size** to 30. Expand **Category labels** and set the **Text size** to 12.

    ![Screenshot of the format tab.](media/pbi-card-format.png 'Multi-row card format')

20. Your multi-row card should look similar to the following:

    ![The multi-row card on the report.](media/pbi-card.png 'Multi-row-card')

21. Select **Save** on the upper-right of the page.

    ![The save button is highlighted.](media/pbi-save.png 'Save')

22. Enter a name, such as "Vehicle Anomalies", then select **Save**.

    ![Screenshot of the save dialog.](media/pbi-save-dialog.png 'Save dialog')

23. Now let's add this report to a dashboard. Select **Pin Live Page** on the upper-right of the page.

    ![The Pin Live Page button is highlighted.](media/pbi-live.png 'Pin Live Page')

24. Select **New dashboard**, then enter a name, such as "Vehicle Anomalies Dashboard". Select **Pin live**. When prompted select the option to view the dashboard. Otherwise, you can find the dashboard under My Workspace on the left-hand menu.

    ![Screenshot of the pin to dashboard dialog.](media/pbi-live-dialog.png 'Pin to dashboard dialog')

25. The live dashboard will automatically refresh and update while data is being captured. You can hover over any point on a chart to view information about the item. Select one of the regions in the legend above the average speed chart. All other charts will filter by that region automatically. Click on a blank area of the chart to clear the filter.

    ![The live dashboard view.](media/pbi-dashboard.png 'Dashboard')
