using HarmonyLib;
using SDG.Unturned;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace RestoreMonarchy.AirdropManager.Patches
{
    [HarmonyPatch(typeof(LevelManager))]
    class LevelManagerPatches
    {
        [HarmonyPrefix]
        [HarmonyPatch("airdrop")]
        static bool airdropPrefix(Vector3 point, ushort id, float speed)
        {
            AirdropManagerConfiguration configuration = AirdropManagerPlugin.Instance.Configuration.Instance;
            if (configuration.BlacklistedAirdrops.Contains(id))
            {
                Logger.Log($"Airdrop {id} is blacklisted, skipping...");
                return false;
            }

            return true;
        }
    }
}
