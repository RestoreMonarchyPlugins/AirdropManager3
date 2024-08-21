using RestoreMonarchy.AirdropManager3.Models;
using Rocket.API;
using Rocket.Core.Logging;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Text.RegularExpressions;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace RestoreMonarchy.AirdropManager3
{
    public partial class AirdropManager3Plugin
    {
        public void SendMessageToPlayer(IRocketPlayer player, string translationKey, params object[] placeholder)
        {
            if (player is ConsolePlayer)
            {
                string message = Translate(translationKey, placeholder);
                FormatMessage(ref message);
                message = Regex.Replace(message, "<.*?>", string.Empty);
                Logger.Log(message);
            }
            else
            {
                UnturnedPlayer unturnedPlayer = (UnturnedPlayer)player;
                SteamPlayer steamPlayer = unturnedPlayer?.SteamPlayer() ?? null;
                SendMessageToPlayer(steamPlayer, translationKey, placeholder);
            }
        }

        public void SendMessageToPlayer(string playerId, string translationKey, params object[] placeholder)
        {
            if (!ulong.TryParse(playerId, out ulong steamId))
            {
                return;
            }

            string message = Translate(translationKey, placeholder);
            FormatMessage(ref message);
            SteamPlayer steamPlayer = PlayerTool.getSteamPlayer(steamId);
            SendMessageToPlayer(steamPlayer, message);
        }

        public void SendMessageToPlayer(Player player, string translationKey, params object[] placeholder)
        {
            SendMessageToPlayer(player.channel.owner, translationKey, placeholder);
        }

        private void SendMessageToPlayer(SteamPlayer steamPlayer, string translationKey, params object[] placeholder)
        {
            if (steamPlayer == null)
            {
                return;
            }

            string message = Translate(translationKey, placeholder);
            FormatMessage(ref message);
            ChatManager.serverSendMessage(message, MessageColor, null, steamPlayer, EChatMode.SAY, null, true);
        }

        public void SendBroadcastMessage(Broadcast broadcast, Airdrop airdrop, AirdropSpawn airdropSpawn, IRocketPlayer player, Vector3 position)
        {
            if (broadcast == null || !broadcast.Enabled)
            {
                return;
            }

            string message = broadcast.Message;
            FormatMessage(ref message);
            message = message.Replace("{position}", position.ToString());
            message = message.Replace("{position_x}", position.x.ToString());
            message = message.Replace("{position_y}", position.y.ToString());
            message = message.Replace("{position_z}", position.z.ToString());
            message = message.Replace("{min_players}", Configuration.Instance.AirdropIntervalMinPlayers.ToString());
            if (player == null)
            {
                player = new ConsolePlayer();
            }
            message = message.Replace("{player}", player.DisplayName);
            if (airdropSpawn != null)
            {
                message = message.Replace("{spawn}", airdropSpawn.DisplayName());
                message = message.Replace("{spawn_name}", airdropSpawn.Name);
            }
            if (airdrop != null)
            {
                message = message.Replace("{airdrop}", airdrop.DisplayName());
                message = message.Replace("{airdrop_name}", airdrop.Name);
                message = message.Replace("{airdrop_speed}", airdrop.Speed.ToString());
                message = message.Replace("{airdrop_id}", airdrop.Id.ToString());
            }

            string iconUrl;
            if (string.IsNullOrEmpty(broadcast.IconUrl))
            {
                iconUrl = Configuration.Instance.Broadcasts.DefaultIconUrl;
            }
            else
            {
                iconUrl = broadcast.IconUrl;
            }

            ChatManager.serverSendMessage(message, MessageColor, null, null, EChatMode.SAY, iconUrl, true);
        }

        private void FormatMessage(ref string message)
        {
            message = message.Replace("[[", "<").Replace("]]", ">");
        }
    }
}
