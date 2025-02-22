namespace FraudSys.Application.UnitTests.Command;

public class CadastrarLimiteUseCaseTest
{
    private readonly Mock<IAppLogger<CadastrarLimiteUseCase>> _appLogger;
    private readonly Mock<ILimiteClienteRepository> _limiteClienteRepository;
    private readonly Mock<ILimiteClienteValidatorFacade> _validatorFacade;
    private readonly Mock<IUnitOfWork> _unitOfWork;

    public CadastrarLimiteUseCaseTest()
    {
        _appLogger = new Mock<IAppLogger<CadastrarLimiteUseCase>>();
        _limiteClienteRepository = new Mock<ILimiteClienteRepository>();
        _validatorFacade = new Mock<ILimiteClienteValidatorFacade>();
        _unitOfWork = new Mock<IUnitOfWork>();

        _limiteClienteRepository
            .Setup(x => x.CreateAsync(It.IsAny<LimiteClienteEntity>(), It.IsAny<CancellationToken>()));
    }

    [Fact]
    public async Task Given_ValidInput_When_Execute_Then_ReturnLimiteCliente()
    {
        // Arrange
        var cadastrarLimiteUseCase = new CadastrarLimiteUseCase(
            _appLogger.Object,
            _limiteClienteRepository.Object,
            _validatorFacade.Object,
            _unitOfWork.Object);

        var fix = LimiteClienteFixture.LimiteClienteValido("1");
        var input = new CadastrarLimiteInput(
            fix.Documento,
            fix.NumeroAgencia,
            fix.NumeroConta,
            fix.LimiteTransacao);

        // Act
        var output = await cadastrarLimiteUseCase.Execute(input, CancellationToken.None);

        // Assert
        _validatorFacade.Verify(x => x.ValidateCriacaoLimiteCliente(
            input.Documento,
            input.NumeroAgencia,
            input.NumeroConta,
            input.LimiteTransacao), Times.Once);
        _limiteClienteRepository
            .Verify(x => x.CreateAsync(It.IsAny<LimiteClienteEntity>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWork.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);

        Assert.NotNull(output);
        Assert.NotNull(output.Documento);
        Assert.Equal(fix.Documento, output.Documento);
    }

    [Fact]
    public async Task Given_InvalidInput_When_Execute_Then_ThrowNotFoundException()
    {
        // Arrange
        var cadastrarLimiteUseCase = new CadastrarLimiteUseCase(
            _appLogger.Object,
            _limiteClienteRepository.Object,
            _validatorFacade.Object,
            _unitOfWork.Object);

        var fix = LimiteClienteFixture.LimiteClienteValido("1");
        var input = new CadastrarLimiteInput(
            fix.Documento,
            fix.NumeroAgencia,
            fix.NumeroConta,
            fix.LimiteTransacao);

        _limiteClienteRepository
            .Setup(x => x.CreateAsync(It.IsAny<LimiteClienteEntity>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Erro ao criar limite"));

        // Act
        var act  = async () => await cadastrarLimiteUseCase.Execute(input, CancellationToken.None);

        // Assert
        await Assert.ThrowsAnyAsync<Exception>(act);

        _validatorFacade.Verify(x => x.ValidateCriacaoLimiteCliente(
            input.Documento,
            input.NumeroAgencia,
            input.NumeroConta,
            input.LimiteTransacao), Times.Once);
        _limiteClienteRepository.Verify(x => x.CreateAsync(It.IsAny<LimiteClienteEntity>(), It.IsAny<CancellationToken>()), Times
            .Once);
        _unitOfWork.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}