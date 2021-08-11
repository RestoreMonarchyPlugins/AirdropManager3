using HarmonyLib;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestoreMonarchy.AirdropManager.Patches
{
    [HarmonyPatch(typeof(Carepackage))]
    class Carepackage_Patches
    {
        [HarmonyPrefix]
        [HarmonyPatch("OnCollisionEnter")]
        static void OnCollisionEnter_Prefix(Carepackage __instance)
        {
            __instance.barricadeID = AirdropManagerPlugin.Instance.Configuration.Instance.AirdropBarricadeId;
        }
    }
}
