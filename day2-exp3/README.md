# Day 2, Experience 3 - Quickly build comprehensive Bot solutions with the Virtual Assistant Solution Accelerator

Contoso Auto is interested in building bots to help automate certain tasks in a way that feels similar to dealing with a real person. The motivation for this is to add self-service capabilities in their automobiles that will help drivers and passengers interact with their vehicles in a more natural way, through voice-driven commands and simple visual feedback and prompts, without needing to involve a live person. They would like to build a proof of concept that will support their users through speech actions, such as controlling the vehicle's climate settings and radio. They would like to take this capability further by automatically monitoring car sensor data and alerting the driver when there is a potential problem with the vehicle, offering an incentive such as a gift card when the driver selects a recommended service and makes an appointment with a service center.

In this experience, you will use Microsoft's [Virtual Assistant Solution](https://docs.microsoft.com/azure/bot-service/bot-builder-virtual-assistant-introduction?view=azure-bot-service-4.0) accelerator to quickly develop this capability on top of foundational capabilities. By following the tasks below, you will register a new skill that monitors car sensor data and alerts the driver when there is a potential problem with the vehicle. Part of the process is to create an Adaptive Card to show vehicle data, recommendation for service (call out to function to get battery replacement prediction), and an option to contact the nearest service center. To entice the driver to service the car at that time, the bot will have them select a gift card of their choice that will give them a promo code for a coupon at that service center.

- [Day 2, Experience 3 - Quickly build comprehensive Bot solutions with the Virtual Assistant Solution Accelerator](#day-2-experience-3---quickly-build-comprehensive-bot-solutions-with-the-virtual-assistant-solution-accelerator)
  - [Experience requirements](#experience-requirements)
    - [Deployment](#deployment)
  - [Task 1: Load and explore the automotive Virtual Assistant starter solution](#task-1-load-and-explore-the-automotive-virtual-assistant-starter-solution)
  - [Task 2: Create and register a new skill and add it to the automotive Virtual Assistant](#task-2-create-and-register-a-new-skill-and-add-it-to-the-automotive-virtual-assistant)
  - [Task 3: Create Adaptive Card to show vehicle data, recommendation for service, and option to contact nearest service center](#task-3-create-adaptive-card-to-show-vehicle-data-recommendation-for-service-and-option-to-contact-nearest-service-center)
  - [Task 4: Configure skill to show promo code with gift card of their choice](#task-4-configure-skill-to-show-promo-code-with-gift-card-of-their-choice)

## Experience requirements

- Azure subscription
- Visual Studio 2017 Community (or better)
- Latest version of [.NET Core](https://www.microsoft.com/net/download)
- [Node.js](https://nodejs.org/) version 8.5 or higher
- Azure Bot Service command line (CLI) tools. It's important to do this even if you have earlier versions as the Virtual Assistant makes use of new deployment capabilities. **Minimum version 4.3.2 required for msbot, and minimum version 1.1.0 required for ludown.**
  ```shell
  npm install -g botdispatch chatdown ludown luis-apis luisgen msbot qnamaker
  ```
- [Azure Command Line Tools (CLI)](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli-windows?view=azure-cli-latest)
- Install the AZ Extension for Bot Service
  ```shell
  az extension add -n botservice
  ```
- Retrieve your LUIS Authoring Key:
  - Review the [LUIS regions](https://docs.microsoft.com/en-us/azure/cognitive-services/luis/luis-reference-regions) documentation page for the correct LUIS portal for the region you plan to deploy to. Note that www.luis.ai refers to the US region and an authoring key retrieved from this portal will not work with a Europe deployment.
  - Once signed in click on your name in the top right hand corner
  - Choose Settings and make a note of the Authoring Key for later steps

### Deployment

The Virtual Assistant requires the following dependencies for end to end operation:

- Azure Web App
- Azure Storage Account (Transcripts)
- Azure Application Insights (Telemetry)
- Azure CosmosDb (State)
- Azure Cognitive Services - Language Understanding
- Azure Cognitive Services - QnAMaker (including Azure Search, Azure Web App)

## Task 1: Load and explore the automotive Virtual Assistant starter solution

## Task 2: Create and register a new skill and add it to the automotive Virtual Assistant

## Task 3: Create Adaptive Card to show vehicle data, recommendation for service, and option to contact nearest service center

## Task 4: Configure skill to show promo code with gift card of their choice
