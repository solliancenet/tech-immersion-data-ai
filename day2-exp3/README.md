# Day 2, Experience 3 - Quickly build comprehensive Bot solutions with the Virtual Assistant Solution Accelerator

Contoso Auto is interested in building bots to help automate certain tasks in a way that feels similar to dealing with a real person. The motivation for this is to add self-service capabilities in their automobiles that will help drivers and passengers interact with their vehicles in a more natural way, through voice-driven commands and simple visual feedback and prompts, without needing to involve a live person. They would like to build a proof of concept that will support their users through speech actions, such as controlling the vehicle's climate settings and radio. They would like to take this capability further by automatically monitoring car sensor data and alerting the driver when there is a potential problem with the vehicle, offering an incentive such as a gift card when the driver selects a recommended service and makes an appointment with a service center.

In this experience, you will use Microsoft's [Virtual Assistant Solution](https://docs.microsoft.com/azure/bot-service/bot-builder-virtual-assistant-introduction?view=azure-bot-service-4.0) accelerator to quickly develop this capability on top of foundational capabilities. By following the tasks below, you will register a new skill that monitors car sensor data and alerts the driver when there is a potential problem with the vehicle. Part of the process is to create an Adaptive Card to show vehicle data, recommendation for service (call out to function to get battery replacement prediction), and an option to contact the nearest service center. To entice the driver to service the car at that time, the bot will have them select a gift card of their choice that will give them a promo code for a coupon at that service center.

