using System.Xml.Serialization;

namespace RestoreMonarchy.AirdropManager.Models
{
    public class AirdropItem
    {
        public AirdropItem() { }
        public AirdropItem(ushort itemId, int chance, string name)
        {
            ItemId = itemId;
            Chance = chance;
            Name = name;
        }
        [XmlAttribute("ItemId")]
        public ushort ItemId { get; set; }

        [XmlAttribute("Chance")]
        public int Chance { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }
        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
    }
}
