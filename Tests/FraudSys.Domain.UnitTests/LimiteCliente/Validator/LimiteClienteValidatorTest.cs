namespace FraudSys.Domain.UnitTests.LimiteCliente.Validator;

public class LimiteClienteValidatorTest
{
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(100.12371)]
    public void Given_LimiteCliente_When_ValidarLimiteCliente_Then_ValidateEntity(decimal limite)
    {
        // Arrange

        // Act
        LimiteClienteValidator.Validate(limite);

        // Assert
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-1.12973129)]
    public void Given_LimiteClienteInvalido_When_ValidarLimiteCliente_Then_ThrowValidationException(
        decimal limite)
    {
        // Arrange

        // Act
        var act = () => LimiteClienteValidator.Validate(limite);

        // Assert
        Assert.Throws<EntityValidationException>(act);
    }
}