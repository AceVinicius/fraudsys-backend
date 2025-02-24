namespace FraudSys.Application.UnitTests.Command;

public class RemoverLimiteUseCaseTest
{
    private readonly Mock<IAppLogger<RemoverLimiteUseCase>> _appLogger;
    private readonly Mock<ILimiteClienteRepository> _limiteClienteRepositorySuccess;
    private readonly Mock<ILimiteClienteRepository> _limiteClienteRepositoryGetByIdFail;
    private readonly Mock<ILimiteClienteRepository> _limiteClienteRepositoryDeleteFail;
    private readonly Mock<IUnitOfWork> _unitOfWorkSuccess;
    private readonly Mock<IUnitOfWork> _unitOfWorkFail;
    private readonly CancellationToken _cancellationToken;

    public RemoverLimiteUseCaseTest()
    {
        _appLogger = new Mock<IAppLogger<RemoverLimiteUseCase>>();
        _limiteClienteRepositorySuccess = new Mock<ILimiteClienteRepository>();
        _limiteClienteRepositoryGetByIdFail = new Mock<ILimiteClienteRepository>();
        _limiteClienteRepositoryDeleteFail = new Mock<ILimiteClienteRepository>();
        _unitOfWorkSuccess = new Mock<IUnitOfWork>();
        _unitOfWorkFail = new Mock<IUnitOfWork>();

        _cancellationToken = CancellationToken.None;

        _limiteClienteRepositorySuccess
            .Setup(x => x.GetByIdAsync("1", _cancellationToken))
            .ReturnsAsync(LimiteClienteFixture.LimiteClienteValido("1"));
        _limiteClienteRepositorySuccess
            .Setup(x => x.DeleteAsync(It.IsAny<string>(), _cancellationToken));

        _limiteClienteRepositoryGetByIdFail
            .Setup(x => x.GetByIdAsync("1", _cancellationToken))
            .Throws(new Exception("Erro de repository"));

        _limiteClienteRepositoryDeleteFail
            .Setup(x => x.GetByIdAsync("1", _cancellationToken))
            .ReturnsAsync(LimiteClienteFixture.LimiteClienteValido("1"));
        _limiteClienteRepositoryDeleteFail
            .Setup(x => x.DeleteAsync(It.IsAny<string>(), _cancellationToken))
            .Throws(new Exception("Erro de repository"));

        _unitOfWorkSuccess
            .Setup(x => x.CommitAsync(_cancellationToken));

        _unitOfWorkFail
            .Setup(x => x.CommitAsync(_cancellationToken))
            .Throws(new Exception("Erro de unitOfWork"));
    }

    [Fact]
    public async Task Given_RemoverLimiteInput_When_Execute_Then_ReturnRemoverLimiteOutput()
    {
        // Arrange
        var input = new RemoverLimiteInput("1");

        var removerLimiteUseCase = new RemoverLimiteUseCase(
            _appLogger.Object,
            _limiteClienteRepositorySuccess.Object,
            _unitOfWorkSuccess.Object);

        // Act
        var output = await removerLimiteUseCase.Execute(input, _cancellationToken);

        // Assert
        Assert.NotNull(output);
        Assert.Equal(input.Documento, output.Documento);

        _limiteClienteRepositorySuccess
            .Verify(x => x.GetByIdAsync("1", _cancellationToken), Times.Once);
        _limiteClienteRepositorySuccess
            .Verify(x => x.DeleteAsync(It.IsAny<string>(), _cancellationToken), Times.Once);

        _unitOfWorkSuccess
            .Verify(x => x.CommitAsync(_cancellationToken), Times.Once);
    }

    [Fact]
    public async Task Given_RemoverLimiteInput_When_GetByIdFails_Then_ThrowException()
    {
        // Arrange
        var input = new RemoverLimiteInput("1");

        var removerLimiteUseCase = new RemoverLimiteUseCase(
            _appLogger.Object,
            _limiteClienteRepositoryGetByIdFail.Object,
            _unitOfWorkSuccess.Object);

        // Act
        var act = async () => await removerLimiteUseCase.Execute(input, _cancellationToken);

        // Assert
        await Assert.ThrowsAsync<Exception>(act);

        _limiteClienteRepositoryGetByIdFail
            .Verify(x => x.GetByIdAsync("1", _cancellationToken), Times.Once);
        _limiteClienteRepositoryGetByIdFail
            .Verify(x => x.DeleteAsync(It.IsAny<string>(), _cancellationToken), Times.Never);

        _unitOfWorkSuccess
            .Verify(x => x.CommitAsync(_cancellationToken), Times.Never);
    }

    [Fact]
    public async Task Given_RemoverLimiteInput_When_DeleteFails_Then_ThrowException()
    {
        // Arrange
        var input = new RemoverLimiteInput("1");

        var removerLimiteUseCase = new RemoverLimiteUseCase(
            _appLogger.Object,
            _limiteClienteRepositoryDeleteFail.Object,
            _unitOfWorkSuccess.Object);

        // Act
        var act = async () => await removerLimiteUseCase.Execute(input, _cancellationToken);

        // Assert
        await Assert.ThrowsAsync<Exception>(act);

        _limiteClienteRepositoryDeleteFail
            .Verify(x => x.GetByIdAsync("1", _cancellationToken), Times.Once);
        _limiteClienteRepositoryDeleteFail
            .Verify(x => x.DeleteAsync(It.IsAny<string>(), _cancellationToken), Times.Once);

        _unitOfWorkSuccess
            .Verify(x => x.CommitAsync(_cancellationToken), Times.Never);
    }

    [Fact]
    public async Task Given_RemoverLimiteInput_When_CommitFails_Then_ThrowException()
    {
        // Arrange
        var input = new RemoverLimiteInput("1");

        var removerLimiteUseCase = new RemoverLimiteUseCase(
            _appLogger.Object,
            _limiteClienteRepositorySuccess.Object,
            _unitOfWorkFail.Object);

        // Act
        var act = async () => await removerLimiteUseCase.Execute(input, _cancellationToken);

        // Assert
        await Assert.ThrowsAsync<Exception>(act);

        _limiteClienteRepositorySuccess
            .Verify(x => x.GetByIdAsync("1", _cancellationToken), Times.Once);
        _limiteClienteRepositorySuccess
            .Verify(x => x.DeleteAsync(It.IsAny<string>(), _cancellationToken), Times.Once);

        _unitOfWorkFail
            .Verify(x => x.CommitAsync(_cancellationToken), Times.Once);
    }
}