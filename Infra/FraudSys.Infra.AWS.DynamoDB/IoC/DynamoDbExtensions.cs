using Amazon.Extensions.NETCore.Setup;

namespace FraudSys.Infra.AWS.DynamoDB.IoC;

public static class DynamoDbExtensions
{
    public static void InjectInfraAwsDynamoDb(this IServiceCollection services, IConfiguration configuration)
    {
        var awsOptions = configuration.GetAWSOptions();

        CreateTableIfNotExists(awsOptions);

        // DynamoDB
        services.AddDefaultAWSOptions(awsOptions);
        services.AddAWSService<IAmazonDynamoDB>();
        services.AddSingleton<IDynamoDBContext, DynamoDBContext>();

        // Domain Repositories
        services.AddSingleton<ILimiteClienteRepository, LimiteClienteRepository>();

        // Application Repositories
        services.AddSingleton<IUnitOfWork, UnitOfWork>();
    }

    private static void CreateTableIfNotExists(AWSOptions options)
    {
        const string tableName = "LimiteCliente";
        var client = new AmazonDynamoDBClient(options.Region);
        var tableExists = DoesTableExistAsync(client, tableName).Result;

        if (!tableExists)
        {
            InitTable(client, tableName);
        }
    }

    private static async Task<bool> DoesTableExistAsync(AmazonDynamoDBClient client, string tableName)
    {
        try
        {
            _ = await client.DescribeTableAsync(new DescribeTableRequest
            {
                TableName = tableName
            });
        }
        catch (ResourceNotFoundException)
        {
            return false;
        }
        catch (System.Exception ex)
        {
            Console.WriteLine($"Error checking if table exists: {ex.Message}");
            return false;
        }

        return true;
    }

    private static void InitTable(AmazonDynamoDBClient client, string tableName)
    {
        var request = new CreateTableRequest
        {
            TableName = tableName,
            KeySchema =
            [
                new KeySchemaElement
                {
                    AttributeName = "Documento",
                    KeyType = KeyType.HASH,
                }
            ],
            AttributeDefinitions =
            [
                new AttributeDefinition
                {
                    AttributeName = "Documento",
                    AttributeType = ScalarAttributeType.S
                }
            ],
            ProvisionedThroughput = new ProvisionedThroughput
            {
                ReadCapacityUnits = 5,
                WriteCapacityUnits = 5
            }
        };

        try
        {
            _ = client.CreateTableAsync(request).Result;
            Console.WriteLine($"Table {tableName} created successfully!");
        }
        catch (System.Exception ex)
        {
            Console.WriteLine($"Error creating table: {ex.Message}");
        }
    }
}
