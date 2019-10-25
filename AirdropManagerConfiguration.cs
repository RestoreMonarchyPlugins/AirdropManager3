using RestoreMonarchy.AirdropManager.Models;
using Rocket.API;
using System.Collections.Generic;

namespace RestoreMonarchy.AirdropManager
{
    public class AirdropManagerConfiguration : IRocketPluginConfiguration
    {
        public string MessageColor { get; set; }
        public bool UseDefaultSpawns;
        public bool UseDefaultAirdrops;
        public string AirdropMessageIcon;
        public double AirdropInterval;
        public List<Airdrop> Airdrops;
        public List<AirdropSpawn> AirdropSpawns;
        public List<ushort> BlacklistedAirdrops;

        public void LoadDefaults()
        {
            MessageColor = "yellow";
            UseDefaultSpawns = true;
            UseDefaultAirdrops = false;
            AirdropMessageIcon = "https://i.imgur.com/JCDmlqI.png";
            AirdropInterval = 3600;
            Airdrops = new List<Airdrop>() { new Airdrop() { AirdropId = 1005, Items = new List<AirdropItem>() { new AirdropItem(363, 10), new AirdropItem(17, 20) } } };
            AirdropSpawns = new List<AirdropSpawn>() { new AirdropSpawn() { AirdropId = 1005, Position = new Position(1, 1, 1) } };
            BlacklistedAirdrops = new List<ushort>();
        }
    }
}
