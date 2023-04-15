using System.Xml.Serialization;

namespace RestoreMonarchy.AirdropManager.Models
{
    public class CustomAirdropSpawn
    {
        [XmlAttribute]
        public ushort AirdropId { get; set; }
        [XmlAttribute]
        public string Name { get; set; }
        public Position Position { get; set; }

        public AirdropSpawn ToAirdropSpawn()
        {
            return new AirdropSpawn()
            {
                AirdropId = AirdropId,
                Name = Name,
                Position = Position.ToVector()
            };
        }
    }
}
