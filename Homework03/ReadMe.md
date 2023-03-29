# Homework 03

The homework requirements are explained in [Requirements file](PCD_Homework3.txt)

- [Homework 03](#homework-03)
- [Proposals (10 examples using Azure Cloud)](#proposals-10-examples-using-azure-cloud)
  - [Smart Irrigation System](#smart-irrigation-system)
  - [Autonomous Crop Monitoring System](#autonomous-crop-monitoring-system)
  - [Livestock Monitoring System](#livestock-monitoring-system)
  - [Precision Farming System](#precision-farming-system)
  - [Crop Yield Prediction System](#crop-yield-prediction-system)
  - [Supply Chain Management System](#supply-chain-management-system)
  - [Environmental Monitoring System](#environmental-monitoring-system)
  - [Plant Disease Detection System](#plant-disease-detection-system)
  - [Farm Equipment Monitoring System](#farm-equipment-monitoring-system)
  - [Agrochemical Management System](#agrochemical-management-system)
- [Easiest to implement (2 examples from 10 above)](#easiest-to-implement-2-examples-from-10-above)
  - [Livestock Monitoring System](#livestock-monitoring-system-1)
  - [Environmental Monitoring System](#environmental-monitoring-system-1)
- [Hardest to implement (2 examples from 10 above)](#hardest-to-implement-2-examples-from-10-above)
  - [Autonomous Crop Monitoring System](#autonomous-crop-monitoring-system-1)
  - [Supply Chain Management System](#supply-chain-management-system-1)
- [A more detailed explanation for the simplest 2 examples](#a-more-detailed-explanation-for-the-simplest-2-examples)
  - [Livestock Monitoring System](#livestock-monitoring-system-2)
  - [Environmental Monitoring System](#environmental-monitoring-system-2)
- [Architecture diagrams](#architecture-diagrams)
  - [Livestock Monitoring System (easy)](#livestock-monitoring-system-easy)
  - [Environmental Monitoring System (easy)](#environmental-monitoring-system-easy)
  - [Precision Agriculture System (difficult)](#precision-agriculture-system-difficult)
  - [Smart Irrigation System (difficult)](#smart-irrigation-system-difficult)
- [Examples of IoT data](#examples-of-iot-data)
  - [Livestock Monitoring System](#livestock-monitoring-system-3)
  - [Environmental Monitoring System](#environmental-monitoring-system-3)
- [Protobuf](#protobuf)
  - [How would you use this Protobuf template to generate a .NET class?](#how-would-you-use-this-protobuf-template-to-generate-a-net-class)
  - [Would you store Protobufs in any Azure database? If so, why?](#would-you-store-protobufs-in-any-azure-database-if-so-why)
  - [Protobuf template for the Environmental Monitoring System data](#protobuf-template-for-the-environmental-monitoring-system-data)
  - [Protobuf template for the Livestock Monitoring System data](#protobuf-template-for-the-livestock-monitoring-system-data)
- [Technologies](#technologies)
  - [Environmental Monitoring System](#environmental-monitoring-system-4)
  - [Livestock Monitoring System](#livestock-monitoring-system-4)
  - [Cost Comparison](#cost-comparison)
- [Work in progress...](#work-in-progress)
- [Bibliography](#bibliography)

# Proposals (10 examples using Azure Cloud)

## Smart Irrigation System
Azure IoT Hub and Azure Stream Analytics can be used to build a smart irrigation system using Azure Cloud. The system can use IoT sensors to collect real-time data on soil moisture, weather conditions, and plant water usage. The data collected can be processed and analyzed using Azure Stream Analytics to determine crop water needs and develop customized irrigation schedules. In addition, the system can use Azure Functions to automate irrigation control based on the analyzed data, resulting in more efficient water use and better crop yields.

## Autonomous Crop Monitoring System
Azure Cognitive Services and Azure IoT Edge can be used to build an autonomous crop monitoring system using Azure Cloud. The system can use drones equipped with cameras and sensors to collect data on crop health, soil conditions, and weather patterns. The data collected can be processed and analyzed locally on the drone using Azure IoT Edge, and then sent to Azure Cognitive Services for further analysis. Azure Cognitive Services can be used to analyze images collected by the drones to identify plant diseases or pests. This information can then be used to create a customized treatment plan for each plant or plot.

## Livestock Monitoring System
Azure IoT Hub and Azure Functions can be used to build a livestock monitoring system. The system can use IoT sensors to collect real-time data on animal health, behavior, and location. This data can be analyzed using Azure Functions to detect any anomalies and alert farmers in case of any potential health issues.

## Precision Farming System
Azure IoT Hub and Azure Machine Learning can be used to build a precision farming system. The system can use IoT sensors to collect real-time data on soil moisture, nutrient levels, and weather conditions. This data can be analyzed using Azure Machine Learning to generate customized fertilization and irrigation schedules for each crop or field.

## Crop Yield Prediction System
Azure Machine Learning and Azure Databricks can be used to build a crop yield prediction system. The system can use historical weather data, crop growth data, and other relevant data to predict crop yield for each field. This information can be used by farmers to make informed decisions regarding crop management and harvesting.

## Supply Chain Management System
Azure Blockchain and Azure Functions can be used to build a supply chain management system for agriculture. The system can use blockchain technology to track and verify the origin and quality of each crop. Azure Functions can be used to automate processes such as payment and inventory management.

## Environmental Monitoring System
Azure IoT Hub and Azure Time Series Insights can be used to build an environmental monitoring system. The system can use IoT sensors to collect real-time data on air quality, water quality, and weather conditions. This data can be analyzed using Azure Time Series Insights to identify any potential environmental hazards or risks.

## Plant Disease Detection System
Azure Cognitive Services and Azure Functions can be used to build a plant disease detection system. The system can use image recognition technology to identify any signs of plant disease or pest infestation. Azure Functions can be used to automate the treatment process based on the identified disease or pest.

## Farm Equipment Monitoring System
Azure IoT Hub and Azure Event Grid can be used to build a farm equipment monitoring system. The system can use IoT sensors to collect real-time data on equipment health, usage, and location. This data can be analyzed using Azure Event Grid to detect any potential equipment failures and alert farmers.

## Agrochemical Management System
Azure Cognitive Services and Azure Functions can be used to build an agrochemical management system. The system can use image recognition technology to identify the types and quantities of agrochemicals used in each field. Azure Functions can be used to automate the ordering and delivery of agrochemicals based on the identified needs.

# Easiest to implement (2 examples from 10 above)

## Livestock Monitoring System
The livestock monitoring system is relatively easy to implement and has a lower cost as it requires only basic IoT sensors to collect real-time data on animal health, behavior, and location. This data can be processed and analyzed using Azure Functions, which is a serverless compute service that charges only for the amount of processing power used, resulting in lower costs.

## Environmental Monitoring System
The environmental monitoring system is also relatively easy to implement and has a lower cost as it requires only basic IoT sensors to collect real-time data on air quality, water quality, and weather conditions. This data can be analyzed using Azure Time Series Insights, which is a fully managed analytics, storage, and visualization service that charges based on the amount of data stored and queried, resulting in lower costs.

# Hardest to implement (2 examples from 10 above)

## Autonomous Crop Monitoring System
The autonomous crop monitoring system involves the use of drones equipped with cameras and sensors to collect data on crop health, soil conditions, and weather patterns. Implementing this system may require specialized hardware and software development, which can be costly. Additionally, processing and analyzing the data collected using Azure Cognitive Services may also incur higher costs.

## Supply Chain Management System
The supply chain management system involves the use of blockchain technology to track and verify the origin and quality of each crop. Implementing this system may require specialized blockchain development and integration with existing systems, which can be complex and potentially costly. Additionally, automating processes such as payment and inventory management using Azure Functions may also add to the costs.

# A more detailed explanation for the simplest 2 examples

## Livestock Monitoring System
The livestock monitoring system involves the use of IoT sensors to collect real-time data on animal health, behavior, and location. These sensors can be placed on animals and can measure parameters such as body temperature, heart rate, and activity levels. This data is then transmitted to the Azure IoT Hub, which is a fully managed cloud service that provides secure and reliable communication between IoT devices and other Azure services. From there, the data can be processed and analyzed using Azure Functions, which is a serverless compute service that charges only for the amount of processing power used. Azure Functions can be used to run code in response to IoT device telemetry, allowing for real-time data processing and analysis. The analyzed data can then be visualized and monitored using Power BI, which is a business analytics service that provides interactive data visualization tools. Power BI offers a free version, as well as more advanced paid versions, making it accessible for a variety of use cases.
Overall, the livestock monitoring system can be relatively easy and cost-effective to implement using Azure Cloud, as it requires only basic IoT sensors and leverages fully managed services like IoT Hub and Azure Functions.

## Environmental Monitoring System
The environmental monitoring system involves the use of IoT sensors to collect real-time data on air quality, water quality, and weather conditions. These sensors can be placed in various locations and can measure parameters such as temperature, humidity, air pressure, and pollutant levels. This data is then transmitted to the Azure IoT Hub, where it can be processed and analyzed using Azure Time Series Insights. Azure Time Series Insights is a fully managed analytics, storage, and visualization service that charges based on the amount of data stored and queried. It can be used to store and analyze large amounts of time-stamped data, making it ideal for environmental monitoring applications. The analyzed data can then be visualized using Power BI or other custom dashboards, allowing for real-time monitoring and analysis.
Overall, the environmental monitoring system can also be relatively easy and cost-effective to implement using Azure Cloud, as it requires only basic IoT sensors and leverages fully managed services like IoT Hub and Azure Time Series Insights.

# Architecture diagrams

## Livestock Monitoring System (easy)

    +---------------------------------------+
    |            IoT Sensors                |
    |         (temperature, heart rate,     |
    |          activity levels, etc.)       |
    +----------------+----------------------+
                     |
                     | HTTP
                     |
    +----------------+----------------------+
    |            Azure IoT Hub              |
    |     (data ingestion and routing)      |
    +----------------+----------------------+
                     |
                     | HTTP/Event Grid
                     |
    +-----------------+---------------------+
    |            Azure Functions            |
    |      (data processing and analysis)   |
    +-----------------+---------------------+
                     |
                     | REST
                     |
    +-----------------+---------------------+
    |            Power BI                   |
    |   (data visualization and monitoring) |
    +---------------------------------------+

## Environmental Monitoring System (easy)

     +---------------------------------------+
     |             IoT Sensors               |
     |  (temperature, humidity, air quality, |
     |          weather conditions, etc.)    |
     +----------------+----------------------+
                      |
                      | HTTP
                      |
    +-----------------+----------------------+
    |            Azure IoT Hub               |
    |     (data ingestion and routing)       |
    +-----------------+----------------------+
                      |
                      | REST
                      |
    +-----------------+----------------------+
    |      Azure Time Series Insights        |
    |  (data storage, analysis and querying) |
    +-----------------+----------------------+
                      |
                      | REST
                      |
    +-----------------+----------------------+
    |            Power BI                    |
    |    (data visualization and monitoring) |
    +----------------------------------------+

## Precision Agriculture System (difficult)

    +---------------------------------------+
    |            IoT Sensors                |
    |     (soil moisture, temperature,      |
    |    humidity, light levels, etc.)      |
    +----------------+----------------------+
                     |
                     | HTTP
                     |
    +----------------+----------------------+
    |            Azure IoT Hub              |
    |     (data ingestion and routing)      |
    +----------------+----------------------+
                     |
                     | HTTP
                     |
    +-----------------+---------------------+
    |            Azure Stream Analytics     |
    |    (data processing and real-time     |
    |              analytics)               |
    +-----------------+---------------------+
                     |
                     | REST/Event Grid
                     |
    +-----------------+---------------------+
    |      Azure Machine Learning Service   |
    |     (training and deployment of ML    |
    |            models)                    |
    +-----------------+---------------------+
                     |
                     | REST
                     |
    +-----------------+---------------------+
    |      Azure Functions                  |
    |  (data processing, workflow management|
    |            and automation)            |
    +-----------------+---------------------+
                     |
                     | REST/Event Grid
                     |
    +-----------------+---------------------+
    |            Power BI                   |
    |    (data visualization and monitoring)|
    +---------------------------------------+

## Smart Irrigation System (difficult)

     +---------------------------------------+
     |             IoT Sensors               |
     |   (soil moisture, weather conditions, |
     |          water flow rates, etc.)      |
     +----------------+----------------------+
                      |
                      | HTTP
                      |
    +-----------------+----------------------+
    |            Azure IoT Hub               |
    |     (data ingestion and routing)       |
    +-----------------+----------------------+
                      |
                      | REST
                      |
    +-----------------+----------------------+
    |       Azure Time Series Insights       |
    | (data storage, analysis and querying)  |
    +-----------------+----------------------+
                      |
                      | REST/Event Grid
                      |
    +-----------------+----------------------+
    |      Azure Machine Learning Service    |
    |     (training and deployment of ML     |
    |              models)                   |
    +-----------------+----------------------+
                      |
                      | REST/Event Grid
                      |
    +-----------------+----------------------+
    |            Azure Functions             |
    |  (data processing, workflow management |
    |            and automation)             |
    +-----------------+----------------------+
                      |
                      | REST
                      |
    +-----------------+----------------------+
    |            Power BI                    |
    |    (data visualization and monitoring) |
    +----------------------------------------+

# Examples of IoT data

## Livestock Monitoring System

    {
      "animal_id": "A123",
      "body_temperature": 39.2,
      "activity_level": 8,
      "heart_rate": 72,
      "respiration_rate": 14,
      "location": {
        "latitude": 37.7749,
        "longitude": -122.4194
      },
      "feed_intake": 4.5,
      "water_consumption": 15,
      "milk_production": 12.5,
      "weight": 450
    }

## Environmental Monitoring System

    {
      "sensor_id": "S123",
      "temperature": 22.1,
      "humidity": 54.3,
      "co2_level": 1280,
      "particulate_matter": 10,
      "soil_moisture": 32,
      "light_level": 2300,
      "wind_speed": 5.3,
      "wind_direction": 270,
      "precipitation": 0,
      "water_quality": {
        "ph_level": 7.2,
        "dissolved_oxygen": 7.5
      },
      "noise_level": 45
    }

# Protobuf

## How would you use this Protobuf template to generate a .NET class?

To generate a .NET class from the Protobuf template, you can use the protoc compiler along with the C# plugin. Here are the steps to generate the .NET class:

Install the protobuf compiler and the C# plugin. You can follow the instructions in the official Protobuf documentation to download and install these tools.

Create a .proto file that includes the Protobuf template. You can save the template in a file named environmental_data.proto.

Run the following command to generate the .NET class:

    protoc --csharp_out=. environmental_data.proto


This command generates a C# class file named environmental_data.cs in the current directory.

Use the generated C# class in your .NET application to encode and decode data using the Protobuf format.

## Would you store Protobufs in any Azure database? If so, why?

Yes, you can store Protobufs in any Azure database that supports binary data, such as Azure Blob Storage, Azure Cosmos DB, and Azure Data Lake Storage. Here are some reasons why you might want to store Protobufs in an Azure database:

Efficient storage and retrieval: Protobufs are binary data that are designed to be compact and efficient, which means that they can take up less storage space and be retrieved faster than equivalent JSON or XML data. By storing Protobufs in an Azure database, you can take advantage of the optimized storage and retrieval capabilities of the database to efficiently store and retrieve large amounts of data.

Interoperability: Protobufs are a language- and platform-independent format that can be used to represent data in a variety of programming languages, including C++, Java, Python, and C#. By storing Protobufs in an Azure database, you can enable interoperability between different applications and systems that use different programming languages and platforms.

Compatibility with cloud services: Many Azure cloud services, such as Azure Event Hubs and Azure Stream Analytics, support Protobufs as a data format. By storing Protobufs in an Azure database, you can easily integrate your data with these cloud services and take advantage of their data processing and analytics capabilities.

Schema evolution: Protobufs support schema evolution, which means that you can change the structure of your data over time without breaking compatibility with older versions of the data. By storing Protobufs in an Azure database, you can take advantage of this feature to evolve your data schema over time as your application and business needs change.

Overall, storing Protobufs in an Azure database can provide a number of benefits for your application, including efficient storage and retrieval, interoperability, compatibility with cloud services, and schema evolution.

## Protobuf template for the Environmental Monitoring System data

    syntax = "proto3";
    
    message EnvironmentalData {
      message Temperature {
        float value = 1;
        string unit = 2;
      }
    
      message Humidity {
        float value = 1;
        string unit = 2;
      }
    
      message Pressure {
        float value = 1;
        string unit = 2;
      }
    
      message Wind {
        float speed = 1;
        float direction = 2;
        string unit = 3;
      }
    
      message DateTime {
        int64 timestamp = 1;
        string timezone = 2;
      }
    
      message Location {
        float latitude = 1;
        float longitude = 2;
      }
    
      Temperature temperature = 1;
      Humidity humidity = 2;
      Pressure pressure = 3;
      Wind wind = 4;
      DateTime datetime = 5;
      Location location = 6;
    }

## Protobuf template for the Livestock Monitoring System data

    syntax = "proto3";
    
    package livestock;
    
    message SensorReading {
      string sensor_id = 1;
      double body_temperature = 2;
      int32 activity_level = 3;
      double heart_rate = 4;
      double respiration_rate = 5;
      int64 timestamp = 6;
      string animal_id = 7;
    }
    
    message AnimalLocation {
      string animal_id = 1;
      double latitude = 2;
      double longitude = 3;
      int64 timestamp = 4;
    }
    
    message FeedIntake {
      string animal_id = 1;
      double amount = 2;
      int64 timestamp = 3;
    }
    
    message WaterConsumption {
      string animal_id = 1;
      double amount = 2;
      int64 timestamp = 3;
    }
    
    message MilkProduction {
      string animal_id = 1;
      double amount = 2;
      int64 timestamp = 3;
    }
    
    message AnimalWeight {
      string animal_id = 1;
      double weight = 2;
      int64 timestamp = 3;
    }

This template defines six message types:

SensorReading: Represents a reading from a sensor attached to an animal. The message contains the ID of the sensor, the body temperature, activity level, heart rate, respiration rate, timestamp, and the ID of the animal.

AnimalLocation: Represents the current location of an animal. The message contains the ID of the animal, the latitude and longitude coordinates, and the timestamp.

FeedIntake: Represents the amount of feed consumed by an animal. The message contains the ID of the animal, the amount of feed consumed, and the timestamp.

WaterConsumption: Represents the amount of water consumed by an animal. The message contains the ID of the animal, the amount of water consumed, and the timestamp.

MilkProduction: Represents the amount of milk produced by a lactating animal. The message contains the ID of the animal, the amount of milk produced, and the timestamp.

AnimalWeight: Represents the weight of an animal. The message contains the ID of the animal, the weight, and the timestamp.

# Technologies

## Environmental Monitoring System

There are several Azure technologies that can be involved in an Environmental Monitoring System, depending on the specific needs and requirements of the system. Some possible examples include:

Azure IoT Hub: to receive and process data from various sensors and devices installed in the environment, such as temperature, humidity, air quality, water quality, or noise level sensors.

Azure Stream Analytics: to analyze real-time data streams from the sensors and devices and extract insights such as trends, patterns, or anomalies.

Azure Functions: to trigger custom logic in response to specific events or conditions in the system, such as sending alerts or notifications to users, or triggering automated actions based on certain data patterns.

Azure Cosmos DB: to store and manage large volumes of data from the sensors and devices, with features such as high availability, low latency, and flexible querying.

Azure Maps: to visualize the data from the sensors and devices on maps and geospatial visualizations, and to provide location-based services such as routing, geocoding, or geofencing.

Azure Machine Learning: to build predictive models or machine learning algorithms that can detect early signs of environmental risks or predict future trends based on historical data.

Azure Virtual Machines: to run software applications or services that require a dedicated, customizable environment, such as a dashboard or a monitoring system that visualizes the data from the sensors and devices.

Azure Active Directory: to manage user access and permissions to the Environmental Monitoring System, with features such as multi-factor authentication, single sign-on, and identity protection.

## Livestock Monitoring System

Azure IoT Hub: to receive and process data from IoT devices attached to the livestock, and to send commands and alerts back to those devices.

Azure Stream Analytics: to analyze real-time data streams from the IoT devices and extract insights such as anomalies, trends, or correlations.

Azure Functions: to trigger custom logic in response to specific events or conditions in the system, such as sending notifications to farmers or triggering automated actions based on certain data patterns.

Azure Cosmos DB: to store and manage large volumes of data from the IoT devices, with features such as high availability, low latency, and flexible querying.

Azure Machine Learning: to build predictive models or machine learning algorithms that can detect early signs of diseases or predict future growth and productivity of the livestock based on historical data.

Azure Virtual Machines: to run software applications or services that require a dedicated, customizable environment, such as a dashboard or a monitoring system that visualizes the data from the IoT devices.

Azure Active Directory: to manage user access and permissions to the Livestock Monitoring System, with features such as multi-factor authentication, single sign-on, and identity protection.

## Cost Comparison

The Livestock Monitoring System would likely be more expensive to implement compared to the Environmental Monitoring System due to the need for more specialized hardware and sensors for tracking the various physiological parameters of the animals.

In terms of Azure technologies, some of the more expensive ones would depend on the specific implementation and requirements of the system. For example, if the Livestock Monitoring System required the use of Azure IoT Hub for collecting and processing data from thousands of sensors attached to the animals, then the cost of the IoT Hub would be a significant factor. Similarly, if the Environmental Monitoring System required the use of Azure Stream Analytics for processing large volumes of real-time data from multiple sources, then the cost of the Stream Analytics service would be a significant factor.

In general, however, some of the more expensive Azure technologies include services that require a lot of processing power or storage, such as Azure Machine Learning, Azure Data Lake Storage, and Azure Cosmos DB. These services can provide significant value for certain use cases but may also come with a higher cost.

# Work in progress...

# Bibliography
[Synthetic Data Generation for the Internet of Things](https://tigerprints.clemson.edu/cgi/viewcontent.cgi?article=1035&context=computing_pubs)

[IoSynth - IoT device/sensor simulator and synthetic data generator](https://github.com/rradev/iosynth)