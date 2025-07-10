using RestoreMonarchy.AirdropManager3.Helpers;
using RestoreMonarchy.AirdropManager3.Models;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;

namespace RestoreMonarchy.AirdropManager3.Configurations
{
    public class AirdropsSpawnsXmlConfiguration
    {
        private AirdropManager3Plugin pluginInstance => AirdropManager3Plugin.Instance;

        public AirdropSpawnsConfiguration Instance { get; private set; }
        private string fileName => $"AirdropSpawns.{Provider.map}.xml";
        private string filePath => $"{pluginInstance.Directory}/{fileName}";

        private XmlSerializer xmlSerializer = new(typeof(AirdropSpawnsConfiguration), new XmlRootAttribute(nameof(AirdropSpawnsConfiguration)));

        public void Load()
        {
            if (File.Exists(filePath))
            {
                using (StreamReader reader = new(filePath))
                {
                    Instance = (AirdropSpawnsConfiguration)xmlSerializer.Deserialize(reader);
                }

                pluginInstance.LogInfo($"Loaded {Instance.AirdropSpawns.Count} airdrop spawns from {fileName}.");
            } else
            {
                Instance = Create();
                Save();
                pluginInstance.LogInfo($"Generated {fileName} with {Instance.AirdropSpawns.Count} airdrop spawns.");
            }
        }

        public void Save()
        {
            using (StreamWriter writer = new(filePath))
            {
                xmlSerializer.Serialize(writer, Instance);
            }

            pluginInstance.LogDebug($"Saved {Instance.AirdropSpawns.Count} airdrop spawns to {fileName} file.");
        }

        private AirdropSpawnsConfiguration Create()
        {
            IReadOnlyList<LocationDevkitNode> locationNodes = LocationDevkitNodeSystem.Get().GetAllNodes();
            List<AirdropDevkitNode> airdropNodes = ReflectionHelper.GetLevelManagerAirdropNodes();

            if (locationNodes == null)
            {
                pluginInstance.LogDebug("No location nodes were found.");
                locationNodes = [];
            }

            if (airdropNodes == null)
            {
                throw new Exception("Airdrop nodes are null. Please check if the level is loaded and airdrop nodes are present.");
            }

            List<AirdropSpawn> airdropSpawns = new();
            foreach (AirdropDevkitNode airdropNode in airdropNodes)
            {
                Vector3 position = airdropNode.transform.position;
                LocationDevkitNode nearestNode = locationNodes.OrderBy(n => Vector3.Distance(n.transform.position, position)).FirstOrDefault();

                AirdropSpawn airdropSpawn = new()
                {
                    AirdropId = airdropNode.CargoSpawnTableRef.LegacyId,
                    Name = nearestNode?.locationName ?? null,
                    X = position.x,
                    Y = position.y,
                    Z = position.z
                };
                airdropSpawns.Add(airdropSpawn);
            }

            return new AirdropSpawnsConfiguration()
            {
                AirdropSpawns = airdropSpawns
            };
        }
    }

}
