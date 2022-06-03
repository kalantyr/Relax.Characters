namespace Relax.Characters.InternalModels
{
    public interface ICharactersStorageReadonly
    {
        Task<IReadOnlyCollection<CharacterRecord>> GetByUserIdAsync(uint userId, CancellationToken cancellationToken);

        Task<CharacterRecord> GetByIdAsync(uint characterId, CancellationToken cancellationToken);
    }

    public interface ICharactersStorage: ICharactersStorageReadonly
    {
        Task<uint> AddAsync(CharacterRecord characterRecord, CancellationToken cancellationToken);
    }
}
