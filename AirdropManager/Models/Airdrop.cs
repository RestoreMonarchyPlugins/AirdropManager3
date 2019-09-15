using System.Collections.Generic;
using System.Xml.Serialization;

namespace RestoreMonarchy.AirdropManager.Models
{
    public class Airdrop
    {
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public double Chance { get; set; }
        [XmlArrayItem("id")]
        public List<ushort> Items { get; set; }
    }
}
