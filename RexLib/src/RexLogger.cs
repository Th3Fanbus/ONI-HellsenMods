using KMod;
using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace RexLib
{
    public static class RexLogger
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void DoWriteLine(string level, object? obj)
            => Console.WriteLine($"{System.DateTime.UtcNow:[HH:mm:ss.fff]} [{Thread.CurrentThread.ManagedThreadId}] [{level}] [Rex/{RexUtils.AssemblyName}]: " + obj);

        public static void L(object? obj) => DoWriteLine("INFO", obj);
        public static void W(object? obj) => DoWriteLine("WARNING", obj);
        public static void E(object? obj) => DoWriteLine("ERROR", obj);

        public static void Log(object? obj) => L(obj);
        public static void LogWarning(object? obj) => W(obj);
        public static void LogError(object? obj) => E(obj);

        public static void LogVersion(UserMod2 usermod) => Log($"{usermod.mod.staticID} - Mod Version: {usermod.mod.packagedModInfo.version} ");
    }
}
