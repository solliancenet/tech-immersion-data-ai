USE master;  
GO
-- Enable external scripts execution for R/Python/Java:
exec sp_configure 'external scripts enabled', 1;
RECONFIGURE WITH OVERRIDE;
GO

IF DB_ID('ContosoAutoDW') IS NULL
	
RESTORE DATABASE [ContosoAutoDW]
FROM  DISK = N'/var/opt/mssql/data/ContosoAutoDW.bak' WITH REPLACE, 
  MOVE N'ContosoAuto_Primary' TO N'/var/opt/mssql/data/ContosoAutoDW.mdf', 
  MOVE N'ContosoAuto_UserData' TO N'/var/opt/mssql/data/ContosoAutoDW_UserData.ndf', 
  MOVE N'ContosoAuto_Log' TO N'/var/opt/mssql/data/ContosoAutoDW.ldf', 
  MOVE N'ContosoAuto_InMemory_Data_1' TO N'/var/opt/mssql/data/ContosoAutoDW_InMemory_Data_1', 
NOUNLOAD,  STATS = 5

GO

USE ContosoAutoDW;
GO

IF NOT EXISTS(SELECT *
FROM sys.external_data_sources
WHERE name = 'SqlDataPool')
CREATE EXTERNAL DATA SOURCE SqlDataPool
WITH (LOCATION = 'sqldatapool://controller-svc:8080/datapools/default');
IF NOT EXISTS(SELECT *
FROM sys.external_data_sources
WHERE name = 'SqlStoragePool')
CREATE EXTERNAL DATA SOURCE SqlStoragePool
WITH (LOCATION = 'sqlhdfs://controller-svc:8080/default');
GO

