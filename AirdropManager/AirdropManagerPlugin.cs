using Harmony;
using Rocket.API.Collections;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using System;
using System.Reflection;

namespace RestoreMonarchy.AirdropManager
{
    public class AirdropManagerPlugin : RocketPlugin<AirdropManagerConfiguration>
    {
        public static AirdropManagerPlugin Instance { get; private set; }

        public const string HarmonyInstanceId = "com.restoremonarchy.airdropmanager";
        private HarmonyInstance HarmonyInstance;

        protected override void Load()
        {
            HarmonyInstance = HarmonyInstance.Create(HarmonyInstanceId);
            HarmonyInstance.PatchAll(Assembly);
            
            Instance = this;

            Logger.Log($"{Name} {Assembly.GetName().Version} has been loaded!", ConsoleColor.Yellow);
        }

        protected override void Unload()
        {
            HarmonyInstance?.UnpatchAll(HarmonyInstanceId);
            HarmonyInstance = null;
        }
    }
}
