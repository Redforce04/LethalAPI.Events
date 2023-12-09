// -----------------------------------------------------------------------
// <copyright file="UsingItemPrefix.cs" company="LethalAPI Event Team">
// Copyright (c) LethalAPI Event Team. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable InconsistentNaming
namespace LethalAPI.Events.Patches.Player;

using EventArgs.Items;
using EventArgs.Player;
using Handlers;
using HarmonyLib;
using LethalAPI.Events.Attributes;

/// <summary>
///     Patches the <see cref="Items.DeniableUsingItem"/> event.
/// </summary>
[EventPatch(typeof(Handlers.Items), nameof(Handlers.Items.DeniableUsingItem))]
[HarmonyPatch(typeof(GrabbableObject), nameof(GrabbableObject.ItemActivate))]
internal static class UsingItemPrefix
{
    [HarmonyPrefix]
    private static bool Prefix(GrabbableObject __instance, bool used, bool buttonDown = true)
    {
        // This needs to become a transpiler.
        DeniableUsingItemEventArgs ev = new (__instance.playerHeldBy, __instance);
        Handlers.Items.DeniableUsingItem.InvokeSafely(ev);
        return ev.IsAllowed;
    }
}