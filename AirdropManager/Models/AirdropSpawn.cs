using UnityEngine;

namespace RestoreMonarchy.AirdropManager.Models
{
    public class AirdropSpawn
    {
        public ushort AirdropId { get; set; }
        public string Name { get; set; }
        public Vector3 Position { get; set; }
        public bool IsDefault { get; set; }
    }
}
