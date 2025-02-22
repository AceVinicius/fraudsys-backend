namespace FraudSys.Application.UnitTests.Query;

public class BuscarLimiteUseCaseTest
{
    private readonly Mock<IAppLogger<BuscarLimiteUseCase>> _appLogger;
    private readonly Mock<ILimiteClienteRepository> _limiteClienteRepository;

    public BuscarLimiteUseCaseTest()
    {
        _appLogger = new Mock<IAppLogger<BuscarLimiteUseCase>>();
        _limiteClienteRepository = new Mock<ILimiteClienteRepository>();

        _limiteClienteRepository
            .Setup(x => x.GetByIdAsync("1", It.IsAny<CancellationToken>()))
            .ReturnsAsync(LimiteClienteFixture.LimiteClienteValido("1"));
    }

    [Fact]
    public async Task Given_ValidInput_When_Execute_Then_ReturnLimiteCliente()
    {
        // Arrange
        var buscarLimiteUseCase = new BuscarLimiteUseCase(
            _appLogger.Object,
            _limiteClienteRepository.Object);

        var input = new BuscarLimiteInput("1");

        // Act
        var output = await buscarLimiteUseCase.Execute(input, CancellationToken.None);

        // Assert
        _limiteClienteRepository.Verify(x => x.GetByIdAsync("1", It.IsAny<CancellationToken>()), Times.Once);

        Assert.NotNull(output);
        Assert.NotNull(output.Documento);
        Assert.Equal("1", output.Documento);
    }

    [Fact]
    public async Task Given_InvalidInput_When_Execute_Then_ThrowNotFoundException()
    {
        // Arrange
        var buscarLimiteUseCase = new BuscarLimiteUseCase(
            _appLogger.Object,
            _limiteClienteRepository.Object);

        var input = new BuscarLimiteInput("2");

        // Act
        var act  = async () => await buscarLimiteUseCase.Execute(input, CancellationToken.None);

        // Assert
        await Assert.ThrowsAnyAsync<Exception>(act);

        _limiteClienteRepository.Verify(x => x.GetByIdAsync("2", It.IsAny<CancellationToken>()), Times.Once);
    }
}