// -----------------------------------------------------------------------
// <copyright file="LoadingGlobalSavePostfix.cs" company="LethalAPI Modding Community">
// Copyright (c) LethalAPI Modding Community. All rights reserved.
// Licensed under the GPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Events.Patches.Server;

using HarmonyLib;
using LethalAPI.Events.Attributes;
using LethalAPI.Events.EventArgs.Server;
using LethalAPI.Events.Handlers;

#pragma warning disable SA1402

/// <summary>
///     Patches the <see cref="Handlers.Server.LoadingSave"/> event.
/// </summary>
[HarmonyPatch(typeof(GameNetworkManager), "Start")]
[EventPatch(typeof(Handlers.Server), nameof(Handlers.Server.LoadingSave))]
internal static class LoadingGlobalSavePostfix
{
    [HarmonyPostfix]
    private static void Postfix()
    {
        Handlers.Server.LoadingSave.InvokeSafely(new LoadingSaveEventArgs("LCGeneralSaveData", LoadedItem.LastSelectedSave));
    }
}

/// <summary>
///     Patches the <see cref="Server.LoadingSave"/> event.
/// </summary>
[HarmonyPatch(typeof(StartOfRound), "SpawnUnlockable")]
[EventPatch(typeof(Server), nameof(Server.LoadingSave))]
internal static class SpawnUnlockablePostfix
{
    [HarmonyPostfix]
    private static void Postfix(StartOfRound __instance)
    {
        Handlers.Server.LoadingSave.InvokeSafely(new LoadingSaveEventArgs(GameNetworkManager.Instance.currentSaveFileName, LoadedItem.SpawnUnlockable));
    }
}

/// <summary>
///     Patches the <see cref="Server.LoadingSave"/> event.
/// </summary>
[HarmonyPatch(typeof(StartOfRound), "LoadUnlockables")]
[EventPatch(typeof(Server), nameof(Server.LoadingSave))]
internal static class LoadUnlockablesPostfix
{
    [HarmonyPostfix]
    private static void Postfix(StartOfRound __instance)
    {
        Handlers.Server.LoadingSave.InvokeSafely(new LoadingSaveEventArgs(GameNetworkManager.Instance.currentSaveFileName, LoadedItem.LoadUnlockables));
    }
}

/// <summary>
///     Patches the <see cref="Server.LoadingSave"/> event.
/// </summary>
[HarmonyPatch(typeof(StartOfRound), "LoadShipGrabbableItems")]
[EventPatch(typeof(Server), nameof(Server.LoadingSave))]
internal static class LoadShipGrabbableItemsPostfix
{
    [HarmonyPostfix]
    private static void Postfix(StartOfRound __instance)
    {
        Handlers.Server.LoadingSave.InvokeSafely(new LoadingSaveEventArgs(GameNetworkManager.Instance.currentSaveFileName, LoadedItem.LoadShipGrabbableItems));
    }
}

/// <summary>
///     Patches the <see cref="Server.LoadingSave"/> event.
/// </summary>
[HarmonyPatch(typeof(StartOfRound), "SetTimeAndPlanetToSavedSettings")]
[EventPatch(typeof(Server), nameof(Server.LoadingSave))]
internal static class SetTimeAndPlanetPostfix
{
    [HarmonyPostfix]
    private static void Postfix(StartOfRound __instance)
    {
        Handlers.Server.LoadingSave.InvokeSafely(new LoadingSaveEventArgs(GameNetworkManager.Instance.currentSaveFileName, LoadedItem.SetTimeAndPlanetToSavedSettings));
    }
}