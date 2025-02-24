namespace FraudSys.Application.UnitTests.Command;

public class EfetuarTransacaoUseCaseTest
{
    private readonly Mock<IAppLogger<EfetuarTransacaoUseCase>> _appLogger;
    private readonly Mock<ITransacaoValidatorFacade> _transacaoValidatorSuccess;
    private readonly Mock<ITransacaoValidatorFacade> _transacaoValidatorValidateFail;
    private readonly Mock<ITransacaoValidatorFacade> _transacaoValidatorValidateEfetuarTransacaoFail;
    private readonly Mock<ITransacaoRepository> _transacaoRepositorySuccess;
    private readonly Mock<ITransacaoRepository> _transacaoRepositoryCreateFail;
    private readonly Mock<ITransacaoRepository> _transacaoRepositoryUpdateStatusFail;
    private readonly Mock<ILimiteClienteRepository> _limiteClienteRepositorySuccess;
    private readonly Mock<ILimiteClienteRepository> _limiteClienteRepositoryGetByIdFail;
    private readonly Mock<ILimiteClienteRepository> _limiteClienteRepositoryTransferirFail;
    private readonly Mock<IUnitOfWork> _unitOfWorkSuccess;
    private readonly Mock<IUnitOfWork> _unitOfWorkFail;
    private readonly CancellationToken _cancellationToken;

    public EfetuarTransacaoUseCaseTest()
    {
        _appLogger = new Mock<IAppLogger<EfetuarTransacaoUseCase>>();
        _transacaoValidatorSuccess = new Mock<ITransacaoValidatorFacade>();
        _transacaoValidatorValidateFail = new Mock<ITransacaoValidatorFacade>();
        _transacaoValidatorValidateEfetuarTransacaoFail = new Mock<ITransacaoValidatorFacade>();
        _transacaoRepositorySuccess = new Mock<ITransacaoRepository>();
        _transacaoRepositoryCreateFail = new Mock<ITransacaoRepository>();
        _transacaoRepositoryUpdateStatusFail = new Mock<ITransacaoRepository>();
        _limiteClienteRepositorySuccess = new Mock<ILimiteClienteRepository>();
        _limiteClienteRepositoryGetByIdFail = new Mock<ILimiteClienteRepository>();
        _limiteClienteRepositoryTransferirFail = new Mock<ILimiteClienteRepository>();
        _unitOfWorkSuccess = new Mock<IUnitOfWork>();
        _unitOfWorkFail = new Mock<IUnitOfWork>();

        _cancellationToken = CancellationToken.None;

        _transacaoValidatorSuccess
            .Setup(x => x.Validate(It.IsAny<LimiteClienteEntity>(), It.IsAny<LimiteClienteEntity>(), It.IsAny<decimal>()));
        _transacaoValidatorSuccess
            .Setup(x => x.ValidateEfetuarTransacao(It.IsAny<StatusTransacao>()));

        _transacaoValidatorValidateFail
            .Setup(x => x.Validate(It.IsAny<LimiteClienteEntity>(), It.IsAny<LimiteClienteEntity>(), It.IsAny<decimal>()))
            .Throws(new EntityValidationException("Erro de validação"));

        _transacaoValidatorValidateEfetuarTransacaoFail
            .Setup(x => x.Validate(It.IsAny<LimiteClienteEntity>(), It.IsAny<LimiteClienteEntity>(), It.IsAny<decimal>()));
        _transacaoValidatorValidateEfetuarTransacaoFail
            .Setup(x => x.ValidateEfetuarTransacao(It.IsAny<StatusTransacao>()))
            .Throws(new EntityValidationException("Erro de validação"));

        _transacaoRepositorySuccess
            .Setup(x => x.CreateAsync(It.IsAny<TransacaoEntity>(), _cancellationToken));
        _transacaoRepositorySuccess
            .Setup(x => x.UpdateStatusAsync(It.IsAny<TransacaoEntity>(), _cancellationToken));

        _transacaoRepositoryCreateFail
            .Setup(x => x.CreateAsync(It.IsAny<TransacaoEntity>(), _cancellationToken))
            .ThrowsAsync(new Exception("Erro ao criar transacao"));

        _transacaoRepositoryUpdateStatusFail
            .Setup(x => x.CreateAsync(It.IsAny<TransacaoEntity>(), _cancellationToken));
        _transacaoRepositoryUpdateStatusFail
            .Setup(x => x.UpdateStatusAsync(It.IsAny<TransacaoEntity>(), _cancellationToken))
            .ThrowsAsync(new Exception("Erro ao atualizar status da transacao"));

        _limiteClienteRepositorySuccess
            .Setup(x => x.GetByIdAsync("1", _cancellationToken))
            .ReturnsAsync(LimiteClienteFixture.LimiteClienteValido("1"));
        _limiteClienteRepositorySuccess
            .Setup(x => x.GetByIdAsync("2", _cancellationToken))
            .ReturnsAsync(LimiteClienteFixture.LimiteClienteValido("2"));
        _limiteClienteRepositorySuccess
            .Setup<Task>(x => x.TransferirAsync(It.IsAny<LimiteClienteEntity>(), It.IsAny<LimiteClienteEntity>(), It.IsAny<TransacaoEntity>(), _cancellationToken));

        _limiteClienteRepositoryGetByIdFail
            .Setup(x => x.GetByIdAsync("1", _cancellationToken))
            .ThrowsAsync(new Exception("Erro ao criar limite"));

        _limiteClienteRepositoryTransferirFail
            .Setup(x => x.GetByIdAsync("1", _cancellationToken))
            .ReturnsAsync(LimiteClienteFixture.LimiteClienteValido("1"));
        _limiteClienteRepositoryTransferirFail
            .Setup(x => x.GetByIdAsync("2", _cancellationToken))
            .ReturnsAsync(LimiteClienteFixture.LimiteClienteValido("2"));
        _limiteClienteRepositoryTransferirFail
            .Setup(x => x.TransferirAsync(It.IsAny<LimiteClienteEntity>(), It.IsAny<LimiteClienteEntity>(), It.IsAny<TransacaoEntity>(), _cancellationToken))
            .ThrowsAsync(new Exception("Erro ao transferir limite"));

        _unitOfWorkSuccess
            .Setup(x => x.CommitAsync(_cancellationToken));

        _unitOfWorkFail
            .Setup(x => x.CommitAsync(_cancellationToken))
            .ThrowsAsync(new Exception("Erro de commit"));
    }

