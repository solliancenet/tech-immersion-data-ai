# Azure Data Factory and Modern Data Warehouse setup

Complete the steps below to prepare the environment for the [Data, Experience 5](../../../data-exp5/README.md) and [Data, Experience 6](../../../data-exp6/README.md) labs.

## Pre-requisites

### Shared resources

The following resources should be provisioned as a shared resource for all lab participants.

- Service Principal account
  - Name: tech-immersion-sp

### Per attendee resources

The following resources should be provisioned on a per-attendee basis in the attendee's resource group:

- Azure Data Factory v2
- Cosmos DB
- Azure Data Lake Storage Gen2 (ADLS Gen2)
- Azure Blob Storage Account
- Azure SQL Data Warehouse
- Azure Key Vault
- Azure Databricks workspace

## Azure Data Factory v2 configuration

Each ADF instance should have the following activities and configuration:

- Pipelines:
  - CopyData
    - Activities:
      - CopyVehicleInfoFromStorage
        - Retrieves the `VehicleInfo.csv` file from blob storage container `data-exp3-data`
        - Writes data to ADLS Gen2 account filesystem `contosoauto/VehicleInfo.csv`
      - CopyTelemetryDataFromCosmos
        - Needs Key to connect to Cosmos DB in Linked Service
        - Writes data to ADLS Gen2 account filesystem `contosoauto/VehicleTelemetry.json`
          - File format: JSON
          - File pattern: Set of objects
      - CopyCarDataFromStorage
        - Retrieves the `Cars.csv` file from blob storage container `data-exp3-data`
        - Writes data to ADLS Gen2 account filesystem `contosoauto/Cars.csv`

> The files `VehicleInfo.csv` and `Cars.csv` can be placed in the user's storage account, or can be placed into a shared storage account. ADF will need to be configured to connect to the `data-exp3-data` container in the target storage account using a key or SAS token.

- There is an ARM template at `environment-setup/data/6/adf-arm-template.zip` that can be used to set up the ADF pipeline.
- You can also view the pipeline JSON in the `environment-setup/data/6/pipeline.json` file.

## Cosmos DB configuration

Create a new database and container (SQL API) with the following parameters:

- **Database id**: ContosoAuto
- **Provision database throughput**: Unchecked
- **Container id**: telemetry
- **Partition key**: /vin
- **Throughput**: 15000

## Azure Data Lake Storage Gen 2

- Filesystem will be initialized by attendees, so no additional configuration needed for ADLS Gen2.

## Azure Blob Storage account

The following step should be taken for the Blob Storage account:

1. Create a container in the storage account named `data-exp3-data`.
2. Upload the provided `Cars.csv` and `VehicleInfo.csv` files to the `data-exp3-data` container.
3. Create a container in the storage account named `dwtemp`.
4. Create a container in the storage account named `data`.
5. Upload the provided `trip_data_1.csv` and `trip_fare_1.csv` files to the `data` container.

## Azure SQL Data Warehouse

The following configuration needs to be performed on the SQL DW:

1. Admin credentials
   - User name: ti-admin
   - Password: Password.1!!
2. Database name: tech-immersion-sql-dw
3. Add the user's jumpbox IP address to the SQL DW firewall
4. Create a master key in the database by running the following script against the tech-immersion-sql-dw database
   1. `CREATE MASTER KEY ENCRYPTION BY PASSWORD = 'Password.1!!';`

## Service Principal configuration

A service principal (SP) should be created in Azure Active Directory for the subscription hosting the lab environments. Only one SP is needed, and it can be shared by all lab attendees.

- Copy Application ID (will be added to Key Vault)
- Generate password key for the SP (will be added to Key Vault)
- Assign to Storage Blob Data Contributor role in the ADLS Gen2 account
  - Select Access Control (IAM) on the ADLS Gen2 account blade.
  - Select Add.
  - Select the role of Storage Blob Data Contributor.
  - Select the Service Principal tech-immersion-sp Service Principal used for this workshop.
  - Save the role assignment.

## Azure Key Vault configuration

Key Vault is used to store secrets and values which need to be accessible from Databricks notebooks.

The following Access policies should be added to Key Vault:

- Azure Databricks account
  - Access rights to Secrets (Get, List permissions)

The following secrets must be added to Key Vault:

| Name                      | Value                                               |
| ------------------------- | --------------------------------------------------- |
| ADLS-Gen2-Account-Name    | `adlsstrgXXXXX`                                     |
| Azure-Tenant-ID           | `f94768c8-8714-4abe-8e2d-37a64b18216a`              |
| ContosoAuto-SP-Client-ID  | `ea2ca9d8-6691-4e6e-b5ee-2d246fd3f0c7`              |
| ContosoAuto-SP-Client-Key | `eSQ8LALrZqo74YcxXhHPRML2Fz37aHOmxI/Z89TCk+o=`      |
| Cosmos-DB-Key             | The Primary Key for the user's Cosmos DB instance   |
| Cosmos-DB-Uri             | The URI for the user's Cosmos DB instance           |
| Sql-Dw-Password           | `Password.1!!`                                      |
| Sql-Dw-Server-Name        | The server name of the user's SQL DW                |
| Storage-Account-Key       | The primary key for the user's Blob Storage account |
| Storage-Account-Name      | The account name of the user's Blob Storage account |

## Azure Databricks workspace configuration

For each attendee's Databricks workspace, the following configuration should be set:

1. Run the `Cluster Setup.dbc` notebook (found in environment-setup/data/6) to create a `Standard_DS3_v2` cluster
   - Running Databricks version `5.2.x-scala2.11`
   - Using Python Version 3
   - Cluster should have the Azure Spark Cosmos DB Spark connector library installed (Maven Coordinates: `com.microsoft.azure:azure-cosmosdb-spark_2.4.0_2.11:1.3.5`)
   - Spark Config
     - Add value: `spark.databricks.delta.preview.enabled true`
2. Key Vault-backed secrets and scopes should be enabled
   - Instructions <https://docs.azuredatabricks.net/user-guide/secrets/secret-scopes.html#akv-ss>
   - Secret scope name: **key-vault-secrets**
   - Managed Principal: Select **Creator**
   - DNS Name: This will be the DNS name assigned to each user-specific Azure Key Vault instance.
     - Can be copied from the Overview blade of the user's Key Vault.
   - Resource ID: This will be the resource ID assigned to the user-specific Azure Key Vault instances.
     - Can be copied from the **Properties** blade of the user's Key Vault.
3. Add the `Tech-Immersion.dbc` notebook (found in lab-files/data/6) to the Shared workspace folder.
