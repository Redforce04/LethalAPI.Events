// -----------------------------------------------------------------------
// <copyright file="DeniableEquippingItemEventArgs.cs" company="LethalAPI Event Team">
// Copyright (c) LethalAPI Event Team. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Events.EventArgs.Items;

using GameNetcodeStuff;

/// <summary>
///     Represents the event args that are called before an item is equipped.
/// </summary>
/// <param name="isAllowed">
///     Indicates whether the event is allowed to execute.
/// </param>
public sealed class DeniableEquippingItemEventArgs(PlayerControllerB player, GrabbableObject item, bool isAllowed = true) : IDeniableEvent
{
    /// <inheritdoc />
    public bool IsAllowed { get; set; } = isAllowed;

    /// <inheritdoc />
    bool IDeniableEvent.HardDenied { get; set; } = false;
}