    [Fact]
    public async Task Given_EfetuarTransacaoInput_When_ExecuteComSaldo_Then_ReturnEfetuarTransacaoOutputAprovada()
    {
        // Arrange
        var input = new EfetuarTransacaoInput("1", "2", 1000);

        var efetuarTransacaoUseCase = new EfetuarTransacaoUseCase(
            _appLogger.Object,
            _transacaoValidatorSuccess.Object,
            _transacaoRepositorySuccess.Object,
            _limiteClienteRepositorySuccess.Object,
            _unitOfWorkSuccess.Object);

        // Act
        var output = await efetuarTransacaoUseCase.Execute(input, _cancellationToken);

        // Assert
        Assert.NotNull(output);
        Assert.Equal(StatusTransacao.Aprovada, output.Status);

        _transacaoValidatorSuccess
            .Verify(x => x.Validate(It.IsAny<LimiteClienteEntity>(), It.IsAny<LimiteClienteEntity>(), It.IsAny<decimal>()), Times.Once);
        _transacaoValidatorSuccess
            .Verify(x => x.ValidateEfetuarTransacao(StatusTransacao.Pendente), Times.Once);

        _limiteClienteRepositorySuccess
            .Verify(x => x.GetByIdAsync("1", _cancellationToken), Times.Once);
        _limiteClienteRepositorySuccess
            .Verify(x => x.GetByIdAsync("2", _cancellationToken), Times.Once);
        _limiteClienteRepositorySuccess
            .Verify(x => x.TransferirAsync(It.IsAny<LimiteClienteEntity>(), It.IsAny<LimiteClienteEntity>(), It.IsAny<TransacaoEntity>(), _cancellationToken), Times.Once);

        _transacaoRepositorySuccess
            .Verify(x => x.CreateAsync(It.IsAny<TransacaoEntity>(), _cancellationToken), Times.Once);
        _transacaoRepositorySuccess
            .Verify(x => x.UpdateStatusAsync(It.IsAny<TransacaoEntity>(), _cancellationToken), Times.Once);

        _unitOfWorkSuccess.Verify(x => x.CommitAsync(_cancellationToken), Times.Exactly(2));
    }

