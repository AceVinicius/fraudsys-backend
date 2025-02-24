namespace FraudSys.Application.UnitTests.Command;

public class AtualizarLimiteUseCaseTest
{
    private readonly Mock<IAppLogger<AtualizarLimiteUseCase>> _appLogger;
    private readonly Mock<ILimiteClienteRepository> _limiteClienteRepositorySuccess;
    private readonly Mock<ILimiteClienteRepository> _limiteClienteRepositoryGetByIdFail;
    private readonly Mock<ILimiteClienteRepository> _limiteClienteRepositoryUpdateFail;
    private readonly Mock<IUnitOfWork> _unitOfWorkSuccess;
    private readonly Mock<IUnitOfWork> _unitOfWorkFail;
    private readonly CancellationToken _cancellationToken;

    public AtualizarLimiteUseCaseTest()
    {
        _appLogger = new Mock<IAppLogger<AtualizarLimiteUseCase>>();
        _limiteClienteRepositorySuccess = new Mock<ILimiteClienteRepository>();
        _limiteClienteRepositoryGetByIdFail = new Mock<ILimiteClienteRepository>();
        _limiteClienteRepositoryUpdateFail = new Mock<ILimiteClienteRepository>();
        _unitOfWorkSuccess = new Mock<IUnitOfWork>();
        _unitOfWorkFail = new Mock<IUnitOfWork>();

        _cancellationToken = CancellationToken.None;

        _limiteClienteRepositorySuccess
            .Setup(x => x.GetByIdAsync("1", _cancellationToken))
            .ReturnsAsync(LimiteClienteFixture.LimiteClienteValido("1"));
        _limiteClienteRepositorySuccess
            .Setup(x => x.UpdateAsync(It.IsAny<LimiteClienteEntity>(), _cancellationToken));

        _limiteClienteRepositoryGetByIdFail
            .Setup(x => x.GetByIdAsync("1", _cancellationToken))
            .Throws(new Exception("Erro de repository"));

        _limiteClienteRepositoryUpdateFail
            .Setup(x => x.GetByIdAsync("1", _cancellationToken))
            .ReturnsAsync(LimiteClienteFixture.LimiteClienteValido("1"));
        _limiteClienteRepositoryUpdateFail
            .Setup(x => x.UpdateAsync(It.IsAny<LimiteClienteEntity>(), _cancellationToken))
            .Throws(new Exception("Erro de repository"));

        _unitOfWorkSuccess
            .Setup(x => x.CommitAsync(_cancellationToken));

        _unitOfWorkFail
            .Setup(x => x.CommitAsync(_cancellationToken))
            .Throws(new Exception("Erro de unitOfWork"));
    }

    [Fact]
    public async Task Given_AtualizarLimiteInput_When_Execute_Then_ReturnAtualizarLimiteOutput()
    {
        // Arrange
        var input = new AtualizarLimiteInput("1", 500);
        var fix = LimiteClienteFixture.LimiteClienteValido("1");

        var atualizarLimiteUseCase = new AtualizarLimiteUseCase(
            _appLogger.Object,
            _limiteClienteRepositorySuccess.Object,
            _unitOfWorkSuccess.Object);

        // Act
        var output = await atualizarLimiteUseCase.Execute(input, _cancellationToken);

        // Assert
        Assert.NotNull(output);
        Assert.Equal(input.Documento, output.Documento);
        Assert.Equal(fix.NumeroAgencia, output.NumeroAgencia);
        Assert.Equal(fix.NumeroConta, output.NumeroConta);
        Assert.Equal(input.NovoLimite, output.LimiteTransacao);

        _limiteClienteRepositorySuccess
            .Verify(x => x.GetByIdAsync("1", _cancellationToken), Times.Once);
        _limiteClienteRepositorySuccess
            .Verify(x => x.UpdateAsync(It.IsAny<LimiteClienteEntity>(), _cancellationToken), Times.Once);

        _unitOfWorkSuccess
            .Verify(x => x.CommitAsync(_cancellationToken), Times.Once);
    }

    [Fact]
    public async Task Given_AtualizarLimiteInput_When_GetByIdNotFound_Then_ThrowException()
    {
        // Arrange
        var input = new AtualizarLimiteInput("1", 500);

        var atualizarLimiteUseCase = new AtualizarLimiteUseCase(
            _appLogger.Object,
            _limiteClienteRepositoryGetByIdFail.Object,
            _unitOfWorkSuccess.Object);

        // Act
        var act = async () => await atualizarLimiteUseCase.Execute(input, _cancellationToken);

        // Assert
        await Assert.ThrowsAnyAsync<Exception>(act);

        _limiteClienteRepositoryGetByIdFail
            .Verify(x => x.GetByIdAsync("1", _cancellationToken), Times.Once);
        _limiteClienteRepositoryGetByIdFail
            .Verify(x => x.UpdateAsync(It.IsAny<LimiteClienteEntity>(), _cancellationToken), Times.Never);

        _unitOfWorkSuccess
            .Verify(x => x.CommitAsync(_cancellationToken), Times.Never);
    }

    [Fact]
    public async Task Given_AtualizarLimiteInput_When_UpdateFails_Then_ThrowException()
    {
        // Arrange
        var input = new AtualizarLimiteInput("1", 500);

        var atualizarLimiteUseCase = new AtualizarLimiteUseCase(
            _appLogger.Object,
            _limiteClienteRepositoryUpdateFail.Object,
            _unitOfWorkSuccess.Object);

        // Act
        var act = async () => await atualizarLimiteUseCase.Execute(input, _cancellationToken);

        // Assert
        await Assert.ThrowsAnyAsync<Exception>(act);

        _limiteClienteRepositoryUpdateFail
            .Verify(x => x.GetByIdAsync("1", _cancellationToken), Times.Once);
        _limiteClienteRepositoryUpdateFail
            .Verify(x => x.UpdateAsync(It.IsAny<LimiteClienteEntity>(), _cancellationToken), Times.Once);

        _unitOfWorkSuccess
            .Verify(x => x.CommitAsync(_cancellationToken), Times.Never);
    }

    [Fact]
    public async Task Given_AtualizarLimiteInput_When_CommitFails_Then_ThrowException()
    {
        // Arrange
        var input = new AtualizarLimiteInput("1", 500);

        var atualizarLimiteUseCase = new AtualizarLimiteUseCase(
            _appLogger.Object,
            _limiteClienteRepositorySuccess.Object,
            _unitOfWorkFail.Object);

        // Act
        var act = async () => await atualizarLimiteUseCase.Execute(input, _cancellationToken);

        // Assert
        await Assert.ThrowsAnyAsync<Exception>(act);

        _limiteClienteRepositorySuccess
            .Verify(x => x.GetByIdAsync("1", _cancellationToken), Times.Once);
        _limiteClienteRepositorySuccess
            .Verify(x => x.UpdateAsync(It.IsAny<LimiteClienteEntity>(), _cancellationToken), Times.Once);

        _unitOfWorkFail
            .Verify(x => x.CommitAsync(_cancellationToken), Times.Once);
    }
}