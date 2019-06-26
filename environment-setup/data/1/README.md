# SQL Server 2019 big data cluster setup

Complete the steps below to deploy and configure SQL Server 2019 for the [Day 1, Experience 1](../../../day1-exp1/README.md) lab.

## Pre-requisites

- Expect event to have one SQL Big Data Cluster pre-provisioned
- Output of `deploy-sql-big-data-aks.py` script needs to be captured and processed to acquire SQL Server master instance IP/port and the HDFS/KNOX IP/port.
- The following databases are used:
  - `ContosoAutoDW` - one instance of this database to be used for all attendees on the cluster. Attendees will need full permissions on this database (?).
  - `sales` - multiple instances of this database, one instance for each attendee, each attendee has full permissions. (?)
    - Permissions needed to create external tables:
      CREATE TABLE
      ALTER ANY SCHEMA
      ALTER ANY EXTERNAL DATA SOURCE
      ALTER ANY EXTERNAL FILE FORMAT
      CONTROL DATABASE
  - `CA_Commerce` - one instance of this Azure SQL Database needs to be deployed for all attendees. Attendees should only have read-only access to the `Reviews` table.
- The following data needs to be pre-loaded on HDFS:
  - `data/stockitemholdings.csv`
  - `data/training-formatted.csv`
  - `data/fleet-formatted.csv`

The computer or VM on which you run the scripts to deploy the cluster and restore the databases requires the following:

