using Rocket.Core.Commands;
using SDG.Unturned;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using static Rocket.Core.Commands.RocketCommandManager;

namespace RestoreMonarchy.AirdropManager3.Helpers
{
    internal static class ReflectionHelper
    {
        internal static FieldInfo LevelManagerAirdropNodesField { get; } = typeof(LevelManager).GetField("airdropNodes", BindingFlags.Static | BindingFlags.NonPublic);
        internal static List<AirdropDevkitNode> GetLevelManagerAirdropNodes() => LevelManagerAirdropNodesField.GetValue(null) as List<AirdropDevkitNode>;
        internal static void SetLevelManagerAirdropNodes(List<AirdropDevkitNode> value) => LevelManagerAirdropNodesField.SetValue(null, value);

        internal static FieldInfo LevelNodesNodes { get; } = typeof(LevelNodes).GetField("_nodes", BindingFlags.Static | BindingFlags.NonPublic);
        internal static List<Node> GetLevelNodesNodes() => LevelNodesNodes.GetValue(null) as List<Node>;

        internal static FieldInfo CarepackageIsExploded { get; } = typeof(Carepackage).GetField("isExploded", BindingFlags.Instance | BindingFlags.NonPublic);
        internal static bool GetCarepackageIsExploded(Carepackage carepackage) => (bool)CarepackageIsExploded.GetValue(carepackage);
        internal static void SetCarepackageIsExploded(Carepackage carepackage, bool value) => CarepackageIsExploded.SetValue(carepackage, value);

        internal static MethodInfo CarepackageSquishPlayersUnderBox { get; } = typeof(Carepackage).GetMethod("squishPlayersUnderBox", BindingFlags.Instance | BindingFlags.NonPublic);
        internal static void SquishPlayersUnderBox(Carepackage carepackage, Transform transform) => CarepackageSquishPlayersUnderBox.Invoke(carepackage, [transform]);

        internal static FieldInfo RocketCommandManagerCommands { get; } = typeof(RocketCommandManager).GetField("commands", BindingFlags.Instance | BindingFlags.NonPublic);
        internal static List<RegisteredRocketCommand> GetRocketCommandManagerCommands(RocketCommandManager rocketCommandManager) => RocketCommandManagerCommands.GetValue(rocketCommandManager) as List<RegisteredRocketCommand>;
    }
}
