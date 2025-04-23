using RestoreMonarchy.AirdropManager3.Models;
using Rocket.Unturned.Player;
using UnityEngine;

namespace RestoreMonarchy.AirdropManager3.Components
{
    public class AirdropGrenadeComponent : MonoBehaviour
    {
        private AirdropManager3Plugin pluginInstance => AirdropManager3Plugin.Instance;
        public Airdrop Airdrop { get; internal set; }
        public UnturnedPlayer Player { get; internal set; }

        void Start()
        {
            InvokeRepeating("CheckPosition", 0.5f, 0.5f);
        }

        private Vector3 position = Vector3.zero;

        void CheckPosition()
        {
            if (transform.position == position)
            {
                CancelInvoke("CheckPosition");

                Destroy(gameObject);
            }
            else
            {
                position = transform.position;
            }
        }

        void OnDestroy()
        {
            Vector3 position = transform.position;

            if (Airdrop != null)
            {
                pluginInstance.Airdrop(Airdrop, position, $"{Player.DisplayName} airdrop grenade");

                Broadcast broadcast = pluginInstance.Configuration.Instance.Broadcasts.AirdropGrenade;
                pluginInstance.SendBroadcastMessage(broadcast, Airdrop, null, Player, position);
            } else
            {
                int count = 0;
                pluginInstance.DisableInfoLog = true;
                foreach (AirdropSpawn airdropSpawn in pluginInstance.AirdropSpawnsConfiguration.Instance.AirdropSpawns)
                {
                    pluginInstance.Airdrop(airdropSpawn);
                    count++;
                }
                pluginInstance.DisableInfoLog = false;
                pluginInstance.LogInfo($"Mass airdrop called in by {Player.DisplayName} with airdrop grenade. {count} airdrops incoming!");

                Broadcast broadcast = pluginInstance.Configuration.Instance.Broadcasts.MassAirdropGrenade;
                pluginInstance.SendBroadcastMessage(broadcast, null, null, Player, default);
            }                
        }
    }
}
