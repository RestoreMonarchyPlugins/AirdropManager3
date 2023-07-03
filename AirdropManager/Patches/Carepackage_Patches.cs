using HarmonyLib;
using SDG.Unturned;

namespace RestoreMonarchy.AirdropManager.Patches
{
    [HarmonyPatch(typeof(Carepackage))]
    class Carepackage_Patches
    {
        [HarmonyPrefix]
        [HarmonyPatch("OnCollisionEnter")]
        static void OnCollisionEnter_Prefix(Carepackage __instance)
        {
            ushort barricadeId = AirdropManagerPlugin.Instance.Configuration.Instance.AirdropBarricadeId;
            if (barricadeId == 0)
            {
                return;
            }

            ItemBarricadeAsset barricadeAsset = Assets.find(EAssetType.ITEM, barricadeId) as ItemBarricadeAsset;
            if (barricadeAsset != null)
            {
                __instance.barricadeAsset = barricadeAsset;
            }            
        }
    }
}
