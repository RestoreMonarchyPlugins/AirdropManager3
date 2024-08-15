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
            pluginInstance.Airdrop(Airdrop, position, $"{Player.DisplayName} airdrop grenade");

            Broadcast broadcast = pluginInstance.Configuration.Instance.Broadcasts.AirdropGrenade;
            pluginInstance.SendBroadcastMessage(broadcast, Airdrop, null, Player, position);
        }
    }
}
