namespace MagicDb.Core.Entities
{
    /// <summary>
    /// Defines the provider type.
    /// </summary>
    public enum DbProvider
    {
        Unspecified,
        MongoDb,
        CosmosDb,
        Firebase,
        DynamoDb
    }
}
