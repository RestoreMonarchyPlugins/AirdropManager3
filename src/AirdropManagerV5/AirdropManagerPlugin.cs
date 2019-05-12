using RestoreMonarchy.AirdropManager.Commands;
using Rocket.API.DependencyInjection;
using Rocket.API.Logging;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using Rocket.UnityEngine.Extensions;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.Timers;
using UnityEngine;
using ILogger = Rocket.API.Logging.ILogger;

namespace RestoreMonarchy.AirdropManager
{
    public class AirdropManagerPlugin : Plugin<AirdropManagerConfiguration>
    {
        private readonly ILogger logger;
        public Timer AirdropTimer { get; set; }

        public DateTime AirdropTimerNext { get; set; }

        public AirdropManagerPlugin(IDependencyContainer container, ILogger logger) : base("AirdropManager", container)
        {
            this.logger = logger;   
        }

        public override Dictionary<string, string> DefaultTranslations => new Dictionary<string, string>
        {
            {"NextAirdrop", "Next airdrop will be in {0}!"}
        };

        protected override async Task OnActivate(bool isFromReload)
        {
            logger.LogInformation($"Starting airdrop timer...");
            AirdropTimer = new Timer(ConfigurationInstance.AirdropInterval * 1000);
            AirdropTimer.Start();
            AirdropTimerNext = DateTime.Now.AddSeconds(ConfigurationInstance.AirdropInterval);
            AirdropTimer.Elapsed += AirdropTimer_Elapsed;

            logger.LogInformation($"Loading {ConfigurationInstance.Airdrops.Count} airdrops...");
            foreach (Airdrop airdrop in ConfigurationInstance.Airdrops)
            {
                if (ConfigurationInstance.BlacklistedAirdrops.Contains(airdrop.AirdropId))
                    continue;

                SpawnAsset asset = new SpawnAsset(airdrop.AirdropId);

                foreach (AirdropItem item in airdrop.Items)
                {
                    asset.tables.Add(new SpawnTable() { assetID = item.ItemId, weight = item.Chance, chance = item.Chance });
                }

                Assets.add(asset, true);
            }

            AirdropCommands airdropCommands = new AirdropCommands(this);
            RegisterCommands(airdropCommands);

            Level.onLevelLoaded = (LevelLoaded)Delegate.Combine(Level.onLevelLoaded, new LevelLoaded(this.OnLevelLoaded));

            logger.LogInformation($"{Assembly.GetExecutingAssembly().GetName().Name} has been loaded!");
            logger.LogInformation($"Version: {Assembly.GetExecutingAssembly().GetName().Version}");
            logger.LogInformation($"Made by MCrow");
        }

        private void AirdropTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            CallAirdrop();
            AirdropTimer.Start();
            AirdropTimerNext = DateTime.Now.AddSeconds(ConfigurationInstance.AirdropInterval);
        }

        public void CallAirdrop()
        {

            System.Random random = new System.Random();
            List<AirdropNode> list = new List<AirdropNode>();
            if (ConfigurationInstance.UseDefaultSpawns)
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
                if (!ConfigurationInstance.UseDefaultAirdrops)
                {
                    if (ConfigurationInstance.Airdrops.Count == 0)
                    {
                        logger.LogWarning("There isn't any custom airdrop, airdrop canceled.");
                        return;
                    }
                    airdropId = ConfigurationInstance.Airdrops[random.Next(ConfigurationInstance.Airdrops.Count)].AirdropId;                    
                }
                LevelManager.airdrop(list[num].point, airdropId, 100);
            }
            else
            {
                if (ConfigurationInstance.AirdropSpawns.Count == 0)
                {
                    logger.LogWarning("There isn't any airdrop spawn, airdrop canceled.");
                    return;
                }
                    
                int num = random.Next(ConfigurationInstance.AirdropSpawns.Count);

                LevelManager.airdrop(ConfigurationInstance.AirdropSpawns[num].Position.ToUnityVector(),
                    ConfigurationInstance.AirdropSpawns[num].AirdropId, 100);
            }
            ChatManager.serverSendMessage(ConfigurationInstance.AirdropMessage, Color.white, null, null, EChatMode.SAY, ConfigurationInstance.AirdropMessageIcon, true);
        }

        public void OnLevelLoaded(int level)
        {
            logger.LogInformation($"Creating your {ConfigurationInstance.AirdropSpawns.Count} airdrop spawns...");

            foreach (AirdropSpawn spawn in ConfigurationInstance.AirdropSpawns)
            {
                AirdropNode airdropNode;
                if (spawn.AirdropId == 0)
                {
                    airdropNode = new AirdropNode(spawn.Position.ToUnityVector());
                }
                else
                {
                    airdropNode = new AirdropNode(spawn.Position.ToUnityVector(), spawn.AirdropId);
                }
                LevelNodes.nodes.Add(airdropNode);
            }               
        }
    }
}
