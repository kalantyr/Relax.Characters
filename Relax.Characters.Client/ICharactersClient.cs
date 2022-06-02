using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Kalantyr.Web;
using Relax.Characters.Models;

namespace Relax.Characters.Client
{
    public interface ICharactersReadonlyClient
    {
        Task<ResultDto<CharacterInfo>> GetCharacterInfoAsync(uint characterId, string userToken, CancellationToken cancellationToken);

        Task<ResultDto<IReadOnlyCollection<uint>>> GetMyCharactersIdsAsync(string userToken, CancellationToken cancellationToken);
    }

    public interface ICharactersClient: ICharactersReadonlyClient
    {
        Task<ResultDto<uint>> CreateAsync(CharacterInfo info, string userToken, CancellationToken cancellationToken);
    }
}
