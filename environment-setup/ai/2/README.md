# Knowledge Mining with Cognitive Services setup

Complete the steps below to prepare the environment for the [AI, Experience 2](../../../ai-exp2/README.md) lab.

## Pre-requisites

The following services must be provisioned prior to the lab:

Each attendee will have a resource group containing the following resources:

- Cosmos DB (Core SQL API)
  - Name: tech-immersionXXXXX
- Azure Search
  - Basic tier
  - Name: tech-immersion
- Cognitive Services
  - S0 Standard
  - Name: tech-immersion-cog-services
- Anomaly Detector
  - S0
  - Name: tech-immersion-anomaly-detector
  - Only available in West US 2 or West Europe - In preview
- Form Recognizer
  - Name: tech-immersion-form-recog
  - In limited private preview
  - Only available in West US 2 or West Europe regions
- Translator Text
  - S1 Standard
  - Name: tech-immersion-translator
- Azure Function App
  - Consumption Plan
  - Name: ti-function-day2
- Azure Blob storage account
  - Name: techimmersionstoreXXXXX
  - Must be in the same region as the Form Recognizer service.

## Lab computer pre-requisites

The computer or VM used for this experience requires the following:

- Visual Studio Community 2019 or higher

## Blob Storage

The sample forms inside the environment-setup/ai/2/sample-forms folder should be added into the Blob storage account above, inside a container named "forms".
