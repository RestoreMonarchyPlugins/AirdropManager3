using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                UnturnedChat.Say(caller, "Format: /setairdrop <airdrop id>", Color.red);
                return;
            }

            if (ushort.TryParse(command[0], out airdropId))
            {
                UnturnedPlayer player = (UnturnedPlayer)caller;
                Vector3 position = player.Position;

                AirdropNode node;
                node = new AirdropNode(position, airdropId);

                AirdropManagerPlugin.Instance.Configuration.Instance.AirdropSpawns.Add(new AirdropSpawn() { AirdropId = airdropId, Position = new Position(position.x, position.y, position.z) });
                AirdropManagerPlugin.Instance.Configuration.Save();
                LevelNodes.nodes.Add(node);

                UnturnedChat.Say(caller, $"Successfully created an airdrop spawn {position.ToString()}", Color.green);

            } else
            {
                UnturnedChat.Say(caller, "Invalid airdropId", Color.red);
            }            
        }

        public string Help
        {
            get { return "Sets airdrop spawn in your position"; }
        }

        public string Name
        {
            get { return "setairdrop"; }
        }

        public string Syntax
        {
            get { return "<airdrop id>"; }
        }

        public List<string> Aliases
        {
            get { return new List<string>() { }; }
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>() { "setairdrop" };
            }
        }


        public AllowedCaller AllowedCaller
        {
            get { return Rocket.API.AllowedCaller.Both; }
        }
    }
}
