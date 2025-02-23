namespace FraudSys.Domain.UnitTests.Transacao.Validator;

public class TransacaoValidatorFacadeTest
{
    [Fact]
    public void Given_TransacaoValidatorFacade_When_Validate_Then_NotThrowException()
    {
        // Arrange
        var pagador = LimiteClienteFixture.LimiteClienteValido("1");
        var recebedor = LimiteClienteFixture.LimiteClienteValido("2");
        var valor = 10.0m;

        var transacaoValidatorFacade = new TransacaoValidatorFacade();

        // Act
        transacaoValidatorFacade.Validate(pagador, recebedor, valor);

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

        var transacaoValidatorFacade = new TransacaoValidatorFacade();

        // Act
        var act = () => transacaoValidatorFacade.Validate(pagador, recebedor, valor);

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

        var transacaoValidatorFacade = new TransacaoValidatorFacade();

        // Act
        transacaoValidatorFacade.ValidateHydration(id, status, pagador, recebedor, valor, dataTransacao);

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

        var transacaoValidatorFacade = new TransacaoValidatorFacade();

        // Act
        var act = () => transacaoValidatorFacade.ValidateHydration(id, status, pagador, recebedor, valor, dataTransacao);

        // Assert
        Assert.Throws<EntityHydrationException>(act);
    }
}