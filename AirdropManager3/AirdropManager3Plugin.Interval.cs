using RestoreMonarchy.AirdropManager3.Models;
using SDG.Unturned;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RestoreMonarchy.AirdropManager3
{
    public partial class AirdropManager3Plugin
    {
        public DateTime? NextAirdrop { get; private set; }

        private IEnumerator AirdropIntervalCoroutine()
        {
            while (true)
            {
                if (NextAirdrop.HasValue && DateTime.Now >= NextAirdrop.Value)
                {
                    NextAirdrop = null;
                    int minPlayers = Configuration.Instance.AirdropIntervalMinPlayers;

                    if (Provider.clients.Count >= minPlayers)
                    {
                        if (AirdropSpawnsConfiguration.Instance.AirdropSpawns.Any())
                        {
                            AirdropSpawn airdropSpawn = AirdropSpawnsConfiguration.Instance.GetRandomAirdropSpawn();

                            DisableInfoLog = true;
                            Airdrop airdrop = Airdrop(airdropSpawn);
                            DisableInfoLog = false;

                            Broadcast broadcast = Configuration.Instance.Broadcasts.AirdropInterval;
                            SendBroadcastMessage(broadcast, airdrop, airdropSpawn, null, airdropSpawn.Vector3);

                            LogInfo($"Airdrop {airdrop.DisplayName()} called in by interval at {airdropSpawn.DisplayName()}.");
                        } else
                        {
                            LogInfo($"There isn't any airdrop spawns.");
                        }
                    }
                    else
                    {
                        Broadcast broadcast = Configuration.Instance.Broadcasts.AirdropIntervalMinPlayers;
                        SendBroadcastMessage(broadcast, null, null, null, default);

                        LogInfo($"Airdrop interval skipped: less than {minPlayers} players online.");
                    }
                }

                if (NextAirdrop == null)
                {
                    float intervalRange = Configuration.Instance.AirdropIntervalMax - Configuration.Instance.AirdropIntervalMin;
                    float randomInterval = Random.Range(0, intervalRange) + Configuration.Instance.AirdropIntervalMin;
                    NextAirdrop = DateTime.Now.AddSeconds(randomInterval);
                }

                yield return new WaitForSeconds(1);
            }
        }
    }
}
