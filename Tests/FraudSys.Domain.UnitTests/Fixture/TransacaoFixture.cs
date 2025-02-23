namespace FraudSys.Domain.UnitTests.Fixture;

public static class TransacaoFixture
{
    public static TransacaoEntity TransacaoValida(decimal valor)
    {
        var limiteClientePagador = LimiteClienteFixture.LimiteClienteValido("1");
        var limiteClienteRecebedor = LimiteClienteFixture.LimiteClienteValido("2");

        return TransacaoEntity.Create(
            new TransacaoValidatorFacade(),
            limiteClientePagador,
            limiteClienteRecebedor,
            valor);
    }
}