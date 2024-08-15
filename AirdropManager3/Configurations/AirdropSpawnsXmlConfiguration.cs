using RestoreMonarchy.AirdropManager3.Helpers;
using RestoreMonarchy.AirdropManager3.Models;
using Rocket.Core.Logging;
using SDG.Unturned;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

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
            IEnumerable<Node> nodes = ReflectionHelper.GetLevelNodesNodes();
            
            if (nodes == null)
            {
                throw new Exception("Nodes are null.");
            }

            IEnumerable<LocationNode> locationNodes = nodes.OfType<LocationNode>().ToArray();
            IEnumerable<AirdropNode> airdropNodes = nodes.OfType<AirdropNode>().ToArray();

            List<AirdropSpawn> airdropSpawns = new();
            foreach (AirdropNode airdropNode in airdropNodes)
            {
                Vector3 position = airdropNode.point;
                LocationNode nearestNode = locationNodes.OrderBy(n => Vector3.Distance(n.point, position)).FirstOrDefault();

                AirdropSpawn airdropSpawn = new()
                {
                    AirdropId = airdropNode.id,
                    Name = nearestNode?.name ?? null,
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
