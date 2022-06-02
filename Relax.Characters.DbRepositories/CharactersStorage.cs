using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Relax.Characters.DbRepositories.Entities;
using Relax.Characters.InternalModels;

namespace Relax.Characters.DbRepositories
{
    public class CharactersStorage: ICharactersStorage, IHealthCheck, ICharactersStorageAdmin
    {
        private readonly string _connectionString;

        public CharactersStorage(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CharactersDB");
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new())
        {
            try
            {
                await using var ctx = new CharacterDbContext(_connectionString);
                await ctx.Characters.FirstOrDefaultAsync(cancellationToken);
                return HealthCheckResult.Healthy();
            }
            catch (Exception e)
            {
                return HealthCheckResult.Unhealthy(nameof(CharactersStorage), e);
            }
        }

        public async Task<uint> AddAsync(CharacterRecord characterRecord, CancellationToken cancellationToken)
        {
            var character = new Character
            {
                UserId = characterRecord.UserId,
                Name = characterRecord.Name,
                Level = characterRecord.Level
            };

            await using var ctx = new CharacterDbContext(_connectionString);
            await ctx.Characters.AddAsync(character, cancellationToken);
            await ctx.SaveChangesAsync(cancellationToken);
            return character.Id;
        }

        public async Task MigrateAsync(CancellationToken cancellationToken)
        {
            await using var ctx = new CharacterDbContext(_connectionString);
            await ctx.Database.MigrateAsync(cancellationToken);
        }
    }
}
