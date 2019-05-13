using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestoreMonarchy.AirdropManager.Commands
{
    public class AirdropCommand : IRocketCommand
    {
        public void Execute(IRocketPlayer caller, params string[] command)
        {
            AirdropManagerPlugin.Instance.CallAirdrop();
        }

        public string Help
        {
            get { return "Calls in airdrop"; }
        }

        public string Name
        {
            get { return "airdrop"; }
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
                return new List<string>() { "airdrop" };
            }
        }


        public AllowedCaller AllowedCaller
        {
            get { return Rocket.API.AllowedCaller.Both; }
        }
    }
}
