using Kalantyr.Web;
using Relax.Characters.Models;

namespace Relax.Characters.Services
{
    public interface ICharacterService
    {
        Task<ResultDto<IReadOnlyCollection<uint>>> GetMyCharactersIdsAsync(string token, CancellationToken cancellationToken);

        Task<ResultDto<CharacterInfo>> GetCharacterInfoAsync(uint characterId, string token, CancellationToken cancellationToken);
        
        Task<ResultDto<uint>> CreateCharacterAsync(CharacterInfo info, string token, CancellationToken cancellationToken);
    }
}
