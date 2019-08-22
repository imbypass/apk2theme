using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apk2theme
{
    static class ColorPrint
    {
        private static ConsoleColor tmp;
        public static void WriteLine(string format, ConsoleColor color = ConsoleColor.Gray, params object[] arg)
        {
            tmp = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(format, arg);
            Console.ForegroundColor = tmp;
        }
        public static void WriteLine(string format, ConsoleColor color)
        {
            WriteLine(format, color, null);
        }
        public static void WriteLine(string format, params object[] arg)
        {
            WriteLine(format, ConsoleColor.Gray, arg);
        }
        public static void WriteLine(string format)
        {
            WriteLine(format, null);
        }
        public static void WriteLine()
        {
            WriteLine("");
        }
        public static void Write(string format, ConsoleColor color = ConsoleColor.Gray, params object[] arg)
        {
            tmp = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(format, arg);
            Console.ForegroundColor = tmp;
        }
        public static void Write(string format, ConsoleColor color)
        {
            Write(format, color, null);
        }
        public static void Write(string format, params object[] arg)
        {
            Write(format, ConsoleColor.Gray, arg);
        }
        public static void Write(string format)
        {
            Write(format, null);
        }
        public static void Write()
        {
            Write("");
        }
    }
}
