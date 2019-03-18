# Azure SQL Managed Instance setup

Complete the steps below to prepare the environment for the [Day 1, Experience 3](../../../day1-exp3/) lab.

## Pre-requisites

The following services must be provisioned prior to the lab:

1. **Azure SQL Database Managed Instance**
   - Provision a single SQL Managed Instance to be shared by all lab attendees.
   - Business Critical service tier
     - Gen5
     - 16 vCores
     - 64 GB storage
   - Admin credentials:
     - Login: tiuser
     - Password: Password.1234567890
2. **SQL Server 2008 R2 on Windows Server 2008 VM**
   - Provision a single SQL Server 2008 R2 on Windows 2008 VM. This will be accessed via IP address.
   - Attendees will need the IP address of this server, and read-only access to the `ContosoAutoDb` database for performing assessments using Data Migration Assistant.
3. **Azure Blob Storage account**
   - A shared Blob Storage account should be created for storing a backup of the `ContosoAutoDb` database (ContosoAutoDb.bak).
4. **Lab VM or jumpbox**
   - Per attendee, provision a VM created in the same VNet as the SQL MI.
5. **App Service Environment (ASE)**
   - Provision a single ASE to be shared by all lab attendees.
   - Must be added to the same VNet as the SQL MI.
6. **App Service (Web App)**
   - Provision one App Service (Web app) per attendee, each deployed into the ASE.
   - Name: tech-immersion-web-XXXX (where XXXX is the attendee's unique ID).

### SQL MI pre-requisites

The following configuration must be applied to the SQL MI prior to the workshop.

#### Create ContosoAutoDb database

- A `ContosoAutoDb` database should be created, from the **ContosoAutoDb.bak** file (found under lab-files/data/3), as a shared read-only database for attendees.
- All attendees must have the ability to restore a copy of the `ContosoAutoDb` database to the SQL MI, and name it `ContosoAutoDb-<unique-user-id>`.

#### Enable Advanced Data Security

1. On the SQL MI blade, select **Advanced Data Security** from the left-hand menu, under Security, and then turn on Advanced Data Security by selecting **ON**.
2. On the Advanced Data Security screen enter the following:
   - **Subscription**: Select the subscription you are using for this workshop.
   - **Storage account**: Select this and then select **+ Create new**. On the Create storage account blade, enter a globally unique name (e.g., **techimmersionsqlmi**), and select **OK**, accepting the default values for everything else.
   - **Periodic recurring scans**: Select **ON**.
   - **Send scan reports to**: Enter your email address.
   - **Send alerts to**: Enter you email address.
   - **Advanced Threat Protection types**: Select this and ensure all options are checked.
3. Your **Advanced Data Security** form should look similar to the following:
4. Select **Save** to enable **Advanced Data Security**.

### SQL Server 2008 R2 pre-requisites

After provisioning the SQL Server 2008 R2 on Windows 2008 VM, the SQL Server 2008 R2 instance requires the following configuration:

- Open port 1433 using an Inbound port rule added to the VM firewall in the Azure portal.
- Open port 1433 on the VM's Windows firewall using an inbound port rule on the VM.
- Add the **ContosoAutoDb** database to each attendees VM by restoring from the provided `ContosoAutoDb.bak` file.
- Reset the `sa` password, enable mixed mode authentication, enable Service broker, and create the `WorkshopUser` account by running the `configure-sql-2008.sql` script found under lab-files/data/3.

### Blob Storage account pre-requisites

The following step should be taken for the Blob Storage account:

1. Create a container in the storage account named `database-backup`.
2. Upload the provided `ContosoAutoDb.bak` file to the `database-backup` container.
3. Create a SAS token providing read access to the Storage account.
4. Update `Day 1 Experience 3, Task 2, Step 6` with the proper storage account uri, ending with `/database-backup` and SAS key. The SAS key should begin with `sv=`, so the leading "?" should be removed from the generated key.
5. Update `Day 1 Experience 3, Task 2` Steps `8` and `9` with the proper storage account URI, ending with `/database-backup/ContosoAutoDb.bak`.

### App Service pre-requisites

The following configuration must be applied to each App Service prior to the workshop.

1. Configure VNet integration with SQL MI VNet.
2. Deploy the **ContosoAutoOpsWeb** application, found under `lab-files/data/3` folder.
   - Open the `ContosoAutoOpsWeb.sln` file, and deploy the application to each App Service.
3. Add two connection string values to each web app. These should be under the Connection Strings section on the Application Settings page of each App Service.
   - **ContosoAutoDbContext**: Will initially contain the connection string to the SQL Server 2008 R2 database (e.g., `Server=tcp:40.70.209.101,1433;Database=ContosoAutoDb;User ID=WorkshopUser;Password=Password.1!!;Trusted_Connection=False;Encrypt=True;`)
   - **ContosoAutoDbReadOnlyContext**: Will initially contain the connection string to the SQL Server 2008 R2 database (e.g., `Server=tcp:40.70.209.101,1433;Database=ContosoAutoDb;User ID=WorkshopUser;Password=Password.1!!;Trusted_Connection=False;Encrypt=True;`)

### Lab computer pre-requisites

The computer or VM on which you restore the database requires the following:

- [SQL Server Management Studio](https://go.microsoft.com/fwlink/?linkid=2043154) (SSMS) v 17.9.1 or greater.
- [Microsoft Data Migration Assistant](https://www.microsoft.com/download/details.aspx?id=53595)