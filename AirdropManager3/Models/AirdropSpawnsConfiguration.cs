using System;
using System.Collections.Generic;

namespace RestoreMonarchy.AirdropManager3.Models
{
    public class AirdropSpawnsConfiguration
    {
        public List<AirdropSpawn> AirdropSpawns { get; set; }

        public AirdropSpawn GetRandomAirdropSpawn()
        {
            if (AirdropSpawns.Count == 0)
            {
                return null;
            }

            return AirdropSpawns[UnityEngine.Random.Range(0, AirdropSpawns.Count)];
        }

        public AirdropSpawn GetAirdropSpawnByName(string name)
        {
            return AirdropSpawns.Find(x => x.Name != null && x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
