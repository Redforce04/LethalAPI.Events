// -----------------------------------------------------------------------
// <copyright file="DeniablePickingUpItemEventArgs.cs" company="LethalAPI Event Team">
// Copyright (c) LethalAPI Event Team. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Events.EventArgs.Items;

using GameNetcodeStuff;

/// <summary>
///     Represents the event args that are called before an item is picked up.
/// </summary>
/// <param name="player">
///     The player picking up the item.
/// </param>
/// <param name="item">
///     The item being picked up.
/// </param>
/// <param name="isAllowed">
///     Indicates whether the event is allowed to execute.
/// </param>
public sealed class DeniablePickingUpItemEventArgs(PlayerControllerB player, GrabbableObject item, bool isAllowed = true) : IDeniableEvent, IPlayerEvent, IItemEvent
{
    /// <inheritdoc />
    public bool IsAllowed { get; set; } = isAllowed;

    /// <inheritdoc />
    bool IDeniableEvent.HardDenied { get; set; } = false;

    /// <inheritdoc />
    public PlayerControllerB Player { get; init; } = player;

    /// <inheritdoc />
    public GrabbableObject Item { get; init; } = item;
}