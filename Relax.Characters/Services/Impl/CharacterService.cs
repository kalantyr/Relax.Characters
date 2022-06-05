using Kalantyr.Auth.Client;
using Kalantyr.Web;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Relax.Characters.InternalModels;
using Relax.Characters.Models;

namespace Relax.Characters.Services.Impl
{
    public class CharacterService: ICharacterService, IHealthCheck
    {
        private static readonly ResultDto<CharacterInfo> CharacterNotFound = new() { Error = Errors.CharacterNotFound };

        private readonly IAppAuthClient _authClient;
        private readonly ICharactersStorage _charactersStorage;
        private readonly ICreateCharacterValidator _createCharacterValidator;
        
        public CharacterService(IAppAuthClient authClient, ICharactersStorage charactersStorage, ICreateCharacterValidator createCharacterValidator)
        {
            _authClient = authClient ?? throw new ArgumentNullException(nameof(authClient));
            _charactersStorage = charactersStorage ?? throw new ArgumentNullException(nameof(charactersStorage));
            _createCharacterValidator = createCharacterValidator ?? throw new ArgumentNullException(nameof(createCharacterValidator));
        }

        public async Task<ResultDto<IReadOnlyCollection<uint>>> GetMyCharactersIdsAsync(string token, CancellationToken cancellationToken)
        {
            var getUserIdResult = await _authClient.GetUserIdAsync(token, cancellationToken);
            if (getUserIdResult.Error != null)
                return new ResultDto<IReadOnlyCollection<uint>> { Error = getUserIdResult.Error };

            cancellationToken.ThrowIfCancellationRequested();

            var records = await _charactersStorage.GetByUserIdAsync(getUserIdResult.Result, cancellationToken);
            return new ResultDto<IReadOnlyCollection<uint>>
            {
                Result = records.Select(r => r.Id).ToArray()
            };
        }

        public async Task<ResultDto<CharacterInfo>> GetCharacterInfoAsync(uint characterId, string token, CancellationToken cancellationToken)
        {
            var record = await _charactersStorage.GetByIdAsync(characterId, cancellationToken);
            if (record == null)
                return CharacterNotFound;

            var characterInfo = new CharacterInfo
            {
                Id = characterId,
                Name = record.Name,
                Level = record.Level,

                LocationId = 1, // TODO
                Position = new (0, 0) // TODO
            };
            return new ResultDto<CharacterInfo> { Result = characterInfo };
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
