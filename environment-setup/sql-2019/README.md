# SQL Server 2019 big data cluster setup

Complete the steps below to deploy and configure SQL Server 2019 for the [Day 1, Experience 1](../../day1-exp1/) lab.

## Pre-requisites

The computer or VM on which you run the scripts to deploy the cluster and restore the databases requires the following:

- PowerShell
- Python3
- [Azure CLI](https://docs.microsoft.com/cli/azure/install-azure-cli?view=azure-cli-latest)
- [mssqlctl](https://docs.microsoft.com/en-us/sql/big-data-cluster/deploy-install-mssqlctl?view=sql-server-ver15)
- [kubectl](https://kubernetes.io/docs/tasks/tools/install-kubectl/#install-with-powershell-from-psgallery)

## Lab computer pre-requisites

The computer or VM on which the attendee completes the lab requires the following:

- [SQL Server Management Studio](https://go.microsoft.com/fwlink/?linkid=2078638) (SSMS) v18.0 (Preview 7) or greater
- [Azure Data Studio](https://docs.microsoft.com/sql/azure-data-studio/download?view=sql-server-ver15)
  - [SQL Server 2019 extension](https://docs.microsoft.com/sql/azure-data-studio/sql-server-2019-extension?view=sql-server-ver15)
- SQL Server 2019 login credentials provided for the lab environment
- Azure SQL Database login credentials provided for the lab environment

## Regional limitations

**L Series VMs** (required for SQL 2019 Big Data Clusters): East US 2, West US, West US 2, and a limited set of others worldwide

**Azure Machine Learning service**: East US, East US 2, West US 2, West Central US, South Central US, and a limited set of others worldwide

## Deployment steps

Open PowerShell and execute the following to deploy the clusters in preparation for the lab. **Note:** these steps should be run for each student.

1.  Before running the script, you must log in to **student's** Azure account with Azure CLI at least once.

    ```bash
    az login
    ```

2.  Use the following steps to run the deployment script. This script will create an AKS service in Azure and then deploy a SQL Server 2019 big data cluster to AKS. The [deploy-sql-big-data-aks.py](deploy-sql-big-data-aks.py) script located in this folder is customized with environment variables that set the memory allocation for the cluster.

    > **Please note:** this script can take up to 30 minutes to complete.

    ```bash
    python deploy-sql-big-data-aks.py
    ```

3.  When prompted, enter the following information:

    | Value                     | Description                                                                                                                                                                                            |
    | ------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
    | **Azure subscription ID** | The Azure subscription ID to use for AKS. You can list all of your subscriptions and their IDs by running `az account list` from another command line.                                                 |
    | **Azure resource group**  | The Azure resource group name to create for the AKS cluster. (suggest **tech-immersion**)                                                                                                              |
    | **Docker username**       | The Docker username provided to you as part of the limited public preview.                                                                                                                             |
    | **Docker password**       | The Docker password provided to you as part of the limited public preview.                                                                                                                             |
    | **Azure region**          | The Azure region for the new AKS cluster (default **westus**).                                                                                                                                         |
    | **Machine size**          | Set to **Standard_L8s**.                                                                                                                                                                               |
    | **Worker nodes**          | Set the number of worker nodes in the AKS cluster to **3**.                                                                                                                                            |
    | **Cluster name**          | Enter a **unique name for the student**. This sets the name of both the AKS cluster and the big data cluster. The name of the cluster must be only lower case alpha-numeric characters, and no spaces. |
    | **Password**              | Password for the controller, HDFS/Spark gateway, and master instance (default **MySQLBigData2019**).                                                                                                   |
    | **Controller user**       | Username for the controller user (default: **admin**).                                                                                                                                                 |

    You can run the following at any time to get the status of the cluster:

    ```bash
    kubectl get all -n <your-cluster-name>
    ```

4.  When the cluster is done deploying, you will see an output of the various IP addresses for the cluster. **Copy the SQL Server Master Instance and HDFS/KNOX values** and save them to a text file that the students can use for reference.

    Example:

    - SQL Server master instance:
        - IP
          - 52.179.172.24
        - PORT
          - 31433
    - HDFS/KNOX:
      - IP
        - 52.167.114.239
      - PORT
        - 30443

5.  Execute the following to upload and restore the sales database:

    ```bash
    bootstrap-sample-db.sh <namespace> <master_ip> <sa password> <path to .bak file> <KNOX_IP>
    ```

    Example:

    ```bash
    ./bootstrap-sample-db.sh sqlbigdata2019 52.179.172.24 MySQLBigData2019 ./ 52.167.114.239
    ```

    > **Note:** the `tpcxbb_1gb.bak` file must be copied to the folder where the script is located before running.

6.  Execute the following to upload and restore the ContosoAutoDW database:

    ```bash
    bootstrap-sample-db_ContosoAutoDW.sh <namespace> <master_ip> <sa password> <path to .bak file> <KNOX_IP>
    ```

    Example:

    ```bash
    ./bootstrap-sample-db_ContosoAutoDW.sh sqlbigdata2019 52.179.172.24 MySQLBigData2019 ./ 52.167.114.239
    ```

    > **Note:** the `ContosoAutoDW.bak` file must be copied to the folder where the script is located before running.

## Post-deployment

After completing all of the above steps, complete the following to connect to the **ContosoAutoDW** database and enlarge it in preparation for the lab.

### Connect with SQL Server Management Studio

1.  On the bottom-left corner of your Windows desktop, locate the search box next to the Start Menu. Type **SQL Server Management Studio**, then select the SQL Server Management Studio desktop app in the search results.

    ![The search box has "SQL Server Management Studio" entered into it and the desktop app is highlighted in the results.](../../day1-exp1/media/launch-ssms.png 'Launch SQL Server Management Studio')

2.  Within the Connection dialog that appears, configure the following:

    - **Server name:** Enter the IP address, followed by port number `31433`. For example: `123.123.123.123,31433`.
    - **Login:** Enter "sa".
    - **Password:** Enter the password provided to you for this lab.
    - **Remember password:** Checked.

    ![The Connect form is filled out with the previously mentioned settings entered into the appropriate fields.](../../day1-exp1/media/ssms-connection.png 'SQL Server Management Studio - Connect')

3.  Click **Options >>**.

4.  Select the **Additional Connection Parameters** tab. In the text area below, enter `TrustServerCertificate=True`. This is needed because the server certificates are dynamically generated for the Big Data Clusters, and are self-signed.

    ![The Additional Connection Parameters tab is selected and the TrustServerCertificate=True value is highlighted.](../../day1-exp1/media/ssms-connection-additional.png 'Additional Connection Parameters')

5.  Click **Connect**.

### Enlarge the ContosoAutoDW database

The script you will execute below enlarges the ContosoAutoDW database. This helps make the impact of the intelligent QP scripts executed during the lab more pronounced.

1.  Open SQL Server Management Studio (SSMS) and connect to your SQL Server 2019 cluster. If you are unsure of how to do this, refer to [Connect with SQL Server Management Studio](#connect-with-sql-server-management-studio) above.

2.  Right-click on `ContosoAutoDW`, then select **New Query**. This will open a new query window into which you can paste the following query.

    ![ContosoAutoDW is selected and the New Query menu option is highlighted.](../../day1-exp1/media/ssms-new-query.png 'New Query')

3.  Paste the following query into the new query window:

    ```sql
    USE ContosoAutoDW;
    GO

    /*
      Assumes a fresh restore of ContosoAutoDW.
    */

    IF OBJECT_ID('Fact.OrderHistory') IS NULL BEGIN
        SELECT [Order Key], [City Key], [Customer Key], [Stock Item Key], [Order Date Key], [Picked Date Key], [Salesperson Key], [Picker Key], [WWI Order ID], [WWI Backorder ID], Description, Package, Quantity, [Unit Price], [Tax Rate], [Total Excluding Tax], [Tax Amount], [Total Including Tax], [Lineage Key]
        INTO Fact.OrderHistory
        FROM Fact.[Order];
    END;

    ALTER TABLE Fact.OrderHistory
    ADD CONSTRAINT PK_Fact_OrderHistory PRIMARY KEY NONCLUSTERED([Order Key] ASC, [Order Date Key] ASC)WITH(DATA_COMPRESSION=PAGE);
    GO

    CREATE INDEX IX_Stock_Item_Key
    ON Fact.OrderHistory([Stock Item Key])
    INCLUDE(Quantity)
    WITH(DATA_COMPRESSION=PAGE);
    GO

    CREATE INDEX IX_OrderHistory_Quantity
    ON Fact.OrderHistory([Quantity])
    INCLUDE([Order Key])
    WITH(DATA_COMPRESSION=PAGE);
    GO

    /*
      Reality check... Starting count should be 231,412
    */
    SELECT COUNT(*) FROM Fact.OrderHistory;
    GO

    /*
      Make this table bigger (exec as desired)
      Notice the "GO 4"
    */
    INSERT Fact.OrderHistory([City Key], [Customer Key], [Stock Item Key], [Order Date Key], [Picked Date Key], [Salesperson Key], [Picker Key], [WWI Order ID], [WWI Backorder ID], Description, Package, Quantity, [Unit Price], [Tax Rate], [Total Excluding Tax], [Tax Amount], [Total Including Tax], [Lineage Key])
    SELECT [City Key], [Customer Key], [Stock Item Key], [Order Date Key], [Picked Date Key], [Salesperson Key], [Picker Key], [WWI Order ID], [WWI Backorder ID], Description, Package, Quantity, [Unit Price], [Tax Rate], [Total Excluding Tax], [Tax Amount], [Total Including Tax], [Lineage Key]
    FROM Fact.OrderHistory;
    GO 4

    /*
      Should be 3,702,592
    */
    SELECT COUNT(*) FROM Fact.OrderHistory;
    GO

    IF OBJECT_ID('Fact.OrderHistoryExtended') IS NULL BEGIN
        SELECT [Order Key], [City Key], [Customer Key], [Stock Item Key], [Order Date Key], [Picked Date Key], [Salesperson Key], [Picker Key], [WWI Order ID], [WWI Backorder ID], Description, Package, Quantity, [Unit Price], [Tax Rate], [Total Excluding Tax], [Tax Amount], [Total Including Tax], [Lineage Key]
        INTO Fact.OrderHistoryExtended
        FROM Fact.[OrderHistory];
    END;

    ALTER TABLE Fact.OrderHistoryExtended
    ADD CONSTRAINT PK_Fact_OrderHistoryExtended PRIMARY KEY NONCLUSTERED([Order Key] ASC, [Order Date Key] ASC)
    WITH(DATA_COMPRESSION=PAGE);
    GO

    CREATE INDEX IX_Stock_Item_Key
    ON Fact.OrderHistoryExtended([Stock Item Key])
    INCLUDE(Quantity);
    GO

    /*
      Should be 3,702,592
    */
    SELECT COUNT(*) FROM Fact.OrderHistoryExtended;
    GO

    /*
      Make this table bigger (exec as desired)
      Notice the "GO 2"
    */
    INSERT Fact.OrderHistoryExtended([City Key], [Customer Key], [Stock Item Key], [Order Date Key], [Picked Date Key], [Salesperson Key], [Picker Key], [WWI Order ID], [WWI Backorder ID], Description, Package, Quantity, [Unit Price], [Tax Rate], [Total Excluding Tax], [Tax Amount], [Total Including Tax], [Lineage Key])
    SELECT [City Key], [Customer Key], [Stock Item Key], [Order Date Key], [Picked Date Key], [Salesperson Key], [Picker Key], [WWI Order ID], [WWI Backorder ID], Description, Package, Quantity, [Unit Price], [Tax Rate], [Total Excluding Tax], [Tax Amount], [Total Including Tax], [Lineage Key]
    FROM Fact.OrderHistoryExtended;
    GO 2

    /*
      Should be 14,810,368
    */
    SELECT COUNT(*) FROM Fact.OrderHistoryExtended;
    GO

    UPDATE Fact.OrderHistoryExtended
    SET [WWI Order ID] = [Order Key];
    GO
    ```

4.  Click the **Execute** button on the top toolbar to run the script, or simply enter _F5_ on your keyboard.

    It will take **up to 10 minutes** to execute this query.

    ![The New Query window has the SQL query pasted into it, and the Execute button is highlighted.](../../day1-exp1/media/ssms-execute.png 'SSMS Execute query')

5.  After the query is finished, you should see output similar to the screenshot below (highlighted in red) in the Results window. Also, the lower-right portion of the status bar shows the total execution time, in this case just under 11 minutes.

    ![The query results are highlighted, as well as the execution time.](../../day1-exp1/media/ssms-execution-completed.png 'SSMS query results')

### Set the database compatibility level to 150 and create a UDF

This lab requires the database compatibility level to be set to 150, and uses a user-defined function (UDF) for one of the queries. Rather than having students set the level and create the UDF, we do this for them ahead of time. This only needs to be done one time since the ContosoAutoDW database is shared.

1.  Open SQL Server Management Studio (SSMS) and connect to your SQL Server 2019 cluster. If you are unsure of how to do this, refer to [Connect with SQL Server Management Studio](#connect-with-sql-server-management-studio) above.

2.  Right-click on `ContosoAutoDW`, then select **New Query**. This will open a new query window into which you can paste the following query.

3.  Paste the following query into the query window. This ensures that the database compatibility level is set to `150`, which is the new compatibility level for SQL Server 2019, enabling the most recent intelligent QP features. It also creates a new user-defined function (UDF) named `customer_category` that will be called inline from the two queries that follow in order to show QP improvements on scalar UDF inlining.

    ```sql
    USE ContosoAutoDW;
    GO

    ALTER DATABASE ContosoAutoDW
    SET COMPATIBILITY_LEVEL = 150;
    GO

    ALTER DATABASE SCOPED CONFIGURATION
    CLEAR PROCEDURE_CACHE;
    GO
    /*
    Adapted from SQL Server Books Online
    https://docs.microsoft.com/en-us/sql/relational-databases/user-defined-functions/scalar-udf-inlining?view=sqlallproducts-allversions
    */
    CREATE OR ALTER FUNCTION
      dbo.customer_category(@CustomerKey INT)
    RETURNS CHAR(10) AS
    BEGIN
      DECLARE @total_amount DECIMAL(18,2);
      DECLARE @category CHAR(10);

      SELECT @total_amount =
      SUM([Total Including Tax])
      FROM [Fact].[OrderHistory]
      WHERE [Customer Key] = @CustomerKey;

      IF @total_amount < 500000
        SET @category = 'REGULAR';
      ELSE IF @total_amount < 1000000
        SET @category = 'GOLD';
      ELSE
        SET @category = 'PLATINUM';

      RETURN @category;
    END
    GO
    ```

### Create copies of the Sales database for each attendee

Each attendee needs their own copy of the Sales database. The name pattern should be `sales_YOUR_UNIQUE_IDENTIFIER`, where `YOUR_UNIQUE_IDENTIFIER` is a value assigned to each attendee that is unique to them. Perhaps it is part of their username.

TODO: Add steps to perform this function.

### Connect with Azure Data Studio

1.  On the bottom-left corner of your Windows desktop, locate the search box next to the Start Menu. Type **Azure Data Studio**, then select the Azure Data Studio desktop app in the search results.

    ![The search box has "Azure Data Studio" entered into it and the desktop app is highlighted in the results.](../../day1-exp1/media/launch-azure-data-studio.png 'Launch Azure Data Studio')

2.  Within Azure Data Studio, select **Servers** from the top of the left-hand menu, then select **New Connection** from the top toolbar to the right of the menu.

    ![The Servers menu icon is selected, as well as the new connection icon.](../../day1-exp1/media/ads-new-connection-link.png 'Azure Data Studio')

3.  Within the Connection dialog, configure the following:

    - **Connection type:** Select Microsoft SQL Server.
    - **Server:** Enter the IP address, followed by port number `31433`. For example: `123.123.123.123,31433`.
    - **Username:** Enter "sa".
    - **Password:** Enter the password provided to you for this lab.
    - **Remember password:** Checked.
    - Leave all other options at their default values.

    ![The Connection form is filled out with the previously mentioned settings entered into the appropriate fields.](../../day1-exp1/media/ads-new-connection.png 'Azure Data Studio - New Connection')

4.  Click **Connect**.

### Upload lab files to HDFS

Upload required lab files to HDFS within the provisioned big data cluster.

1.  Within Azure Data Studio, scroll down below the list of SQL Server 2019 databases to find the **Data Services** folder. Expand that folder, then expand the **HDFS** sub-folder. **Right-click on HDFS**, then select **New directory** on the context menu.

    ![The HDFS folder and New directory menu items are highlighted.](../../day1-exp1/media/ads-new-directory-link.png "New directory")

2.  In the new dialog that appears, type "data", then press Enter on your keyboard.

    ![The new directory dialog is displayed with data typed in as the new directory name.](../../day1-exp1/media/ads-new-directory.png 'New directory dialog')

3.  **Right-click** on your newly created **data** folder, then select **Upload files**.

    ![The data folder and Upload files menu item are highlighted.](../../day1-exp1/media/ads-upload-files-link.png 'Upload files')

4.  Upload each of the following files:

    - [fleet-formatted.csv](fleet-formatted.csv)
    - [stockitemholdings.csv](stockitemholdings.csv)
    - [training-formatted.csv](training-formatted.csv)

### Install the required Python libraries

This needs to be done from each user's jump box. It is required for the Python libraries for the big data cluster to be installed on the user's machine before they can execute Jupyter notebooks.

1.  Within Azure Data Studio, right-click on the connection (1) then select **Manage** (2). Select the **SQL Server Big Data Cluster** tab (3). Select **New Notebook** (4).

     ![New Notebook.](../../day1-exp1/media/ads-new-notebook.png 'New Notebook')

2.  When prompted, select the option to install the required Python libraries to the default location.

### Create Azure SQL Server and import CA_Commerce bacpac

All attendees will access one Azure SQL Database named `CA_Commerce` for the lab. They only require Read Only access.

1.  Provision a shared Azure SQL Server that will be used for all attendees.

2.  Create a user with Read Only access.

3.  Share the Azure SQL Server path, username, and password with attendees (add to attendee notes).

4.  Follow [these steps](https://docs.microsoft.com/en-us/azure/sql-database/sql-database-import) to create a new Azure SQL Database on this new SQL Server, using the [CA_Commerce.bacpac](CA_Commerce.bacpac) file.