    [Fact]
    public async Task Given_EfetuarTransacaoInput_When_ExecuteSemSaldo_Then_ReturnEfetuarTransacaoOutputRejeitada()
    {
        // Arrange
        var input = new EfetuarTransacaoInput("1", "2", 10000);

        var efetuarTransacaoUseCase = new EfetuarTransacaoUseCase(
            _appLogger.Object,
            _transacaoValidatorSuccess.Object,
            _transacaoRepositorySuccess.Object,
            _limiteClienteRepositorySuccess.Object,
            _unitOfWorkSuccess.Object);

        // Act
        var output = await efetuarTransacaoUseCase.Execute(input, _cancellationToken);

        // Assert
        Assert.NotNull(output);
        Assert.Equal(StatusTransacao.Rejeitada, output.Status);

        _transacaoValidatorSuccess
            .Verify(x => x.Validate(It.IsAny<LimiteClienteEntity>(), It.IsAny<LimiteClienteEntity>(), It.IsAny<decimal>()), Times.Once);
        _transacaoValidatorSuccess
            .Verify(x => x.ValidateEfetuarTransacao(StatusTransacao.Pendente), Times.Once);

        _limiteClienteRepositorySuccess
            .Verify(x => x.GetByIdAsync("1", _cancellationToken), Times.Once);
        _limiteClienteRepositorySuccess
            .Verify(x => x.GetByIdAsync("2", _cancellationToken), Times.Once);
        _limiteClienteRepositorySuccess
            .Verify(x => x.TransferirAsync(It.IsAny<LimiteClienteEntity>(), It
                .IsAny<LimiteClienteEntity>(), It.IsAny<TransacaoEntity>(), _cancellationToken),
            Times.Never);

        _transacaoRepositorySuccess
            .Verify(x => x.CreateAsync(It.IsAny<TransacaoEntity>(), _cancellationToken), Times.Once);
        _transacaoRepositorySuccess
            .Verify(x => x.UpdateStatusAsync(It.IsAny<TransacaoEntity>(), _cancellationToken), Times.Once);

        _unitOfWorkSuccess.Verify(x => x.CommitAsync(_cancellationToken), Times.Exactly(2));
    }

    [Fact]
    public async Task Given_EfetuarTransacaoInput_When_GetByIdFails_Then_ThrowException()
    {
        // Arrange
        var input = new EfetuarTransacaoInput("1", "2", 1000);

        var efetuarTransacaoUseCase = new EfetuarTransacaoUseCase(
            _appLogger.Object,
            _transacaoValidatorSuccess.Object,
            _transacaoRepositorySuccess.Object,
            _limiteClienteRepositoryGetByIdFail.Object,
            _unitOfWorkSuccess.Object);

        // Act
        var act = async () => await efetuarTransacaoUseCase.Execute(input, _cancellationToken);

        // Assert
        await Assert.ThrowsAnyAsync<Exception>(act);

        _transacaoValidatorSuccess
            .Verify(x => x.Validate(It.IsAny<LimiteClienteEntity>(), It.IsAny<LimiteClienteEntity>(), It.IsAny<decimal>()), Times.Never);
        _transacaoValidatorSuccess
            .Verify(x => x.ValidateEfetuarTransacao(StatusTransacao.Pendente), Times.Never);

        _limiteClienteRepositoryGetByIdFail
            .Verify(x => x.GetByIdAsync("1", _cancellationToken), Times.Once);
        _limiteClienteRepositoryGetByIdFail
            .Verify(x => x.GetByIdAsync("2", _cancellationToken), Times.Never);
        _limiteClienteRepositoryGetByIdFail
            .Verify(x => x.TransferirAsync(It.IsAny<LimiteClienteEntity>(), It.IsAny<LimiteClienteEntity>(), It.IsAny<TransacaoEntity>(), _cancellationToken), Times.Never);

        _transacaoRepositorySuccess
            .Verify(x => x.CreateAsync(It.IsAny<TransacaoEntity>(), _cancellationToken), Times.Never);
        _transacaoRepositorySuccess
            .Verify(x => x.UpdateStatusAsync(It.IsAny<TransacaoEntity>(), _cancellationToken), Times.Never);

        _unitOfWorkSuccess.Verify(x => x.CommitAsync(_cancellationToken), Times.Never);
    }

