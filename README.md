# :small_red_triangle: Prisma
[![Build](https://github.com/p-brito/prisma/workflows/build%20and%20test/badge.svg)](https://github.com/p-brito/prisma/actions/workflows/build.yml?query=workflow%3A%22build+-+and+-+test%22)
[![Nuget](https://img.shields.io/nuget/v/Prisma)](https://www.nuget.org/packages/prisma/)
[![Apache License 2.0](https://img.shields.io/badge/License-MIT-blue.svg)](https://github.com/p-brito/Prisma/blob/main/license)

Prisma provides a fast and easy way to implement a NoSQL db provider in your project.

## Getting started

- Install the [Prisma package](https://www.nuget.org/packages/Prisma/) in your project.

- Register the service in your Startup.cs.

```cshsarp
  services.AddPrisma();
```

- Add the `PrismaOptions` to appsetting.json and register these.

```json
{
    "PrismaOptions":{
        "Provider":"",
        "ConnectionString":"",
    }
}
```
```csharp
 services.Configure<PrismaOptions>(configuration.GetSection(nameof(PrismaOptions)));
```

> Notice, that these options will change accordingly to the provider that you are using. Read the documentation on how to configure your provider below.

- Get the provider

```csharp
IPrismaProvider<TEntity> myProvider = this.serviceProvider.GetRequiredService<IPrismaProvider<TEntity>>();
```

The PrismaProvider has the following methods:
- GetAsync
- InsertAsync
- UpdateAsync
- DeleteAsync

> Notice, that all the entities that are used need to inherit from `BaseEntity`.

## Providers
* [MongoDB](#MongoDB)
* [DynamoDB](#DynamoDB)
* [CosmosDB](#CosmosDB)


## MongoDB

- First and if you do not have an account create one [here](https://www.mongodb.com/cloud/atlas/signup).

- Create your organization and after that your project.

- Create a cluster.
![Create a cluster](assets/mongodb_createCluster.gif)

- Configure your cluster, defining if this is accessible to anyone or not, and adding a database user.
![Configure a cluster](assets/mongodb_configureCluster.gif)

- Now, follow these [steps](#Getting-started).

- Add the prisma options to your appsettings.json file, consider the following example.
    ```json
    {
        "PrismaOptions":{
            "Provider":"MongoDb",
            "ConnectionString":"mongodb+srv://admin:mypass123@myddbcluster.c900q.mongodb.net/MyDatabaseName?retryWrites=true&w=majority"
        }
    }
    ```

## DynamoDB

- First and if you haven't, create an account [here.](https://aws.amazon.com/free)

- Follow these steps to configure your account, [see this.](https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/SettingUp.DynamoWebService.html#SettingUp.DynamoWebService.GetCredentials)

- Create a new group.
![Create a group](assets/aws_create_group.gif)

- Create a user, add it to the group that you have created, save the `Access Key ID` and the `Secret access key` you will need this later on.

- Now, follow these [steps](#Getting-started).

- Add the prisma options to your appsettings.json file, consider the following example.
    ```json
    {
        "PrismaOptions": {
            "Provider": "DynamoDb",
            "ConnectionString": "accesskey=AKIATEJJHIIBH5Z6XUMA;secretkey=9VB7jBKQvdyNrVX5d2t+cD4UTwnW4Nc2f98vigHa;region=eu-west-1"
        }
    }
    ```
## CosmosDB

- First and if you haven't, create an account [here.](https://azure.microsoft.com/en-us/)

- Search for `Azure Cosmos DB` and create an account.
![Create a cosmosdb account](assets/create_cosmosdb.gif)

- Click on `keys` and get your `PRIMARY CONNECTION STRING`.
![Get primary connection string](assets/cosmosdb_get_keys.png)

- Now, follow these [steps](#Getting-started).

- Add the prisma options to your appsettings.json file, consider the following example.

    ```json
    {
        "PrismaOptions":{
            "Provider":"CosmosDb",
            "ConnectionString":"AccountEndpoint=MyEndpoint;AccountKey=myAccountKey;DatabaseKey=myDatabaseKey;"
        }
    }
    ```
> Notice, that you can define the throughput in your connection string by using the following parameter `AutoscaleThroughput=value` or `ManualThroughput=value`. By default the value of the throughput is set to 1000 RU/s.

The `AutoScaleThroughput`, provisions the RU/s based on the workload between the range of 10% of Max RU/s to Max RU’s. Be aware that the cost of autoscale is 50% more than the standard throughput.

The `ManualThroughput`, use this to define a custom value for your throughput.

To know more about this consider this [article.](https://medium.com/@ravisonga/understanding-azure-cosmos-db-autoscale-30935aa35477)
