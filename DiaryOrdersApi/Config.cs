namespace OrdersApi;

public interface IConfig
{
    bool RunDbMigrations { get; init; }
}

public record Config : IConfig
{
    public bool RunDbMigrations { get; init; }
}