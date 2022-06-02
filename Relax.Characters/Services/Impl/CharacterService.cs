using Kalantyr.Web;
using Relax.Characters.Models;

namespace Relax.Characters.Services.Impl
{
    public class CharacterService: ICharacterService
    {
        public Task<ResultDto<IReadOnlyCollection<uint>>> GetAllCharactersIdsAsync(string token, CancellationToken cancellationToken)
        {
            var result = new ResultDto<IReadOnlyCollection<uint>>
            {
                Result = new [] { 123u }
            };
            return Task.FromResult(result);
        }

        public Task<ResultDto<CharacterInfo>> GetCharacterInfoAsync(string token, uint characterId, CancellationToken cancellationToken)
        {
            var result = new ResultDto<CharacterInfo>
            {
                Result = new CharacterInfo
                {
                    Id = 123,
                    Name = "Адам"
                }
            };
            return Task.FromResult(result);
        }
    }
}
