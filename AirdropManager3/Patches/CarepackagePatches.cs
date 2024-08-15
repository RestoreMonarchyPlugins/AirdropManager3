using HarmonyLib;
using RestoreMonarchy.AirdropManager3.Helpers;
using RestoreMonarchy.AirdropManager3.Models;
using SDG.Unturned;
using UnityEngine;

namespace RestoreMonarchy.AirdropManager3.Patches
{
    [HarmonyPatch(typeof(Carepackage))]
    class CarepackagePatches
    {
        [HarmonyPrefix]
        [HarmonyPatch("OnCollisionEnter")]
        static bool OnCollisionEnterPrefix(Carepackage __instance, Collision collision)
        {
            AirdropManager3Plugin pluginInstance = AirdropManager3Plugin.Instance;
            if (pluginInstance == null)
            {
                return true;
            }

            bool isExploded = ReflectionHelper.GetCarepackageIsExploded(__instance);
            if (isExploded)
            {
                return false;
            }
            if (collision.collider.isTrigger)
            {
                return false;
            }

            isExploded = true;
            ReflectionHelper.SetCarepackageIsExploded(__instance, isExploded);

            Airdrop airdrop = pluginInstance.AirdropsConfiguration.Instance.GetAirdropById(__instance.id);

            Vector3 position = __instance.transform.position;

            ushort barricadeId = pluginInstance.Configuration.Instance.GetAirdropStorageBarricadeId(airdrop);
            ItemBarricadeAsset barricadeAsset = (Assets.find(EAssetType.ITEM, barricadeId) as ItemBarricadeAsset);

            if (barricadeAsset == null)
            {
                pluginInstance.LogError("Invalid barricade id: " + barricadeId);
                barricadeAsset = Assets.find(EAssetType.ITEM, 1374) as ItemBarricadeAsset;
            }

            Transform transform = BarricadeManager.dropBarricade(new(barricadeAsset), null, position, 0, 0, 0, 0, 0);
            if (transform != null)
            {
                ReflectionHelper.SquishPlayersUnderBox(__instance, transform);
                InteractableStorage component = transform.GetComponent<InteractableStorage>();

                byte width = pluginInstance.Configuration.Instance.GetAirdropStorageWidth(airdrop);
                byte height = pluginInstance.Configuration.Instance.GetAirdropStorageHeight(airdrop);
                component.items.resize(width, height);

                component.despawnWhenDestroyed = true;
                if (component != null && component.items != null)
                {
                    int i = 0;
                    while (i < 12)
                    {
                        AirdropItem airdropItem  = airdrop.RandomAirdropItem();
                        if (airdropItem == null)
                        {
                            break;
                        }

                        if (!component.items.tryAddItem(new Item(airdropItem.Id, EItemOrigin.ADMIN), false))
                        {
                            i++;
                        }
                    }
                    component.items.onStateUpdated();
                }
                transform.gameObject.AddComponent<CarepackageDestroy>();
                Transform transform2 = transform.Find("Flare");
                if (transform2 != null)
                {
                    position = transform2.position;
                }
            }

            string landedEffectGuid = pluginInstance.Configuration.Instance.GetLandedEffectGuid(airdrop);
            EffectAsset effectAsset = Assets.find(new AssetReference<EffectAsset>(landedEffectGuid));
            if (effectAsset != null)
            {
                EffectManager.triggerEffect(new TriggerEffectParameters(effectAsset)
                {
                    position = position,
                    reliable = true,
                    relevantDistance = EffectManager.INSANE
                });
            }

            Object.Destroy(__instance.gameObject);
            return false;
        }
    }
}
