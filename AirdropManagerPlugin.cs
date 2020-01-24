using Rocket.API.Collections;
using Rocket.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using SDG.Unturned;
using Logger = Rocket.Core.Logging.Logger;
using Random = System.Random;
using System.Reflection;
using RestoreMonarchy.AirdropManager.Utilities;
using Rocket.Unturned.Chat;
using RestoreMonarchy.AirdropManager.Models;
using Rocket.Core.Utils;

namespace RestoreMonarchy.AirdropManager
{
    public class AirdropManagerPlugin : RocketPlugin<AirdropManagerConfiguration>
    {
        public static AirdropManagerPlugin Instance { get; set; }
        public Timer AirdropTimer { get; set; }
        public DateTime AirdropTimerNext { get; set; }
        public Color MessageColor { get; set; }

        public override TranslationList DefaultTranslations =>  new TranslationList()
        {
            { "NextAirdrop", "Next airdrop will be in {0}" },
            { "SuccessAirdrop", "Successfully called in airdrop!" },
            { "SuccessMassAirdrop", "Successfully called in mass airdrop!" },            
            { "Airdrop", "{size=17}{color=magenta}{i}Airdrop{/i} is coming!{/color}{/size}" },
            { "MassAirdrop", "{size=17}{color=magenta}{i}Mass Airdrop{/i} is coming!{/color}{/size}" },
            { "SetAirdropFormat", "Format: /setairdrop <AirdropID>" },
            { "SetAirdropSuccess", "Successfully set an airdrop spawn at your position!" },
            { "SetAirdropInvalid", "Invalid AirdropID" }
        };

        protected override void Load()
        {
            Instance = this;
            MessageColor = UnturnedChat.GetColorFromName(Configuration.Instance.MessageColor, Color.green);
            AirdropTimer = new Timer(Configuration.Instance.AirdropInterval * 1000);
            AirdropTimer.Elapsed += AirdropTimer_Elapsed;
            AirdropTimer.AutoReset = true;
            AirdropTimer.Start();
            AirdropTimerNext = DateTime.Now.AddSeconds(Configuration.Instance.AirdropInterval);

            foreach (Airdrop airdrop in Configuration.Instance.Airdrops)
            {
                if (Configuration.Instance.BlacklistedAirdrops.Contains(airdrop.AirdropId))
                    continue;

                SpawnAsset asset = new SpawnAsset(airdrop.AirdropId);
                foreach (AirdropItem item in airdrop.Items)
                {
                    asset.tables.Add(new SpawnTable()
                    {
                        assetID = item.ItemId,
                        weight = item.Chance,
                        spawnID = 0,
                        chance = 0
                    });
                }

                // Setting this to true solved the issue with only last time being dropped
                asset.GetType().GetProperty("areTablesDirty", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).SetValue(asset, new object[0]);
                Assets.add(asset, true);
            }

            Level.onLevelLoaded += OnLevelLoaded;

            Logger.Log($"{Name} {Assembly.GetName().Version} has been loaded!", ConsoleColor.Yellow);
            Logger.Log($"Brought to You by RestoreMonarchy.com", ConsoleColor.Yellow);
        }

        protected override void Unload()
        {
            Level.onLevelLoaded -= OnLevelLoaded;
            AirdropTimer.Elapsed -= AirdropTimer_Elapsed;
            Logger.Log($"{Name} has been unloaded!", ConsoleColor.Yellow);
        }

        private void AirdropTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // Make sure it's executed on the main thread
            TaskDispatcher.QueueOnMainThread(() => 
            {
                CallAirdrop();
                AirdropTimerNext = DateTime.Now.AddSeconds(Configuration.Instance.AirdropInterval);
                Logger.Log("Airdrop has been sent by a timer!", ConsoleColor.Yellow);
            });
        }

        public void CallAirdrop(bool isMass = false)
        {
            if (isMass)
            {
                foreach (var airdrop in typeof(LevelManager).GetField("airdropNodes", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) as List<AirdropNode>)
                {
                    LevelManager.airdrop(airdrop.point, airdrop.id, Provider.modeConfigData.Events.Airdrop_Speed);
                }
                ChatManager.serverSendMessage(Translate("MassAirdrop").ToRich(), Color.white, null, null, EChatMode.SAY, Configuration.Instance.AirdropMessageIcon, true);
            }
            else
            {
                ChatManager.serverSendMessage(Translate("Airdrop").ToRich(), Color.white, null, null, EChatMode.SAY, Configuration.Instance.AirdropMessageIcon, true);
                LevelManager.airdropFrequency = 0;
            }
        }

        public void OnLevelLoaded(int level)
        {
            var field = typeof(LevelManager).GetField("airdropNodes", BindingFlags.Static | BindingFlags.NonPublic);
            List<AirdropNode> nodes = field.GetValue(null) as List<AirdropNode>;

            if (Configuration.Instance.UseDefaultSpawns)
            {
                if (!Configuration.Instance.UseDefaultAirdrops)
                {
                    Random random = new Random();
                    foreach (AirdropNode node in nodes)
                    {
                        node.id = Configuration.Instance.Airdrops[random.Next(Configuration.Instance.Airdrops.Count)].AirdropId;
                    }
                }
            }
            else
            {
                nodes = new List<AirdropNode>();
            }

            foreach (AirdropSpawn spawn in Configuration.Instance.AirdropSpawns)
            {
                AirdropManagerUtility.AddAirdropToNodes(nodes, spawn);
            }

            field.SetValue(null, nodes);
        }       
    }
}