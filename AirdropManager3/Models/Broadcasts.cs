using System.Xml.Serialization;

namespace RestoreMonarchy.AirdropManager3.Models
{
    public class Broadcasts
    {
        [XmlAttribute]
        public string DefaultIconUrl { get; set; }
        public bool ShouldSerializeDefaultIconUrl() => !string.IsNullOrEmpty(DefaultIconUrl);

        public Broadcast AirdropCommand { get; set; }
        public Broadcast AirdropHereCommand { get; set; }
        public Broadcast MassAirdropCommand { get; set; }
        public Broadcast AirdropGrenade { get; set; }
        public Broadcast AirdropInterval { get; set; }
        public Broadcast AirdropIntervalMinPlayers { get; set; }
    }
}
