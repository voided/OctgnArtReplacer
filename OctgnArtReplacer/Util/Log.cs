using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctgnArtReplacer.Util
{
    static class Log
    {
        static object logLock = new object();


        public static void Error(string message)
        {
            Write(ConsoleColor.Red, message);
        }
        public static void Warning(string message)
        {
            Write(ConsoleColor.Yellow, message);
        }
        public static void Info(string message)
        {
            Write(ConsoleColor.Gray, message);
        }
        public static void Good(string message)
        {
            Write(ConsoleColor.Green, message);
        }


        static void Write(ConsoleColor color, string message)
        {
            lock (logLock)
            {
                var origColor = Console.ForegroundColor;
                Console.ForegroundColor = color;

                Console.WriteLine(message);

                Console.ForegroundColor = origColor;
            }
        }
    }
}
