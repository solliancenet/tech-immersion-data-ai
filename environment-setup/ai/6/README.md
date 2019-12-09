# MLOps with Azure Machine Learning and Azure DevOps

Complete the steps below to prepare the environment for the [AI, Experience 6](../../../ai-exp6/README.md) lab.

## Pre-requisites

- General: The experience runs within an Azure Machine Learning (AML) Notebook VM and uses an AML compute cluster.
- Attendees will want ready access to their: subscription ID and resource group name.
- Need pre-created resource group (e.g. `tech_immersion_XXXXX`) and machine learning workspace (e.g. `tech_immersion_aml_XXXXX`).
- Need permission to create Service Principal in the tenant. This translates to `Ensure that the user has 'Owner' or 'User Access Administrator' permissions on the Subscription`.
- The AML compute cluster is created within Azure DevOps / Azure CLI, which requires 1 node, STANDARD_D2_V2.
- An ACI instance is created within Azure DevOps / Azure CLI, which requires cpu_cores=1, and memory_gb=1.
