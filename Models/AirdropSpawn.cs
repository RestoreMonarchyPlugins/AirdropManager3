using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RestoreMonarchy.AirdropManager.Models
{
    public class AirdropSpawn
    {
        [XmlAttribute("AirdropId")]
        public ushort AirdropId { get; set; }
        public Position Position { get; set; }
    }
}
