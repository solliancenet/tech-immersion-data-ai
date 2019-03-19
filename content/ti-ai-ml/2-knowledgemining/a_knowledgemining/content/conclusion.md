## Wrap-up

In this experience, you learned how to leverage Azure Search and Cognitive Services to perform knowledge mining on unstructured data stored in Cosmos DB. Using a combination of preconfigured and custom cognitive skills, you built a Cognitive Search pipeline to enrich the source data in route to an Azure Search Index.

Using preconfigured cognitive skills, you were able to add language detection, sentiment analysis, and key phrase and entity extraction to your search pipeline. These skills enriched your search index with additional metadata about the tweets being indexed.

You then used an Azure Function App to create a custom cognitive skill, which used the Translator Text Cognitive Service to translate tweets into English. Using the Custom Web API skill, you integrated the custom skill to your cognitive search pipeline.

The end result is rich additional content in an Azure Search index, created by a cognitive search indexing pipeline. The output is a full-text searchable index on Azure Search.

## Additional resources and more information

To continue learning and expand your understanding of Knowledge Mining with Cognitive Search, use the links below.

- [Introduction to Azure Search](https://docs.microsoft.com/azure/search/search-what-is-azure-search)
- [Introduction to Cognitive Services](https://docs.microsoft.com/azure/cognitive-services/welcome)
- [Introduction to Cognitive Search](https://docs.microsoft.com/azure/search/cognitive-search-concept-intro)
- [Attach a Cognitive Services resource with a skillset in Azure Search](https://docs.microsoft.com/azure/search/cognitive-search-attach-cognitive-services)
- [Azure Search Service REST API](https://docs.microsoft.com/azure/search/search-api-preview)
- [Predefined Cognitive Search skills](https://docs.microsoft.com/azure/search/cognitive-search-predefined-skills)
  - [Key Phrase Extraction](https://docs.microsoft.com/azure/search/cognitive-search-skill-keyphrases)
  - [Language Detection](https://docs.microsoft.com/azure/search/cognitive-search-skill-language-detection)
  - [Entity Recognition](https://docs.microsoft.com/azure/search/cognitive-search-skill-entity-recognition)
  - [Text Merger](https://docs.microsoft.com/azure/search/cognitive-search-skill-textmerger)
  - [Text Split](https://docs.microsoft.com/azure/search/cognitive-search-skill-textsplit)
  - [Sentiment](https://docs.microsoft.com/azure/search/cognitive-search-skill-sentiment)
  - [Image Analysis](https://docs.microsoft.com/azure/search/cognitive-search-skill-image-analysis)
  - [Optical Character Recognition (OCR)](https://docs.microsoft.com/azure/search/cognitive-search-skill-ocr)
  - [Shaper](https://docs.microsoft.com/azure/search/cognitive-search-skill-shaper)
- [Custom Web API skill](https://docs.microsoft.com/azure/search/cognitive-search-custom-skill-web-api)
- [How to add a custom skill to a cognitive search pipeline](https://docs.microsoft.com/azure/search/cognitive-search-custom-skill-interface)
- [Learn how to call cognitive search APIs](https://docs.microsoft.com/azure/search/cognitive-search-tutorial-blob)
- [Learn Cognitive Search](https://azure.github.io/LearnAI-Cognitive-Search/)
- [Enterprise Knowledge Mining Bootcamp](https://azure.github.io/LearnAI-KnowledgeMiningBootcamp/)
- [Azure Search pricing](https://azure.microsoft.com/pricing/details/search/)