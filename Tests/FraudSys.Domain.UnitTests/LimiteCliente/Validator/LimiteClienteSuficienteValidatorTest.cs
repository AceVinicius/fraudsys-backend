namespace FraudSys.Domain.UnitTests.LimiteCliente.Validator;

public class LimiteClienteSuficienteValidatorTest
{
    [Theory]
    [InlineData(1)]
    [InlineData(500)]
    [InlineData(1000)]
    public void DeveRetornarErroQuandoLimiteClienteNaoForSuficiente(decimal transferencia)
    {
        // Arrange
        var limiteCliente = LimiteClienteFixture.LimiteClienteValido("1");

        // Act
        LimiteClienteSuficienteValidator.Validate(transferencia, limiteCliente);
    }

    [Theory]
    [InlineData(1001)]
    [InlineData(2000)]
    [InlineData(3000)]
    public void DeveRetornarSucessoQuandoLimiteClienteForSuficiente(decimal transferencia)
    {
        // Arrange
        var limiteCliente = LimiteClienteFixture.LimiteClienteValido("1");

        // Act
        var act = () => LimiteClienteSuficienteValidator.Validate(transferencia, limiteCliente);

        // Assert
        Assert.Throws<EntityValidationException>(act);
    }
}