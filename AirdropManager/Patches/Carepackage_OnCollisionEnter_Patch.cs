﻿using Harmony;
using RestoreMonarchy.AirdropManager.Models;
using Rocket.Unturned.Chat;
using SDG.Unturned;
using UnityEngine;

namespace RestoreMonarchy.AirdropManager.Patches
{
    [HarmonyPatch(typeof(Carepackage), "OnCollisionEnter")]
    class Carepackage_OnCollisionEnter_Patch
    {
        [HarmonyPrefix]
        public static void OnCollisionEnter(Carepackage __instance, Collision collision)
        {
            var isExploded = Traverse.Create(__instance).Field("isExploded");

            if (isExploded.GetValue<bool>() || collision.collider.isTrigger)
            {
                return;
            }

            isExploded.SetValue(true);
            if (Provider.isServer)
            {
                Transform transform = BarricadeManager.dropBarricade(new Barricade(1374), null, __instance.transform.position, 0f, 0f, 0f, 0UL, 0UL);
                if (transform != null)
                {
                    InteractableStorage component = transform.GetComponent<InteractableStorage>();
                    component.despawnWhenDestroyed = true;
                    if (component != null && component.items != null)
                    {
                        CustomAirdrop custom = new CustomAirdrop();

                        foreach (ushort itemId in custom.Airdrop.Items)
                        {
                            if (!component.items.tryAddItem(new Item(itemId, EItemOrigin.ADMIN), false))
                            {
                                ItemManager.dropItem(new Item(itemId, EItemOrigin.ADMIN), __instance.transform.position, true, true, true);
                            }
                        }
                    }                    
                }

                transform.gameObject.AddComponent<CarepackageDestroy>();
                EffectManager.sendEffectReliable(120, EffectManager.INSANE, __instance.transform.position);
                
            }

            Object.Destroy(__instance.gameObject);
            return;
        }
    }
}