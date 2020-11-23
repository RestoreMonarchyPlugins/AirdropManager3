using RestoreMonarchy.AirdropManager.Models;
using Rocket.API;
using System.Collections.Generic;

namespace RestoreMonarchy.AirdropManager
{
    public class AirdropManagerConfiguration : IRocketPluginConfiguration
    {
        public string MessageColor { get; set; }
        public bool UseDefaultSpawns { get; set; }
        public bool UseDefaultAirdrops { get; set; }
        public string AirdropMessageIcon { get; set; }
        public double AirdropInterval { get; set; }
        public List<Airdrop> Airdrops { get; set; }
        public List<AirdropSpawn> AirdropSpawns { get; set; }
        public List<ushort> BlacklistedAirdrops { get; set; }

        public void LoadDefaults()
        {
            MessageColor = "yellow";
            UseDefaultSpawns = true;
            UseDefaultAirdrops = false;
            AirdropMessageIcon = "https://i.imgur.com/JCDmlqI.png";
            AirdropInterval = 3600;
            Airdrops = new List<Airdrop>() { new Airdrop() { AirdropId = 1005, Items = new List<AirdropItem>() { new AirdropItem(363, 10), new AirdropItem(17, 20) } } };
            AirdropSpawns = new List<AirdropSpawn>() { new AirdropSpawn() { AirdropId = 1005, Name = "Taylor Beach", Position = new Position(1, 1, 1) } };
            BlacklistedAirdrops = new List<ushort>();
        }
    }
}
