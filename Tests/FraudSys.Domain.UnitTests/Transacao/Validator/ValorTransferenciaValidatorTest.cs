namespace FraudSys.Domain.UnitTests.Transacao.Validator;

public class ValorTransferenciaValidatorTest
{
    [Theory]
    [InlineData(1)]
    [InlineData(100.1283)]
    public void Given_ValorTransferencia_When_ValorTransferenciaIsValid_Then_ReturnsVoid(decimal valor)
    {
        // Arrange

        // Act
        ValorTransferenciaValidator.Validate(valor);

        // Assert
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100.1283)]
    public void
    Given_ValorTransferencia_When_ValorTransferenciaIsInvalid_Then_ThrowsEntityValidationException(decimal valor)
    {
        // Arrange

        // Act
        Action act = () => ValorTransferenciaValidator.Validate(valor);

        // Assert
        Assert.Throws<EntityValidationException>(act);
    }
}