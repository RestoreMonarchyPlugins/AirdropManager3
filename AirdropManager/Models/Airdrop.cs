using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RestoreMonarchy.AirdropManager.Models
{
    public class Airdrop
    {
        [XmlAttribute("AirdropId")]
        public ushort AirdropId { get; set; }
        public List<AirdropItem> Items { get; set; }
    }
}
