namespace FraudSys.Domain.UnitTests.Transacao.Validator;

public class TransacaoValidatorFacadeTest
{
    private readonly ITransacaoValidatorFacade _transacaoValidatorFacade;

    public TransacaoValidatorFacadeTest()
    {
        _transacaoValidatorFacade = new TransacaoValidatorFacade();
    }

    [Fact]
    public void Given_TransacaoValidatorFacade_When_Validate_Then_NotThrowException()
    {
        // Arrange
        var pagador = LimiteClienteFixture.LimiteClienteValido("1");
        var recebedor = LimiteClienteFixture.LimiteClienteValido("2");
        var valor = 10.0m;

        // Act
        _transacaoValidatorFacade.Validate(pagador, recebedor, valor);

        // Assert
    }

    [Theory]
    [InlineData("1", "1", 10.0)]
    [InlineData("1", "2", -10.0)]
    public void Given_TransacaoValidatorFacadeInvalidData_When_Validate_Then_ThrowEntityValidationException(
        string pagadorId,
        string recebedorId,
        decimal valor)
    {
        // Arrange
        var pagador = LimiteClienteFixture.LimiteClienteValido(pagadorId);
        var recebedor = LimiteClienteFixture.LimiteClienteValido(recebedorId);

        // Act
        var act = () => _transacaoValidatorFacade.Validate(pagador, recebedor, valor);

        // Assert
        Assert.Throws<EntityValidationException>(act);
    }

    [Fact]
    public void Given_TransacaoValidatorFacade_When_ValidateHydration_Then_NotThrowException()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        var status = (int) StatusTransacao.Aprovada;
        var pagador = LimiteClienteFixture.LimiteClienteValido("1");
        var recebedor = LimiteClienteFixture.LimiteClienteValido("2");
        var valor = 10.0m;
        var dataTransacao = DateTime.Now;

        // Act
        _transacaoValidatorFacade.ValidateHydration(id, status, pagador, recebedor, valor, dataTransacao);

        // Assert
    }

    // uuid, int, string, string, decimal
    [Theory]
    [InlineData("11287361283761", 1, "1", "2", 10.0)]
    [InlineData("5bad710b-1d1a-4754-88be-f9866efd21c7", 4, "1", "2", 10.0)]
    [InlineData("5bad710b-1d1a-4754-88be-f9866efd21c7", 2, "1", "1", 10.0)]
    [InlineData("5bad710b-1d1a-4754-88be-f9866efd21c7", 3, "1", "2", 0)]
    public void Given_TransacaoValidatorFacadeInvalidData_When_ValidateHydration_Then_ThrowEntityHydrationException(
        string id,
        int status,
        string pagadorId,
        string recebedorId,
        decimal valor)
    {
        // Arrange
        var pagador = LimiteClienteFixture.LimiteClienteValido(pagadorId);
        var recebedor = LimiteClienteFixture.LimiteClienteValido(recebedorId);
        var dataTransacao = DateTime.Now;

        // Act
        var act = () => _transacaoValidatorFacade.ValidateHydration(id, status, pagador, recebedor, valor, dataTransacao);

        // Assert
        Assert.Throws<EntityHydrationException>(act);
    }

    [Fact]
    public void Given_TransacaoValidatorFacade_When_ValidateEfetuarTransacao_Then_ThrowEntityHydrationException()
    {
        // Arrange
        var status = StatusTransacao.Pendente;

        // Act
        _transacaoValidatorFacade.ValidateEfetuarTransacao(status);

        // Assert
    }

    [Fact]
    public void Given_TransacaoValidatorFacade_When_StatusTransacaoErrado_Then_ThrowEntityHydrationException()
    {
        // Arrange
        var status = StatusTransacao.Aprovada;

        // Act
        var act = () => _transacaoValidatorFacade.ValidateEfetuarTransacao(status);

        // Assert
        Assert.Throws<EntityValidationException>(act);
    }
}