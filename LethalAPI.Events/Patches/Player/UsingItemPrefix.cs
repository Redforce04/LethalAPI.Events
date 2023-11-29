// -----------------------------------------------------------------------
// <copyright file="UsingItemPrefix.cs" company="Redforce04">
// Copyright (c) Redforce04. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable InconsistentNaming
namespace LethalAPI.Events.Patches.Player;

using EventArgs.Player;
using HarmonyLib;
using LethalAPI.Events.Attributes;

/// <summary>
///     Patches the <see cref="Handlers.Player.UsingItem"/> event.
/// </summary>
[EventPatch(typeof(Handlers.Player), nameof(Handlers.Player.UsingKey))]
[HarmonyPatch(typeof(GrabbableObject), nameof(GrabbableObject.ItemActivate))]
internal static class UsingItemPrefix
{
    [HarmonyPrefix]
    private static bool Prefix(GrabbableObject __instance, bool used, bool buttonDown = true)
    {
        // This needs to become a transpiler.
        UsingItemEventArgs ev = new UsingItemEventArgs(__instance);
        Handlers.Player.UsingItem.InvokeSafely(ev);
        return ev.IsAllowed;
    }
}