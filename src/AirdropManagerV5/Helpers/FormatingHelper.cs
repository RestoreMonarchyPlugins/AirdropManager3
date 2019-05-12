using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestoreMonarchy.AirdropManager.Helpers
{
    public class FormatingHelper
    {
        public static string ToPrettyFormat(TimeSpan span)
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
    }
}
