using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using DynamoDBNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// DynamoDB
var dynamoDbConfig = builder.Configuration.GetSection("DynamoDb");
var runLocalDynamoDb = dynamoDbConfig.GetValue<bool>("LocalMode");
var clientConfig = new AmazonDynamoDBConfig
{
    RegionEndpoint = RegionEndpoint.EUWest2,
    ServiceURL = dynamoDbConfig.GetValue<string>("LocalServiceUrl")
};
var credentials = new BasicAWSCredentials("xxx", "xxx");
var dynamoClient = new AmazonDynamoDBClient(credentials, clientConfig);
var dynamoContext = new DynamoDBContext(dynamoClient);
builder.Services.AddSingleton<AmazonDynamoDBClient>(dynamoClient);
builder.Services.AddSingleton<DynamoDBContext>(dynamoContext);

// Create table if not exist
var tableExists = await dynamoClient.ListTablesAsync();
if (!tableExists.TableNames.Contains("customers"))
{
    await dynamoClient.CreateTableAsync(new CreateTableRequest
    {
        TableName = Customer.TableName,
        KeySchema = new List<KeySchemaElement>
        {
            new KeySchemaElement
            {
                AttributeName = Customer.HashKey,
                KeyType = "HASH"
            }
        },
        AttributeDefinitions = new List<AttributeDefinition>
        {
            new AttributeDefinition
            {
                AttributeName = Customer.HashKey,
                AttributeType = "S"
            }
        },
        ProvisionedThroughput = new ProvisionedThroughput
        {
            ReadCapacityUnits = 5,
            WriteCapacityUnits = 5
        }
    });
}
if (!tableExists.TableNames.Contains(Product.TableName))
{
    await dynamoClient.CreateTableAsync(new CreateTableRequest
    {
        TableName = Product.TableName,
        KeySchema = new List<KeySchemaElement>
        {
            new KeySchemaElement
            {
                AttributeName = Product.HashKey,
                KeyType = "HASH"
            }
        },
        AttributeDefinitions = new List<AttributeDefinition>
        {
            new AttributeDefinition
            {
                AttributeName = Product.HashKey,
                AttributeType = "S"
            }
        },
        ProvisionedThroughput = new ProvisionedThroughput
        {
            ReadCapacityUnits = 5,
            WriteCapacityUnits = 5
        }
    });
}

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
