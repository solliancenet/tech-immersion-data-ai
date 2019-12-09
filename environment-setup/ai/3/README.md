# Automated Machine Learning setup

Complete the steps below to prepare the environment for the [AI, Experience 3](../../../ai-exp3/README.md) lab.

## Pre-requisites

- General: The experience runs within an Azure Machine Learning (AML) Notebook VM and uses an AML compute cluster.
- Attendees will want ready access to their: subscription ID and resource group name.
- Need pre-created resource group (e.g. `tech-immersion-xxxxx`) and machine learning workspace (e.g. `tech-immersion-aml-xxxxx`) (Workspace edition must be set to **Enterprise**)
- The AML compute cluster is created within the notebook, which requires a GPU enabled cluster (1 node, Standard_NC12)
- An ACI instance is created within the notebook, which requires cpu_cores=1, and memory_gb=1
