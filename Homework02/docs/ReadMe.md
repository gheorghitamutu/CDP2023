# Homework 02

The homework requirements are explained in [Requirements file](PCD_Homework2.txt)

- [Homework 02](#homework-02)
  - [Technologies (Azure)](#technologies-azure)
    - [Cosmos DB](#cosmos-db)
    - [Azure API App](#azure-api-app)
    - [Event Hub](#event-hub)
    - [Azure Functions](#azure-functions)
    - [Web PubSub](#web-pubsub)
    - [Client/Web Apps](#clientweb-apps)
  - [Description (Requirements facing)](#description-requirements-facing)
    - [Components enumeration](#components-enumeration)
    - [Stateful vs Stateless](#stateful-vs-stateless)
    - [Functions as a Service (FaaS)](#functions-as-a-service-faas)
    - [Websockets](#websockets)
  - [System description](#system-description)
    - [Data Sources](#data-sources)
    - [Event Processor](#event-processor)
    - [Websockets / Broadcasting to users in real time](#websockets--broadcasting-to-users-in-real-time)
    - [Get/Post data](#getpost-data)
    - [CI/CD](#cicd)
    - [Stack](#stack)
  - [Architecture Diagram](#architecture-diagram)
  - [System Analysis](#system-analysis)

## Technologies (Azure)

[Azure Stack](https://azure.microsoft.com/en-us/products/azure-stack) is a portfolio of products that extend Azure services and capabilities to your environment of choice—from the datacenter to edge locations and remote offices. Build and deploy hybrid and edge computing applications and run them consistently across location boundaries.

### Cosmos DB
[Cosmos DB](https://learn.microsoft.com/en-us/azure/cosmos-db/introduction) is a globally distributed, multi-model NoSQL database service provided by Microsoft Azure. It is designed to handle large-scale, globally distributed applications and supports a variety of data models, including document, key-value, graph, and column-family data.

Cosmos DB provides a highly scalable and highly available database service with low latency and fast throughput for mission-critical applications. It supports ACID transactions and provides multiple consistency levels, enabling developers to choose the right balance of consistency and availability for their applications.

Cosmos DB can be used for a wide range of applications, including web and mobile applications, IoT, gaming, e-commerce, and more. It supports multiple APIs, including SQL, MongoDB, Cassandra, Azure Tables, and Gremlin (for graph data). This makes it easy for developers to use the API they are most familiar with or that best fits their application's needs.

### Azure API App
[Azure API App](https://azure.microsoft.com/en-us/products/app-service/api) is a cloud-based service provided by Microsoft Azure that enables developers to quickly build, deploy, and manage RESTful APIs. It is part of the Azure App Service platform and provides a fully managed hosting environment for building and running APIs.

With Azure API App, developers can use a variety of programming languages and frameworks, including .NET, Java, Node.js, and Python, to build their APIs. They can also take advantage of features such as automatic scaling, built-in authentication and authorization, and integration with Azure services like Azure Active Directory and Azure Functions.

Azure API App also provides tools for monitoring and debugging APIs, as well as analytics and reporting capabilities to help developers understand API usage and performance. It also supports continuous deployment and integration with popular development tools like Visual Studio, GitHub, and Jenkins.

Overall, Azure API App provides a comprehensive platform for building, deploying, and managing APIs in a scalable and secure manner.

### Event Hub
[Azure Event Hub](https://learn.microsoft.com/en-us/azure/event-hubs/event-hubs-about) is a cloud-based service provided by Microsoft Azure that enables the real-time streaming of large amounts of data from various sources into the cloud. It is designed to handle high volume, low latency, and highly scalable event streams, making it ideal for scenarios such as IoT telemetry, application logging, and analytics.

With Azure Event Hub, developers can capture, process, and store large volumes of data from sources such as applications, devices, and sensors. The service can handle millions of events per second and can store events for days, weeks, or months, depending on the configuration.

Azure Event Hub provides several features to help developers manage their event streams, including partitioning, event ordering, and durable storage. It also supports integration with other Azure services such as Azure Stream Analytics, Azure Functions, and Azure Data Lake Storage, making it easy to process and analyze event data in real-time.

Overall, Azure Event Hub provides a highly scalable and reliable platform for capturing, processing, and analyzing large volumes of event data from various sources in real-time.

### Azure Functions
[Azure Functions](https://learn.microsoft.com/en-us/azure/azure-functions/functions-overview) is a serverless computing service provided by Microsoft Azure that enables developers to build and run event-driven, serverless applications in the cloud. It allows developers to focus on writing code for their application logic without having to manage the underlying infrastructure.

With Azure Functions, developers can write code in a variety of programming languages, including C#, Java, JavaScript, Python, and PowerShell. They can also choose from a variety of triggers, such as HTTP requests, timers, and messaging services like Azure Service Bus and Event Hubs. This allows them to create functions that execute in response to specific events or requests.

Azure Functions provides several features to help developers manage their serverless applications, including automatic scaling, pay-per-use pricing, and integration with other Azure services such as Azure Storage, Azure Cosmos DB, and Azure Event Grid.

Overall, Azure Functions provides a flexible and scalable platform for building and running event-driven, serverless applications in the cloud. It allows developers to focus on writing code for their application logic without having to worry about managing the underlying infrastructure.

### Web PubSub
[Azure Web PubSub](https://azure.microsoft.com/en-us/products/web-pubsub) is a cloud-based service provided by Microsoft Azure that enables real-time communication between client and server applications over websockets. It is designed to handle high-frequency, low-latency messaging scenarios, making it ideal for use cases such as real-time chat, gaming, and live broadcasting.

With Azure Web PubSub, developers can create and manage hubs, which are channels for sending and receiving messages between clients and server applications. Clients can connect to a hub using websockets and subscribe to one or more channels within the hub to receive messages.

Azure Web PubSub provides several features to help developers manage their messaging scenarios, including broadcasting messages to all connected clients, authorizing and authenticating connections, and scaling up or down the number of instances of the service based on demand.

Azure Web PubSub can integrate with other Azure services, such as Azure Functions and Azure Logic Apps, to trigger serverless functions or workflows in response to messages received by the service.

Overall, Azure Web PubSub provides a highly scalable and reliable platform for real-time communication between client and server applications over websockets. It enables developers to build real-time messaging scenarios quickly and easily without worrying about the underlying infrastructure.

### Client/Web Apps
[Azure Web Apps](https://azure.microsoft.com/en-ca/products/app-service/web) is a cloud-based service provided by Microsoft Azure that enables developers to quickly and easily build, deploy, and scale web applications in the cloud. It provides a fully managed hosting environment for web applications, allowing developers to focus on building their applications rather than managing the underlying infrastructure.

With Azure Web Apps, developers can build web applications using a variety of programming languages and frameworks, including .NET, Java, Node.js, PHP, and Python. They can also take advantage of features such as automatic scaling, built-in load balancing, and integration with Azure services such as Azure SQL Database and Azure Storage.

Azure Web Apps provides tools for monitoring and debugging web applications, as well as analytics and reporting capabilities to help developers understand application usage and performance. It also supports continuous deployment and integration with popular development tools like Visual Studio, GitHub, and Jenkins.

Overall, Azure Web Apps provides a comprehensive platform for building, deploying, and managing web applications in a scalable and secure manner. It allows developers to focus on building their applications rather than managing the underlying infrastructure, making it an ideal choice for organizations of all sizes.

## Description (Requirements facing)
A stock ticker is a report of the price for certain securities, updated continuously throughout the trading session by the various stock exchanges.

### Components enumeration
Azure Cosmos DB, Azure Event Hub, Azure API App, Azure Functions, Azure Web PubSub, and Azure Web Apps.


### Stateful vs Stateless
Azure Cosmos DB and Azure Event Hub are stateful services, meaning they maintain a record of the state of the data that they store or process over time.

Azure Cosmos DB is a NoSQL database service that allows for multiple data models to be used and provides features such as global distribution, automatic indexing, and multiple consistency levels. It stores and manages data in a structured format and allows for data retrieval using various query methods.

Azure Event Hub is a streaming platform designed to handle large amounts of data, such as real-time event streams from IoT devices. It provides features such as partitioning, event ordering, and durable storage, allowing it to maintain the state of the data it processes over time.

On the other hand, Azure API App, Azure Functions, Azure Web PubSub, and Azure Web Apps are all stateless services, meaning they do not maintain any state of the data they process. They execute code in response to events or requests but do not store or manage data over time.

### Functions as a Service (FaaS)
Functions as a Service (FaaS), such as Azure Functions, are stateless by default, but they can also be designed to be stateful depending on the use case.

For example, a FaaS application can be designed to store state externally in a database or other storage service. This allows the function to access and update the state as needed, allowing it to maintain state across function invocations.

However, it's generally recommended to design FaaS applications as stateless to take advantage of the scalability benefits of serverless computing. By keeping the functions stateless, the provider can easily spin up new instances of the function to handle increased load without worrying about managing state across multiple instances.

### Websockets
Azure Web PubSub is a cloud-based service provided by Microsoft Azure that enables real-time messaging scenarios. WebSockets are one of the primary protocols used by Azure Web PubSub to provide real-time bi-directional communication between clients and the server.

When a client connects to Azure Web PubSub using WebSockets, a persistent connection is established between the client and the server. This connection remains open for the duration of the client's session, allowing the server to push messages to the client in real-time.

Azure Web PubSub supports both JSON and MessagePack encoding for WebSocket messages. JSON encoding is the default and is used when the message is small or when interoperability with other WebSocket clients is necessary. MessagePack encoding is a more compact binary format that can be used when bandwidth efficiency is a concern or when dealing with large volumes of data.

Azure Web PubSub also provides features such as authentication and authorization, message broadcasting, and message filtering, which can be used in conjunction with WebSockets to create a variety of real-time messaging scenarios.

## System description
The system gets data about stocks (ticks) while sending them to the users and also storing them.

### Data Sources
The Azure Function generates dummy data that is sent to Event Hub.
The client can also add data through the client app using the API.

### Event Processor
The Azure function gets triggered by new events from the Event Hub.
It processes them adding them to the database and publishing them to Web PubSub.

### Websockets / Broadcasting to users in real time
The Web PubSub service broadcasts the data received from Azure Function to
all its subscribers via websockets in real time.

### Get/Post data
Via the API you can either query all the data available or add new entries via client app.

### CI/CD
All the code written here is deployed via Github Actions to the Azure Services.

### Stack
Everything is written in .NET with several snippets of Javascript (requests, websockets, real-time table rows addition).

## Architecture Diagram
![Architecture Diagram](Architecture.drawio.png)

## System Analysis
When analyzing the performance of a system architecture, it is important to consider a variety of factors such as scalability, reliability, latency, throughput, and cost-effectiveness.

The system architecture described includes several Azure components optimized for high performance, low latency, and reliability. Azure Cosmos DB is a distributed database that offers high availability and low latency, with multiple consistency levels to meet application requirements. Azure Event Hub is a streaming platform that handles large volumes of data in real-time, with low-latency event delivery and support for multiple protocols. Azure API App creates and exposes APIs that can handle high request rates, with features like caching and auto-scaling. Azure Functions is a serverless compute service that responds to events and can handle high volumes of requests. Azure Web PubSub is a messaging service for real-time communication that can handle high volumes of connections, with low-latency messaging. Azure Web Apps allows hosting and running web applications in the cloud, with high traffic volume handling, auto-scaling, and load balancing. Each component is optimized for low latency, high performance, and reliability, with managed services that can reduce operational overhead and cost. The architecture is well-designed for a high-performing system, but monitoring and optimization are necessary as the system evolves.
