# Data & AI Tech Immersion Workshop – Product Review Guide and Lab Instructions

## Day 1, Experience 1 - Business Critical Performance and Security with SQL Server 2019

- [Data & AI Tech Immersion Workshop – Product Review Guide and Lab Instructions](#Data--AI-Tech-Immersion-Workshop-%E2%80%93-Product-Review-Guide-and-Lab-Instructions)
  - [Day 1, Experience 1 - Business Critical Performance and Security with SQL Server 2019](#Day-1-Experience-1---Business-Critical-Performance-and-Security-with-SQL-Server-2019)
  - [Technology overview](#Technology-overview)
  - [Scenario overview](#Scenario-overview)
  - [Experience requirements](#Experience-requirements)
  - [Task 1: Connect to SQL Server 2019 with SSMS](#Task-1-Connect-to-SQL-Server-2019-with-SSMS)
  - [Task 2: Query performance improvements with intelligent query processing](#Task-2-Query-performance-improvements-with-intelligent-query-processing)
  - [Task 3: Identify PII and GDPR-related compliance issues using Data Discovery & Classification in SSMS](#Task-3-Identify-PII-and-GDPR-related-compliance-issues-using-Data-Discovery--Classification-in-SSMS)
  - [Task 4: Fix compliance issues with dynamic data masking](#Task-4-Fix-compliance-issues-with-dynamic-data-masking)
  - [Task 5: Restrict data access with Row-level security](#Task-5-Restrict-data-access-with-Row-level-security)
  - [Task 6: Always Encrypted with secure enclaves](#Task-6-Always-Encrypted-with-secure-enclaves)
    - [Configure a secure enclave](#Configure-a-secure-enclave)
    - [Provision enclave-enabled keys](#Provision-enclave-enabled-keys)
    - [Encrypt customer email column](#Encrypt-customer-email-column)
    - [Run rich queries against an encrypted column](#Run-rich-queries-against-an-encrypted-column)
  - [Wrap-up](#Wrap-up)
  - [Additional resources and more information](#Additional-resources-and-more-information)

## Technology overview

![Diagram shows improvements to SQL Server with version 2019, as detailed below.](media/sql-2019-overview.png 'Modernize with SQL Server 2019')

Modernize with SQL Server 2019 with new features that help combine new technologies, and updates to existing SQL Server features you use today. New big data clusters brings Apache Spark and enhanced PolyBase features that allow you to easily access structured and unstructured data stored in HDFS and external relational stores, enabling data virtualization and analytics at scale.

![A tree shows the available IQP features.](media/iqp-family.png 'The Intelligent Query Processing feature family')

> The IQP features shown in bold are new and improved features in SQL Server 2019.

Intelligent Query Processing (IQP) features in SQL Server 2019 improve scaling of queries, and Automatic Plan Correction resolves performance problems. In addition, you can gain performance insights anytime and anywhere with Lightweight Query Profiling.

To further boost performance, SQL Server 2019 provides more in-memory database options than ever before, such as:

- Hybrid Buffer Pool
- Memory-Optimized TempDB Metadata
- In-Memory OLTP
- Persistent Memory Support

The Hybrid Buffer Pool allows the database engine to directly access data pages in database files stored on persistent memory (PMEM) devices. SQL Server will automatically detect if data files reside on an appropriately formatted PMEM device and perform memory mapping in user space. This mapping happens upon startup, when a new database is attached, restored, created, or when the hybrid buffer pool feature is enabled for a database.

![Always Encrypted with secure enclaves](media/always-encrypted-with-secure-enclaves-overview.png 'Always Encrypted with secure enclaves')

SQL Server 2019 enables several layers of security including protection of computations in Always Encrypted with secure enclaves. Customers can track compliance with sophisticated tools such as Data Discovery & Classification labeling for GDPR and Vulnerability Assessment tool. Transparent Data Encryption (TDE) encryption scanning now offers more control with suspend and resume syntax so that you can pause the scan while the workload on the system is heavy, or during business-critical hours, and then resume the scan later.

SSL/TLS certificates are widely used to secure access to SQL Server. In previous editions, certificate management was a more manual and time-consuming process, through developing scripts and running manual commands. With SQL Server 2019, certificate management is integrated into the SQL Server Configuration Manager, simplifying common tasks such as:

- Viewing and validating certificates installed in a SQL Server instance.
- Identifying which certificates may be close to expiring.
- Deploying certificates across Availability Group machines from the node holding the primary replica.
- Deploying certificates across machines participating in a Failover Cluster instance from the active node.

![Availability groups on Kubernetes.](media/availability-groups-kubernetes.png 'Availability groups on Kubernetes')

For High Availability and Disaster Recovery, SQL Server 2019 now supports up to eight secondary replicas in an Always On Availability Group. Customers can also run Always On Availability Groups on containers using Kubernetes.

SQL Server 2019 also has powerful tools for Business Intelligence including Analysis Services and Power BI Report Server which provide visual data exploration and interactive analysis of business data.

## Scenario overview

This experience will highlight the new features of SQL Server 2019 with a focus on performance and security. You will begin by performing an assessment of Contoso Auto's on-premises database to determine feasibility for migrating to SQL Server on a VM in Azure, and then complete the database migration. Next, you will gain hands-on experience by running queries using some of the new query performance enhancements and evaluating the results. You will evaluate the data security and compliance features provided by SQL Server 2019 by using the Data Discovery & Classification tool in SSMS to identify tables and columns with PII and GDPR-related compliance issues. You will then address some of the security issues by layering on dynamic data masking, row-level security, and Always Encrypted with secure enclaves.

## Experience requirements

Before you begin this lab, you need to find the following information on the Tech Immersion Mega Data & AI Workshop On Demand Lab environment details page, or the document provided to you for this experience:

- SQL Server 2019 VM IP address: `SQL_SERVER_2019_VM_IP`
- Sales database name (your unique copy): `SALES_DB`

## Task 1: Connect to SQL Server 2019 with SSMS

You will be accessing a shared SQL Server 2019 server for this experience. Please follow the steps below to connect to the SQL Server 2019 VM with SQL Server Management Studio (SSMS).

1. On the bottom-left corner of your Windows desktop, locate the search box next to the Start Menu. Type **SQL Server Management** into the search box, then select the SQL Server Management Studio 18 desktop app in the search results.

   ![The search box has "SQL Server Management" entered into it and the desktop app is highlighted in the results.](media/launch-ssms.png 'Launch SQL Server Management Studio')

2. Within the Connection dialog that appears, configure the following:

   - **Server name:** Enter the SQL Server 2019 VM IP address. Use the value from the `SQL_SERVER_2019_VM_IP` for this from the environment documentation.
   - **Authentication:** Select SQL Server Authentication.
   - **Login:** Enter `demouser`
   - **Password:** Enter `Password.1!!`
   - **Remember password:** Check this box.

   ![The Connect form is filled out with the previously mentioned settings entered into the appropriate fields.](media/ssms-connection-sql-2019.png 'SQL Server Management Studio - Connect')

3. Select **Connect**.

## Task 2: Query performance improvements with intelligent query processing

In this task, you will execute a series of SQL scripts in SQL Server Management Studio (SSMS) to explore the improvements to the family of intelligent query processing (QP) features in SQL Server 2019. These features improve the performance of existing workloads with minimal work on your part to implement. The key to enabling these features in SQL Server 2019 is to set the [database compatibility level](https://docs.microsoft.com/en-us/sql/t-sql/statements/alter-database-transact-sql-compatibility-level?view=sql-server-ver15) to `150`. You will be executing these queries against the `sales_XXXXX` database (where XXXXX is the unique identifier assigned to you for this workshop).

To learn more, read [intelligent query processing](https://docs.microsoft.com/sql/relational-databases/performance/intelligent-query-processing?view=sql-server-ver15) in SQL databases.

1. To get started, expand databases in the SQL Server Management Studio (SSMS) Object Explorer, right-click the `sales_XXXXX` database (where XXXXX is the unique identifier assigned to you for this workshop), and then select **New Query**.

   ![The sales database and New Query menu item are highlighted.](media/ssms-sales-new-query.png 'New Query')

2. The first query you will run is to set the database compatibility level to `150`, which is the new compatibility level for SQL Server 2019, enabling the most recent intelligent QP features. Copy the SQL script below and paste it into the new query window. Replace `XXXXX` with the unique identifier you have been given for this workshop in both the `USE` and `ALTER DATABASE` statements.

   ```sql
   USE sales_XXXXX;
   GO

   ALTER DATABASE sales_XXXXX
   SET COMPATIBILITY_LEVEL = 150;
   GO
   ```

3. To run the query, select **Execute** in the SSMS toolbar.

   ![The Execute button is highlighted in the SSMS toolbar.](media/ssms-execute-query.png 'Execute')

4. Next, you will run a query to create a user-defined function (UDF) named `customer_category`. This UDF contains several steps to identify the discount price category for each customer. Notice that at the top of the query we run to create this UDF sets the database compatibility level to `150`, which is the new compatibility level for SQL Server 2019, enabling the most recent intelligent QP features. This UDF will be called inline from the two queries that follow in order to show QP improvements on scalar UDF inlining. Paste the following SQL code into your query window, overwriting the current content, replace `XXXXX` in the `USE` statement with the unique identifier assigned to you for this workshop, and then select **Execute** on the SSMS toolbar.

   ```sql
   USE sales_XXXXX;
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
     SUM([ws_net_paid_inc_ship])
     FROM [dbo].[web_sales]
     WHERE [ws_bill_customer_sk] = @CustomerKey;

     IF @total_amount < 50000
       SET @category = 'REGULAR';
     ELSE IF @total_amount < 100000
       SET @category = 'GOLD';
     ELSE
       SET @category = 'PLATINUM';

     RETURN @category;
   END
   GO
   ```

   > Scalar UDF inlining automatically transforms [scalar UDFs](https://docs.microsoft.com/sql/relational-databases/user-defined-functions/create-user-defined-functions-database-engine?view=sql-server-2017#Scalar) into relational expressions. It embeds them in the calling SQL query. This transformation improves the performance of workloads that take advantage of scalar UDFs. Scalar UDF inlining facilitates cost-based optimization of operations inside UDFs. The results are efficient, set-oriented, and parallel instead of inefficient, iterative, serial execution plans. This feature is enabled by default under database compatibility level 150. _For more information, see [Scalar UDF inlining](https://docs.microsoft.com/sql/relational-databases/user-defined-functions/scalar-udf-inlining?view=sql-server-2017)_.

5. Right-click on the `sales_XXXXX` database (where XXXXX is the unique identifier assigned to you for this workshop), then select **New Query**. This will open a new query window into which you can paste the following queries. You may wish to reuse the same query window, replacing its contents with each SQL statement blocks below, or follow these same steps to create new query windows for each.

   ![The sales database and New Query menu item are highlighted.](media/ssms-sales-new-query.png 'New Query')

6. The query below selects the top 100 rows from the `customer` table, calling the `customer_category` user-defined function (UDF) inline for each row. It uses the `DISABLE_TSQL_SCALAR_UDF_INLINING` hint to disable the new scalar UDF inlining QP feature. Paste the following query into the the empty query window. Replace `XXXXX` in the `USE` statement with the unique identifier assigned to you for this workshop. **Do not execute yet**.

   ```sql
   USE sales_XXXXX;
   GO

   -- Before (show actual query execution plan for legacy behavior)
   SELECT TOP 100
       [c_customer_sk], [c_first_name], [c_last_name],
         dbo.customer_category([c_customer_sk]) AS [Discount Category]
   FROM [dbo].[customer]
   ORDER BY [c_customer_sk]
   OPTION (RECOMPILE, USE HINT('DISABLE_TSQL_SCALAR_UDF_INLINING'));
   ```

7. Select the **Include Actual Execution Plan** (Ctrl+M) button in the toolbar above the query window. This will allow us to view the actual (not estimated) query plan after executing the query.

   ![The Actual Query Plan button is highlighted in the toolbar.](media/ssms-enable-actual-query-plan-customer.png 'Enable Actual Query Plan')

8. Execute the query by selecting **Execute** from the SSMS toolbar.

9. After the query executes, select the **Execution plan** tab. As the plan shows, SQL Server adopts a simple strategy here: for every tuple in the `customer` table, invoke the UDF and output the results (single line from the clustered index scan to compute scalar). This strategy is naïve and inefficient, especially with more complex queries.

   ![This screenshot shows the query execution plan using the legacy method.](media/ssms-udf-inlining-before.png 'Query execution plan with legacy method')

10. Clear the query window, or open a new one, then paste the following query that makes use of the scalar UDF inlining QP feature. Replace `XXXXX` in the `USE` statement with the unique identifier assigned to you for this workshop. If you opened a new query window instead of reusing this one, make sure to select the **Include Actual Execution Plan** button to enable it. **Execute** the query.

    ```sql
    USE sales_XXXXX;
    GO

    -- After (show actual query execution plan for legacy behavior)
    SELECT TOP 100
        [c_customer_sk], [c_first_name], [c_last_name],
          dbo.customer_category([c_customer_sk]) AS [Discount Category]
    FROM [dbo].[customer]
    ORDER BY [c_customer_sk]
    OPTION (RECOMPILE);
    ```

11. After the query executes, select the **Execution plan** tab once again. With scalar UDF inlining, this UDF is transformed into equivalent scalar subqueries, which are substituted in the calling query in place of the UDF.

    ![This screenshot shows the query execution plan using the new QP feature.](media/ssms-udf-inlining-after.png 'Query execution plan with new method')

    > As you can see, the query plan no longer has a user-defined function operator, but its effects are now observable in the plan, like views or inline TVFs. Here are some key observations from the above plan:

    A. SQL Server has inferred the implicit join between `dbo.customer` and `dbo.web_sales` and made that explicit via a join operator.

    B. SQL Server has also inferred the implicit `GROUP BY [Customer Key] on dbo.web_sales` and has used the IndexSpool + StreamAggregate to implement it.

    > Depending upon the complexity of the logic in the UDF, the resulting query plan might also get bigger and more complex. As we can see, the operations inside the UDF are now no longer a black box, and hence the query optimizer is able to cost and optimize those operations. Also, since the UDF is no longer in the plan, iterative UDF invocation is replaced by a plan that completely avoids function call overhead.

12. Either highlight and delete everything in the query window, or open a new query window. Paste the following query into the query window, replacing `XXXXX` in the `USE` statement with the unique identifier assigned to you for this workshop. This query makes use of the table variable deferred compilation feature, since the database compatibility level is set to `150`. If you opened a new query window instead of reusing this one, make sure to click the **Include Actual Execution Plan** button to enable it. **Execute** the query.

    ```sql
    USE sales_XXXXX
    GO

    DECLARE @ItemClick TABLE (
      [itemKey] BIGINT NOT NULL,
      [clickDate] BIGINT NOT NULL
    );

    INSERT @ItemClick
    SELECT [wcs_item_sk], [wcs_click_date_sk]
    FROM [dbo].[web_clickstreams]

    -- Look at estimated rows, speed, join algorithm
    SELECT i.[i_item_sk], i.[i_current_price], c.[clickDate]
    FROM dbo.item AS i
    INNER JOIN @ItemClick AS c
      ON i.[i_item_sk] = c.[itemKey]
    WHERE i.[i_current_price] > 90
    ORDER BY i.[i_current_price] DESC
    OPTION (USE HINT('DISABLE_DEFERRED_COMPILATION_TV'));
    GO
    ```

    > The script above assigns a table variable, `@ItemClick`, storing the `itemKey` and `clickDate` fields from the `web_clickstreams` table to be used in an INNER JOIN below.

    > The `DISABLE_DEFERRED_COMPILATION_TV` hint **disables** the table-deferred compilation feature.

    **Old method**

    In prior versions of SQL Server (compatibility level of 140 or lower), the table variable deferred compilation QP feature is not used (more on this below).

    There are two plans. The one you want to observe is the second query plan. Because we disabled the table-deferred compilation feature with the `DISABLE_DEFERRED_COMPILATION_TV` hint, when we mouse over the INNER JOIN to view the estimated number of rows and the output list, which shows the join algorithm. The estimated number of rows is 1. Also, observe the execution time. In our case, it took 10 seconds to complete.

    ![This screenshot shows the query execution plan using the legacy method.](media/ssms-tvdc-old-method.png 'Query execution plan with old method')

    **New method**

    Execute the following updated query, which removes the hint we used in the previous query to disable the table-deferred compilation feature:

    ```sql
    USE sales_XXXXX
    GO

    DECLARE @ItemClick TABLE (
      [itemKey] BIGINT NOT NULL,
      [clickDate] BIGINT NOT NULL
    );

    INSERT @ItemClick
    SELECT [wcs_item_sk], [wcs_click_date_sk]
    FROM [dbo].[web_clickstreams]

    -- Look at estimated rows, speed, join algorithm
    SELECT i.[i_item_sk], i.[i_current_price], c.[clickDate]
    FROM dbo.item AS i
    INNER JOIN @ItemClick AS c
      ON i.[i_item_sk] = c.[itemKey]
    WHERE i.[i_current_price] > 90
    ORDER BY i.[i_current_price] DESC;
    GO
    ```

    After the query above executes, select the **Execution plan** tab once again. Since our database compatibility level is set to 150, notice that the join algorithm is a hash match, and that the overall query execution plan looks different. When you hover over the INNER JOIN, notice that there is a high value for estimated number of rows and that the output list shows the use of hash keys and an optimized join algorithm. Once again, observe the execution time. In our case, it took 6 seconds to complete, which is approximately half the time it took to execute without the table variable deferred compilation feature.

    ![This screenshot shows the query execution plan using the new method.](media/ssms-tvdc-new-method.png 'Query execution plan with new method')

    > Table variable deferred compilation improves plan quality and overall performance for queries that reference table variables. During optimization and initial compilation, this feature propagates cardinality estimates that are based on actual table variable row counts. This accurate row count information optimizes downstream plan operations. Table variable deferred compilation defers compilation of a statement that references a table variable until the first actual run of the statement. This deferred compilation behavior is the same as that of temporary tables. This change results in the use of actual cardinality instead of the original one-row guess. _For more information, see [Table variable deferred compilation](https://docs.microsoft.com/sql/t-sql/data-types/table-transact-sql?view=sql-server-2017#table-variable-deferred-compilation)._

13. Either highlight and delete everything in the query window, or open a new query window. Paste the following query to simulate out-of-date statistics on the `web_sales` table, followed by a query that executes a hash match, replacing `XXXXX` in the `USE` statement with the unique identifier assigned to you for this workshop. If you opened a new query window instead of reusing this one, make sure to click the **Include Actual Execution Plan** button to enable it. **Execute** the query.

    ```sql
    USE sales_XXXXX;
    GO

    -- Simulate out-of-date stats
    UPDATE STATISTICS dbo.web_sales
    WITH ROWCOUNT = 1;
    GO

    SELECT
      ws.[ws_order_number], ws.ws_quantity,
      i.[i_current_price], i.[i_item_desc]
    FROM    dbo.web_sales AS ws
    INNER HASH JOIN dbo.[item] AS i
      ON ws.[ws_item_sk] = i.[i_item_sk]
    WHERE   i.[i_current_price] > 10
      AND ws.[ws_quantity] > 40;
    ```

14. After the query executes, select the **Execution plan** tab. Hover over the Hash Match step of the execution plan. You should see a warning toward the bottom of the Hash Match dialog showing spilled data. Also observe the execution time. In our case, this query took 16 seconds to execute.

    ![The Hash Match dialog shows spilled data warnings.](media/ssms-memory-grant-feedback-old.png 'Query execution plan showing spilled data')

15. Either highlight and delete everything in the query window, or open a new query window. Paste the following query to execute the select query that contains the hash match once more. If you opened a new query window instead of reusing this one, make sure to click the **Include Actual Execution Plan** button to enable it. **Execute** the query.

```sql
USE sales_XXXXX;
GO

SELECT
  ws.[ws_order_number], ws.ws_quantity,
  i.[i_current_price], i.[i_item_desc]
FROM    dbo.web_sales AS ws
INNER HASH JOIN dbo.[item] AS i
  ON ws.[ws_item_sk] = i.[i_item_sk]
WHERE   i.[i_current_price] > 10
  AND ws.[ws_quantity] > 40;
```

16. After the query executes, select the **Execution plan** tab. Hover over the Hash Match step of the execution plan. You may no longer see a warning about spilled data. If you do, the **number of pages Hash wrote** should have decreased. This happens as the STATISTICS table is updated with each run.

    ![The Hash Match dialog shows spilled data warnings, but with fewer written pages.](media/ssms-memory-grant-feedback-pages-decreased.png 'Query execution plan showing fewer pages.')

17. Execute the query 2-3 more times. Each time, select the **Execution plan** tab and hover over the Hash Match step of the execution plan. After a few executions, you should **no longer** see a warning about spilled data.

    ![The Hash Match dialog no longer contains spilled data warnings.](media/ssms-memory-grant-feedback-fix.png 'Query execution plan with no spilled data')

    > So what happened? A query's post-execution plan in SQL Server includes the minimum required memory needed for execution and the ideal memory grant size to have all rows fit in memory. Performance suffers when memory grant sizes are incorrectly sized. Excessive grants result in wasted memory and reduced concurrency. Insufficient memory grants cause expensive spills to disk. By addressing repeating workloads, batch mode memory grant feedback recalculates the actual memory required for a query and then updates the grant value for the cached plan. **When an identical query statement is executed**, the query uses the revised memory grant size, reducing excessive memory grants that impact concurrency and fixing underestimated memory grants that cause expensive spills to disk. Row mode memory grant feedback expands on the batch mode memory grant feedback feature by adjusting memory grant sizes for both batch and row mode operators. _For more information, see [Row mode memory grant feedback](https://docs.microsoft.com/sql/relational-databases/performance/adaptive-query-processing?view=sql-server-2017#row-mode-memory-grant-feedback)._

## Task 3: Identify PII and GDPR-related compliance issues using Data Discovery & Classification in SSMS

Contoso Auto has several databases that include tables containing sensitive data, such as personally identifiable information (PII) like phone numbers, social security numbers, financial data, etc. Since some of their personnel and customer data include individuals who reside within the European Union (EU), they need to adhere to the General Data Protection Regulation ([GDPR](https://en.wikipedia.org/wiki/General_Data_Protection_Regulation)) as well. Because of this, Contoso Auto is required to provide periodic data auditing reports to identify sensitive and GDPR-related data that reside within their various databases.

With SQL Server Management Studio, they are able to identify, classify, and generate reports on sensitive and GDPR-related data by using the [SQL Data Discovery & Classification](https://docs.microsoft.com/sql/relational-databases/security/sql-data-discovery-and-classification?view=sql-server-ver15) tool. This tool introduces a set of advanced services, forming a new SQL Information Protection paradigm aimed at protecting the data, not just the database:

- **Discovery & recommendations** - The classification engine scans your database and identifies columns containing potentially sensitive data. It then provides you an easy way to review and apply the appropriate classification recommendations, as well as to manually classify columns.
- **Labeling** - Sensitivity classification labels can be persistently tagged on columns.
- **Visibility** - The database classification state can be viewed in a detailed report that can be printed/exported to be used for compliance & auditing purposes, as well as other needs.

In this exercise, you will run the SQL Data Discovery & Classification tool against their customer database, which includes personal, demographic, and sales data.

1. To get started, expand databases in the SQL Server Management Studio (SSMS) Object Explorer, right-click the `sales_XXXXX` database (where XXXXX is the unique identifier assigned to you for this workshop), and then choose **Tasks > Data Discovery and Classification > Classify Data...**.

   ![The sales database, Tasks menu, and Classify Data items are highlighted.](media/ssms-ddc-classify-data-link.png 'Data Classification')

2. When the tool runs, it will analyze all of the columns within all of the tables and recommend appropriate data classifications for each. What you should see is the Data Classification dashboard showing no currently classified columns, and a classification recommendations box at the top showing that there are 45 columns that the tool identified as containing sensitive (PII) or GDPR-related data. **Click** on this classification recommendations box.

   ![The data classification recommendations box is highlighted.](media/ssms-classification-recommendations-box.png 'Data classification recommendations box')

3. The list of recommendations displays the schema, table, column, type of information, and recommended sensitivity label for each identified column. You can change the information type and sensitivity labels for each if desired. In this case, accept all recommendations by **checking the checkbox** in the recommendations table header.

   ![The recommendations are shown with each checkbox checked.](media/ssms-recommendations.png 'Classification recommendations')

4. Click **Accept selected recommendations**.

   ![The Accept selected recommendations button is highlighted.](media/ssms-accept-selected-recommendations.png 'Accept selected recommendations')

5. Click **Save** in the toolbar above to apply your changes.

   ![The Save button is highlighted.](media/ssms-save-classification-changes.png 'Save classification changes')

6. After the changes are saved, click **View Report**.

   ![The View Report button is highlighted.](media/ssms-view-report.png 'View Report')

7. What you should see is a report with a full summary of the database classification state. When you right-click on the report, you can see options to print or export the report in different formats.

   ![The report is displayed, as well as the context menu showing export options after right-clicking on the report.](media/ssms-report.png 'SQL Data Classification Report')

## Task 4: Fix compliance issues with dynamic data masking

Some of the columns identified by the Data Discovery & Classification tool as containing sensitive (PII/GDPR) information include phone numbers, email addresses, billing addresses, and credit card numbers. One way to ensure compliance with various rules and regulations that enforce policies to protect such sensitive data is to prevent those who are not authorized from seeing it. An example would be displaying `XXX-XXX-XX95` instead of `123-555-2695` when outputting a phone number within a SQL query result, report, web page, etc. This is commonly called data masking. Traditionally, modifying systems and applications to implement data masking can be challenging. This is especially true when the masking has to apply all the way down to the data source level. Fortunately, SQL Server and its cloud-related product, Azure SQL Database, provides a feature named [Dynamic Data Masking](https://docs.microsoft.com/sql/relational-databases/security/dynamic-data-masking?view=sql-server-ver15) (DDM) to automatically protect this sensitive data from non-privileged users.

![Dynamic Data Masking graphic.](media/dynamic-data-masking.png 'Dynamic Data Masking')

DDM helps prevent unauthorized access to sensitive data by enabling customers to designate how much of the sensitive data to reveal with minimal impact on the application layer. DDM can be configured on the database to hide sensitive data in the result sets of queries over designated database fields, while the data in the database is not changed. Dynamic Data Masking is easy to use with existing applications, since masking rules are applied in the query results. Many applications can mask sensitive data without modifying existing queries.

In this task, you will apply DDM to one of the database fields so you can see how to address the reported compliance issues. To test the data mask, you will create a test user and query the field as that user.

1. To get started, expand databases in the SQL Server Management Studio (SSMS) Object Explorer, right-click the `sales_XXXXX` database (where XXXXX is the unique identifier assigned to you for this workshop), and then select **New Query**.

   ![The sales database and New Query menu item are highlighted.](media/ssms-sales-new-query.png 'New Query')

2. Add a dynamic data mask to the existing `dbo.customer.c_last_name` field by pasting the below query into the new query window:

   ```sql
   ALTER TABLE dbo.customer
   ALTER COLUMN c_last_name ADD MASKED WITH (FUNCTION = 'partial(2,"XXX",0)');
   ```

   > The `partial` custom string masking method above exposes the first two characters and adds a custom padding string after for the remaining characters. The parameters are: `prefix,[padding],suffix`

3. Execute the query by selecting the **Execute** button in the SSMS toolbar, or pressing the _F5_ key on your keyboard.

   ![The dynamic data mask query is shown and the Execute button is highlighted above.](media/ssms-execute-ddm-query.png 'Execute query')

4. Clear the query window and replace the previous query with the following to add a dynamic data mask to the `dbo.customer.c_email_address` field:

   ```sql
   ALTER TABLE dbo.customer
   ALTER COLUMN c_email_address ADD MASKED WITH (FUNCTION = 'email()');
   ```

   > The `email` masking method exposes the first letter of an email address and the constant suffix ".com", in the form of an email address: `aXXX@XXXX.com`.

5. Clear the query window and replace the previous query with the following, selecting all rows from the customer table:

   ```sql
   SELECT * FROM dbo.customer
   ```

   ![The query results are shown with no mask applied to the Postal Code field.](media/ssms-ddm-results-no-mask.png 'Query results')

6. Notice that the full last name and email address values are visible. That is because the user you are logged in as is a privileged user. Let's create a new user and execute the query again:

   ```sql
   CREATE USER TestUser WITHOUT LOGIN;
   GRANT SELECT ON dbo.customer TO TestUser;

   EXECUTE AS USER = 'TestUser';
   SELECT * FROM dbo.customer;
   REVERT;
   ```

7. Run the query by selecting the **Execute** button. Observe that with the the `c_last_name` and `c_email_address` filed values are now masked, hiding the full value contained within the field and preventing expose of sensitive information.

   ![The query results are shown with the mask applied to the c_last_name and c_email_address fields.](media/ssms-ddm-results-mask.png 'Query results')

## Task 5: Restrict data access with Row-level security

[Row-Level Security](https://docs.microsoft.com/en-us/sql/relational-databases/security/row-level-security?view=sql-server-ver15) (RLS) enables you to use group membership or execution context to control access to rows in a database table. RLS simplifies the design and coding of security in your application by helping you implement restrictions on data row access. For example, a hospital can create a security policy that allows doctors to view data rows for their patients only. Another example is a multi-tenant application can create a policy to enforce a logical separation of each tenant's data rows from every other tenant's rows. Efficiencies are achieved by the storage of data for many tenants in a single table. Each tenant can see only its data rows.

![Row-level security graphic.](media/row-level-security.png 'Row-level security')

The access restriction logic is located in the database tier rather than away from the data in another application tier. The database system applies the access restrictions every time that data access is attempted from any tier. This makes your security system more reliable and robust by reducing the surface area of your security system.

In this task, you will apply row-level security to one of the database tables and run some queries using multiple user contexts to see how data access is restricted.

1. To get started, expand databases in the SQL Server Management Studio (SSMS) Object Explorer, right-click the `sales_XXXXX` database (where XXXXX is the unique identifier assigned to you for this workshop), and then select **New Query**.

   ![The sales database and New Query menu item are highlighted.](media/ssms-sales-new-query.png 'New Query')

2. Within the `store` table, there are three stores located in Kentucky. Using RLS, we will restrict access to sales records so that user can only see sales from their own store, while the regional manager for Kentucky will be able to see all sales data for the state. In the new query window, enter the following SQL commands to create users that will be used for querying data secured by RLS with different contexts, and then grant them `SELECT` permissions on the `dbo.store_sales` table. Run the query by selecting **Execute** on the SSMS toolbar.

   ```sql
   CREATE USER ManagerKY WITHOUT LOGIN;
   CREATE USER Elmwood WITHOUT LOGIN;
   CREATE USER Egypt WITHOUT LOGIN;
   CREATE USER Bedford WITHOUT LOGIN;
   GO

   GRANT SELECT ON dbo.store_sales TO ManagerKY;
   GRANT SELECT ON dbo.store_sales TO Elmwood;
   GRANT SELECT ON dbo.store_sales TO Egypt;
   GRANT SELECT ON dbo.store_sales TO Bedford;
   GO
   ```

   > Row-level security will be applied to the `dbo.store_sales` table, and you will run queries against that to observe how RLS affects the results displayed for users with different contexts.

3. Next, you will create a new schema, and an inline table-valued function that will be used to apply the RLS policy. The function returns 1 when a row in the `ss_store_sk` column is the same as the store for the user executing the query (`s.s_store_sk = @StoreId`) and the store name is the same of the user's store (`s.s_store_name = USER_NAME()`), or if the user executing the query is the Manager user (`USER_NAME() = 'ManagerKY'`). Create a new query window and paste the following SQL script into the window, and then select **Execute**.

   ```sql
   CREATE SCHEMA Security;
   GO

   CREATE FUNCTION Security.fn_securitypredicate(@StoreId AS INT)
     RETURNS TABLE
   WITH SCHEMABINDING
   AS
   RETURN (SELECT 1 AS fn_securitypredicate_result
      FROM dbo.store_sales ss
         INNER JOIN dbo.store s ON ss.ss_store_sk = s.s_store_sk
      WHERE s.s_state = 'KY'
      AND s.s_store_sk = @StoreId
      AND (s.s_store_name = USER_NAME() OR USER_NAME() = 'ManagerKY'));
   GO
   ```

   > RLS is implement by using the [`CREATE SECURITY POLICY`](https://docs.microsoft.com/en-us/sql/t-sql/statements/create-security-policy-transact-sql?view=sql-server-2017) Transact-SQL statement, and predicates created as [inline table-valued functions](https://docs.microsoft.com/en-us/sql/relational-databases/user-defined-functions/create-user-defined-functions-database-engine?view=sql-server-2017).

4. The next step is to create a security policy by adding the function as a filter predicate. The state must be set to ON to enable the policy. Create a new query window and paste the following SQL script into the window, and then select **Execute**.

   ```sql
   CREATE SECURITY POLICY SalesFilter
   ADD FILTER PREDICATE Security.fn_securitypredicate(ss_store_sk)
   ON dbo.store_sales
   WITH (STATE = ON);
   GO
   ```

   > RLS filter predicates are functionally equivalent to appending a `WHERE` clause to a query against the target table. The predicate can be as sophisticated as business practices dictate, or the clause can be as simple as `WHERE TenantId = 42`.

5. Allow `SELECT` permissions to the `fn_securitypredicate` function by executing the following query in a new query window.

   ```sql
   GRANT SELECT ON security.fn_securitypredicate TO ManagerKY;
   GRANT SELECT ON security.fn_securitypredicate TO Elmwood;
   GRANT SELECT ON security.fn_securitypredicate TO Egypt;
   GRANT SELECT ON security.fn_securitypredicate TO Bedford;
   GO
   ```

6. Now test the filtering predicate, by selecting from the `store_sales` table as each user. Paste each of the queries below into a new query window and run each the commands. Observe the count of records returned, as well as the data in the `ss_store_sk` column.

   ```sql
   EXECUTE AS USER = 'Elmwood';
   SELECT * FROM dbo.store_sales;
   REVERT;
   ```

   ![Results for query run within the Elmwood user context.](media/rls-result-elmwood.png 'Query results')

   ```sql
   EXECUTE AS USER = 'Egypt';
   SELECT * FROM dbo.store_sales;
   REVERT;
   ```

   ![Results for query run within the Egypt user context.](media/rls-result-egypt.png 'Query results')

   ```sql
   EXECUTE AS USER = 'Bedford';
   SELECT * FROM dbo.store_sales;
   REVERT;
   ```

   ![Results for query run within the Bedford user context.](media/rls-result-bedford.png 'Query results')

   ```sql
   EXECUTE AS USER = 'ManagerKY';
   SELECT * FROM dbo.store_sales;
   REVERT;
   ```

   ![Results for query run within the ManagerKY user context.](media/rls-results-manager.png 'Query results')

> As you can see within each set of results, users in a Store context see only the data for their store, while queries run within the Manager context see data for all three stores. Row-Level Security makes it easy to implement filtering to restrict access to data within SQL Server tables.

## Task 6: Always Encrypted with secure enclaves

[Always Encrypted](https://docs.microsoft.com/en-us/sql/relational-databases/security/encryption/always-encrypted-database-engine?view=sql-server-ver15) is a feature designed to protect sensitive data, such as credit card numbers or national identification numbers (for example, U.S. social security numbers), stored in SQL Server databases. It allows clients to encrypt sensitive data inside client applications and never reveal the encryption keys to the Database Engine. As a result, Always Encrypted provides a separation between those who own the data (and can view it) and those who manage the data (but should have no access). By ensuring on-premises database administrators, cloud database operators, or other high-privileged, but unauthorized users, cannot access the encrypted data, Always Encrypted enables customers to confidently store sensitive data outside of their direct control.

Always Encrypted protects data by encrypting it on the client side and never allowing the data or the corresponding cryptographic keys to appear in plaintext inside the SQL Server Engine. As a result, the functionality on encrypted columns inside the database is severely restricted. The only operations SQL Server could perform on encrypted data were equality comparisons (and equality comparisons were only available with deterministic encryption). All other operations, including cryptographic operations (initial data encryption or key rotation), or rich computations (for example, pattern matching) were not supported inside the database. Users needed to move the data outside of the database to perform these operations on the client-side.

[Always Encrypted with secure enclaves](https://docs.microsoft.com/en-us/sql/relational-databases/security/encryption/always-encrypted-enclaves?view=sql-server-ver15) addresses these limitations by allowing computations on plaintext data inside a secure enclave on the server side. A secure enclave is a protected region of memory within the SQL Server process, and acts as a trusted execution environment for processing sensitive data inside the SQL Server engine. A secure enclave appears as a black box to the rest of the SQL Server and other processes on the hosting machine. There is no way to view any data or code inside the enclave from the outside, even with a debugger.

![Always Encrypted with secure enclaves](media/always-encrypted-with-secure-enclaves.png 'Always Encrypted with secure enclaves')

When parsing an application's query, the SQL Server Engine determines if the query contains any operations on encrypted data that require the use of the secure enclave. For queries where the secure enclave needs to be accessed:

- The client driver sends the column encryption keys required for the operations to the secure enclave (over a secure channel).
- Then, the client driver submits the query for execution along with the encrypted query parameters.

During query processing, the data or the column encryption keys are not exposed in plaintext in the SQL Server Engine outside of the secure enclave. The SQL Server engine delegates cryptographic operations and computations on encrypted columns to the secure enclave. If needed, the secure enclave decrypts the query parameters and/or the data stored in encrypted columns and performs the requested operations.

### Configure a secure enclave

In this task, you will use SSMS to verify that Always Encrypted with secure enclaves is loaded on the SQL Server instance.

> **NOTE**: When you use SSMS to configure Always Encrypted, SSMS handles both Always Encrypted keys and sensitive data, so both the keys and the data appear in plaintext inside the SSMS process. As the primary goal of Always Encrypted is to ensure encrypted sensitive data is safe even if the database system gets compromised, this approach is recommended only for development and non-production environments.

1. Return to SSMS, open a new query window, and verify Always Encrypted with secure enclaves is loaded by running the following query:

   ```sql
   SELECT [name], [value], [value_in_use] FROM sys.configurations
   WHERE [name] = 'column encryption enclave type';
   ```

2. The query should return the following result:

   | name                           | value | value_in_use |
   | ------------------------------ | ----- | ------------ |
   | column encryption enclave type | 1     | 1            |

3. Next, enable [rich computations](https://docs.microsoft.com/en-us/sql/relational-databases/security/encryption/configure-always-encrypted-enclaves?view=sql-server-ver15#configure-a-secure-enclave) on encrypted columns by running the following query:

   ```sql
   DBCC traceon(127,-1)
   ```

   > **NOTE**: Rich computations are disabled by default in SQL Server 2019 preview. They need to be enabled using the above statement after each restart of your SQL Server instance.

### Provision enclave-enabled keys

In this step, you will create a column master key and a column encryption key that allow enclave computations.

1. Expand databases in the SQL Server Management Studio (SSMS) Object Explorer, then expand the `sales_XXXXX` database (where XXXXX is the unique identifier assigned to you for this workshop), and locate the **Security** folder under the database.

   ![The sales database and Security folder are highlighted.](media/ssms-sales-security.png 'Database security')

2. Expand the **Security** folder, right-click **Always Encrypted Keys**, and then select **New Column Master Key...**"

   ![New Column master key is highlighted in the Always Encrypted Keys context menu.](media/ssms-sales-new-column-master-key.png 'New Column master key')

3. In the New Column Master Key dialog, enter **TI-MK** as the name, ensure the Key store is set to **Windows Certificate Store - Current User**, and then select **Generate Certificate**.

   ![On the New Column Master Key dialog, the name and key store fields are highlighted and the Generate Certificate button is highlighted.](media/ssms-new-column-master-key-dialog.png 'New Column Master Key')

4. Ensure the generated key is highlighted in the list of keys, and then select **OK** in the New Column Master Key dialog.

   ![The generated column master key is highlighted and selected in the keys list.](media/ssms-new-column-master-key-keys.png 'Keys')

5. Right-click on **Always Encrypted Keys** again, and this time select **New Column Encryption Key...**

   ![New Column Encryption Key is highlighted in the Always Encrypted Keys context menu.](media/ssms-sales-new-column-encryption-key.png 'New Column Encryption Key')

6. In the **New Column Encryption Key** dialog, enter **TI-EK** for the name, select **TI-MK** for the Column master key, and select **OK**.

   ![In the New Column Encryption Key dialog, the name is set to TI-EK and TI-MK is selected as the Column master key.](media/ssms-new-column-encryption-key-dialog.png 'New Column Encryption Key')

### Encrypt customer email column

In this step, you will encrypt the data stored in the email column inside the server-side enclave, and then test a SELECT query of the data. For testing purposes, you will open two new query windows. One of the query windows will be configured with Always Encrypted enabled and the other will have it disabled, so you can see the results of running with Always Encrypted with secure enclaves.

1. In SSMS, select the `sales` database in the Object Explorer, and then select **New query** in the SSMS toolbar to open a new query window.

2. Right-click anywhere in the new query window, and select **Connection > Change Connection**.

   ![Change Connection is highlighted under Connection in the query window context menu.](media/new-query-change-connection.png 'Change connection')

3. In the Connect to Database Engine dialog, select **Options**, navigate to the **Always Encrypted** tab, then check the **Enable Always Encrypted** box and enter the `AttestationServerUrl` from the value sheet you were provided for this lab into the Enclave Attestation URL box. For example, `http://168.61.49.245/Attestation`.

   ![On the Always Encrypted tab of the Connect to Database Engine dialog, Enable Always Encrypted is checked, and the URL above is entered into the Enclave Attestation URL field.](media/ssms-connect-enable-always-encrypted-checked.png "Connect to Database Engine")

4. Select **Connect**.

5. Now, on the SSMS toolbar, change the database back to your **sales_XXXXX** database by selecting the drop down arrow next to **master** and selecting the **sales_XXXXX** database ((where XXXX is the unique identifier assigned to you for this lab).

   ![The sales_XXXXX database is highlighted in the database selected drop down on the SSMS toolbar.](media/ssms-change-database.png "Change database")

6. In the query window, copy and paste the following query to encrypt the `c_email_address` column in the `dbo.customer` table.

   ```sql
   DROP INDEX [cci_customer] ON [dbo].[customer]
   GO

   ALTER TABLE [dbo].[customer]
   ALTER COLUMN [c_email_address] [char](50) COLLATE Latin1_General_BIN2
   ENCRYPTED WITH (COLUMN_ENCRYPTION_KEY = [TI-EK], ENCRYPTION_TYPE = Randomized, ALGORITHM = 'AEAD_AES_256_CBC_HMAC_SHA_256') NULL
   WITH (ONLINE = ON);

   ALTER DATABASE SCOPED CONFIGURATION CLEAR PROCEDURE_CACHE;

   CREATE CLUSTERED COLUMNSTORE INDEX [cci_customer] ON [dbo].[customer] WITH (DROP_EXISTING = OFF, COMPRESSION_DELAY = 0) ON [PRIMARY]
   GO
   ```

   > **NOTE**: There are two additional aspects of the query above you should take note of: 1.) The `DROP INDEX` statement listed first will drop the Clustered ColumnStore index on the `customer` table, as the encryption cannot be applied while that is in place. The final statement is a `CREATE INDEX` command to recreate it. 2.) Notice the `ALTER DATABASE SCOPED CONFIGURATION CLEAR PROCEDURE_CACHE` statement to clear the query plan cache for the database in the above script. After you have altered the table, you need to clear the plans for all batches and stored procedures that access the table, to refresh parameters encryption information.

7. If you see a dialog appear after executing the above script, asking whether to enable parameterization for Always Encrypted, click **Enable**.

   ![The Enable button is highlighted on the dialog.](media/ssms-parameterization-always-encrypted.png "Parameterization for Always Encrypted")

8. Open another new query window in SSMS, and right-click anywhere in the new query window, then select **Connection > Change Connection**.

9. In the Connect to Database Engine dialog, select **Options**, navigate to the **Always Encrypted** tab and make sure **Enable Always Encrypted** is not checked, and then select **Connect**.

   ![On the Always Encrypted tab of the Connect to Database Engine dialog, Enable Always Encrypted is unchecked.](media/ssms-connect-enable-always-encrypted-unchecked.png "Connect to Database Engine")

10. As you did previously, on the SSMS toolbar, change the database back to your **sales_XXXXX** database by selecting the drop down arrow next to **master** and selecting the **sales_XXXXX** database ((where XXXX is the unique identifier assigned to you for this lab).

    ![The sales_XXXXX database is highlighted in the database selected drop down on the SSMS toolbar.](media/ssms-change-database.png "Change database")

11. To verify the `c_email_address` column is now encrypted, paste in and execute the below statement in the query window with Always Encrypted disabled. The query window should return encrypted values in the SSN and Salary columns.

    ```sql
    SELECT c_customer_sk, c_first_name, c_last_name, c_email_address FROM [dbo].[customer]
    ```

    ![Encrypted email address is highlighted in the query results.](media/always-encrypted-without-secure-enclave-results.png "Always Encrypted with secure enclaves")

12. Now, run the same query in the query window with the Always Encrypted enabled. The returned results should contain the data decrypted.

    ![Decrypted email address is highlighted in the query results.](media/always-encrypted-with-secure-enclave-results.png "Always Encrypted with secure enclaves")

    > Using a secure enclave, you are now able to run queries against columns using Always Encrypted, and view decrypted results in SSMS.

### Run rich queries against an encrypted column

Now, you can run rich queries against the encrypted columns. Some query processing will be performed inside your server-side enclave.

1. Ensure that Parameterization for Always Encrypted is enabled, by selecting **Query** from the main menu of SSMS, and then selecting **Query Option...**.

   ![In the SSMS Query menu, Query options is highlighted.](media/ssms-query-menu.png 'SSMS Query menu')

2. On the Query Options dialog, select **Advanced** under Execution, and then check the box for **Enable Parameterization for Always Encrypted**.

   ![In the Query Options dialog, Advanced is selected and highlighted under Execution, and the Enable Parameterization for Always Encrypted box is checked.](media/query-options.png 'Query options')

3. Select **OK**.

4. In the query window with Always Encrypted enabled, paste in and execute the below query. The query should return plaintext values and rows meeting the specified search criteria, which are records with email addresses with `@gmx.` in them.

   ```sql
   DECLARE @EmailPattern [char](50) = '%@gmx.%';

   SELECT c_customer_sk, c_first_name, c_last_name, c_email_address FROM [dbo].[customer]
   WHERE c_email_address LIKE @EmailPattern;
   ```

   ![Decrypted email address is highlighted in the query results.](media/always-encrypted-rich-results-success.png "Always Encrypted with secure enclaves")

   > In the results, notices that all email address have a domain of `@gmx`.

5. Try the same query again in the query window that does not have Always Encrypted enabled, and note the failure that occurs. You will see an error similar to the following:

   ```sql
   Msg 33277, Level 16, State 6, Line 5
   Encryption scheme mismatch for columns/variables '@EmailPattern'. The encryption scheme for the columns/variables is (encryption_type = 'PLAINTEXT') and the expression near line '4' expects it to be Randomized, , a BIN2 collation for string data types, and an enclave-enabled column encryption key, or PLAINTEXT.
   ```

   ![The error message above is displayed in the Messages pane.](media/always-encrypted-rich-results-error.png "Always Encrypted with secure enclaves")

## Wrap-up

Thank you for participating in the SQL Server 2019 DBA experience! We hope you are excited about the new capabilities, and will refer back to this experience to learn more about these features.

To recap, you experienced:

1. Using migration tools to evaluate and perform a migration from SQL Server 2008 R2 to a newer version of SQL Server.
2. Intelligent Query Processing (QP) performance improvements with SQL Server 2019's new database compatibility level: `150`.
3. Using the [SQL Data Discovery & Classification](https://docs.microsoft.com/sql/relational-databases/security/sql-data-discovery-and-classification?view=sql-server-ver15) tool to identify and tag PII and GDPR-related compliance issues.
4. Used dynamic data masking to automatically protect sensitive data from unauthorized users.
5. Enabled Row-Level Security to restrict access to data rows using user contexts.
6. Using Always Encrypted with secure enclaves to perform rich queries on encrypted data columns.

## Additional resources and more information

- [What's new in SQL Server 2019 preview](https://docs.microsoft.com/en-us/sql/sql-server/what-s-new-in-sql-server-ver15?view=sql-server-ver15)
- [Security Center for SQL Server Database Engine and Azure SQL Database](https://docs.microsoft.com/en-us/sql/relational-databases/security/security-center-for-sql-server-database-engine-and-azure-sql-database?view=sql-server-2017)
- [SQL Data Discovery and Classification tool documentation](https://docs.microsoft.com/en-us/sql/relational-databases/security/sql-data-discovery-and-classification?view=sql-server-2017)
- [Intelligent query processing in SQL databases](https://docs.microsoft.com/en-us/sql/relational-databases/performance/intelligent-query-processing?view=sql-server-2017)
- [What's new in SQL Server Machine Learning Services](https://docs.microsoft.com/en-us/sql/advanced-analytics/what-s-new-in-sql-server-machine-learning-services?view=sql-server-ver15)
- [Learning content in GitHub: SQL Server Workshops](https://github.com/Microsoft/sqlworkshops)
- [SQL Server Samples Repository in GitHub. Feature demos, code samples etc.](https://github.com/Microsoft/sql-server-samples)
- [Always Encrypted with secure enclaves](https://docs.microsoft.com/en-us/sql/relational-databases/security/encryption/always-encrypted-enclaves?view=sql-server-ver15)
