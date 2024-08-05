using SDG.Unturned;

namespace RestoreMonarchy.AirdropManager.Models
{
    public class Airdrop
    {
        public ushort Id { get; set; }
        public string Name { get; set; }


        public SpawnAsset SpawnAsset { get; set; }
        public CustomAirdrop CustomAirdrop { get; set; }
    }
}
