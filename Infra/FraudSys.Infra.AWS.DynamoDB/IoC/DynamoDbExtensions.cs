using Amazon.Extensions.NETCore.Setup;
using FraudSys.Domain.Transacao.Repository;

namespace FraudSys.Infra.AWS.DynamoDB.IoC;

public static class DynamoDbExtensions
{
    public static void InjectInfraAwsDynamoDb(this IServiceCollection services, IConfiguration configuration)
    {
        var awsOptions = configuration.GetAWSOptions();

        CreateTablesIfNotExists(awsOptions);

        // DynamoDB
        services.AddDefaultAWSOptions(awsOptions);
        services.AddAWSService<IAmazonDynamoDB>();

        // Domain Repositories
        services.AddScoped<ILimiteClienteRepository, LimiteClienteRepository>();
        services.AddScoped<ITransacaoRepository, TransacaoRepository>();

        // Application Repositories
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    private static void CreateTablesIfNotExists(AWSOptions options)
    {
        const string tableLimiteClienteName = "LimiteClienteEntity";
        const string tableTransacaoName = "TransacaoEntity";

        var client = new AmazonDynamoDBClient(options.Region);

        var tableLimiteClienteExists = DoesTableExistAsync(client, tableLimiteClienteName).Result;
        var tableTransacaoExists = DoesTableExistAsync(client, tableTransacaoName).Result;

        if (!tableLimiteClienteExists)
        {
            InitLimiteClienteTable(client, tableLimiteClienteName);
        }

        if (!tableTransacaoExists)
        {
            InitTransacaoTable(client, tableTransacaoName);
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
            Console.WriteLine($"Erro ao verificar se tabela '{tableName}' existe no DynamoDB: {ex.Message}");
            return false;
        }

        return true;
    }

    private static void InitLimiteClienteTable(AmazonDynamoDBClient client, string tableName)
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

        InitTable(client, request, tableName);
    }

    private static void InitTransacaoTable(AmazonDynamoDBClient client, string tableName)
    {
        var request = new CreateTableRequest
        {
            TableName = tableName,
            KeySchema =
            [
                new KeySchemaElement
                {
                    AttributeName = "Id",
                    KeyType = KeyType.HASH,
                }
            ],
            AttributeDefinitions =
            [
                new AttributeDefinition
                {
                    AttributeName = "Id",
                    AttributeType = ScalarAttributeType.S
                }
            ],
            ProvisionedThroughput = new ProvisionedThroughput
            {
                ReadCapacityUnits = 5,
                WriteCapacityUnits = 5
            }
        };

        InitTable(client, request, tableName);
    }

    private static void InitTable(
        AmazonDynamoDBClient client,
        CreateTableRequest request,
        string tableName)
    {
        try
        {
            _ = client.CreateTableAsync(request).Result;
            Console.WriteLine($"Tabela '{tableName}' criada com sucesso!");
        }
        catch (System.Exception ex)
        {
            Console.WriteLine($"Erro ao criar tabela '{tableName}': {ex.Message}");
        }
    }
}
