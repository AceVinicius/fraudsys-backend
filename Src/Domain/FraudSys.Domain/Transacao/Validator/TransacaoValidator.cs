namespace FraudSys.Domain.Transacao.Validator;

public static class TransacaoValidator
{
    public static void ValidateId(string id)
    {
        if (! Guid.TryParse(id, out _))
        {
            throw new EntityValidationException("Id da transação não pode ser nulo");
        }
    }

    public static void ValidateStatus(int status)
    {
        if (! System.Enum.IsDefined(typeof(StatusTransacao), status))
        {
            throw new EntityValidationException("Status da transação não pode ser nulo");
        }
    }

    public static void ValidateLimiteClienteEntity(LimiteClienteEntity limiteCliente)
    {
        if (limiteCliente == null)
        {
            throw new EntityValidationException("Limite do cliente não pode ser nulo");
        }
    }

    public static void ValidatePagadorERecebedor(LimiteClienteEntity limiteClientePagador, LimiteClienteEntity limiteClienteRecebedor)
    {
        if (limiteClientePagador.Documento == limiteClienteRecebedor.Documento)
        {
            throw new EntityValidationException("Limite do cliente pagador e recebedor não podem ser iguais");
        }
    }

    public static void ValidadeValorTransacao(decimal valor)
    {
        if (valor <= 0)
        {
            throw new EntityValidationException("Valor da transação deve ser maior que zero");
        }
    }

    public static void ValidateStatusTransacao(StatusTransacao status)
    {
        if (status != StatusTransacao.Pendente)
        {
            throw new EntityValidationException("Transação já foi efetuada");
        }
    }
}