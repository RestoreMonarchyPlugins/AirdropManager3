using System.Collections.Generic;
using System.Xml.Serialization;

namespace RestoreMonarchy.AirdropManager.Models
{
    public class Airdrop
    {
        public string Name { get; set; }
        [XmlArrayItem("id")]
        public List<ushort> Items { get; set; }
    }
}
