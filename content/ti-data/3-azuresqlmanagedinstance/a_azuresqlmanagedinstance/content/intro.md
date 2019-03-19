# Day 1, Experience 3 - Unlocking new capabilities with friction-free migrations to Azure SQL Database Managed Instance

## Technology overview

### Migrate your SQL Server databases without changing your apps

[Azure SQL Database Managed Instance](https://docs.microsoft.com/azure/sql-database/sql-database-managed-instance) (SQL MI) is a new deployment option of Azure SQL Database which enables the migration of existing on-premises SQL Server databases to the cloud with minimal or no application and database changes. With SQL MI, you get the broadest SQL Server engine compatibility and native virtual network (VNET) support. This option gives you the best of SQL Server, plus the operational and cost benefits of an intelligent, fully managed service. SQL MI is ideal for migrating a large number of existing SQL Server databases from on-premises or virtual machines to SQL Database.

![Diagram outlining the key features of managed instances.](imgs/azure-sql-database-managed-instance.png 'What is SQL MI?')

### Accelerate your database migration

Reduce the complexity of your cloud migration by using a single comprehensive service instead of multiple tools. [Azure Database Migration Service](https://docs.microsoft.com/azure/dms/dms-overview) is designed as a seamless, end-to-end solution for moving on-premises SQL Server databases to the cloud. Use the [Database Migration Guide](https://datamigration.microsoft.com/) for recommendations, step-by-step guidance, and expert tips on your specific database migration.

### Maximize ROI by migrating to the cloud

Reduce the burden of data-tier management and save time and costs by migrating workloads to the cloud. [Azure Hybrid Benefit](https://azure.microsoft.com/pricing/hybrid-benefit/) for SQL Server provides a cost-effective path for migrating hundreds or thousands of SQL Server databases with minimal effort. Use your SQL Server licenses with Software Assurance to pay a reduced rate when migrating to the cloud. Save up to 55 percent with Azure Hybrid Benefit, and up to 80 percent with [reserved capacity](https://docs.microsoft.com/azure/sql-database/sql-database-reserved-capacity). Learn how [customers have increased productivity](https://azure.microsoft.com/resources/forrester-tei-sql-database-managed-instance/) by up to 40 percent by migrating to Azure SQL Database.

## Scenario overview

ContosoAuto runs their operations and finance database, `ContosoAutoDb`, on an on-premises SQL Server 2008 R2 database. This system is vital to the company's daily activities and as SQL Server 2008 R2 is approaching end of support, they are looking at options for migrating this database into Azure. They have read about some of the advanced security and performance tuning options that are available only in Azure and would prefer to a migrate the database into a platform-as-a-service (PaaS) offering, if possible.

ContosoAuto is using the Service Broker feature of SQL Server within the `ContosoAutoDb` database. Service Broker is a feature of SQL Server used for sending and receiving guaranteed, asynchronous messages by using extensions to the Transact-SQL Data Manipulation Language (DML). This functionality is being used for several critical business processes, and they cannot afford to lose this capability when migrating their operations database to the cloud. They have also stated that, at this time, they do not have the resources to rearchitect the solution to use an alternative message broker.

In this experience, you will use the Microsoft Data Migration Assistant (DMA) to perform assessments of feature parity and compatibility against both Azure SQL Database and Azure SQL Database Managed Instance, with the goal of migrating the `ContosoAutoDb` database into an Azure PaaS offering with minimal or no changes. After completing the assessments, you will perform the database migration and then update ContosoAuto's operations web application to use the new database. Once that is complete, you will review and enable some of the database features that are only available in Azure.
