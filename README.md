# Prisma
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
IPrismaProvider<TEntity> DbProvider = this.serviceProvider.GetRequiredService<IPrismaProvider<TEntity>>();
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

- Add the Magic DbOptions to your appsettings.json file, consider the following example.
    ```json
    {
        "PrismaOptions":{
            "Provider":"MongoDB",
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
- Add the Magic DbOptions to your appsettings.json file, consider the following example.
    ```json
    {
        "PrismaOptions": {
            "Provider": "DynamoDb",
            "ConnectionString": "accesskey=AKIATEJJHIIBH5Z6XUMA;secretkey=9VB7jBKQvdyNrVX5d2t+cD4UTwnW4Nc2f98vigHa;region=eu-west-1"
        }
    }
    ```
## CosmosDB
> [Under Development]
