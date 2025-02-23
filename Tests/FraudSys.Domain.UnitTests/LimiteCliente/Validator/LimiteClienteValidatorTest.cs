namespace FraudSys.Domain.UnitTests.LimiteCliente.Validator;

public class LimiteClienteValidatorTest
{
    [Theory]
    [InlineData("a")]
    [InlineData("Teste")]
    [InlineData("Teste 123")]
    public void Given_String_When_ValidateEmptyString_Then_NotThrowException(string value)
    {
        // Arrange

        // Act
        LimiteClienteValidator.ValidateEmptyString(value);

        // Assert
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Given_EmptyString_When_ValidateEmptyString_Then_ThrowEntityValidationException(
        string? value)
    {
        // Arrange

        // Act
        var act = () => LimiteClienteValidator.ValidateEmptyString(value!);

        // Assert
        Assert.Throws<EntityValidationException>(act);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(100.12371)]
    public void Given_LimiteCliente_When_ValidateLimiteCliente_Then_NotThrowException(
        decimal limite)
    {
        // Arrange

        // Act
        LimiteClienteValidator.ValidateLimiteCliente(limite);

        // Assert
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-1.12973129)]
    public void Given_LimiteClienteInvalido_When_ValidarLimiteCliente_Then_ThrowEntityValidationException(
        decimal limite)
    {
        // Arrange

        // Act
        var act = () => LimiteClienteValidator.ValidateLimiteCliente(limite);

        // Assert
        Assert.Throws<EntityValidationException>(act);
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 2)]
    [InlineData(10, 100)]
    [InlineData(100.12371, 100.12372)]
    public void Given_ValorDebitoMenorQueLimiteCliente_When_LimiteClienteSuficiente_Then_NotThrowException(
        decimal valorDebito,
        decimal limiteCliente)
    {
        // Arrange

        // Act
        LimiteClienteValidator.LimiteClienteSuficiente(valorDebito, limiteCliente);

        // Assert
    }

    [Theory]
    [InlineData(1, 0)]
    [InlineData(2, 1)]
    [InlineData(100, 10)]
    [InlineData(100.12372, 100.12371)]
    public void Given_ValorDebitoMaiorQueLimiteCliente_When_LimiteClienteSuficiente_Then_ThrowTransactionException(
        decimal valorDebito,
        decimal limiteCliente)
    {
        // Arrange

        // Act
        var act = () => LimiteClienteValidator.LimiteClienteSuficiente(valorDebito, limiteCliente);

        // Assert
        Assert.Throws<EntityValidationException>(act);
    }
}