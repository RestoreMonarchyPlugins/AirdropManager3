using HarmonyLib;
using RestoreMonarchy.AirdropManager3.Commands;
using RestoreMonarchy.AirdropManager3.Components;
using RestoreMonarchy.AirdropManager3.Configurations;
using RestoreMonarchy.AirdropManager3.Helpers;
using RestoreMonarchy.AirdropManager3.Models;
using Rocket.API;
using Rocket.API.Collections;
using Rocket.Core;
using Rocket.Core.Plugins;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using UnityEngine;
using static Rocket.Core.Commands.RocketCommandManager;
using Logger = Rocket.Core.Logging.Logger;

namespace RestoreMonarchy.AirdropManager3
{
    public partial class AirdropManager3Plugin : RocketPlugin<AirdropManager3Configuration>
    {
        public static AirdropManager3Plugin Instance { get; private set; }
        public Color MessageColor { get; set; }

        public AirdropsXmlConfiguration AirdropsConfiguration { get; private set; }
        public AirdropsSpawnsXmlConfiguration AirdropSpawnsConfiguration { get; private set; }

        private Coroutine airdropIntervalCoroutine;

        public const string HarmonyId = "com.restoremonarchy.airdropmanager3";
        private Harmony harmony;

        public List<AirdropDevkitNode> OriginalAirdropNodes { get; private set; }

        protected override void Load()
        {
            Instance = this;
            MessageColor = UnturnedChat.GetColorFromName(Configuration.Instance.MessageColor, Color.green);

            AirdropsConfiguration = new();
            AirdropSpawnsConfiguration = new();

            if (Level.isLoaded)
            {
                OnLevelLoaded(0);
            } else
            {
                Level.onLevelLoaded += OnLevelLoaded;
            }

            List<RegisteredRocketCommand> registeredRocketCommands = ReflectionHelper.GetRocketCommandManagerCommands(R.Commands);
            int count = registeredRocketCommands.RemoveAll(x => x.Name.Equals("airdrop", StringComparison.OrdinalIgnoreCase) && x.Command.GetType().Assembly != typeof(AirdropCommand).Assembly);
            if (count > 0)
            {
                LogInfo($"Deregistered {count} old airdrop command{(count > 1 ? "s" : "")}.");
            }

            UseableThrowable.onThrowableSpawned += OnThrowableSpawned;

            if (Configuration.Instance.EnableAirdropInterval)
            {
                airdropIntervalCoroutine = StartCoroutine(AirdropIntervalCoroutine());
            }

            if (harmony == null)
            {
                harmony = new(HarmonyId);
                harmony.PatchAll();
            }

            Logger.Log($"{Name} {Assembly.GetName().Version.ToString(3)} has been loaded!", ConsoleColor.Yellow);
            Logger.Log("Check out more Unturned plugins at restoremonarchy.com");
        }

        protected override void Unload()
        {
            Instance = null;
            if (airdropIntervalCoroutine != null)
            {
                StopCoroutine(airdropIntervalCoroutine);
            }
            NextAirdrop = null;

            Level.onLevelLoaded -= OnLevelLoaded;
            UseableThrowable.onThrowableSpawned -= OnThrowableSpawned;

            Logger.Log($"{Name} has been unloaded!", ConsoleColor.Yellow);
        }

        public override TranslationList DefaultTranslations => new()
        {
            { "AirdropNoPermission", "You don't have permission to specify airdrop spawn." },
            { "AirdropHereNoPermission", "You don't have permission to specify airdrop." },
            { "MassAirdropNoPermission", "You don't have permission to specify airdrop speed." },
            { "AirdropSpawnNotFound", "Airdrop spawn with name [[b]]{0}[[/b]] doesn't exist." },
            { "AirdropSpawnNone", "There isn't any airdrop spawns." },
            { "AirdropNone", "There isn't any airdrops." },
            { "AirdropSpawned", "Successfully called in [[b]]{0}[[/b]] airdrop to [[b]]{1}.[[/b]]" },
            { "AirdropNotFound", "Airdrop with name or ID [[b]]{0}[[/b]] doesn't exist." },
            { "AirdropHereSpawned", "Successfully called in [[b]]{0}[[/b]] to your position." },
            { "MassAirdropSpawned", "Successfully called in [[b]]mass airdrop[[/b]] to all [[b]]{0}[[/b]] spawns." },
            { "AirdropSpawnSet", "Successfully set airdrop spawn [[b]]{0}.[[/b]]" },
            { "AirdropSpawnSetWithAirdrop", "Successfully set airdrop spawn [[b]]{0}[[/b]] for [[b]]{1}[[/b]] airdrop." },
            { "WhenAirdropTimeLeft", "Next airdrop will be in [[b]]{0}.[[/b]]" },
            { "WhenAirdropNotPlanned", "There isn't any automatic airdrop scheduled." },
            { "Day", "1 day" },
            { "Days", "{0} days" },
            { "Hour", "1 hour" },
            { "Hours", "{0} hours" },
            { "Minute", "1 minute" },
            { "Minutes", "{0} minutes" },
            { "Second", "1 second" },
            { "Seconds", "{0} seconds" },
            { "Zero", "a moment" }
        };

