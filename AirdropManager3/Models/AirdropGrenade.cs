using System.Xml.Serialization;

namespace RestoreMonarchy.AirdropManager3.Models
{
    public class AirdropGrenade
    {
        [XmlAttribute]
        public ushort Id { get; set; }
        [XmlAttribute]
        public string Name { get; set; }

        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
    }
}
