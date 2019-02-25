# Day 1, Experience 1 - Handling Big Data with SQL Server 2019 Big Data Clusters

Contoso Auto stores data in several data stores, including relational databases, NoSQL databases, data warehouses, and unstructured data stored in a data lake. They have heard of data virtualization in SQL Server 2019, and are interested to see whether this feature will allow them to more easily access their data stored in these disparate locations. They have heard of the new Big Data Clusters that can be scaled out to handle their Big Data workloads, including machine learning tasks and advanced analytics. They are also interested in any performance improvements against their internal SQL tables by moving to 2019, since the overall amount of data is growing at a rapid pace.

This experience will highlight the new features of SQL Server 2019 with a focus on Big Data Clusters and data virtualization. You will gain hands-on experience with querying both structured and unstructured data in a unified way using T-SQL. This capability will be illustrated by joining different data sets, such as telemetry data in flat CSV files in Azure Storage, CRM data stored in Oracle, parts data in Mongo DB, and transactional data in SQL Server for exploratory data analysis within Azure Data Studio. This joined data will be prepared into a table used for reporting, highlighting query performance against this table due to intelligent query processing. You will also learn how Docker containers are used to scale-out storage and compute to handle heavy workloads. With the inclusion of Apache Spark packaged with Big Data Clusters, it is now possible to use Spark to train machine learning models over data lakes and use those models in SQL Server in one system. You will learn how to use Azure Data Studio to work with Jupyter notebooks to train a simple model that can predict vehicle battery lifetime, then operationalize it within a stored procedure. Finally, you will experience the data security and compliance features provided by SQL Server 2019 by using the Data Discovery & Classification tool in SSMS to identify tables and columns with PII and GDPR-related compliance issues, then address the issues by layering on dynamic data masking to identified columns.

