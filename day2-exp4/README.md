# Day 2, Experience 4 - Making deep learning portable with ONNX

- [Day 2, Experience 4 - Making deep learning portable with ONNX](#day-2-experience-4---making-deep-learning-portable-with-onnx)
  - [Technology overview](#technology-overview)
  - [Scenario overview](#scenario-overview)
  - [Task 1: Train and deploy a deep learning model](#task-1-train-and-deploy-a-deep-learning-model)
  - [Wrap-up](#wrap-up)
  - [Additional resources and more information](#additional-resources-and-more-information)

## Technology overview

Using the [main Python SDK](https://docs.microsoft.com/python/api/overview/azure/ml/intro?view=azure-ml-py) and the [Data Prep SDK](https://docs.microsoft.com/python/api/overview/azure/dataprep/intro?view=azure-dataprep-py) for Azure Machine Learning as well as open-source Python packages, you can build and train highly accurate machine learning and deep-learning models yourself in an Azure Machine Learning service Workspace. You can choose from many machine learning components available in open-source Python packages, such as the following examples:

- [Scikit-learn](https://scikit-learn.org/stable/)
- [Tensorflow](https://www.tensorflow.org/)
- [PyTorch](https://pytorch.org/)
- [CNTK](https://www.microsoft.com/en-us/cognitive-toolkit/)
- [MXNet](https://mxnet.incubator.apache.org/)

After you have a model, you use it to create a container, such as Docker, that can be deployed locally for testing. After testing is done, you can deploy the model as a production web service in either Azure Container Instances or Azure Kubernetes Service. For more information, see the article on [how to deploy and where](https://docs.microsoft.com/azure/machine-learning/service/how-to-deploy-and-where).

Then you can manage your deployed models by using the [Azure Machine Learning SDK for Python](https://docs.microsoft.com/python/api/overview/azure/ml/intro?view=azure-ml-py) or the [Azure portal](https://portal.azure.com). You can evaluate model metrics, retrain, and redeploy new versions of the model, all while tracking the model's experiments.

For deep neural network (DNN) training using TensorFlow, Azure Machine Learning provides a custom TensorFlow class of the Estimator. The Azure SDK's [TensorFlow estimator](https://docs.microsoft.com/python/api/azureml-train-core/azureml.train.dnn.tensorflow?view=azure-ml-py) (not to be conflated with the tf.estimator.Estimator class) enables you to easily submit TensorFlow training jobs for both single-node and distributed runs on Azure compute.

The TensorFlow Estimator also enables you to train your models at scale across CPU and GPU clusters of Azure VMs. You can easily run distributed TensorFlow training with a few API calls, while Azure Machine Learning will manage behind the scenes all the infrastructure and orchestration needed to carry out these workloads.

Azure Machine Learning supports two methods of distributed training in TensorFlow:

- MPI-based distributed training using the [Horovod](https://github.com/horovod/horovod) framework
- Native [distributed TensorFlow](https://github.com/tensorflow/examples/blob/master/community/en/docs/deploy/distributed.md) via the parameter server method

The [Open Neural Network Exchange](https://onnx.ai/) (ONNX) format is an open standard for representing machine learning models. ONNX is supported by a [community of partners](https://onnx.ai/supported-tools), including Microsoft, who create compatible frameworks and tools. Microsoft is committed to open and interoperable AI so that data scientists and developers can:

- Use the framework of their choice to create and train models
- Deploy models cross-platform with minimal integration work

Microsoft supports ONNX across its products including Azure and Windows to help you achieve these goals.

The interoperability you get with ONNX makes it possible to get great ideas into production faster. With ONNX, data scientists can choose their preferred framework for the job. Similarly, developers can spend less time getting models ready for production, and deploy across the cloud and edge.

You can create ONNX models from many frameworks, including PyTorch, Chainer, Microsoft Cognitive Toolkit (CNTK), MXNet, ML.Net, TensorFlow, Keras, SciKit-Learn, and more.
There is also an ecosystem of tools for visualizing and accelerating ONNX models. A number of pre-trained ONNX models are also available for common scenarios.
[ONNX models can be deployed](https://docs.microsoft.com/azure/machine-learning/service/how-to-build-deploy-onnx#deploy) to the cloud using Azure Machine Learning and ONNX Runtime. They can also be deployed to Windows 10 devices using [Windows ML](https://docs.microsoft.com/windows/ai/). They can even be deployed to other platforms using converters that are available from the ONNX community.

## Scenario overview

In this experience you will learn how Contoso Auto can leverage Deep Learning technologies to scan through their vehicle specification documents to find compliance issues with new regulations. Then they will deploy this model, standardizing operationalization with ONNX. You will see how this simplifies inference runtime code, enabling pluggability of different models and targeting a broad range of runtime environments from Linux based web services to Windows/.NET based apps.

## Task 1: Train and deploy a deep learning model

In this task, you will train a deep learning model to classify the descriptions of car components provided by technicians as compliant or non-compliant, convert it to ONNX, and deploy it as a web service. To accomplish this, you will use an Azure Databricks notebook to explore the transaction and account data. 

1. From the Azure Portal, navigate to your deployed Azure Databricks workspace and select **Launch Workspace**.

    ![Launch Workspace](media/01.png 'Launch Workspace')

2. Within the Workspace, using the command bar on the left, select **Workspace**, **Users** and select your username (the entry with house icon).

    ![Selecting Workspace, Users](media/02.png 'Selecting Workspace, Users')

3. In the blade that appears, select the downwards pointing chevron next to your username, and select **Import**.

    ![Select Import](media/03.png 'Select Import')

4. On the Import Notebooks dialog, select **browse** and then select `Deep_Learning.dbc` from your lab files folder for this experience (C:\lab-files\ai\4) and select **Import**.

    ![Import the notebook](media/04.png 'Import Notebooks')

5. The notebook should appear in the list. Double-click this notebook to open it. This is the notebook you will use in completing this lab. Follow the instructions in the notebook, and then return to this guide to complete the experience.

## Wrap-up

Congratulations on completing the deep learning with ONNX experience. In this experience you completed an end-to-end process for training a deep learning model, converting it to ONNX and deploying the model into production, all from within an Azure Databricks notebook.

To recap, you experienced:

1. Using [Keras](https://keras.io/) to create and train a deep learning model for classifying text data on a GPU enabled cluster provided by Azure Databricks.
2. Converting the Keras model to an ONNX model.
3. Using the ONNX model to classify text data.
4. Creating and deploying a web service to [Azure Container Instances](https://docs.microsoft.com/azure/container-instances/) that uses the ONNX model for classification.

## Additional resources and more information

To learn more about the Azure Machine Learning service, visit the [documentation](https://docs.microsoft.com/azure/machine-learning/service)

To learn more about Azure Databricks, visit the [documentation](https://docs.microsoft.com/azure/azure-databricks/)