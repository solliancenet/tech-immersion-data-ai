# Environment setup: Data, Experience 3 - Unlocking new capabilities with friction-free migrations to Azure SQL Database Managed Instance

Complete the steps below to prepare the environment for the [Data, Experience 3](../../../data-exp3/README.md) lab.

- [Environment setup: Data, Experience 3 - Unlocking new capabilities with friction-free migrations to Azure SQL Database Managed Instance](#environment-setup-data-experience-3---unlocking-new-capabilities-with-friction-free-migrations-to-azure-sql-database-managed-instance)
  - [Resources](#resources)
    - [Shared resources](#shared-resources)
      - [Create a virtual network](#create-a-virtual-network)
      - [Create VPN gateway](#create-vpn-gateway)
      - [Provision SQL MI](#provision-sql-mi)
      - [Create a service principal](#create-a-service-principal)
      - [Configure point-to-site addresses](#configure-point-to-site-addresses)
    - [Per attendee resources](#per-attendee-resources)
      - [Azure Storage Account](#azure-storage-account)
      - [Create a SQL Server 2008 R2 virtual machine](#create-a-sql-server-2008-r2-virtual-machine)
      - [Create the JumpBox VM](#create-the-jumpbox-vm)
      - [Create Azure Database Migration Service](#create-azure-database-migration-service)
      - [Provision a Web App](#provision-a-web-app)
  - [Resource configuration](#resource-configuration)
    - [SQL Server 2008 R2 VM configuration](#sql-server-2008-r2-vm-configuration)
    - [JumpBox VM configuration](#jumpbox-vm-configuration)
    - [SQL MI configuration](#sql-mi-configuration)
      - [Restore the ContosoAutoDb database](#restore-the-contosoautodb-database)
      - [Enable Advanced Data Security](#enable-advanced-data-security)
    - [Web App and App Service configuration](#web-app-and-app-service-configuration)
      - [Configure VNet integration with App Services](#configure-vnet-integration-with-app-services)
      - [Verify Virtual Network Gateway configuration](#verify-virtual-network-gateway-configuration)
  - [Environment Details](#environment-details)

## Resources

> **IMPORTANT**: Due to the higher security requirements around Azure SQL Database Managed Instance, all resources, both shared and per-attendee, should be created in the same region for this experience.

### Shared resources

The following steps detail setting up the shared resources for this workshop. These are provisioned as shared resources for all lab participants.

> **IMPORTANT**: The shared resources should be provisioned before setting up the per-attendee resources.

#### Create a virtual network

Create and configure a shared virtual network (VNet). This will contain subnets for the SQL managed instance, JumpBox and SqlServer2008R2 VMs, and the Azure Database Migration Service.

1. In the [Azure portal](https://portal.azure.com), select **+Create a resource**, enter "virtual network" into the Search the Marketplace box, and then select **Virtual Network** from the results.
2. Select **Create** on the Virtual Network blade.
3. On the Create virtual network blade, enter the following:
    - **Name**: Enter **tech-immersion-vnet**.
    - **Address space**: Accept the default value here. This should be /16 block, in the format 10.X.0.0/16.
    - **Subscription**: Select the workshop subscription.
    - **Resource group**: Select the tech immersion shared resource group.
    - **Location**: Select a region. **NOTE**: All resources that need to be in the VNet will need to be in the same region.
    - **Subnet Name**: Enter **ManagedInstance**.
    - **Subnet Address range**: Accept the default value. This should have a subnet mask of /24, and be within the address space indicated above, in the format 10.X.0.0/24.
    - **DDOS protection**: Choose **Basic**.
    - **Service endpoints**: Select **Disabled**.
    - **Firewall**: Select **Disabled**.
4. Select **Create**. It will take a few seconds for the virtual network to provision.
5. Navigate to the Virtual network blade, select **Subnets** under Settings in the left-hand menu, and then select **+ Subnet** from the top menu.
6. On the Add subnet blade, enter the following:
    - **Name**: Enter **Management**.
    - **Address range**: Accept the default value, which should be a subnet mask of /24, within the address range of your VNet.
    - Select **OK**.
7. Back on the **Subnets** blade, select **+ Gateway Subnet**.
8. The **Name** for gateway subnet is automatically filled in with the value `GatewaySubnet`. This value is required in order for Azure to recognize the subnet as the gateway subnet. Create a gateway subnet using a CIDR block of /24 to provide enough IP addresses to accommodate large lab groups. You should be able to accept the auto-filled Address range value. Don't configure Route table or Service endpoints.
9. Select **OK**.

#### Create VPN gateway

1. In the [Azure portal](https://portal.azure.com), select **+ Create a resource**, enter "virtual network gateway" into the Search the Marketplace box, and select **Virtual network gateway** from the results.
2. Select **Create** on the Virtual network gateway blade.
3. On the Create virtual network gateway **Basics** tab, enter the following:
    - Project Details:
        - **Subscription**: Select the workshop subscription.
    - Instance Details:
        - **Name**: Enter tech-immersion-vnet-gateway.
        - **Region**: Select the same region you used for the VNet.
        - **Gateway type**: Choose **VPN**.
        - **VPN type**: Choose **Route-based**.
        - **SKU**: Select **VpnGw1**.
    - Virtual Network:
        - **Virtual network**: Select the **tech-immersion-vnet** created above.
    - Public IP Address:
        - **Public IP address**: Choose **Create new**.
        - **Public IP address name**: Enter **vnet-gateway-ip**.
        - **Enable active-active mode**: Choose **Disabled**.
        - **Configure BGP ASN**: Choose **Disabled**.
4. Select **Review + create**.
5. On the **Review + create** tab, ensure the *Validation passed* message is displayed and then select **Create**.

> **NOTE**: It can take up to 45 minutes for the Virtual network gateway to provision. You can continue with the steps below while this is deploying.

#### Provision SQL MI

Provision a shared Azure SQL Managed Instance.

1. In the [Azure portal](https://portal.azure.com), select **+Create a resource**, enter "sql managed instance" into the Search the Marketplace box, and then select **Azure SQL Managed Instance** from the results.
2. Select **Create** on the Azure SQL Managed Instance blade.
3. On the SQL managed instance Basics tab, enter the following:
    - **Subscription**: Select the workshop subscription.
    - **Resource group**: Select the tech immersion shared resource group.
    - **Managed instance name**: Enter **tech-immersion-sql-mi**
    - **Region**: Select the same region as used for the VNet above.
    - **Compute + storage**: Select this, and on the Configure performance blade, select **Business Critical**, **Gen5**, and set the vCores to **8** and the Storage to **32**, and then select **Apply**.
    - **Managed instance admin login**: Enter **tiuser**
    - **Password**: Enter **Password.1234567890**
4. Select **Next: Networking**, and on the **Networking** tab enter the following:
    - **Virtual network**: Select the **tech-immersion-vnet/ManagedInstance** VNet and subnet you created above from the dropdown list. **IMPORTANT**: The default value is to create a new VNet, so make sure you select the VNet you created above, and don't take the default value.
    - **Prepare subnet for Managed Instance**: Set to **Automatic**.
    - **Connection type**: Leave Proxy selected.
    - **Public endpoint**: Leave set to Disable.
5. Select **Next: Additional settings**, and on the **Additional settings** tab enter the following:
    - **Collation**: Accept the default value, **SQL_Latin1_General_CP1_CI_AS**.
    - **Time zone**: Select **(UTC) Coordinated Universal Time**.
    - **Use as failover secondary**: Select **No**.
6. Select **Next: Review + create**, and then select **Create**.

> **NOTE**: Provisioning of SQL Managed Instance can take 6+ hours, if this is the first instance being deployed into a subnet. You can move on to the remaining tasks while the provisioning is in process.

#### Create a service principal

Use the Azure Cloud Shell to create an Azure Active Directory (Azure AD) application and service principal (SP) that will provide the Azure Database Migration Service access to Azure SQL MI. You will grant the SP "contributor" permissions to the workshop subscription.

1. In the [Azure portal](https://portal.azure.com), open an Azure Cloud Shell from the top menu.
2. In the Cloud Shell window that opens at the bottom of your browser window, select **PowerShell**.
3. If prompted that you have no storage mounted, select the subscription you are using for this hands-on lab and select **Create storage**.
4. At the Cloud Shell prompt, issue a command to create a service principal named **tech-immersion-sp** and assign it `owner` permissions to the **shared** tech-immersion resource group.
5. First, you need to retrieve your subscription ID. Enter the following at the Cloud Shell prompt:

    ```powershell
    az account list --output table
    ```

6. In the output table, locate the workshop subscription and copy the SubscriptionId value into a text editor, such as Notepad, for use below.
7. Next, enter the following `az ad sp create-for-rbac` command at the Cloud Shell prompt, replacing `{SubscriptionID}` with the value you copied above, and then press `Enter` to run the command.

    ```powershell
    az ad sp create-for-rbac -n "tech-immersion-sp" --role contributor --scopes subscriptions/{SubscriptionID}
    ```

8. **IMPORTANT**: Copy the `appId` and `password` values from the output of the command into the Environment Details sheet, as lab attendees will need the `appId` and `password` to configure the online migration. The output should be similar to:

    ```json
    {
        "appId": "aeab3b83-9080-426c-94a3-4828db8532e9",
        "displayName": "tech-immersion-sp",
        "name": "http://tech-immersion-sp",
        "password": "76ff5bae-8d25-469a-a74b-4a33ad868585",
        "tenant": "d280491c-b27a-XXXX-XXXX-XXXXXXXXXXXX"
    }
    ```

9. Copy the `appId` value above into the Environment Details sheet with a name of `Application ID`.

10. Copy the `password` value above into the Environment Details sheet with a name of `Application Key`.

#### Configure point-to-site addresses

Configure the client address pool on the Virtual Network Gateway. This is a range of private IP addresses that you will specify below. Clients that connect over a Point-to-Site VPN dynamically receive an IP address from this range. You will use a private IP address range that does not overlap with the VNet you will connect to.

1. Navigate to the **tech-immersion-vnet-gateway** Virtual network gateway (created above) in the [Azure portal](https://portal.azure.com).
2. On the virtual network gateway blade, select **Point-to-site configuration** under Settings in the left-hand menu, and then select **Configure now**.
3. On the **Point-to-site** configuration page, set the following configuration:
    - **Address pool**: Add a private IP address range that you want to use. The address space must be in one of the following address blocks, but should not overlap the address space used by the VNet.
      - `10.0.0.0/8` - This means an IP address range from 10.0.0.0 to 10.255.255.255
      - `172.16.0.0/12` - This means an IP address range from 172.16.0.0 to 172.31.255.255
      - `192.168.0.0/16` - This means an IP address range from 192.168.0.0 to 192.168.255.255
    - **Tunnel type**: Select **SSTP (SSL)**.
    - **Authentication type**: Choose **Azure certificate**.
4. Select **Save** to validate and save the settings. It will take 1 - 2 minutes for the save to finish.

### Per attendee resources

The following steps detail setting up the per-attendee resources for this workshop. These are provisioned in each attendees **tech-immersion-XXXXX** resource group, where XXXXX is the attendees unique identifier.

#### Azure Storage Account

1. In the [Azure portal](https://portal.azure.com/), select **+Create a resource**, and enter "storage account" into the Search the Marketplace box.
2. On the **Storage account** blade, select **Create**.
3. On the Create storage account **Basics** tab, set the following configuration:
    - Project Details:
        - **Subscription**: Select the workshop subscription.
        - **Resource Group**: Select the tech-immersion-XXXXX resource group, where XXXXX is the unique Id for each lab attendee.
    - Instance Details:
        - **Storage account name**: Enter **techimmersionstoreXXXXX**, where XXXXX is the unique Id for each lab attendee.
        - **Location**: Select the same region you used for the virtual network and SQL MI.
        - **Performance**: Select Standard.
        - **Account kind**: Select **Storage V2 (general purpose v2)**.
        - **Replication**: Select **Locally-redundant storage (LRS)**.
        - **Access tier**: Select **Hot**.
4. Select **Review + create** and then select **Create**.

#### Create a SQL Server 2008 R2 virtual machine

In this task, you will a virtual machine (VM) in Azure which will simulate an "on-premises" instance of SQL Server 2008 R2. The VM will use the SQL Server 2008 R2 SP3 Standard on Windows Server 2008 R2 image.

> **NOTE**: An older version of Windows Server is being used because SQL Server 2008 R2 is not supported on Windows Server 2016.

1. In the [Azure portal](https://portal.azure.com/), select **+Create a resource**, and enter "SQL Server 2008R2SP3 on Windows Server 2008R2" into the Search the Marketplace box.
2. On the **SQL Server 2008 R2 SP3 on Windows Server 2008 R2** blade, select **SQL Server R2 SP3 Standard on Windows Server 2008 R2** for the software plan and then select **Create**.
3. On the Create a virtual machine **Basics** tab, set the following configuration:
    - Project Details:
        - **Subscription**: Select the workshop subscription.
        - **Resource Group**: Select the tech-immersion-XXXXX resource group, where XXXXX is the unique Id for each lab attendee.
    - Instance Details:
        - **Virtual machine name**: Enter SqlServer2008R2. (The VM name can be the same for all attendees, so it does not require their unique ID to be appended.)
        - **Region**: Select the same region you used for the virtual network and SQL MI.
        - **Availability options**: Select no infrastructure redundancy required.
        - **Image**: Leave SQL Server 2008 R2 SP3 Standard on Windows Server 2008 R2 selected.
        - **Size**: Select **Change size**, and select Standard D2s v3 from the list and then select **Accept**.
    - Administrator Account:
        - **Username**: Enter **tiuser**
        - **Password**: Enter **Password.1234567890**
    - Inbound Port Rules:
        - **Public inbound ports**: Choose Allow selected ports.
        - **Select inbound ports**: Select RDP (3389) in the list.
4. Select **Next: Disks** to move to the next step, and on the **Disks** tab, set OS disk type to **Premium SSD**.
5. Select **Next: Networking** and on the **Networking** tab, set the following configuration:
    - **Virtual network**: Select the **tech-immersion-vnet** you provisioned above in the **tech-immersion-shared** resource group.
    - **Subnet**: Select the **Management** subnet.
    - **Public IP**: Leave **(new) SqlServer2008R2-ip** selected.
    - **NIC network security group**: Select **Basic**.
    - **Public inbound ports**: Leave **Allow selected ports** selected.
    - **Select inbound ports**: Leave **RDP** selected.
6. Select **Next: Management** and on the **Management** blade turn Boot diagnostics **Off**.
7. Select **Next: Advanced** and on the **Advanced** tab do the following:
   - Under Extensions, select **Select an extension to install**.
   - In the New resource blade that appears, select **Custom Script Extension**, and then select **Create** in the Custom Script Extension blade.
   - Select the **Browse** icon next to the Script file box, and navigate to the `configure-sqlvm.ps1` file located within the `environment-setup/data/3` folder of this lab repository.
   - Select **OK**.
8. Select **Next: SQL Server settings** and on the **SQL Server settings** tab, enter the following:
   - **SQL Connectivity**: Leave set to Private (within Virtual Network).
   - **Port**: Leave set to 1433.
   - **SQL Authentication**: Set to **Enable**, and ensure the login name is set to **tiuser** and the password is **Password.1234567890**.
9. Select **Review + create** and then select **Create** to provision the virtual machine.

> **NOTE**: It will take approximately 10 minutes for the SQL VM to finish provisioning. You can move on to the next task while you wait.

#### Create the JumpBox VM

In this task, you will provision a virtual machine (VM) in Azure. The VM image used will have Visual Studio Community 2019 installed.

1. In the [Azure portal](https://portal.azure.com/), select **+Create a resource**, enter "visual studio 201 latest" into the Search the Marketplace box, expand the **Visual Studio 2019 Latest** group, and then select **Visual Studio Community 2019 (latest release) on Windows Server 2016 (x64)** from the results.
2. Select **Create** on the Visual Studio blade.
3. On the Create a virtual machine **Basics** tab, set the following configuration:
    - Project Details:
        - **Subscription**: Select the workshop subscription.
        - **Resource Group**: Select the tech-immersion-XXXXX resource group, where XXXXX is the unique Id for each lab attendee.
    - Instance Details:
        - **Virtual machine name**: Enter JumpBox. (The VM name can be the same for all attendees, so it does not require their unique ID to be appended.)
        - **Region**: Select the same region you used for the virtual network and SQL MI.
        - **Availability options**: Select no infrastructure redundancy required.
        - **Image**: Leave Visual Studio Community 2019 (latest release) on Windows Server 2016 (x64) selected.
        - **Size**: Select **Change size**, and select Standard D2s v3 from the list and then select **Accept**.
    - Administrator Account:
        - **Username**: Enter **tiuser**
        - **Password**: Enter **Password.1234567890**
    - Inbound Port Rules:
        - **Public inbound ports**: Choose Allow selected ports.
        - **Select inbound ports**: Select RDP (3389) in the list.
4. Select **Next: Disks** and on the **Disks** tab, set OS disk type to **Premium SSD**, and then select **Next: Networking**.
5. On the **Networking** tab, set the following configuration:
    - **Virtual network**: Select the **tech-immersion-vnet** you provisioned above in the **tech-immersion-shared** resource group.
    - **Subnet**: Select the **Management** subnet.
    - **Public IP**: Leave **(new) JumpBox-ip** selected.
    - **NIC network security group**: Select **Basic**.
    - **Public inbound ports**: Leave **Allow selected ports** selected.
    - **Select inbound ports**: Leave **RDP** selected.
6. Select **Next: Management** and on the **Management** blade turn Boot diagnostics **Off**.
7. Select **Next: Advanced** and on the **Advanced** tab do the following:
   - Under Extensions, select **Select an extension to install**.
   - In the New resource blade that appears, select **Custom Script Extension**, and then select **Create** in the Custom Script Extension blade.
   - Select the **Browse** icon next to the Script file box, and navigate to the `configure-jumpbox.ps1` file located within the `environment-setup/data/3` folder of this lab repository.
   - Select **OK**.
8. Select **Review + create** and then select **Create** to provision the virtual machine.

> **NOTE**: It will take approximately 15 minutes for the VM to finish provisioning. You can move on to the next task while you wait.

#### Create Azure Database Migration Service

In this task, you will provision an instance of the Azure Database Migration Service (DMS).

> **IMPORTANT**: You must register the `Microsoft.DataMigration` the resource provider for the workshop subscription prior to provisioning DMS in the attendee resource groups.

1. In the [Azure portal](https://portal.azure.com/), select **+Create a resource**, enter "database migration" into the Search the Marketplace box, select **Azure Database Migration Service** from the results, and select **Create**.
2. On the Create Migration Service blade, enter the following:
    - **Service Name**: Enter tech-immersion-dms. (The DMS service name can be the same for all attendees, so it does not require their unique ID to be appended.)
    - **Subscription**: Select the workshop subscription.
    - **Resource Group**: Select the tech-immersion-XXXXX resource group, where XXXXX is the unique Id for each lab attendee.
    - **Location**: Select the same region you used for the virtual network and SQL MI.
    - **Virtual network**: Select the **tech-immersion-vnet/Management** virtual network, and then select **OK**. This will place the DMS instance into the same VNet as your SQL MI and Lab VMs.
    - **Pricing tier**: Select Premium: 4 vCores.
3. Select **Create**.

> **NOTE**: It can take 15 minutes to deploy the Azure Data Migration Service. You can move on to the next task while you wait.

#### Provision a Web App

In this task, you will provision an App Service (Web app), which will be used for hosting the ContosoAuto web application.

1. In the [Azure portal](https://portal.azure.com/), select **+Create a resource**, enter "web app" into the Search the Marketplace box, select **Web App** from the results.
2. On the Web App blade, select **Create**.
3. On the Create Web App **Basics** tab, set the following configuration:
    - Project Details:
        - **Subscription**: Select the workshop subscription.
        - **Resource Group**: Select the tech-immersion-XXXXX resource group, where XXXXX is the unique Id for each lab attendee.
    - Instance Details:
        - **Name**: Enter techimmersionwebXXXXX, where XXXXX is the unique Id for each lab attendee.
        - **Publish**: Select Code.
        - **Runtime stack**: Select .NET Core 2.1.
        - **Operating System**: Select Windows.
        - **Region**: Select the same region you used for the virtual network and SQL MI.
    - App Service Plan:
        - **Plan**: Select **Create new** and enter **tech-immersion-XXXXX-asp** as the new App Service Plan name, where XXXXX is the unique Id for each lab attendee.
        - **Sku and size**: Accept the default value of Standard S1.
4. Select **Review and create** and then select **Create**.

## Resource configuration

Once the resources above have finished provisioning, complete the steps below to configure each of the resources.

### SQL Server 2008 R2 VM configuration

After provisioning a SQL Server 2008 R2 on Windows 2008 VM for every attendee, each VM instance requires the following configuration:

1. Open port 1433 using an Inbound port rule added to the VM firewall in the Azure portal. (This should have been handled by the provisioning process, but should be verified.)
2. Open port 1433 on the VM's Windows firewall using an inbound port rule on the VM. (This should have been handled by the provisioning process, but should be verified.)
3. Add the **ContosoAutoDb** database to the VM by restoring from the provided `ContosoAutoDb.bak` file.
4. Open the `configure-sql-2008.sql` script found under `lab-files/data/3` in SSMS and execute the script.
   - This script resets the `sa` password, enables mixed mode authentication, enables Service broker, and creates the `WorkshopUser` account.
5. Restart the SQL Server (MSSQLSERVER) Service by right-clicking the database service in SSMS and selecting **Restart**.
6. Use SQL Server Configuration Manager to change the MSSQLSERVER service to run under the `tiuser` account.
   1. On the SqlServer2008r2 VM, open the SQL Server Configuration Manager, and then select **SQL Server Services** from the left-hand treeview.
   2. Right-click the SQL Server (MSSQLSERVER) service and select **Properties** from the context menu.
   3. In the SQL Server (MSSQLSERVER) Properties dialog, select **This account** under Log on as, and enter the following:
      - **Account name**: tiuser
      - **Password**: Password.1234567890
7. Select **OK**.
8. Select **Yes** in the Confirm Account Change dialog.
9. You will now see the **Log On As** value for the SQL Server (MSSQLSERVER) service changed to `./tiuser`.
10. Create a new SMB network share on each SqlServer2008r2 VM.
    1. This will be the folder used by DMS for retrieving backups of the `TailspinToys` database during the database migration process.
    2. On the SqlServer2008 VM, open **Windows Explorer** by selecting its icon on the Windows Task bar.
    3. In the Windows Explorer window, expand **Computer** in the tree view, select **Windows (C:)**, and then select **New folder** in the top menu.
    4. Name the new folder **dms-backups**, then right-click the folder and select **Share with** and **Specific people** in the context menu.
    5. In the File Sharing dialog, ensure the **tiuser** is listed with a **Read/Write** permission level, and then select **Share**.
    6. In the **Network discovery and file sharing** dialog, select the default value of **No, make the network that I am connected to a private network**.
    7. Back on the File Sharing dialog, note the path of the shared folder, `\\SQLSERVER2008R2\dms-backups`, and select **Done** to complete the sharing process.
11. Open SSMS, right-click the `ContosoAutoDb` database, and select **Properties**.
    1. Select the Options page.
    2. Ensure the Recovery model is set to Full.
12. Open SSMS and create a backup of the `ContosoAutoDb` database. **NOTE**: This can be done on one VM, and then the backup file can be copied into the `C:\dms-backups` folder on all the other attendee VMs.
    1. Right-click the `ContosoAutoDb` database in the Object Explorer and select **Tasks -> Back Up...**.
    2. Remove any existing backup destinations.
    3. Next, select **Add** and browse to the `C:\dms-backups` SMB share you created above.
    4. Enter `ContosoAutoDb.bak` as the File name and select **OK**.
    5. Then, select **Media Options** from the **Select a page** box on the left-hand side.
    6. On the Media options page, select **Overwrite all existing backup data** and check **Perform checksum before writing to media**.
    7. Select **OK** to perform the backup.

> **IMPORTANT**: The shared folder path, `\\SQLSERVER2008R2\dms-backups`, should be added to the Environment Details sheet provided to lab attendees.

### JumpBox VM configuration

The JumpBox VM requires the following:

- The latest version of [SQL Server Management Studio](https://go.microsoft.com/fwlink/?linkid=2043154) (SSMS) should be installed (v 18.2 or greater).
- [Microsoft Data Migration Assistant](https://www.microsoft.com/download/details.aspx?id=53595)

### SQL MI configuration

The following configuration must be applied to the SQL MI prior to the workshop.

#### Restore the ContosoAutoDb database

- Restore the `ContosoAutoDb` database from the **ContosoAutoDb.bak** file found under `lab-files/data/3`.
  - This will serve as a shared read-only database for attendees.
- All attendees must have the ability to restore a copy of the `ContosoAutoDb` database to the SQL MI, and name it `ContosoAutoDb-<unique-user-id>`.

#### Enable Advanced Data Security

1. On the SQL MI blade in the Azure portal, select **Advanced Data Security** from the left-hand menu, under Security, and then turn on Advanced Data Security by selecting **ON**.
2. On the Advanced Data Security screen enter the following:
   - **Subscription**: Select the workshop subscription.
   - **Storage account**: Select this and then select **+ Create new**. On the Create storage account blade, enter a globally unique name (e.g., **techimmersionsqlmi**), and select **OK**, accepting the default values for everything else.
   - **Periodic recurring scans**: Select **ON**.
   - **Send scan reports to**: Enter your email address.
   - **Send alerts to**: Enter you email address.
   - **Advanced Threat Protection types**: Select this and ensure all options are checked.
3. Select **Save** to enable **Advanced Data Security**.

### Web App and App Service configuration

The following configuration must be applied to each App Service prior to the workshop.

#### Configure VNet integration with App Services

In this task, you will add the networking configuration to your App Service to enable communication with resources in the VNet.

1. In the [Azure portal](https://portal.azure.com), select **Resource groups** from the left-hand menu, select the **tech-immersion-XXXXX** resource group, and then select the **techimmersionwebXXXXX** App Service from the list of resources.
2. On the App Service blade, select **Networking** from the left-hand menu and then select **Click here to configure** under **VNet Integration**.
3. Select **Add VNet** on the VNet Configuration blade.
4. Select the **tech-immersion-vnet** from the tech-immersion-shared resource group in the Virtual Network dialog.
5. Within a few minutes, the VNet will be added and your App Service will be restarted to apply the changes. Select **Refresh** to see the details. You should see that the certificate status is Certificates in sync. If the certificates are not in sync, wait a few minutes and select **Refresh**.

    > **Note**: In you receive a message adding the Virtual Network to Web App failed, select **Disconnect** on the VNet Configuration blade, and repeat steps 3 - 5 above.

6. Deploy the **ContosoAutoOpsWeb** application, found under `lab-files/data/3` folder.
   - Open the `ContosoAutoOpsWeb.sln` file, and deploy the application to each App Service.
7. Add two connection string values to each web app. These should be under the Connection Strings section on the Application Settings page of each App Service.
   - **ContosoAutoDbContext**: Will initially contain the connection string to the SQL Server 2008 R2 database (e.g., `Server=tcp:<sql-2008-r2-vm-ip-address>,1433;Database=ContosoAutoDb;User ID=WorkshopUser;Password=Password.1!!;Trusted_Connection=False;Encrypt=True;TrustServerCertificate=true;`)
   - **ContosoAutoDbReadOnlyContext**: Will initially contain the connection string to the SQL Server 2008 R2 database (e.g., `Server=tcp:<sql-2008-r2-vm-ip-address>,1433;Database=ContosoAutoDb;User ID=WorkshopUser;Password=Password.1!!;Trusted_Connection=False;Encrypt=True;TrustServerCertificate=true;`)
   - Both connection string will have a type of **SQLServer**.

#### Verify Virtual Network Gateway configuration

The Point-to-site configuration of the VNet Gateway has a tendency to change the Tunnel type during the VNet integration process, so the proper setting of **SSTP (SSL)** should be verified.

1. Navigate to the VNet Gateway in the shared resource group in the Azure portal.
2. Select **Point-to-site configuration** from the left-hand menu.
3. Ensure the Tunnel type is set to **SSTP (SSL)**. Change it to that setting, if not, and select **Save**.

   ![The tunnel type is highlighted on the point-to-site configuration page.](point-to-site-configuration.png "Point-to-site configuration")

## Environment Details

The following values should be added to the Environment Details sheet provided to attendees.

- SqlServer2008R2 VM IP Address: IP address of each user's SqlServer2008R2 VM instance
- SMB Network Share Path: `\\SQLSERVER2008R2\dms-backups`
- Application ID: `appId` value from the output of creating the service principal
- Application Key: `password` value from the output of creating the service principal
- SQL MI URL: Can be retrieved from the SQL MI blade in the Azure portal
- SQL MI Admin login: tiuser
- SQL MI Admin password: Password.1234567890
