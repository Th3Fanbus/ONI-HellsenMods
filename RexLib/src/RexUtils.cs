using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace RexLib
{
    public static partial class Extensions
    {
#pragma warning disable IDE0029 // Null check cannot be simplified because Unity is cursed
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T? GetValid<T>(this T? obj) where T : class? => obj == null ? null : obj;
#pragma warning restore IDE0029
        public static bool IsNull(this WorkerBase? worker) => worker == null || worker.HasTag(GameTags.Dying) || worker.HasTag(GameTags.Dead);
        public static bool IsValid(this WorkerBase? worker) => !IsNull(worker);
        public static bool IsNull(this UnityEngine.Object? obj) => obj == null;
        public static bool IsValid(this UnityEngine.Object? obj) => !IsNull(obj);

        public static bool TryGetDef<DefType>(this GameObject go, [NotNullWhen(true)] out DefType? def) where DefType : StateMachine.BaseDef
            => (def = go.TryGetComponent(out StateMachineController smc) ? smc.GetDef<DefType>() : null) != null;
    }

    public static class RexUtils
    {
        public static string AssemblyName => Assembly.GetExecutingAssembly().GetName().Name;
        public static string ModPath => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static IEnumerable<MethodBase> MethodFromTypes(string methodName, params Type?[] targets)
            => targets.Select(tgt => AccessTools.Method(tgt, methodName)).Where(tgt => tgt is not null);

        private static Texture2D LoadTexture(string assetPath)
        {
            string filepath = Path.Combine(ModPath, assetPath);
            Texture2D texture = new(1, 1);
            if (File.Exists(filepath)) {
                Debug.Log($"Found texture at: {filepath}");
                texture.LoadImage(File.ReadAllBytes(filepath));
            } else {
                Debug.LogWarning($"Texture does not exist at: {filepath}");
            }
            return texture;
        }

        public static void RegisterSprite(string name)
        {
            Texture2D texture = LoadTexture($"assets/sprites/{name}.png");
            Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), Vector2.zero);
            Assets.Sprites.Add((HashedString)name, sprite);
        }
    }
}
