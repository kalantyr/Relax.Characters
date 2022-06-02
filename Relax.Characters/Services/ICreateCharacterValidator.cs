using Kalantyr.Web;
using Relax.Characters.Models;

namespace Relax.Characters.Services
{
    public interface ICreateCharacterValidator
    {
        Task<ResultDto<bool>> CanCreateAsync(CharacterInfo info, CancellationToken cancellationToken);
    }
}
