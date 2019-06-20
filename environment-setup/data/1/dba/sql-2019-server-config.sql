USE master;
GO

-- SET the sa password
ALTER LOGIN [sa] WITH PASSWORD=N'Password.1!!';
GO

-- Enable Mixed Mode AUthentication
EXEC xp_instance_regwrite N'HKEY_LOCAL_MACHINE',
N'Software\Microsoft\MSSQLServer\MSSQLServer', N'LoginMode', REG_DWORD, 2;
GO

-- Create a login and user named demouser with the password Password.1!!
CREATE LOGIN demouser WITH PASSWORD = 'Password.1!!';
GO

EXEC sp_addsrvrolemember 
    @loginame = N'demouser', 
    @rolename = N'sysadmin';
GO