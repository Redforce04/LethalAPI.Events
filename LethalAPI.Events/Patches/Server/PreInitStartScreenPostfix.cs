// -----------------------------------------------------------------------
// <copyright file="PreInitStartScreenPostfix.cs" company="LethalAPI Modding Community">
// Copyright (c) LethalAPI Modding Community. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Events.Patches.Server;

using HarmonyLib;
using LethalAPI.Events.Attributes;
using MEC;

/// <summary>
///     Patches the <see cref="Handlers.Server.GameOpened"/> event.
/// </summary>
[EventPatch(typeof(Handlers.Server), nameof(Handlers.Server.GameOpened))]
[HarmonyPatch(typeof(PreInitSceneScript), "Start")]
internal static class PreInitStartScreenPostfix
{
    [HarmonyPostfix]
    private static void Postfix(PreInitSceneScript __instance)
    {
        Timing.Instance = __instance.gameObject.AddComponent<Timing>();
        Handlers.Server.GameOpened.InvokeSafely();
    }
}