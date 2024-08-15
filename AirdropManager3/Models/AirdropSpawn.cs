using UnityEngine;
using System.Xml.Serialization;

namespace RestoreMonarchy.AirdropManager3.Models
{
    public class AirdropSpawn
    {
        [XmlAttribute]
        public ushort AirdropId { get; set; }
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public float X { get; set; }
        [XmlAttribute]
        public float Y { get; set; }
        [XmlAttribute]
        public float Z { get; set; }

        [XmlIgnore]
        public Vector3 Vector3 => new(X, Y, Z);

        public string DisplayName() => string.IsNullOrEmpty(Name) ? Vector3.ToString() : Name;

        public bool ShouldSerializeAirdropId() => AirdropId != 0;
        private bool ShouldSerializeName() => !string.IsNullOrEmpty(Name);
    }
}
