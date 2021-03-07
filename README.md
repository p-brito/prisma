# MagicDb
[![Build](https://github.com/p-brito/MagicDb/workflows/MagicDb-Build/badge.svg)](https://github.com/p-brito/MagicDb/actions?query=workflow%3A%22MagicDb+-+build%22)
[![Nuget](https://img.shields.io/nuget/v/MagicDb)](https://www.nuget.org/packages/MagicDb/)
[![Nuget Downloads](https://img.shields.io/nuget/dt/MagicDb)](https://www.nuget.org/stats/packages/MagicDb?groupby=Version)
[![Apache License 2.0](https://img.shields.io/badge/License-MIT-blue.svg)](https://github.com/p-brito/MagicDb/blob/main/license)

MagicDb provides a fast and easy way to implement a NoSQL db provider in your project.

## Getting started

- Install the [MagicDb package](https://www.nuget.org/packages/MagicDb/) in your project.

- Register the service in your Startup.cs.

```cshsarp
  services.AddMagicDb();
```

- Add the `MagicDbOptions` to appsetting.json and register these.

```json
{
    "MagicDbOptions":{
        "Provider":"",
        "ConnectionString":"",
        "DatabaseName":""
    }
}
```
```csharp
 services.Configure<MagicDbOptions>(configuration.GetSection(nameof(MagicDbOptions)));
```

> Notice, that these options will change accordingly to the provider that you are using. Read the documentation on how to configure your provider below.

- Get the provider

```csharp
IMagicDbProvider<TEntity> DbProvider = this.serviceProvider.GetRequiredService<IMagicDbProvider<TEntity>>();
```

The MagicDbProvider has the following methods:
- GetAsync
- InsertAsync
- UpdateAsync
- DeleteAsync

> Notice, that all the entities that are used need to inherit from `BaseEntity`.

## Providers
* [MongoDB](#MongoDB)
* [Firebase](#Firebase)
* [DynamoDB](#DynamoDB)
* [CosmosDB](#CosmosDB)


## MongoDB

- First and if you do not have an account create one [here](https://www.mongodb.com/cloud/atlas/signup).

- Create you organization and after that your project.

- Create a cluster.
![Create a cluster](assets/mongodb_createCluster.gif)

- Configure your cluster, defining if this is accessible to anyone or not, and adding a database user.
![Configure a cluster](assets/mongodb_configureCluster.gif)

- Now, follow these [steps](#Getting-started).

- Add the Magic DbOptions to your appsettings.json file, consider the following example.
    ```json
    {
        "MagicDbOptions":{
            "Provider":"MongoDB",
            "ConnectionString":"mongodb+srv://admin:mypass123@myddbcluster.c900q.mongodb.net/MyDatabaseName?retryWrites=true&w=majority",
            "DatabaseName":"MyDatabaseName"
        }
    }
    ```

## Firebase

> [Under Development]

## DynamoDB
> [Under Development]

## CosmosDB
> [Under Development]
