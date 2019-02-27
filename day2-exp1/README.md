# Day 2, Experience 1 - Better models made easy with Automated Machine Learning

In this experience you will learn how the automated machine learning capability in Azure Machine Learning (AML) can be used for the life cycle management of the manufactured vehicles and how AML helps in creation of better vehicle maintenance plans. To accomplish this, you will train a Linear Regression model to predict the number of days until battery failure using Automated Machine Learning in Jupyter Notebooks.

- [Day 2, Experience 1 - Leveraging Cosmos DB for near real-time analytics](#day-1-experience-2---leveraging-cosmos-db-for-near-real-time-analytics)
  - [Task 1:Train and evaluate a model using Azure Machine Learning](#exercise-1-configure-cosmos-db)


## Task 1: Train and evaluate a model using Azure Machine Learning

In this exercise, you will use compute resources provided by Azure Machine Learning to remotely train a set of models using Automated Machine Learning, evaluate performance of each model and pick the best performing model to deploy as a web service. You will perform this lab using Azure Notebooks.

1.  To start, open a new web browser window and navigate to <https://notebooks.azure.com>. 

2.  Select **Sign In** and then use your Microsoft Account to complete the sign in process.

    ![The Sign In button](media/01.png 'Sign In')

3.  Within the Microsoft Azure Notebooks portal, select **My Projects** from the menu at the top. 

    ![The My Projects button](media/02.png 'My Projects')

4.  Then select **New Project**.

    ![The New Project button](media/03.png 'New Project')

5.  On the Create New Project dialog, provide a Project Name (this should be a user friendly description) and Project ID (this will form a part of the URL used to access this project in the browser) and uncheck Public. Select **Create**.

    ![The Create New Project dialog](media/04.png 'Create New Project')

6.  Select the **Upload** menu and then choose **From URL**.

    ![The Upload menu](media/05.png 'Upload')

7. In the Upload files from URL dialog, for the File URL copy and paste in the following URL and then press TAB to leave the File URL field: 

[predict-battery-life-with-AML.ipynb](./predict-battery-life-with-AML.ipynb)

8.  The File Name field will be auto-populated with the name of the notebook.  Select **Done** to upload the notebook.

    ![The Upload files from URL dialog](media/06.png 'Upload files from URL')

9.  In the listing, select the Notebook you just uploaded (predict-battery-life-with-AML.ipynb) to open it. 

10.  Follow the instructions within the notebook to complete the experience.