using RestoreMonarchy.AirdropManager.Models;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace RestoreMonarchy.AirdropManager.Utilities
{
    public static class AirdropManagerUtility
    {
        public static string ToPrettyFormat(this TimeSpan span)
        {
            if (span <= TimeSpan.Zero) return string.Empty;

            var sb = new StringBuilder();
            if (span.Hours > 0)
                sb.AppendFormat("{0} hour{1} ", span.Hours, span.Hours > 1 ? "s" : String.Empty);
            if (span.Minutes > 0)
                sb.AppendFormat("{0} minute{1} ", span.Minutes, span.Minutes > 1 ? "s" : String.Empty);
            if (span.Seconds > 0)
                sb.AppendFormat("{0} second{1} ", span.Seconds, span.Seconds > 1 ? "s" : String.Empty);

            return sb.ToString().TrimEnd(' ');
        }

        public static string ToRich(this string value)
        {
            return value.Replace('{', '<').Replace('}', '>');
        }

        public static void AddAirdropSpawn(AirdropSpawn spawn)
        {
            var field = typeof(LevelManager).GetField("airdropNodes", BindingFlags.Static | BindingFlags.NonPublic);
            List<AirdropNode> nodes = field.GetValue(null) as List<AirdropNode>;

            AddAirdropToNodes(nodes, spawn);
            field.SetValue(null, nodes);
        }

        public static void AddAirdropToNodes(List<AirdropNode> nodes, AirdropSpawn spawn)
        {
            if (spawn.AirdropId == 0)
                nodes.Add(new AirdropNode(spawn.Position.ToVector()));
            else
                nodes.Add(new AirdropNode(spawn.Position.ToVector(), spawn.AirdropId));
        }
    }
}
