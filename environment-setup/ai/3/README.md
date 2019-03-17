# Bot framework setup

Complete the steps below to prepare the environment for the [Day 2, Experience 3](../../../day2-exp3/) lab.

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

Lab attendees will run a script to deploy required Azure resources during the lab, using the Azure account provided to them.
