using RestoreMonarchy.AirdropManager.Models;
using RestoreMonarchy.AirdropManager.Utilities;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Collections.Generic;
using UnityEngine;

namespace RestoreMonarchy.AirdropManager.Commands
{
    public class SetAirdropCommand : IRocketCommand
    {
        public void Execute(IRocketPlayer caller, params string[] command)
        {
            ushort airdropId = 0;

            if (command.Length < 1)
            {
                UnturnedChat.Say(caller, AirdropManagerPlugin.Instance.Translate("SetAirdropFormat"), Color.red);
                return;
            }

            if (ushort.TryParse(command[0], out airdropId))
            {
                UnturnedPlayer player = (UnturnedPlayer)caller;
                Vector3 position = player.Position;
                AirdropSpawn spawn = new AirdropSpawn() { AirdropId = airdropId, Position = new Position(position.x, position.y, position.z) };

                AirdropManagerUtility.AddAirdropSpawn(spawn);
                AirdropManagerPlugin.Instance.Configuration.Instance.AirdropSpawns.Add(spawn);
                AirdropManagerPlugin.Instance.Configuration.Save();

                UnturnedChat.Say(caller, AirdropManagerPlugin.Instance.Translate("SetAirdropSuccess"), Color.yellow);
            } else
            {
                UnturnedChat.Say(caller, AirdropManagerPlugin.Instance.Translate("SetAirdropInvalid"), Color.yellow);
            }            
        }

        public string Help => "Sets airdrop spawn in your position";

        public string Name => "setairdrop";

        public string Syntax => "<airdropId>";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();

        public AllowedCaller AllowedCaller => AllowedCaller.Player;
    }
}
