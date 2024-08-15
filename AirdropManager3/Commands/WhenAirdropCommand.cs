using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestoreMonarchy.AirdropManager3.Commands
{
    public class WhenAirdropCommand : IRocketCommand
    {
        private AirdropManager3Plugin pluginInstance => AirdropManager3Plugin.Instance;

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (pluginInstance.NextAirdrop.HasValue)
            {
                TimeSpan timeLeft = pluginInstance.NextAirdrop.Value - DateTime.Now;
                string timeLeftString = pluginInstance.FormatTimespan(timeLeft);
                pluginInstance.SendMessageToPlayer(caller, "WhenAirdropTimeLeft", timeLeftString);
            } else
            {
                pluginInstance.SendMessageToPlayer(caller, "WhenAirdropNotPlanned");
            }
        }

        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public string Name => "whenairdrop";

        public string Help => "";

        public string Syntax => "";

        public List<string> Aliases => new();

        public List<string> Permissions => new();
    }
}
