namespace FraudSys.Application.UnitTests.Command;

public class CadastrarLimiteUseCaseTest
{
    private readonly Mock<IAppLogger<CadastrarLimiteUseCase>> _appLogger;
    private readonly Mock<ILimiteClienteValidatorFacade> _validatorFacadeSuccess;
    private readonly Mock<ILimiteClienteValidatorFacade> _validatorFacadeFail;
    private readonly Mock<ILimiteClienteRepository> _limiteClienteRepositorySuccess;
    private readonly Mock<ILimiteClienteRepository> _limiteClienteRepositoryFail;
    private readonly Mock<IUnitOfWork> _unitOfWorkSuccess;
    private readonly Mock<IUnitOfWork> _unitOfWorkFail;
    private readonly CancellationToken _cancellationToken;

    public CadastrarLimiteUseCaseTest()
    {
        _appLogger = new Mock<IAppLogger<CadastrarLimiteUseCase>>();
        _validatorFacadeSuccess = new Mock<ILimiteClienteValidatorFacade>();
        _validatorFacadeFail = new Mock<ILimiteClienteValidatorFacade>();
        _limiteClienteRepositorySuccess = new Mock<ILimiteClienteRepository>();
        _limiteClienteRepositoryFail = new Mock<ILimiteClienteRepository>();
        _unitOfWorkSuccess = new Mock<IUnitOfWork>();
        _unitOfWorkFail = new Mock<IUnitOfWork>();

        _cancellationToken = CancellationToken.None;

        _validatorFacadeSuccess
            .Setup(x => x.Validate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>()));