- PowerShell
- Python3
- [Azure CLI](https://docs.microsoft.com/cli/azure/install-azure-cli?view=azure-cli-latest)
- [mssqlctl](https://docs.microsoft.com/en-us/sql/big-data-cluster/deploy-install-mssqlctl?view=sql-server-ver15)
- [kubectl](https://kubernetes.io/docs/tasks/tools/install-kubectl/#install-with-powershell-from-psgallery)

## Lab computer pre-requisites

The computer or VM on which the attendee completes the lab requires the following:

- [SQL Server Management Studio](https://go.microsoft.com/fwlink/?linkid=2078638) (SSMS) v18.0 or greater
- [Azure Data Studio](https://docs.microsoft.com/sql/azure-data-studio/download?view=sql-server-ver15)
  - [SQL Server 2019 extension](https://docs.microsoft.com/en-us/sql/azure-data-studio/sql-server-2019-extension?view=sql-server-2017)
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

5.  Execute the following to retrieve the **External-IP** address for the endpoint **controller-svc-external** service.

    ```bash
    kubectl get svc controller-svc-external -n <namespace>
    ```

6.  Provide students with the address to the **controller-svc-external** service from the previous step in the following format: `https://<IP-of-controller-svc-external>:30080`. Name the property as **SQL SERVER Controller IP ADDRESS**.

7.  Execute the following to upload and restore the sales database:

    ```bash
    bootstrap-sample-db.sh <namespace> <master_ip> <sa password> <path to .bak file> <KNOX_IP>
    ```

    Example:

    ```bash
    ./bootstrap-sample-db.sh sqlbigdata2019 52.179.172.24 MySQLBigData2019 ./ 52.167.114.239
    ```

    > **Note:** the `tpcxbb_1gb.bak` file must be copied to the folder where the script is located before running. You can **download the database** [here](https://databricksdemostore.blob.core.windows.net/data/contoso-auto/tpcxbb_1gb.bak).

8.  Execute the following to upload and restore the ContosoAutoDW database:

    ```bash
    bootstrap-sample-db_ContosoAutoDW.sh <namespace> <master_ip> <sa password> <path to .bak file> <KNOX_IP>
    ```

    Example:

    ```bash
    ./bootstrap-sample-db_ContosoAutoDW.sh sqlbigdata2019 52.179.172.24 MySQLBigData2019 ./ 52.167.114.239
    ```

    > **Note:** the `ContosoAutoDW.bak` file must be copied to the folder where the script is located before running. You can **download the database** [here](https://databricksdemostore.blob.core.windows.net/data/contoso-auto/ContosoAutoDW.bak).

## Post-deployment

After completing all of the above steps, complete the following to connect to the **ContosoAutoDW** database and enlarge it in preparation for the lab.

### Connect with SQL Server Management Studio

1.  On the bottom-left corner of your Windows desktop, locate the search box next to the Start Menu. Type **SQL Server Management Studio**, then select the SQL Server Management Studio desktop app in the search results.

    ![The search box has "SQL Server Management Studio" entered into it and the desktop app is highlighted in the results.](../../../day1-exp1/media/launch-ssms.png 'Launch SQL Server Management Studio')

2.  Within the Connection dialog that appears, configure the following:

    - **Server name:** Enter the IP address, followed by port number `31433`. For example: `123.123.123.123,31433`.
    - **Login:** Enter "sa".
    - **Password:** Enter the password provided to you for this lab.
    - **Remember password:** Checked.

    ![The Connect form is filled out with the previously mentioned settings entered into the appropriate fields.](../../../day1-exp1/media/ssms-connection.png 'SQL Server Management Studio - Connect')

3.  Click **Connect**.

### Create copies of the Sales database for each attendee

Each attendee needs their own copy of the Sales database. The name pattern should be `sales_YOUR_UNIQUE_IDENTIFIER`, where `YOUR_UNIQUE_IDENTIFIER` is a value assigned to each attendee that is unique to them. Perhaps it is part of their username.

TODO: Add steps to perform this function.

### Connect with Azure Data Studio

1.  On the bottom-left corner of your Windows desktop, locate the search box next to the Start Menu. Type **Azure Data Studio**, then select the Azure Data Studio desktop app in the search results.

    ![The search box has "Azure Data Studio" entered into it and the desktop app is highlighted in the results.](../../../day1-exp1/media/launch-azure-data-studio.png 'Launch Azure Data Studio')

2.  Within Azure Data Studio, select **Servers** from the top of the left-hand menu, then select **New Connection** from the top toolbar to the right of the menu.

    ![The Servers menu icon is selected, as well as the new connection icon.](../../../day1-exp1/media/ads-new-connection-link.png 'Azure Data Studio')

3.  Within the Connection dialog, configure the following:

    - **Connection type:** Select Microsoft SQL Server.
    - **Server:** Enter the IP address, followed by port number `31433`. For example: `123.123.123.123,31433`.
    - **Username:** Enter "sa".
    - **Password:** Enter the password provided to you for this lab.
    - **Remember password:** Checked.
    - Leave all other options at their default values.

    ![The Connection form is filled out with the previously mentioned settings entered into the appropriate fields.](../../../day1-exp1/media/ads-new-connection.png 'Azure Data Studio - New Connection')

4.  Click **Connect**.

### Upload lab files to HDFS

Upload required lab files to HDFS within the provisioned big data cluster.

1.  Within Azure Data Studio, scroll down below the list of SQL Server 2019 databases to find the **Data Services** folder. Expand that folder, then expand the **HDFS** sub-folder. **Right-click on HDFS**, then select **New directory** on the context menu.

    ![The HDFS folder and New directory menu items are highlighted.](../../../day1-exp1/media/ads-new-directory-link.png 'New directory')

2.  In the new dialog that appears, type "data", then press Enter on your keyboard.

    ![The new directory dialog is displayed with data typed in as the new directory name.](../../../day1-exp1/media/ads-new-directory.png 'New directory dialog')

3.  **Right-click** on your newly created **data** folder, then select **Upload files**.

    ![The data folder and Upload files menu item are highlighted.](../../../day1-exp1/media/ads-upload-files-link.png 'Upload files')

4.  Upload each of the following files:

    - [fleet-formatted.csv](fleet-formatted.csv)
    - [stockitemholdings.csv](stockitemholdings.csv)
    - [training-formatted.csv](training-formatted.csv)

### Install the required Python libraries

This needs to be done from each user's jump box. It is required for the Python libraries for the big data cluster to be installed on the user's machine before they can execute Jupyter notebooks.

1.  Within Azure Data Studio, right-click on the connection (1) then select **Manage** (2). Select the **SQL Server Big Data Cluster** tab (3). Select **New Notebook** (4).

    ![New Notebook.](../../../day1-exp1/media/ads-new-notebook.png 'New Notebook')

2.  When prompted, select the option to install the required Python libraries to the default location.

### Create Azure SQL Server and import CA_Commerce bacpac

All attendees will access one Azure SQL Database named `CA_Commerce` for the lab. They only require Read Only access.

1.  Provision a shared Azure SQL Server that will be used for all attendees.

2.  Create a user with Read Only access.

3.  Share the Azure SQL Server path, username, and password with attendees (add to attendee notes).

4.  Follow [these steps](https://docs.microsoft.com/en-us/azure/sql-database/sql-database-import) to create a new Azure SQL Database on this new SQL Server, using the [CA_Commerce.bacpac](CA_Commerce.bacpac) file.
