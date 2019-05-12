using RestoreMonarchy.AirdropManager.Helpers;
using Rocket.API;
using Rocket.Unturned.Chat;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RestoreMonarchy.AirdropManager.Commands
{
    public class WhenAirdropCommand : IRocketCommand
    {
        public void Execute(IRocketPlayer caller, params string[] command)
        {
            TimeSpan timeLeft = TimeSpan.FromTicks(AirdropManagerPlugin.Instance.AirdropTimerNext.Subtract(DateTime.Now).Ticks);           

            UnturnedChat.Say(caller, $"Next airdrop will be in {FormatingHelper.ToPrettyFormat(timeLeft)}", Color.green);
        }

        public string Help
        {
            get { return "Tells time left to next airdrop"; }
        }

        public string Name
        {
            get { return "whenairdrop"; }
        }

        public string Syntax
        {
            get { return ""; }
        }

        public List<string> Aliases
        {
            get { return new List<string>() { }; }
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>() { "whenairdrop" };
            }
        }


        public AllowedCaller AllowedCaller
        {
            get { return Rocket.API.AllowedCaller.Both; }
        }
    }
}
