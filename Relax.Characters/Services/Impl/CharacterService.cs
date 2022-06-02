using Kalantyr.Auth.Client;
using Kalantyr.Web;
using Relax.Characters.Models;

namespace Relax.Characters.Services.Impl
{
    public class CharacterService: ICharacterService
    {
        private readonly IAppAuthClient _authClient;

        public CharacterService(IAppAuthClient authClient)
        {
            _authClient = authClient ?? throw new ArgumentNullException(nameof(authClient));
        }

        public Task<ResultDto<IReadOnlyCollection<uint>>> GetMyCharactersIdsAsync(string token, CancellationToken cancellationToken)
        {
            var result = new ResultDto<IReadOnlyCollection<uint>>
            {
                Result = new [] { 123u, 321u }
            };
            return Task.FromResult(result);
        }

        public Task<ResultDto<CharacterInfo>> GetCharacterInfoAsync(uint characterId, string token, CancellationToken cancellationToken)
        {
            var characterInfo = new CharacterInfo
            {
                Id = characterId,
                Name = characterId == 123 ? "Адам" : "Ева"
            };
            return Task.FromResult(new ResultDto<CharacterInfo> { Result = characterInfo });
        }

        public async Task<ResultDto<uint>> CreateCharacterAsync(CharacterInfo info, string token, CancellationToken cancellationToken)
        {
            var getUserIdResult = await _authClient.GetUserIdAsync(token, cancellationToken);
            if (getUserIdResult.Error != null)
                return new ResultDto<uint> { Error = getUserIdResult.Error };

            throw new NotImplementedException();
        }
    }
}
