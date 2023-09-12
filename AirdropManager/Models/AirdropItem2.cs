using System.Xml.Serialization;

namespace RestoreMonarchy.AirdropManager.Models
{
    public class AirdropItem2
    {
        public AirdropItem2() { }
        public AirdropItem2(ushort itemId, int quantity)
        {
            ItemId = itemId;
            Quantity = quantity;
        }

        [XmlAttribute]
        public ushort ItemId { get; set; }
        [XmlAttribute]
        public int Quantity { get; set; }
    }
}
