using Amazon.DynamoDBv2.DataModel;

namespace DynamoDBNetCore
{
    [DynamoDBTable("products")]
    public class Product
    {
        [DynamoDBHashKey]
        [DynamoDBProperty(HashKey)]
        public string ProductId { get; set; } = Guid.NewGuid().ToString();

        [DynamoDBProperty("name")]
        public string Name { get; set; } = string.Empty;

        public const string TableName = "products";
        public const string HashKey = "product_id";
    }
}
