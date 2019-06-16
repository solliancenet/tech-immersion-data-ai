# Open source databases at scale setup

Complete the steps below to prepare the environment for the [Day 1, Experience 5](../../../day1-exp5/README.md) lab.

## Pre-requisites

- Each attendee needs a resource group with:
  - Azure Database for PostgreSQL Hyperscale (Citus) server group

A computer or VM the attendee will use for the lab requires the following:

- [pgAdmin](https://www.pgadmin.org/download/) 4 or greater
- [ora2pg](http://ora2pg.darold.net/) v20 or greater ([Windows installation instructions](https://github.com/Microsoft/DataMigrationTeam/blob/master/Whitepapers/Steps%20to%20Install%20ora2pg%20on%20Windows.pdf))
  - [Oracle Instant Client](https://www.oracle.com/technetwork/database/enterprise-edition/downloads/index.html)

To install the Oracle Instant Client on Windows, perform these steps:

- Go to the website and select **See All** next to the `Microsoft Windows x64 (64-bit)` line item underneath the latest database version.

  ![The See All link is highlighted.](media/oracle-see-all.png 'Oracle website')

- Scroll down until you find the `Oracle Database 19c Client (19.X) for Microsoft Windows x64 (64-bit)` section, then select the link to download the WINDOWS.X64_VERSION_client.zip file (not \_client_home.zip). You may need to log in to an Oracle account to download.

  ![The Oracle client zip file is highlighted.](media/oracle-client-download.png 'Oracle website')

- When the Oracle Database Client installer launches, select the **Instant Client** option for the installation type.

## PostgreSQL configuration

Fill out the new server details form with the following information:

- Server group name: enter a unique name for the new server group, such as **ti-postgres-SUFFIX**, which will also be used for a server subdomain.
- Admin username: currently required to be the value **citus**, and can't be changed.
- Password: **Record the password and make it available to attendees**.
- Location: use the location you provided when creating the resource group, or the closest available.
- Configure server group: leave the settings in this section unchanged.

## Azure VM with Oracle

For the Oracle to PostgreSQL migration portion of the experience, we need a VM with Oracle installed that all users can access for the exercise.

1. Create the VM with an Oracle image.

   ```bash
   az vm create \
     --resource-group tech-immersion \
     --name tech-immersion-ora \
     --image Oracle:Oracle-Database-Ee:12.1.0.2:latest \
     --size Standard_DS2_v2 \
     --admin-username oradm \
     --admin-password 'Abc!1234567890'
   ```

2. SSH into the VM and run the following to switch to the Oracle user:

   ```bash
   sudo su - oracle
   ```

3. Execute the following to silently create a database:

   ```bash
   dbca -silent \
       -createDatabase \
       -templateName General_Purpose.dbc \
       -gdbname ContosoAuto \
       -sid ContosoAuto \
       -responseFile NO_VALUE \
       -characterSet AL32UTF8 \
       -sysPassword 'Abc!1234567890' \
       -systemPassword 'Abc!1234567890' \
       -createAsContainerDatabase true \
       -numberOfPDBs 1 \
       -pdbName pdb1 \
       -pdbAdminPassword 'Abc!1234567890' \
       -databaseType MULTIPURPOSE \
       -automaticMemoryManagement false \
       -storageType FS \
       -ignorePreReqs
   ```

4. Open the VM in the Azure portal and turn **off** auto-shutdown.
