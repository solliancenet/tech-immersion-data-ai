# Data & AI Tech Immersion Workshop – Product Review Guide and Lab Instructions



## Day 1, Experience 6 - Simplifying data flows with Azure Data Factory

- [Data & AI Tech Immersion Workshop – Product Review Guide and Lab Instructions](#data--ai-tech-immersion-workshop-%E2%80%93-product-review-guide-and-lab-instructions)
  - [Day 1, Experience 6 - Simplifying data flows with Azure Data Factory](#day-1-experience-6---simplifying-data-flows-with-azure-data-factory)
  - [Technology overview](#technology-overview)
  - [Scenario overview](#scenario-overview)
    - [Task 1: Create the data flow](#task-1-create-the-data-flow)
    - [Task 2: Previewing the trip data](#task-2-previewing-the-trip-data)
    - [Task 3: Configuring the tripfare data source](#task-3-configuring-the-tripfare-data-source)
    - [Task 4: Join the data sources](#task-4-join-the-data-sources)
    - [Task 5: Create an aggregate](#task-5-create-an-aggregate)
    - [Task 6: Add the destination sink](#task-6-add-the-destination-sink)
    - [Task 7: Run the pipeline](#task-7-run-the-pipeline)
  - [Wrap-up](#wrap-up)
  - [Additional resources and more information](#additional-resources-and-more-information)

## Technology overview

Mapping Data Flows in ADF provide a way to transform data at scale without any coding required. You can design a data transformation job in the data flow designer by constructing a series of transformations. Start with any number of source transformations followed by data transformation steps. Then, complete your data flow with sink to land your results in a destination.

## Scenario overview

Like many organizations, ContosoAuto generates data from numerous system, each of which has its own location and format, including structured, unstructured, and semi-structured data. They would like the ability to combine and analyze these disparate datasets in order to gain actionable insights that can help them operate their business more efficiently.

In this experience, ​​you will see how you can use the capabilities of Azure Data Factory (ADF) to visually create data flows that read, transform and write data, perform data wrangling or data exploration with no code. 

##Exercise 1: Using ADF Mapping Data Flows to visually configure data transformation pipelines

###  Task 1: Create the data flow

1.  Browse to the Azure Portal and navigate to the `tech-immersion` resource group and select the Azure Data Factory instance `tech-immersion-data-factory`.
   
2.  On the Overview blade, select **Author & Monitor**.

    ![The author and monitor button in the Azure Data Factory overview page](./media/01-author-monitor.png)

3.  Select the Author tab from the left hand side.

    ![The author tab](media/02-author-tab.png)

4.  On the Factory Resources panel, select **+** and then choose **Pipeline**. 

    ![Adding a new pipeline](media/03-factory-add-pipeline.png)

5.  On the Activities panel, in the Search Activities text box type `data flow`. 

    ![Searching for the data flow activity](media/04-add-data-flow.png)

6.  Drag the Data Flow entry that appears to the right on to the pipeline design surface and drop it. 

    ![Dragging the data flow activity on to the pipeline design surface](media/05-drag-drop-data-flow.png)

7.  In the Adding Data Flow panel, select **Create new Data Flow** and provide the Data Flow Name field a value of `FleetCalculations`. Select **Finish**.

    ![Setting the values for the data flow](media/07-adding-data-flow.png)

8.  In the tool tip that appears, click **Finish**.

9.  Select Add Source on the data flow design surface.

    ![The Add Source drop target](media/07-adding-data-flow.png)

10.  In the tool tip that appears, read the guide text and select **Next** until it is dismissed.

11.  Near the top of the window, toggle the Data Flow Debug to on and then select OK in the dialog that appears. This will provision a small Databricks cluster behind the scenes that you will use to inspect your data as you are building the data flow. It takes about 5 minutes to be ready, but you can continue on with the next steps while it starts up.

     ![Toggling the Data Flow Debug on](media/10-data-flow-debug.png)

12.  In the Source Setting tab, select **+ New** next to *Source dataset*.

     ![Configuring the Source Settings](media/10-new-source-dataset.png)

13.  On the New Data Set panel, select **Azure Blob Storage** and then select **Continue**.

     ![Selecting Blob Storage as the New Dataset source](media/12-blob-storage-source.png)

14.  On the Select Format panel, select **DelimitedText** and select **Continue**.

     ![Selecting DelimitedText as the format](media/13-text-source.png)

15.  On Set Properties, set the Name to `trip_data_input` and the under Linked service select `+ New`.

     ![Setting the name and linked service](media/14-set-properties.png)

16.  On the New Linked Service blade, under *Azure Subscription*, select your Azure Subscription. Under *Storage account name*, select the storage account that begins with `samplefiles`. Select **Finish**.

     ![Selecting the subscription and storage account](media/15-linked-service.png)

17.  On Set Properties, for the File Path configure the following values:
     - Container: `data`
     - File: `trip_data_1.csv`
     - First row as header: `checked`

     ![Setting the file path to the input file](media/16-set-file-path.png)

18.   You now have your source CSV file configured in the data flow.  

###  Task 2: Previewing the trip data

1.  Check if the Data Flow Debug toggle has turned solid blue and shows a green dot. If it has your cluster, is ready and you can continue. If not, please wait at this step until is ready before continuing.

     ![The Data Flow Debug indicates the cluster is ready](media/17-debug-ready.png)


2.  With `source1` still selected in the data flow designer, select Source Settings in the property panel and set the Output stream name to `tripdata`.

     ![Setting the output stream name to tripdata](media/18-source-name.png)

3.  In the property panel, select **Projection** and then select **Detect data type**. This will sample your data and automatically set the data type for each of the columns. 

     ![The detect data type button](media/19-detect-data-button.png)

4.  The process will run for a few moments. When it completes Review the assignments in the table.

     ![The data types auto-assigned](media/20-data-types-assigned.png)

5.  You can preview the data output from any data flow node, by selecting that node in the designer and then selecting the **Data Preview** tab in the property panel and selecting **Refresh**.

![Selecting the refresh button](media/21-data-preview-refresh.png)

6.  In a few moment the data preview will load with the preview of the output data for that node.

![The preview showing some rows of the data](media/22-data-preview-loaded.png)


###  Task 3: Configuring the tripfare data source

1.  In the Data Flow design surface, below tripdata, select **Add Source**.

     ![Selecting Add Source](media/23-add-second-source.png)

2.  For the Output stream name provide `tripfare`, and then select + New to the right of Source dataset.

3.  On New Dataset, select **Azure Blob Storage** and select **Continue**.

4.  On Select Format, choose **DelimitedText** and select **Continue**.

5.  For Set Properties, for Name set it to `trip_fare_input`, for the Linked service select `AzureBlobStorage1` and for the File Path provide the following and select **Finish**: 

     - Container: `data`
     - File: `trip_fare_1.csv`
     - First row as header: `checked`


     ![The properties for the trip fare input](media/24-properties-tripfare.png)

7.  With the tripfare node selected, in the property panel select Projection and then select Detect data type to set the schema for this data source.

     ![Viewing the auto assigned data types for the tripfare source](media/25-tripfare-datatypes.png)

8.  At this point you should have both the tripdata and tripfare data sources on the design surface.

     ![Both sources shown on the designer](media/25-trip-sources.png)


### Task 4: Join the data sources

1.  Select the **+** on the bottom-right of the tripdata source and then select **Join**.

     ![Selecting join](media/26-add-join.png)

2.  In the property panel, under Join Settings, for Right stream select tripfare. Leave the Join type as `Inner`. For the Join conditions match up the fields as follows (click the **+** that appears when hovering over the condition row to add new condition):

     - hack_license : hack_license
     - medallion : medallion
     - vendor_id : vendor_id
     - pickup_datetime : pickup_datetime

     ![The completed join settings](media/27-completed-join-settings.png)

### Task 5: Create an aggregate

1.  Select the **+** to the bottom-right of the Join1 node, and select **Aggregate**.

![Add an aggregate](media/28-add-aggregate.png)

2.  On the property panel, under **Aggregate Settings**, select **Group By**. For Join1's columns select `vendor_id`.

![Aggregate settings](media/29-aggregate-settings.png)

3.  Select the **Aggregates** tab (to the right of **Group By**). Add the following aggregates (use the + that appears when hovering over each row to add another column):

     - passenger_count : round(sum(passenger_count), 2)
     - trip_time_in_secs : round(sum(trip_time_in_secs)/60, 2)
     - trip_distance : round(sum(trip_distance), 2)
     - TotalTripFare : round(sum({ total_amount}), 2)

     ![Configured aggregates](media/30-trip-aggregates.png)

4.  At this point your data flow should look like the following:

![Data flow showing the two sources, the join and the aggregate](media/31-flow-snapshot.png)

### Task 6: Add the destination sink

1.  Select the **+** to the bottom-right of the Aggregate1 node, and select **Sink**.

![Add sink](media/32-add-sink.png)

2.  Select **Finish** on the tooltip that appears.

3.  With sink1 selected, on the property panel, select **+ New next** to * Sink dataset*.

4.  On New Dataset, select **Azure Blob Storage** and select **Continue**.

5.  On Select Format, select **DelimitedText** and select **Continue**.

6.  For Set Properties, for Name set it to `vendorstats`, for the Linked service select `AzureBlobStorage1` and for the File Path provide the following and select **Finish**:

     - Container: `data`
     - File: Leave blank

     ![The properties for the vendorstats output](media/33-set-vendorstats-props.png)

7.  Save your data flow and pipeline by selecting **Publish All**. It will take a few moments to publish.

![The publish all button](media/34-publish-all.png)

### Task 7: Run the pipeline

1.  From the Factory Resources panel, select **pipeline1**. 

![Select pipeline1 in the panel](media/34-select-pipeline1.png)

2.  From the button bar select **Add trigger**, **Trigger Now** and then select **Finish** to execute your pipeline and the data flow it contains.

![Selecting trigger now](media/35-trigger-now.png)

3.  Select **Monitor** from the tabs on the left. Watch the status of your pipeline as it runs. Select **Refresh** at any point to update the listing. It should take **about 7 minutes** to completely execute your pipeline. 

4.  When the pipeline has completed, navigate to the `tech-immersion` resource group, select the `samplefiles` storage account and select Blobs and then the `data` folder.

![Select blobs in the Azure Storage account](media/37-select-blobs.png)

5.  You should see a new file that is the output of your pipeline (one CSV file is output per partition of the data)!

![Showing the listing of part CSV files](media/38-output-csv-files.png)

## Wrap-up

In this experience, ​​you used Azure Data Factory (ADF) and mapping data flows to create a data processing pipeline using the visual designer.

## Additional resources and more information

To continue learning and expand your understanding use the links below.

- [Azure Data Factory documentation](https://docs.microsoft.com/en-us/azure/data-factory/)
