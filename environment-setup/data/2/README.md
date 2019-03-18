# Cosmos DB real-time analytics setup

Complete the steps below to prepare the environment for the [Day 1, Experience 2](../../../day1-exp2/) lab.

## Pre-requisites

  - Each attendee needs a resource group with:
    - App Service
    - Cosmos DB
    - Stream Analytics
    - Azure Storage
    - Azure Function
    - Event Hub
  - Also, each attendee needs the AAD account they use to login to the portal, to be signed up for Power BI Pro trial.
  - Can we just have the transaction generator pre-compiled on the lab VM? What about the configuration it needs (since it will be different for each attendee)?

The computer or VM on which you run the scripts to deploy the cluster and restore the databases requires the following:

- Visual Studio 2017 Community (or better)

In addition, the users will require a Power BI account (sign up at <https://powerbi.microsoft.com>).

The following Azure Services must be provisioned prior to the lab:

- Azure Cosmos DB account
- Stream Analytics job
- Event Hubs Namespace
- Azure Function App (consumption plan, .NET runtime)
