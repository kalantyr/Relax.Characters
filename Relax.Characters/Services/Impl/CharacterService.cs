using Kalantyr.Auth.Client;
using Kalantyr.Web;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Relax.Characters.InternalModels;
using Relax.Characters.Models;

namespace Relax.Characters.Services.Impl
{
    public class CharacterService: ICharacterService, IHealthCheck
    {
        private readonly IAppAuthClient _authClient;
        private readonly ICharactersStorage _charactersStorage;
        private readonly ICreateCharacterValidator _createCharacterValidator;

        public CharacterService(IAppAuthClient authClient, ICharactersStorage charactersStorage, ICreateCharacterValidator createCharacterValidator)
        {
            _authClient = authClient ?? throw new ArgumentNullException(nameof(authClient));
            _charactersStorage = charactersStorage ?? throw new ArgumentNullException(nameof(charactersStorage));
            _createCharacterValidator = createCharacterValidator ?? throw new ArgumentNullException(nameof(createCharacterValidator));
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

            cancellationToken.ThrowIfCancellationRequested();

            var validationResult = await _createCharacterValidator.CanCreateAsync(info, cancellationToken);
            if (validationResult.Error != null)
                return new ResultDto<uint> { Error = validationResult.Error };

            cancellationToken.ThrowIfCancellationRequested();

            var record = new CharacterRecord
            {
                UserId = getUserIdResult.Result,
                Name = info.Name,
                Level = 1
            };
            var newId = await _charactersStorage.AddAsync(record, cancellationToken);
            return new ResultDto<uint> { Result = newId };
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new())
        {
            if (_authClient is IHealthCheck hc1)
            {
                var result = await hc1.CheckHealthAsync(context, cancellationToken);
                if (result.Status != HealthStatus.Healthy)
                    return result;
            }

            if (_charactersStorage is IHealthCheck hc2)
            {
                var result = await hc2.CheckHealthAsync(context, cancellationToken);
                if (result.Status != HealthStatus.Healthy)
                    return result;
            }

            return HealthCheckResult.Healthy();
        }
    }
}
