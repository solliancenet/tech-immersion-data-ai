# Data & AI Tech Immersion Workshop – Product Review Guide and Lab Instructions

## Day 2, Experience 4 - Creating repeatable processes with Azure Machine Learning pipelines

- [Data & AI Tech Immersion Workshop – Product Review Guide and Lab Instructions](#data--ai-tech-immersion-workshop-%E2%80%93-product-review-guide-and-lab-instructions)
  - [Day 2, Experience 4 - Creating repeatable processes with Azure Machine Learning pipelines](#day-2-experience-4---creating-repeatable-processes-with-azure-machine-learning-pipelines)
- [Technology overview](#technology-overview)
  - [What are machine learning pipelines?](#what-are-machine-learning-pipelines)
  - [Scenario Overview](#scenario-overview)
  - [Task 1: Create the Azure Notebooks project](#task-1-create-the-azure-notebooks-project)
  - [Task 2: Upload the starter notebook](#task-2-upload-the-starter-notebook)
  - [Wrap-up](#wrap-up)
  - [Additional resources and more information](#additional-resources-and-more-information)


# Technology overview

## What are machine learning pipelines?
Using [Azure Machine Learning SDK for Python](https://docs.microsoft.com/en-us/python/api/azureml-pipeline-core/?view=azure-ml-py), data scientists, data engineers, and IT professionals can collaborate on the steps involved in:
* Data preparation, such as normalization and transformations
* Model training
* Model evaluation
* Deployment

The following diagram shows an example pipeline:

![azure machine learning pipelines](./media/pipelines.png)

## Scenario Overview
In this experience, you will learn how Contoso Auto can benefit from creating re-usable machine learning pipelines with Azure Machine Learning.

The goal is to build a pipeline that demonstrates the basic data science workflow of data preparation, model training, and predictions. Azure Machine Learning allows you to define distinct steps and make it possible to re-use these pipelines as well as to rerun only the steps you need as you tweak and test your workflow.

In this experience, you will be using a subset of data collected from Contoso Auto's fleet management program used by a fleet of taxis. The data is enriched with holiday and weather data. The goal is to train a regression model to predict taxi fares in New York City based on input features such as, number of passengers, trip distance, datetime, holiday information and weather information.

The machine learning pipeline in this quickstart is organized into three steps:

- **Preprocess Training and Input Data:** We want to preprocess the data to better represent the datetime features, such as hours of the day, and day of the week to capture the cyclical nature of these features.

- **Model Training:** We will use data transformations and the GradientBoostingRegressor algorithm from the scikit-learn library to train a regression model. The trained model will be saved for later making predictions.

- **Model Inference:** In this scenario, we want to support **bulk predictions**. Each time an input file is uploaded to a data store, we want to initiate bulk model predictions on the input data. However, before we do model predictions, we will re-use the preprocess data step to process input data, and then make predictions on the processed data using the trained model.

Each of these pipelines is going to have implicit data dependencies and the example will demonstrate how AML make it possible to re-use previously completed steps and run only the modified or new steps in your pipeline.

The pipelines will be run on the Azure Machine Learning compute.

## Task 1: Create the Azure Notebooks project

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

## Task 2: Upload the starter notebook

1. Navigate to your `connected-car` project in your Azure Notebook environment. 

2. Select the **Upload** menu and then choose **From URL**.

    ![The Upload menu](media/05.png 'Upload')

7. In the Upload files from URL dialog, copy and paste the following URL into the `File URL`.

    https://github.com/solliancenet/tech-immersion-data-ai/blob/master/lab-files/ai/4/pipelines-AML.ipynb
   
    Then select **Done** to upload and dismiss the dialog.

    ![The Upload files from Computer dialog](media/06.png 'Upload files from Computer')

8. In the listing, select the Notebook you just uploaded (pipelines-AML.ipynb) to open it.

9. Follow the instructions within the notebook to complete the experience.

## Wrap-up

Congratulations on completing the experience ...

To recap, you experienced:

1. Using [Keras](https://keras.io/) to create and train a deep learning model for classifying text data on a GPU enabled cluster provided by Azure Machine Learning.
2. Converting the Keras model to an ONNX model.
3. Using the ONNX model to classify text data.
4. Creating and deploying a web service to [Azure Container Instances](https://docs.microsoft.com/azure/container-instances/) that uses the ONNX model for classification.

## Additional resources and more information

To learn more about the Azure Machine Learning service, visit the [documentation](https://docs.microsoft.com/azure/machine-learning/service)
