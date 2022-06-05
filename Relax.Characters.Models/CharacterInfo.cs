namespace Relax.Characters.Models
{
    public class CharacterInfo
    {
        public uint Id { get; set; }

        public string Name { get; set; }

        public byte Level { get; set; }

        public uint LocationId { get; set; }

        public (float, float) Position { get; set; }
    }
}
