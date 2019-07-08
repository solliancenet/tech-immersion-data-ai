# Automated Machine Learning setup

Complete the steps below to prepare the environment for the [Day 2, Experience 3](../../../day2-exp3/README.md) lab.

## Pre-requisites

- General: The experience runs within the free tier of Azure Notebooks and uses a cluster created within Azure by using Azure Machine Learning.
- In addition to their AAD account used to access Azure, attendees will need a Microsoft Account (e.g., Live ID/Passport) provided by the hosting environment to sign in to Azure Notebooks.
- Attendees will want ready access to their: subscription ID and resource group name.
- Need pre-created resource group `tech-immersion-onnx-xxxxx` and machine learning workspace `gpu-tech-immersion-aml-xxxxx`
- Within the workspace, need a pre-created machine learning compute named: `gpucluster`, Standard_DS3_v2, min nodes = 1 and max nodes = 1
- The AML cluster is created within the notebook, which requires 4 nodes, STANDARD_D12_V2
- An ACI instance is created within the notebook, which requires cpu_cores=1, and memory_gb=1
