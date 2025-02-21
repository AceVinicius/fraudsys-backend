namespace FraudSys.Domain.UnitTests.Transacao;

public class TransacaoEntityTest
{
    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(1000)]
    [InlineData(10000)]
    public void Given_TransacaoEntity_When_ValorTransferenciaIsValid_Then_CreateEntity(decimal valor)
    {
        // Arrange
        var limiteClientePagador = LimiteClienteFixture.LimiteClienteValido("1");
        var limiteClienteRecebedor = LimiteClienteFixture.LimiteClienteValido("2");

        // Act
        var transacao = new TransacaoEntity(limiteClientePagador, limiteClienteRecebedor, valor);

        // Assert
        Assert.NotNull(transacao);
        Assert.Equal(valor, transacao.Valor);
        Assert.Equal(StatusTransacao.Pendente, transacao.Status);
        Assert.NotEmpty(transacao.Id.ToString());
    }

    [Theory]
    [InlineData(-123.1231)]
    [InlineData(0)]
    public void
    Given_TransacaoEntity_When_ValorTransferenciaIsInvalid_Then_ThrowEntityCreationException(decimal valor)
    {
        // Arrange
        var limiteClientePagador = LimiteClienteFixture.LimiteClienteValido("1");
        var limiteClienteRecebedor = LimiteClienteFixture.LimiteClienteValido("2");

        // Act
        Action act = () =>
            _ = new TransacaoEntity(limiteClientePagador, limiteClienteRecebedor, valor);

        // Assert
        Assert.Throws<EntityCreationException>(act);
    }

    [Theory]
    [InlineData(1, 999, 1001)]
    [InlineData(100, 900, 1100)]
    [InlineData(1000, 0, 2000)]
    public void Given_TransacaoEntityValida_When_EfetuarTransacao_Then_TransacaoIsApproved(
        decimal valor,
        decimal valorPagador,
        decimal valorRecebedor)
    {
        // Arrange
        var limiteClientePagador = LimiteClienteFixture.LimiteClienteValido("1");
        var limiteClienteRecebedor = LimiteClienteFixture.LimiteClienteValido("2");
        var transacao = new TransacaoEntity(limiteClientePagador, limiteClienteRecebedor, valor);

        // Act
        transacao.EfetuarTransacao();

        // Assert
        Assert.Equal(StatusTransacao.Aprovada, transacao.Status);
        Assert.Equal(valorPagador, limiteClientePagador.LimiteTransacao);
        Assert.Equal(valorRecebedor, limiteClienteRecebedor.LimiteTransacao);
        Assert.NotEmpty(transacao.DataTransacao.ToString(CultureInfo.InvariantCulture));
    }

    [Theory]
    [InlineData(1001)]
    [InlineData(1100)]
    public void Given_TransacaoEntityInvalida_When_EfetuarTransacao_Then_TransacaoIsRejected(decimal valor)
    {
        // Arrange
        var limiteClientePagador = LimiteClienteFixture.LimiteClienteValido("1");
        var limiteClienteRecebedor = LimiteClienteFixture.LimiteClienteValido("2");
        var transacao = new TransacaoEntity(limiteClientePagador, limiteClienteRecebedor, valor);

        // Act
        transacao.EfetuarTransacao();

        // Assert
        Assert.Equal(StatusTransacao.Rejeitada, transacao.Status);
        Assert.Equal(1000m, limiteClientePagador.LimiteTransacao);
        Assert.Equal(1000m, limiteClienteRecebedor.LimiteTransacao);
        Assert.NotEmpty(transacao.DataTransacao.ToString(CultureInfo.InvariantCulture));
    }
}