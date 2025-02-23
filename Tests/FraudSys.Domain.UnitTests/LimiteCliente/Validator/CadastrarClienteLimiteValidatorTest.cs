namespace FraudSys.Domain.UnitTests.LimiteCliente.Validator;

public class CadastrarClienteLimiteValidatorTest
{
    [Fact]
    public void LimiteClienteSuficienteValidator_ValidParameters_ReturnsTrue()
    {
        // Arrange
        var documento = "1";
        var agencia = "1234";
        var conta = "4567-0";

        // Act
        CadastrarLimiteClienteValidator.Validate(documento, agencia, conta);

        // Assert
    }

    [Theory]
    [InlineData(null, "1234", "4567-0")]
    [InlineData("1", null, "4567-0")]
    [InlineData("1", "1234", null)]
    [InlineData(" ", "1234", "4567-0")]
    [InlineData("1", " ", "4567-0")]
    [InlineData("1", "1234", " ")]
    public void LimiteClienteSuficienteValidator_InvalidParameters_ThrowsException(
        string? documento,
        string? agencia,
        string? conta)
    {
        // Arrange

        // Act
        var act = () => CadastrarLimiteClienteValidator.Validate(documento!, agencia!, conta!);

        // Assert
        Assert.Throws<EntityValidationException>(act);
    }
}