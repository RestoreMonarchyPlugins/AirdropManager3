using RestoreMonarchy.AirdropManager3.Models;
using Rocket.API;
using System;
using System.Collections.Generic;

namespace RestoreMonarchy.AirdropManager3.Commands
{
    public class AirdropCommand : IRocketCommand
    {
        private AirdropManager3Plugin pluginInstance => AirdropManager3Plugin.Instance;

        public void Execute(IRocketPlayer caller, string[] command)
        {
            AirdropSpawn airdropSpawn;

            if (pluginInstance.AirdropSpawnsConfiguration.Instance.AirdropSpawns.Count == 0)
            {
                pluginInstance.SendMessageToPlayer(caller, "AirdropSpawnNone");
                return;
            }

            if (pluginInstance.AirdropsConfiguration.Instance.Airdrops.Count == 0)
            {
                pluginInstance.SendMessageToPlayer(caller, "AirdropNone");
                return;
            }


            if (command.Length == 0 || command[0].Equals("random", StringComparison.OrdinalIgnoreCase))
            {
                airdropSpawn = pluginInstance.AirdropSpawnsConfiguration.Instance.GetRandomAirdropSpawn();
            }
            else
            {
                if (!caller.HasPermission("airdrop.full"))
                {
                    pluginInstance.SendMessageToPlayer(caller, "AirdropNoPermission");
                    return;
                }
                string airdropSpawnName = command[0];
                airdropSpawn = pluginInstance.AirdropSpawnsConfiguration.Instance.GetAirdropSpawnByName(airdropSpawnName);

                if (airdropSpawn == null)
                {
                    pluginInstance.SendMessageToPlayer(caller, "AirdropSpawnNotFound", airdropSpawnName);
                    return;
                }
            }

            float speed = 0;
            if (command.Length > 1)
            {
                float.TryParse(command[1], out speed);
            }

            Airdrop airdrop = pluginInstance.Airdrop(airdropSpawn, speed);

            Broadcast broadcast = pluginInstance.Configuration.Instance.Broadcasts.AirdropCommand;
            pluginInstance.SendBroadcastMessage(broadcast, airdrop, airdropSpawn, caller, airdropSpawn.Vector3);

            if (caller is not ConsolePlayer)
            {
                pluginInstance.SendMessageToPlayer(caller, "AirdropSpawned", airdrop.DisplayName(), airdropSpawn.DisplayName());
            }
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public string Name => "airdrop";

        public string Help => "";

        public string Syntax => "<airdrop spawn name> [speed]";

        public List<string> Aliases => new();

        public List<string> Permissions => new();
    }
}
