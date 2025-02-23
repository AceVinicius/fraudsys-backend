using FraudSys.Domain.LimiteCliente;

namespace FraudSys.Domain.UnitTests.Fixture;

public static class LimiteClienteFixture
{
    public static LimiteClienteEntity LimiteClienteValido(string id)
    {
        return LimiteClienteEntity.Create(
            new LimiteClienteValidatorFacade(),
            id,
            "1234",
            "1000-1",
            1000m
        );
    }
}