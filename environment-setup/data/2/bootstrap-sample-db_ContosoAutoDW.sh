#!/bin/bash
set -e
set -o pipefail
USAGE_MESSAGE="USAGE: $0 <CLUSTER_NAMESPACE> <SQL_MASTER_IP> <SQL_MASTER_SA_PASSWORD> <BACKUP_FILE_PATH> <KNOX_IP> [<KNOX_PASSWORD>]"
ERROR_MESSAGE="Bootstrap of the sample database failed."

# Print usage if mandatory parameters are missing
: "${1:?$USAGE_MESSAGE}"
: "${2:?$USAGE_MESSAGE}"
: "${3:?$USAGE_MESSAGE}"
: "${4:?$USAGE_MESSAGE}"
: "${5:?$USAGE_MESSAGE}"
: "${DEBUG=}"

# Save the input parameters
CLUSTER_NAMESPACE=$1
SQL_MASTER_IP=$2
SQL_MASTER_SA_PASSWORD=$3
BACKUP_FILE_PATH=$4
KNOX_IP=$5
KNOX_PASSWORD=$6
# If Knox password is not supplied then default to SQL Master password
KNOX_PASSWORD=${KNOX_PASSWORD:=$SQL_MASTER_SA_PASSWORD}

SQL_MASTER_INSTANCE=$SQL_MASTER_IP,31433
KNOX_ENDPOINT=$KNOX_IP:30443

# Copy the backup file, restore the database, create necessary objects and data file
echo Copying database backup file...
pushd "$BACKUP_FILE_PATH"
$DEBUG kubectl cp ContosoAutoDW.bak master-0:/var/opt/mssql/data -c mssql-server -n $CLUSTER_NAMESPACE || (echo $ERROR_MESSAGE && sleep 15s)
popd

echo Configuring sample database...
# WSL ex: "/mnt/c/Program Files/Microsoft SQL Server/Client SDK/ODBC/130/Tools/Binn/SQLCMD.EXE"
$DEBUG sqlcmd -S $SQL_MASTER_INSTANCE -Usa -P $SQL_MASTER_SA_PASSWORD -i "bootstrap-sample-db-cadw.sql" -o "bootstrap-cadw.out" -I -b || (echo $ERROR_MESSAGE && exit 2)

# rm -f *.out *.err *.csv
echo Finished
sleep 15s
exit