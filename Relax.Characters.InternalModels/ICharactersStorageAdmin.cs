namespace Relax.Characters.InternalModels
{
    public interface ICharactersStorageAdmin
    {
        Task MigrateAsync(CancellationToken cancellationToken);
    }
}
