using FraudSys.Domain.LimiteCliente;

namespace FraudSys.Domain.UnitTests.LimiteCliente;

public class LimiteClienteTest
{
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(100.12371)]
    public void Given_LimiteCliente_When_CriarLimiteCliente_Then_CreateEntity(decimal limite)
    {
        // Arrange
        var fix = LimiteClienteFixture.LimiteClienteValido("1");

        // Act
        var limiteCliente = new LimiteClienteEntity(
            fix.Documento,
            fix.NumeroAgencia,
            fix.NumeroConta,
            limite);

        // Assert
        Assert.NotNull(limiteCliente);
        Assert.Equal(fix.Documento, limiteCliente.Documento);
        Assert.Equal(fix.NumeroAgencia, limiteCliente.NumeroAgencia);
        Assert.Equal(fix.NumeroConta, limiteCliente.NumeroConta);
        Assert.Equal(limite, limiteCliente.LimiteTransacao);
    }

    [Theory]
    [InlineData("1", "1234", "4567-0", -1)]
    [InlineData("1", "1234", "4567-0", -1.12973129)]
    public void Given_LimiteClienteInvalido_When_CriarLimiteCliente_Then_ThrowEntityCreationException(
        string documento,
        string numeroAgencia,
        string numeroConta,
        decimal limite)
    {
        // Arrange

        // Act
        Action act = () =>
            _ = new LimiteClienteEntity(
                documento,
                numeroAgencia,
                numeroConta,
                limite);

        // Assert
        Assert.Throws<EntityCreationException>(act);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(100.12371)]
    public void Given_LimiteCliente_When_AtualizarLimiteCliente_Then_UpdateEntity(decimal limite)
    {
        // Arrange
        var fix = LimiteClienteFixture.LimiteClienteValido("1");
        var limiteCliente = new LimiteClienteEntity(
            fix.Documento,
            fix.NumeroAgencia,
            fix.NumeroConta,
            fix.LimiteTransacao);

        // Act
        limiteCliente.AtualizarLimite(limite);

        // Assert
        Assert.NotNull(limiteCliente);
        Assert.Equal(fix.Documento, limiteCliente.Documento);
        Assert.Equal(fix.NumeroAgencia, limiteCliente.NumeroAgencia);
        Assert.Equal(fix.NumeroConta, limiteCliente.NumeroConta);
        Assert.Equal(limite, limiteCliente.LimiteTransacao);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-1.23984329)]
    public void Given_LimiteCliente_When_AtualizarLimiteClienteInvalido_Then_ThrowEntityUpdateException(
        decimal limite)
    {
        // Arrange
        var fix = LimiteClienteFixture.LimiteClienteValido("1");
        var limiteCliente = new LimiteClienteEntity(
            fix.Documento,
            fix.NumeroAgencia,
            fix.NumeroConta,
            fix.LimiteTransacao);

        // Act
        var act = () => limiteCliente.AtualizarLimite(limite);

        // Assert
        Assert.Throws<EntityCreationException>(act);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(12.1231)]
    public void Given_LimiteCliente_When_CreditarCliente_Then_UpdateEntity(decimal valor)
    {
        // Arrange
        var fix = LimiteClienteFixture.LimiteClienteValido("1");
        var limiteCliente = new LimiteClienteEntity(
            fix.Documento,
            fix.NumeroAgencia,
            fix.NumeroConta,
            fix.LimiteTransacao);

        // Act
        limiteCliente.Creditar(valor);

        // Assert
        Assert.Equal(fix.LimiteTransacao + valor, limiteCliente.LimiteTransacao);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    [InlineData(-12.1231)]
    public void Given_LimiteCliente_When_CreditarClienteInvalido_Then_ThrowTransactionException(decimal valor)
    {
        // Arrange
        var fix = LimiteClienteFixture.LimiteClienteValido("1");
        var limiteCliente = new LimiteClienteEntity(
            fix.Documento,
            fix.NumeroAgencia,
            fix.NumeroConta,
            fix.LimiteTransacao);

        // Act
        var act = () => limiteCliente.Creditar(valor);

        // Assert
        Assert.Throws<TransactionException>(act);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(12.1231)]
    [InlineData(1000)]
    public void Given_LimiteCliente_When_DebitarCliente_Then_UpdateEntity(decimal valor)
    {
        // Arrange
        var fix = LimiteClienteFixture.LimiteClienteValido("1");
        var limiteCliente = new LimiteClienteEntity(
            fix.Documento,
            fix.NumeroAgencia,
            fix.NumeroConta,
            fix.LimiteTransacao);

        // Act
        limiteCliente.Debitar(valor);

        // Assert
        Assert.Equal(fix.LimiteTransacao - valor, limiteCliente.LimiteTransacao);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    [InlineData(-12.1231)]
    public void Given_LimiteCliente_When_DebitarClienteInvalido_Then_ThrowTransactionException(decimal valor)
    {
        // Arrange
        var fix = LimiteClienteFixture.LimiteClienteValido("1");
        var limiteCliente = new LimiteClienteEntity(
            fix.Documento,
            fix.NumeroAgencia,
            fix.NumeroConta,
            fix.LimiteTransacao);

        // Act
        var act = () => limiteCliente.Debitar(valor);

        // Assert
        Assert.Throws<TransactionException>(act);
    }
}