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
            return value.Replace("{", "<").Replace("}", ">");
        }
    }
}
