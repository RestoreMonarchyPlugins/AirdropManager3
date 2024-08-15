using RestoreMonarchy.AirdropManager3.Models;
using Rocket.API;
using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RestoreMonarchy.AirdropManager3.Commands
{
    public class AirdropHereCommand : IRocketCommand
    {
        private AirdropManager3Plugin pluginInstance => AirdropManager3Plugin.Instance;

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (!pluginInstance.AirdropsConfiguration.Instance.Airdrops.Any())
            {
                pluginInstance.SendMessageToPlayer(caller, "AirdropNone");
                return;
            }

            Airdrop airdrop;

            if (command.Length == 0 || command[0].Equals("random", StringComparison.OrdinalIgnoreCase))
            {
                airdrop = pluginInstance.AirdropsConfiguration.Instance.GetRandomAirdrop();
            }
            else
            {
                if (!caller.HasPermission("airdrophere.full"))
                {
                    pluginInstance.SendMessageToPlayer(caller, "AirdropHereNoPermission");
                    return;
                }

                string airdropNameOrId = command[0];
                airdrop = pluginInstance.AirdropsConfiguration.Instance.GetAirdropByNameOrId(airdropNameOrId);

                if (airdrop == null)
                {
                    pluginInstance.SendMessageToPlayer(caller, "AirdropNotFound", airdropNameOrId);
                    return;
                }
            }

            float speed = 0;
            if (command.Length > 1)
            {
                float.TryParse(command[1], out speed);
            }

            UnturnedPlayer player = (UnturnedPlayer)caller;

            Vector3 position = player.Position;
            pluginInstance.Airdrop(airdrop, position, $"{caller.DisplayName} position", speed);

            Broadcast broadcast = pluginInstance.Configuration.Instance.Broadcasts.AirdropHereCommand;
            pluginInstance.SendBroadcastMessage(broadcast, airdrop, null, caller, position);

            pluginInstance.SendMessageToPlayer(caller, "AirdropHereSpawned", airdrop.DisplayName(), position);
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "airdrophere";

        public string Help => "";

        public string Syntax => "<airdrop id> [speed]";

        public List<string> Aliases => new();

        public List<string> Permissions => new();
    }
}
