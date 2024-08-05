using System.Xml.Serialization;

namespace RestoreMonarchy.AirdropManager.Models
{
    public class CustomAirdropItem
    {
        public CustomAirdropItem() { }
        public CustomAirdropItem(ushort itemId, int chance, string name)
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