    [Fact]
    public async Task Given_EfetuarTransacaoInput_When_TransferirFails_Then_ThrowException()
    {
        // Arrange
        var input = new EfetuarTransacaoInput("1", "2", 1000);

        var efetuarTransacaoUseCase = new EfetuarTransacaoUseCase(
            _appLogger.Object,
            _transacaoValidatorSuccess.Object,
            _transacaoRepositorySuccess.Object,
            _limiteClienteRepositoryTransferirFail.Object,
            _unitOfWorkSuccess.Object);

        // Act
        var act = async () => await efetuarTransacaoUseCase.Execute(input, _cancellationToken);

        // Assert
        await Assert.ThrowsAnyAsync<Exception>(act);

        _transacaoValidatorSuccess
            .Verify(x => x.Validate(It.IsAny<LimiteClienteEntity>(), It
                .IsAny<LimiteClienteEntity>(), It.IsAny<decimal>()), Times.Once);
        _transacaoValidatorSuccess
            .Verify(x => x.ValidateEfetuarTransacao(StatusTransacao.Pendente), Times.Once);

        _limiteClienteRepositoryTransferirFail
            .Verify(x => x.GetByIdAsync("1", _cancellationToken), Times.Once);
        _limiteClienteRepositoryTransferirFail
            .Verify(x => x.GetByIdAsync("2", _cancellationToken), Times.Once);
        _limiteClienteRepositoryTransferirFail
            .Verify(x => x.TransferirAsync(It.IsAny<LimiteClienteEntity>(), It.IsAny<LimiteClienteEntity>(), It.IsAny<TransacaoEntity>(), _cancellationToken), Times.Once);

        _transacaoRepositorySuccess
            .Verify(x => x.CreateAsync(It.IsAny<TransacaoEntity>(), _cancellationToken), Times
            .Once);
        _transacaoRepositorySuccess
            .Verify(x => x.UpdateStatusAsync(It.IsAny<TransacaoEntity>(), _cancellationToken), Times.Never);

        _unitOfWorkSuccess.Verify(x => x.CommitAsync(_cancellationToken), Times.Once);
    }

    [Fact]
    public async Task Given_EfetuarTransacaoInput_When_CreateFails_Then_ThrowException()
    {
        // Arrange
        var input = new EfetuarTransacaoInput("1", "2", 1000);

        var efetuarTransacaoUseCase = new EfetuarTransacaoUseCase(
            _appLogger.Object,
            _transacaoValidatorSuccess.Object,
            _transacaoRepositoryCreateFail.Object,
            _limiteClienteRepositorySuccess.Object,
            _unitOfWorkSuccess.Object);

        // Act
        var act = async () => await efetuarTransacaoUseCase.Execute(input, _cancellationToken);

        // Assert
        await Assert.ThrowsAnyAsync<Exception>(act);

        _transacaoValidatorSuccess
            .Verify(x => x.Validate(It.IsAny<LimiteClienteEntity>(), It
                .IsAny<LimiteClienteEntity>(), It.IsAny<decimal>()), Times.Once);
        _transacaoValidatorSuccess
            .Verify(x => x.ValidateEfetuarTransacao(StatusTransacao.Pendente), Times.Never);

        _limiteClienteRepositorySuccess
            .Verify(x => x.GetByIdAsync("1", _cancellationToken), Times.Once);
        _limiteClienteRepositorySuccess
            .Verify(x => x.GetByIdAsync("2", _cancellationToken), Times.Once);
        _limiteClienteRepositorySuccess
            .Verify(x => x.TransferirAsync(It.IsAny<LimiteClienteEntity>(), It
                .IsAny<LimiteClienteEntity>(), It.IsAny<TransacaoEntity>(), _cancellationToken), Times.Never);

        _transacaoRepositoryCreateFail
            .Verify(x => x.CreateAsync(It.IsAny<TransacaoEntity>(), _cancellationToken), Times.Once);
        _transacaoRepositoryCreateFail
            .Verify(x => x.UpdateStatusAsync(It.IsAny<TransacaoEntity>(), _cancellationToken), Times.Never);

        _unitOfWorkSuccess.Verify(x => x.CommitAsync(_cancellationToken), Times.Never);
    }

