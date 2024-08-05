using System.Xml.Serialization;

namespace RestoreMonarchy.AirdropManager.Models
{
    public class CustomAirdropItem2
    {
        public CustomAirdropItem2() { }
        public CustomAirdropItem2(ushort itemId, int quantity, string name)
        {
            ItemId = itemId;
            Quantity = quantity;
            Name = name;
        }

        [XmlAttribute]
        public ushort ItemId { get; set; }
        [XmlAttribute]
        public int Quantity { get; set; }
        [XmlAttribute]
        public string Name { get; set; }

        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
    }
}
