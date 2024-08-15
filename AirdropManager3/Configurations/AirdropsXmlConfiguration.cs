using RestoreMonarchy.AirdropManager3.Helpers;
using RestoreMonarchy.AirdropManager3.Models;
using Rocket.Core.Logging;
using SDG.Unturned;
using System;
using System.Collections;
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
                if (!spawnIds.Contains(airdropNode.id))
                {
                    spawnIds.Add(airdropNode.id);
                }
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
                airdrops.Add(airdrop);
            }

            return new AirdropsConfiguration()
            {
                Airdrops = airdrops
            };
        }
    }

}