        _validatorFacadeFail
            .Setup(x => x.Validate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>()))
            .Throws(new EntityValidationException("Erro de validação"));

        _limiteClienteRepositorySuccess
            .Setup(x => x.CreateAsync(It.IsAny<LimiteClienteEntity>(), _cancellationToken));

        _limiteClienteRepositoryFail
            .Setup(x => x.CreateAsync(It.IsAny<LimiteClienteEntity>(), _cancellationToken))
            .ThrowsAsync(new Exception("Erro de repository"));

        _unitOfWorkSuccess
            .Setup(x => x.CommitAsync(_cancellationToken));

        _unitOfWorkFail
            .Setup(x => x.CommitAsync(_cancellationToken))
            .ThrowsAsync(new Exception("Erro de commit"));
    }

    [Fact]
    public async Task Given_CadastrarLimiteInput_When_Execute_Then_ReturnLCadastrarLimiteOutput()
    {
        // Arrange
        var fix = LimiteClienteFixture.LimiteClienteValido("1");
        var input = new CadastrarLimiteInput(
            fix.Documento,
            fix.NumeroAgencia,
            fix.NumeroConta,
            fix.LimiteTransacao);

        var cadastrarLimiteUseCase = new CadastrarLimiteUseCase(
            _appLogger.Object,
            _validatorFacadeSuccess.Object,
            _limiteClienteRepositorySuccess.Object,
            _unitOfWorkSuccess.Object);

        // Act
        var output = await cadastrarLimiteUseCase.Execute(input, _cancellationToken);

        // Assert
        Assert.NotNull(output);
        Assert.Equal(fix.Documento, output.Documento);
        Assert.Equal(fix.NumeroAgencia, output.NumeroAgencia);
        Assert.Equal(fix.NumeroConta, output.NumeroConta);
        Assert.Equal(fix.LimiteTransacao, output.LimiteTransacao);

        _validatorFacadeSuccess
            .Verify(x => x.Validate(
                input.Documento,
                input.NumeroAgencia,
                input.NumeroConta,
                input.LimiteTransacao), Times.Once);

        _limiteClienteRepositorySuccess
            .Verify(x => x.CreateAsync(It.IsAny<LimiteClienteEntity>(), _cancellationToken), Times.Once);

        _unitOfWorkSuccess.Verify(x => x.CommitAsync(_cancellationToken), Times.Once);
    }

    [Fact]
    public async Task Given_CadastrarLimiteInput_When_ValidatorFails_Then_ThrowEntityValidationException()
    {
        // Arrange
        var fix = LimiteClienteFixture.LimiteClienteValido("1");
        var input = new CadastrarLimiteInput(
            fix.Documento,
            fix.NumeroAgencia,
            fix.NumeroConta,
            fix.LimiteTransacao);

        var cadastrarLimiteUseCase = new CadastrarLimiteUseCase(
            _appLogger.Object,
            _validatorFacadeFail.Object,
            _limiteClienteRepositorySuccess.Object,
            _unitOfWorkSuccess.Object);

        // Act
        var act = async () => await cadastrarLimiteUseCase.Execute(input, _cancellationToken);

        // Assert
        await Assert.ThrowsAsync<EntityValidationException>(act);

        _validatorFacadeFail
            .Verify(x => x.Validate(
                input.Documento,
                input.NumeroAgencia,
                input.NumeroConta,
                input.LimiteTransacao), Times.Once);

        _limiteClienteRepositorySuccess
            .Verify(x => x.CreateAsync(It.IsAny<LimiteClienteEntity>(), _cancellationToken), Times.Never);

        _unitOfWorkSuccess.Verify(x => x.CommitAsync(_cancellationToken), Times.Never);
    }

    [Fact]
    public async Task Given_CadastrarLimiteInput_When_LimiteClienteRepositoryFails_Then_ThrowException()
    {
        // Arrange
        var fix = LimiteClienteFixture.LimiteClienteValido("1");
        var input = new CadastrarLimiteInput(
            fix.Documento,
            fix.NumeroAgencia,
            fix.NumeroConta,
            fix.LimiteTransacao);

        var cadastrarLimiteUseCase = new CadastrarLimiteUseCase(
            _appLogger.Object,
            _validatorFacadeSuccess.Object,
            _limiteClienteRepositoryFail.Object,
            _unitOfWorkSuccess.Object);

        // Act
        var act = async () => await cadastrarLimiteUseCase.Execute(input, _cancellationToken);

        // Assert
        await Assert.ThrowsAnyAsync<Exception>(act);

        _validatorFacadeSuccess
            .Verify(x => x.Validate(
                input.Documento,
                input.NumeroAgencia,
                input.NumeroConta,
                input.LimiteTransacao), Times.Once);

        _limiteClienteRepositoryFail
            .Verify(x => x.CreateAsync(It.IsAny<LimiteClienteEntity>(), _cancellationToken), Times.Once);

        _unitOfWorkSuccess.Verify(x => x.CommitAsync(_cancellationToken), Times.Never);
    }

    [Fact]
    public async Task Given_CadastrarLimiteInput_When_UnitOfWorkFails_Then_ThrowException()
    {
        // Arrange
        var fix = LimiteClienteFixture.LimiteClienteValido("1");
        var input = new CadastrarLimiteInput(
            fix.Documento,
            fix.NumeroAgencia,
            fix.NumeroConta,
            fix.LimiteTransacao);

        var cadastrarLimiteUseCase = new CadastrarLimiteUseCase(
            _appLogger.Object,
            _validatorFacadeSuccess.Object,
            _limiteClienteRepositorySuccess.Object,
            _unitOfWorkFail.Object);

        // Act
        var act = async () => await cadastrarLimiteUseCase.Execute(input, _cancellationToken);

        // Assert
        await Assert.ThrowsAnyAsync<Exception>(act);

        _validatorFacadeSuccess
            .Verify(x => x.Validate(
                input.Documento,
                input.NumeroAgencia,
                input.NumeroConta,
                input.LimiteTransacao), Times.Once);

        _limiteClienteRepositorySuccess
            .Verify(x => x.CreateAsync(It.IsAny<LimiteClienteEntity>(), _cancellationToken), Times.Once);

        _unitOfWorkFail.Verify(x => x.CommitAsync(_cancellationToken), Times.Once);
    }
}