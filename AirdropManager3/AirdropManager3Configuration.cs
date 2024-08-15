using RestoreMonarchy.AirdropManager3.Models;
using Rocket.API;

namespace RestoreMonarchy.AirdropManager3
{
    public class AirdropManager3Configuration : IRocketPluginConfiguration
    {
        public bool Debug { get; set; }
        public string MessageColor { get; set; }

        public bool EnableAirdropInterval { get; set; }
        public float AirdropIntervalMin { get; set; }
        public float AirdropIntervalMax { get; set; }
        public byte AirdropIntervalMinPlayers { get; set; }

        public float DefaultAirdropSpeed { get; set; }
        public ushort DefaultAirdropStorageBarricadeId { get; set; }
        public byte DefaultAirdropStorageWidth { get; set; }
        public byte DefaultAirdropStorageHeight { get; set; }
        public string DefaultLandedEffectGuid { get; set; }

        public Broadcasts Broadcasts { get; set; }

        public void LoadDefaults()
        {
            Debug = false;
            MessageColor = "yellow";

            EnableAirdropInterval = true;
            AirdropIntervalMin = 2400;
            AirdropIntervalMax = 3600;
            AirdropIntervalMinPlayers = 5;

            DefaultAirdropSpeed = 128;
            DefaultAirdropStorageBarricadeId = 1374;
            DefaultAirdropStorageWidth = 7;
            DefaultAirdropStorageHeight = 7;
            DefaultLandedEffectGuid = "2c17fbd0f0ce49aeb3bc4637b68809a2";

            Broadcasts = new()
            {
                DefaultIconUrl = "https://i.imgur.com/kRIfsOg.png",
                AirdropCommand = new() { Enabled = true, Message = "Airdrop is on the way to [[b]]{spawn}![[/b]]" },
                AirdropHereCommand = new() { Enabled = true, Message = "[[b]]{player}[[/b]] ordered an airdrop at their position!" },
                MassAirdropCommand = new() { Enabled = true, Message = "[[b]]Mass airdrop is on the way![[/b]]" },
                AirdropGrenade = new() { Enabled = true, Message = "[[b]]{player}[[/b]] threw an airdrop grenade!" },
                AirdropInterval = new() { Enabled = true, Message = "Airdrop is on the way to [[b]]{spawn}![[/b]]" },
                AirdropIntervalMinPlayers = new() { Enabled = true, Message = "Airdrop skipped: less than [[b]]{min_players}[[/b]] players online." }
            };
        }

        public float GetAirdropSpeed(Airdrop airdrop) => airdrop.Speed == 0 ? DefaultAirdropSpeed : airdrop.Speed;
        public ushort GetAirdropStorageBarricadeId(Airdrop airdrop) => airdrop.Storage == null || airdrop.Storage.BarricadeId == 0 ? DefaultAirdropStorageBarricadeId : airdrop.Storage.BarricadeId;
        public byte GetAirdropStorageWidth(Airdrop airdrop) => airdrop.Storage == null || airdrop.Storage.Width == 0 ? DefaultAirdropStorageWidth : airdrop.Storage.Width;
        public byte GetAirdropStorageHeight(Airdrop airdrop) => airdrop.Storage == null || airdrop.Storage.Height == 0 ? DefaultAirdropStorageHeight : airdrop.Storage.Height;
        public string GetLandedEffectGuid(Airdrop airdrop) => string.IsNullOrEmpty(airdrop.LandedEffectGuid) ? DefaultLandedEffectGuid : airdrop.LandedEffectGuid;
    }
}
