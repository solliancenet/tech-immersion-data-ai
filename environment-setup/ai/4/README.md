# Deep learning with ONNX setup

Complete the steps below to prepare the environment for the [Day 2, Experience 4](../../../day2-exp4/) lab.

## Pre-requisites

  - General: Assumes you host one Databricks Workspace per attendee, and that each attendee can spin up their own Azure Databricks cluster.
  - Azure Databricks Cluster configured per supplied notebook/script, which requires a GPU enabled cluster (1 node, Standard_NC12)  
  - glove.6b.100d.txt and connected-car_components.csv need to be uploaded to hosted Storage location (and notebook updated to suit)
  - Convert completed notebook (with all outputs) into ipynb for inclusion in PDF/Word doc.
  - Attendees will need their: subscription id, resource group name and region

## Cluster Setup
The cluster setup script is provided as a notebook:
[Cluster_Setup.dbc](./Cluster_Setup.dbc)