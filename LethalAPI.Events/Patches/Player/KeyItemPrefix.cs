// -----------------------------------------------------------------------
// <copyright file="KeyItemPrefix.cs" company="LethalAPI Event Team">
// Copyright (c) LethalAPI Event Team. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable InconsistentNaming
namespace LethalAPI.Events.Patches.Player;

using Attributes;
using HarmonyLib;
using UnityEngine;

/// <summary>
///     Patches the <see cref="Handlers.Map.DeniableUnlockingDoor"/> event.
/// </summary>
[EventPatch(typeof(Handlers.Map), nameof(Handlers.Map.DeniableUnlockingDoor))]
[HarmonyPatch(typeof(KeyItem), nameof(KeyItem.ItemActivate))]
internal static class KeyItemPrefix
{
    [HarmonyPrefix]
    private static bool Prefix(KeyItem __instance, bool used, bool buttonDown = true)
    {
        // This needs to become a transpiler.
        return true;
    }
}