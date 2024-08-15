using System.Xml.Serialization;

namespace RestoreMonarchy.AirdropManager3.Models
{
    public class AirdropItem
    {
        public AirdropItem() { }
        public AirdropItem(ushort id, string name, float weight)
        {
            Id = id;
            Name = name;
            Weight = weight;
        }

        [XmlAttribute]
        public ushort Id { get; set; }
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public float Weight { get; set; }

        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
    }
}
