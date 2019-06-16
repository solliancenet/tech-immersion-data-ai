# Data & AI Tech Immersion Workshop – Product Review Guide and Lab Instructions

## Day 1, Experience 1 - Handling Big Data with SQL Server 2019 Big Data Clusters

- [Data & AI Tech Immersion Workshop – Product Review Guide and Lab Instructions](#data--ai-tech-immersion-workshop-%E2%80%93-product-review-guide-and-lab-instructions)
  - [Day 1, Experience 1 - Handling Big Data with SQL Server 2019 Big Data Clusters](#day-1-experience-1---handling-big-data-with-sql-server-2019-big-data-clusters)
  - [Technology overview](#technology-overview)
  - [Scenario overview](#scenario-overview)
  - [Experience requirements](#experience-requirements)
  - [Before the lab: Connecting to SQL Server 2019](#before-the-lab-connecting-to-sql-server-2019)
    - [Connect with Azure Data Studio](#connect-with-azure-data-studio)
    - [Connect with SQL Server Management Studio](#connect-with-sql-server-management-studio)
  - [Task 1: Query performance improvements with intelligent query processing](#task-1-query-performance-improvements-with-intelligent-query-processing)
  - [Task 2: Identify PII and GDPR-related compliance issues using Data Discovery & Classification in SSMS](#task-2-identify-pii-and-gdpr-related-compliance-issues-using-data-discovery--classification-in-ssms)
  - [Task 3: Fix compliance issues with dynamic data masking](#task-3-fix-compliance-issues-with-dynamic-data-masking)
  - [Wrap-up](#wrap-up)
  - [Additional resources and more information](#additional-resources-and-more-information)

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
    - **Server:** Enter the IP address, followed by port number `31433`  to the SQL Server 2019 Big Data cluster. Use the value from the `SQL SERVER_2019_CLUSTER URL` for this from the environment documentation. It should have a format of IP separated by a comma from the port, such as: `11.122.133.144,31433`.
    - **Authentication type:** Select SQL Login.
    - **Username:** Enter `sa`.
    - **Password:** Enter the password provided to you for this lab, you can find this value documented as `SQL 2019 Big Data Cluster password`.
    - **Remember password:** Checked.
    - Leave all other options at their default values.

    ![The Connection form is filled out with the previously mentioned settings entered into the appropriate fields.](media/ads-new-connection.png 'Azure Data Studio - New Connection')

4.  Click **Connect**.

### Connect with SQL Server Management Studio

The version of SQL Server Management Studio (SSMS) used in this lab is v17.x. There is a [newer preview version (v18)](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-2017#ssms-180-preview-7) that includes some SQL Server 2019 features, which is not included in the provided environment.

1.  On the bottom-left corner of your Windows desktop, locate the search box next to the Start Menu. Type **SQL Server Management Studio**, then select the SQL Server Management Studio desktop app in the search results.

    ![The search box has "SQL Server Management Studio" entered into it and the desktop app is highlighted in the results.](media/launch-ssms.png 'Launch SQL Server Management Studio')

2.  Within the Connection dialog that appears, configure the following:

    - **Server name:** Enter the IP address, followed by port number `31433` to the SQL Server 2019 Big Data cluster. Use the value from the `SQL SERVER_2019_CLUSTER URL` for this from the environment documentation. It should have a format of IP separated by a comma from the port, such as: `11.122.133.144,31433`.
    - **Authentication:** Select SQL Server Authentication.
    - **Login:** Enter `sa`.
    - **Password:** Enter the password provided to you for this lab, you can find this value documented as `SQL 2019 Big Data Cluster password`.
    - **Remember password:** Checked.

    ![The Connect form is filled out with the previously mentioned settings entered into the appropriate fields.](media/ssms-connection.png 'SQL Server Management Studio - Connect')

3.  Click **Options >>**.

4.  Select the **Additional Connection Parameters** tab. In the text area below, enter `TrustServerCertificate=True`. This is needed because the server certificates are dynamically generated for the Big Data Clusters, and are self-signed.

    ![The Additional Connection Parameters tab is selected and the TrustServerCertificate=True value is highlighted.](media/ssms-connection-additional.png 'Additional Connection Parameters')

5.  Click **Connect**.

## Task 1: Query performance improvements with intelligent query processing

In this task, you will execute a series of SQL scripts in SQL Server Management Studio (SSMS) to explore the improvements to family of intelligent query processing (QP) features in SQL Server 2019. These features improve the performance of existing workloads with minimal work on your part to implement. The key to enabling these features in SQL Server 2019 is to set the database compatibility level to `150`. You will be executing these queries against the `ContosoAutoDW` database.

Read more about [intelligent query processing](https://docs.microsoft.com/sql/relational-databases/performance/intelligent-query-processing?view=sql-server-ver15) in SQL databases.

The first query you will run uses a user-defined function (UDF) that we have created for you, named `customer_category`. This UDF contains several steps to identify the discount price category for each customer. Notice that the top of the query we ran to create this UDF sets the database compatibility level to `150`, which is the new compatibility level for SQL Server 2019, enabling the most recent intelligent QP features. This UDF will be called inline from the two queries that follow in order to show QP improvements on scalar UDF inlining.

```sql
USE ContosoAutoDW;
GO

ALTER DATABASE ContosoAutoDW
SET COMPATIBILITY_LEVEL = 150;
GO

ALTER DATABASE SCOPED CONFIGURATION
CLEAR PROCEDURE_CACHE;
GO

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

1.  Open SQL Server Management Studio (SSMS) and connect to your SQL Server 2019 cluster. If you are unsure of how to do this, refer to [Connect with SQL Server Management Studio](#connect-with-sql-server-management-studio) at the top of this guide.

2.  Right-click on `ContosoAutoDW`, then select **New Query**. This will open a new query window into which you can paste the following queries. You may wish to reuse the same query window, replacing its contents with each SQL statement blocks below, or follow these same steps to create new query windows for each.

    ![ContosoAutoDW is selected and the New Query menu option is highlighted.](media/ssms-new-query.png 'New Query')

3.  The query below selects the top 100 rows from the `Customer` dimension table, calling a user-defined function (UDF) inline for each row. It uses the `DISABLE_TSQL_SCALAR_UDF_INLINING` hint to disable the new scalar UDF inlining QP feature. Paste the following query into the the empty query window. **Do not execute yet**.

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

4.  Click the **Include Actual Execution Plan** (Ctrl+M) button in the toolbar above the query window. This will allow us to view the actual (not estimated) query plan after executing the query.

    ![The Actual Query Plan button is highlighted in the toolbar.](media/ssms-enable-actual-query-plan.png 'Enable Actual Query Plan')

5.  **Execute** the query.

6.  After the query executes, select the **Execution plan** tab. As the plan shows, SQL Server adopts a simple strategy here: for every tuple in the `Customer` table, invoke the UDF and output the results (single line from the clustered index scan to compute scalar). This strategy is naïve and inefficient, especially with more complex queries.

    ![This screenshot shows the query execution plan using the legacy method.](media/ssms-udf-inlining-old.png 'Query execution plan with legacy method')

7.  Clear the query window, or open a new one, then paste the following query that makes use of the scalar UDF inlining QP feature. If you opened a new query window instead of reusing this one, make sure to click the **Include Actual Execution Plan** button to enable it. **Execute** the query.

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

8.  After the query executes, select the **Execution plan** tab once again. With scalar UDF inlining, this UDF is transformed into equivalent scalar subqueries, which are substituted in the calling query in place of the UDF.

    ![This screenshot shows the query execution plan using the new QP feature.](media/ssms-udf-inlining-new.png 'Query execution plan with new method')

    > As you can see, the query plan no longer has a user-defined function operator, but its effects are now observable in the plan, like views or inline TVFs. Here are some key observations from the above plan:

    A. SQL Server has inferred the implicit join between `Dimension.Customer` and `Fact.OrderHistory` and made that explicit via a join operator.

    B. SQL Server has also inferred the implicit `GROUP BY [Customer Key] on Fact.OrderHistory` and has used the IndexSpool + StreamAggregate to implement it.

    > Depending upon the complexity of the logic in the UDF, the resulting query plan might also get bigger and more complex. As we can see, the operations inside the UDF are now no longer a black box, and hence the query optimizer is able to cost and optimize those operations. Also, since the UDF is no longer in the plan, iterative UDF invocation is replaced by a plan that completely avoids function call overhead.

9.  Either highlight and delete everything in the query window, or open a new query window. Paste the following query that makes use of the table variable deferred compilation feature, since the database compatibility level is set to 150. If you opened a new query window instead of reusing this one, make sure to click the **Include Actual Execution Plan** button to enable it. **Execute** the query.

    ```sql
    USE [ContosoAutoDW]
    GO

    DECLARE @Order TABLE
      ([Order Key] BIGINT NOT NULL,
      [Quantity] INT NOT NULL
      );

    INSERT @Order
    SELECT [Order Key], [Quantity]
    FROM [Fact].[OrderHistory]
    WHERE  [Quantity] > 99;

    -- Look at estimated rows, speed, join algorithm
    SELECT oh.[Order Key], oh.[Order Date Key],
        oh.[Unit Price], o.Quantity
    FROM Fact.OrderHistoryExtended AS oh
    INNER JOIN @Order AS o
      ON o.[Order Key] = oh.[Order Key]
    WHERE oh.[Unit Price] > 0.10
    ORDER BY oh.[Unit Price] DESC;
    GO
    ```

    > The script above assigns a table variable, `@Order`, storing the `Order Key` and `Quantity` fields from the `OrderHistory` table to be used in an INNER JOIN further below.

    **Old method**

    In prior versions of SQL Server (compatibility level of 140 or lower), the table variable deferred compilation QP feature is not used (more on this below).

    There are two plans. The one you want to observe is the second query plan. When we over the second INNER JOIN to view the estimated number of rows and the output list, which shows the join algorithm. The estimated number of rows is around 1. Also, observe the execution time. In our case, it took 11 seconds to complete.

    ![This screenshot shows the query execution plan using the legacy method.](media/ssms-tvdc-old.png 'Query execution plan with old method')

    **New method**

    After the query executes, select the **Execution plan** tab once again. Since our database compatibility level is set to 150,notice that the join algorithm is a hash match, and that the overall query execution plan looks different. When you hover over the INNER JOIN, notice that there is a high value for estimated number of rows and that the output list shows the use of hash keys and an optimized join algorithm. Once again, observe the execution time. In our case, it took 5 seconds to complete, which is less than half the time it took to execute without the table variable deferred compilation feature.

    ![This screenshot shows the query execution plan using the new method.](media/ssms-tvdc-new.png 'Query execution plan with new method')

    > Table variable deferred compilation improves plan quality and overall performance for queries that reference table variables. During optimization and initial compilation, this feature propagates cardinality estimates that are based on actual table variable row counts. This accurate row count information optimizes downstream plan operations. Table variable deferred compilation defers compilation of a statement that references a table variable until the first actual run of the statement. This deferred compilation behavior is the same as that of temporary tables. This change results in the use of actual cardinality instead of the original one-row guess. _For more information, see [Table variable deferred compilation](https://docs.microsoft.com/sql/t-sql/data-types/table-transact-sql?view=sql-server-2017#table-variable-deferred-compilation)._

10. Either highlight and delete everything in the query window, or open a new query window. Paste the following query to simulate out-of-date statistics on the `OrderHistory` table, followed by a query that executes a hash match. If you opened a new query window instead of reusing this one, make sure to click the **Include Actual Execution Plan** button to enable it. **Execute** the query.

    ```sql
    ALTER DATABASE ContosoAutoDW SET COMPATIBILITY_LEVEL = 150;
    GO

    ALTER DATABASE SCOPED CONFIGURATION CLEAR PROCEDURE_CACHE;
    GO

    USE ContosoAutoDW;
    GO

    -- Simulate out-of-date stats
    UPDATE STATISTICS Fact.OrderHistory
    WITH ROWCOUNT = 1;
    GO

    SELECT
      fo.[Order Key], fo.Description,
      si.[Lead Time Days]
    FROM    Fact.OrderHistory AS fo
    INNER HASH JOIN Dimension.[Stock Item] AS si
      ON fo.[Stock Item Key] = si.[Stock Item Key]
    WHERE   fo.[Lineage Key] = 9
      AND si.[Lead Time Days] > 19;
    ```

11. After the query executes, select the **Execution plan** tab. Hover over the Hash Match step of the execution plan. You should see a warning toward the bottom of the Hash Match dialog showing spilled data. Also observe the execution time. In our case, this query took 26 seconds to execute.

    ![The Hash Match dialog shows spilled data warnings.](media/ssms-memory-grant-feedback.png 'Query execution plan showing spilled data')

12. Either highlight and delete everything in the query window, or open a new query window. Paste the following query to execute the select query that contains the hash match once more. If you opened a new query window instead of reusing this one, make sure to click the **Include Actual Execution Plan** button to enable it. **Execute** the query.

    ```sql
    USE ContosoAutoDW;
    GO

    SELECT
      fo.[Order Key], fo.Description,
      si.[Lead Time Days]
    FROM    Fact.OrderHistory AS fo
    INNER HASH JOIN Dimension.[Stock Item] AS si
      ON fo.[Stock Item Key] = si.[Stock Item Key]
    WHERE   fo.[Lineage Key] = 9
      AND si.[Lead Time Days] > 19;
    ```

13. After the query executes, select the **Execution plan** tab. Hover over the Hash Match step of the execution plan. You should **no longer** see a warning about spilled data. Also observe the execution time. In our case, this query took 4 seconds to execute.

    ![The Hash Match dialog no longer contains spilled data warnings.](media/ssms-memory-grant-feedback-fixed.png 'Query execution plan with no spilled data')

    > So what happened? A query's post-execution plan in SQL Server includes the minimum required memory needed for execution and the ideal memory grant size to have all rows fit in memory. Performance suffers when memory grant sizes are incorrectly sized. Excessive grants result in wasted memory and reduced concurrency. Insufficient memory grants cause expensive spills to disk. By addressing repeating workloads, batch mode memory grant feedback recalculates the actual memory required for a query and then updates the grant value for the cached plan. **When an identical query statement is executed**, the query uses the revised memory grant size, reducing excessive memory grants that impact concurrency and fixing underestimated memory grants that cause expensive spills to disk. Row mode memory grant feedback expands on the batch mode memory grant feedback feature by adjusting memory grant sizes for both batch and row mode operators. _For more information, see [Row mode memory grant feedback](https://docs.microsoft.com/sql/relational-databases/performance/adaptive-query-processing?view=sql-server-2017#row-mode-memory-grant-feedback)._

## Task 2: Identify PII and GDPR-related compliance issues using Data Discovery & Classification in SSMS

Contoso Auto has several databases that include tables containing sensitive data, such as personally identifiable information (PII) like phone numbers, social security numbers, financial data, etc. Since some of their personnel and customer data include individuals who reside within the European Union (EU), they need to adhere to the General Data Protection Regulation ([GDPR](https://en.wikipedia.org/wiki/General_Data_Protection_Regulation)) as well. Because of this, Contoso Auto is required to provide periodic data auditing reports to identify sensitive and GDPR-related data that reside within their various databases.

With SQL Server Management Studio, they are able to identify, classify, and generate reports on sensitive and GDPR-related data by using the [SQL Data Discovery & Classification](https://docs.microsoft.com/sql/relational-databases/security/sql-data-discovery-and-classification?view=sql-server-ver15) tool. This tool introduces a set of advanced services, forming a new SQL Information Protection paradigm aimed at protecting the data, not just the database:

- **Discovery & recommendations** - The classification engine scans your database and identifies columns containing potentially sensitive data. It then provides you an easy way to review and apply the appropriate classification recommendations, as well as to manually classify columns.
- **Labeling** - Sensitivity classification labels can be persistently tagged on columns.
- **Visibility** - The database classification state can be viewed in a detailed report that can be printed/exported to be used for compliance & auditing purposes, as well as other needs.

In this exercise, you will run the SQL Data Discovery & Classification tool against their customer database, which includes personal, demographic, and sales data.

1.  Open SQL Server Management Studio (SSMS) and connect to your SQL Server 2019 cluster.

2.  Right-click on the **sales_YOUR_UNIQUE_IDENTIFIER** database, then choose **Tasks > Classify Data...**.

    ![The sales database, Tasks menu, and Classify Data items are highlighted.](media/ssms-classify-data-link.png 'Data Classification')

3.  When the tool runs, it will analyze all of the columns within all of the tables and recommend appropriate data classifications for each. What you should see is the Data Classification dashboard showing no currently classified columns, and a classification recommendations box at the top showing that there are 45 columns that the tool identified as containing sensitive (PII) or GDPR-related data. **Click** on this classification recommendations box.

    ![The data classification recommendations box is highlighted.](media/ssms-classification-recommendations-box.png 'Data classification recommendations box')

4.  The list of recommendations displays the schema, table, column, type of information, and recommended sensitivity label for each identified column. You can change the information type and sensitivity labels for each if desired. In this case, accept all recommendations by **checking the checkbox** in the recommendations table header.

    ![The recommendations are shown with each checkbox checked.](media/ssms-recommendations.png 'Classification recommendations')

5.  Click **Accept selected recommendations**.

    ![The Accept selected recommendations button is highlighted.](media/ssms-accept-selected-recommendations.png 'Accept selected recommendations')

6.  Click **Save** in the toolbar above to apply your changes.

    ![The Save button is highlighted.](media/ssms-save-classification-changes.png 'Save classification changes')

7.  After the changes are saved, click **View Report**.

    ![The View Report button is highlighted.](media/ssms-view-report.png 'View Report')

8.  What you should see is a report with a full summary of the database classification state. When you right-click on the report, you can see options to print or export the report in different formats.

    ![The report is displayed, as well as the context menu showing export options after right-clicking on the report.](media/ssms-report.png 'SQL Data Classification Report')

## Task 3: Fix compliance issues with dynamic data masking

Some of the columns identified by the Data Discovery & Classification tool as containing sensitive (PII/GDPR) information include phone numbers, email addresses, billing addresses, and credit card numbers. One way to ensure compliance with various rules and regulations that enforce policies to protect such sensitive data is to prevent those who are not authorized from seeing it. An example would be displaying `XXX-XXX-XX95` instead of `123-555-2695` when outputting a phone number within a SQL query result, report, web page, etc. This is commonly called data masking. Traditionally, modifying systems and applications to implement data masking can be challenging. This is especially true when the masking has to apply all the way down to the data source level. Fortunately, SQL Server and its cloud-related product, Azure SQL Database, provides a feature named [dynamic data masking](https://docs.microsoft.com/sql/relational-databases/security/dynamic-data-masking?view=sql-server-ver15) (DDM) to automatically protect this sensitive data from non-privileged users.

Dynamic data masking helps prevent unauthorized access to sensitive data by enabling customers to designate how much of the sensitive data to reveal with minimal impact on the application layer. DDM can be configured on the database to hide sensitive data in the result sets of queries over designated database fields, while the data in the database is not changed. Dynamic data masking is easy to use with existing applications, since masking rules are applied in the query results. Many applications can mask sensitive data without modifying existing queries.

In this task, you will apply dynamic data masking to one of the database fields so you can see how to address the reported compliance issues. To test the data mask, you will create a test user and query the field as that user.

1.  Open SQL Server Management Studio (SSMS) and connect to your SQL Server 2019 cluster.

2.  Expand the databases list, right-click on **sales_YOUR_UNIQUE_IDENTIFIER**, then select **New Query**.

    ![The sales database and New Query menu item are highlighted.](media/ssms-sales-new-query.png 'New Query')

3.  Add a dynamic data mask to the existing `dbo.customer.c_last_name` field by pasting the below query into the new query window:

    ```sql
    ALTER TABLE dbo.customer
    ALTER COLUMN c_last_name ADD MASKED WITH (FUNCTION = 'partial(2,"XXX",0)');
    ```

    > The `partial` custom string masking method above exposes the first two characters and adds a custom padding string after for the remaining characters. The parameters are: `prefix,[padding],suffix`

4.  Execute the query by clicking the **Execute** button above the query window, or enter _F5_.

    ![The dynamic data mask query is shown and the Execute button is highlighted above.](media/ssms-execute-ddm-query.png 'Execute query')

5.  Clear the query window and replace the previous query with the following to add a dynamic data mask to the `dbo.customer.c_email_address` field:

    ```sql
    ALTER TABLE dbo.customer
    ALTER COLUMN c_email_address ADD MASKED WITH (FUNCTION = 'email()');
    ```

    > The `email` masking method exposes the first letter of an email address and the constant suffix ".com", in the form of an email address: `aXXX@XXXX.com`.

6.  Clear the query window and replace the previous query with the following, selecting all rows from the customer table:

    ```sql
    SELECT * FROM dbo.customer
    ```

    ![The query results are shown with no mask applied to the Postal Code field.](media/ssms-ddm-results-no-mask.png 'Query results')

7.  Notice that the full last name and email address values are visible. That is because the user you are logged in as a privileged user. Let's create a new user and execute the query again:

    ```sql
    CREATE USER TestUser WITHOUT LOGIN;
    GRANT SELECT ON dbo.customer TO TestUser;

    EXECUTE AS USER = 'TestUser';
    SELECT * FROM dbo.customer;
    REVERT;
    ```

8.  Execute the query by clicking the **Execute** button. Notice this time that the Postal Code values are masked (`90XXX`).

    ![The query results are shown with the mask applied to the Postal Code field.](media/ssms-ddm-results-mask.png 'Query results')


## Task 4: Mounting an Azure Data Lake Gen2 Storage Account to SQL Server 2019 Big Data Cluster using HDFS Tiering

With tiering, applications can seamlessly access data in a variety of external stores as though the data resides in the local HDFS.   This allows you to interact with the files in Azure Data Lake Store Gen2 as if they were local files.  You can either use an Azure Storage access key or an Azure Active Directory User Account to gain permission to the files.  For this lab, we will use the access key.

1.  In Windows, open PowerShell.  

  ![Search for PowerShell .](media/powershell.png 'SQL Server Management Studio - Connect')

2. In PowerShell, install the mssqlstl package using pip:

    ```powershell
      pip3 install -r  https://private-repo.microsoft.com/python/ctp3.0/mssqlctl/requirements.txt
    ```
3. Once mssqlctl installs, connect to your Microsoft SQL Server 2019 Big Data Cluster:

    ```python
      mssqlctl login -e https://<SQL SERVER MASTER INSTANCE IP ADDRESS>:31433
    ```
    a.  You will be prompted for your big data cluster name.
    b.  The user name is admin
    c.  The password is MySQLBigData2019

4. Create an empty text file named file.creds in your temp folder on the c:\ drive.  Add this line as the contents:

fs.azure.abfs.account.name=ikedatabricks.dfs.core.windows.net
fs.azure.account.key.ikedatabricks.dfs.core.windows.net=HUYPk/VUjdYzkCvrKXTgFBObt5VQcp5DCY7C9KiSHX42lv65mjmBFmKFVTLy7Z7suQ0WV44mncuUOvnE8NkxGg==

5. In PowerShell, type the following command to mount the drive

    ```powershell
        mssqlctl cluster storage-pool mount create --remote-uri abfs://databricksfiels@ikedatabricks.dfs.core.windows.net/ --mount-path   /mounts/dbfiles --credential-file c:\temp\file.creds
    ```
5.  Once the storage account has been mounted, you can check the status:

  ```powershell
    mssqlctl cluster storage-pool mount status
  ```  
  
6.  Now that the drive is mounted, create an external file format for CSV:

  ```sql
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
7.  Now create an external connection to your HDFS cluster:

  ```sql
    IF NOT EXISTS(SELECT * FROM sys.external_data_sources WHERE name = 'SqlStoragePool')
    BEGIN
      CREATE EXTERNAL DATA SOURCE SqlStoragePool
      WITH (LOCATION = 'sqlhdfs://controller-svc:8080/default');
    END
  ```
 8.  Now let's create two tables to two different files that exist in the storage account: 
  
  ```sql
 CREATE EXTERNAL TABLE planes
("tailnum" VARCHAR(100),	"year" VARCHAR(4),	"type" VARCHAR(100),	"manufacturer" VARCHAR(100),	"model" VARCHAR(20),	"engines" BIGINT,	"seats" BIGINT,	"speed" VARCHAR(20),	"engine" VARCHAR(20))
WITH
(
    DATA_SOURCE = SqlStoragePool,
    LOCATION = '/dbfiles/planes',
    FILE_FORMAT = csv_file
);
GO

 CREATE EXTERNAL TABLE flights
("year" BIGINT, 	"month" BIGINT, 	"day" BIGINT,	"dep_time" BIGINT,	"dep_delay" BIGINT,	"arr_time" BIGINT,	"arr_delay" BIGINT,	"carrier" VARCHAR(100),	"tailnum" VARCHAR(20),	"flight" VARCHAR(20),	"origin" VARCHAR(50),	"dest" VARCHAR(50),	"air_time" BIGINT,	"distance" BIGINT,	"hour" BIGINT,	"minute" BIGIN)
)
WITH
(
    DATA_SOURCE = SqlStoragePool,
    LOCATION = '/dbfiles/flights_small',
    FILE_FORMAT = csv_file
);
 ```
9.  Once the tables are created, you can interact with them like normal tables.  For instance, you can run a query that joins the two tables like this:
 ```sql
  SELECT * 
   FROM planes p 
   JOIN flights f
    on p.tail_num = f.tail_num
 
  ```
  
## Wrap-up

Thank you for participating in the SQL Server 2019 Big Data Clusters experience! We hope you are excited about the new capabilities, and will refer back to this experience to learn more about these features.

To recap, you experienced:

1. Intelligent Query Processing (QP) performance improvements with SQL Server 2019's new database compatibility level: `150`.
2. Using the [SQL Data Discovery & Classification](https://docs.microsoft.com/sql/relational-databases/security/sql-data-discovery-and-classification?view=sql-server-ver15) tool to identify and tag PII and GDPR-related compliance issues.
3. Used dynamic data masking to automatically protect sensitive data from unauthorized users.
4. Using HDFS tiering to mount files from ADLS Gen2 as if they were local to SQL Server 2019 in HDFS.

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
