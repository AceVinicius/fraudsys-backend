namespace FraudSys.Application.UnitTests.Command;

public class EfetuarTransacaoUseCaseTest
{
    private readonly Mock<IAppLogger<EfetuarTransacaoUseCase>> _appLogger;
    private readonly Mock<ITransacaoRepository> _transacaoRepository;
    private readonly Mock<ITransacaoValidatorFacade> _transacaoValidatorFacade;
    private readonly Mock<ILimiteClienteRepository> _limiteClienteRepository;
    private readonly Mock<IUnitOfWork> _unitOfWork;

    public EfetuarTransacaoUseCaseTest()
    {
        _appLogger = new Mock<IAppLogger<EfetuarTransacaoUseCase>>();
        _transacaoRepository = new Mock<ITransacaoRepository>();
        _transacaoValidatorFacade = new Mock<ITransacaoValidatorFacade>();
        _limiteClienteRepository = new Mock<ILimiteClienteRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();

        _limiteClienteRepository
            .Setup(x => x.GetByIdAsync("1", It.IsAny<CancellationToken>()))
            .ReturnsAsync(LimiteClienteFixture.LimiteClienteValido("1"));
        _limiteClienteRepository
            .Setup(x => x.GetByIdAsync("2", It.IsAny<CancellationToken>()))
            .ReturnsAsync(LimiteClienteFixture.LimiteClienteValido("2"));
        _limiteClienteRepository
            .Setup<Task>(x => x.TransferirAsync(
                It.IsAny<LimiteClienteEntity>(),
                It.IsAny<LimiteClienteEntity>(),
                It.IsAny<TransacaoEntity>(),
                It.IsAny<CancellationToken>()));
    }

    [Fact]
    public async Task Given_ValidInput_When_Execute_Then_ReturnEfetuarTransacaoOutput()
    {
        // Arrange
        var efetuarTransacaoUseCase = new EfetuarTransacaoUseCase(
            _appLogger.Object,
            _transacaoValidatorFacade.Object,
            _transacaoRepository.Object,
            _limiteClienteRepository.Object,
            _unitOfWork.Object);

        var input = new EfetuarTransacaoInput("1", "2", 1000);

        // Act
        var output = await efetuarTransacaoUseCase.Execute(input, CancellationToken.None);

        // Assert
        Assert.NotNull(output);
        Assert.Equal(StatusTransacao.Aprovada, output.Status);


        _limiteClienteRepository.Verify(x => x.GetByIdAsync(
                "1",
                It.IsAny<CancellationToken>()
            ),
            Times.Once);
        _limiteClienteRepository.Verify(x => x.GetByIdAsync(
                "2",
                It.IsAny<CancellationToken>()
            ),
            Times.Once);
        _limiteClienteRepository.Verify(x => x.TransferirAsync(
                It.IsAny<LimiteClienteEntity>(),
                It.IsAny<LimiteClienteEntity>(),
                It.IsAny<TransacaoEntity>(),
                It.IsAny<CancellationToken>()
            ),
            Times.Once);
        _transacaoRepository.Verify(x => x.CreateAsync(
                It.IsAny<TransacaoEntity>(),
                It.IsAny<CancellationToken>()
            ),
            Times.Once);
    }

    [Fact]
    public async Task Given_InvalidInput_When_Execute_Then_ReturnEfetuarTransacaoOutput()
    {
        // Arrange
        var efetuarTransacaoUseCase = new EfetuarTransacaoUseCase(
            _appLogger.Object,
            _transacaoValidatorFacade.Object,
            _transacaoRepository.Object,
            _limiteClienteRepository.Object,
            _unitOfWork.Object);

        var input = new EfetuarTransacaoInput("1", "2", 1000000);

        // Act
        var output = await efetuarTransacaoUseCase.Execute(input, CancellationToken.None);

        // Assert
        Assert.NotNull(output);
        Assert.Equal(StatusTransacao.Rejeitada, output.Status);

        _limiteClienteRepository.Verify(x => x.GetByIdAsync(
                "1",
                It.IsAny<CancellationToken>()
            ),
            Times.Once);
        _limiteClienteRepository.Verify(x => x.GetByIdAsync(
                "2",
                It.IsAny<CancellationToken>()
            ),
            Times.Once);
        // _transacaoValidatorFacade.Verify(x => x.ValidateTransacao(
        //         1000000,
        //         It.IsAny<LimiteClienteEntity>()
        //     ),
        //     Times.Once);
        _limiteClienteRepository.Verify(x => x.TransferirAsync(
                It.IsAny<LimiteClienteEntity>(),
                It.IsAny<LimiteClienteEntity>(),
                It.IsAny<TransacaoEntity>(),
                It.IsAny<CancellationToken>()
            ),
            Times.Never);
        _transacaoRepository.Verify(x => x.CreateAsync(
                It.IsAny<TransacaoEntity>(),
                It.IsAny<CancellationToken>()
            ),
            Times.Once);
    }

    [Fact]
    public async Task Given_InvalidInput_When_Execute_Then_ThrowException()
    {
        // Arrange
        var efetuarTransacaoUseCase = new EfetuarTransacaoUseCase(
            _appLogger.Object,
            _transacaoValidatorFacade.Object,
            _transacaoRepository.Object,
            _limiteClienteRepository.Object,
            _unitOfWork.Object);

        var input = new EfetuarTransacaoInput("1", "2", 1000000);

        _limiteClienteRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Erro ao criar limite"));

        // Act
        var act = async () => await efetuarTransacaoUseCase.Execute(input, CancellationToken.None);

        // Assert
        await Assert.ThrowsAnyAsync<Exception>(act);

        _limiteClienteRepository.Verify(x => x.GetByIdAsync("1", It.IsAny<CancellationToken>()),
            Times.Once);
        _limiteClienteRepository.Verify(x => x.GetByIdAsync("2", It.IsAny<CancellationToken>()),
            Times.Never);
        _limiteClienteRepository.Verify(
            x => x.TransferirAsync(It.IsAny<LimiteClienteEntity>(), It.IsAny<LimiteClienteEntity>(),
                It.IsAny<TransacaoEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        _transacaoRepository.Verify(x => x.CreateAsync(
                It.IsAny<TransacaoEntity>(),
                It.IsAny<CancellationToken>()
            ),
            Times.Never);
    }
}