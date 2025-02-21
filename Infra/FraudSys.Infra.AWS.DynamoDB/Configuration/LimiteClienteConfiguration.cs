using Amazon.DynamoDBv2.DataModel;

namespace FraudSys.Infra.AWS.DynamoDB.Configuration;

[DynamoDBTable("LimiteClientesTable")]
public class LimiteClienteModel
{
    [DynamoDBHashKey]
    public string Documento { get; set; }

    [DynamoDBProperty]
    public string NumeroAgencia { get; set; }

    [DynamoDBProperty]
    public string NumeroConta { get; set; }

    [DynamoDBProperty]
    public decimal LimiteTransacao { get; set; }

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

    public static LimiteCliente ModelToEntity(
        LimiteClienteModel limiteClienteConfiguration)
    {
        return new LimiteCliente(
            limiteClienteConfiguration.Documento,
            limiteClienteConfiguration.NumeroAgencia,
            limiteClienteConfiguration.NumeroConta,
            limiteClienteConfiguration.LimiteTransacao);
    }

    public static LimiteClienteModel EntityToModel(
        LimiteCliente limiteCliente)
    {
        return new LimiteClienteModel(
            limiteCliente.Documento,
            limiteCliente.NumeroAgencia,
            limiteCliente.NumeroConta,
            limiteCliente.LimiteTransacao);
    }
}