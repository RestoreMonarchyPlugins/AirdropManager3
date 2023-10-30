using System.Collections.Generic;
using System.Xml.Serialization;

namespace RestoreMonarchy.AirdropManager.Models
{
    public class Airdrop
    {
        [XmlAttribute]
        public ushort AirdropId { get; set; }
        [XmlAttribute]
        public string Name { get; set; }
        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);

        public List<AirdropItem> Items { get; set; }        
    }
}
