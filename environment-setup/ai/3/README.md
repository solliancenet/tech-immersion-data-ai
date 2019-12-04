# Automated Machine Learning setup

Complete the steps below to prepare the environment for the [AI, Experience 3](../../../ai-exp3/README.md) lab.

## Pre-requisites

- General: The experience runs within the free tier of Azure Notebooks and uses a cluster created within Azure by using Azure Machine Learning.
- In addition to their AAD account used to access Azure, attendees will need a Microsoft Account (e.g., Live ID/Passport) provided by the hosting environment to sign in to Azure Notebooks.
- Attendees will want ready access to their: subscription ID and resource group name.
- Need pre-created resource group `tech-immersion-xxxxx` and machine learning workspace `tech-immersion-aml-xxxxx` (Workspace edition must be set to **Enterprise**)
- Within the workspace, need a pre-created machine learning compute under the Training Clusters section, named: `aml-compute-cpu`, CPU type: **Standard_DS3_v2**, min nodes = 1 and max nodes = 1.
- An ACI instance is created within the notebook, which requires cpu_cores=1, and memory_gb=1
- A Notebook VM compute under the `tech-immersion-aml-xxxxx` workspace (VM type: **Standard_DS3**)
