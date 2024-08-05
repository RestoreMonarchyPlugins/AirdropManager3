using RestoreMonarchy.AirdropManager.Models;
using Rocket.API;
using Rocket.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestoreMonarchy.AirdropManager.Commands
{
    public class AirdropCommand : IRocketCommand
    {
        private AirdropManagerPlugin pluginInstance => AirdropManagerPlugin.Instance;

        public void Execute(IRocketPlayer caller, params string[] command)
        {
            string airdropName = string.Join(" ", command);
            if (command.Length == 0)
            {
                pluginInstance.CallAirdrop();
                return;
            }

            CustomAirdrop airdrop = pluginInstance.Configuration.Instance.Airdrops.FirstOrDefault(x => x.Name.Equals(airdropName, StringComparison.OrdinalIgnoreCase));
            if (airdrop == null)
            {

            }
            pluginInstance.CallAirdrop();
        }

        public string Help => "Call in airdrop";

        public string Name => "airdrop";

        public string Syntax => "";

        public List<string> Aliases => new();

        public List<string> Permissions => new();

        public AllowedCaller AllowedCaller => AllowedCaller.Both;
    }
}
