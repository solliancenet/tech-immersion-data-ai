# Data & AI Tech Immersion Workshop – Product Review Guide and Lab Instructions

## Day 1, Experience 1 - Handling Big Data with SQL Server 2019 Big Data Clusters

- [Data & AI Tech Immersion Workshop – Product Review Guide and Lab Instructions](#Data--AI-Tech-Immersion-Workshop-%E2%80%93-Product-Review-Guide-and-Lab-Instructions)
  - [Day 1, Experience 1 - Handling Big Data with SQL Server 2019 Big Data Clusters](#Day-1-Experience-1---Handling-Big-Data-with-SQL-Server-2019-Big-Data-Clusters)
  - [Technology overview](#Technology-overview)
  - [Scenario overview](#Scenario-overview)
  - [Experience requirements](#Experience-requirements)
  - [Before the lab: Connecting to SQL Server 2019](#Before-the-lab-Connecting-to-SQL-Server-2019)
    - [Connect with Azure Data Studio](#Connect-with-Azure-Data-Studio)
    - [Connect with SQL Server Management Studio](#Connect-with-SQL-Server-Management-Studio)
  - [Task 1: Query and join data from flat files, data from external database systems, and SQL Server](#Task-1-Query-and-join-data-from-flat-files-data-from-external-database-systems-and-SQL-Server)
  - [Task 2: Train a machine learning model, score and save data as external table](#Task-2-Train-a-machine-learning-model-score-and-save-data-as-external-table)
  - [Task 3: Mounting an Azure Data Lake Gen2 Storage Account to SQL Server 2019 Big Data Cluster using HDFS Tiering](#Task-3-Mounting-an-Azure-Data-Lake-Gen2-Storage-Account-to-SQL-Server-2019-Big-Data-Cluster-using-HDFS-Tiering)
  - [Wrap-up](#Wrap-up)
  - [Additional resources and more information](#Additional-resources-and-more-information)

## Technology overview

SQL Server 2019 brings innovative security and compliance features, industry leading performance, mission-critical availability, and advanced analytics to all data workloads, now with support for big data built-in.

SQL Server 2019 is a hub for data integration. Data virtualization allows queries across relational and non-relational data without movement or replication. The enhanced PolyBase feature of SQL Server 2019 is able to connect to Hadoop clusters, Oracle, Teradata, MongoDB, and more.

Customers will be able to deliver transformational insights over structured and unstructured data with the power of SQL Server, Hadoop and Spark. SQL Server 2019 big data clusters offer scalable compute and storage composed of SQL Server, Spark and HDFS. Big data clusters will also cache data in scale-out data marts.

SQL Server 2019 is a complete AI platform to train and operationalize R and Python models in SQL Server Machine Learning Services or Spark ML using Azure Data Studio notebooks.

SQL Server 2019 will give customers and ISVs the choice of programming language and platform. They will be able to build modern applications with innovative features using .NET, PHP, Node.JS, Java, Python, Ruby, and more – and deploy the application on either Windows, Linux, or containers both on-premises and in the cloud. Application developers are now able to run Java code on SQL Server and store and analyze graph data.

SQL Server 2019 allows customers to run real-time analytics on operational data using HTAP (Hybrid Transactional and Analytical Processing), leverage the in-memory technologies for faster transactions and analytical queries, and get higher concurrency and scale through persistent memory.

Intelligent Query Processing features in SQL Server 2019 improve scaling of queries, and Automatic Plan Correction resolves performance problems.

SQL Server 2019 enables several layers of security including protection of computations in Always Encrypted secure enclaves. Customers can track compliance with sophisticated tools such as Data Discovery & Classification labeling for GDPR and Vulnerability Assessment tool.

For High Availability and Disaster Recovery, SQL Server 2019 now supports up to eight secondary replicas in an Always On Availability Group. Customers can also run Always On Availability Groups on containers using Kubernetes.

SQL Server 2019 also has powerful tools for Business Intelligence including Analysis Services and Power BI Report Server which provide visual data exploration and interactive analysis of business data.

## Scenario overview

Contoso Auto stores data in several data stores, including relational databases, NoSQL databases, data warehouses, and unstructured data stored in a data lake. They have heard of data virtualization in SQL Server 2019, and are interested to see whether this feature will allow them to more easily access their data stored in these disparate locations. They have heard of the new Big Data Clusters that can be scaled out to handle their Big Data workloads, including machine learning tasks and advanced analytics. They are also interested in any performance improvements against their internal SQL tables by moving to 2019, since the overall amount of data is growing at a rapid pace.

This experience will highlight the new features of SQL Server 2019 with a focus on Big Data Clusters and data virtualization. You will gain hands-on experience with querying both structured and unstructured data in a unified way using T-SQL. This capability will be illustrated by joining different data sets, such as product stock data in flat CSV files in Azure Storage, product reviews stored in Azure SQL Database, and transactional data in SQL Server 2019 for exploratory data analysis within Azure Data Studio. This joined data will be prepared into a table used for reporting, highlighting query performance against this table due to intelligent query processing. With the inclusion of Apache Spark packaged with Big Data Clusters, it is now possible to use Spark to train machine learning models over data lakes and use those models in SQL Server in one system. You will learn how to use Azure Data Studio to work with Jupyter notebooks to train a simple model that can predict vehicle battery lifetime, score new data and save the result as an external table. Finally, you will experience the data security and compliance features provided by SQL Server 2019 by using the Data Discovery & Classification tool in SSMS to identify tables and columns with PII and GDPR-related compliance issues, then address the issues by layering on dynamic data masking to identified columns.

## Experience requirements

Before you begin this lab, you need to find the following information on the Tech Immersion Mega Data & AI Workshop On Demand Lab environment details page, or the document provided to you for this experience:

- SQL Server 2019 Big Data Cluster IP address and port number: `SQL SERVER_2019_CLUSTER URL`
- SQL username: `SQL 2019 Big Data Cluster username`
- SQL password: `SQL 2019 Big Data Cluster password`
- Sales database name (your unique copy): `SALES DB`
- Azure SQL Database server: `AZURE DATABASE SERVER`
- Azure SQL Database name: `DATABASE NAME`
- Azure SQL Database username: `DATABASE USER`
- Azure SQL Database password: `DATABASE PASSWORD`

## Before the lab: Connecting to SQL Server 2019

Follow the steps below to connect to your SQL Server 2019 cluster with both Azure Data Studio and SQL Server Management Studio (SSMS).

### Connect with Azure Data Studio

A link to Azure Data Studio should already be on the desktop of the VM. If not, follow the instructions in Step 1 below.

![Azure Data Studio is highlighted on the desktop.](media/ads-desktop.png 'Desktop')

1.  On the bottom-left corner of your Windows desktop, locate the search box next to the Start Menu. Type **Azure Data Studio**, then select the Azure Data Studio desktop app in the search results.

    ![The search box has "Azure Data Studio" entered into it and the desktop app is highlighted in the results.](media/launch-azure-data-studio.png 'Launch Azure Data Studio')

    > **Please note:** If Azure Data Studio prompts you to update, please **do not apply** the update at this time. The lab has been tested with the software and library versions loaded in the provided environment.

2.  Within Azure Data Studio, select **Servers** from the top of the left-hand menu, then select **New Connection** from the top toolbar to the right of the menu.

    ![The Servers menu icon is selected, as well as the new connection icon.](media/ads-new-connection-link.png 'Azure Data Studio')

3.  Within the Connection dialog, configure the following:

    - **Connection type:** Select Microsoft SQL Server.
    - **Server:** Enter the IP address, followed by port number `31433` to the SQL Server 2019 Big Data cluster. Use the value from the `SQL SERVER_2019_CLUSTER URL` for this from the environment documentation. It should have a format of IP separated by a comma from the port, such as: `11.122.133.144,31433`.
    - **Authentication type:** Select SQL Login.
    - **Username:** Enter `sa`.
    - **Password:** Enter the password provided to you for this lab, you can find this value documented as `SQL 2019 Big Data Cluster password`.
    - **Remember password:** Checked.
    - Leave all other options at their default values.

    ![The Connection form is filled out with the previously mentioned settings entered into the appropriate fields.](media/ads-new-connection.png 'Azure Data Studio - New Connection')

4.  Click **Connect**.

### Connect with SQL Server Management Studio

1.  On the bottom-left corner of your Windows desktop, locate the search box next to the Start Menu. Type **SQL Server Management Studio**, then select the SQL Server Management Studio desktop app in the search results.

    ![The search box has "SQL Server Management Studio" entered into it and the desktop app is highlighted in the results.](media/launch-ssms.png 'Launch SQL Server Management Studio')

2.  Within the Connection dialog that appears, configure the following:

    - **Server name:** Enter the IP address, followed by port number `31433` to the SQL Server 2019 Big Data cluster. Use the value from the `SQL SERVER_2019_CLUSTER URL` for this from the environment documentation. It should have a format of IP separated by a comma from the port, such as: `11.122.133.144,31433`.
    - **Authentication:** Select SQL Server Authentication.
    - **Login:** Enter `sa`.
    - **Password:** Enter the password provided to you for this lab, you can find this value documented as `SQL 2019 Big Data Cluster password`.
    - **Remember password:** Checked.

    ![The Connect form is filled out with the previously mentioned settings entered into the appropriate fields.](media/ssms-connection.png 'SQL Server Management Studio - Connect')

3.  Click **Connect**.

## Task 1: Query and join data from flat files, data from external database systems, and SQL Server

One of the key new features of SQL Server 2019 is data virtualization. What this means is that you can _virtualize_ external data in a SQL Server instance, regardless of source, location, and format, so that it can be queried like any other table, or sets of tables, within your SQL Server instance. In essence, data virtualization helps you create a single "virtual" layer of data from these disparate sources, providing unified data services to support multiple applications and users. A more familiar term we could use is data lake, or perhaps data hub. Unlike a typical data lake, however, you do not have to move data out from where it lives, yet you can still query that data through a consistent interface. This is a huge advantage over traditional ETL (extract-transform-load) processes where data must be moved from its original source to a new destination, oftentimes with some data transformation or mapping. This causes delays, extra storage, additional security, and a fair amount of engineering in most cases. With data virtualization, no data movement is required, which means the data sets are up-to-date, and it is possible to query and join these different data sources through these new capabilities, thanks to the use of new [PolyBase](https://docs.microsoft.com/sql/relational-databases/polybase/polybase-guide?view=sql-server-ver15) connectors. The data sources you can connect to include Cosmos DB, SQL Server (including Azure SQL Database), Oracle, HDFS (for flat files), and DB2.

![Data Virtualization compared to traditional ETL.](media/data-virtualization-vs-etl.png 'ETL vs. Data Virtualization')

The image to the left represents traditional data movement using ETL. Compare that to data virtualization, which does not require data movement and provides a unified layer over top of existing data sources.

In this task, you will experience how to configure data virtualization in SQL Server 2019 by joining data in a single query from a SQL Server 2019 table, an external file stored in HDFS (Hadoop Filesystem), and an external database.

Learn more about using data virtualization with [relational data sources](https://docs.microsoft.com/sql/relational-databases/polybase/data-virtualization?toc=%2fsql%2fbig-data-cluster%2ftoc.json&view=sql-server-ver15) and [CSV files](https://docs.microsoft.com/sql/relational-databases/polybase/data-virtualization-csv?view=sql-server-ver15), using the External Table Wizard.

To start, we will use the External Table Wizard in Azure Data Studio to connect to an external Azure SQL Database.

1.  Open Azure Data Studio and connect to your SQL Server 2019 cluster, following the [connection steps](#connect-with-azure-data-studio) above.

2.  Expand the Databases folder, right-click on the **sales_YOUR_UNIQUE_IDENTIFIER** database, then select **Create External Table**. The `YOUR_UNIQUE_IDENTIFIER` portion of the name is the unique identifier assigned to you for this lab.

    ![The sales database and the Create External Table sub-menu item are highlighted.](media/ads-create-external-table-sales.png 'Create External Table')

3.  Select the **SQL Server** data source type, then click **Next**.

    ![The SQL Server data source type is selected.](media/ads-external-table-wizard-data-source-type.png 'Select data source type')

4.  The next step is to create a database master key, if it does not already exist. This secures the credentials used by an external data source. Enter `MySecure@MasterKey1` in the **Password** and **Confirm Password** fields. If you see a message stating that a master key already exists, you may skip this step. Click **Next**.

    ![The Master Key step is displayed.](media/ads-external-table-wizard-master-key.png 'Master Key')

5.  Now, enter the credentials provided to you for the **CA_Commerce** Azure SQL Database within the following fields:

    - **External Data Source Name:** Enter the string "SQLReviews".
    - **Server Name:** Enter the value provided to you for the Azure SQL Server name. The name should end with `.database.windows.net` (you can find this value in the lab environment details page, with the label `AZURE DATABASE SERVER`).
    - **Database Name:** Enter "CA_Commerce".
    - **Choose Credential:** Select "-- Create New Credential --".
    - **New Credential Name:** Enter "SQLCred".
    - **Username:** Enter the Azure SQL Server username provided to you for this lab (look to the lab environment details for the label `DATABASE USER`).
    - **Password:** Enter the Azure SQL Server password provided to you for this lab (look to the lab environment details for the label `DATABASE PASSWORD`).

    ![The external data source connection form is filled out with the previously mentioned settings entered into the appropriate fields.](media/ads-external-table-wizard-data-source.png 'Create a connection to your Data Source')

6.  Click **Next**. This process will take a few moments while the External Table Wizard attempts to connect to your data source.

7.  The next screen allows you to configure external table mapping and select the tables for which you want to create external views. Expand the **CA_Commerce** database node, then expand Tables, and check the box next to the **dbo.Reviews** table. Click on the table name to highlight it as well. It is here where you can rename the external table if you wish. For now, just click **Next**.

    ![The external tables are listed and the Reviews table is checked and selected.](media/ads-external-table-wizard-table-mapping.png 'External tables')

8.  In the Summary page that follows, you can see the name of the database scoped credential and external data source objects to be created in the destination database. Here you can click Generate Script to view the SQL script that will run to create the external table. Instead, click **Create**.

    ![A screenshot of the summary is displayed.](media/ads-external-table-wizard-summary.png 'Summary')

9.  After a few moments, a "Create External Table succeeded" message will display.

    ![The Create External Table succeeded message is displayed.](media/ads-external-table-wizard-succeeded.png 'Create External Table succeeded message')

10. Select the Servers link (Ctrl+G) on the left-hand menu, then expand the Tables list underneath your **sales_YOUR_UNIQUE_IDENTIFIER** database and find the **dbo.Reviews (External)** table. If you do not see it, right-click on the Tables folder, then select Refresh. The "(External)" portion of the table name denotes that it is a virtual data object that was added as an external table.

    ![The Reviews external table is displayed in the sales tables list.](media/ads-reviews-table-in-list.png 'Reviews external table')

11. Right-click the **dbo.Reviews (External)** table, then select the **Select Top 1000** menu option to display the table contents.

    ![The Select Top 1000 rows menu item is highlighted.](media/ads-reviews-select-top-1000.png 'Select Top 1000')

12. You should see a SQL query selecting the top 1000 records from the Reviews table and its results. The interesting thing to note is that the query selects the table and fields using the same syntax you would use to select from any other table in the sales database. The fact that the Reviews table is external is completely seamless and transparent to the user. This is the power of data virtualization in SQL Server 2019.

    ![The Reviews query and results are displayed.](media/ads-reviews-query-results.png 'Reviews query results')

    ```sql
    SELECT TOP (1000) [product_id]
        ,[customer_id]
        ,[review]
        ,[date_added]
    FROM [sales_YOUR_UNIQUE_IDENTIFIER].[dbo].[Reviews]
    ```

13. The next data source we will be virtualizing is a CSV file that was uploaded to HDFS.

14. Within Azure Data Studio, scroll down below the list of SQL Server 2019 databases to find the **Data Services** folder. Expand that folder, expand the **HDFS** folder, then expand the **data** subfolder. Right-click on the `stockitemholdings.csv` file, then select **Create External Table From CSV Files**.

    ![The CSV file and the Create External Table From CSV Files menu item are highlighted.](media/ads-create-external-table-csv.png 'Create External Table From CSV Files')

15. In the Create External Table from CSV dialog, confirm that the **sales** database is selected and that the name of the external table is **stockitemholdings**.

    ![The previously mentioned form is displayed.](media/ads-external-table-csv-wizard-active-connection.png 'Active SQL Server connections')

16. Click **Next**.

17. The next step displays a preview of the first 50 rows CSV data for validation. Click **Next** to continue.

    ![A preview of the CSV data is displayed.](media/ads-external-table-csv-preview.png 'Preview Data')

18. In the next step, you will be able to modify the columns of the external table you intend to create. You are able to alter the column name, change the data type, and allow for Nullable rows. For now, leave everything as-is and click **Next**.

    ![The Modify Columns step is displayed.](media/ads-external-table-csv-modify.png 'Modify Columns')

19. Verify that everything looks correct in the Summary step, then click **Create Table**.

    ![The Summary step is displayed.](media/ads-external-table-csv-create.png 'Summary')

20. As with the previous external table you created, a "Create External Table succeeded" dialog will appear under your task history in a few moments. Select the Servers link (Ctrl+G) on the left-hand menu, then expand the Tables list underneath your **sales** database and find the **dbo.stockitemholdings (External)** table. If you do not see it, right-click on the Tables folder, then select Refresh. **Right-click** the **dbo.stockitemholdings (External)** table, then select **Select Top 1000** from the context menu.

    ![The Select Top 1000 rows menu item is highlighted.](media/ads-stockitemholdings-select-top-1000.png 'Select Top 1000')

21. Just as before, you should see a SQL query selecting the top 1000 rows and its query results, this time from the `stockitemholdings` table. Again, the SQL query is the same type of query you would write to select from a table internal to the sales database.

    ![The stockitemholdings query and results are displayed.](media/ads-stockitemholdings-results.png 'Stockitemholdings results')

    ```sql
    SELECT TOP (1000) [StockItemID]
        ,[QuantityOnHand]
        ,[BinLocation]
        ,[LastStocktakeQuantity]
        ,[LastCostPrice]
        ,[ReorderLevel]
        ,[TargetStockLevel]
        ,[LastEditedBy]
        ,[LastEditedWhen]
    FROM [sales_YOUR_UNIQUE_IDENTIFIER].[dbo].[stockitemholdings]
    ```

22. Now that we have our two external tables added, we will now join those two external tables and two internal tables with a new SQL query to demonstrate how you can seamlessly combine all these data sources without having to copy any files or with separate queries or additional processing of that data. **Right-click** the **sales_YOUR_UNIQUE_IDENTIFIER** database, then select **New Query**.

    ![The sales database and New Query menu item are highlighted.](media/ads-new-query.png 'New Query')

23. Paste the following into the new query window:

    ```sql
    SELECT i.i_item_sk AS ItemID, i.i_item_desc AS Item, c.c_first_name AS FirstName,
      c.c_last_name AS LastName, s.QuantityOnHand, r.review AS Review, r.date_added AS DateReviewed
    FROM dbo.item as i
    JOIN dbo.Reviews AS r ON i.i_item_sk = r.product_id
    JOIN dbo.customer AS c ON c.c_customer_sk = r.customer_id
    JOIN dbo.stockitemholdings AS s ON i.i_item_sk = s.StockItemID
    ```

24. Click the **Run** button above the query window to execute.

    ![The Run button above the query window is highlighted.](media/ads-run.png 'Run')

25. At the bottom of the query window, you will see results that include columns from the four data sources.

    ![Query results from the four data sets.](media/ads-query-results.png 'Query results')

## Task 2: Train a machine learning model, score and save data as external table

In this task, you will use Azure Data Studio to execute a notebook that will enable you to train a model to predict the battery lifetime, apply the model to make batch predictions against a set of vehicle telemetry and save the scored telemetry to an external table that you can query using SQL.

1. In Azure Data Studio, click **File**, then **Open File...**.

2. In the folder browser dialog, navigate to the **C:\lab-files\data\1** folder and select **predict-battery-life-with-sqlbdc.ipynb**.

3. When the notebook opens, you need to select the **Kernel** you would like to use to run the notebook. Locate the **Kernel** dropdown in the toolbar above the notebook, then select **Python 3**.

   ![The Python 3 kernel is selected.](media/ads-notebook-select-kernel.png 'Kernel dropdown')

4. Follow the instructions in the notebook and return to the next step after you have completed the notebook.

   > There may be a kernel error pertaining to there not being a valid SQL connection when you open the notebook. If this happens, close the notebook and Azure Data Studio, then re-launch, reconnect, then re-open the notebook.

5. In Azure Data Studio, under Servers, expand your Big Data Cluster, `Data Services`, `HDFS`, `data`.

6. Right click the `data` folder and select `Refresh` to see the newly created folder.

   ![Refresh data](media/task02-refresh-data.png 'Refresh data')

7. You should see `battery-life-YOUR_UNIQUE_IDENTIFIER.csv` as a folder (where `YOUR_UNIQUE_IDENTIFIER` is your assigned identifier), expand it and then right click on the CSV file whose name starts with `part-00000-` and select `Create External Table From CSV Files`.

   ![Create External Table](media/task02-create-external-menu.png 'Create External Table')

8. In Step 1 of the wizard, select the `sales` database and for the `Name for new external table` field provide **battery-life-predictions**. Click **Next**.

   ![Step 1 of the wizard is displayed.](media/ads-predictions-csv-wizard-step1.png 'Step 1')

9. On Step 2, Preview Data, click **Next**.

10. On Step 3, for the column `Car_Has_EcoStart` set the Data Type to **char(10)**. Click **Next**.

    ![Step 3 of the wizard is displayed.](media/ads-predictions-csv-wizard-step3.png 'Step 3')

11. On Step, click **Create Table**. Your predictions are now available for SQL querying in the battery-life-predictions table in the sales database.

12. In Azure Data Studio, Servers, expand your Big Data Cluster, `Databases`, `sales_YOUR-UNIQUE-IDENTIFIER`, right click `Tables` and then select `Refresh`.

    ![Refresh sales](media/task02-refresh-sales.png 'Refresh sales')

13. Expand `tables`, right click `battery-life-prediction` and select `query` to view the data contained by the external table.

    ![Select Top 1000](media/task02-select-top.png 'Select Top 1000')

14. The vehicle telemetry along with predictions will appear. These are queried from the external table which is sourced from the CSV you created using the notebook.

    ![View data](media/task02-view-data.png 'View data')

## Task 3: Mounting an Azure Data Lake Gen2 Storage Account to SQL Server 2019 Big Data Cluster using HDFS Tiering

With tiering, applications can seamlessly access data in a variety of external stores as though the data resides in the local HDFS. This allows you to interact with the files in Azure Data Lake Store Gen2 as if they were local files. You can either use an Azure Storage access key or an Azure Active Directory User Account to gain permission to the files. For this lab, we will use the access key.

1. In Windows, open PowerShell.

   ![Search for PowerShell .](media/powershell.png 'SQL Server Management Studio - Connect')

2. Connect to your Microsoft SQL Server 2019 Big Data Cluster:

   ```python
   mssqlctl login -e https://<SQL SERVER Controller IP ADDRESS>:30080
   ```

   - You will be prompted for your big data cluster name.
   - The user name is admin
   - The password is MySQLBigData2019

3. Create an empty text file named filename.creds in your temp folder on the c:\ drive. Add this line as the contents:

   ```text
   fs.azure.abfs.account.name=ikedatabricks.dfs.core.windows.net
   fs.azure.account.key.ikedatabricks.dfs.core.windows.net=HUYPk/VUjdYzkCvrKXTgFBObt5VQcp5DCY7C9KiSHX42lv65mjmBFmKFVTLy7Z7suQ0WV44mncuUOvnE8NkxGg==
   ```

4. In PowerShell, type the following command to mount the drive

   ```powershell
   mssqlctl cluster storage-pool mount create --remote-uri abfs://databricksfiles@ikedatabricks.dfs.core.windows.net/ --mount-path /mounts/dbfiles --credential-file c:\temp\filename.creds
   ```

5. Once the storage account has been mounted, you can check the status:

   ```powershell
   mssqlctl cluster storage-pool mount status
   ```

6. Now you can use Azure Data Studio and view the files from ADLS Gen2 under your HDFS folder in your Servers pane. Look under /HDFS/mounts/dbfiles/

   ![Azure Data Studio server pane for new dbfiles folder](media/data-studio-mounts.png)

7. Now that the drive is mounted, create an external file format for CSV. Open a new query window and paste the following command:

   ```sql
     USE Sales;
     GO
     CREATE EXTERNAL FILE FORMAT csv_file
     WITH (
         FORMAT_TYPE = DELIMITEDTEXT,
         FORMAT_OPTIONS(
             FIELD_TERMINATOR = ',',
             STRING_DELIMITER = '"',
             FIRST_ROW = 2,
             USE_TYPE_DEFAULT = TRUE)
     );
   ```

8. Now create an external connection to your HDFS cluster:

   ```sql
     IF NOT EXISTS(SELECT * FROM sys.external_data_sources WHERE name = 'SqlStoragePool')
     BEGIN
       CREATE EXTERNAL DATA SOURCE SqlStoragePool
       WITH (LOCATION = 'sqlhdfs://controller-svc:8080/default');
     END
   ```

9. Now let's create two tables to two different files that exist in the storage account:


    ```sql
    CREATE EXTERNAL TABLE planes
    ("tailnum" VARCHAR(100),	"year" VARCHAR(4),	"type" VARCHAR(100)
    ,	"manufacturer" VARCHAR(100),	"model" VARCHAR(20),	"engines" BIGINT
    ,	"seats" BIGINT,	"speed" VARCHAR(20)
    ,	"engine" VARCHAR(20))
    WITH
    (
        DATA_SOURCE = SqlStoragePool,
        LOCATION = '/mounts/dbfiles/planes.csv',
        FILE_FORMAT = csv_file
    );
    GO

    CREATE EXTERNAL TABLE flights
    ("year" BIGINT, 	"month" BIGINT, 	"day" BIGINT,	"dep_time" BIGINT
        ,	"dep_delay" BIGINT,	"arr_time" BIGINT,	"arr_delay" BIGINT,	"carrier" VARCHAR(100)
        ,	"tailnum" VARCHAR(20),	"flight" VARCHAR(20),	"origin" VARCHAR(50)
        ,	"dest" VARCHAR(50),	"air_time" BIGINT,	"distance" BIGINT,
          "hour" BIGINT,	"minute" BIGINT)
    WITH
    (
        DATA_SOURCE = SqlStoragePool,
        LOCATION = '/mounts/dbfiles/flights_small.csv',
        FILE_FORMAT = csv_file
    );
    ```

11. Once the tables are created, you can interact with them like normal tables. For instance, you can run a query that joins the two tables like this:

    ```sql
    SELECT *
      FROM planes p
      JOIN flights f
        on p.tailnum = f.tailnum
    ```

## Wrap-up

Thank you for participating in the SQL Server 2019 Big Data Clusters experience! We hope you are excited about the new capabilities, and will refer back to this experience to learn more about these features.

To recap, you experienced:

1. How to minimize or remove the need for ETL through **data virtualization** with [relational data sources](https://docs.microsoft.com/sql/relational-databases/polybase/data-virtualization?toc=%2fsql%2fbig-data-cluster%2ftoc.json&view=sql-server-ver15) and [CSV files](https://docs.microsoft.com/sql/relational-databases/polybase/data-virtualization-csv?view=sql-server-ver15), by being able to query against these alongside internal SQL 2019 tables with no data movement required.
2. Training a machine learning model by running a Jupyter notebook on the Big Data cluster, then scoring data with the trained model and saving it as an external table for easy access.
3. You learned to use HDFS tiering to mount files from an Azure Data Lake Store Gen2 account, which allowed you to create tables from files as if they were local to the cluster.

## Additional resources and more information

- [What's new in SQL Server 2019 preview](https://docs.microsoft.com/en-us/sql/sql-server/what-s-new-in-sql-server-ver15?view=sql-server-ver15)
- [SQL Server 2019 big data clusters overview and architecture](https://docs.microsoft.com/en-us/sql/big-data-cluster/big-data-cluster-overview?view=sql-server-ver15)
- [How to run a sample notebook in Azure Data Studio on a SQL Server 2019 big data cluster, and leverage Spark](https://docs.microsoft.com/en-us/sql/big-data-cluster/tutorial-notebook-spark?view=sqlallproducts-allversions)
- [What is Azure Data Studio?](https://docs.microsoft.com/en-us/sql/azure-data-studio/what-is?view=sql-server-ver15)
- [Security Center for SQL Server Database Engine and Azure SQL Database](https://docs.microsoft.com/en-us/sql/relational-databases/security/security-center-for-sql-server-database-engine-and-azure-sql-database?view=sql-server-2017)
- [SQL Data Discovery and Classification tool documentation](https://docs.microsoft.com/en-us/sql/relational-databases/security/sql-data-discovery-and-classification?view=sql-server-2017)
- [Intelligent query processing in SQL databases](https://docs.microsoft.com/en-us/sql/relational-databases/performance/intelligent-query-processing?view=sql-server-2017)
- [What's new in SQL Server Machine Learning Services](https://docs.microsoft.com/en-us/sql/advanced-analytics/what-s-new-in-sql-server-machine-learning-services?view=sql-server-ver15)
- [How to run Java code in SQL Server 2019](https://docs.microsoft.com/en-us/sql/advanced-analytics/java/extension-java?view=sql-server-ver15)
- [Learning content in GitHub: SQL Server Workshops](https://github.com/Microsoft/sqlworkshops)
- [SQL Server Samples Repository in GitHub. Feature demos, code samples etc.](https://github.com/Microsoft/sql-server-samples)

SQL Server 2019 has a new database compatibility level: `150`. When you set your database to this level, you will gain query performance benefits due to improvements to the family of intelligent query processing (QP) features in SQL Server 2019. These features improve the performance of existing workloads with minimal work on your part to implement.

A few QP features we would like to call out are:

1. [Scalar UDF inlining](https://docs.microsoft.com/sql/relational-databases/user-defined-functions/scalar-udf-inlining?view=sql-server-2017): This feature automatically transforms [scalar UDFs](https://docs.microsoft.com/sql/relational-databases/user-defined-functions/create-user-defined-functions-database-engine?view=sql-server-2017#Scalar) into relational expressions. It embeds them in the calling SQL query. This transformation improves the performance of workloads that take advantage of scalar UDFs. Scalar UDF inlining facilitates cost-based optimization of operations inside UDFs. The results are efficient, set-oriented, and parallel instead of inefficient, iterative, serial execution plans. This feature is enabled by default under database compatibility level 150.
2. [Table variable deferred compilation](https://docs.microsoft.com/sql/t-sql/data-types/table-transact-sql?view=sql-server-2017#table-variable-deferred-compilation): This feature improves plan quality and overall performance for queries that reference table variables. During optimization and initial compilation, this feature propagates cardinality estimates that are based on actual table variable row counts. This accurate row count information optimizes downstream plan operations. Table variable deferred compilation defers compilation of a statement that references a table variable until the first actual run of the statement. This deferred compilation behavior is the same as that of temporary tables. This change results in the use of actual cardinality instead of the original one-row guess.
3. [Row mode memory grant feedback](https://docs.microsoft.com/sql/relational-databases/performance/adaptive-query-processing?view=sql-server-2017#row-mode-memory-grant-feedback): A query's post-execution plan in SQL Server includes the minimum required memory needed for execution and the ideal memory grant size to have all rows fit in memory. Performance suffers when memory grant sizes are incorrectly sized. Excessive grants result in wasted memory and reduced concurrency. Insufficient memory grants cause expensive spills to disk. By addressing repeating workloads, batch mode memory grant feedback recalculates the actual memory required for a query and then updates the grant value for the cached plan. **When an identical query statement is executed**, the query uses the revised memory grant size, reducing excessive memory grants that impact concurrency and fixing underestimated memory grants that cause expensive spills to disk. Row mode memory grant feedback expands on the batch mode memory grant feedback feature by adjusting memory grant sizes for both batch and row mode operators.

Read more about [intelligent query processing](https://docs.microsoft.com/sql/relational-databases/performance/intelligent-query-processing?view=sql-server-ver15) in SQL databases.
