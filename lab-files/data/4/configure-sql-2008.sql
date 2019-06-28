USE master;
GO

-- SET the sa password
ALTER LOGIN [sa] WITH PASSWORD=N'Password.1!!';
GO

-- Enable Service Broker on the database
ALTER DATABASE ContosoAutoDb SET ENABLE_BROKER WITH ROLLBACK immediate;
GO

-- Enable Mixed Mode AUthentication
EXEC xp_instance_regwrite N'HKEY_LOCAL_MACHINE',
N'Software\Microsoft\MSSQLServer\MSSQLServer', N'LoginMode', REG_DWORD, 2;
GO

-- Create a login and user named WorkshopUser with the password Password.1!!
CREATE LOGIN WorkshopUser WITH PASSWORD = 'Password.1!!';
GO

EXEC sp_addsrvrolemember 
    @loginame = N'WorkshopUser', 
    @rolename = N'sysadmin';
GO

USE ContosoAutoDb;
GO

IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'WorkshopUser')
BEGIN
    CREATE USER [WorkshopUser] FOR LOGIN [WorkshopUser]
    EXEC sp_addrolemember N'db_datareader', N'WorkshopUser'
END;
GO