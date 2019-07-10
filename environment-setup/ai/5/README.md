# Deep learning with ONNX setup

Complete the steps below to prepare the environment for the [AI, Experience 5](../../../ai-exp5/README.md) lab.

## Pre-requisites

- General: The experience runs within the free tier of Azure Notebooks and uses a cluster created within Azure by using Azure Machine Learning.
- In addition to their AAD account used to access Azure, attendees will need a Microsoft Account (e.g., Live ID/Passport) provided by the hosting environment to sign in to Azure Notebooks.
- Attendees will want ready access to their: subscription ID and resource group name.
- Need pre-created resource group `tech-immersion-onnx-xxxxx` and machine learning workspace `gpu-tech-immersion-aml-xxxxx`
- The AML cluster is created within the notebook, which requires a GPU enabled cluster (1 node, Standard_NC12)
- An ACI instance is created within the notebook, which requires cpu_cores=1, and memory_gb=1
