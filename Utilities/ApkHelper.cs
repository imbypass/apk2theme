using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;

namespace apk2theme
{
    public static class ApkHelper
    {
        public static string[] GetIconConversionList()
        {
            if (File.Exists("icons.txt"))
            {
                ColorPrint.WriteLine("Found local icons file! Using local filter list..");
                return File.ReadAllText("icons.txt").Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            }
            using (WebClient c = new WebClient() { Proxy = null })
            {
                ColorPrint.WriteLine("Retrieving online icon filter list..");
                return c.DownloadString(Globals.ConversionListUrl).Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            }
        }
        public static bool CheckIsValidPackage(string apk_location)
        {
            string valid_file = Path.Combine(Globals.WorkingDirectory, "valid.tmp");

            if (File.Exists(valid_file))
                File.Delete(valid_file);

            string directory = "res\\drawable-nodpi-v4";
            try
            {
                using (ZipArchive archive = ZipFile.OpenRead(apk_location))
                {
                    var result = from currEntry in archive.Entries
                                 where Path.GetDirectoryName(currEntry.FullName) == directory
                                 where !String.IsNullOrEmpty(currEntry.Name)
                                 select currEntry;


                    result.ElementAt(0).ExtractToFile(valid_file);
                    File.Delete(valid_file);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        public static bool ExtractDrawableFolder(string apk_location)
        {
            try
            {
                string directory = "res\\drawable-nodpi-v4";
                using (ZipArchive archive = ZipFile.OpenRead(apk_location))
                {
                    var result = from currEntry in archive.Entries
                                 where Path.GetDirectoryName(currEntry.FullName) == directory
                                 where !String.IsNullOrEmpty(currEntry.Name)
                                 select currEntry;

                    foreach (ZipArchiveEntry entry in result)
                    {
                        entry.ExtractToFile(Path.Combine(Globals.AndroidIcons, entry.Name));
                        Globals.ExtractedIcons++;
                    }
                    ColorPrint.WriteLine("\tExtracted {0} icons from package", Globals.ExtractedIcons);
                    Globals.ExtractedIcons = 0;
                    return true;
                }
            }
            catch { return false; }
        }
        public static bool ConvertIcon(List<string[]> icon_list, int index)
        {

            string android_icon = string.Format("{0}.png", icon_list[index][0]);
            string iphone_icon = string.Format("{0}-large.png", icon_list[index][1]);

            string android_icon_path = Path.Combine(Globals.AndroidIcons, android_icon);
            string iphone_icon_path = Path.Combine(Globals.iPhoneIcons, iphone_icon);

            try
            {
                if (File.Exists(android_icon_path))
                {
                    if (!File.Exists(iphone_icon_path))
                    {
                        ColorPrint.WriteLine("\t" + string.Format("{0} -> {1}", android_icon, iphone_icon));
                        File.Copy(android_icon_path, iphone_icon_path);
                        Globals.ConvertedIcons++;
                        return true;
                    }
                }
            }
            catch { }
            return false;
        }
    }
}
