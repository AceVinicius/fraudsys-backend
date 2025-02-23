using FraudSys.Domain.LimiteCliente.Validator;

namespace FraudSys.Infra.AWS.DynamoDB.Configuration;

[DynamoDBTable("LimiteClienteTable")]
public class LimiteClienteModel : IModel<LimiteClienteModel, LimiteClienteEntity, string>
{
    [DynamoDBHashKey]
    public string Documento { get; set; }

    [DynamoDBProperty]
    public string NumeroAgencia { get; set; }

    [DynamoDBProperty]
    public string NumeroConta { get; set; }

    [DynamoDBProperty]
    public decimal LimiteTransacao { get; set; }

    public LimiteClienteModel()
    {
        Documento = string.Empty;
        NumeroAgencia = string.Empty;
        NumeroConta = string.Empty;
    }

    public LimiteClienteModel(
        string documento,
        string numeroAgencia,
        string numeroConta,
        decimal limiteTransacao)
    {
        Documento = documento;
        NumeroAgencia = numeroAgencia;
        NumeroConta = numeroConta;
        LimiteTransacao = limiteTransacao;
    }

    public LimiteClienteModel EntityToModel(LimiteClienteEntity entity)
    {
        return new LimiteClienteModel(
            entity.Documento,
            entity.NumeroAgencia,
            entity.NumeroConta,
            entity.LimiteTransacao);
    }

    public LimiteClienteEntity ModelToEntity(LimiteClienteModel model)
    {
        return LimiteClienteEntity.Hydrate(
            new LimiteClienteValidatorFacade(),
            model.Documento,
            model.NumeroAgencia,
            model.NumeroConta,
            model.LimiteTransacao);
    }

    public Dictionary<string, AttributeValue> ToAttributeMap(LimiteClienteModel model)
    {
        return new Dictionary<string, AttributeValue>
        {
            { "Documento", new AttributeValue { S = model.Documento } },
            { "NumeroAgencia", new AttributeValue { S = model.NumeroAgencia } },
            { "NumeroConta", new AttributeValue { S = model.NumeroConta } },
            { "LimiteTransacao", new AttributeValue { N = model.LimiteTransacao.ToString(CultureInfo.InvariantCulture) } }
        };
    }

    public LimiteClienteModel FromAttributeMap(Dictionary<string, AttributeValue> attributeMap)
    {
        return new LimiteClienteModel
        {
            Documento = attributeMap["Documento"].S,
            NumeroAgencia = attributeMap["NumeroAgencia"].S,
            NumeroConta = attributeMap["NumeroConta"].S,
            LimiteTransacao = decimal.Parse(attributeMap["LimiteTransacao"].N,  CultureInfo.InvariantCulture)
        };
    }

    public string GetIdFromEntity(LimiteClienteEntity entity)
    {
        return entity.Documento;
    }
}