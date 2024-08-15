using HarmonyLib;
using RestoreMonarchy.AirdropManager.Models;
using RestoreMonarchy.AirdropManager.Utilities;
using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Rocket.Unturned.Chat;
using SDG.Unturned;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace RestoreMonarchy.AirdropManager
{
    public partial class AirdropManagerPlugin : RocketPlugin<AirdropManagerConfiguration>
    {
        public static AirdropManagerPlugin Instance { get; set; }
        public DateTime AirdropTimerNext { get; set; }
        public Color MessageColor { get; set; }

        public override TranslationList DefaultTranslations =>  new TranslationList()
        {
            { "NextAirdrop", "Next airdrop will be in {0}" },        
            { "Airdrop", "<size=17>Airdrop is coming!</size>" },
            { "MassAirdrop", "<size=20>Mass Airdrop is coming!</size>" },
            { "SetAirdropFormat", "Format: /setairdrop <AirdropID>" },
            { "SetAirdropSuccess", "Successfully set an airdrop spawn at your position!" },
            { "SetAirdropInvalid", "You must specify AirdropID and optionally spawn name" },
            { "AirdropWithSpawnName", "<size=17>Airdrop will be dropped at {0}!</size>" }
        };

        public const string HarmonyId = "com.restoremonarchy.airdropmanager";

        private Harmony harmony;
        protected override void Load()
        {
            Instance = this;
            MessageColor = UnturnedChat.GetColorFromName(Configuration.Instance.MessageColor, Color.green);

            InitializeReflection();

            if (Level.isLoaded)
            {
                LoadAirdropSpawns(0);
                LoadAirdropAssets(0);
            } else
            {
                Level.onLevelLoaded += LoadAirdropSpawns;
                Level.onPreLevelLoaded += LoadAirdropAssets;
            }

            if (Provider.modeConfigData.Events.Use_Airdrops)
            {
                Provider.modeConfigData.Events.Use_Airdrops = false;
            }

            harmony = new Harmony(HarmonyId);
            harmony.PatchAll();

            Logger.Log($"{Name} {Assembly.GetName().Version} has been loaded!", ConsoleColor.Yellow);
            Logger.Log("Check out more Unturned plugins at restoremonarchy.com");
        }

        protected override void Unload()
        {
            Level.onPreLevelLoaded -= LoadAirdropAssets;
            Level.onLevelLoaded -= LoadAirdropSpawns;

            Logger.Log($"{Name} has been unloaded!", ConsoleColor.Yellow);
        }

        internal void LogDebug(string message)
        {
            if (Configuration.Instance.Debug)
            {
                Logger.Log($"Debug >> {message}", ConsoleColor.Gray);
            }
        }


        public List<CustomAirdrop> MyProperty { get; set; }
        public List<AirdropSpawn> AirdropSpawns { get; set; }

        private IEnumerator AirdropTimerCoroutine()
        {
            while (true)
            {
                float interval = UnityEngine.Random.Range(Configuration.Instance.AirdropIntervalMin, Configuration.Instance.AirdropIntervalMax);
                AirdropTimerNext = DateTime.Now.AddSeconds(Configuration.Instance.AirdropIntervalMin);
                yield return new WaitForSeconds(interval);
                
                CallAirdrop(null, false);                
                Logger.Log("Airdrop has been called by a timer!", ConsoleColor.Yellow);
            }
        }

        private ushort GetRandomCustomAirdropId(ushort defaultValue)
        {
            int airdropsCount = Configuration.Instance.Airdrops.Count;

            if (airdropsCount == 0)
            {
                return defaultValue;
            }

            int randomIndex = UnityEngine.Random.Range(0, airdropsCount);
            CustomAirdrop airdrop = Configuration.Instance.Airdrops[randomIndex];

            return airdrop.AirdropId;
        }

        private float GetAirdropSpeed(Airdrop airdrop)
        {
            if (airdrop.CustomAirdrop != null && airdrop.CustomAirdrop.Speed.HasValue)
            {
                return airdrop.CustomAirdrop.Speed.Value;
            }

            if (Configuration.Instance.AirdropSpeed.HasValue)
            {
                return Configuration.Instance.AirdropSpeed.Value;
            }

            return Configuration.Instance.AirdropSpeed ?? Provider.modeConfigData.Events.Airdrop_Speed;
        }   
        

        public void CallMassAirdrop(bool shouldLog = true)
        {
            

            foreach (AirdropSpawn airdropSpawn in AirdropSpawns)
            {
                float airdropSpeed = GetAirdropSpeed();
                LevelManager.airdrop(airdropSpawn.Position, airdropSpawn.AirdropId, airdropSpeed);
            }

            string message = Translate("MassAirdrop").ToRich();
            ChatManager.serverSendMessage(message, MessageColor, null, null, EChatMode.SAY, Configuration.Instance.AirdropMessageIcon, true);
            
            if (shouldLog)
            {
                Logger.Log("Mass airdrop has been called!", ConsoleColor.Yellow);
            }            
        }

        public void CallAirdrop(CustomAirdrop airdrop = null, bool shouldLog = true)
        {
            if (AirdropSpawns.Count == 0)
            {
                Logger.LogWarning("There isn't any airdrop spawns on this map or in the config. Use /setairdrop command to set custom spawns!");
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
                string message = Translate("AirdropWithSpawnName", airdropSpawn.Name).ToRich();
                ChatManager.serverSendMessage(message, MessageColor, null, null, EChatMode.SAY, Configuration.Instance.AirdropMessageIcon, true);
            }

            if (shouldLog)
            {
                Logger.Log("Airdrop has been called!", ConsoleColor.Yellow);
            }            
        }
    }
}