- [Day 2, Experience 3 - Quickly build comprehensive Bot solutions with the Virtual Assistant Solution Accelerator](#day-2-experience-3---quickly-build-comprehensive-bot-solutions-with-the-virtual-assistant-solution-accelerator)
  - [Experience requirements](#experience-requirements)
  - [Task 1: Sign in to LUIS to retrieve the Authoring Key](#task-1-sign-in-to-luis-to-retrieve-the-authoring-key)
  - [Task 2: Register a new Azure App](#task-2-register-a-new-azure-app)
  - [Task 3: Deployment](#task-3-deployment)
  - [Task 4: Load and explore the automotive Virtual Assistant starter solution](#task-4-load-and-explore-the-automotive-virtual-assistant-starter-solution)
  - [Task 5: Open the generated bot file in the Bot Framework Emulator](#task-5-open-the-generated-bot-file-in-the-bot-framework-emulator)
  - [Task 6: Open LUIS to view the generated apps](#task-6-open-luis-to-view-the-generated-apps)
  - [Task 7: View Power BI report for bot statistics](#task-7-view-power-bi-report-for-bot-statistics)

## Experience requirements

- Azure subscription
- Visual Studio 2017 Community (or better)
- Latest version of [.NET Core](https://www.microsoft.com/net/download)
- [Node.js](https://nodejs.org/) version 8.5 or higher
- [Bot Framework Emulator](https://github.com/Microsoft/BotFramework-Emulator/releases/latest)
- Azure Bot Service command line (CLI) tools
- [Azure Command Line Tools (CLI)](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli-windows?view=azure-cli-latest) version **2.0.59** or later
- AZ Extension for Bot Service
- Retrieve your LUIS Authoring Key

## Task 1: Sign in to LUIS to retrieve the Authoring Key

The bot you will be creating uses [Microsoft's LUIS](https://azure.microsoft.com/services/cognitive-services/language-understanding-intelligent-service/), which is a natural language understanding service, to process and interpret user language to a set of actions or goals. One of the goals of building a bot is to have it interact with users in as human a way as possible. Understanding casual text or voice commands is an increasingly natural way to interact with bots and other virtual assistants. Before we can begin, we must first sign in to the LUIS website and obtain an authoring key that allows the service to be called by your bot.

1. If you will be running your bot services in the United States, navigate to [https://www.luis.ai/](www.luis.ai) and sign in with the Azure account you are using for this experience. If you are outside of the US, sign in to the LUIS site for your [region](https://docs.microsoft.com/en-us/azure/cognitive-services/luis/luis-reference-regions).

2. If it is your first time signing in, you may see a "Permission requested" dialog. If so, select **Accept**.

   ![Select Accept on the Permission requested dialog to continue.](media/luis-permission-requested.png 'Permission requested')

3. Again, if this is your first time signing in, you will be prompted to select your **Country/Region**. Select the appropriate option from the list, check the service agreement checkbox, then select **Continue**.

   ![The service agreement page has the Country/Region and the service agreement acceptance checkbox highlighted.](media/luis-accept.png 'Accept Service Agreement')

4. If you do not currently have any LUIS apps, you will see a page explaining what LUIS is and how it works. Within this page, select **Create a LUIS app now**.

   ![The LUIS welcome page.](media/luis-welcome-create.png 'Create a LUIS app now')

5. At this point, you will see a page called "My Apps". You do not need to create a new app. The Bot Framework SDK will do this for you. Click on your name in the top right-hand corner, then select **Settings**.

   ![Click on your name then select Settings.](media/luis-name-menu.png 'Settings')

6. Copy the **Authoring Key** and save it to Notepad or a similar text application for later steps.

   ![The User Settings page is displayed with the Authoring Key highlighted.](media/luis-authoring-key.png 'Authoring Key')

## Task 2: Register a new Azure App

The Bot Framework SDK uses what's called an Azure application for authentication and authorization between the published bot and its required Azure services. In this task, you will create a new app and retrieve its application Id that you will use when you run the bot creation script.

1. Navigate to <https://apps.dev.microsoft.com/> and sign in with your same Azure account you are using for this experience.

2. After signing in, you will see a page named "My applications". Select **Add an app**.

   ![The Add an app button is highlighted.](media/app-portal-applications.png 'My applications')

3. In the New Application Registration dialog, enter `tech-immersion-vehicle` into the **Name** field, then select **Create application**.

   ![The New Applicatino Registration dialog is displayed.](media/app-portal-new-application.png 'New Application Registration')

4. After a moment, your application will be created and its properties will be displayed. Select **Generate New Password** underneath the Application Secrets section.

   ![The Generate New Password button is highlighted.](media/app-portal-generate-password.png 'Application Secrets')

5. When the "New password generated" dialog appears, **copy and paste** your password to Notepad or similar text application for future reference. This will be the only time you see this password. Select **OK**.

   ![The new password is displayed.](media/app-portal-new-password.png 'New password generated')

6. **Copy and paste** the **Application Id** value to Notepad or a similar text application for later steps.

## Task 3: Deployment

The Virtual Assistant automotive bot requires the following Azure dependencies for end to end operation:

- Azure Web App (hosts the bot when published)
- Azure Storage Account (stores all chat transcripts)
- Azure Application Insights (captures bot and related services telemetry)
- Azure Cosmos DB (maintains the state of all conversations)
- Azure Cognitive Services - Language Understanding (LUIS)

In this task, you will deploy all of these Azure dependencies as well as configure and train [LUIS](https://docs.microsoft.com/en-us/azure/cognitive-services/luis/what-is-luis) with hundreds of text entries. Fortunately, we supply a script to do this for you.

1. On the bottom-left corner of your Windows desktop, locate the search box next to the Start Menu. Type **cmd**, then either press Enter or select the Command Prompt desktop app in the search results.

   ![The search box has "cmd" entered into it and the desktop app is highlighted in the results.](media/launch-cmd.png 'Launch Command Prompt')

2. Within the command prompt, type in the following, then hit `Enter`:

   `cd\`

3. Copy and paste the command below into the command prompt, then hit `Enter`. This will change directories to the Virtual Assistant automotive bot project folder:

   `cd C:\lab-files\bot\skills\automotiveskill\automotiveskill`

4. This next command requires two of your custom values you saved in the previous task when you registered your Azure App. **Copy** the command below and paste it into Notepad or other text editor. Replace `YOUR-APP-ID` with the Azure App's Application Id value, and replace `YOUR-APP-SECRET` with the App secret you copied. Make certain that your app secret value is surrounded by double quotes (").

   `PowerShell.exe -ExecutionPolicy Bypass -File DeploymentScripts\deploy_bot.ps1 --appId YOUR-APP-ID --% --appSecret "YOUR-APP-SECRET"`

5. Copy and paste your _edited_ command into the command prompt, then hit `Enter`. Supply the following values when prompted:

   - **name:** Enter a unique name that starts with `tech-immersion-bot-` and ends with your initials followed by a random 2-3 digit number. Example: `tech-immersion-bot-jdh77`. This value must be all lower case, contain no spaces and no special characters except for dashes (-) and underscores (\_).
   - **location:** Enter `westus`.
   - **luisAuthoringKey:** Paste the LUIS Authoring Key you copied at the end of Task 1 above.

   ![The bot creation script and parameters are highlighted.](media/cmd-bot-script.png 'Command Prompt')

6. This script will take around 10 minutes to run. **Important:** Keep the window open. There is a value you will need to copy once it is complete. For now, please move on to the next task.

## Task 4: Load and explore the automotive Virtual Assistant starter solution

## Task 5: Open the generated bot file in the Bot Framework Emulator

## Task 6: Open LUIS to view the generated apps

## Task 7: View Power BI report for bot statistics
