# Data & AI Tech Immersion Workshop – Product Review Guide and Lab Instructions

## AI, Experience 6 - MLOps with Azure Machine Learning and Azure DevOps

- [Data &amp; AI Tech Immersion Workshop – Product Review Guide and Lab Instructions](#data-amp-ai-tech-immersion-workshop-%e2%80%93-product-review-guide-and-lab-instructions)
  - [AI, Experience 6 - MLOps with Azure Machine Learning and Azure DevOps](#ai-experience-6---mlops-with-azure-machine-learning-and-azure-devops)
  - [Technology overview](#technology-overview)
  - [Scenario overview](#scenario-overview)
  - [Exercise 1: Setup New Project in Azure DevOps](#exercise-1-setup-new-project-in-azure-devops)
    - [Task 1: Create New Project](#task-1-create-new-project)
    - [Task 2: Import Quickstart code from a GitHub Repo](#task-2-import-quickstart-code-from-a-github-repo)
    - [Task 3: Update the build YAML file](#task-3-update-the-build-yaml-file)
    - [Task 4: Create new Service Connection](#task-4-create-new-service-connection)
  - [Exercise 2: Setup and Run the Build Pipeline](#exercise-2-setup-and-run-the-build-pipeline)
    - [Task 1: Setup Build Pipeline](#task-1-setup-build-pipeline)
    - [Task 2: Run the Build Pipeline](#task-2-run-the-build-pipeline)
    - [Task 3: Review Build Artifacts](#task-3-review-build-artifacts)
    - [Task 4: Review Build Outputs](#task-4-review-build-outputs)
  - [Exercise 3: Setup the Release Pipeline](#exercise-3-setup-the-release-pipeline)
    - [Task 1: Create an Empty Job](#task-1-create-an-empty-job)
    - [Task 2: Add Build Artifact](#task-2-add-build-artifact)
    - [Task 3: Add Variables to Deploy &amp; Test stage](#task-3-add-variables-to-deploy-amp-test-stage)
    - [Task 4: Setup Agent Pool for Deploy &amp; Test stage](#task-4-setup-agent-pool-for-deploy-amp-test-stage)
    - [Task 5: Add Use Python Version task](#task-5-add-use-python-version-task)
    - [Task 6: Add Install Requirements task](#task-6-add-install-requirements-task)
    - [Task 7: Add Deploy &amp; Test Webservice task](#task-7-add-deploy-amp-test-webservice-task)
    - [Task 8: Define Deployment Trigger](#task-8-define-deployment-trigger)
    - [Task 9: Enable Continuous Deployment Trigger](#task-9-enable-continuous-deployment-trigger)
    - [Task 10: Save the Release Pipeline](#task-10-save-the-release-pipeline)
  - [Exercise 4: Test Build and Release Pipelines](#exercise-4-test-build-and-release-pipelines)
    - [Task 1: Make Edits to Source Code](#task-1-make-edits-to-source-code)
    - [Task 2: Monitor Build Pipeline](#task-2-monitor-build-pipeline)
    - [Task 3: Monitor Release Pipeline](#task-3-monitor-release-pipeline)
    - [Task 4: Review Release Pipeline Outputs](#task-4-review-release-pipeline-outputs)
  - [Wrap-up](#wrap-up)
  - [Additional resources and more information](#additional-resources-and-more-information)

## Technology overview

Azure Machine Learning uses a Machine Learning Operations (MLOps) approach, which improves the quality and consistency of your machine learning solutions. Azure Machine Learning Service provides the following MLOps capabilities:

- Integration with Azure Pipelines. Define continuous integration and deployment workflows for your models.
- A model registry that maintains multiple versions of your trained models.
- Model validation. Automatically validate your trained models and select the optimal configuration for deploying them into production.
- Deploy your models as a web service in the cloud, locally, or to IoT Edge devices.
- Monitor your deployed model's performance, so you can drive improvements in the next version of the model.

## Scenario overview

In this experience you will learn how Contoso Auto can use MLOps to formalize the process of training and deploying new models using a DevOps approach.

## Exercise 1: Setup New Project in Azure DevOps

Duration: 20 minutes

### Task 1: Create New Project

1. Sign in to [Azure DevOps](http://dev.azure.com).

2. Select **New project**.

    ![Create new project in Azure DevOPs.](media/devops-project-01.png 'Create new project')

3. Provide Project Name: `mlops-quickstart` and select **Create**.

    ![Provide project name in the create new project dialog and then select create.](media/devops-project-02.png 'Create New Project Dialog')

### Task 2: Import Quickstart code from a GitHub Repo

1. Within the new project:

   a. Select **Repos** from left navigation bar.

   b. Select **Import** from the content section.

    ![Import Quickstart code from a GitHub Repo.](media/devops-project-03.png 'Azure DevOps Repos')

2. Provide the following GitHub URL: `https://github.com/solliancenet/mcw-mlops-starter` and select **Import**. This should import the code required for the quickstart.

    ![Provide the above GitHub URL and select import to import the source code.](media/devops-project-04.png 'Import a Git repository dialog')
    
    *Note that if you receive an error while importing the repository, please disable the preview feature `New Repos landing pages` and import the GitHub repository from the old UI, as shown in steps #3, #4, and #5 below.*

3. [Optional] Select **Account settings, Preview features**.

    ![The image shows how to navigate to the list of preview features.](media/preview_features-01.png 'Preview features')

4. [Optional] From the list of preview features, disable the preview feature **New Repos landing pages**.

   ![The image shows a list of preview features and highlights the preview feature, New Repos landing pages.](media/preview_features-02.png 'Disable New Repos landing pages')

5. [Optional] Repeat Step #1 above to import the GitHub repository from the old UI.

### Task 3: Update the build YAML file

1. Select and open the `azure-pipelines.yml` file.

2. Select **Edit** and update the following variables: `resourcegroup`, and `workspace`. If you are using your own Azure subscription, please provide names to use. If an environment is provided to you, be sure to replace XXXXX in the values below with your unique identifier. Typically, for the provided environment, the format for the resourcegroup is `tech_immersion_XXXXX` and for workspace is `tech_immersion_aml_XXXXX` where XXXXX is your unique identifier.

    ![Edit build YAML file and provide your resource group and workspace information.](media/devops-build-pipeline-01.png 'Edit Build YAML file')

3. Select **Commit** to save your changes.

    ![Commit your changes to the build YAML file.](media/devops-build-pipeline-02.png 'Commit Build YAML file')
  
### Task 4: Create new Service Connection

1. From the left navigation select **Project settings** and then select **Service connections**.

    ![Navigate to Project Settings, Service connections section.](media/devops-build-pipeline-03.png 'Service Connections')

2. Select **Create service connection**, select **Azure Resource Manager**, and then select **Next**.

    ![Select Create Service Connection, Azure Resource Manager.](media/devops-build-pipeline-04.png 'Azure Resource Manager')

3. If an environment is provided to you **goto step #5**. Select **Service principal (automatic)** and then select **Next**.

    ![Select Service principal (automatic), and then select Next.](media/devops-build-pipeline-05.png 'Service principal authentication')

4. Provide the following information in the `New Azure service connection` dialog box and then select **Save**:

    a. Scope Level: `Machine Learning Workspace`

    b. Subscription: Select the Azure subscription to use.

    > **Note**: It might take up to 30 seconds for the **Subscription** dropdown to be populated with available subscriptions, depending on the number of different subscriptions your account has access to.

    c. Resource group: This value should match the value you provided in the `azure-pipelines.yml` file. (Typically, for the provided environment, the format for the resourcegroup is `tech_immersion_XXXXX`.)

    d. Machine Learning Workspace: This value should match the value you provided in the `azure-pipelines.yml` file. (Typically, for the provided environment, the format for the workspace is `tech_immersion_aml_XXXXX`.)

    e. Service connection name: `quick-starts-sc`

    f. Grant access permission to all pipelines: this checkbox must be selected.

    ![Provide connection name, Azure Resource Group, Machine Learning Workspace, and then select Save. The resource group and machine learning workspace must match the value you provided in the YAML file.](media/devops-build-pipeline-06.png 'Add an Azure Resource Manager service connection dialog')

     >**Note**: If you are unable to select your **Machine Learning Workspace**, do the following steps:

    - Quit the `New Azure service connection` dialog
    - Refresh or reload the web browser
    - Repeat steps 1-3 above
    - In step 4, change the `Scope level` to **Subscription** and then select your **Resource group**
    - Please remember to name your service connection as `quick-starts-sc`
    - Grant access permission to all pipelines

    >**Note**: If you successfully created the new service connection **goto Exercise 2**.

5. Select **Service principal (manual)** and then select **Next**.

    ![Select Service principal (manual), and then select Next.](media/sc_01.png 'Service principal authentication')

6. Provide the following information in the `New Azure service connection` dialog box and then select **Verify and save**:

    1. Scope Level: `Subscription`

    1. Subscription id: (Lab environment details page: Service Principal Details->Subscription Id)

    1. Subscription Name: You can find the subscription name from [Azure Portal](https://portal.azure.com)

    1. Service principal Id: (Lab environment details page: Service Principal Details->Application/Client Id)

    1. Service principal key: (Lab environment details page: Service Principal Details->Application Secret Key)

    1. Tenant ID: (Lab environment details page: Service Principal Details->Tenant Id)

    1. Service connection name: `quick-starts-sc`

    1. Grant access permission to all pipelines: this checkbox must be selected.

    ![Provide information as shown in the dialog.](media/sc_02.png 'Add an Azure Resource Manager service connection dialog')

## Exercise 2: Setup and Run the Build Pipeline

Duration: 25 minutes

> **Note**: This exercise requires the new version of the **Pipelines** user interface. To activate it, select **Pipelines** from the left navigation. If the first option below **Pipelines** is **Builds**, you are still running on the previous version of the user interface. In this case, a popup should appear suggesting the activation of the new user interface.
>
> ![Activate the new Azure Pipelines user interface.](media/devops-ui-activation.png 'Multi-stage pipelines activation')
>
> Select **Try it!** to activate the new Pipelines user interface. When successfully activated, the first option below **Pipelines** from the left navigation will change to **Pipelines**.

### Task 1: Setup Build Pipeline

1. From left navigation select **Pipelines, Pipelines** and then select **Create pipeline**.

    ![Navigate to Pipelines, Pipelines, and then select Create pipeline](media/devops-build-pipeline-07.png 'Create Build Pipeline')

2. Select **Azure Repos Git** as your code repository.

    ![Select your code repository source for your new build pipeline.](media/devops-build-pipeline-08.png 'Repository Source')

3. Select **mlops-quickstart** as your repository.

    ![Select mlops-quickstart as your repository.](media/devops-build-pipeline-09.png 'Select Repository')

4. Review the YAML file.

    The build pipeline has four key steps:

    a. Attach folder to workspace and experiment. This command creates the `.azureml` subdirectory that contains a `config.json` file that is used to communicate with your Azure Machine Learning workspace. All subsequent steps rely on the `config.json` file to instantiate the workspace object.

    b. Create the AML Compute target to run your master pipeline for model training and model evaluation.

    c. Run the master pipeline. The master pipeline has two steps: (1) Train the machine learning model, and (2) Evaluate the trained machine learning model. The evaluation step evaluates if the new model performance is better than the currently deployed model. If the new model performance is improved, the evaluate step will create a new Image for deployment. The results of the evaluation step will be saved in a file called `eval_info.json` that will be made available for the release pipeline. You can review the code for the master pipeline and its steps in `aml_service/pipelines_master.py`,  `scripts/train.py`, and `scripts/evaluate.py`.

    d. Publish the build artifacts. The `snapshot of the repository`, `config.json`, and `eval_info.json` files are published as build artifacts and thus can be made available for the release pipeline.

    ![Review the build pipeline YAML file.](media/devops-build-pipeline-10.png 'Build pipeline YAML')

### Task 2: Run the Build Pipeline

1. Select **Run** to start running your build pipeline.

    ![Start the run for your build pipeline.](media/devops-build-pipeline-11.png 'Run Build Pipeline')

2. Monitor the build run. The build pipeline, for the first run, will take around 20 minutes to run.

    ![Monitor your build pipeline. It will take around 20 minutes to run.](media/devops-build-pipeline-12.png 'Monitor Build Pipeline')

3. Select **Job** to monitor the detailed status of the build pipeline execution.

    ![Monitor the details of your build pipeline.](media/devops-build-pipeline-13.png 'Monitor Build Pipeline Details')

### Task 3: Review Build Artifacts

1. The build will publish an artifact named `devops-for-ai`. Select **Artifacts, 1 published** to review the artifact contents.

    ![Select Artifacts, 1 published to review the artifact contents.](media/devops-build-pipeline-14.png 'Build Artifacts')

2. Select **outputs, eval_info.json** and  then select **ellipsis** and click on **Download artifacts** . The `eval_info.json` is the output from the *model evaluation* step and the information from the evaluation step will be later used in the release pipeline to deploy the model. Select the back arrow to return to the previous screen.

    ![Download output from the model evaluation step.](media/ai6artifacts.png 'Download JSON file')

3. Open the `eval_info.json` in a json viewer or a text editor and observe the information. The json output contains information such as if the model passed the evaluation step (`deploy_model`: *true or false*) and evaluation accuracy.

    ![Review information in the eval_info json file.](media/devops-build-pipeline-16.png 'Eval Info JSON File')

### Task 4: Review Build Outputs

1. Log in to [Azure Machine Learning studio](https://ml.azure.com) either directly or via the [Azure Portal](https://portal.azure.com). Make sure you select the Azure Machine Learning workspace that you created from the notebook earlier. Open your **Models** section, and observe the versions of the registered model: `compliance-classifier`. The latest version is the one registered by the build pipeline you have run in the previous task.

    ![Review registered model in Azure Machine Learning studio.](media/ai-20.png 'Registered Models in Azure Machine Learning studio')

2. Select the latest version of your model to review its properties. Notice the ```build_number``` tag which links the registered to model to the Azure DevOps build that generated it.

    ![Review registered model properties, notice Build_Number tag.](!media/../media/ai-21.png 'Registered model details and Build_Number tag')

3. Open your **Datasets** section and observe the versions of the registered dataset: ```connected_car_components```. The latest version is the one registered by the build pipeline you have run in the previous task.

    ![Review registered dataset in Azure Machine Learning studio.](media/ai-22.png 'Registered Datasets in Azure Machine Learning studio')

4. Select the latest version of your dataset to review its properties. Notice the ```build_number``` tag that links the dataset version to the Azure DevOps build that generated it.

    ![Review registered dataset version properties, notice Build_Number tag.](medial/../media/devops-build-outputs-04.png 'Registered dataset details in Azure Machine Learning studio')

5. Select **Models** to view a list of registered models that reference the dataset.

    ![Review list of registered models that reference dataset in Azure Machine Learning studio.](media/devops-build-outputs-05.png 'Registered dataset model references in Azure Machine Learning studio')

## Exercise 3: Setup the Release Pipeline

Duration: 20 minutes

### Task 1: Create an Empty Job

1. Return to Azure DevOps and navigate to **Pipelines, Releases** and select **New pipeline**.

    ![To create new Release Pipeline navigate to Pipelines, Releases and select New pipeline.](media/devops-release-pipeline-01.png 'New Release Pipeline')

2. Select **Empty job**.

    ![Select empty job to start building the release pipeline.](media/devops-release-pipeline-02.png 'Select a template: Empty job')

3. Provide Stage name: `Deploy & Test` and close the dialog.

    ![Provide stage name for the release stage.](media/devops-release-pipeline-03.png 'Deploy & Test Stage')

### Task 2: Add Build Artifact

1. Select **Add an artifact**.

    ![Add a new artifact to the release pipeline.](media/devops-release-pipeline-04.png 'Add an artifact')

2. Select Source type: `Build`, Source (build pipeline): `mlops-quickstart`. *Observe the note that shows that the mlops-quickstart publishes the build artifact named devops-for-ai*. Finally, select **Add**.

    ![Provide information to add the build artifact.](media/devops-release-pipeline-05.png 'Add a build artifact')

### Task 3: Add Variables to Deploy & Test stage

1. Open **View stage tasks** link.

    ![Open view stage tasks link.](media/devops-release-pipeline-06.png 'View Stage Tasks')

2. Open the **Variables** tab.

    ![Open variables tab.](media/devops-release-pipeline-07.png 'Release Pipeline Variables')

3. Add four Pipeline variables as name - value pairs and then select **Save** (use the default values in the **Save** dialog):

    a. Name: `aks_name` Value: `aks-cluster01`

    b. Name: `aks_region` Value: should be the same region as the region of your Azure Machine Learning workspace (e.g. `eastus`)

    c. Name: `service_name` Value: `compliance-classifier-service`

    d. Name: `description` Value: `"Compliance Classifier Web Service"` *Note the double quotes around description value*.

    >**Note**:
    >- Keep the scope for the variables to `Deploy & Test` stage.
    >- The name of the Azure region should be the same one that was used to create Azure Machine Learning workspace earlier on.

      ![Add four pipeline variables as name value pairs and save.](media/devops-release-pipeline-08.png 'Add Pipeline Variables')

### Task 4: Setup Agent Pool for Deploy & Test stage

1. Open the **Tasks** tab.

    ![Open view stage tasks link.](media/devops-release-pipeline-09.png 'Pipeline Tasks')

2. Select **Agent job** and change **Agent pool** to `Azure Pipelines` and change **Agent Specification** to `ubuntu-16.04`.

    ![Change Agent pool to be Hosted Ubuntu 1604.](media/devops-release-pipeline-10.png 'Agent Job Setup')

### Task 5: Add Use Python Version task

1. Select **Add a task to Agent job** (the **+** button), search for `Use Python Version`, and select **Add**.

    ![Add Use Python Version task to Agent job.](media/devops-release-pipeline-11.png 'Add Use Python Version Task')

2. Provide **Display name:** `Use Python 3.6` and **Version spec:** `3.6`.

    ![Provide Display name and Version spec for the Use Python version task.](media/devops-release-pipeline-12.png 'Use Python Version Task Dialog')

### Task 6: Add Install Requirements task

1. Select **Add a task to Agent job** (the **+** button), search for `Bash`, and select **Add**.

    ![Add Bash task to Agent job.](media/devops-release-pipeline-13.png 'Add Bash Task')

2. Provide **Display name:** `Install Requirements` and select **object browser ...** to provide **Script Path**.

    ![Provide Display name for the Bash task.](media/devops-release-pipeline-14.png 'Bash Task Dialog')

3. Navigate to **Linked artifacts/_mlops-quickstart (Build)/devops-for-ai/environment_setup** and select **install_requirements.sh**.

    ![Provide Script Path to the Install Requirements bash file.](media/devops-release-pipeline-15.png 'Select Path Dialog')

4. Expand **Advanced** and select **object browser ...** to provide **Working Directory**.

    ![Expand advanced section to provide Working Directory.](media/devops-release-pipeline-16.png 'Bash Task - Advanced Section')

5. Navigate to **Linked artifacts/_mlops-quickstart (Build)/devops-for-ai** and select **environment_setup**.

    ![Provide path to the Working Directory.](media/devops-release-pipeline-17.png 'Select Path Dialog')

### Task 7: Add Deploy & Test Webservice task

1. Select **Add a task to Agent job** (the **+** button), search for `Azure CLI`, and select **Add**.

    ![Add Azure CLI task to Agent job.](media/devops-release-pipeline-18.png 'Azure CLI Task')

2. Provide the following information for the Azure CLI task:

    a. **Task version**: `1.*`

    b. **Display name**: `Deploy and Test Webservice`

    c. **Azure subscription**: `quick-starts-sc`

    > **Note**: This is the service connection we created in Exercise 1 / Task 4.

    d. **Script Location**: `Inline script`

    e. **Inline Script**: `python aml_service/deploy.py --service_name $(service_name) --aks_name $(aks_name) --aks_region $(aks_region) --description $(description)`

    ![Setup the Azure CLI task using the information above.](media/devops-release-pipeline-19.png 'Azure CLI Task Dialog')

3. Expand **Advanced** and provide **Working Directory:** `$(System.DefaultWorkingDirectory)/_mlops-quickstart/devops-for-ai`.

    ![Provide Working Directory for the Azure CLI task.](media/devops-release-pipeline-20.png 'Azure CLI Task - Working Directory')

In a separate browser tab, navigate to the **Repo** section in Azure DevOps and please review the code in `aml_service/deploy.py`. This step will read the `eval_info.json` and if the evaluation step recommended to deploy the new trained model, it will deploy the new model to production in an **Azure Kubernetes Service (AKS)** cluster.

### Task 8: Define Deployment Trigger

1. Navigate to **Pipeline** tab, and select **Pre-deployment conditions** for the `Deploy & Test` stage.

2. Select **After release**.

    ![Setup Pre-deployment conditions for the Deploy & Test stage.](media/devops-release-pipeline-21.png 'Pre-deployment Conditions Dialog')

3. Close the dialog.

### Task 9: Enable Continuous Deployment Trigger

1. Select **Continuous deployment trigger** for `_mlops-quickstart` artifact.

2. Enable: **Creates a release every time a new build is available**.

    ![Enable Continuous Deployment Trigger for the Release pipeline.](media/devops-release-pipeline-22.png 'Continuous Deployment Trigger Dialog')

3. Close the dialog

### Task 10: Save the Release Pipeline

1. Provide name: `mlops-quickstart-release`.

2. Select: **Save** (use the default values in the **Save** dialog).

    ![Provide name for the release pipeline and select save.](media/devops-release-pipeline-23.png 'Save')

## Exercise 4: Test Build and Release Pipelines

Duration: 30 minutes

### Task 1: Make Edits to Source Code

1. Navigate to: **Repos -> Files -> scripts -> `train.py`**.

2. **Edit** `train.py`.

3. Change the **learning rate (lr)** for the optimizer from **0.1** to **0.001**.

4. Change the number of training **epochs** from **3** to **5**.

5. Select **Commit**.

    ![Make edits to train.py by changing the learning rate. Select Commit after editing.](media/devops-test-pipelines-01.png 'Edit Train.py')

6. Provide comment: `Improving model performance: changed learning rate.` and select **Commit**.

    ![Provide commit comment for train.py.](media/devops-test-pipelines-02.png 'Commit - Comment')

### Task 2: Monitor Build Pipeline

1. Navigate to **Pipelines, Pipelines**. Observe that the CI build is triggered because of the source code change.

   ![Navigate to Pipelines, Builds.](media/devops-test-pipelines-03.png 'Pipelines - pipelines')

2. Select the pipeline run and monitor the pipeline steps. The pipeline will run for 16-18 minutes. Proceed to the next task when the build pipeline successfully completes.

   ![Monitor Build Pipeline. It will take around 15 minutes to complete.](media/devops-test-pipelines-04.png 'Build Pipeline Steps')

### Task 3: Monitor Release Pipeline

1. Navigate to **Pipelines, Releases**. Observe that the Release pipeline is automatically trigger upon successful completion of the build pipeline. Select as shown in the figure to view pipeline logs.

   ![Navigate to Pipelines, Releases and Select as shown in the figure to view pipeline logs.](media/devops-test-pipelines-05.png 'Pipelines - Releases')

2. The release pipeline will run for about 15 minutes. Proceed to the next task when the release pipeline successfully completes.

### Task 4: Review Release Pipeline Outputs

1. From the pipeline logs view, select **Deploy & Test Webservice** task to view details.

    ![Select Deploy & Test Webservice task to view details.](media/devops-test-pipelines-06.png 'Pipeline Logs')

2. Observe the **Scoring URI** and **API Key** for the deployed webservice. Please note down both the `Scoring URI` and `API Key` for *Exercise 7*.

    ![View Deploy & Test Webservice task logs and note down the Scoring URI of the deployed webservice.](media/ai-26.png 'Deploy & Test Webservice Task Logs')

3. Log in to Azure Machine Learning studio. Open your **Endpoints** section, and observe the deployed webservice: **compliance-classifier-service**.

    ![View deployed webservice in Azure Machine Learning studio.](media/ai-27.png 'Azure Machine Learning studio - Workspace, Deployments')

## Wrap-up

Congratulations on completing this experience.

To recap, you experienced:

1. Creating a new project in Azure DevOps.

2. Creating a Build Pipeline to support model training.

3. Creating a Release Pipeline to support model deployment.

## Additional resources and more information

To learn more about MLOps with the Azure Machine Learning service, visit the [documentation](https://docs.microsoft.com/en-us/azure/machine-learning/service/concept-model-management-and-deployment)
