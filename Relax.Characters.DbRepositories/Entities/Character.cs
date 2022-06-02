using System.ComponentModel.DataAnnotations;

namespace Relax.Characters.DbRepositories.Entities
{
    public class Character
    {
        public uint Id { get; set; }

        public uint UserId { get; set; }

        public byte Level { get; set; }

        [MaxLength(64)]
        public string Name { get; set; }
    }
}