    [Fact]
    public async Task Given_EfetuarTransacaoInput_When_UpdateStatusFails_Then_ThrowException()
    {
        // Arrange
        var input = new EfetuarTransacaoInput("1", "2", 1000);

        var efetuarTransacaoUseCase = new EfetuarTransacaoUseCase(
            _appLogger.Object,
            _transacaoValidatorSuccess.Object,
            _transacaoRepositoryUpdateStatusFail.Object,
            _limiteClienteRepositorySuccess.Object,
            _unitOfWorkSuccess.Object);

        // Act
        var act = async () => await efetuarTransacaoUseCase.Execute(input, _cancellationToken);

        // Assert
        await Assert.ThrowsAnyAsync<Exception>(act);

        _transacaoValidatorSuccess
            .Verify(x => x.Validate(It.IsAny<LimiteClienteEntity>(), It.IsAny<LimiteClienteEntity>(), It.IsAny<decimal>()), Times.Once);
        _transacaoValidatorSuccess
            .Verify(x => x.ValidateEfetuarTransacao(StatusTransacao.Pendente), Times.Once);

        _limiteClienteRepositorySuccess
            .Verify(x => x.GetByIdAsync("1", _cancellationToken), Times.Once);
        _limiteClienteRepositorySuccess
            .Verify(x => x.GetByIdAsync("2", _cancellationToken), Times.Once);
        _limiteClienteRepositorySuccess
            .Verify(x => x.TransferirAsync(It.IsAny<LimiteClienteEntity>(), It.IsAny<LimiteClienteEntity>(), It.IsAny<TransacaoEntity>(), _cancellationToken), Times.Once);

        _transacaoRepositoryUpdateStatusFail
            .Verify(x => x.CreateAsync(It.IsAny<TransacaoEntity>(), _cancellationToken), Times.Once);
        _transacaoRepositoryUpdateStatusFail
            .Verify(x => x.UpdateStatusAsync(It.IsAny<TransacaoEntity>(), _cancellationToken), Times.Once);

        _unitOfWorkSuccess.Verify(x => x.CommitAsync(_cancellationToken), Times.Once);
    }

    [Fact]
    public async Task Given_EfetuarTransacaoInput_When_ValidateFails_Then_ThrowException()
    {
        // Arrange
        var input = new EfetuarTransacaoInput("1", "2", 1000);

        var efetuarTransacaoUseCase = new EfetuarTransacaoUseCase(
            _appLogger.Object,
            _transacaoValidatorValidateFail.Object,
            _transacaoRepositorySuccess.Object,
            _limiteClienteRepositorySuccess.Object,
            _unitOfWorkSuccess.Object);

        // Act
        var act = async () => await efetuarTransacaoUseCase.Execute(input, _cancellationToken);

        // Assert
        await Assert.ThrowsAnyAsync<Exception>(act);

        _transacaoValidatorValidateFail
            .Verify(x => x.Validate(It.IsAny<LimiteClienteEntity>(), It.IsAny<LimiteClienteEntity>(), It.IsAny<decimal>()), Times.Once);
        _transacaoValidatorValidateFail
            .Verify(x => x.ValidateEfetuarTransacao(StatusTransacao.Pendente), Times.Never);

        _limiteClienteRepositorySuccess
            .Verify(x => x.GetByIdAsync("1", _cancellationToken), Times.Once);
        _limiteClienteRepositorySuccess
            .Verify(x => x.GetByIdAsync("2", _cancellationToken), Times.Once);
        _limiteClienteRepositorySuccess
            .Verify(x => x.TransferirAsync(It.IsAny<LimiteClienteEntity>(), It.IsAny<LimiteClienteEntity>(), It.IsAny<TransacaoEntity>(), _cancellationToken), Times.Never);

        _transacaoRepositorySuccess
            .Verify(x => x.CreateAsync(It.IsAny<TransacaoEntity>(), _cancellationToken), Times.Never);
        _transacaoRepositorySuccess
            .Verify(x => x.UpdateStatusAsync(It.IsAny<TransacaoEntity>(), _cancellationToken), Times.Never);

        _unitOfWorkSuccess.Verify(x => x.CommitAsync(_cancellationToken), Times.Never);
    }

