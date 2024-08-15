using RestoreMonarchy.AirdropManager3.Models;
using Rocket.API;
using Rocket.Core.Logging;
using System.Collections.Generic;

namespace RestoreMonarchy.AirdropManager3.Commands
{
    public class MassAirdropCommand : IRocketCommand
    {
        private AirdropManager3Plugin pluginInstance => AirdropManager3Plugin.Instance;

        public void Execute(IRocketPlayer caller, string[] command)
        {
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

            float speed = 0;

            if (command.Length > 0)
            {
                if (!caller.HasPermission("massairdrop.full"))
                {
                    pluginInstance.SendMessageToPlayer(caller, "MassAirdropNoPermission");
                    return;
                }

                float.TryParse(command[0], out speed);
            }

            int count = 0;
            pluginInstance.DisableInfoLog = true;
            foreach (AirdropSpawn airdropSpawn in pluginInstance.AirdropSpawnsConfiguration.Instance.AirdropSpawns)
            {
                pluginInstance.Airdrop(airdropSpawn, speed);
                count++;
            }
            pluginInstance.DisableInfoLog = false;
            pluginInstance.LogInfo($"Mass airdrop called in by {caller.DisplayName}.");

            Broadcast broadcast = pluginInstance.Configuration.Instance.Broadcasts.MassAirdropCommand;
            pluginInstance.SendBroadcastMessage(broadcast, null, null, caller, default);

            pluginInstance.SendMessageToPlayer(caller, "MassAirdropSpawned", count);
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public string Name => "massairdrop";

        public string Help => "";

        public string Syntax => "[speed]";

        public List<string> Aliases => new();

        public List<string> Permissions => new();
    }
}
