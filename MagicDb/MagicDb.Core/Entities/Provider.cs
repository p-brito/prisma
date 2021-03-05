namespace MagicDb.Core.Entities
{
    /// <summary>
    /// Defines the provider type.
    /// </summary>
    internal enum Provider
    {
        Unspecified,
        MongoDb,
        CosmosDb,
        Firebase,
        DynamoDb
    }
}
