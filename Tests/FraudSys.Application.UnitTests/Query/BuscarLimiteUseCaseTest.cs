namespace FraudSys.Application.UnitTests.Query;

public class BuscarLimiteUseCaseTest
{
    private readonly Mock<IAppLogger<BuscarLimiteUseCase>> _appLogger;
    private readonly Mock<ILimiteClienteRepository> _limiteClienteRepositorySuccess;
    private readonly Mock<ILimiteClienteRepository> _limiteClienteRepositoryGetByIdFail;
    private readonly CancellationToken _cancellationToken;

    public BuscarLimiteUseCaseTest()
    {
        _appLogger = new Mock<IAppLogger<BuscarLimiteUseCase>>();
        _limiteClienteRepositorySuccess = new Mock<ILimiteClienteRepository>();
        _limiteClienteRepositoryGetByIdFail = new Mock<ILimiteClienteRepository>();

        _cancellationToken = CancellationToken.None;

        _limiteClienteRepositorySuccess
            .Setup(x => x.GetByIdAsync("1", _cancellationToken))
            .ReturnsAsync(LimiteClienteFixture.LimiteClienteValido("1"));
        _limiteClienteRepositorySuccess
            .Setup(x => x.DeleteAsync(It.IsAny<string>(), _cancellationToken));

        _limiteClienteRepositoryGetByIdFail
            .Setup(x => x.GetByIdAsync("1", _cancellationToken))
            .Throws(new Exception("Erro de repository"));
    }

    [Fact]
    public async Task Given_BuscarLimiteInput_When_Execute_Then_ReturnBuscarLimiteOutput()
    {
        // Arrange
        var input = new BuscarLimiteInput("1");

        var buscarLimiteUseCase = new BuscarLimiteUseCase(
            _appLogger.Object,
            _limiteClienteRepositorySuccess.Object);

        // Act
        var output = await buscarLimiteUseCase.Execute(input, _cancellationToken);

        // Assert
        Assert.NotNull(output);
        Assert.Equal("1", output.Documento);

        _limiteClienteRepositorySuccess
            .Verify(x => x.GetByIdAsync("1", _cancellationToken), Times.Once);
    }

    [Fact]
    public async Task Given_BuscarLimiteInput_When_GetByIdFails_Then_ThrowException()
    {
        // Arrange
        var input = new BuscarLimiteInput("1");

        var buscarLimiteUseCase = new BuscarLimiteUseCase(
            _appLogger.Object,
            _limiteClienteRepositoryGetByIdFail.Object);

        // Act
        var act = async () => _ = await buscarLimiteUseCase.Execute(input, _cancellationToken);

        // Assert
        await Assert.ThrowsAsync<Exception>(act);

        _limiteClienteRepositoryGetByIdFail
            .Verify(x => x.GetByIdAsync("1", _cancellationToken), Times.Once);
    }
}