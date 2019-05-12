using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RestoreMonarchy.AirdropManager
{
    public class AirdropManagerConfiguration : IRocketPluginConfiguration
    {
        public bool UseDefaultSpawns;
        public bool UseDefaultAirdrops;
        public string AirdropMessage;
        public string AirdropMessageIcon;
        public double AirdropInterval;
        public List<Airdrop> Airdrops;
        public List<AirdropSpawn> AirdropSpawns;
        public List<ushort> BlacklistedAirdrops;

        public void LoadDefaults()
        {
            UseDefaultSpawns = true;
            UseDefaultAirdrops = false;
            AirdropMessage = "{size=17}{color=magenta}{i}Airdrop{/i} is coming!{/color}{/size}";
            AirdropMessageIcon = "https://i.imgur.com/JCDmlqI.png";
            AirdropInterval = 3600;
            Airdrops = new List<Airdrop>() { new Airdrop() { AirdropId = 1005, Items = new List<AirdropItem>() { new AirdropItem(363, 10), new AirdropItem(17, 20) } } };
            AirdropSpawns = new List<AirdropSpawn>() { new AirdropSpawn() { AirdropId = 1005, Position = new Position(1, 1, 1) } };
            BlacklistedAirdrops = new List<ushort>();
        }
    }

    public class Airdrop
    {
        public ushort AirdropId { get; set; }
        public List<AirdropItem> Items { get; set; }
    }

    public class AirdropItem
    {
        public AirdropItem() { }
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
        public Position Position { get; set; }
    }

    public class Position
    {
        public Position() { }
        public Position(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3 ToVector()
        {
            return new Vector3(X, Y, Z);
        }

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
    }
}
