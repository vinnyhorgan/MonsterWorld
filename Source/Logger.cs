using System;
using System.Runtime.InteropServices;
using Raylib_cs;

namespace MonsterWorld
{
    static class Logger
    {
        public static unsafe void Init(bool debug)
        {
            Raylib.SetTraceLogCallback(&LogCustom);

            if (debug)
            {
                Raylib.SetTraceLogLevel(TraceLogLevel.LOG_ALL);
            }
        }

        public static void Info(string message)
        {
            Raylib.TraceLog(TraceLogLevel.LOG_INFO, message);
        }

        public static void Warning(string message)
        {
            Raylib.TraceLog(TraceLogLevel.LOG_WARNING, message);
        }

        public static void Error(string message)
        {
            Raylib.TraceLog(TraceLogLevel.LOG_ERROR, message);
        }

        public static void Debug(string message)
        {
            Raylib.TraceLog(TraceLogLevel.LOG_DEBUG, message);
        }

        [UnmanagedCallersOnly(CallConvs = new[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
        private static unsafe void LogCustom(int logLevel, sbyte* text, sbyte* args)
        {
            var message = Logging.GetLogMessage(new IntPtr(text), new IntPtr(args));

            Console.Write($"[{DateTime.Now:G}] ");

            switch ((TraceLogLevel)logLevel)
            {
                case TraceLogLevel.LOG_INFO:
                    Console.Write("[");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("info");
                    Console.ResetColor();
                    Console.Write("] ");
                    Console.WriteLine(message);
                    break;

                case TraceLogLevel.LOG_WARNING:
                    Console.Write("[");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("warning");
                    Console.ResetColor();
                    Console.Write("] ");
                    Console.WriteLine(message);
                    break;

                case TraceLogLevel.LOG_ERROR:
                    Console.Write("[");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("error");
                    Console.ResetColor();
                    Console.Write("] ");
                    Console.WriteLine(message);
                    break;

                case TraceLogLevel.LOG_DEBUG:
                    Console.Write("[");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write("debug");
                    Console.ResetColor();
                    Console.Write("] ");
                    Console.WriteLine(message);
                    break;
            }
        }
    }
}
