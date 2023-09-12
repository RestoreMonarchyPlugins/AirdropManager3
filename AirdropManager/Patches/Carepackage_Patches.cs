using HarmonyLib;
using RestoreMonarchy.AirdropManager.Models;
using Rocket.Core.Logging;
using SDG.Unturned;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace RestoreMonarchy.AirdropManager.Patches
{
    [HarmonyPatch(typeof(Carepackage))]
    class Carepackage_Patches
    {
        [HarmonyPrefix]
        [HarmonyPatch("OnCollisionEnter")]
        static bool OnCollisionEnter_Prefix(Carepackage __instance, Collision collision)
        {
            ushort barricadeId = AirdropManagerPlugin.Instance.Configuration.Instance.AirdropBarricadeId;
            if (barricadeId != 0)
            {
                ItemBarricadeAsset barricadeAsset = Assets.find(EAssetType.ITEM, barricadeId) as ItemBarricadeAsset;
                if (barricadeAsset != null)
                {
                    __instance.barricadeAsset = barricadeAsset;
                }
            }            

            ushort airdropId = __instance.id;

            Airdrop airdrop = AirdropManagerPlugin.Instance.Configuration.Instance.Airdrops.FirstOrDefault(x => x.AirdropId == airdropId);

            if (airdrop == null || airdrop.Items2 == null || airdrop.Items2.Count == 0)
            {
                return true;
            }

            // Override original code

            FieldInfo isExplodedField = AccessTools.Field(typeof(Carepackage), "isExploded");

            bool isExploded = (bool)isExplodedField.GetValue(__instance);

            if (isExploded)
            {
                return false;
            }
            if (collision.collider.isTrigger)
            {
                return false;
            }
            isExplodedField.SetValue(__instance, true);

            if (!Provider.isServer)
            {
                UnityEngine.Object.Destroy(__instance.gameObject);
                return false;
            }

            MethodInfo squishPlayersUnderBoxMethod = AccessTools.Method(typeof(Carepackage), "squishPlayersUnderBox");

            Vector3 position = __instance.transform.position;
            ItemBarricadeAsset itemBarricadeAsset = __instance.barricadeAsset;

            Transform transform = BarricadeManager.dropBarricade(new Barricade(itemBarricadeAsset), null, __instance.transform.position, 0f, 0f, 0f, 0UL, 0UL);
            if (transform != null)
            {
                squishPlayersUnderBoxMethod.Invoke(__instance, new object[] { transform });
                InteractableStorage component = transform.GetComponent<InteractableStorage>();
                component.despawnWhenDestroyed = true;
                component.items.resize(airdrop.StorageSizeX, airdrop.StorageSizeY);
                if (component != null && component.items != null)
                {
                    foreach (AirdropItem2 item in airdrop.Items2)
                    {
                        for (int i = 0; i < item.Quantity; i++)
                        {
                            if (!component.items.tryAddItem(new Item(item.ItemId, EItemOrigin.ADMIN), false))
                            {
                                Logger.Log($"{item.ItemId} could not be added to airdrop {airdropId} storage!", ConsoleColor.Yellow);
                                break;
                            }
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

            EffectAsset effectAsset = Assets.find(new AssetReference<EffectAsset>(__instance.landedEffectGuid));
            if (effectAsset != null)
            {
                EffectManager.triggerEffect(new TriggerEffectParameters(effectAsset)
                {
                    position = position,
                    reliable = true,
                    relevantDistance = EffectManager.INSANE
                });
            }

            UnityEngine.Object.Destroy(__instance.gameObject);
            return false;
        }
    }
}
