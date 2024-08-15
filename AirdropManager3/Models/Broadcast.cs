using System.Xml.Serialization;

namespace RestoreMonarchy.AirdropManager3.Models
{
    public class Broadcast
    {
        [XmlAttribute]
        public string Message { get; set; }
        [XmlAttribute]
        public bool Enabled { get; set; }
        [XmlAttribute]
        public string IconUrl { get; set; }

        public bool ShouldSerializeIconUrl() => !string.IsNullOrEmpty(IconUrl);

    }
}
