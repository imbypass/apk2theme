using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace apk2theme
{
    class Program
    {
        static void Main(string[] args)
        {

            // Display our pretty little header message. ;)
            PrintBannerMessage();

            List<string[]> icon_list = new List<string[]>();

            // Get our icon filter list. We check for a local file first, then fallback to the online database.
            if (!GetFilterList(icon_list)) return;

            // Check to make sure we provided at least one APK file for the converter to use.
            RunArgsCheck(ref args);

            // A simple for loop to handle any/all files thrown at the converter.
            foreach (string apk in args)
            {
                // Read some file information about our APK file.
                FileInfo apk_info = new FileInfo(apk);

                // Get a basic name of the APK file (without any .apk or .zip extensions)
                string apk_name = new FileInfo(apk).Name.Replace(".apk", "").Replace(".zip", "");

                // Set up our temporary work environment. This basically just clears and creates a directory in APPDATA for us to move files around in.
                EnvHelper.SetupWorkEnvironment();

                // Check to see if the given file is a valid APK. We do this by attempting to extract one icon; if we fail, it can't be a valid icon pack.
                if (!CheckPackage(apk, apk_info)) continue;

                // Extract apk's icon folder. These will be extracted to our working directory that we created earlier.
                if (!ExtractIcons(apk)) continue;

                // Rename all icons according to our filter. Rename and move all of the icons found in the filter list.
                if (!RenameIcons(icon_list)) continue;

                // Clean up after all of the copying. Move some folders around to clean up and delete any unused android icons.
                if (!CleanUp(apk, apk_name)) continue;

                ColorPrint.WriteLine("Conversion successful!", ConsoleColor.Green);
                Globals.ConvertedIcons = 0;
                Globals.ConvertedThemes++;
            }

            ColorPrint.WriteLine("\nConverted {0} themes!", Globals.ConvertedThemes);
        }


        public static bool GetFilterList(List<string[]> icon_list)
        {
            try
            {
                string[] icons_file = ApkHelper.GetIconConversionList();
                foreach (string icon in icons_file)
                {
                    string[] icon_data = icon.Split('|');
                    icon_list.Add(new string[] { icon_data[0], icon_data[1] });
                }
                ColorPrint.WriteLine("\tLoaded {0} filter entries!", icon_list.Count);
                ColorPrint.WriteLine("\nSuccessfully retrieved filter list", ConsoleColor.Green);
                return true;
            } catch
            {
                ColorPrint.WriteLine("\nFailed retrieving filter list", ConsoleColor.Red);
                return false;
            }
        }
        public static bool CheckPackage(string apk_location, FileInfo apk_info)
        {
            ColorPrint.WriteLine("\nChecking package");
            ColorPrint.WriteLine("\t{0}", apk_info.FullName);
            bool isInvalid = (!apk_info.Extension.Contains("apk") && !apk_info.Extension.Contains("zip")) || !ApkHelper.CheckIsValidPackage(apk_location);
            if (isInvalid)
            {
                ColorPrint.WriteLine("\n{0} is invalid\n", ConsoleColor.Red, apk_info.Name);
                return false;
            }
            else
            {
                ColorPrint.WriteLine("\n{0} is valid\n", ConsoleColor.Green, apk_info.Name);
                return true;
            }
        }
        public static bool ExtractIcons(string apk_location)
        {
            ColorPrint.WriteLine("Extracting icons");
            if (ApkHelper.ExtractDrawableFolder(apk_location))
            {
                ColorPrint.WriteLine("\nSuccessfully extracted icons\n", ConsoleColor.Green);
                return true;
            }
            else
            {
                ColorPrint.WriteLine("\nFailed extracting icons\n", ConsoleColor.Red);
                return false;
            }
        }
        public static bool RenameIcons(List<string[]> icon_list)
        {
            ColorPrint.WriteLine("Converting icons");
            for (int i = 0; i < icon_list.Count; i++)
                ApkHelper.ConvertIcon(icon_list, i);

            if (Globals.ConvertedIcons > 0)
            {
                ColorPrint.WriteLine("\nSuccessfully converted icons\n", ConsoleColor.Green);
                return true;
            }
            else
            {
                ColorPrint.WriteLine("\nFailed converting icons\n", ConsoleColor.Red);
                return false;
            }
        }
        public static bool CleanUp(string apk_location, string theme_name)
        {
            ColorPrint.WriteLine("Cleaning up");
            try
            {
                EnvHelper.CleanWorkEnvironment(theme_name);
                string apk_folder = new FileInfo(apk_location).DirectoryName;
                try
                {
                    if (Directory.Exists(Path.Combine(apk_folder, theme_name + ".theme")))
                    {
                        ColorPrint.WriteLine("\tREMOVING: {0}..", Path.Combine(apk_folder, theme_name + ".theme"));
                        Directory.Delete(Path.Combine(apk_folder, theme_name + ".theme"), true);
                    }
                }
                catch { }
                ColorPrint.WriteLine("\tMOVING: {0}..", Path.Combine(Globals.WorkingDirectory, theme_name + ".theme"));
                Directory.Move(Path.Combine(Globals.WorkingDirectory, theme_name + ".theme"), Path.Combine(String.IsNullOrEmpty(Globals.OutputDirectory) ? apk_folder : Globals.OutputDirectory, theme_name + ".theme"));
                ColorPrint.WriteLine("\nSuccessfully cleaned up\n", ConsoleColor.Green);
                return true;
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
                ColorPrint.WriteLine("\nFailed cleaning up\n", ConsoleColor.Red);
                return false;
            }
        }


        static void RunArgsCheck(ref string[] args)
        {
            if (args.Length <= 0)
            {
                ColorPrint.WriteLine("\nERROR: No APK file was provided! Exiting...", ConsoleColor.Yellow);
                Environment.Exit(0);
            }
            if (args[0] == "--output")
            {
                Globals.OutputDirectory = args[1];
                args = args.Skip(2).ToArray();
            }
        }
        static void PrintBannerMessage()
        {
            string[] banner_components =
            {
                "────────────────────────────────────────────────────────────────",
                "───────────────────────────────",
                "[v" + Globals.CurrentVersion + "]",
                "───",
                "[" + Globals.ConversionListUrl.Split(new string[] { "/icon" }, StringSplitOptions.None)[0] + "]",
                "──"
            };
            string[] banner_title =
            {
                "                 _    ____  _   _                         ",
                "      __ _ _ __ | | _|___ \\| |_| |__   ___ _ __ ___   ___ ",
                "     / _` | '_ \\| |/ / __) | __| '_ \\ / _ \\ '_ ` _ \\ / _ \\",
                "    | (_| | |_) |   < / __/| |_| | | |  __/ | | | | |  __/",
                "     \\__,_| .__/|_|\\_\\_____|\\__|_| |_|\\___|_| |_| |_|\\___|",
                "          |_|                                             "
            };

            ColorPrint.WriteLine(banner_components[0], ConsoleColor.Blue);

            foreach (string line in banner_title)
                ColorPrint.WriteLine(line, ConsoleColor.White);

            ColorPrint.Write(banner_components[1], ConsoleColor.Blue);
            ColorPrint.Write(banner_components[2], ConsoleColor.White);
            ColorPrint.Write(banner_components[3], ConsoleColor.Blue);
            ColorPrint.Write(banner_components[4], ConsoleColor.White);
            ColorPrint.WriteLine(banner_components[5], ConsoleColor.Blue);

            ColorPrint.WriteLine();
        }
    }
}
