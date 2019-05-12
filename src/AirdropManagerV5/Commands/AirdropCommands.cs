using Rocket.API.Commands;
using Rocket.API.User;
using Rocket.Core.Commands;
using Rocket.Core.User;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Random = System.Random;
using Color = System.Drawing.Color;
using System.Numerics;
using Rocket.UnityEngine.Extensions;
using RestoreMonarchy.AirdropManager.Helpers;
using Rocket.API.Player;

namespace RestoreMonarchy.AirdropManager.Commands
{
    public class AirdropCommands
    {
        private AirdropManagerPlugin airdropManagerInstance;
        public AirdropCommands(AirdropManagerPlugin airdropManagerInstance)
        {
            this.airdropManagerInstance = airdropManagerInstance;
        }

        [Command(Summary = "Creates an airdrop spawn in your position", Name = "setairdrop")]
        [CommandUser(typeof(IPlayer))]
        public async Task SetAirdropCommandAsync(ICommandContext context, IUser sender, ushort airdropId)
        {
            UnturnedUser unturnedUser = (UnturnedUser)sender;
            Vector3 position = unturnedUser.Player.Entity.Position;

            AirdropNode node;
            node = new AirdropNode(unturnedUser.Player.NativePlayer.transform.position, airdropId);

            airdropManagerInstance.ConfigurationInstance.AirdropSpawns.Add(new AirdropSpawn() { AirdropId = airdropId, Position = position });
            await airdropManagerInstance.Configuration.SaveAsync();
            LevelNodes.nodes.Add(node);            

            await sender.SendMessageAsync($"Successfully created an airdrop spawn {position.ToString()}", System.Drawing.Color.Lime);
        }

        [Command(Summary = "Calls in an airdrop", Name = "airdrop")]
        public async Task AirdropCommandAsync(ICommandContext context, IUser sender)
        {
            UnturnedUser unturnedUser = (UnturnedUser)sender;

            airdropManagerInstance.CallAirdrop();
        }

        [Command(Summary = "Checks when will be next airdrop", Name = "whenairdrop")]
        public async Task NextAirdropCommandAsync(ICommandContext context, IUser sender)
        {            
            TimeSpan timeLeft = TimeSpan.FromTicks(airdropManagerInstance.AirdropTimerNext.Subtract(DateTime.Now).Ticks);

            await sender.SendMessageAsync(await airdropManagerInstance.Translations.GetAsync("NextAirdrop", FormatingHelper.ToPrettyFormat(timeLeft)), Color.LightGreen);
        }
    }

    
}
