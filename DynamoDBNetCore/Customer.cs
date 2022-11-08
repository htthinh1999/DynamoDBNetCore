using Amazon.DynamoDBv2.DataModel;

namespace DynamoDBNetCore
{
    [DynamoDBTable("customers")]
    public class Customer
    {
        [DynamoDBHashKey]
        [DynamoDBProperty(HashKey)]
        public string CustomerId { get; set; } = Guid.NewGuid().ToString();

        [DynamoDBProperty("username")]
        public string Username { get; set; } = string.Empty;
        [DynamoDBProperty("product_id")]
        public string? ProductId { get; set; }

        public const string TableName = "customers";
        public const string HashKey = "customer_id";
    }
}
