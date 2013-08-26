using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace OpenGrooves.Core.Extensions
{
    public static class Extensions
    {
        #region String/Value Helpers
        public static string PrettyDate(this DateTime dt)
        {
            return dt.ToString("MMM d, yyyy");
        }

        public static string PrettyTime(this DateTime dt)
        {
            return dt.ToString("h:mm tt");
        }

        public static string PrettyDateTime(this DateTime dt)
        {
            return String.Format("{0:MMM d, yyyy} at {0:h:mm tt}", dt);
        }

        public static bool IsToday(this DateTime date)
        {
            return date.Date == DateTime.Now.Date;
        }

        public static string NL2BR(this string s)
        {
            if (s != null)
            {
                return s.Replace("\n", "<br />");
            }

            return String.Empty;
        }

        public static string DetectLinks(this string s)
        {
            return Regex.Replace(s, @"(?i)\b((?:[a-z][\w-]+:(?:/{1,3}|[a-z0-9%])|www\d{0,3}[.]|[a-z0-9.\-]+[.][a-z]{2,4}/)(?:[^\s()<>]+|\(([^\s()<>]+|(\([^\s()<>]+\)))*\))+(?:\(([^\s()<>]+|(\([^\s()<>]+\)))*\)|[^\s`!()\[\]{};:'"".,<>?«»“”‘’]))", "<a href=\"$1\" target=\"_blank\">$1</a>");
        }

        public static String Truncate(this string strText, int iLimit)
        {
            if (strText != null)
            {
                string strOutput = strText;

                if (strOutput.Length > iLimit && iLimit > 0)
                {
                    strOutput = strOutput.Substring(0, iLimit);

                    if (strText.Substring(strOutput.Length, 1) != " ")
                    {
                        int LastSpace = strOutput.LastIndexOf(" ");

                        if (LastSpace != -1)
                            strOutput = strOutput.Substring(0, LastSpace);
                    }

                    strOutput += "...";
                }
                return strOutput;
            }

            return String.Empty;

        }

        public static string DefaultIfEmpty(this string s, string defaultValue)
        {
            if (String.IsNullOrWhiteSpace(s))
            {
                return defaultValue;
            }

            return s;
        }

        public static bool IsNullOrWhiteSpace(this string str)
        {
            return String.IsNullOrWhiteSpace(str);
        }

        public static string AddOrdinal(this int num)
        {
            switch (num % 100)
            {
                case 11:
                case 12:
                case 13:
                    return num.ToString() + "th";
            }

            switch (num % 10)
            {
                case 1:
                    return num.ToString() + "st";
                case 2:
                    return num.ToString() + "nd";
                case 3:
                    return num.ToString() + "rd";
                default:
                    return num.ToString() + "th";
            }

        }
        #endregion

        #region Linq Helpers
        public static MvcHtmlString ToHtmlList(this IEnumerable<string> list)
        {
            var sb = new StringBuilder("<ul>");

            list.ToList().ForEach(i => sb.Append(String.Format("<li>{0}</li>", i)));

            return MvcHtmlString.Create(sb.Append("</ul>").ToString());
        }

        public static string ToString(this IEnumerable<string> list, string separator)
        {
            return String.Join(separator, list.ToArray());
        }

        public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            foreach (T item in list)
            {
                action(item);
            }
        }

        public static string Join(this IEnumerable<object> list, string separator)
        {
            return String.Join(separator, list.Select(i => i.ToString()).ToArray());
        }
        #endregion

        #region Data Reader Helpers
        public static T GetValue<T>(this IDataReader reader, string col)
        {
            return reader[col] == DBNull.Value ? default(T) : (T)reader[col];
        }
        #endregion
    }
}
