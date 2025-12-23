namespace SiseApi.Seed
{
    public interface IDbSeeder
    {
        Task SeedAsync(CancellationToken cancellationToken = default);
    }
}
