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
        public float AirdropIntervalMin { get; set; } = 2400;
        public float AirdropIntervalMax { get; set; } = 3600;
        public ushort AirdropBarricadeId { get; set; }
        public float? AirdropSpeed { get; set; }
        [XmlArrayItem("Airdrop")]
        public List<CustomAirdrop> Airdrops { get; set; }
        [XmlArrayItem("AirdropSpawn")]
        public List<CustomAirdropSpawn> AirdropSpawns { get; set; }
        [XmlArrayItem("BlacklistedAirdrop")]
        public List<ushort> BlacklistedAirdrops { get; set; }

        public void LoadDefaults()
        {
            MessageColor = "yellow";
            UseDefaultSpawns = true;
            UseDefaultAirdrops = false;
            AirdropMessageIcon = "https://i.imgur.com/JCDmlqI.png";
            AirdropIntervalMin = 2400;
            AirdropIntervalMax = 3600;
            AirdropBarricadeId = 0;
            AirdropSpeed = 128;
            Airdrops = new List<CustomAirdrop>() 
            { 
                new CustomAirdrop() 
                {
                    AirdropId = 13623,
                    Name = "Military",
                    Items = new List<CustomAirdropItem>() 
                    { 
                        new CustomAirdropItem(363, 10, "Maplestrike"), 
                        new CustomAirdropItem(17, 20, "Military Drum") 
                    } 
                },
                new CustomAirdrop()
                {
                    AirdropId = 13624,
                    StorageSizeX = 7,
                    StorageSizeY = 3,
                    Items2 = new List<CustomAirdropItem2>()
                    {
                        new CustomAirdropItem2(132, 1, "Dragonfang"),
                        new CustomAirdropItem2(133, 1, "Dragonfang Box"),
                        new CustomAirdropItem2(254, 5, "Fragmentation Grenade")
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