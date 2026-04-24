using RestoreMonarchy.AirdropManager3.Models;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace RestoreMonarchy.AirdropManager3.Configurations
{
    public class AirdropsXmlConfiguration
    {
        private AirdropManager3Plugin pluginInstance => AirdropManager3Plugin.Instance;

        public AirdropsConfiguration Instance { get; private set; }
        private string fileName => $"Airdrops.{Provider.map}.xml";
        private string filePath => $"{pluginInstance.Directory}/{fileName}";

        private XmlSerializer xmlSerializer = new(typeof(AirdropsConfiguration), new XmlRootAttribute(nameof(AirdropsConfiguration)));

        public void Load()
        {
            if (File.Exists(filePath))
            {
                using (StreamReader reader = new(filePath))
                {
                    Instance = (AirdropsConfiguration)xmlSerializer.Deserialize(reader);
                }

                pluginInstance.LogInfo($"Loaded {Instance.Airdrops.Count} airdrops from {fileName}.");
            } else
            {
                Instance = Create();
                Save();
                pluginInstance.LogInfo($"Generated {fileName} with {Instance.Airdrops.Count} airdrops.");
            }

            Validate();
        }

        private void Validate()
        {
            if (Instance?.Airdrops == null)
            {
                return;
            }

            foreach (Airdrop airdrop in Instance.Airdrops)
            {
                if (airdrop.Items != null && airdrop.Items.Count > 0)
                {
                    List<ushort> invalidItemIds = new();
                    airdrop.Items.RemoveAll(item =>
                    {
                        if (Assets.find(EAssetType.ITEM, item.Id) is not ItemAsset)
                        {
                            invalidItemIds.Add(item.Id);
                            return true;
                        }
                        return false;
                    });

                    if (invalidItemIds.Count > 0)
                    {
                        pluginInstance.LogWarning($"Airdrop '{airdrop.DisplayName()}': removed {invalidItemIds.Count} item(s) with unresolved IDs: {string.Join(", ", invalidItemIds)}. Check that the workshop mods providing these items are loaded correctly.");
                    }
                }

                if (airdrop.Grenade != null && airdrop.Grenade.Id != 0
                    && Assets.find(EAssetType.ITEM, airdrop.Grenade.Id) is not ItemAsset)
                {
                    pluginInstance.LogWarning($"Airdrop '{airdrop.DisplayName()}': grenade ID {airdrop.Grenade.Id} doesn't resolve to a valid item. Grenade binding disabled for this airdrop.");
                    airdrop.Grenade = null;
                }

                if (airdrop.Storage != null && airdrop.Storage.BarricadeId != 0
                    && Assets.find(EAssetType.ITEM, airdrop.Storage.BarricadeId) is not ItemBarricadeAsset)
                {
                    pluginInstance.LogWarning($"Airdrop '{airdrop.DisplayName()}': storage barricade ID {airdrop.Storage.BarricadeId} doesn't resolve to a valid barricade. Falling back to default storage.");
                    airdrop.Storage.BarricadeId = 0;
                }

                if (airdrop.Items == null || airdrop.Items.Count == 0)
                {
                    pluginInstance.LogError($"Airdrop '{airdrop.DisplayName()}' has no valid items. Airdrops of this type will spawn empty.");
                }
            }
        }

        public void Save()
        {
            if (File.Exists(filePath))
            {
                return;
            }

            using (StreamWriter writer = new(filePath))
            {
                xmlSerializer.Serialize(writer, Instance);
            }

            pluginInstance.LogDebug($"Saved {Instance.Airdrops.Count} airdrops to {fileName} file.");
        }

        public List<AirdropItem> GetAirdropItems(SpawnAsset spawnAsset, int num = 0)
        {
            List<AirdropItem> airdropItems = new();

            if (num++ > 32)
            {
                return airdropItems;
            }

            foreach (SpawnTable spawnTable in spawnAsset.tables)
            {
                Asset asset = spawnTable.FindAsset(EAssetType.ITEM);

                if (asset == null)
                {
                    return [];
                }

                if (asset is SpawnAsset spawnAsset2)
                {
                    airdropItems.AddRange(GetAirdropItems(spawnAsset2, num));
                }
                else if (asset is ItemAsset itemAsset)
                {
                    airdropItems.Add(new AirdropItem()
                    {
                        Id = itemAsset.id,
                        Name = itemAsset.itemName,
                        Weight = spawnTable.weight
                    });
                }
                else
                {
                    pluginInstance.LogDebug($"Unknown asset type: {asset.GetType().Name} - {asset.id} - {asset.name}");
                }
            }

            return airdropItems;
        }

        private AirdropsConfiguration Create()
        {
            IEnumerable<AirdropDevkitNode> airdropNodes = pluginInstance.OriginalAirdropNodes;

            if (airdropNodes == null)
            {
                throw new ArgumentNullException(nameof(airdropNodes));
            }

            List<ushort> spawnIds = new();
            foreach (AirdropDevkitNode airdropNode in airdropNodes)
            {
                if (!spawnIds.Contains(airdropNode.CargoSpawnTableRef.LegacyId))
                {
                    spawnIds.Add(airdropNode.CargoSpawnTableRef.LegacyId);
                }
            }

            LevelAsset levelAsset = Level.getAsset();
            AssetReference<AirdropAsset> assetReference = levelAsset != null ? levelAsset.airdropRef : AssetReference<AirdropAsset>.invalid;
            AirdropAsset airdropAsset = null;
            if (!assetReference.isNull)
            {
                airdropAsset = assetReference.Find();
            }

            List<Airdrop> airdrops = new();
            foreach (ushort spawnId in spawnIds)
            {
                SpawnAsset spawnAsset = Assets.find(EAssetType.SPAWN, spawnId) as SpawnAsset;
                if (spawnAsset == null)
                {
                    pluginInstance.LogDebug($"SpawnAsset not found: {spawnId}");
                    continue;
                }

                List<AirdropItem> items = GetAirdropItems(spawnAsset);

                Airdrop airdrop = new()
                {
                    Id = spawnAsset.id,
                    Name = spawnAsset.name,
                    Items = items
                };

                if (airdropAsset != null)
                {
                    ItemStorageAsset barricade = airdropAsset.barricadeRef.Find() as ItemStorageAsset;
                    if (barricade != null)
                    {
                        airdrop.Storage = new()
                        {
                            BarricadeId = barricade.id,
                            Name = barricade.FriendlyName,
                            Width = barricade.storage_x,
                            Height = barricade.storage_y
                        };
                    }
                }

                airdrops.Add(airdrop);
            }

            return new AirdropsConfiguration()
            {
                Airdrops = airdrops
            };
        }
    }

}
