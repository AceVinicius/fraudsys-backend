namespace FraudSys.Domain.UnitTests.Transacao.Validator;

public class TransacaoValidatorTest
{
    [Fact]
    public void Given_Id_When_ValidateId_Then_NotThrowException()
    {
        // Arrange
        var guid = Guid.NewGuid().ToString();

        // Act
        TransacaoValidator.ValidateId(guid);

        // Assert
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("invalid")]
    public void Given_InvalidId_When_ValidateId_Then_ThrowEntityValidationException(string? invalidGuid)
    {
        // Arrange

        // Act
        var act = () => TransacaoValidator.ValidateId(invalidGuid!);

        // Assert
        Assert.Throws<EntityValidationException>(act);
    }

    [Theory]
    [InlineData(StatusTransacao.Pendente)]
    [InlineData(StatusTransacao.Aprovada)]
    [InlineData(StatusTransacao.Rejeitada)]
    public void Given_Status_When_ValidateStatus_Then_NotThrowException(StatusTransacao status)
    {
        // Arrange

        // Act
        TransacaoValidator.ValidateStatus((int) status);

        // Assert
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(4)]
    public void Given_InvalidStatus_When_ValidateStatus_Then_ThrowEntityValidationException(int invalidStatus)
    {
        // Arrange

        // Act
        var act = () => TransacaoValidator.ValidateStatus(invalidStatus);

        // Assert
        Assert.Throws<EntityValidationException>(act);
    }

    [Fact]
    public void Given_LimiteClienteEntity_When_ValidateLimiteClienteEntity_Then_NotThrowException()
    {
        // Arrange
        var limiteCliente = LimiteClienteFixture.LimiteClienteValido("1");

        // Act
        TransacaoValidator.ValidateLimiteClienteEntity(limiteCliente);

        // Assert
    }

    [Fact]
    public void Given_NullLimiteClienteEntity_When_ValidateLimiteClienteEntity_Then_ThrowEntityValidationException()
    {
        // Arrange

        // Act
        var act = () => TransacaoValidator.ValidateLimiteClienteEntity(null!);

        // Assert
        Assert.Throws<EntityValidationException>(act);
    }

    [Fact]
    public void Given_LimiteClienteEntity_When_ValidatePagadorERecebedor_Then_NotThrowException()
    {
        // Arrange
        var pagador = LimiteClienteFixture.LimiteClienteValido("1");
        var recebedor = LimiteClienteFixture.LimiteClienteValido("2");

        // Act
        TransacaoValidator.ValidatePagadorERecebedor(pagador, recebedor);

        // Assert
    }

    [Fact]
    public void Given_SameLimiteClienteEntity_When_ValidatePagadorERecebedor_Then_ThrowEntityValidationException()
    {
        // Arrange
        var limiteCliente = LimiteClienteFixture.LimiteClienteValido("1");

        // Act
        var act = () => TransacaoValidator.ValidatePagadorERecebedor(limiteCliente, limiteCliente);

        // Assert
        Assert.Throws<EntityValidationException>(act);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(12.1231)]
    public void Given_ValidValor_When_ValidadeValor_Then_NotThrowException(decimal valor)
    {
        // Arrange

        // Act
        TransacaoValidator.ValidadeValorTransacao(valor);

        // Assert
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    [InlineData(-12.1231)]
    public void Given_InvalidValor_When_ValidadeValor_Then_ThrowEntityValidationException(decimal invalidValor)
    {
        // Arrange

        // Act
        var act = () => TransacaoValidator.ValidadeValorTransacao(invalidValor);

        // Assert
        Assert.Throws<EntityValidationException>(act);
    }

    [Fact]
    public void Given_StatusTransacao_When_ValidateStatusTransacao_Then_NotThrowException()
    {
        // Arrange
        var status = StatusTransacao.Pendente;

        // Act
        TransacaoValidator.ValidateStatusTransacao(status);

        // Assert
    }

    [Fact]
    public void Given_StatusTransacaoErrado_When_ValidateStatusTransacao_Then_ThrowException()
    {
        // Arrange
        var status = StatusTransacao.Aprovada;

        // Act
        var act = () => TransacaoValidator.ValidateStatusTransacao(status);

        // Assert
        Assert.Throws<EntityValidationException>(act);
    }
}