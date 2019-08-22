using System.IO;

namespace apk2theme
{
    class EnvHelper
    {
        public static void SetupWorkEnvironment()
        {
            try
            {
                if (!Directory.Exists(Globals.WorkingDirectory))
                    Directory.CreateDirectory(Globals.WorkingDirectory);
            }
            catch { }

            try
            {
                if (Directory.Exists(Globals.AndroidIcons))
                    Directory.Delete(Globals.AndroidIcons, true);
            }
            catch { }

            try
            {
                if (Directory.Exists(Globals.iPhoneIcons))
                    Directory.Delete(Globals.iPhoneIcons, true);
            }
            catch { }

            try
            {
                Directory.CreateDirectory(Globals.AndroidIcons);
                Directory.CreateDirectory(Globals.iPhoneIcons);
            }
            catch { }

            try
            {
                if (Directory.Exists(Path.Combine(Globals.WorkingDirectory, "valid.tmp")))
                    Directory.Delete(Path.Combine(Globals.WorkingDirectory, "valid.tmp"), true);
            }
            catch { }
        }
        public static void CleanWorkEnvironment(string theme_name)
        {
            ColorPrint.WriteLine("\tREMOVING: {0}..", Globals.AndroidIcons);
            Directory.Delete(Globals.AndroidIcons, true);
            if (!Directory.Exists(Path.Combine(Globals.WorkingDirectory, theme_name + ".theme")))
            {
                ColorPrint.WriteLine("\tCREATING: {0}..", Path.Combine(Globals.WorkingDirectory, theme_name + ".theme"));
                Directory.CreateDirectory(Path.Combine(Globals.WorkingDirectory, theme_name + ".theme"));
            }
            ColorPrint.WriteLine("\tMOVING: {0}..", Path.Combine(Globals.WorkingDirectory, theme_name + ".theme/IconBundles"));
            Directory.Move(Globals.iPhoneIcons, Path.Combine(Globals.WorkingDirectory, theme_name + ".theme/IconBundles"));
        }
    }
}
