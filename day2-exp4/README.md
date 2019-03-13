# Day 2, Experience 4 - Making deep learning portable with ONNX
In this experience you will learn how Contoso Auto can leverage Deep Learning technologies to scan through their vehicle specification documents to find compliance issues with new regulations. Then they will deploy this model, standardizing operationalization with ONNX. You will see how this simplifies inference runtime code, enabling pluggability of different models and targeting a broad range of runtime environments from Linux based web services to Windows/.NET based apps.

- [Day 2, Experience 4 - Leveraging Cosmos DB for near real-time analytics](#day-2-experience-4---leveraging-cosmos-db-for-near-real-time-analytics)
  - [Task 1:Train and deploy a deep learning model](#task-1-train-and-deploy-a-deep-learning-model )


## Task 1: Train and deploy a deep learning model 

In this task, you will train a deep learning model to classify the descriptions of car components provided by technicians as compliant or non-compliant, convert it to ONNX, and deploy it as a web service. To accomplish this, you will use an Azure Databricks notebook to explore the transaction and account data. 

1. From the Azure Portal, navigate to your deployed Azure Databricks workspace and select **Launch Workspace**.
2. Within the Workspace, using the command bar on the left, select **Workspace**, **Users** and select your username (the entry with house icon).
3. In the blade that appears, select the downwards pointing chevron next to your name, and select **Import**.
4. On the Import Notebooks dialog, select URL and paste in the following URL (copy the URL from the link below):

    [Deep_Learning.dbc](./Deep_Learning.dbc)


5. Select **Import**.
6. A folder named after the archive should appear. Select that folder.
7. The folder will contain a notebook. This is the notebooks you will use in completing this lab. Follow the instructions in the notebook, and then return to this guide to complete the experience.