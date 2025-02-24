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

    public static IEnumerable<TransacaoEntity> TransacoesValidas(int quantidade)
    {
        var limites = new List<TransacaoEntity>();

        for (var i = 1; i <= quantidade; i++)
        {
            limites.Add(TransacaoValida(i * 100m));
        }

        return limites;
    }
}