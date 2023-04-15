using RestoreMonarchy.AirdropManager.Models;
using Rocket.API;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace RestoreMonarchy.AirdropManager
{
    public class AirdropManagerConfiguration : IRocketPluginConfiguration
    {
        public string MessageColor { get; set; }
        public bool UseDefaultSpawns { get; set; }
        public bool UseDefaultAirdrops { get; set; }
        public string AirdropMessageIcon { get; set; }
        public double AirdropInterval { get; set; }
        public ushort AirdropBarricadeId { get; set; }
        public float? AirdropSpeed { get; set; }
        public List<Airdrop> Airdrops { get; set; }
        [XmlArrayItem("AirdropSpawn")]
        public List<CustomAirdropSpawn> AirdropSpawns { get; set; }
        public List<ushort> BlacklistedAirdrops { get; set; }

        public void LoadDefaults()
        {
            MessageColor = "yellow";
            UseDefaultSpawns = true;
            UseDefaultAirdrops = false;
            AirdropMessageIcon = "https://i.imgur.com/JCDmlqI.png";
            AirdropInterval = 3600;
            AirdropBarricadeId = 1374;
            AirdropSpeed = 128;
            Airdrops = new List<Airdrop>() 
            { 
                new Airdrop() 
                { 
                    AirdropId = 13623, 
                    Items = new List<AirdropItem>() 
                    { 
                        new AirdropItem(363, 10), 
                        new AirdropItem(17, 20) 
                    } 
                } 
            };
            AirdropSpawns = new List<CustomAirdropSpawn>() 
            { 
                new CustomAirdropSpawn() 
                { 
                    AirdropId = 13623, 
                    Name = "Middle of the map", 
                    Position = new Position(0, 0, 0) 
                } 
            };
            BlacklistedAirdrops = new List<ushort>();
        }
    }
}
