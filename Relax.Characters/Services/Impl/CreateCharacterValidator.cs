using Kalantyr.Web;
using Relax.Characters.Models;

namespace Relax.Characters.Services.Impl
{
    public class CreateCharacterValidator: ICreateCharacterValidator
    {
        public Task<ResultDto<bool>> CanCreateAsync(CharacterInfo info, CancellationToken cancellationToken)
        {
            return Task.FromResult(ResultDto<bool>.Ok);
        }
    }
}
