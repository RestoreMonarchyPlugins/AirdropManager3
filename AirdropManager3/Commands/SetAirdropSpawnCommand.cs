using RestoreMonarchy.AirdropManager3.Models;
using Rocket.API;
using Rocket.Unturned.Player;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RestoreMonarchy.AirdropManager3.Commands
{
    public class SetAirdropSpawnCommand : IRocketCommand
    {
        private AirdropManager3Plugin pluginInstance => AirdropManager3Plugin.Instance;

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;

            Airdrop airdrop = null;
            if (command.Length > 1)
            {
                airdrop = pluginInstance.AirdropsConfiguration.Instance.GetAirdropByNameOrId(command[1]);
                if (airdrop == null)
                {
                    pluginInstance.SendMessageToPlayer(player, "AirdropNotFound", command[1]);
                    return;
                }
            }

            string airdropSpawnName = command.ElementAtOrDefault(0);
            Vector3 position = player.Position;

            AirdropSpawn airdropSpawn = new()
            {
                AirdropId = airdrop?.Id ?? 0,
                Name = airdropSpawnName,
                X = position.x,
                Y = position.y,
                Z = position.z
            };
            
            pluginInstance.AirdropSpawnsConfiguration.Instance.AirdropSpawns.Add(airdropSpawn);
            pluginInstance.AirdropSpawnsConfiguration.Save();
            if (airdrop == null)
            {
                pluginInstance.SendMessageToPlayer(player, "AirdropSpawnSet", airdropSpawn.DisplayName());
            } else
            {
                pluginInstance.SendMessageToPlayer(player, "AirdropSpawnSetWithAirdrop", airdropSpawn.DisplayName(), airdrop.DisplayName());
            }
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "setairdropspawn";

        public string Help => "";

        public string Syntax => "[name] [airdrop id]";

        public List<string> Aliases => new();

        public List<string> Permissions => new();
    }
}