using Rocket.API;
using Rocket.Core.Logging;
using System;
using System.Collections.Generic;

namespace RestoreMonarchy.AirdropManager.Commands
{
    public class MassAirdropCommand : IRocketCommand
    {
        private AirdropManagerPlugin pluginInstance => AirdropManagerPlugin.Instance;

        public void Execute(IRocketPlayer caller, string[] command)
        {
            pluginInstance.CallAirdrop(false);
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public string Name => "massairdrop";

        public string Help => "Calls an airdrop in every spawn possible";

        public string Syntax => "";

        public List<string> Aliases => new();

        public List<string> Permissions => new();
    }
}
