using System.Collections.Generic;
using System.Xml.Serialization;

namespace RestoreMonarchy.AirdropManager.Models
{
    public class Airdrop
    {
        [XmlAttribute]
        public ushort AirdropId { get; set; }

        [XmlAttribute]
        public byte StorageSizeX { get; set; }
        public bool ShouldSerializeStorageSizeX() => StorageSizeX != 0;
        [XmlAttribute]
        public byte StorageSizeY { get; set; }
        public bool ShouldSerializeStorageSizeY() => StorageSizeY != 0;

        public List<AirdropItem> Items { get; set; }
        public bool ShouldSerializeItems() => Items != null && Items.Count > 0;

        public List<AirdropItem2> Items2 { get; set; }
        public bool ShouldSerializeItems2() => Items2 != null && Items2.Count > 0;
    }
}
