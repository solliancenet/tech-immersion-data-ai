#
# Prerequisites: 
# 
# Azure CLI (https://docs.microsoft.com/en-us/cli/azure/install-azure-cli), python3 (https://www.python.org/downloads), mssqlctl CLI (pip3 install -r  https://private-repo.microsoft.com/python/ctp3.0/mssqlctl/requirements.txt )
#
# Run `az login` at least once BEFORE running this script
#

from subprocess import check_output, CalledProcessError, STDOUT, Popen, PIPE
import os
import getpass

def executeCmd (cmd):
    if os.name=="nt":
        process = Popen(cmd.split(),stdin=PIPE, shell=True)
    else:
        process = Popen(cmd.split(),stdin=PIPE)
    stdout, stderr = process.communicate()
    if (stderr is not None):
        raise Exception(stderr)

#
# MUST INPUT THESE VALUES!!!!!
#
SUBSCRIPTION_ID = input("Provide your Azure subscription ID:")
GROUP_NAME = input("Provide Azure resource group name to be created:")
DOCKER_USERNAME = input("Provide your Docker username:")
DOCKER_PASSWORD  = getpass.getpass("Provide your Docker password:")

#
# Optionally change these configuration settings
#
AZURE_REGION=input("Provide Azure region - Press ENTER for using `westus`:") or "westus"
VM_SIZE=input("Provide VM size for the AKS cluster - Press ENTER for using  `Standard_L8s`:") or "Standard_L8s"
AKS_NODE_COUNT=input("Provide number of worker nodes for AKS cluster - Press ENTER for using  `1`:") or "1"

#This is both Kubernetes cluster name and SQL Big Data cluster name
CLUSTER_NAME=input("Provide name of AKS cluster and SQL big data cluster - Press ENTER for using  `sqlbigdata`:") or "sqlbigdata"

#This password will be use for Controller user, Knox user and SQL Server Master SA accounts
PASSWORD=input("Provide password to be used for Controller user, Knox user and SQL Server Master SA accounts - Press ENTER for using  `MySQLBigData2019`:") or "MySQLBigData2019"
CONTROLLER_USERNAME=input("Provide username to be used for Controller user - Press ENTER for using  `admin`:") or "admin"

#
DOCKER_REGISTRY="private-repo.microsoft.com"
DOCKER_REPOSITORY="mssql-private-preview"
DOCKER_IMAGE_TAG="ctp3.0"

print ('Setting environment variables')
os.environ['MSSQL_SA_PASSWORD'] = PASSWORD
os.environ['CONTROLLER_USERNAME'] = CONTROLLER_USERNAME
os.environ['CONTROLLER_PASSWORD'] = PASSWORD
os.environ['KNOX_PASSWORD'] = PASSWORD
os.environ['DOCKER_USERNAME']=DOCKER_USERNAME
os.environ['DOCKER_PASSWORD']=DOCKER_PASSWORD
os.environ['DOCKER_IMAGE_POLICY']="IfNotPresent"
os.environ['ACCEPT_EULA']="Yes"
os.environ['STORAGE_SIZE']="60Gi"

print ("Set azure context to subcription: "+SUBSCRIPTION_ID)
command = "az account set -s "+ SUBSCRIPTION_ID
executeCmd (command)

print ("Creating azure resource group: "+GROUP_NAME)
command="az group create --name "+GROUP_NAME+" --location "+AZURE_REGION
executeCmd (command)

print("Creating AKS cluster: "+CLUSTER_NAME)
command = "az aks create --name "+CLUSTER_NAME+" --resource-group "+GROUP_NAME+" --generate-ssh-keys --node-vm-size "+VM_SIZE+" --node-count "+AKS_NODE_COUNT+" --kubernetes-version 1.12.8"
executeCmd (command)

command = "az aks get-credentials --overwrite-existing --name "+CLUSTER_NAME+" --resource-group "+GROUP_NAME+" --admin"
executeCmd (command)

print("Creating SQL Big Data cluster:" +CLUSTER_NAME)
command="mssqlctl cluster config init --src aks-dev-test.json --target custom.json --force"
executeCmd (command)

command="mssqlctl cluster config section set -c custom.json -j ""metadata.name=" + CLUSTER_NAME + ""
executeCmd (command)

command="mssqlctl cluster config section set -c custom.json -j ""$.spec.controlPlane.spec.docker.registry=" + DOCKER_REGISTRY + ""
executeCmd (command)
command="mssqlctl cluster config section set -c custom.json -j ""$.spec.controlPlane.spec.docker.repository=" + DOCKER_REPOSITORY + ""
executeCmd (command)
command="mssqlctl cluster config section set -c custom.json -j ""$.spec.controlPlane.spec.docker.imageTag=" + DOCKER_IMAGE_TAG + ""
executeCmd (command)
command="mssqlctl cluster config section set -c custom.json -j ""$.spec.controlPlane.spec.storage.data.size=60Gi""
executeCmd (command)

command="mssqlctl cluster create -c custom.json --accept-eula yes"
executeCmd (command)

print("")
print("SQL Server big data cluster connection endpoints: ")

print("SQL Server master instance:")
command="kubectl get service master-svc-external -o=custom-columns=""IP:.status.loadBalancer.ingress[0].ip,PORT:.spec.ports[0].port"" -n "+CLUSTER_NAME
executeCmd(command)

print("")
print("HDFS/KNOX:")
command="kubectl get service gateway-svc-external -o=custom-columns=""IP:status.loadBalancer.ingress[0].ip,PORT:.spec.ports[0].port"" -n "+CLUSTER_NAME
executeCmd(command)

print("")
print("Cluster administration portal (https://<ip>:<port>):")
command="kubectl get service mgmtproxy-svc-external -o=custom-columns=""IP:status.loadBalancer.ingress[0].ip,PORT:.spec.ports[0].port"" -n "+CLUSTER_NAME
executeCmd(command)

print("")