using RestoreMonarchy.AirdropManager.Models;
using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestoreMonarchy.AirdropManager
{
    public class AirdropManagerConfiguration : IRocketPluginConfiguration
    {
        public List<Airdrop> Airdrops { get; set; }
        public void LoadDefaults()
        {
            Airdrops = new List<Airdrop>()
            {
                new Airdrop()
                {
                    Name = "Military",
                    Chance = 10,
                    Items = new List<ushort>()
                    {
                        363,
                        363,
                        6,
                        6,
                        6,
                        17,
                        15,
                        40,
                        40,
                        40,
                        1021
                    }
                }
            };
        }
    }
}
