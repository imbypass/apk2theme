using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apk2theme
{
    public static class Globals
    {
        // The current version of our converter.
        public const double CurrentVersion = 1.05;

        // The location of the online icon database.
        public const string ConversionListUrl = "https://imbypass.pw/icon_conversion_list.txt";

        // Int counters to keep track of basic stats.
        public static int ConvertedIcons = 0;
        public static int ExtractedIcons = 0;
        public static int ConvertedThemes = 0;

        // Working directories used in the conversion process.
        public static string WorkingDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "apk2theme");
        public static string AndroidIcons = Path.Combine(WorkingDirectory, "AndroidIcons");
        public static string iPhoneIcons = Path.Combine(WorkingDirectory, "IconBundles");

        public static string OutputDirectory = string.Empty;
    }
}
