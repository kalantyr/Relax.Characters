using Kalavarda.Primitives.Abstract;
using Kalavarda.Primitives.Geometry;

namespace Relax.Characters.Models
{
    public class CharacterInfo: IHasPosition
    {
        public uint Id { get; set; }

        public string Name { get; set; }

        public byte Level { get; set; }

        public uint LocationId { get; set; }

        public PointF Position { get; set; }
    }
}
