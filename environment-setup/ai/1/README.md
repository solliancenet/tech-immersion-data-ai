# Automated Machine Learning setup

Complete the steps below to prepare the environment for the [Day 2, Experience 1](../../../day2-exp1/) lab.

## Pre-requisites

  - In addition to their AAD account used to access Azure, attendees will need a Microsoft Account (e.g., Live ID/Passport) provided by the hosting environment to sign in to Azure Notebooks.
  - Attendees will want ready access to their: subscription ID and resource group name. 
  - A shared AKS Cluster that attendees can attach their AML workspace to.
    - Kubernetes Cluster Name: my-aks-cluster  (must be provided to attendees, default used is a shown)
    - Resource Group Name: ti-aks (must be provided to attendees, default used is a shown)
    - VM Size: STANDARD_D3_V2 (total of at least 12 cores)
    - Kubernetes Version: 1.11.8
    - Node Count: 3 
  - Resource group should contain:
    - AML Workspace
    - AML Compute Cluster (tied to AML Workspace). This will be created dynamically by each attendee within the lab as their is no way for them to attach to a pre-created instance.
  - Prepare ipynb (with all outputs) for inclusion in PDF/Word doc.