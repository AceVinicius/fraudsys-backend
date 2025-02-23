
namespace FraudSys.Domain.UnitTests.LimiteCliente;

public class LimiteClienteTest
{
    private readonly Mock<ILimiteClienteValidatorFacade> _validatorSuccess;
    private readonly Mock<ILimiteClienteValidatorFacade> _validatorFail;
    private readonly Mock<ILimiteClienteValidatorFacade> _validatorFailAtualizacaoLimite;
    private readonly Mock<ILimiteClienteValidatorFacade> _validatorFailCreditar;
    private readonly Mock<ILimiteClienteValidatorFacade> _validatorFailDebitar;

    public LimiteClienteTest()
    {
        _validatorSuccess = new Mock<ILimiteClienteValidatorFacade>();
        _validatorFail = new Mock<ILimiteClienteValidatorFacade>();
        _validatorFailAtualizacaoLimite = new Mock<ILimiteClienteValidatorFacade>();
        _validatorFailCreditar = new Mock<ILimiteClienteValidatorFacade>();
        _validatorFailDebitar = new Mock<ILimiteClienteValidatorFacade>();

        _validatorSuccess
            .Setup(x => x.Validate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>()));
        _validatorSuccess
            .Setup(x => x.ValidateAtualizacaoLimiteCliente(It.IsAny<decimal>()));
        _validatorSuccess
            .Setup(x => x.ValidateDebito(It.IsAny<decimal>(), It.IsAny<decimal>()));
        _validatorSuccess
            .Setup(x => x.ValidateCredito(It.IsAny<decimal>()));
        _validatorSuccess
            .Setup(x => x.ValidateHydration(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>()));

