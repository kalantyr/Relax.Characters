using Microsoft.EntityFrameworkCore;
using Relax.Characters.DbRepositories.Entities;

namespace Relax.Characters.DbRepositories
{
    internal class CharacterDbContext : DbContext
    {
        private readonly string _connectionString;

        public CharacterDbContext() { }

        public CharacterDbContext(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                if (string.IsNullOrEmpty(_connectionString))
                    options.UseSqlServer();
                else
                    options.UseSqlServer(_connectionString);
            }

            base.OnConfiguring(options);
        }

        public DbSet<Character> Characters { get; set; }
    }
}
