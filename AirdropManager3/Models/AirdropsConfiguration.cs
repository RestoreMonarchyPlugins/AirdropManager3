using System.Collections.Generic;

namespace RestoreMonarchy.AirdropManager3.Models
{
    public class AirdropsConfiguration
    {
        public List<Airdrop> Airdrops { get; set; }

        public Airdrop GetRandomAirdrop()
        {
            if (Airdrops.Count == 0)
            {
                return null;
            }                

            return Airdrops[UnityEngine.Random.Range(0, Airdrops.Count)];
        }

        public Airdrop GetAirdropById(ushort airdropId)
        {
            return Airdrops.Find(x => x.Id == airdropId);
        }

        public Airdrop GetAirdropByName(string name)
        {
            return Airdrops.Find(x => x.Name == name);
        }

        public Airdrop GetAirdropByNameOrId(string nameOrId)
        {
            if (ushort.TryParse(nameOrId, out ushort airdropId))
            {
                return GetAirdropById(airdropId);
            }

            return GetAirdropByName(nameOrId);
        }

        public Airdrop GetAirdropByGrenadeId(ushort grenadeId)
        {
            return Airdrops.Find(x => x.Grenade != null && x.Grenade.Id == grenadeId);
        }
    }
}
