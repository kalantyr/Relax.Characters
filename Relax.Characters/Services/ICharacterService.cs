using Kalantyr.Web;
using Relax.Characters.Models;

namespace Relax.Characters.Services
{
    public interface ICharacterService
    {
        Task<ResultDto<IReadOnlyCollection<uint>>> GetAllCharactersIdsAsync(string token, CancellationToken cancellationToken);

        Task<ResultDto<CharacterInfo>> GetCharacterInfoAsync(string token, uint characterId, CancellationToken cancellationToken);
    }
}
