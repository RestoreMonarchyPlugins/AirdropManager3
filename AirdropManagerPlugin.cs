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

namespace RestoreMonarchy.AirdropManager
{
    public class AirdropManagerPlugin : RocketPlugin<AirdropManagerConfiguration>
    {
        public static AirdropManagerPlugin Instance { get; set; }
        public Timer AirdropTimer { get; set; }
        public DateTime AirdropTimerNext { get; set; }  

        public override TranslationList DefaultTranslations
        {
            get
            {
                return new TranslationList(){
                    {"Next_Airdrop","Next airdrop will be in {0}"}
                };
            }
        }

        protected override void Load()
        {
            AirdropTimer = new Timer(Configuration.Instance.AirdropInterval * 1000);
            AirdropTimer.Elapsed += AirdropTimer_Elapsed;
            AirdropTimer.AutoReset = true;

            AirdropTimerNext = DateTime.Now.AddSeconds(Configuration.Instance.AirdropInterval);
            AirdropTimer.Start();
            Logger.Log($"Timer started. Next airdrop {AirdropTimerNext.ToShortTimeString()}");

            Logger.Log($"Creating your {Configuration.Instance.Airdrops.Count} airdrops...");

            foreach (Airdrop airdrop in Configuration.Instance.Airdrops)
            {
                if (Configuration.Instance.BlacklistedAirdrops.Contains(airdrop.AirdropId))
                    continue;

                SpawnAsset asset = new SpawnAsset(airdrop.AirdropId);

                foreach (AirdropItem item in airdrop.Items)
                {
                    asset.tables.Add(new SpawnTable() { assetID = item.ItemId, weight = item.Chance });
                }

                Assets.add(asset, true);
            }

            Level.onLevelLoaded += OnLevelLoaded;

            Instance = this;
            Logger.Log($"AirdropManager has been loaded!");
            Logger.Log($"Version: 2.0");
            Logger.Log($"Made by RestoreMonarchy.com");
        }

        private void AirdropTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            CallAirdrop();
            AirdropTimerNext = DateTime.Now.AddSeconds(Configuration.Instance.AirdropInterval);
        }

        public void CallAirdrop()
        {
            LevelManager.airdropFrequency = 0u;
            ChatManager.serverSendMessage(Configuration.Instance.AirdropMessage.Replace('{', '<').Replace('}', '>'), Color.white, null, null, 
                EChatMode.SAY, Configuration.Instance.AirdropMessageIcon, true);
        }

        public void OnLevelLoaded(int level)
        {
            Logger.Log($"Creating your {Configuration.Instance.AirdropSpawns.Count} airdrop spawns...");
            
            var field = typeof(LevelManager).GetField("airdropNodes", BindingFlags.Static | BindingFlags.NonPublic);
            List<AirdropNode> airdropNodes = field.GetValue(null) as List<AirdropNode>;            

            if (Configuration.Instance.UseDefaultSpawns)
            {
                if (!Configuration.Instance.UseDefaultAirdrops)
                {
                    Random random = new Random();                    
                    foreach (AirdropNode airdropNode in airdropNodes)
                    {                        
                        airdropNode.id = Configuration.Instance.Airdrops[random.Next(Configuration.Instance.Airdrops.Count)].AirdropId;
                    }
                }
            } else
            {
                airdropNodes = new List<AirdropNode>();
            }

            foreach (AirdropSpawn spawn in Configuration.Instance.AirdropSpawns)
            {
                if (spawn.AirdropId == 0)
                    airdropNodes.Add(new AirdropNode(spawn.Position.ToVector()));
                else
                    airdropNodes.Add(new AirdropNode(spawn.Position.ToVector(), spawn.AirdropId));               
            }

            field.SetValue(null, airdropNodes);
        }
    }
}