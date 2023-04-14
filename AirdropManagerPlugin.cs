using HarmonyLib;
using RestoreMonarchy.AirdropManager.Models;
using RestoreMonarchy.AirdropManager.Utilities;
using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Rocket.Core.Utils;
using Rocket.Unturned.Chat;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Timers;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace RestoreMonarchy.AirdropManager
{
    public class AirdropManagerPlugin : RocketPlugin<AirdropManagerConfiguration>
    {
        public static AirdropManagerPlugin Instance { get; set; }
        public Timer AirdropTimer { get; set; }
        public DateTime AirdropTimerNext { get; set; }
        public Color MessageColor { get; set; }
                
        public FieldInfo FieldAirdropNodes { get; set; }
        public FieldInfo FieldHasAirdrop { get; set; }
        public FieldInfo SpawnAssetTables { get; set; }
        public PropertyInfo SpawnAssetInsertRoots { get; set; }

        public override TranslationList DefaultTranslations =>  new TranslationList()
        {
            { "NextAirdrop", "Next airdrop will be in {0}" },
            { "SuccessAirdrop", "Successfully called in airdrop!" },
            { "SuccessMassAirdrop", "Successfully called in mass airdrop!" },            
            { "Airdrop", "<size=17>Airdrop is coming!</size>" },
            { "MassAirdrop", "<size=20>Mass Airdrop is coming!</size>" },
            { "SetAirdropFormat", "Format: /setairdrop <AirdropID>" },
            { "SetAirdropSuccess", "Successfully set an airdrop spawn at your position!" },
            { "SetAirdropInvalid", "You must specify AirdropID and optionally spawn name" },
            { "AirdropWithName", "<size=17>Airdrop will be dropped at {0}!</size>" }
        };

        private const string HarmonyId = "com.restoremonarchy.airdropmanager";

        private Harmony _harmony;
        protected override void Load()
        {
            Instance = this;
            MessageColor = UnturnedChat.GetColorFromName(Configuration.Instance.MessageColor, Color.green);

            _harmony = new Harmony(HarmonyId);
            _harmony.PatchAll();

            FieldAirdropNodes = typeof(LevelManager).GetField("airdropNodes", BindingFlags.Static | BindingFlags.NonPublic);
            FieldHasAirdrop = typeof(LevelManager).GetField("_hasAirdrop", BindingFlags.Static | BindingFlags.NonPublic);
            SpawnAssetTables = typeof(SpawnAsset).GetField("_tables", BindingFlags.Instance | BindingFlags.NonPublic);
            SpawnAssetInsertRoots =
                typeof(SpawnAsset).GetProperty("insertRoots", BindingFlags.Instance | BindingFlags.Public);
            
            LoadAirdropAssets();
            
            if (Level.isLoaded)
            {
                LoadAirdropSpawns(0);
                InitializeTimer(0);
            } else
            {
                Level.onLevelLoaded += LoadAirdropSpawns;
                Level.onLevelLoaded += InitializeTimer;
            }

            if (Provider.modeConfigData.Events.Use_Airdrops)
            {
                Provider.modeConfigData.Events.Use_Airdrops = false;
                Logger.Log("Automatically disabled Use_Airdrops in the Config.json", ConsoleColor.Yellow);
            }

            Logger.Log($"{Name} {Assembly.GetName().Version} has been loaded!", ConsoleColor.Yellow);
            Logger.Log($"Brought to You by RestoreMonarchy.com", ConsoleColor.Yellow);
        }

        protected override void Unload()
        {
            _harmony.UnpatchAll();
            Level.onLevelLoaded -= LoadAirdropSpawns;
            Level.onLevelLoaded -= InitializeTimer;
            AirdropTimer.Elapsed -= AirdropTimer_Elapsed;
            Logger.Log($"{Name} has been unloaded!", ConsoleColor.Yellow);
        }

        public List<AirdropSpawn> AirdropSpawns { get; set; }

        private void InitializeTimer(int level)
        {
            AirdropTimer = new Timer(Configuration.Instance.AirdropInterval * 1000);
            AirdropTimer.Elapsed += AirdropTimer_Elapsed;
            AirdropTimer.AutoReset = true;
            AirdropTimer.Start();
            AirdropTimerNext = DateTime.Now.AddSeconds(Configuration.Instance.AirdropInterval);

            Logger.Log("Airdrop timer has been started!", ConsoleColor.Yellow);
        }

        private ushort GetRandomCustomAirdropId(ushort defaultValue)
        {
            int airdropsCount = Configuration.Instance.Airdrops.Count;

            if (airdropsCount == 0)
            {
                return defaultValue;
            }

            int randomIndex = UnityEngine.Random.Range(0, airdropsCount);
            Airdrop airdrop = Configuration.Instance.Airdrops[randomIndex];

            return airdrop.AirdropId;
        }

        private float GetAirdropSpeed()
        {
            return Configuration.Instance.AirdropSpeed ?? Provider.modeConfigData.Events.Airdrop_Speed;
        }

        private void LoadAirdropAssets()
        {
            foreach (Airdrop airdrop in Configuration.Instance.Airdrops)
            {
                if (Configuration.Instance.BlacklistedAirdrops.Contains(airdrop.AirdropId))
                    continue;

                var asset = new SpawnAsset()
                {
                    id = airdrop.AirdropId,
                    GUID = Guid.NewGuid(),
                };
                SpawnAssetInsertRoots.SetValue(asset, new List<SpawnTable>());
                SpawnAssetTables.SetValue(asset, airdrop.Items.Select(item => new SpawnTable()
                {
                    assetID = item.ItemId,
                    weight = item.Chance,
                    spawnID = 0,
                    chance = 0
                }).ToList());

                asset.markTablesDirty();
                Assets.add(asset, true); // obsolete but its the only way to access the internal version. You can access it with reflection but this also works :>
            }
        }

        private void LoadAirdropSpawns(int level)
        {
            AirdropSpawns = new List<AirdropSpawn>();

            int defaultAirdropSpawnsCount = 0;
            int customAirdropSpawnsCount = 0;

            foreach (CustomAirdropSpawn customAirdropSpawn in Configuration.Instance.AirdropSpawns)
            {
                AirdropSpawn airdropSpawn = customAirdropSpawn.ToAirdropSpawn();

                AirdropSpawns.Add(airdropSpawn);
                customAirdropSpawnsCount++;
            }

            Logger.Log($"{customAirdropSpawnsCount} custom airdrop spawns have been loaded!");

            if (Configuration.Instance.UseDefaultSpawns)
            {
                List<AirdropDevkitNode> defaultAirdrops = FieldAirdropNodes.GetValue(null) as List<AirdropDevkitNode>;
                if (defaultAirdrops == null || defaultAirdrops.Count == 0)
                {
                    Logger.LogWarning("There isn't any default airdrop spawns on this server. You should disable UseDefaultSpawns in the config");
                } else
                {
                    foreach (AirdropDevkitNode defaultAirdrop in defaultAirdrops)
                    {
                        AirdropSpawn airdropSpawn = new AirdropSpawn()
                        {
                            AirdropId = defaultAirdrop.id,
                            Name = null,
                            Position = defaultAirdrop.transform.position,
                            IsDefault = true
                        };

                        if (!Configuration.Instance.UseDefaultAirdrops)
                        {
                            airdropSpawn.AirdropId = GetRandomCustomAirdropId(defaultAirdrop.id);
                        }

                        AirdropSpawns.Add(airdropSpawn);
                        defaultAirdropSpawnsCount++;
                    }
                    Logger.Log($"{defaultAirdropSpawnsCount} default airdrop spawns have been loaded!");
                }
            }
        }

        private void AirdropTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // Make sure it's executed on the main thread
            TaskDispatcher.QueueOnMainThread(() => 
            {
                CallAirdrop(false, false);
                AirdropTimerNext = DateTime.Now.AddSeconds(Configuration.Instance.AirdropInterval);
                Logger.Log("Airdrop has been called by a timer!", ConsoleColor.Yellow);
            });
        }

        public void CallMassAirdrop(bool shouldLog = true)
        {
            float airdropSpeed = GetAirdropSpeed();

            foreach (AirdropSpawn airdropSpawn in AirdropSpawns)
            {
                LevelManager.airdrop(airdropSpawn.Position, airdropSpawn.AirdropId, airdropSpeed);
            }

            ChatManager.serverSendMessage(Translate("MassAirdrop").ToRich(), MessageColor, null, null, EChatMode.SAY, Configuration.Instance.AirdropMessageIcon, true);
            
            if (shouldLog)
            {
                Logger.Log("Mass airdrop has been called!", ConsoleColor.Yellow);
            }            
        }

        public void CallAirdrop(bool isMass = false, bool shouldLog = true)
        {
            if (isMass)
            {
                CallMassAirdrop();
                return;
            }

            AirdropSpawn airdropSpawn = AirdropSpawns[UnityEngine.Random.Range(0, AirdropSpawns.Count)];
            float airdropSpeed = GetAirdropSpeed();

            LevelManager.airdrop(airdropSpawn.Position, airdropSpawn.AirdropId, airdropSpeed);

            if (string.IsNullOrEmpty(airdropSpawn.Name)) 
            {
                ChatManager.serverSendMessage(Translate("Airdrop").ToRich(), MessageColor, null, null, EChatMode.SAY, Configuration.Instance.AirdropMessageIcon, true);
            } else
            {
                ChatManager.serverSendMessage(Translate("AirdropWithName", airdropSpawn.Name), MessageColor, null, null, EChatMode.SAY, Configuration.Instance.AirdropMessageIcon, true);
            }

            if (shouldLog)
            {
                Logger.Log("Airdrop has been called!", ConsoleColor.Yellow);
            }            
        }
    }
}