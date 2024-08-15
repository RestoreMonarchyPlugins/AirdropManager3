using System.Xml.Serialization;

namespace RestoreMonarchy.AirdropManager3.Models
{
    public class AirdropStorage
    {
        [XmlAttribute]
        public ushort BarricadeId { get; set; }
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public byte Width { get; set; }
        [XmlAttribute]
        public byte Height { get; set; }

        public bool ShouldSerializeBarricadeId() => BarricadeId != 0;
        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeWidth() => Width != 0;
        public bool ShouldSerializeHeight() => Height != 0;
    }
}
