# Tech Immersion Mega Data & AI Workshop

## Setup: Connecting to your VM with Remote Desktop
	When setting up a cloud infrastructure, it's a good idea to create a VM where you can install all of the tools and applications you need to manage your environment and products.  This is called a jumpbox.  In order to use your jumpbox, you need to open Remote Desktop Connection.
	
	You will need three pieces of information:
	- Jump VM DNS Name
	- VM Admin User Name
	- VM Admin Password 
	
	Start Remote Desktop and enter the Jump DNS Name as the computer name.  When you are asked to enter your credentials, click "More Choices".  Click "Use a different account".

	In the user name box, enter your VM Admin User Name.  Enter your VM Admin Password as the password.  Click Connect.  When the warning comes up, click "Yes."  You are now connected to your jumpbox and can continue with each individual Experience.

## Day 1: Data-focused

- **Day 1, Experience 1** - [Handling Big Data with SQL Server 2019 Big Data Clusters](./day1-exp1/README.md)

  Highlight the new features of SQL Server 2019 with a focus on Big Data Clusters and data virtualization. Attendees will gain hands-on experience with querying both structured and unstructured data in a unified way using T-SQL. This capability will be illustrated by joining different data sets, such as product stock data in flat CSV files in Azure Storage, product reviews stored in Azure SQL Database, and transactional data in SQL Server 2019 for exploratory data analysis within Azure Data Studio. This joined data will be prepared into a table used for reporting, highlighting query performance against this table due to intelligent query processing. With the inclusion of Apache Spark packaged with Big Data Clusters, it is now possible to use Spark to train machine learning models over data lakes and use those models in SQL Server in one system. Attendees will learn how to use Azure Data Studio to work with Jupyter notebooks to train a simple model that can predict vehicle battery lifetime, train a simple model that can predict vehicle battery lifetime, score new data and save the result as an external table. Finally, attendees will experience the data security and compliance features provided by SQL Server 2019 by using the Data Discovery & Classification tool in SSMS to identify tables and columns with PII and GDPR-related compliance issues, then address the issues by layering on dynamic data masking to identified columns.

- **Day 1, Experience 2** - [Leveraging Cosmos DB for near real-time analytics](./day1-exp2/README.md)

  In this experience, attendees will use Azure Cosmos DB to ingest streaming vehicle telemetry data as the entry point to a near real-time analytics pipeline built on Cosmos DB, Azure Functions, Event Hubs, Azure Stream Analytics, and Power BI. To start, attendees will complete performance-tuning on Cosmos DB to prepare it for data ingest, and use the change feed capability of Cosmos DB to trigger Azure Functions for data processing. The function will enrich the telemetry data with location information, then send it to Event Hubs. Azure Stream Analytics extracts the enriched sensor data from Event Hubs, performs aggregations over windows of time, then sends the aggregated data to Power BI for data visualization and analysis. A vehicle telemetry data generator will be used to send vehicle telemetry data to Cosmos DB.

- **Day 1, Experience 3** - [Unlocking new capabilities with friction-free migrations to Azure SQL Managed Instance](./day1-exp3/README.md)

  Show how databases previously prevented from using PaaS services can be migrated to SQL MI and take advantage of features only available in Azure. Migrate an on-premises parts catalog database, currently running on SQL Server 2012 and using Service Broker, to SQL MI. Create an online secondary database for reporting on operations and finance using SQL MI, using transactional replication.

- **Day 1, Experience 4** - [Delivering the Modern Data Warehouse with Azure SQL Data Warehouse, Azure Databricks, Azure Data Factory, and Power BI](./day1-exp4/README.md)

  A modern data warehouse lets you bring together all your data at any scale easily, and to get insights through analytical dashboards, operational reports, or advanced analytics for all your users. In this experience we will demonstrate how to use Azure Data Factory (ADF) to automate the movement and transformation of data gathered from various sources, including Cosmos DB, into Azure Data Lake Storage Gen2, Azure Databricks and Azure SQL DW to build a modern data warehouse.

## Day 2: AI & Machine Learning-focused

- **Day 2, Experience 1** - [Quickly build comprehensive Bot solutions with the Virtual Assistant Solution Accelerator](./day2-exp1/README.md)

  Show how the Virtual Assistant Solution accelerator can rapidly accelerate developing conversation bots. This exercise will use the automotive Virtual Assistant starter solution, which converts the user’s speech to actions, such as controlling the vehicle’s climate settings and radio. Attendees will register a new skill that monitors car sensor data and alerts the driver when there is a potential problem with the vehicle. Part of the process is to create an Adaptive Card to show vehicle data, recommendation for service (call out to function to get battery replacement prediction), and an option to contact the nearest service center. To entice the driver to service the car at that time, the bot will have them select a gift card of their choice that will give them a promo code for a coupon at that service center.

- **Day 2, Experience 2** - [Yield quick insights from unstructured data with Knowledge Mining and Cognitive Services](./day2-exp2/README.md)

  Show how building a cognitive search pipeline using Cognitive Services and Knowledge Mining can yield quick insights on unstructured data. Cognitive search is an AI feature in Azure Search, used to extract text from images, blobs, and other unstructured data sources - enriching the content to make it more searchable in an Azure Search index. Attendees will create a cognitive search pipeline in Azure Search, using Cosmos DB as a data source, and applying pre-configured and custom cognitive skills to enrich the data in the indexing pipeline.

- **Day 2, Experience 3** - [Better models made easy with Automated Machine Learning](./day2-exp3/README.md)

  Show how automated ML capability in Azure Machine Learning can be used for Life Cycle Management of the manufactured vehicles and how AML helps in creation of better vehicle maintenance plans. Attendees will train a Linear Regression model to predict the number of days until battery failure using Automated Machine Learning in Jupyter Notebooks.

- **Day 2, Experience 4** - [Making deep learning portable with ONNX](./day2-exp4/README.md)

  Attendees will experience how Contoso Auto can leverage Deep Learning technologies to scan through their vehicle specification documents to find compliance issues with new regulations. Then they will deploy this model, standardizing operationalization with ONNX. The will see how this simplifies inference runtime code, enabling pluggability of different models and targeting a broad range of runtime environments from Linux based web services to Windows/.NET based apps.