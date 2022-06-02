namespace Relax.Characters.InternalModels
{
    public interface ICharactersStorageReadonly
    {
    }

    public interface ICharactersStorage: ICharactersStorageReadonly
    {
        Task<uint> AddAsync(CharacterRecord characterRecord, CancellationToken cancellationToken);
    }
}
