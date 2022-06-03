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

        public async Task<IReadOnlyCollection<CharacterRecord>> GetByUserIdAsync(uint userId, CancellationToken cancellationToken)
        {
            await using var ctx = new CharacterDbContext(_connectionString);
            var records = await ctx.Characters
                .AsNoTracking()
                .Where(ch => ch.UserId == userId)
                .ToArrayAsync(cancellationToken);
            return records
                .Select(Map)
                .ToArray();
        }

        public async Task<CharacterRecord> GetByIdAsync(uint characterId, CancellationToken cancellationToken)
        {
            await using var ctx = new CharacterDbContext(_connectionString);
            var record = await ctx.Characters
                .AsNoTracking()
                .FirstOrDefaultAsync(ch => ch.Id == characterId, cancellationToken);
            return record == null ? null : Map(record);
        }

        private static CharacterRecord Map(Character r)
        {
            return new CharacterRecord
            {
                Id = r.Id,
                Level = r.Level,
                Name = r.Name,
                UserId = r.UserId
            };
        }
    }
}
