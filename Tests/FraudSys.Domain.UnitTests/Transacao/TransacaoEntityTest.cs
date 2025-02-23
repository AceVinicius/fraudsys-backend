namespace FraudSys.Domain.UnitTests.Transacao;

public class TransacaoEntityTest
{
    private readonly Mock<ITransacaoValidatorFacade> _validatorSuccess;
    private readonly Mock<ITransacaoValidatorFacade> _validatorFail;
    private readonly Mock<ITransacaoValidatorFacade> _validatorFailEfetuarTransacao;

    public TransacaoEntityTest()
    {
        _validatorSuccess = new Mock<ITransacaoValidatorFacade>();
        _validatorFail = new Mock<ITransacaoValidatorFacade>();
        _validatorFailEfetuarTransacao = new Mock<ITransacaoValidatorFacade>();

        _validatorSuccess
            .Setup(x => x.Validate(It.IsAny<LimiteClienteEntity>(), It.IsAny<LimiteClienteEntity>(), It.IsAny<decimal>()));
        _validatorSuccess
            .Setup(x => x.ValidateHydration(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<LimiteClienteEntity>(),
                It.IsAny<LimiteClienteEntity>(), It.IsAny<decimal>(), It.IsAny<DateTime>()));
        _validatorSuccess
            .Setup(x => x.ValidateEfetuarTransacao(It.IsAny<StatusTransacao>()));

        _validatorFail
            .Setup(x => x.Validate(It.IsAny<LimiteClienteEntity>(), It.IsAny<LimiteClienteEntity>(),
                It.IsAny<decimal>()))
            .Throws(new EntityValidationException("Erro de validação"));
        _validatorFail
            .Setup(x => x.ValidateHydration(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<LimiteClienteEntity>(),
                It.IsAny<LimiteClienteEntity>(), It.IsAny<decimal>(), It.IsAny<DateTime>()))
            .Throws(new EntityHydrationException("Erro de validação", new SystemException()));

        _validatorFailEfetuarTransacao
            .Setup(x => x.Validate(It.IsAny<LimiteClienteEntity>(), It.IsAny<LimiteClienteEntity>(), It.IsAny<decimal>()));
        _validatorFailEfetuarTransacao
            .Setup(x => x.ValidateEfetuarTransacao(It.IsAny<StatusTransacao>()))
            .Throws(new TransactionException("Erro de validação", new System.Exception()));
    }

    [Fact]
    public void Given_TransacaoEntity_When_CriarTransacao_Then_CreateEntity()
    {
        // Arrange
        var limiteClientePagador = LimiteClienteFixture.LimiteClienteValido("1");
        var limiteClienteRecebedor = LimiteClienteFixture.LimiteClienteValido("2");
        var valor = 1000m;

        // Act
        var transacao = TransacaoEntity.Create(
            _validatorSuccess.Object,
            limiteClientePagador,
            limiteClienteRecebedor,
            valor);

        // Assert
        Assert.NotNull(transacao);
        Assert.Equal(valor, transacao.Valor);
        Assert.Equal(StatusTransacao.Pendente, transacao.Status);
        Assert.NotEmpty(transacao.Id.ToString());
    }

    [Fact]
    public void Given_TransacaoEntity_When_CriarTransacao_Then_ThrowEntityValidationException()
    {
        // Arrange
        var limiteClientePagador = LimiteClienteFixture.LimiteClienteValido("1");
        var limiteClienteRecebedor = LimiteClienteFixture.LimiteClienteValido("2");
        var valor = -1000m;

        // Act
        var act = () => _ = TransacaoEntity.Create(
            _validatorFail.Object,
            limiteClientePagador,
            limiteClienteRecebedor,
            valor);

        // Assert
        Assert.Throws<EntityValidationException>(act);
    }

    [Fact]
    public void Given_TransacaoEntity_When_HydrateTransacao_Then_CreateEntity()
    {
        // Arrange
        var limiteClientePagador = LimiteClienteFixture.LimiteClienteValido("1");
        var limiteClienteRecebedor = LimiteClienteFixture.LimiteClienteValido("2");
        var id = Guid.NewGuid();
        var status = StatusTransacao.Pendente;
        var valor = 1000m;
        var dataTransacao = DateTime.Now;

        // Act
        var transacao = TransacaoEntity.Hydrate(
            _validatorSuccess.Object,
            id.ToString(),
            (int) status,
            limiteClientePagador,
            limiteClienteRecebedor,
            valor,
            dataTransacao);

        // Assert
        Assert.NotNull(transacao);
        Assert.Equal(id, transacao.Id);
        Assert.Equal(status, transacao.Status);
        Assert.Equal(valor, transacao.Valor);
        Assert.Equal(dataTransacao, transacao.DataTransacao);
    }

    [Fact]
    public void Given_TransacaoEntity_When_HydrateTransacao_Then_ThrowEntityHydrationException()
    {
        // Arrange
        var limiteClientePagador = LimiteClienteFixture.LimiteClienteValido("1");
        var limiteClienteRecebedor = LimiteClienteFixture.LimiteClienteValido("2");
        var id = Guid.NewGuid();
        var status = StatusTransacao.Pendente;
        var valor = -1000m;
        var dataTransacao = DateTime.Now;

        // Act
        var act = () => _ = TransacaoEntity.Hydrate(
            _validatorFail.Object,
            id.ToString(),
            (int) status,
            limiteClientePagador,
            limiteClienteRecebedor,
            valor,
            dataTransacao);

        // Assert
        Assert.Throws<EntityHydrationException>(act);
    }

    [Fact]
    public void Given_TransacaoEntityValida_When_EfetuarTransacao_Then_TransacaoIsApproved()
    {
        // Arrange
        var valor = 100m;
        var limiteClientePagador = LimiteClienteFixture.LimiteClienteValido("1");
        var limiteClienteRecebedor = LimiteClienteFixture.LimiteClienteValido("2");
        var transacao = TransacaoEntity.Create(
            _validatorSuccess.Object,
            limiteClientePagador,
            limiteClienteRecebedor,
            valor);

        // Act
        transacao.EfetuarTransacao();

        // Assert
        Assert.Equal(StatusTransacao.Aprovada, transacao.Status);
    }

    [Fact]
    public void Given_TransacaoEntityInvalida_When_EfetuarTransacao_Then_TransacaoIsRejectedAndThrowTransactionException()
    {
        // Arrange
        var valor = -100m;
        var limiteClientePagador = LimiteClienteFixture.LimiteClienteValido("1");
        var limiteClienteRecebedor = LimiteClienteFixture.LimiteClienteValido("2");
        var transacao = TransacaoEntity.Create(
            _validatorSuccess.Object,
            limiteClientePagador,
            limiteClienteRecebedor,
            valor);

        // Act
        var act = () => transacao.EfetuarTransacao();

        // Assert
        Assert.Throws<TransactionException>(act);
        Assert.Equal(StatusTransacao.Rejeitada, transacao.Status);
    }
}