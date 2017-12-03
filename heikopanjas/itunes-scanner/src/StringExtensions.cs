using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ultraschall_scanner
{
   public static class StringExtensions
   {
      public static List<int> AllIndexesOf(this string str, string value)
      {
         List<int> result = new List<int>();

         if (String.IsNullOrEmpty(value) == false)
         {
            int i = 0;
            while (i != -1)
            {
               i = str.IndexOf(value, i);
               if (i != -1)
               {
                  result.Add(i);
                  i += value.Length;
               }

            }
         }

         return result;
      }

      public static String TrimToChar(this string str, char delimiter)
      {
         String result = str;

         if (String.IsNullOrEmpty(str) == false)
         {
            int index = str.IndexOf('#');
            if (index != -1)
            {
               result = str.Substring(0, index);
            }
         }

         return result;
      }

      public static String FindPodcastId(this string str)
      {
         String id = null;

         if (String.IsNullOrEmpty(str) == false)
         {
            int offset = str.LastIndexOf("/id");
            if ((offset != -1) && (offset < str.Length))
            {
               try
               {
                  id = str.Substring(offset + "/id".Length);
                  Convert.ToUInt64(id, 10);
                  return id;
               }
               catch (Exception e)
               {
                  Console.WriteLine(e.Message);
                  Console.WriteLine(e.StackTrace);
                  id = null;
               }
            }
         }

         return id;
      }

      public static List<String> FindUrls(this string str)
      {
         List<String> urls = new List<String>();

         Regex regex = new Regex("(?:href|src)=[\"|']?(.*?)[\"|'|>]+", RegexOptions.Singleline | RegexOptions.CultureInvariant);
         if (regex.IsMatch(str) == true)
         {
            regex.Matches(str)?.ToList().ForEach(m =>
            {
               if (m.Groups[1].Value.StartsWith("http"))
               {
                  urls.Add(m.Groups[1].Value);
               }
            });
         }

         return urls;
      }

      public static List<String> FindPageUrls(this string str)
      {
         //List<int> indexes = content?.AllIndexesOf("page=");
         //return indexes?.Select(x => content.Substring(x + "page=".Length, 3).TrimToChar('#')).Distinct().ToArray();

         List<String> pages = new List<String>();

         if (String.IsNullOrEmpty(str) == false)
         {
            List<int> indexes = str?.AllIndexesOf("page=");
            pages = indexes.Select(x => str.Substring(x + "page=".Length, 3).TrimToChar('#')).Distinct().ToList();
         }

         return pages;
      }
   }
}
