using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace RestoreMonarchy.AirdropManager
{
    public class AirdropManagerConfiguration
    {
        public bool UseDefaultSpawns = true;
        public bool UseDefaultAirdrops = false;
        public string AirdropMessage = "<size=17><color=magenta><i>Airdrop</i> is coming!</color></size>";
        public string AirdropMessageIcon = "https://i.imgur.com/JCDmlqI.png"; 
        public double AirdropInterval = 3600;
        public List<Airdrop> Airdrops = new List<Airdrop>() { new Airdrop() { AirdropId = 1005,
            Items = new List<AirdropItem>() { new AirdropItem(363, 10), new AirdropItem(17, 20) } } };
        public List<AirdropSpawn> AirdropSpawns = new List<AirdropSpawn>() { new AirdropSpawn() { AirdropId = 1005, Position = Vector3.One } };
        public List<ushort> BlacklistedAirdrops = new List<ushort>();
    }

    public class Airdrop
    {
        public ushort AirdropId { get; set; }
        public List<AirdropItem> Items { get; set; }
    }

    public class AirdropItem
    {
        public AirdropItem(ushort itemId, int chance)
        {
            this.ItemId = itemId;
            this.Chance = chance;
        }

        public ushort ItemId { get; set; }
        public int Chance { get; set; }
    }

    public class AirdropSpawn
    {
        public ushort AirdropId { get; set; }
        public Vector3 Position { get; set; }
    }
}
