# Data & AI Tech Immersion Workshop – Product Review Guide and Lab Instructions

## AI, Experience 3 - Better models made easy with Automated Machine Learning

- [Data & AI Tech Immersion Workshop – Product Review Guide and Lab Instructions](#Data--AI-Tech-Immersion-Workshop-%E2%80%93-Product-Review-Guide-and-Lab-Instructions)
  - [AI, Experience 3 - Better models made easy with Automated Machine Learning](#AI-Experience-3---Better-models-made-easy-with-Automated-Machine-Learning)
  - [Technology overview](#Technology-overview)
  - [Scenario overview](#Scenario-overview)
  - [Exercise 1: Creating a model using automated machine learning](#Exercise-1-Creating-a-model-using-automated-machine-learning)
    - [Task 1: Create an automated machine learning experiment using the Portal](#Task-1-Create-an-automated-machine-learning-experiment-using-the-Portal)
    - [Task 2: Review the experiment run results](#Task-2-Review-the-experiment-run-results)
    - [Task 3: Register the Best Model](#Task-3-Register-the-Best-Model)
  - [Exercise 2: Understanding the automated ML generated model using model explainability](#Exercise-2-Understanding-the-automated-ML-generated-model-using-model-explainability)
    - [Task 1: Create the Azure Notebooks project](#Task-1-Create-the-Azure-Notebooks-project)
    - [Task 2: Upload the starter notebook](#Task-2-Upload-the-starter-notebook)
  - [Exercise 3 (Optional): Train and evaluate a model using Azure Machine Learning](#Exercise-3-Optional-Train-and-evaluate-a-model-using-Azure-Machine-Learning)
    - [Task 1: Upload and open the starter notebook](#Task-1-Upload-and-open-the-starter-notebook)
  - [Wrap-up](#Wrap-up)
  - [Additional resources and more information](#Additional-resources-and-more-information)

## Technology overview

Azure Machine Learning service provides a cloud-based environment you can use to prep data, train, test, deploy, manage, and track machine learning models.

![Azure Machine Learning overview](media/intro.png 'Azure Machine Learning overview')

Azure Machine Learning service fully supports open-source technologies. So you can use tens of thousands of open-source Python packages with machine learning components. Examples are PyTorch, TensorFlow, and scikit-learn. Support for rich tools makes it easy to interactively explore and prepare data and then develop and test models. Examples are [Jupyter notebooks](https://jupyter.org) or the [Azure Machine Learning for Visual Studio Code](https://marketplace.visualstudio.com/items/itemName/ms-toolsai.vscode-ai/overview) extension.

By using Azure Machine Learning service, you can start training on your local machine and then scale out to the cloud. With many available [compute targets](https://docs.microsoft.com%/en-us/azure/machine-learning/service/how-to-set-up-training-targets), like Azure Machine Learning Compute and [Azure Databricks](https://docs.microsoft.com/en-us/azure/azure-databricks/what-is-azure-databricks), and with [advanced hyperparameter tuning services](https://docs.microsoft.com/en-us/azure/machine-learning/service/how-to-tune-hyperparameters), you can build better models faster by using the power of the cloud. When you have the right model, you can easily deploy it in a container such as Docker. So it's simple to deploy to Azure Container Instances or Azure Kubernetes Service. Or you can use the container in your own deployments, either on-premises or in the cloud. For more information, see the article on [how to deploy and where](https://docs.microsoft.com/en-us/azure/machine-learning/service/how-to-deploy-and-where).

You can manage the deployed models and track multiple runs as you experiment to find the best solution. After it's deployed, your model can return predictions in [real time](https://docs.microsoft.com/en-us/azure/machine-learning/service/how-to-consume-web-service) or [asynchronously](https://docs.microsoft.com/en-us/azure/machine-learning/service/how-to-run-batch-predictions) on large quantities of data. And with advanced [machine learning pipelines](https://docs.microsoft.com/en-us/azure/machine-learning/service/concept-ml-pipelines), you can collaborate on all the steps of data preparation, model training and evaluation, and deployment.

Azure Machine Learning service also includes features that [automate model generation and tuning](https://docs.microsoft.com/en-us/azure/machine-learning/service/tutorial-auto-train-models) to help you create models with ease, efficiency, and accuracy. Automated machine learning is the process of taking training data with a defined target feature, and iterating through combinations of algorithms and feature selections to automatically select the best model for your data based on the training scores. The traditional machine learning model development process is highly resource-intensive, and requires significant domain knowledge and time investment to run and compare the results of dozens of models. Automated machine learning simplifies this process by generating models tuned from the goals and constraints you defined for your experiment, such as the time for the experiment to run or which models to blacklist.

## Scenario overview

In this experience you will learn how the automated machine learning capability in Azure Machine Learning (AML) can be used for the life cycle management of the manufactured vehicles and how AML helps in creation of better vehicle maintenance plans. To accomplish this, you will train a Linear Regression model to predict the number of days until battery failure using Automated Machine Learning in Jupyter Notebooks.

## Exercise 1: Creating a model using automated machine learning

In this exercise, you will create a model that predicts battery failure from time-series data using the visual interface to automated machine learning in an Azure Machine Learning workspace.

### Task 1: Create an automated machine learning experiment using the Portal

1. Navigate to your Azure Machine Learning workspace in the Azure Portal.
2. Select `Automated machine learning` in the left navigation bar.
3. Select **Create Experiment**.

   ![Create new experiment](./media/02_CreateExperiment.png)

4. Provide the experiment name: `automl-regression` and from the drop-down select **gpucluster** and then select **Next**.

   ![Provide experiment name and click on compute](./media/03_NewExperiment_1.png)

5) Now you will upload the training data. Start by downloading the data from https://quickstartsws9073123377.blob.core.windows.net/azureml-blobstore-0d1c4218-a5f9-418b-bf55-902b65277b85/training-formatted.csv. Select **Upload** and then in the dialog that appears choose the file `training-formatted.csv` from your local disk.

   ![Uploading the training data](./media/05_UploadDataFile.png)

6) Select **training-formatted.csv** and review the training data. Be sure to scroll to the right to observe all of the columns in the data.

   ![Reviewing the training data](./media/06_ReviewDataFile.png)

7) Now, you will setup the Auto ML Experiment Basic Settings by providing the following values:

   - Prediction Task: select **Regression**
   - Target column: select **Survival_In_Days**

     ![Setup Auto ML experiment basic settings](./media/07_SetupExp_1.png)

8) Select **Advanced Settings** to expand that area. Provide the following settings:

   - Primary metric: **normalized_root-mean_squared_error**
   - Max number of iterations: **3**
   - Number of Cross Validations: **5**
   - Max concurrent iterations: **1**

     ![Configuring the Advanced Settings as described](./media/08_SetupExp_2.png)

9) Select **Start** to run the experiment and begin automated machine learning process.

### Task 2: Review the experiment run results

1. The experiment will run for about _10 minutes_. Once it completes you should examine the chart to observe the model performance for the primary metric for different iterations.

   ![Review run details - graph view](./media/09_ReviewRunDetails_1.png)

2. Scroll down to see a table view of different iterations and select the iteration with the best **normalized root mean square error** score. Note that the normalized root mean square error measures the error between the predicted value and actual value. In this case, the model with the lowest normalized root mean square error is the best model.

   ![Review run details - table view](./media/010_ReviewRunDetails_2.png)

### Task 3: Register the Best Model

1. Return to the top of the `Run Details` screen and select **Deploy Best Model** as shown. Note that deployment consists of four steps: (1) _Register Best Model_, (2) Download _Scoring and Environment Script files_, (3) Create _Deployment Image_ using the downloaded script files, and (4) Deploy _Scoring Web Service_ using the created image.

   ![The Deploy Best Model button](./media/014_DeployBestModel.png)

2. You register the best model with the Azure Machine Learning model registry so that you can retrieve it later when you want to use it for scoring. Select **Register Model** link. Once the registration process has completed, the link will change to the text `Model has been registered`.

   ![Register Best Model](./media/015_RegisterModel.png)

3. The model registration, will create a new model in your Azure Machine Learning workspace with the same name as the experiment: `automl-regression`. To view this model from the Azure Machine Learning workspace, select **Models**.

   ![Viewing the list of models in the Azure Machine Learning workspace](media/03-automl-registered-model.png)

4. If you see your model in the above list, you are now ready to continue on to the next exercise.

## Exercise 2: Understanding the automated ML generated model using model explainability

### Task 1: Create the Azure Notebooks project

To complete this task, you will use an Azure Notebook and Azure Machine Learning.

If you have not already created the `connected-car` project in Azure Notebooks follow these steps. If you already have this project in your environment, continue with the **Task 2**.

1. To start, open a new web browser window and navigate to <https://notebooks.azure.com>.

2. Select **Sign In** and then use your Microsoft Account to complete the sign in process.

   ![The Sign In button](media/01.png 'Sign In')

3. Dismiss the dialog to create the user ID (you will not need this). Within the Microsoft Azure Notebooks portal, select **My Projects** from the menu at the top.

   ![The My Projects button](media/02.png 'My Projects')

4. Then select **New Project**.

   ![The New Project button](media/03.png 'New Project')

5. On the Create New Project dialog, provide a Project Name (this should be a user friendly description) and Project ID (this will form a part of the URL used to access this project in the browser) and uncheck Public. Select **Create**.

   ![The Create New Project dialog](media/04.png 'Create New Project')

### Task 2: Upload the starter notebook

1. Navigate to your `connected-car` project in your Azure Notebook environment.

2. Select the **Upload** menu and then choose **From URL**.

   ![The Upload menu](media/05.png 'Upload')

3. In the Upload files from URL dialog, copy and paste the following URL into the `File URL`.

   https://github.com/solliancenet/tech-immersion-data-ai/blob/master/lab-files/ai/3/scoring-from-automl-model.ipynb

   Then select **Done** to upload and dismiss the dialog.

   ![The Upload files from Computer dialog](media/06.png 'Upload files from Computer')

4. In the listing, select the Notebook you just uploaded (scoring-from-automl-model.ipynb) to open it.

5. Follow the instructions within the notebook to complete the experience.

## Exercise 3 (Optional): Train and evaluate a model using Azure Machine Learning

### Task 1: Upload and open the starter notebook

In this exercise, you will use compute resources provided by Azure Machine Learning to remotely train a set of models using Automated Machine Learning, evaluate performance of each model and pick the best performing model to deploy as a web service. You will perform this lab using Azure Notebooks. The model you train here is created using automated machine learning just as you did in exercise 1, except instead of using the visual interface in the Azure Portal you setup the model training using Python.

1. Navigate to your `connected-car` project in your Azure Notebook environment.

2. Select the **Upload** menu and then choose **From URL**.

   ![The Upload menu](media/05.png 'Upload')

3. In the Upload files from URL dialog, copy and paste the following URL into the `File URL`.

   https://github.com/solliancenet/tech-immersion-data-ai/blob/master/lab-files/ai/3/predict-battery-life-with-AML.ipynb

   Then select **Done** to upload and dismiss the dialog.

   ![The Upload files from Computer dialog](media/06.png 'Upload files from Computer')

4. In the listing, select the Notebook you just uploaded (predict-battery-life-with-AML.ipynb) to open it.

5. Follow the instructions within the notebook to complete the experience.

## Wrap-up

Congratulations on completing the Auto ML experience.

To recap, you experienced:

1. How to use automated machine learning from the Azure Machine Learning workspace to simplify the process of getting to a performant model.
2. Using Auto ML to train multiple models by using remote capabilities provided by compute targets.
3. Capturing and querying the telemetry of training runs using an Experiment.
4. Retrieving the best model created from an Auto ML session.
5. Registering the best model with the Model Registry, which enables versioning and makes the model file available for deployment to a web service.
6. Understanding the model using the model interpretability features of the Azure Machine Learning Python SDK.

## Additional resources and more information

To learn more about the Azure Machine Learning service, visit the [documentation](https://docs.microsoft.com/azure/machine-learning/service)
