using Rocket.API;
using Rocket.Core.Logging;
using System;
using System.Collections.Generic;

namespace RestoreMonarchy.AirdropManager.Commands
{
    public class AirdropCommand : IRocketCommand
    {
        public void Execute(IRocketPlayer caller, params string[] command)
        {
            bool isMass = false;
            if (command.Length > 0)
                bool.TryParse(command[0], out isMass);

            AirdropManagerPlugin.Instance.CallAirdrop(isMass);

            if (isMass)
                Logger.Log(AirdropManagerPlugin.Instance.Translate("SuccessMassAirdrop"), ConsoleColor.Yellow);
            else
                Logger.Log(AirdropManagerPlugin.Instance.Translate("SuccessAirdrop"), ConsoleColor.Yellow);
        }

        public string Help => "Calls in airdrop or mass airdrop";

        public string Name => "airdrop";

        public string Syntax => "[isMass]";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>() { "airdrop" };

        public AllowedCaller AllowedCaller => AllowedCaller.Both;
    }
}
