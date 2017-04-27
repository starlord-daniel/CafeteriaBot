# Intro

## Solution Overview


## Key technologies

- [Microsoft Bot Framework](https://dev.botframework.com/)
- [Microsoft Cognitive Services](https://www.microsoft.com/cognitive-services/) (LUIS = Language Understanding Intelligent Service)

## Core team

- Björn Matthies | Bi Consultant | bjoern.matthies@daimler.com
- Christoph Seip | IT Project Lead | christoph.seip@daimler.com
- Oliver Keller | AEM Microsoft Germany | @hossa_eSport
- Daniel Heinze | Technical Evangelist Microsoft Germany | @starlord_daniel


# Customer profile #


 
# Problem statement #


 
# Solution and steps #

## Solution in general ## 


## Architecture ##

### High Level Architecture ###

 ![Architecture Diagram]

### Bot Process Flow Diagram ###

![Bot Process Flow Diagram]

# Technical delivery #
This section will include the following details of how the solution was implemented.

To get started working with bots, take a look at the following links first:

- [Documentation Bots](https://docs.botframework.com/en-us/core-concepts/getstarted/#n)
- [Step-by-step guide](https://github.com/Danielius1012/BotLabs/tree/master/Bot_Builder/1_Basic_Echo_Bot)

## Bot Patterns ##

The implemented bot consist of multiple dialogs, these are:

- Root Dialog: The main dialog which handles the routing of the requests, sends the welcome message and displays the results in a carousel form



- Menue Dialog: 

- Allergy Dialog: 

# Core Bot Capabilities #

## API Callers ##

The Cafeteria bot and the LUIS API are used in the solution are connected to the bot with the following logic:



## Bot Intelligence ##

The Cognitive Service called LUIS is used, to support the "free search" scenario. The query made by the user is send to the service, which then analyses it and specifies the intent of the query. The existing intents are:

- None: 
- 1
- 2
- 3
- 4
- 5

The following [LUIS Bot Sample](https://github.com/Microsoft/BotBuilder-Samples/tree/master/CSharp/intelligence-LUIS) explains how to develop a LUIS bot.

## SDKs used, languages, etc.

The following technologies are used for the implementation of the application:

- C#: The language the bot is build in.
- Bot Builder SDK: The SDK provided by Microsoft that is used to build the bot
- JSON: The response of the API is given as a JSON file. It is deserialized by the Newtonsoft.Json library
- REST: The LUIS API is a REST interface, which is called by the bot by using the built-in library WebRequest from C#. For more info on LUIS, go to the following link: [LUIS code story](https://www.microsoft.com/developerblog/real-life-code/2015/12/16/Speech-Intent-with-Project-Luis.html)

# Conclusion #

This section will briefly summarize the technical story with the following details included:



General lessons:

  

Next steps:

The solution is the basis for further refinement of the bot and enables the Deutsche Telekom to offer their services with the channels now available through the Microsoft Bot Framework. This will be the next step for this developed solution.

# Additional resources #
In this section, include a list of links to resources that complement your story, including (but not limited to) the following:

- [Documentation](https://dev.botframework.com)

- [Blog posts](https://blog.botframework.com/)

- [GitHub repos](https://github.com/Microsoft/BotBuilder)

- [LUIS](https://www.luis.ai)

- [Congitive Services](https://www.microsoft.com/cognitive-services)