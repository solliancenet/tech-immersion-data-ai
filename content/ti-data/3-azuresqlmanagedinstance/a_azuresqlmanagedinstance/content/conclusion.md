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