    [Fact]
    public async Task Given_EfetuarTransacaoInput_When_ValidateEfetuarTransacaoFails_Then_ThrowException()
    {
        // Arrange
        var input = new EfetuarTransacaoInput("1", "2", 1000);

        var efetuarTransacaoUseCase = new EfetuarTransacaoUseCase(
            _appLogger.Object,
            _transacaoValidatorValidateEfetuarTransacaoFail.Object,
            _transacaoRepositorySuccess.Object,
            _limiteClienteRepositorySuccess.Object,
            _unitOfWorkSuccess.Object);

        // Act
        var act = async () => await efetuarTransacaoUseCase.Execute(input, _cancellationToken);

        // Assert
        await Assert.ThrowsAnyAsync<Exception>(act);

        _transacaoValidatorValidateEfetuarTransacaoFail
            .Verify(x => x.Validate(It.IsAny<LimiteClienteEntity>(), It.IsAny<LimiteClienteEntity>(), It.IsAny<decimal>()), Times.Once);
        _transacaoValidatorValidateEfetuarTransacaoFail
            .Verify(x => x.ValidateEfetuarTransacao(StatusTransacao.Pendente), Times.Once);

        _limiteClienteRepositorySuccess
            .Verify(x => x.GetByIdAsync("1", _cancellationToken), Times.Once);
        _limiteClienteRepositorySuccess
            .Verify(x => x.GetByIdAsync("2", _cancellationToken), Times.Once);
        _limiteClienteRepositorySuccess
            .Verify(x => x.TransferirAsync(It.IsAny<LimiteClienteEntity>(), It.IsAny<LimiteClienteEntity>(), It.IsAny<TransacaoEntity>(), _cancellationToken), Times.Never);

        _transacaoRepositorySuccess
            .Verify(x => x.CreateAsync(It.IsAny<TransacaoEntity>(), _cancellationToken), Times.Once);
        _transacaoRepositorySuccess
            .Verify(x => x.UpdateStatusAsync(It.IsAny<TransacaoEntity>(), _cancellationToken), Times.Never);

        _unitOfWorkSuccess.Verify(x => x.CommitAsync(_cancellationToken), Times.Once);
    }

    [Fact]
    public async Task Given_EfetuarTransacaoInput_When_UnitOfWorkFails_Then_ThrowException()
    {
        // Arrange
        var input = new EfetuarTransacaoInput("1", "2", 1000);

        var efetuarTransacaoUseCase = new EfetuarTransacaoUseCase(
            _appLogger.Object,
            _transacaoValidatorSuccess.Object,
            _transacaoRepositorySuccess.Object,
            _limiteClienteRepositorySuccess.Object,
            _unitOfWorkFail.Object);

        // Act
        var act = async () => await efetuarTransacaoUseCase.Execute(input, _cancellationToken);

        // Assert
        await Assert.ThrowsAnyAsync<Exception>(act);

        _transacaoValidatorSuccess
            .Verify(x => x.Validate(It.IsAny<LimiteClienteEntity>(), It.IsAny<LimiteClienteEntity>(), It.IsAny<decimal>()), Times.Once);
        _transacaoValidatorSuccess
            .Verify(x => x.ValidateEfetuarTransacao(StatusTransacao.Pendente), Times.Never);

        _limiteClienteRepositorySuccess
            .Verify(x => x.GetByIdAsync("1", _cancellationToken), Times.Once);
        _limiteClienteRepositorySuccess
            .Verify(x => x.GetByIdAsync("2", _cancellationToken), Times.Once);
        _limiteClienteRepositorySuccess
            .Verify(x => x.TransferirAsync(It.IsAny<LimiteClienteEntity>(), It.IsAny<LimiteClienteEntity>(), It.IsAny<TransacaoEntity>(), _cancellationToken), Times.Never);

        _transacaoRepositorySuccess
            .Verify(x => x.CreateAsync(It.IsAny<TransacaoEntity>(), _cancellationToken), Times.Once);
        _transacaoRepositorySuccess
            .Verify(x => x.UpdateStatusAsync(It.IsAny<TransacaoEntity>(), _cancellationToken), Times.Never);

        _unitOfWorkFail.Verify(x => x.CommitAsync(_cancellationToken), Times.Once);
    }
}