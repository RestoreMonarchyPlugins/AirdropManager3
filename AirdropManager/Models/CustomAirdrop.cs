using System.Collections.Generic;
using UnityEngine;

namespace RestoreMonarchy.AirdropManager.Models
{
    public class CustomAirdrop
    {
        // I'll use this type in future to compute how much space in storage is needed to store all items in it

        public Airdrop Airdrop { get; private set; }

        public CustomAirdrop()
        {
            List<Airdrop> airdrops = AirdropManagerPlugin.Instance.Configuration.Instance.Airdrops;
            Airdrop = airdrops[Random.Range(0, airdrops.Count - 1)];
        }
    }
}
