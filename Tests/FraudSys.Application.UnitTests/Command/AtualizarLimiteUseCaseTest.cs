namespace FraudSys.Application.UnitTests.Command;

public class AtualizarLimiteUseCaseTest
{
    private readonly Mock<IAppLogger<AtualizarLimiteUseCase>> _appLogger;
    private readonly Mock<ILimiteClienteRepository> _limiteClienteRepository;
    private readonly Mock<ILimiteClienteValidatorFacade> _validatorFacade;
    private readonly Mock<IUnitOfWork> _unitOfWork;

    public AtualizarLimiteUseCaseTest()
    {
        _appLogger = new Mock<IAppLogger<AtualizarLimiteUseCase>>();
        _limiteClienteRepository = new Mock<ILimiteClienteRepository>();
        _validatorFacade = new Mock<ILimiteClienteValidatorFacade>();
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
        var atualizarLimiteUseCase = new AtualizarLimiteUseCase(
            _appLogger.Object,
            _limiteClienteRepository.Object,
            _validatorFacade.Object,
            _unitOfWork.Object);

        var fix = LimiteClienteFixture.LimiteClienteValido("1");
        var input = new AtualizarLimiteInput("1", 500);

        // Act
        var output = await atualizarLimiteUseCase.Execute(input, CancellationToken.None);

        // Assert
        _validatorFacade
            .Verify(x => x.ValidateAtualizacaoLimiteCliente(input.NovoLimite), Times.Once);
        _limiteClienteRepository
            .Verify(x => x.GetByIdAsync("1", It.IsAny<CancellationToken>()), Times.Once);
        _limiteClienteRepository
            .Verify(x => x.UpdateAsync(It.IsAny<LimiteClienteEntity>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWork
            .Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);

        Assert.NotNull(output);
        Assert.NotNull(output.Documento);
        Assert.Equal(fix.Documento, output.Documento);
        Assert.Equal(fix.NumeroAgencia, output.NumeroAgencia);
        Assert.Equal(fix.NumeroConta, output.NumeroConta);
        Assert.Equal(500, output.LimiteTransacao);
    }

    [Fact]
    public async Task Given_InvalidInput_When_Execute_Then_ThrowException()
    {
        // Arrange
        var atualizarLimiteUseCase = new AtualizarLimiteUseCase(
            _appLogger.Object,
            _limiteClienteRepository.Object,
            _validatorFacade.Object,
            _unitOfWork.Object);

        var input = new AtualizarLimiteInput("2", 500);

        _limiteClienteRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Throws(new Exception("Erro de validação"));

        // Act
        var act = async () => await atualizarLimiteUseCase.Execute(input, CancellationToken.None);

        // Assert
        await Assert.ThrowsAnyAsync<Exception>(act);

        _limiteClienteRepository.Verify(x => x.GetByIdAsync("2", It.IsAny<CancellationToken>()), Times.Once);
        _validatorFacade.Verify(x => x.ValidateAtualizacaoLimiteCliente(500), Times.Never);
        _limiteClienteRepository.Verify(x => x.UpdateAsync(It.IsAny<LimiteClienteEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWork.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Given_ValidateException_When_Execute_Then_ThrowException()
    {
        // Arrange
        var atualizarLimiteUseCase = new AtualizarLimiteUseCase(
            _appLogger.Object,
            _limiteClienteRepository.Object,
            _validatorFacade.Object,
            _unitOfWork.Object);

        _validatorFacade
            .Setup(x => x.ValidateAtualizacaoLimiteCliente(It.IsAny<decimal>()))
            .Throws(new Exception("Erro de validação"));

        var input = new AtualizarLimiteInput("1", -200);

        // Act
        var act = async () => await atualizarLimiteUseCase.Execute(input, CancellationToken.None);

        // Assert
        await Assert.ThrowsAnyAsync<Exception>(act);

        _limiteClienteRepository.Verify(x => x.GetByIdAsync("1", It.IsAny<CancellationToken>()), Times.Once);
        _validatorFacade.Verify(x => x.ValidateAtualizacaoLimiteCliente(-200), Times.Once);
        _limiteClienteRepository.Verify(x => x.UpdateAsync(It.IsAny<LimiteClienteEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWork.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Given_UpdateException_When_Execute_Then_ThrowException()
    {
        // Arrange
        var atualizarLimiteUseCase = new AtualizarLimiteUseCase(
            _appLogger.Object,
            _limiteClienteRepository.Object,
            _validatorFacade.Object,
            _unitOfWork.Object);

        _limiteClienteRepository
            .Setup(x => x.UpdateAsync(It.IsAny<LimiteClienteEntity>(), It.IsAny<CancellationToken>()))
            .Throws(new Exception("Erro de atualização"));

        var input = new AtualizarLimiteInput("1", 500);

        // Act
        var act = async () => await atualizarLimiteUseCase.Execute(input, CancellationToken.None);

        // Assert
        await Assert.ThrowsAnyAsync<Exception>(act);

        _limiteClienteRepository.Verify(x => x.GetByIdAsync("1", It.IsAny<CancellationToken>()), Times.Once);
        _validatorFacade.Verify(x => x.ValidateAtualizacaoLimiteCliente(500), Times.Once);
        _limiteClienteRepository.Verify(x => x.UpdateAsync(It.IsAny<LimiteClienteEntity>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWork.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}