using System.Collections.Generic;
using UnityEngine;

namespace RestoreMonarchy.AirdropManager.Models
{
    public class CustomAirdrop
    {

        public Airdrop Airdrop { get; private set; }

        public CustomAirdrop()
        {            
            List<Airdrop> airdrops = AirdropManagerPlugin.Instance.Configuration.Instance.Airdrops;

            if (airdrops.Count < 1)
                Airdrop = null;
            else
            {
                int value = 0;
                foreach (var airdrop in airdrops)
                {
                    value += airdrop.Chance;
                }

                int result = Random.Range(0, value);
                value = 0;
                foreach (var airdrop in airdrops)
                {
                    int previousValue = value;
                    value += airdrop.Chance;
                    if (result >= previousValue && result <= value)
                    {
                        Airdrop = airdrop;
                        break;
                    }
                }
            }
        }
    }
}
