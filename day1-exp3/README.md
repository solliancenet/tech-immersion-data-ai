# Day 1, Experience 3 - Unlocking new capabilities with friction-free migrations to Azure SQL Database Managed Instance

- [Day 1, Experience 3 - Unlocking new capabilities with friction-free migrations to Azure SQL Database Managed Instance](#day-1-experience-3---unlocking-new-capabilities-with-friction-free-migrations-to-azure-sql-database-managed-instance)
  - [Technology overview](#technology-overview)
    - [Migrate your SQL Server databases without changing your apps](#migrate-your-sql-server-databases-without-changing-your-apps)
    - [Accelerate your database migration](#accelerate-your-database-migration)
    - [Maximize ROI by migrating to the cloud](#maximize-roi-by-migrating-to-the-cloud)
  - [Scenario overview](#scenario-overview)
  - [Task 1: Perform database assessments for migration](#task-1-perform-database-assessments-for-migration)
  - [Task 2: Migrate the database to SQL MI](#task-2-migrate-the-database-to-sql-mi)
  - [Task 3: Update the web application to use the new SQL MI database](#task-3-update-the-web-application-to-use-the-new-sql-mi-database)
  - [Task 4: Enable Dynamic Data Masking](#task-4-enable-dynamic-data-masking)
  - [Task 5: Add clustered columnstore index](#task-5-add-clustered-columnstore-index)
  - [Task 6: Use online secondary for read-only queries](#task-6-use-online-secondary-for-read-only-queries)
  - [Task 7: Review Advanced Data Security Vulnerability Assessment](#task-7-review-advanced-data-security-vulnerability-assessment)
  - [Task 8: SQL Data Discovery and Classification](#task-8-sql-data-discovery-and-classification)
  - [Wrap-up](#wrap-up)
  - [Additional resources and more information](#additional-resources-and-more-information)

## Technology overview

### Migrate your SQL Server databases without changing your apps

[Azure SQL Database Managed Instance](https://docs.microsoft.com/azure/sql-database/sql-database-managed-instance) (SQL MI) is a new deployment option of Azure SQL Database which enables the migration of existing on-premises SQL Server databases to the cloud with minimal or no application and database changes. With SQL MI, you get the broadest SQL Server engine compatibility and native virtual network (VNET) support. This option gives you the best of SQL Server, plus the operational and cost benefits of an intelligent, fully managed service. SQL MI is ideal for migrating a large number of existing SQL Server databases from on-premises or virtual machines to SQL Database.

![Diagram outlining the key features of managed instances.](media/azure-sql-database-managed-instance.png "What is SQL MI?")

### Accelerate your database migration

Reduce the complexity of your cloud migration by using a single comprehensive service instead of multiple tools. [Azure Database Migration Service](https://docs.microsoft.com/azure/dms/dms-overview) is designed as a seamless, end-to-end solution for moving on-premises SQL Server databases to the cloud. Use the [Database Migration Guide](https://datamigration.microsoft.com/) for recommendations, step-by-step guidance, and expert tips on your specific database migration.

### Maximize ROI by migrating to the cloud

Reduce the burden of data-tier management and save time and costs by migrating workloads to the cloud. [Azure Hybrid Benefit](https://azure.microsoft.com/pricing/hybrid-benefit/) for SQL Server provides a cost-effective path for migrating hundreds or thousands of SQL Server databases with minimal effort. Use your SQL Server licenses with Software Assurance to pay a reduced rate when migrating to the cloud. Save up to 55 percent with Azure Hybrid Benefit, and up to 80 percent with [reserved capacity](https://docs.microsoft.com/azure/sql-database/sql-database-reserved-capacity). Learn how [customers have increased productivity](https://azure.microsoft.com/resources/forrester-tei-sql-database-managed-instance/) by up to 40 percent by migrating to Azure SQL Database.

## Scenario overview

ContosoAuto runs their operations and finance database, `ContosoAutoDb`, on an on-premises SQL Server 2008 R2 database. This system is vital to the company's daily activities and as SQL Server 2008 R2 is approaching end of support, they are looking at options for migrating this database into Azure. They have read about some of the advanced security and performance tuning options that are available only in Azure and would prefer to a migrate the database into a platform-as-a-service (PaaS) offering, if possible.

ContosoAuto is using the Service Broker feature of SQL Server within the `ContosoAutoDb` database. Service Broker is a feature of SQL Server used for sending and receiving guaranteed, asynchronous messages by using extensions to the Transact-SQL Data Manipulation Language (DML). This functionality is being used for several critical business processes, and they cannot afford to lose this capability when migrating their operations database to the cloud. They have also stated that, at this time, they do not have the resources to rearchitect the solution to use an alternative message broker.

In this experience, you will use the Microsoft Data Migration Assistant (DMA) to perform assessments of feature parity and compatibility against both Azure SQL Database and Azure SQL Database Managed Instance, with the goal of migrating the `ContosoAutoDb` database into an Azure PaaS offering with minimal or no changes. After completing the assessments, you will perform the database migration and then update ContosoAuto's operations web application to use the new database. Once that is complete, you will review and enable some of the database features that are only available in Azure.

## Task 1: Perform database assessments for migration

In this task, you will use the Microsoft [Data Migration Assistant](https://docs.microsoft.com/sql/dma/dma-overview?view=azuresqldb-mi-current) (DMA) to perform assessments on the `ContosoAutoDb` database. You will create two assessments, one for a migration to Azure SQL Database, and then a second for SQL MI. These assessments will provide reports about any feature parity and compatibility issues between the on-premises database and the Azure managed SQL database service options.

> DMA helps you upgrade to a modern data platform by detecting compatibility issues that can impact database functionality in your new version of SQL Server or Azure SQL Database. DMA recommends performance and reliability improvements for your target environment and allows you to move your schema, data, and uncontained objects from your source server to your target server.

1. Launch the Microsoft Data Migration Assistant from the Windows Start menu within your lab environment.

   ![The Microsoft Data Migration Assistant is highlighted in the Windows start menu.](media/windows-start-menu-dma.png "Data Migration Assistant")

2. In the DMA dialog, select **+** from the left-hand menu to create a new project.

   ![The new project icon is highlighted in DMA.](media/dma-new.png "New DMA project")

3. In the New project pane, set the following:

   - **Project type**: Select Assessment.
   - **Project name**: Enter ToAzureSqlDb.
   - **Source server type**: Select SQL Server.
   - **Target server type**: Select Azure SQL Database.

   ![New project settings for doing an assessment of a migration from SQL Server to Azure SQL Database.](media/dma-new-project-to-azure-sql-db.png "New project settings")

4. Select **Create**.

5. On the **Options** screen, ensure **Check database compatibility** and **Check feature parity** are both checked, and then select **Next**.

   ![Check database compatibility and check feature parity are checked on the Options screen.](media/dma-options.png "DMA options")

6. On the **Sources** screen, enter the following into the **Connect to a server** dialog that appears on the right-hand side:

   - **Server name**: Enter the DNS name of the shared sqlServer2008R2 VM, **`sqlserver2008r2.westus.cloudapp.azure.com`**.
   - **Authentication type**: Select **SQL Server Authentication**.
   - **Username**: Enter **WorkshopUser**.
   - **Password**: Enter **Password.1!!**.
   - **Encrypt connection**: Check this box.
   - **Trust server certificate**: Check this box.

   ![In the Connect to a server dialog, the values specified above are entered into the appropriate fields.](media/dma-connect-to-a-server.png "Connect to a server")

7. Select **Connect**.

8. On the **Add sources** dialog that appears next, check the box for **ContosoAutoDb** and select **Add**.

   ![The ContosoAutoDb box is checked on the Add sources dialog.](media/dma-add-sources.png "Add sources")

9. Select **Start Assessment**.

   ![Start assessment](media/dma-start-assessment-to-azure-sql-db.png "Start assessment")

10. Review the assessment of ability to migrate to Azure SQL Database.

    ![For a target platform of Azure SQL Database, feature parity shows two features which are not supported in Azure SQL Database. The Service broker feature is selected on the left and on the right Service Broker feature is not supported in Azure SQL Database is highlighted.](media/dma-feature-parity-service-broker-not-supported.png "Database feature parity")

    > The DMA assessment for a migrating the `ContosoAutoDb` database to a target platform of Azure SQL Database shows two features in use which are not supported in Azure SQL Database. These features, cross-database references and Service broker, will prevent ContosoAuto from being able to migrate to the Azure SQL Database PaaS offering.

11. With one PaaS offering ruled out due to feature parity, you will now perform a second assessment, this time for a migration to Azure SQL Database Managed Instance (SQL MI). To get started, select **+** on the left-hand menu in DMA to create another new project.

    ![The new project icon is highlighted in DMA.](media/dma-new.png "New DMA project")

12. In the New project pane, set the following:

    - **Project type**: Select Assessment.
    - **Project name**: Enter ToAzureSqlMi.
    - **Source server type**: Select SQL Server.
    - **Target server type**: Select Azure SQL Database Managed Instance.

    ![New project settings for doing an assessment of a migration from SQL Server to Azure SQL Database Managed Instance.](media/dma-new-project-to-azure-sql-mi.png "New project settings")

13. Select **Create**.

14. On the **Options** screen, ensure **Check database compatibility** and **Check feature parity** are both checked, and then select **Next**.

    ![Check database compatibility and check feature parity are checked on the Options screen.](media/dma-options.png "DMA options")

15. On the **Sources** screen, enter the following into the **Connect to a server** dialog that appears on the right-hand side:

    - **Server name**: Enter the DNS name of the shared sqlServer2008R2 VM, **`sqlserver2008r2.westus.cloudapp.azure.com`**.
    - **Authentication type**: Select **SQL Server Authentication**.
    - **Username**: Enter **WorkshopUser**.
    - **Password**: Enter **Password.1!!**.
    - **Encrypt connection**: Check this box.
    - **Trust server certificate**: Check this box.

    ![In the Connect to a server dialog, the values specified above are entered into the appropriate fields.](media/dma-connect-to-a-server.png "Connect to a server")

16. Select **Connect**.

17. On the **Add sources** dialog that appears next, check the box for **ContosoAutoDb** and select **Add**.

    ![The ContosoAutoDb box is checked on the Add sources dialog.](media/dma-add-sources.png "Add sources")

18. Select **Start Assessment**.

    ![Start assessment](media/dma-start-assessment-to-azure-sql-mi.png "Start assessment")

19. Review the assessment of ability to migrate to Azure SQL Database Managed Instance.

    ![For a target platform of Azure SQL Database Managed Instance, there are no feature parity issues found.](media/dma-feature-parity-azure-sql-mi.png "Database feature parity")

    > The assessment report for a migrating the `ContosoAutoDb` database to a target platform of Azure SQL Database Managed Instance shows no feature parity. The database, including the cross-database references and Service broker features, can be migrated as is, providing the opportunity for ContosoAuto to have a fully managed PaaS database instance running in Azure. Previously, their options for migrating a database using features, such as Service Broker, incompatible with Azure SQL Database, were to deploy the database to a virtual machine running in Azure (IaaS) or modify their database and applications to not use the unsupported features. The introduction of Azure SQL MI, however, provides the ability to migrate databases into a managed Azure SQL database with near 100% compatibility, including the features that prevented them from using Azure SQL Database.

## Task 2: Migrate the database to SQL MI

In this task, you will migrate the `ContosoAutoDb` database from the on-premises SQL 2008 R2 database to SQL MI, targeting the [Business Critical service tier](https://docs.microsoft.com/azure/sql-database/sql-database-managed-instance#managed-instance-service-tiers).

> The Business Critical service tier is designed for business applications with the highest performance and high-availability (HA) requirements.

To migrate the `ContosoAutoDb` database from SQL 2008 R2 to SQL MI you will use a backup of the database stored in an Azure Blob storage account. `RESTORE` of native backups (.bak files) taken from SQL Server on-premises or SQL Server on Virtual Machines, available on Azure Storage, is one of key capabilities of the managed instance deployment option that enables quick and easy offline database migration. The following diagram provides a high-level overview of the process:

![Diagram of the native RESTORE from URL capability.](media/sql-mi-native-restore.png "Native RESTORE")

1. Open **SQL Server Management Studio 17** (SSMS) from the Microsoft SQL Server Tools 17 folder in the Windows Start menu and connect to your SQL MI database. On the connection dialog enter the following:

   - **Server name**: Enter the name of the shared SQL MI server, **`tech-immersion-sqlmi-shared.521f7783692d.database.windows.net`**.
   - **Authentication**: Select **SQL Server Authentication**.
   - **Login**: Enter **tiuser**.
   - **Password**: Enter **Password.1234567890**.
   - Check the **Remember password** box.

   ![Connection dialog for SSMS.](media/ssms-connect-sql-mi.png "SSMS")

2. Select **Connect**.

3. To preform the `RESTORE` process, credentials for a pre-configured storage account and SAS token have already been added to the Managed Instance using the [create a credential](https://docs.microsoft.com/sql/t-sql/statements/create-credential-transact-sql?view=sql-server-2017) method. This process essentially creates a connection from your SQL MI database to the Blob storage account, allowing you to access files stored in the target container, `database-backup`. Because this is a shared SQL MI, only one credential is needed for all attendees. If you are curious, you would create it with a SQL statement similar to the below.

   ```sql
   CREATE CREDENTIAL [https://techimmersion.blob.core.windows.net/labfiles/data/3]
   WITH IDENTITY = 'SHARED ACCESS SIGNATURE',
   SECRET = 'sv=2018-03-28&ss=bfqt&srt=sco&sp=rwdlacup&se=2099-03-22T05:33:07Z&st=2019-03-21T21:33:07Z&spr=https&sig=xCq7hZfgdtM1UaN9%2FToz04GT5d5RsKaeb1JjWrpuKHE%3D'
   ```

4. You can verify the credential's access to the Blob storage account by selecting **New Query** again from the SSMS toolbar. Paste the following SQL script to get a backup file list from the storage account into the new query window and select **Execute** from the toolbar.

   ```sql
   RESTORE FILELISTONLY FROM URL = 'https://techimmersion.blob.core.windows.net/labfiles/data/3/ContosoAutoDb.bak'
   ```

   ![Script to list files in a backup file in Blob storage.](media/ssms-sql-mi-restore-filelistonly.png "SSMS")

5. You are now ready to restore the `ContosoAutoDb` database in SQL MI. In this step, you will be creating a new database on the Managed Instance. Select **New Query** on the SSMS toolbar again, then paste the following SQL script into the new query window. **Replace the `XXXXX` value** with the unique identifier assigned to your for this workshop. The database name in the  query should look something like `ContosoAutoDb-01234`.

   ```sql
   RESTORE DATABASE [ContosoAutoDb-XXXXX] FROM URL = 'https://techimmersion.blob.core.windows.net/labfiles/data/3/ContosoAutoDb.bak'
   ```

   > **NOTE**: You may notice multiple databases in on the Managed Instance. This is because the SQL MI is a shared resource for all workshop attendees, so make sure you use your assigned unique ID when restoring and accessing the database.

6. Select **Execute** on the SSMS toolbar.

7. The restore will take 1 - 2 minutes to complete. You will receive a "Commands completed successfully" message when it is done.

8. When the restore completes, expand **Databases** in the Object Explorer, and then expand **ContosoAutoDb-XXXXX** (where XXXXX is the unique identifier assigned to you for this workshop) and **Tables**. You will see that the tables are all listed, and the SQL Server 2008 R2 database has been successfully restored into SQL MI.

    ![The Object Explorer is displayed with Databases, ContosoAutoDb, and Tables expanded.](media/ssms-sql-mi-object-explorer.png "SSMS Object Explorer")

    > NOTE: Your database name will differ from the above screen shot, in that it will contain the unique identifier assigned to you for this workshop, such as `ContosoAutoDb-01234`. The SQL Managed Instance is shared for all workshop participants, so you may also see databases for other participants.

## Task 3: Update the web application to use the new SQL MI database

With the `ContosoAutoDb` database now running on SQL MI in Azure, the next step is to make the required modifications to the ContosoAuto operations web application. The operations web app is currently running an [Azure App Service Environment](https://docs.microsoft.com/azure/app-service/environment/intro), which was provisioned in the same virtual network as the SQL Managed Instance.

> SQL Managed Instance has private IP address in its own VNet, so to connect an application you need to configure access to the VNet where Managed Instance is deployed. To learn more, read [Connect your application to Azure SQL Database Managed Instance](https://docs.microsoft.com/azure/sql-database/sql-database-managed-instance-connect-app).

In this task, you will make updates to the ContosoAuto operations web application to enable it to connect to and utilize the SQL MI database.

1. Using a web browser, navigate to the [Azure portal](https://portal.azure.com), select **Resource groups** from the left-hand menu, and then select the resource group named **tech-immersion-XXXXX** (where XXXXX is the unique identifier assigned to you for this workshop).

   ![The tech-immersion resource group is selected.](media/tech-immersion-rg.png "Resource groups")

2. Select the **Tech Immersion Web App Service** ending with your unique identifier (e.g., techimmersionwebapp01234) from the list of resources.

   ![The App Service resource is selected from the list of resources in the tech-immersion resource group.](media/tech-immersion-rg-appservice.png "Tech Immersion resource group")

3. On the App Service blade, select **Application settings** under Settings on the left-hand side.

   ![The Application settings item is selected under Settings.](media/tech-immersion-app-service-app-settings.png "Application settings")

4. On the Application settings blade, scroll down and locate the **Connection strings** section. Paste the connection string value below into the value for the `ContosoAutoDbContext` connection string.

    ```sql
    Server=tcp:tech-immersion-sqlmi-shared.521f7783692d.database.windows.net,1433;Persist Security Info=False;User ID=tiuser;Password=Password.1234567890;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;
    ```

   ![The copied SQL MI connection string is pasted into the value for the ContosoAutoDbContext connection string.](media/app-service-app-settings-connection-strings.png "Connection strings")

5. Repeat the previous step, this time pasting the same connection string into the `ContosoAutoDbReadOnlyContext` connection string.

   ![Read-only connection string.](media/app-service-app-settings-connection-strings-read-only.png "Connection strings")

6. Select **Save** at the top of the Application settings blade.

   ![The save button on the Application settings blade is highlighted.](media/application-settings-save.png "Save")

   > NOTE: The astute reader may have noticed in the above steps that the Web App continues to query a database called `ContosoAutoDb` and not the database that was just restored. This is intended only to shorten the application configuration steps. Rest assured that the changes you made to the database connection string would enable your application to reach any of the databases loaded on to the SQL Server Managed Instance.

7. Select **Overview** to the left of the Application settings blade to return to the overview blade of your App Service.

    ![Overview is highlighted on the left-hand menu for App Service](media/app-service-overview-menu-item.png "Overview menu item")

8. On the overview blade, click the **URL** of your App service to launch the website. This will open the URL in a browser window.

    ![The App service URL is highlighted.](media/app-service-url.png "App service URL")

9. Verify that the web site and data is loaded correctly. The page should look similar to the following:

    ![Screenshot of the ContosoAuto Operations Web App.](media/contosoauto-web-app.png "ContosoAuto Web")

> That is it. You were able to successfully connect your application to the new SQL MI database by simply updating the application's connection string. No code changes or other updates are needed!

## Task 4: Enable Dynamic Data Masking

[Dynamic Data Masking](https://docs.microsoft.com/azure/sql-database/sql-database-dynamic-data-masking-get-started) (DDM) limits sensitive data exposure by masking it to non-privileged users. This feature helps prevent unauthorized access to sensitive data by enabling customers to designate how much of the sensitive data to reveal with minimal impact on the application layer. It’s a policy-based security feature that hides the sensitive data in the result set of a query over designated database fields, while the data in the database is not changed.

> For example, a service representative at a call center may identify callers by several digits of their credit card number, but those data items should not be fully exposed to the service representative. A masking rule can be defined that masks all but the last four digits of any credit card number in the result set of any query. As another example, an appropriate data mask can be defined to protect personally identifiable information (PII) data, so that a developer can query production environments for troubleshooting purposes without violating compliance regulations.

In this task, you will enable DDM on the `CardNumber` field in the `CreditCard` table in the `ContosoAutoDb` database, to prevent queries against that table from returning the full credit card number.

1. Return to the SQL Server Management Studio (SSMS) window you opened previously.

2. Expand **Tables** under the **ContosoAutoDb-XXXXX** (where XXXXX is the unique identifier assigned to you for this workshop) and locate the `Sales.CreditCard` table. Expand the table columns and observe that there is a column named `CardNumber`. Right-click the table, and choose **Select Top 1000 Rows** from the context menu.

   ![The Select Top 1000 Rows item is highlighted in the context menu for the Sales.CreditCard table.](media/ssms-sql-mi-credit-card-table-select.png "Select Top 1000 Rows")

3. In the query window that opens, review the Results, including the `CardNumber` field. Notice it is displayed in plain text, making the data available to anyone with access to query the database.

   ![Plain text credit card numbers are highlighted in the query results.](media/ssms-sql-mi-credit-card-table-select-results.png "Results")

4. So we can test the mask being applied to the `CardNumber` field, you will first create a user in the database that will be used for testing the masked field. In SSMS, select **New Query** and paste the following SQL script into the new query window, replacing `XXXXX` with your unique ID:

   ```sql
   USE ContosoAutoDb-XXXXX;
   GO

   CREATE USER DDMUser WITHOUT LOGIN;
   GRANT SELECT ON [Sales].[CreditCard] TO DDMUser;
   ```

   ![A Create User query is pasted into the new query window.](media/ssms-sql-mi-ddm-create-user.png "Create User")

   > The SQL script above create a new user in the database named `DDMUser`, and grants that user `SELECT` rights on the `Sales.CreditCard` table.

5. Select **Execute** from the SSMS toolbar to run the query. You will get a message that the commands completed successfully in the Messages pane.

6. With the new user created, let's run a quick query to verify the results. Select **New Query** again, and paste the following into the new query window. Replace `XXXXX` in the `USE` statement to include the unique identifier of your database which will be `ContosoAutoDb-XXXXX` (e.g., ContosoAutoDb-01234).

   ```sql
   USE ContosoAutoDb-XXXXX;
   GO

   EXECUTE AS USER = "DDMUser';
   SELECT * FROM [Sales].[CreditCard];
   REVERT;
   ```

   ![The SQL query above is pasted into the new query window in SSMS.](media/ssms-sql-mi-ddm-select-unmasked.png "Select Unmasked")

7. Select **Execute** from the toolbar, and examine the Results pane. Notice the credit card number, as above, is visible in clear text.

   ![The credit card number is unmasked in the query results.](media/ssms-sql-mi-ddm-results-unmasked.png "Query results")

8. You will now apply DDM on the `CardNumber` field to prevent it from being viewed in query results. Select **New Query** from the SSMS toolbar and paste the following query into the query window to apply a mask to the `CardNumber` field, replacing `XXXXX` with your unique ID. Select **Execute** to run the query.

   ```sql
   USE ContosoAutoDb-XXXXX;
   GO

   ALTER TABLE [Sales].[CreditCard]
   ALTER COLUMN [CardNumber] NVARCHAR(25) MASKED WITH (FUNCTION = "partial(0,"xxx-xxx-xxx-",4)")
   ```

   ![The SQL script above is pasted into the new query window. The Execute button is highlighted and a success message is displayed in the Messages pane.](media/ssms-sql-mi-ddm-add-mask.png "Add DDM Mask")

9. Run the `SELECT` query you opened in step 7 above again, and observe the results, specifically inspect the output in the `CardNumber` field. For reference the query is below. You replaced `XXXXX` in the `USE` statement to include the unique identifier of your database which will be `ContosoAutoDb-XXXXX` (e.g., ContosoAutoDb-01234).

    ```sql
    USE ContosoAutoDb-XXXXX;
    GO

    EXECUTE AS USER = "DDMUser';
    SELECT * FROM [Sales].[CreditCard];
    REVERT;
    ```

    ![The credit card number is masked in the query results.](media/ssms-sql-mi-ddm-results-masked.png "Query results")

    > The `CardNumber` is now displayed using the mask applied to it, so only the last four digits of the card number are visible. Dynamic Data Masking is a powerful feature that enables you to prevent unauthorized users from viewing sensitive or restricted information. It’s a policy-based security feature that hides the sensitive data in the result set of a query over designated database fields, while the data in the database is not changed.

## Task 5: Add clustered columnstore index

ContosoAuto is looking to take advantage of some of the performance improvement features available in Azure SQL MI. In particular, they are interested in optimizing performance by using [In-Memory technologies](https://docs.microsoft.com/azure/sql-database/sql-database-in-memory).

In this task, you will create a new table based on the existing `[Sales].[SalesOrderDetail]` table and apply a [ColumnStore index](https://docs.microsoft.com/sql/relational-databases/indexes/columnstore-indexes-overview?view=azuresqldb-mi-current).

> Columnstore indexes are the standard for storing and querying large data warehousing fact tables. This index uses column-based data storage and query processing to achieve gains up to **10 times the query performance** in your data warehouse over traditional row-oriented storage. You can also achieve gains up to **10 times the data compression** over the uncompressed data size.

1. In SSMS, ensure you are connected to the Azure SQL Database Managed Instance.

2. Open a new query window by selecting **New Query** from the toolbar.

   ![The New Query icon is highlighted on the SSMS toolbar.](./media/ssms-toolbar-new-query.png "SSMS New Query")

3. Copy the script below, and paste it into the query window. Replace `XXXXX` in the `USE` statement to include the unique identifier of your database which will be `ContosoAutoDb-XXXXX` (e.g., ContosoAutoDb-01234).

   ```sql
   USE ContosoAutoDb-XXXXX;
   GO

   SELECT *
   INTO [Sales].[ColumnStore_SalesOrderDetail]
   FROM [Sales].[SalesOrderDetail]
   GO
   ```

4. Select **Execute** on the toolbar to run the query, and create a new table named `[Sales].[ColumnStore_SalesOrderDetail]`, populated with data from the `[Sales].[SalesOrderDetail]` table.

   ![The Execute icon is highlighted on the SSMS toolbar.](./media/ssms-toolbar-execute-query.png "Select Execute")

5. Select **New Query** in the toolbar again, and paste the following query into the new query window. The query contains multiple parts; one to get the size of the `ColumnStore_SalesOrderDetail` table, a second to create a clustered ColumnStore index on the `[Sales].[ColumnStore_SalesOrderDetail]` table, and then the size query is repeated to get the size after adding the clustered ColumnStore index. Replace `XXXXX` in the `USE` statement to include the unique identifier of your database which will be `ContosoAutoDb-XXXXX` (e.g., ContosoAutoDb-01234).

   ```sql
   USE ContosoAutoDb-XXXXX;
   GO

   -- Get the Size of the [Sales].[ColumnStore_SalesOrderDetail] table
   SELECT
   t.Name AS TableName,
   p.rows AS RowCounts,
   CAST(ROUND((SUM(a.total_pages) / 128.00), 2) AS NUMERIC(36, 2)) AS Size_MB
   FROM sys.tables t
   INNER JOIN sys.indexes i ON t.OBJECT_ID = i.object_id
   INNER JOIN sys.partitions p ON i.object_id = p.OBJECT_ID AND i.index_id = p.index_id
   INNER JOIN sys.allocation_units a ON p.partition_id = a.container_id
   WHERE t.Name = "ColumnStore_SalesOrderDetail'
   GROUP BY t.Name, p.Rows
   GO

   -- Create a clustered columnstore index on the [Sales].[ColumnStore_SalesOrderDetail] table
   CREATE CLUSTERED COLUMNSTORE INDEX [cci_SalesOrderDetail]
   ON [Sales].[ColumnStore_SalesOrderDetail]
   GO

   -- Get the Size of the [Sales].[ColumnStore_SalesOrderDetail] table
   SELECT
   t.Name AS TableName,
   p.rows AS RowCounts,
   CAST(ROUND((SUM(a.total_pages) / 128.00), 2) AS NUMERIC(36, 2)) AS Size_MB
   FROM sys.tables t
   INNER JOIN sys.indexes i ON t.OBJECT_ID = i.object_id
   INNER JOIN sys.partitions p ON i.object_id = p.OBJECT_ID AND i.index_id = p.index_id
   INNER JOIN sys.allocation_units a ON p.partition_id = a.container_id
   WHERE t.Name = "ColumnStore_SalesOrderDetail'
   GROUP BY t.Name, p.Rows
   GO
   ```

6. Select **Execute** on the toolbar to run the query.

7. In the query results, observe the `Size_MB` value of the table before and after the creation of the clustered ColumnStore index. The first value is the size before the index was created, and the second value is the size after the ColumnStore index was created.

   ![The SSMS results pane is displayed, with the size of the [Sales].[ColumnStore_SalesOrderDetail] table highlighted both before and after the creation of the clustered ColumnStore index.](media/ssms-sql-mi-columnstore-size-reduction.png "ColumnStore_SalesOrderDetail size query results")

8. Create another new query window by selecting **New Query** from the toolbar, and then select **Include Actual Execution Plan** by selecting its button in the toolbar.

   ![The Include Actual Execution Plan icon is highlighted on the New Query the toolbar.](./media/ssms-toolbar-include-actual-execution-plan.png "Select the Include Actual Execution Plan")

9. Paste the queries below into the new query window, replace `XXXXX` with your unique ID, and select **Execute** on the toolbar:

   ```sql
   USE ContosoAutoDb-XXXXX;
   GO

   SELECT ProductId, LineTotal
   FROM [Sales].[ColumnStore_SalesOrderDetail]

   SELECT ProductId, LineTotal
   FROM [Sales].[SalesOrderDetail]
   ```

   > Running queries against both the `SalesOrderDetail` and `ColumnStore_SalesOrderDetail` will allow you to compare the query execution plans between tables with and without a columnstore index.

10. In the Results pane, select the **Execution Plan** tab. Check the _Query cost (relative to the batch)_ percentage value of the two queries and compare them.

    ![The Execution Plan tab is highlighted in the Results pane, 12% is highlighted for Query 1, and 88% is highlighted for Query 2.](./media/ssms-query-results-execution-plan-columnstore-index.png "Compare the two queries")

    > From the query cost, it is clear the query against the table with the columnstore index was more performant. Using a columnstore index, queries get an order of magnitude better performance boost with _BatchMode_ processing, a unique value proposition in SQL Server. The basic idea of batch mode processing is to process multiple values, hence the term ‘batch’, together instead of one value at a time. Batch mode processing is perfectly suited for analytics where a large number of rows need to be processed, for example, to compute aggregates or apply filter predicates.

11. Run the same queries again, but this time set statistics IO on in the query by adding the following to the top of the query window:

    ```sql
    SET STATISTICS IO ON
    GO
    ```

12. Your finaly query should look like, with `XXXXX` replaced with your unique ID:

    ```sql
    USE ContosoAutoDb-XXXXX;
    GO

    SET STATISTICS IO ON
    GO

    SELECT ProductId, LineTotal
    FROM [Sales].[ColumnStore_SalesOrderDetail]

    SELECT ProductId, LineTotal
    FROM [Sales].[SalesOrderDetail]
    ```

13. Select **Execute** from the toolbar to run the query.

    > Statistics IO reports on the amount of logical pages that are read in order to return the query results.

14. Select the **Messages** tab of the Results pane, and compare two numbers, logical reads and lob logical reads. You should see a significant drop in total number of logical reads on the columns store table.

    ![Various information is highlighted on the Messages tab of the Results pane.](./media/ssms-query-results-messages-stastics-io.png "Compare the information")

## Task 6: Use online secondary for read-only queries

In this task, you will look at how you can use the automatically created online secondary for reporting, without feeling the impacts of a heavy transactional load on the primary database. Each database in the SQL MI Business Critical tier is automatically provisioned with several AlwaysON replicas to support the availability SLA.

> High availability in this architectural model is achieved by replication of compute (SQL Server Database Engine process) and storage (locally attached SSD) deployed in 4-node cluster, using technology similar to SQL Server [Always On Availability Groups](https://docs.microsoft.com/sql/database-engine/availability-groups/windows/overview-of-always-on-availability-groups-sql-server). You can read more in the [SQL Database high availability](https://docs.microsoft.com/azure/sql-database/sql-database-high-availability#premium-and-business-critical-service-tier-availability) documentation.

[**Read Scale-Out**](https://docs.microsoft.com/azure/sql-database/sql-database-read-scale-out) allows you to load balance Azure SQL Database read-only workloads using the capacity of one read-only replica. This way the read-only workload will be isolated from the main read-write workload and will not affect its performance. To learn more, check out the [SQL Database Read Scale-Out documentation](https://docs.microsoft.com/azure/sql-database/sql-database-read-scale-out).

![Business Critical service tier: collocated compute and storage.](media/sql-mi-read-scale-out.png "Read Scale-Out")

> The feature is intended for the applications that include logically separated read-only workloads, such as analytics, and therefore could gain performance benefits using this additional capacity at no extra cost.

When you enable Read Scale-Out for a database, the `ApplicationIntent` option in the connection string provided by the client dictates whether the connection is routed to the write replica or to a read-only replica. Specifically, if the `ApplicationIntent` value is `ReadWrite` (the default value), the connection will be directed to the database’s read-write replica. This is identical to existing behavior. If the `ApplicationIntent` value is `ReadOnly`, the connection is routed to a read-only replica.

For example, the following connection string connects the client to a read-only replica of the `tech-immersion-sql-mi` database:

```sql
Server=tcp:tech-immersion-sql-mi.3e134c88d9f6.database.windows.net;Database=ContosoAutoDb;User ID=tiuser;Password=Password.1234567890;Trusted_Connection=False;Encrypt=True;ApplicationIntent=ReadOnly;
```

> Note the addition of `ApplicationIntent=ReadOnly;` to the end of the connection string.

1. Using a web browser, navigate to the [Azure portal](https://portal.azure.com), select **Resource groups** from the left-hand menu, and then select the resource group named **tech-immersion**.

   ![The tech-immersion resource group is selected.](media/tech-immersion-rg.png "Resource groups")

2. In the tech-immersion resource group, select the **tech-immersion-web App Service** from the list of resources.

   ![The App Service resource is selected from the list of resources in the tech-immersion resource group.](media/tech-immersion-rg-appservice.png "Tech Immersion resource group")

3. On the App Service overview blade, select the **URL** to open the web application in a browser window.

   ![The App service URL is highlighted.](media/app-service-url.png "App service URL")

4. In the ContosoAuto web app, select **Reports** from the menu.

   ![READ_WRITE is highlighted on the Reports page.](media/contosoauto-web-reports-read-write.png "ContosoAuto Web App")

   > Note the `READ_WRITE` string on the page. This is the output from reading the `Updateability` propertry associated with the `ApplicationIntent` option on the target database. This can be retrieved using the SQL query `SELECT DATABASEPROPERTYEX(DB_NAME(), "Updateability")`.

5. Return to the App Service blade, and then select **Application settings** under Settings on the left-hand side.

   ![The Application settings item is selected under Settings.](media/tech-immersion-app-service-app-settings.png "Application settings")

6. On the Application settings blade, scroll down and locate the connection string named `ContosoAutoDbReadOnlyContext` within the **Connection strings** section.

   ![The read-only connection string is highlighted.](media/tech-immersion-app-settings-conn-string-read-only.png "Connection strings")

7. Select the **Value** for the `ContosoAutoDbReadOnlyContext` and paste the following parameter to end of the connection string.

   ```sql
   ApplicationIntent=ReadOnly;
   ```

8. Your `ContosoAutoDbReadOnlyContext` connection string should now look something like the following:

   ```sql
   Server=tcp:tech-immersion-sqlmi-shared.521f7783692d.database.windows.net,1433;Persist Security Info=False;Database=ContosoAutoDb;User ID=tiuser;Password=Password.1234567890;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;ApplicationIntent=ReadOnly;
   ```

9. Select **Save** at the top of the Application settings blade.

   ![The save button on the Application settings blade is highlighted.](media/application-settings-save.png "Save")

10. Return to the ContosoAuto operations website you opened previously, and refresh the **Reports** page. The page should now look similar to the following:

    ![READ_ONLY is highlighted on the Reports page.](media/contosoauto-web-reports-read-only.png "ContosoAuto Web App")

    > Notice the `updability` option is now displaying as `READ_ONLY`. With a simple addition to your database connection string, you are able to send read-only queries to the online secondary of your SQL MI database, allowing you to load-balance read-only workloads using the capacity of one read-only replica. The SQL MI Business Critical cluster has built-in Read Scale-Out capability that provides free-of charge built-in read-only node that can be used to run read-only queries that should not affect performance of your primary workload.

## Task 7: Review Advanced Data Security Vulnerability Assessment

[SQL Database Advance Data Security](https://docs.microsoft.com/azure/sql-database/sql-database-advanced-data-security) (ADS) provides advanced SQL security capabilities, including functionality for discovering and classifying sensitive data, surfacing and mitigating potential database vulnerabilities, and detecting anomalous activities that could indicate a threat to your database. ADS is enabled at the managed instance level by selecting **ON** on the **Advanced Data Security** blade for your managed instance. This turns ADS on for all databases on the managed instance. ADS uses an Azure Blob Storage account to save the associated outputs (e.g., assessment and vulnerability reports). In the interest of time for this workshop, the steps to enable ADS have already been performed on the shared SQL MI.

In this task, you will review an assessment report generated by [Advance Data Security](https://docs.microsoft.com/azure/sql-database/sql-database-advanced-data-security) for the `ContosoAutoDb` database and take action to remediate one of the findings in your copy of the `ContosoAutoDb` database.

1. To review the Advanced Data Security assessment for the `ContosoAutoDb` database. Select **Overview** from the left-hand menu.

   ![The Overview menu item is highlighted.](media/sql-mi-overview-menu.png "Overview")

2. On the SQL MI Overview blade, scroll down and locate the list of databases on the Managed Instance, and then select your copy of the **ContosoAutoDb** database, which will be named `ContosoAutoDb-XXXXX` (e.g., ContosoAutoDb-0123), where XXXXX is the unique identifier assigned to you for this workshop.

   ![ContosoAutoDb is highlighted in the list of databases on the SQL MI.](media/sql-mi-database-list.png "Manged Instance databases")

3. On the **ContosoAutoDb-XXXXX** Managed database blade, select **Advanced Data Security** under Security in the left-hand menu and then select the **Vulnerability Assessment** tile.

   ![Advanced Data Security is selected in the left-hand menu, and the Vulnerability tile is highlighted.](media/sql-mi-contosoautodb-ads.png "Advanced Data Security")

   > The [SQL Vulnerability Assessment service](https://docs.microsoft.com/azure/sql-database/sql-vulnerability-assessment) is a service that provides visibility into your security state, and includes actionable steps to resolve security issues, and enhance your database security.

4. On the Vulnerability Assessment blade, you will see a dashboard, displaying the number of failing checks, passing checks, and a breakdown of the risk summary by severity level.

    ![The Vulnerability Assessment dashboard is displayed.](media/sql-mi-vulnerability-assessment-dashboard.png "Vulnerability Assessment dashboard")

5. Take a few minutes to browse both the Failed and Passed checks, and review the types of checks that are performed. In the **Failed** the list, locate the security check for **Transparent data encryption**. This check has an ID of **VA1219**.

    ![The VA1219 finding for Transparent data encryption is highlighted.](media/sql-mi-vulnerability-assessment-failed-va1219.png "Vulnerability assessment")

6. Select the **VA1219** finding to view the detailed description.

    ![The details of the VA1219 - Transparent data encryption should be enabled finding are displayed with the description, impact, and remediation fields highlighted.](media/sql-mi-vulnerability-assessment-failed-va1219-details.png "Vulnerability Assessment")

    > The details for each finding provide more insight into the reason for the finding. Of note are the fields describing the finding, the impact of the recommeneded settings, and details on remediation for the finding.

7. Let's now act on the recommendation remediation steps for the finding, and enable [Transparent Data Encryption](https://docs.microsoft.com/azure/sql-database/transparent-data-encryption-azure-sql) for the `ContosoAutoDb` database. To accomplish this, you will switch back to using SSMS for the next few steps.

    > Transparent data encryption (TDE) needs to be manually enabled for Azure SQL Managed Instance. TDE helps protect Azure SQL Database, Azure SQL Managed Instance, and Azure Data Warehouse against the threat of malicious activity. It performs real-time encryption and decryption of the database, associated backups, and transaction log files at rest without requiring changes to the application.

8. In SSMS, select **New Query** from the toolbar, paste the following SQL script into the new query window. Replace `<XXXXX>` in the `ALTER DATABASE` statement to include the unique identifier of your database which will be `ContosoAutoDb-XXXXX` (e.g., ContosoAutoDb-0123).

    ```sql
    ALTER DATABASE ContosoAutoDb-XXXXX SET ENCRYPTION ON
    ```

    ![A new query window is displayed, with the script above pasted into it.](media/ssms-sql-mi-enable-tde.png "New query")

    > You turn transparent data encryption on and off on the database level. To enable transparent data encryption on a database in Azure SQL Managed Instance use must use T-SQL.

9. Select **Execute** from the SSMS toolbar. After a few seconds, you will see a message that the "Commands completed successfully."

    ![The Excute button is highlighted on the SSMS toolbar, and the Commands completed successfully message is highlighted in the output window.](media/ssms-sql-mi-enable-tde-success.png "Execute")

10. You can verify the encryption state and view information the associated encryption keys by using the [sys.dm_database_encryption_keys view](https://docs.microsoft.com/sql/relational-databases/system-dynamic-management-views/sys-dm-database-encryption-keys-transact-sql). Select **New Query** on the SSMS toolbar again, and paste the following query into the new query window:

    ```sql
    SELECT * FROM sys.dm_database_encryption_keys
    ```

    ![The query above is pasted into a new query window in SSMS.](media/ssms-sql-mi-database-encryption-keys.png "New query")

11. Select **Execute** from the SSMS toolbar. You will see two records in the Results window, which provide information about the encryption state and keys used for encryption.

    ![The Execute button on the SSMS toolbar is highlighted, and in the Results pane the two records about the encryption state and keys for the ContosoAutoDb database are highlighted.](media/ssms-sql-mi-database-encryption-keys-results.png "Results")

    > By default, service-managed transparent data encryption is used. A transparent data encryption certificate is automatically generated for the server that contains the database.

12. Return to the Azure portal and the Vulnerability Assessment blade for your copy of the `ContosoAutoDb` managed database (e.g, ContosoAutoDb-0123). On the toolbar, select **Scan** to start a new assessment of the database.

    ![The Scan button on the SQL MI Vulnerability Assessment dialog is highlighted.](media/sml-mi-vulnerability-assessment-scan.png "Scan")

13. When the scan completes, notice that the numbers for failing and passing checks has changed. The number of failing checks has been reduced by 1 and the number of passing checks has increased by 1.

    ![The total number of failing and passing checks is highlighted.](media/sql-mi-vulnerability-assessment-checks-totals.png "Vulnerability Assessment")

14. On the **Failed** tab, enter **VA1219** into the search filter box, and observe that the previous failure is no longer in the Failed list.

    ![The Failed tab is highlighted and VA1219 is entered into the search filter. The list displays no results.](media/sql-mi-vulnerability-assessment-failed-filter-va1219.png "Failed")

15. Now, select the **Passed** tab, and observe the **VA1219** check is listed with a status of PASS.

    ![The Passed tab is highlighted and VA1219 is entered into the search filter. VA1219 with a status of PASS is highlighted in the results.](media/sql-mi-vulnerability-assessment-passed-va1219.png "Passed")

    > Using the SQL Vulnerability Assessment it is simple to identify and remediate potential database vulnerabilities, allowing you to proactively improve your database security.

## Task 8: SQL Data Discovery and Classification

In this task, you will look at another **Advanced Data Security** feature available within the SQL MI database, [SQL Data Discovery and Classification](https://docs.microsoft.com/sql/relational-databases/security/sql-data-discovery-and-classification?view=sql-server-2017). Data Discovery & Classification introduces a new tool built into SQL Server Management Studio (SSMS) for discovering, classifying, labeling & reporting the sensitive data in your databases. It introduces a set of advanced services, forming a new SQL Information Protection paradigm aimed at protecting the data in your database, not just the database. Discovering and classifying your most sensitive data (business, financial, healthcare, etc.) can play a pivotal role in your organizational information protection stature.

    > This functionality is not currently available for SQL MI through the Azure portal, so you return to SSMS to use this capability.

1. In SSMS, right-click the `ContosoAutoDb-XXXXX` database (e.g., ContosoAutoDb-0123) in the Object Explorer (where XXXXX is the unique identifier assigned to you for this workshop), and then select **Tasks** and **Classify Data** in the context menus.

   ![The Tasks > Classify Data context menu items are highlighted for the ContosoAutoDb database in SSMS.](media/ssms-sql-mi-classify-data-menu.png "Classify Data")

2. In the Data Classification - ContosoAutoDb window, select the info link with the message _39 columns with classification recommendations (click to view)_.

   ![The link to classification recommendations is displayed.](media/ssms-sql-mi-classify-data-recommendations-link.png "Recommendations")

3. In the list of classification recommendations, select the recommendation for the **NationalIDNumber** field, and then expand the **Sensitivity Label** drop down list. You can see the list of built-is sensitivity classification, including those related to compliance requirements around GDPR.

   ![The NationalIDNumber field is highlighted within the recommenations list, and the Sensitivity Label drop down is expanded and highlighted.](media/ssms-sql-mi-classify-data-recommendations-labels.png "Recommendations")

4. Select the check box at the top of the list to select all of the recommended classifications, and then select **Accept selected recommendations**.

   ![All the recommended classifications are checked and the Accept selected recommendations button is highlighted.](media/ssms-sql-mi-classify-data-accept-recommendations.png "Recommendations")

5. Select **Save** on the toolbar of the Data Classification window.

   ![Save the updates to the classified columns list.](media/ssms-sql-mi-classify-data-save.png "Save")

6. Select **View Report** on the Data Classification window to generate a report with a full summary of the database classification state.

   ![The View Report button is highlighted on the toolbar.](media/ssms-sql-mi-classify-data-view-report.png "View report")

7. View the report.

   ![The SQL Data Classification Report is displayed.](media/ssms-sql-mi-classify-data-report.png "SQL Data Classification Report")

## Wrap-up

In this experience you unlocked new capabilities for a SQL Server 2008 R2 database by performing a friction-free migration to Azure SQL Database Managed Instance. You learned how Azure SQL Database Managed Instance enables you to migrate on-premises databases quickly and easily into a fully-managed PaaS database running in Azure, with no application code changes. SQL MI provides a migration path for databases using features, such as Service broker, which previously prevented them from running in Azure SQL Database.

After you migrated the database into SQL MI, you explored some of advanced SQL features available only in Azure, including Advanced Data Security Vulnerability Assessments and Data Classification and Discovery. In addition, you enabled Dynamic Data Masking and created a ColumnStore index on a table in the database, demonstrating how SQL MI allows you to utilize features unavailable in SQL Server 2008 R2. You also examined how to connect to an online secondary replica of your database, which provides a free read-only copy of your database. This feature takes advantage of one the high-availability features of the Azure SQL MI Business Critical service tier.

This experience was meant to provided a brief introduction to Azure SQL Database Managed Instance. There are many more features of SQL MI that you can now explore, including [Advanced Threat Detection](https://docs.microsoft.com/en-us/azure/sql-database/sql-database-threat-detection-overview) and [Transactional replication](https://docs.microsoft.com/azure/sql-database/replication-with-sql-database-managed-instance). Threat detection for Azure SQL Database Managed Instance detects anomalous activities indicating unusual and potentially harmful attempts to access or exploit databases. Transactional replication allows you to replicate data into an Azure SQL MI database from a remote SQL Server database or another instance database. You an also use it to push changes made in an instance database in SQL MI to a remote SQL Server database, to a single database in Azure SQL Database, or to a pooled database in an Azure SQL Database elastic pool.

## Additional resources and more information

Use the links below as a starting point to continue learning about the capabilities and features available with Azure SQL Database Managed Instance.

- [Azure SQL Database](https://azure.microsoft.com/services/sql-database/)
  - [Service tiers](https://docs.microsoft.com/azure/sql-database/sql-database-service-tiers-general-purpose-business-critical)
- [What is Azure SQL Database Managed Instance?](https://docs.microsoft.com/azure/sql-database/sql-database-managed-instance-index)
- [Database Migration Guide](https://datamigration.microsoft.com/)
  - [Database Migration Assistant](https://docs.microsoft.com/sql/dma/dma-overview?view=azuresqldb-mi-current)
  - [Azure Database Migration Service](https://docs.microsoft.com/azure/dms/dms-overview)
- [Migrate SQL Server to an Azure SQL Database Managed Instance](https://datamigration.microsoft.com/scenario/sql-to-azuresqldbmi)
- [SQL Database Platform as a Service](https://docs.microsoft.com/azure/sql-database/sql-database-paas)
  - [Business continuity](https://docs.microsoft.com/en-us/azure/sql-database/sql-database-business-continuity)
  - [High availability](https://docs.microsoft.com/en-us/azure/sql-database/sql-database-high-availability)
  - [Automated backups](https://docs.microsoft.com/en-us/azure/sql-database/sql-database-automated-backups)
  - [Long-term back retention](https://docs.microsoft.com/en-us/azure/sql-database/sql-database-long-term-retention)
  - [Geo-replication](https://docs.microsoft.com/en-us/azure/sql-database/sql-database-auto-failover-group)
  - [Scale resources](https://docs.microsoft.com/en-us/azure/sql-database/sql-database-scale-resources)
- [How to use Azure SQL Database](https://docs.microsoft.com/en-us/azure/sql-database/sql-database-howto)
- [Azure updates for Azure SQL Database](https://azure.microsoft.com/en-us/updates/?product=sql-database)
- [Azure SQL Database pricing](https://azure.microsoft.com/en-us/pricing/details/sql-database/managed/)
- [Overview of Azure SQL Database security capabilities](https://docs.microsoft.com/azure/sql-database/sql-database-security-overview)
  - [Advanced data security](https://docs.microsoft.com/azure/sql-database/sql-database-advanced-data-security)
  - [Data discovery and classification](https://docs.microsoft.com/azure/sql-database/sql-database-data-discovery-and-classification)
  - [SQL Vulnerability Assessment service](https://docs.microsoft.com/azure/sql-database/sql-vulnerability-assessment)
  - [Threat detection](https://docs.microsoft.com/en-us/azure/sql-database/sql-database-threat-detection-overview)
- [SQL Database Read Scale-Out](https://docs.microsoft.com/azure/sql-database/sql-database-read-scale-out)
- [Connect an application to Azure SQL Database Managed Instance](https://docs.microsoft.com/azure/sql-database/sql-database-managed-instance-connect-app)
