namespace FraudSys.Application.UnitTests.Command;

public class RemoverLimiteUseCaseTest
{
    private readonly Mock<IAppLogger<RemoverLimiteUseCase>> _appLogger;
    private readonly Mock<ILimiteClienteRepository> _limiteClienteRepository;
    private readonly Mock<IUnitOfWork> _unitOfWork;

    public RemoverLimiteUseCaseTest()
    {
        _appLogger = new Mock<IAppLogger<RemoverLimiteUseCase>>();
        _limiteClienteRepository = new Mock<ILimiteClienteRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();

        _limiteClienteRepository
            .Setup(x => x.GetByIdAsync("1", It.IsAny<CancellationToken>()))
            .ReturnsAsync(LimiteClienteFixture.LimiteClienteValido("1"));
        _limiteClienteRepository
            .Setup(x => x.UpdateAsync(It.IsAny<LimiteClienteEntity>(), It.IsAny<CancellationToken>()));
    }

    [Fact]
    public async Task Given_ValidInput_When_Execute_Then_ReturnLimiteClienteAtualizado()
    {
        // Arrange
        var removerLimiteUseCase = new RemoverLimiteUseCase(
            _appLogger.Object,
            _limiteClienteRepository.Object,
            _unitOfWork.Object);

        var fix = LimiteClienteFixture.LimiteClienteValido("1");
        var input = new RemoverLimiteInput("1");

        // Act
        var output = await removerLimiteUseCase.Execute(input, CancellationToken.None);

        // Assert
        _limiteClienteRepository.Verify(x => x.GetByIdAsync("1", It.IsAny<CancellationToken>()), Times.Once);
        _limiteClienteRepository
            .Verify(x => x.DeleteAsync("1", It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWork.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);

        Assert.NotNull(output);
        Assert.NotNull(output.Documento);
        Assert.Equal(fix.Documento, output.Documento);
        Assert.Equal(fix.NumeroAgencia, output.NumeroAgencia);
        Assert.Equal(fix.NumeroConta, output.NumeroConta);
        Assert.Equal(fix.LimiteTransacao, output.LimiteTransacao);
    }

    [Fact]
    public async Task Given_InvalidInput_When_Execute_Then_ThrowException()
    {
        // Arrange
        var removerLimiteUseCase = new RemoverLimiteUseCase(
            _appLogger.Object,
            _limiteClienteRepository.Object,
            _unitOfWork.Object);

        var input = new RemoverLimiteInput("2");

        _limiteClienteRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Erro ao criar limite"));

        // Act
        var act = async () => await removerLimiteUseCase.Execute(input, CancellationToken.None);

        // Assert
        await Assert.ThrowsAnyAsync<Exception>(act);

        _limiteClienteRepository.Verify(x => x.GetByIdAsync("2", It.IsAny<CancellationToken>()), Times.Once);
        _limiteClienteRepository.Verify(x => x.DeleteAsync("2", It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWork.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}