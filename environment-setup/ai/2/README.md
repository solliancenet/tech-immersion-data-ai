# Knowledge Mining with Cognitive Services setup

Complete the steps below to prepare the environment for the [Day 2, Experience 2](../../../day2-exp2/README.md) lab.

## Pre-requisites

The following services must be provisioned prior to the lab:

Each attendee will have a resource group containing the following resources:

- Cosmos DB (Core SQL API) (name: tech-immersionXXXXX)
- Azure Search (Basic tier) (name: tech-immersion)
- Cognitive Services (S0 Standard) (name: tech-immersion-cog-serv)
- Anomaly Detector (S0) (name: tech-immersion-anomaly-detector) - Only available in West US 2 or West Europe - In preview
- Form Recognizer (name: tech-immersion-form-recog) - In limited private preview
- Personalizer (S0) (name: tech-immersion-personalizer) - Only available in West US 2 or West Europe - In preview
- Translator Text (S1 Standard) (name: tech-immersion-translator)
- Azure Function App (Consumption Plan) (name: ti-function-day2)
  - Will also create an App Service Plan
- Azure Blob storage account (name: techimmersionstorageXXXXX)
  
## Lab computer pre-requisites

The computer or VM used for this experience requires the following:

- Visual Studio Community 2019 or higher

## Blob Storage

The sample forms inside the environment-setup/ai/2/sample-forms folder should be added into the Blob storage account above, inside a container named "forms".