        _validatorFail
            .Setup(x => x.Validate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>()))
            .Throws(new EntityValidationException("Erro de validação"));

        _validatorFailAtualizacaoLimite
            .Setup(x => x.Validate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>()));
        _validatorFailAtualizacaoLimite
            .Setup(x => x.ValidateAtualizacaoLimiteCliente(It.IsAny<decimal>()))
            .Throws(new EntityValidationException("Erro de validação"));

        _validatorFailCreditar
            .Setup(x => x.Validate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>()));
        _validatorFailCreditar
            .Setup(x => x.ValidateCredito(It.IsAny<decimal>()))
            .Throws(new EntityValidationException("Erro de validação"));

        _validatorFailDebitar
            .Setup(x => x.Validate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>()));
        _validatorFailDebitar
            .Setup(x => x.ValidateDebito(It.IsAny<decimal>(), It.IsAny<decimal>()))
            .Throws(new EntityValidationException("Erro de validação"));
    }

    [Fact]
    public void Given_LimiteCliente_When_CriarLimiteCliente_Then_CreateEntity()
    {
        // Arrange
        var fix = LimiteClienteFixture.LimiteClienteValido("1");

        // Act
        var limiteCliente = LimiteClienteEntity.Create(
            _validatorSuccess.Object,
            fix.Documento,
            fix.NumeroAgencia,
            fix.NumeroConta,
            fix.LimiteTransacao);

        // Assert
        Assert.NotNull(limiteCliente);
        Assert.Equal(fix.Documento, limiteCliente.Documento);
        Assert.Equal(fix.NumeroAgencia, limiteCliente.NumeroAgencia);
        Assert.Equal(fix.NumeroConta, limiteCliente.NumeroConta);
        Assert.Equal(fix.LimiteTransacao, limiteCliente.LimiteTransacao);
    }

    [Fact]
    public void Given_LimiteClienteInvalido_When_CriarLimiteCliente_Then_ThrowEntityValidationException()
    {
        // Arrange
        var fix = LimiteClienteFixture.LimiteClienteValido("1");
        // Act
        Action act = () => _ = LimiteClienteEntity.Create(
            _validatorFail.Object,
            fix.Documento,
            fix.NumeroAgencia,
            fix.NumeroConta,
            fix.LimiteTransacao);

        // Assert
        Assert.Throws<EntityValidationException>(act);
    }

    [Fact]
    public void Given_LimiteCliente_When_HydrateLimiteCliente_Then_HydrateEntity()
    {
        // Arrange
        var fix = LimiteClienteFixture.LimiteClienteValido("1");


        // Act
        var hydratedLimiteCliente = LimiteClienteEntity.Hydrate(
            _validatorSuccess.Object,
            fix.Documento,
            fix.NumeroAgencia,
            fix.NumeroConta,
            fix.LimiteTransacao);

        // Assert
        Assert.NotNull(hydratedLimiteCliente);
        Assert.Equal(fix.Documento, hydratedLimiteCliente.Documento);
        Assert.Equal(fix.NumeroAgencia, hydratedLimiteCliente.NumeroAgencia);
        Assert.Equal(fix.NumeroConta, hydratedLimiteCliente.NumeroConta);
        Assert.Equal(fix.LimiteTransacao, hydratedLimiteCliente.LimiteTransacao);
    }

    [Fact]
    public void Given_LimiteCliente_When_HydrateLimiteClienteInvalido_Then_ThrowEntityValidationException()
    {
        // Arrange
        var fix = LimiteClienteFixture.LimiteClienteValido("1");

        // Act
        Action act = () => _ = LimiteClienteEntity.Hydrate(
            _validatorFail.Object,
            fix.Documento,
            fix.NumeroAgencia,
            fix.NumeroConta,
            fix.LimiteTransacao);

        // Assert
        Assert.Throws<EntityValidationException>(act);
    }

    [Fact]
    public void Given_LimiteCliente_When_AtualizarLimiteCliente_Then_UpdateEntity()
    {
        // Arrange
        var limite = 1000;
        var fix = LimiteClienteFixture.LimiteClienteValido("1");
        var limiteCliente = LimiteClienteEntity.Create(
            _validatorSuccess.Object,
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

    [Fact]
    public void Given_LimiteCliente_When_AtualizarLimiteClienteInvalido_Then_ThrowEntityValidationException()
    {
        // Arrange
        var limite = -1;
        var fix = LimiteClienteFixture.LimiteClienteValido("1");
        var limiteCliente = LimiteClienteEntity.Create(
            _validatorFailAtualizacaoLimite.Object,
            fix.Documento,
            fix.NumeroAgencia,
            fix.NumeroConta,
            fix.LimiteTransacao);

        // Act
        var act = () => limiteCliente.AtualizarLimite(limite);

        // Assert
        Assert.Throws<EntityValidationException>(act);
    }

    [Fact]
    public void Given_LimiteCliente_When_CreditarCliente_Then_UpdateEntity()
    {
        // Arrange
        var valor = 1000;
        var fix = LimiteClienteFixture.LimiteClienteValido("1");
        var limiteCliente = LimiteClienteEntity.Create(
            _validatorSuccess.Object,
            fix.Documento,
            fix.NumeroAgencia,
            fix.NumeroConta,
            fix.LimiteTransacao);

        // Act
        limiteCliente.Creditar(valor);

        // Assert
        Assert.Equal(fix.LimiteTransacao + valor, limiteCliente.LimiteTransacao);
    }

    [Fact]
    public void Given_LimiteCliente_When_CreditarClienteInvalido_Then_ThrowTransactionException()
    {
        // Arrange
        var valor = -1;
        var fix = LimiteClienteFixture.LimiteClienteValido("1");
        var limiteCliente = LimiteClienteEntity.Create(
            _validatorFailCreditar.Object,
            fix.Documento,
            fix.NumeroAgencia,
            fix.NumeroConta,
            fix.LimiteTransacao);

        // Act
        var act = () => limiteCliente.Creditar(valor);

        // Assert
        Assert.Throws<TransactionException>(act);
    }

    [Fact]
    public void Given_LimiteCliente_When_DebitarCliente_Then_UpdateEntity()
    {
        // Arrange
        var valor = 1000;
        var fix = LimiteClienteFixture.LimiteClienteValido("1");
        var limiteCliente = LimiteClienteEntity.Create(
            _validatorSuccess.Object,
            fix.Documento,
            fix.NumeroAgencia,
            fix.NumeroConta,
            fix.LimiteTransacao);

        // Act
        limiteCliente.Debitar(valor);

        // Assert
        Assert.Equal(fix.LimiteTransacao - valor, limiteCliente.LimiteTransacao);
    }

    [Fact]
    public void Given_LimiteCliente_When_DebitarClienteInvalido_Then_ThrowTransactionException()
    {
        // Arrange
        var valor = -1;
        var fix = LimiteClienteFixture.LimiteClienteValido("1");
        var limiteCliente = LimiteClienteEntity.Create(
            _validatorFailDebitar.Object,
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