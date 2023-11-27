// -----------------------------------------------------------------------
// <copyright file="SavingPostfix.cs" company="LethalAPI Modding Community">
// Copyright (c) LethalAPI Modding Community. All rights reserved.
// Licensed under the GPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable InconsistentNaming
namespace LethalAPI.Events.Patches.Server;

using HarmonyLib;
using LethalAPI.Events.Attributes;
using LethalAPI.Events.EventArgs.Server;

/// <summary>
///     Patches the <see cref="Handlers.Server.Saving"/> event.
/// </summary>
[HarmonyPatch(typeof(GameNetworkManager), "SaveGameValues")]
[EventPatch(typeof(Handlers.Server), nameof(Handlers.Server.Saving))]
internal static class SaveGameValuesPostfix
{
    [HarmonyPostfix]
    private static void Postfix(GameNetworkManager __instance)
    {
        Handlers.Server.Saving.InvokeSafely(new SavingEventArgs(__instance.currentSaveFileName, SaveItem.GameValues));
    }
}

/// <summary>
///     Patches the <see cref="Handlers.Server.Saving"/> event.
/// </summary>
[HarmonyPatch(typeof(GameNetworkManager), "SaveLocalPlayerValues")]
[EventPatch(typeof(Handlers.Server), nameof(Handlers.Server.Saving))]
internal static class SaveLocalPlayerValues
{
    [HarmonyPostfix]
    private static void Postfix(GameNetworkManager __instance)
    {
        Handlers.Server.Saving.InvokeSafely(new SavingEventArgs("LCGeneralSaveData", SaveItem.LocalPlayerValues));
    }
}

/// <summary>
///     Patches the <see cref="Handlers.Server.Saving"/> event.
/// </summary>
[HarmonyPatch(typeof(GameNetworkManager), "SaveItemsInShip")]
[EventPatch(typeof(Handlers.Server), nameof(Handlers.Server.Saving))]
internal static class SaveItemsInShipPostfix
{
    [HarmonyPostfix]
    private static void Postfix(GameNetworkManager __instance)
    {
        Handlers.Server.Saving.InvokeSafely(new SavingEventArgs(__instance.currentSaveFileName, SaveItem.ShipItems));
    }
}

/// <summary>
///     Patches the <see cref="Handlers.Server.Saving"/> event.
/// </summary>
[HarmonyPatch(typeof(GameNetworkManager), "ConvertUnsellableItemsToCredits")]
[EventPatch(typeof(Handlers.Server), nameof(Handlers.Server.Saving))]
internal static class SaveUnsellableItemsPostfix
{
    [HarmonyPostfix]
    private static void Postfix(GameNetworkManager __instance)
    {
        Handlers.Server.Saving.InvokeSafely(new SavingEventArgs(__instance.currentSaveFileName, SaveItem.UnsellableItems));
    }
}