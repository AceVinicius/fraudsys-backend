namespace FraudSys.Domain.UnitTests.LimiteCliente.Validator;

public class LimiteClienteValidatorFacadeTest
{
    private readonly ILimiteClienteValidatorFacade _limiteClienteValidatorFacade;
    public LimiteClienteValidatorFacadeTest()
    {
        _limiteClienteValidatorFacade = new LimiteClienteValidatorFacade();
    }

    [Fact]
    public void Validate_WhenCalled_ShouldValidate()
    {
        // Arrange
        var documento = "12345678901";
        var numeroAgencia = "1234";
        var numeroConta = "123456";
        var limiteTransacao = 1000m;

        // Act
        _limiteClienteValidatorFacade.Validate(documento, numeroAgencia, numeroConta, limiteTransacao);

        // Assert
    }

    [Fact]
    public void Validate_WhenCalled_ShouldThrowEntityValidationException()
    {
        // Arrange
        var documento = "12345678901";
        var numeroAgencia = "1234";
        var numeroConta = "123456";
        var limiteTransacao = -1000m;

        // Act
        var act = () => _limiteClienteValidatorFacade.Validate(documento, numeroAgencia, numeroConta, limiteTransacao);

        // Assert
        Assert.Throws<EntityValidationException>(act);
    }

    [Fact]
    public void ValidateHydration_WhenCalled_ShouldValidate()
    {
        // Arrange
        var documento = "12345678901";
        var numeroAgencia = "1234";
        var numeroConta = "123456";
        var limiteTransacao = 1000m;

        // Act
        _limiteClienteValidatorFacade.ValidateHydration(documento, numeroAgencia, numeroConta, limiteTransacao);

        // Assert
    }

    [Fact]
    public void ValidateHydration_WhenCalled_ShouldThrowEntityHydrationException()
    {
        // Arrange
        var documento = "12345678901";
        var numeroAgencia = "1234";
        var numeroConta = "123456";
        var limiteTransacao = -1000m;

        // Act
        var act = () => _limiteClienteValidatorFacade.ValidateHydration(documento, numeroAgencia, numeroConta, limiteTransacao);

        // Assert
        Assert.Throws<EntityHydrationException>(act);
    }

    [Fact]
    public void ValidateAtualizacaoLimiteCliente_WhenCalled_ShouldValidate()
    {
        // Arrange
        var limiteCliente = 1000m;

        // Act
        _limiteClienteValidatorFacade.ValidateAtualizacaoLimiteCliente(limiteCliente);

        // Assert
    }

    [Fact]
    public void ValidateAtualizacaoLimiteCliente_WhenCalled_ShouldThrowEntityValidationException()
    {
        // Arrange
        var limiteCliente = -1000m;

        // Act
        var act = () => _limiteClienteValidatorFacade.ValidateAtualizacaoLimiteCliente(limiteCliente);

        // Assert
        Assert.Throws<EntityValidationException>(act);
    }

    [Fact]
    public void ValidateCredito_WhenCalled_ShouldValidate()
    {
        // Arrange
        var valorCredito = 1000m;

        // Act
        _limiteClienteValidatorFacade.ValidateCredito(valorCredito);

        // Assert
    }

    [Fact]
    public void ValidateCredito_WhenCalled_ShouldThrowEntityValidationException()
    {
        // Arrange
        var valorCredito = -1000m;

        // Act
        var act = () => _limiteClienteValidatorFacade.ValidateCredito(valorCredito);

        // Assert
        Assert.Throws<EntityValidationException>(act);
    }

    [Fact]
    public void ValidateDebito_WhenCalled_ShouldValidate()
    {
        // Arrange
        var valorDebito = 1000m;
        var limiteCliente = 1000m;

        // Act
        _limiteClienteValidatorFacade.ValidateDebito(valorDebito, limiteCliente);

        // Assert
    }

    [Fact]
    public void ValidateDebito_WhenCalled_ShouldThrowEntityValidationException()
    {
        // Arrange
        var valorDebito = 1000m;
        var limiteCliente = 500m;

        // Act
        var act = () => _limiteClienteValidatorFacade.ValidateDebito(valorDebito, limiteCliente);

        // Assert
        Assert.Throws<EntityValidationException>(act);
    }
}