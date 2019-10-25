using RestoreMonarchy.AirdropManager.Helpers;
using Rocket.API;
using Rocket.Unturned.Chat;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RestoreMonarchy.AirdropManager.Commands
{
    public class WhenAirdropCommand : IRocketCommand
    {
        public void Execute(IRocketPlayer caller, params string[] command)
        {
            TimeSpan timeLeft = TimeSpan.FromSeconds(AirdropManagerPlugin.Instance.AirdropTimerNext.Subtract(DateTime.Now).TotalSeconds);
            UnturnedChat.Say(caller, AirdropManagerPlugin.Instance.Translations.Instance.Translate("NextAirdrop", FormatingHelper.ToPrettyFormat(timeLeft)), Color.green);
        }

        public string Help => "Displays time left to next airdrop";

        public string Name => "whenairdrop";

        public string Syntax => string.Empty;

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();

        public AllowedCaller AllowedCaller => AllowedCaller.Both;
    }
}