        internal string FormatTimespan(TimeSpan span)
        {
            if (span <= TimeSpan.Zero) return Translate("Zero");

            List<string> items = new();
            if (span.Days > 0)
                items.Add(span.Days == 1 ? Translate("Day") : Translate("Days", span.Days));
            if (span.Hours > 0)
                items.Add(span.Hours == 1 ? Translate("Hour") : Translate("Hours", span.Hours));
            if (items.Count < 2 && span.Minutes > 0)
                items.Add(span.Minutes == 1 ? Translate("Minute") : Translate("Minutes", span.Minutes));
            if (items.Count < 2 && span.Seconds > 0)
                items.Add(span.Seconds == 1 ? Translate("Second") : Translate("Seconds", span.Seconds));

            return string.Join(" ", items);

        }

        internal void LogDebug(string message)
        {
            if (Configuration.Instance.Debug)
            {
                Logger.Log($"Debug >> {message}", ConsoleColor.Gray);
            }
        }

        internal void LogError(string message)
        {
            Logger.Log($"Error >> {message}", ConsoleColor.Red);
        }

        internal bool DisableInfoLog { get; set; }

        internal void LogInfo(string message)
        {
            if (DisableInfoLog)
            {
               return;
            }

            Logger.Log($"{message}");
        }

        private void OnLevelLoaded(int level)
        {
            if (OriginalAirdropNodes == null)
            {
                OriginalAirdropNodes = ReflectionHelper.GetLevelManagerAirdropNodes();
            }            

            AirdropsConfiguration.Load();
            AirdropSpawnsConfiguration.Load();

            // Disable default airdrops
            ReflectionHelper.SetLevelManagerAirdropNodes([]);
        }

        private void OnThrowableSpawned(UseableThrowable useable, GameObject throwable)
        {
            Airdrop airdrop = AirdropsConfiguration.Instance.GetAirdropByGrenadeId(useable.equippedThrowableAsset.id);
            if (airdrop == null)
            {
                return;
            }

            AirdropGrenadeComponent component = throwable.AddComponent<AirdropGrenadeComponent>();
            component.Airdrop = airdrop;
            component.Player = UnturnedPlayer.FromPlayer(useable.player);
        }

        public Airdrop Airdrop(AirdropSpawn airdropSpawn, float speed = 0)
        {
            if (airdropSpawn == null)
            {
                throw new ArgumentNullException(nameof(airdropSpawn));
            }

            if (AirdropsConfiguration.Instance.Airdrops.Count == 0)
            {
                LogError("There isn't any airdrops.");
            }

            Airdrop airdrop;
            if (airdropSpawn.AirdropId == 0)
            {
                airdrop = AirdropsConfiguration.Instance.GetRandomAirdrop();
            }
            else
            {
                airdrop = AirdropsConfiguration.Instance.GetAirdropById(airdropSpawn.AirdropId);

                if (airdrop == null)
                {
                    LogError($"Airdrop with id {airdropSpawn.AirdropId} doesn't exist.");
                    airdrop = AirdropsConfiguration.Instance.GetRandomAirdrop();
                }
            }

            Airdrop(airdrop, airdropSpawn.Vector3, airdropSpawn.DisplayName());

            return airdrop;
        }

        public void Airdrop(Airdrop airdrop, Vector3 position, string positionName = null, float speed = 0)
        {
            ushort airdropId = airdrop.Id;
            if (speed <= 0)
            {
                speed = Configuration.Instance.GetAirdropSpeed(airdrop);
            }            
            LevelManager.airdrop(position, airdropId, speed);

            positionName = positionName ?? position.ToString();
            LogInfo($"Airdrop {airdrop.DisplayName()} has been called to {positionName} with speed {speed}.");
        }
    }
}