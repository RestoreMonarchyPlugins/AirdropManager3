using RestoreMonarchy.AirdropManager.Models;
using Rocket.Core.Logging;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace RestoreMonarchy.AirdropManager
{
    public partial class AirdropManagerPlugin
    {
        private FieldInfo LevelManagerAirdropNodesField { get; set; }
        private FieldInfo LevelManagerHasAirdropField { get; set; }

        private FieldInfo SpawnAssetRootsField { get; set; }
        private FieldInfo SpawnAssetTablesField { get; set; }
        private PropertyInfo SpawnAssetAreTablesDirtyProperty { get; set; }
        private PropertyInfo SpawnsAssetInsertRootsProperty { get; set; }
        private MethodInfo AddToMappingMethod { get; set; }
        private FieldInfo CurrentAssetMappingField { get; set; }
        private FieldInfo SpawnTableLegacyAssetIdField { get; set; }

        private void InitializeReflection()
        {
            LevelManagerAirdropNodesField = typeof(LevelManager).GetField("airdropNodes", BindingFlags.Static | BindingFlags.NonPublic);
            LevelManagerHasAirdropField = typeof(LevelManager).GetField("_hasAirdrop", BindingFlags.Static | BindingFlags.NonPublic);

            SpawnAssetRootsField = typeof(SpawnAsset).GetField("_roots", BindingFlags.NonPublic | BindingFlags.Instance);
            SpawnAssetTablesField = typeof(SpawnAsset).GetField("_tables", BindingFlags.NonPublic | BindingFlags.Instance);
            SpawnAssetAreTablesDirtyProperty = typeof(SpawnAsset).GetProperty("areTablesDirty", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            SpawnsAssetInsertRootsProperty = typeof(SpawnAsset).GetProperty("insertRoots", BindingFlags.Instance | BindingFlags.Public);
            AddToMappingMethod = typeof(Assets).GetMethod("AddToMapping", BindingFlags.NonPublic | BindingFlags.Static);
            CurrentAssetMappingField = typeof(Assets).GetField("currentAssetMapping", BindingFlags.NonPublic | BindingFlags.Static);
            SpawnTableLegacyAssetIdField = typeof(SpawnTable).GetField("legacyAssetId", BindingFlags.NonPublic | BindingFlags.Instance);
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
                List<AirdropDevkitNode> defaultAirdrops = LevelManagerAirdropNodesField.GetValue(null) as List<AirdropDevkitNode>;
                if (defaultAirdrops == null || defaultAirdrops.Count == 0)
                {
                    Logger.LogWarning("There isn't any default airdrop spawns on this server. You should disable UseDefaultSpawns in the config");
                }
                else
                {
                    foreach (AirdropDevkitNode defaultAirdrop in defaultAirdrops)
                    {
                        AirdropSpawn airdropSpawn = new()
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

        private void LoadAirdropAssets(int level)
        {
            Logger.Log("Loading airdrop assets...", ConsoleColor.Yellow);
            foreach (CustomAirdrop airdrop in Configuration.Instance.Airdrops)
            {
                if (Configuration.Instance.BlacklistedAirdrops.Contains(airdrop.AirdropId))
                {
                    continue;
                }

                SpawnAsset asset = new()
                {
                    id = airdrop.AirdropId
                };

                SpawnsAssetInsertRootsProperty.SetValue(asset, new List<SpawnTable>());
                SpawnAssetRootsField.SetValue(asset, new List<SpawnTable>());
                SpawnAssetTablesField.SetValue(asset, new List<SpawnTable>());

                foreach (CustomAirdropItem item in airdrop.Items)
                {
                    SpawnTable spawnTable = new()
                    {
                        weight = item.Chance
                    };
                    SpawnTableLegacyAssetIdField.SetValue(spawnTable, item.ItemId);

                    asset.tables.Add(spawnTable);
                }

                SpawnAssetAreTablesDirtyProperty.SetValue(asset, true);

                object assetMapping = CurrentAssetMappingField.GetValue(null);
                AddToMappingMethod.Invoke(null, [asset, true, assetMapping]);
            }
            Assets.linkSpawns();
            Logger.Log($"{Configuration.Instance.Airdrops.Count} airdrop assets have been loaded!", ConsoleColor.Yellow);
        }
    }
}
