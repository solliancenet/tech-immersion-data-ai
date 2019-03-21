# Bot framework setup

Complete the steps below to prepare the environment for the [Day 2, Experience 1](../../../day2-exp1/README.md) lab.

## Pre-requisites

The computer or VM on which you run the scripts to deploy the cluster and restore the databases requires the following:

- Visual Studio 2017 Community (or better)
- Latest version of [.NET Core](https://www.microsoft.com/net/download)
- [Node.js](https://nodejs.org/) version 8.5 or higher
- [Bot Framework Emulator](https://github.com/Microsoft/BotFramework-Emulator/releases/latest)
- Azure Bot Service command line (CLI) tools. It's important to do this even if you have earlier versions as the Virtual Assistant makes use of new deployment capabilities. **Minimum version 4.3.2 required for msbot, and minimum version 1.1.0 required for ludown.**

  ```shell
  npm install -g botdispatch chatdown ludown luis-apis luisgen msbot qnamaker
  ```

- [Azure Command Line Tools (CLI)](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli-windows?view=azure-cli-latest) version **2.0.59** or later
- Install the AZ Extension for Bot Service

  ```shell
  az extension add -n botservice
  ```

### Azure resources

Users will need to be able to sign into [LUIS](https://www.luis.ai/) with their Azure Active Directory account and create a new LUIS app.

Users will also need to be able to register a new Azure App (<https://apps.dev.microsoft.com/>) with their Azure Active Directory account. If they do not have permissions to do this step, then an Azure App will need to be created for them and they will need to be supplied with both the **Application Id** and **generated password** (application secret) for the application. The steps to do this can be found [here](../../../day2-exp3#task-2-register-a-new-azure-app).

Lab attendees will run a script to deploy required Azure resources during the lab, using the Azure account provided to them.