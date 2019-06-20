-- This script needs to be run for each user database

-- REPLACE XXXXX with the unique identifier for each database
USE salesXXXXX;
GO

-- Assign the demouser account permissions to the database
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'demouser')
BEGIN
    CREATE USER [demouser] FOR LOGIN [demouser]
    EXEC sp_addrolemember N'db_datareader', N'demouser'
END;
GO