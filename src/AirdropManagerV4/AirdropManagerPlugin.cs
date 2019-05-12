using Rocket.API;
using Rocket.API.Collections;
using Rocket.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using UnityEngine;
using SDG.Unturned;
using Logger = Rocket.Core.Logging.Logger;

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
            AirdropTimer.Start();
            AirdropTimerNext = DateTime.Now.AddSeconds(Configuration.Instance.AirdropInterval);
            AirdropTimer.Elapsed += AirdropTimer_Elapsed;
            Logger.Log($"Timer started. Next airdrop {AirdropTimerNext.ToShortDateString()}");

            Logger.Log($"Creating your {Configuration.Instance.Airdrops.Count} airdrops...");

            foreach (Airdrop airdrop in Configuration.Instance.Airdrops)
            {
                if (Configuration.Instance.BlacklistedAirdrops.Contains(airdrop.AirdropId))
                    continue;

                SpawnAsset asset = new SpawnAsset(airdrop.AirdropId);

                foreach (AirdropItem item in airdrop.Items)
                {
                    asset.tables.Add(new SpawnTable() { assetID = item.ItemId, weight = item.Chance, chance = item.Chance });
                }

                Assets.add(asset, true);
            }

            Level.onLevelLoaded = (LevelLoaded)Delegate.Combine(Level.onLevelLoaded, new LevelLoaded(this.OnLevelLoaded));

            Instance = this;
            Logger.Log($"AirdropManager has been loaded!");
            Logger.Log($"Version: 2.0");
            Logger.Log($"Made by MCrow");
        }

        private void AirdropTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            CallAirdrop();
            AirdropTimer.Start();
            AirdropTimerNext = DateTime.Now.AddSeconds(Configuration.Instance.AirdropInterval);
        }

        public void CallAirdrop()
        {
            System.Random random = new System.Random();
            List<AirdropNode> list = new List<AirdropNode>();
            if (Configuration.Instance.UseDefaultSpawns)
            {
                foreach (Node node in LevelNodes.nodes)
                {
                    if (node.type == ENodeType.AIRDROP)
                    {
                        AirdropNode airdropNode = (AirdropNode)node;
                        list.Add(airdropNode);
                    }
                }
                int num = random.Next(list.Count);
                ushort airdropId = list[num].id;
                if (!Configuration.Instance.UseDefaultAirdrops)
                {
                    if (Configuration.Instance.Airdrops.Count == 0)
                    {
                        Logger.LogWarning("There isn't any custom airdrop, airdrop canceled.");
                        return;
                    }
                    airdropId = Configuration.Instance.Airdrops[random.Next(Configuration.Instance.Airdrops.Count)].AirdropId;
                }
                LevelManager.airdrop(list[num].point, airdropId, 100);
            }
            else
            {
                if (Configuration.Instance.AirdropSpawns.Count == 0)
                {
                    Logger.LogWarning("There isn't any airdrop spawn, airdrop canceled.");
                    return;
                }

                int num = random.Next(Configuration.Instance.AirdropSpawns.Count);

                LevelManager.airdrop(Configuration.Instance.AirdropSpawns[num].Position.ToVector(),
                    Configuration.Instance.AirdropSpawns[num].AirdropId, 100);
            }
            ChatManager.serverSendMessage(Configuration.Instance.AirdropMessage.Replace('{', '<').Replace('}', '>'), Color.white, null, null, EChatMode.SAY, Configuration.Instance.AirdropMessageIcon, true);
        }

        public void OnLevelLoaded(int level)
        {
            Logger.Log($"Creating your {Configuration.Instance.AirdropSpawns.Count} airdrop spawns...");

            foreach (AirdropSpawn spawn in Configuration.Instance.AirdropSpawns)
            {
                AirdropNode airdropNode;
                if (spawn.AirdropId == 0)
                {
                    airdropNode = new AirdropNode(spawn.Position.ToVector());
                }
                else
                {
                    airdropNode = new AirdropNode(spawn.Position.ToVector(), spawn.AirdropId);
                }
                LevelNodes.nodes.Add(airdropNode);
            }
        }
    }
}
