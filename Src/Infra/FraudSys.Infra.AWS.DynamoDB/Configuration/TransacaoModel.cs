using FraudSys.Domain.LimiteCliente.Validator;
using FraudSys.Domain.Transacao.Validator;

namespace FraudSys.Infra.AWS.DynamoDB.Configuration;

[DynamoDBTable("TransacaoTable")]
public class TransacaoModel : IModel<TransacaoModel, TransacaoEntity, Guid>
{
    [DynamoDBHashKey]
    public string Id { get; set; }
    [DynamoDBProperty]
    public int Status { get; set; }
    [DynamoDBProperty]
    public string FromDocumento { get; set; }
    [DynamoDBProperty]
    public string FromNumeroAgencia { get; set; }
    [DynamoDBProperty]
    public string FromNumeroConta { get; set; }
    [DynamoDBProperty]
    public decimal FromLimiteTransacao { get; set; }
    [DynamoDBProperty]
    public string ToDocumento { get; set; }
    [DynamoDBProperty]
    public string ToNumeroAgencia { get; set; }
    [DynamoDBProperty]
    public string ToNumeroConta { get; set; }
    [DynamoDBProperty]
    public decimal ToLimiteTransacao { get; set; }
    [DynamoDBProperty]
    public decimal ValorTransferencia { get; set; }
    [DynamoDBProperty]
    public DateTime DataTransacao { get; set; }

    public TransacaoModel()
    {
        Id = string.Empty;
        Status = 0;
        FromDocumento = string.Empty;
        FromNumeroAgencia = string.Empty;
        FromNumeroConta = string.Empty;
        FromLimiteTransacao = 0;
        ToDocumento = string.Empty;
        ToNumeroAgencia = string.Empty;
        ToNumeroConta = string.Empty;
        ToLimiteTransacao = 0;
        ValorTransferencia = 0;
        DataTransacao = DateTime.MinValue;
    }

    public TransacaoModel(
        string id,
        int status,
        string fromDocumento,
        string fromNumeroAgencia,
        string fromNumeroConta,
        decimal fromLimiteTransacao,
        string toDocumento,
        string toNumeroAgencia,
        string toNumeroConta,
        decimal toLimiteTransacao,
        decimal valorTransferencia,
        DateTime dataTransacao)
    {
        Id = id;
        Status = status;
        FromDocumento = fromDocumento;
        FromNumeroAgencia = fromNumeroAgencia;
        FromNumeroConta = fromNumeroConta;
        FromLimiteTransacao = fromLimiteTransacao;
        ToDocumento = toDocumento;
        ToNumeroAgencia = toNumeroAgencia;
        ToNumeroConta = toNumeroConta;
        ToLimiteTransacao = toLimiteTransacao;
        ValorTransferencia = valorTransferencia;
        DataTransacao = dataTransacao;
    }

    public TransacaoModel EntityToModel(TransacaoEntity entity)
    {
        return new TransacaoModel(
            entity.Id.ToString(),
            (int) entity.Status,
            entity.LimiteClientePagador.Documento,
            entity.LimiteClientePagador.NumeroAgencia,
            entity.LimiteClientePagador.NumeroConta,
            entity.LimiteClientePagador.LimiteTransacao,
            entity.LimiteClienteRecebedor.Documento,
            entity.LimiteClienteRecebedor.NumeroAgencia,
            entity.LimiteClienteRecebedor.NumeroConta,
            entity.LimiteClienteRecebedor.LimiteTransacao,
            entity.Valor,
            entity.DataTransacao);
    }

    public TransacaoEntity ModelToEntity(TransacaoModel model)
    {
        var limiteClientePagador = LimiteClienteEntity.Hydrate(
            new LimiteClienteValidatorFacade(),
            model.FromDocumento,
            model.FromNumeroAgencia,
            model.FromNumeroConta,
            model.FromLimiteTransacao);

        var limiteClienteRecebedor = LimiteClienteEntity.Hydrate(
            new LimiteClienteValidatorFacade(),
            model.ToDocumento,
            model.ToNumeroAgencia,
            model.ToNumeroConta,
            model.ToLimiteTransacao);

        return TransacaoEntity.Hydrate(
            new TransacaoValidatorFacade(),
            model.Id,
            model.Status,
            limiteClientePagador,
            limiteClienteRecebedor,
            model.ValorTransferencia,
            model.DataTransacao);
    }

    public Dictionary<string, AttributeValue> ToAttributeMap(TransacaoModel model)
    {
        return new Dictionary<string, AttributeValue>
        {
            { "Id", new AttributeValue { S = model.Id } },
            { "Status", new AttributeValue { N = model.Status.ToString() } },
            { "FromDocumento", new AttributeValue { S = model.FromDocumento } },
            { "FromNumeroAgencia", new AttributeValue { S = model.FromNumeroAgencia } },
            { "FromNumeroConta", new AttributeValue { S = model.FromNumeroConta } },
            { "FromLimiteTransacao", new AttributeValue { N = model.FromLimiteTransacao.ToString(CultureInfo.InvariantCulture) } },
            { "ToDocumento", new AttributeValue { S = model.ToDocumento } },
            { "ToNumeroAgencia", new AttributeValue { S = model.ToNumeroAgencia } },
            { "ToNumeroConta", new AttributeValue { S = model.ToNumeroConta } },
            { "ToLimiteTransacao", new AttributeValue { N = model.ToLimiteTransacao.ToString(CultureInfo.InvariantCulture) } },
            { "ValorTransferencia", new AttributeValue { N = model.ValorTransferencia.ToString(CultureInfo.InvariantCulture) } },
            { "DataTransacao", new AttributeValue { S = model.DataTransacao.ToString(CultureInfo.InvariantCulture) } }
        };
    }

    public TransacaoModel FromAttributeMap(Dictionary<string, AttributeValue> attributeMap)
    {
        return new TransacaoModel
        {
            Id = attributeMap["Id"].S,
            Status = int.Parse(attributeMap["Status"].N),
            FromDocumento = attributeMap["FromDocumento"].S,
            FromNumeroAgencia = attributeMap["FromNumeroAgencia"].S,
            FromNumeroConta = attributeMap["FromNumeroConta"].S,
            FromLimiteTransacao = decimal.Parse(attributeMap["FromLimiteTransacao"].N, CultureInfo.InvariantCulture),
            ToDocumento = attributeMap["ToDocumento"].S,
            ToNumeroAgencia = attributeMap["ToNumeroAgencia"].S,
            ToNumeroConta = attributeMap["ToNumeroConta"].S,
            ToLimiteTransacao = decimal.Parse(attributeMap["ToLimiteTransacao"].N, CultureInfo.InvariantCulture),
            ValorTransferencia = decimal.Parse(attributeMap["ValorTransferencia"].N, CultureInfo.InvariantCulture),
            DataTransacao = DateTime.ParseExact(attributeMap["DataTransacao"].S, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture)
        };
    }

    public Guid GetIdFromEntity(TransacaoEntity entity)
    {
        return entity.Id;
    }
}