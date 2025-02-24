using FraudSys.Application.Query.BuscarTodosLimites;

namespace FraudSys.Application.UnitTests.Query;

public class BuscarTodosLimitesUseCaseTest
{
    private readonly Mock<IAppLogger<BuscarTodosLimitesUseCase>> _appLogger;
    private readonly Mock<ILimiteClienteRepository> _limiteClienteRepositorySuccess;
    private readonly Mock<ILimiteClienteRepository> _limiteClienteRepositoryGetAllFail;
    private readonly CancellationToken _cancellationToken;

    public BuscarTodosLimitesUseCaseTest()
    {
        _appLogger = new Mock<IAppLogger<BuscarTodosLimitesUseCase>>();
        _limiteClienteRepositorySuccess = new Mock<ILimiteClienteRepository>();
        _limiteClienteRepositoryGetAllFail = new Mock<ILimiteClienteRepository>();

        _cancellationToken = CancellationToken.None;

        _limiteClienteRepositorySuccess
            .Setup(x => x.GetAllAsync(_cancellationToken))
            .ReturnsAsync(LimiteClienteFixture.LimiteClienteValidos(4));

        _limiteClienteRepositoryGetAllFail
            .Setup(x => x.GetAllAsync(_cancellationToken))
            .Throws(new Exception("Erro de repository"));
    }

    [Fact]
    public async Task Given_BuscarTodosLimitesInput_When_Execute_Then_ReturnBuscarTodosLimitesOutput()
    {
        // Arrange
        var input = new BuscarTodosLimitesInput();

        var buscarTodosLimitesUseCase = new BuscarTodosLimitesUseCase(
            _appLogger.Object,
            _limiteClienteRepositorySuccess.Object);

        // Act
        var output = await buscarTodosLimitesUseCase.Execute(input, _cancellationToken);

        // Assert
        Assert.NotNull(output);
        Assert.Equal(4, output.data.Count());

        _limiteClienteRepositorySuccess
            .Verify(x => x.GetAllAsync(_cancellationToken), Times.Once);
    }

    [Fact]
    public async Task Given_BuscarTodosLimitesInput_When_GetAllFails_Then_ThrowException()
    {
        // Arrange
        var input = new BuscarTodosLimitesInput();

        var buscarTodosLimitesUseCase = new BuscarTodosLimitesUseCase(
            _appLogger.Object,
            _limiteClienteRepositoryGetAllFail.Object);

        // Act
        var act = async () => _ = await buscarTodosLimitesUseCase.Execute(input, _cancellationToken);

        // Assert
        await Assert.ThrowsAsync<Exception>(act);

        _limiteClienteRepositoryGetAllFail
            .Verify(x => x.GetAllAsync(_cancellationToken), Times.Once);
    }
}