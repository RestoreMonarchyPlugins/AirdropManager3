using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace RestoreMonarchy.AirdropManager3.Models
{
    public class Airdrop
    {
        [XmlAttribute]
        public ushort Id { get; set; }
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public float Speed { get; set; }

        public AirdropStorage Storage { get; set; }
        public string LandedEffectGuid { get; set; }
        public AirdropGrenade Grenade { get; set; }

        [XmlArrayItem("Item")]
        public List<AirdropItem> Items { get; set; }

        public string DisplayName() => string.IsNullOrEmpty(Name) ? Id.ToString() : Name;

        public bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
        public bool ShouldSerializeSpeed() => Speed != 0;
        public bool ShouldSerializeStorage() => Storage != null;
        public bool ShouldSerializeLandedEffectGuid() => !string.IsNullOrEmpty(LandedEffectGuid);
        public bool ShouldSerializeGrenade() => Grenade != null;

        public AirdropItem RandomAirdropItem()
        {
            if (Items == null || Items.Count == 0)
            {
                return null;
            }                

            float totalWeight = 0;
            foreach (AirdropItem item in Items)
            {
                totalWeight += item.Weight;
            }                

            float randomWeight = new Random().Next(0, (int)totalWeight);
            float currentWeight = 0;

            foreach (var item in Items)
            {
                currentWeight += item.Weight;
                if (randomWeight <= currentWeight)
                {
                    return item;
                }   
            }

            return null;
        }
    }
}