- [Day 1, Experience 1 - Handling Big Data with SQL Server 2019 Big Data Clusters](#day-1-experience-1---handling-big-data-with-sql-server-2019-big-data-clusters)
  - [Experience requirements](#experience-requirements)
  - [Before the lab: Connecting to SQL Server 2019](#before-the-lab-connecting-to-sql-server-2019)
    - [Connect with Azure Data Studio](#connect-with-azure-data-studio)
    - [Connect with SQL Server Management Studio](#connect-with-sql-server-management-studio)
    - [Enlarge the ContosoAutoDW database](#enlarge-the-contosoautodw-database)
  - [Task 1: Query and join data from flat files, data from external database systems, and SQL Server](#task-1-query-and-join-data-from-flat-files-data-from-external-database-systems-and-sql-server)
  - [Task 2: Train a machine learning model and deploy it to a SQL stored procedure](#task-2-train-a-machine-learning-model-and-deploy-it-to-a-sql-stored-procedure)
  - [Task 3: Scale out both storage and compute capabilities of a SQL cluster](#task-3-scale-out-both-storage-and-compute-capabilities-of-a-sql-cluster)
  - [Task 4: Query performance improvements with intelligent query processing](#task-4-query-performance-improvements-with-intelligent-query-processing)
  - [Task 5: Identify PII and GDPR-related compliance issues using Data Discovery & Classification in SSMS](#task-5-identify-pii-and-gdpr-related-compliance-issues-using-data-discovery--classification-in-ssms)
  - [Task 6: Fix compliance issues with dynamic data masking](#task-6-fix-compliance-issues-with-dynamic-data-masking)

## Experience requirements

- Azure subscription
- [SQL Server Management Studio](https://docs.microsoft.com/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver15) (SSMS) v17.9.1 or greater
- [Azure Data Studio](https://docs.microsoft.com/sql/azure-data-studio/download?view=sql-server-ver15)
  - [SQL Server 2019 extension](https://docs.microsoft.com/sql/azure-data-studio/sql-server-2019-extension?view=sql-server-ver15)
- SQL Server 2019 login credentials provided for your lab environment

## Before the lab: Connecting to SQL Server 2019

Follow the steps below to connect to your SQL Server 2019 cluster with both Azure Data Studio and SQL Server Management Studio (SSMS).

### Connect with Azure Data Studio

1.  On the bottom-left corner of your Windows desktop, locate the search box next to the Start Menu. Type **Azure Data Studio**, then select the Azure Data Studio desktop app in the search results.

    ![The search box has "Azure Data Studio" entered into it and the desktop app is highlighted in the results.](media/launch-azure-data-studio.png 'Launch Azure Data Studio')

2.  Within Azure Data Studio, select **Servers** from the top of the left-hand menu, then select **New Connection** from the top toolbar to the right of the menu.

    ![The Servers menu icon is selected, as well as the new connection icon.](media/ads-new-connection-link.png 'Azure Data Studio')

3.  Within the Connection dialog, configure the following:

    - **Connection type:** Select Microsoft SQL Server.
    - **Server:** Enter the IP address, followed by port number `31433`. For example: `123.123.123.123,31433`.
    - **Username:** Enter "sa".
    - **Password:** Enter the password provided to you for this lab.
    - **Remember password:** Checked.
    - Leave all other options at their default values.

    ![The Connection form is filled out with the previously mentioned settings entered into the appropriate fields.](media/ads-new-connection.png 'Azure Data Studio - New Connection')

4.  Click **Connect**.

### Connect with SQL Server Management Studio

1.  On the bottom-left corner of your Windows desktop, locate the search box next to the Start Menu. Type **SQL Server Management Studio**, then select the SQL Server Management Studio desktop app in the search results.

    ![The search box has "SQL Server Management Studio" entered into it and the desktop app is highlighted in the results.](media/launch-ssms.png 'Launch SQL Server Management Studio')

2.  Within the Connection dialog that appears, configure the following:

    - **Server name:** Enter the IP address, followed by port number `31433`. For example: `123.123.123.123,31433`.
    - **Login:** Enter "sa".
    - **Password:** Enter the password provided to you for this lab.
    - **Remember password:** Checked.

    ![The Connect form is filled out with the previously mentioned settings entered into the appropriate fields.](media/ssms-connection.png 'SQL Server Management Studio - Connect')

3.  Click **Options >>**.

4.  Select the **Additional Connection Parameters** tab. In the text area below, enter `TrustServerCertificate=True`. This is needed because the server certificates are dynamically generated for the Big Data Clusters, and are self-signed.

    ![The Additional Connection Parameters tab is selected and the TrustServerCertificate=True value is highlighted.](media/ssms-connection-additional.png 'Additional Connection Parameters')

5.  Click **Connect**.

### Enlarge the ContosoAutoDW database

The script you will execute below enlarges the ContosoAutoDW database. This helps make the impact of the intelligent QP scripts you execute in [Task 4 below](#task-4-query-performance-improvements-with-intelligent-query-processing) more pronounced.

1.  Open SQL Server Management Studio (SSMS) and connect to your SQL Server 2019 cluster. If you are unsure of how to do this, refer to [Connect with SQL Server Management Studio](#connect-with-sql-server-management-studio) at the top of this guide.

2.  Right-click on `ContosoAutoDW`, then select **New Query**. This will open a new query window into which you can paste the following queries. You may wish to reuse the same query window, replacing its contents with each SQL statement blocks below, or follow these same steps to create new query windows for each.

    ![ContosoAutoDW is selected and the New Query menu option is highlighted.](media/ssms-new-query.png 'New Query')

3.  Paste the following query into the new query window:

    ```sql
    USE ContosoAutoDW;
    GO

    /*
      Assumes a fresh restore of ContosoAutoDW.
    */

    IF OBJECT_ID('Fact.OrderHistory') IS NULL BEGIN
        SELECT [Order Key], [City Key], [Customer Key], [Stock Item Key], [Order Date Key], [Picked Date Key], [Salesperson Key], [Picker Key], [WWI Order ID], [WWI Backorder ID], Description, Package, Quantity, [Unit Price], [Tax Rate], [Total Excluding Tax], [Tax Amount], [Total Including Tax], [Lineage Key]
        INTO Fact.OrderHistory
        FROM Fact.[Order];
    END;

    ALTER TABLE Fact.OrderHistory
    ADD CONSTRAINT PK_Fact_OrderHistory PRIMARY KEY NONCLUSTERED([Order Key] ASC, [Order Date Key] ASC)WITH(DATA_COMPRESSION=PAGE);
    GO

    CREATE INDEX IX_Stock_Item_Key
    ON Fact.OrderHistory([Stock Item Key])
    INCLUDE(Quantity)
    WITH(DATA_COMPRESSION=PAGE);
    GO

    CREATE INDEX IX_OrderHistory_Quantity
    ON Fact.OrderHistory([Quantity])
    INCLUDE([Order Key])
    WITH(DATA_COMPRESSION=PAGE);
    GO

    /*
      Reality check... Starting count should be 231,412
    */
    SELECT COUNT(*) FROM Fact.OrderHistory;
    GO

    /*
      Make this table bigger (exec as desired)
      Notice the "GO 4"
    */
    INSERT Fact.OrderHistory([City Key], [Customer Key], [Stock Item Key], [Order Date Key], [Picked Date Key], [Salesperson Key], [Picker Key], [WWI Order ID], [WWI Backorder ID], Description, Package, Quantity, [Unit Price], [Tax Rate], [Total Excluding Tax], [Tax Amount], [Total Including Tax], [Lineage Key])
    SELECT [City Key], [Customer Key], [Stock Item Key], [Order Date Key], [Picked Date Key], [Salesperson Key], [Picker Key], [WWI Order ID], [WWI Backorder ID], Description, Package, Quantity, [Unit Price], [Tax Rate], [Total Excluding Tax], [Tax Amount], [Total Including Tax], [Lineage Key]
    FROM Fact.OrderHistory;
    GO 4

    /*
      Should be 3,702,592
    */
    SELECT COUNT(*) FROM Fact.OrderHistory;
    GO

    IF OBJECT_ID('Fact.OrderHistoryExtended') IS NULL BEGIN
        SELECT [Order Key], [City Key], [Customer Key], [Stock Item Key], [Order Date Key], [Picked Date Key], [Salesperson Key], [Picker Key], [WWI Order ID], [WWI Backorder ID], Description, Package, Quantity, [Unit Price], [Tax Rate], [Total Excluding Tax], [Tax Amount], [Total Including Tax], [Lineage Key]
        INTO Fact.OrderHistoryExtended
        FROM Fact.[OrderHistory];
    END;

    ALTER TABLE Fact.OrderHistoryExtended
    ADD CONSTRAINT PK_Fact_OrderHistoryExtended PRIMARY KEY NONCLUSTERED([Order Key] ASC, [Order Date Key] ASC)
    WITH(DATA_COMPRESSION=PAGE);
    GO

    CREATE INDEX IX_Stock_Item_Key
    ON Fact.OrderHistoryExtended([Stock Item Key])
    INCLUDE(Quantity);
    GO

    /*
      Should be 3,702,592
    */
    SELECT COUNT(*) FROM Fact.OrderHistoryExtended;
    GO

    /*
      Make this table bigger (exec as desired)
      Notice the "GO 2"
    */
    INSERT Fact.OrderHistoryExtended([City Key], [Customer Key], [Stock Item Key], [Order Date Key], [Picked Date Key], [Salesperson Key], [Picker Key], [WWI Order ID], [WWI Backorder ID], Description, Package, Quantity, [Unit Price], [Tax Rate], [Total Excluding Tax], [Tax Amount], [Total Including Tax], [Lineage Key])
    SELECT [City Key], [Customer Key], [Stock Item Key], [Order Date Key], [Picked Date Key], [Salesperson Key], [Picker Key], [WWI Order ID], [WWI Backorder ID], Description, Package, Quantity, [Unit Price], [Tax Rate], [Total Excluding Tax], [Tax Amount], [Total Including Tax], [Lineage Key]
    FROM Fact.OrderHistoryExtended;
    GO 2

    /*
      Should be 14,810,368
    */
    SELECT COUNT(*) FROM Fact.OrderHistoryExtended;
    GO

    UPDATE Fact.OrderHistoryExtended
    SET [WWI Order ID] = [Order Key];
    GO
    ```

4.  Click the **Execute** button on the top toolbar to run the script, or simply enter _F5_ on your keyboard.

    It will take **up to 10 minutes** to execute this query. You may continue performing the tasks below **while this runs**.

    ![The New Query window has the SQL query pasted into it, and the Execute button is highlighted.](media/ssms-execute.png 'SSMS Execute query')

5.  After the query is finished, you should see output similar to the screenshot below (highlighted in red) in the Results window. Also, the lower-right portion of the status bar shows the total execution time, in this case just under 11 minutes.

    ![The query results are highlighted, as well as the execution time.](media/ssms-execution-completed.png 'SSMS query results')

## Task 1: Query and join data from flat files, data from external database systems, and SQL Server

TBD

## Task 2: Train a machine learning model and deploy it to a SQL stored procedure

TBD

## Task 3: Scale out both storage and compute capabilities of a SQL cluster

TBD

## Task 4: Query performance improvements with intelligent query processing

In this task, you will execute a series of SQL scripts in SQL Server Management Studio (SSMS) to explore the improvements to family of intelligent query processing (QP) features in SQL Server 2019. These features improve the performance of existing workloads with minimal work on your part to implement. The key to enabling these features in SQL Server 2019 is to set the database compatibility level to `150`. You will be executing these queries against the `ContosoAutoDW` database.

Read more about [intelligent query processing](https://docs.microsoft.com/sql/relational-databases/performance/intelligent-query-processing?view=sql-server-ver15) in SQL databases.

1.  Open SQL Server Management Studio (SSMS) and connect to your SQL Server 2019 cluster. If you are unsure of how to do this, refer to [Connect with SQL Server Management Studio](#connect-with-sql-server-management-studio) at the top of this guide.

2.  Right-click on `ContosoAutoDW`, then select **New Query**. This will open a new query window into which you can paste the following queries. You may wish to reuse the same query window, replacing its contents with each SQL statement blocks below, or follow these same steps to create new query windows for each.

    ![ContosoAutoDW is selected and the New Query menu option is highlighted.](media/ssms-new-query.png 'New Query')

3.  Paste the following query into the query window. This ensures that the database compatibility level is set to `150`, which is the new compatibility level for SQL Server 2019, enabling the most recent intelligent QP features. It also creates a new user-defined function (UDF) named `customer_category` that will be called inline from the two queries that follow in order to show QP improvements on scalar UDF inlining.

    ```sql
    USE ContosoAutoDW;
    GO

    ALTER DATABASE ContosoAutoDW
    SET COMPATIBILITY_LEVEL = 150;
    GO

    ALTER DATABASE SCOPED CONFIGURATION
    CLEAR PROCEDURE_CACHE;
    GO
    /*
    Adapted from SQL Server Books Online
    https://docs.microsoft.com/en-us/sql/relational-databases/user-defined-functions/scalar-udf-inlining?view=sqlallproducts-allversions
    */
    CREATE OR ALTER FUNCTION
      dbo.customer_category(@CustomerKey INT)
    RETURNS CHAR(10) AS
    BEGIN
      DECLARE @total_amount DECIMAL(18,2);
      DECLARE @category CHAR(10);

      SELECT @total_amount =
      SUM([Total Including Tax])
      FROM [Fact].[OrderHistory]
      WHERE [Customer Key] = @CustomerKey;

      IF @total_amount < 500000
        SET @category = 'REGULAR';
      ELSE IF @total_amount < 1000000
        SET @category = 'GOLD';
      ELSE
        SET @category = 'PLATINUM';

      RETURN @category;
    END
    GO
    ```

    > Scalar UDF inlining automatically transforms [scalar UDFs](https://docs.microsoft.com/sql/relational-databases/user-defined-functions/create-user-defined-functions-database-engine?view=sql-server-2017#Scalar) into relational expressions. It embeds them in the calling SQL query. This transformation improves the performance of workloads that take advantage of scalar UDFs. Scalar UDF inlining facilitates cost-based optimization of operations inside UDFs. The results are efficient, set-oriented, and parallel instead of inefficient, iterative, serial execution plans. This feature is enabled by default under database compatibility level 150. _For more information, see [Scalar UDF inlining](https://docs.microsoft.com/sql/relational-databases/user-defined-functions/scalar-udf-inlining?view=sql-server-2017)_.

4.  Click the **Execute** button on the top toolbar to run the script, or simply enter _F5_ on your keyboard.

    ![The New Query window has the SQL query pasted into it, and the Execute button is highlighted.](media/ssms-execute-udf.png 'SSMS Execute query')

5.  After executing the query, you should see a message below that says, `Commands completed successfully.`

    ![The Messages output says Commands completed successfully.](media/ssms-completed-successfully.png 'SSMS Messages')

6.  Next, either highlight and delete everything in the query window, or repeat step 2 above to open a new query window.

7.  The next query selects the top 100 rows from the `Customer` dimension table, calling the UDF you created inline for each row. It uses the `DISABLE_TSQL_SCALAR_UDF_INLINING` hint to disable the new scalar UDF inlining QP feature. Paste the following query into the the empty query window. **Do not execute yet**.

    ```sql
    USE ContosoAutoDW;
    GO

    -- Before (show actual query execution plan for legacy behavior)
    SELECT TOP 100
        [Customer Key], [Customer],
          dbo.customer_category([Customer Key]) AS [Discount Price]
    FROM [Dimension].[Customer]
    ORDER BY [Customer Key]
    OPTION (RECOMPILE,USE HINT('DISABLE_TSQL_SCALAR_UDF_INLINING'));
    ```

8.  Click the **Include Actual Execution Plan** (Ctrl+M) button in the toolbar above the query window. This will allow us to view the actual (not estimated) query plan after executing the query.

    ![The Actual Query Plan button is highlighted in the toolbar.](media/ssms-enable-actual-query-plan.png 'Enable Actual Query Plan')

9.  Execute the query.

10. After the query executes, select the **Execution plan** tab. As the plan shows, SQL Server adopts a simple strategy here: for every tuple in the `Customer` table, invoke the UDF and output the results. This strategy is naïve and inefficient. Also, make note of the query execution time. In our case in the screenshot below, it took 9 seconds to complete.

    ![This screenshot shows the query execution plan using the legacy method.](media/ssms-udf-inlining-old.png 'Query execution plan with legacy method')

11. Clear the query window, or open a new one, then paste the following query that makes use of the scalar UDF inlining QP feature. If you opened a new query window instead of reusing this one, make sure to click the **Include Actual Execution Plan** button to enable it.

    ```sql
    USE ContosoAutoDW;
    GO

    -- After (show actual query execution plan for Scalar UDF Inlining)
    SELECT TOP 100
        [Customer Key], [Customer],
          dbo.customer_category([Customer Key]) AS [Discount Price]
    FROM [Dimension].[Customer]
    ORDER BY [Customer Key]
    OPTION (RECOMPILE);
    ```

12. After the query executes, select the **Execution plan** tab once again. With scalar UDF inlining, this UDF is transformed into equivalent scalar subqueries, which are substituted in the calling query in place of the UDF.

    ![This screenshot shows the query execution plan using the new QP feature.](media/ssms-udf-inlining-new.png 'Query execution plan with new method')

    > As you can see, the query plan no longer has a user-defined function operator, but its effects are now observable in the plan, like views or inline TVFs. Here are some key observations from the above plan:

    1. SQL Server has inferred the implicit join between `CUSTOMER` and `ORDERS` and made that explicit via a join operator.
    2. SQL Server has also inferred the implicit `GROUP BY O_CUSTKEY on ORDERS` and has used the IndexSpool + StreamAggregate to implement it.
    3. SQL Server is now using parallelism across all operators.

    > Depending upon the complexity of the logic in the UDF, the resulting query plan might also get bigger and more complex. As we can see, the operations inside the UDF are now no longer a black box, and hence the query optimizer is able to cost and optimize those operations. Also, since the UDF is no longer in the plan, iterative UDF invocation is replaced by a plan that completely avoids function call overhead.

## Task 5: Identify PII and GDPR-related compliance issues using Data Discovery & Classification in SSMS

TBD

## Task 6: Fix compliance issues with dynamic data masking

TBD
