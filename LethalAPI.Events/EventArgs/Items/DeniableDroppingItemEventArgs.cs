// -----------------------------------------------------------------------
// <copyright file="DeniableDroppingItemEventArgs.cs" company="LethalAPI Event Team">
// Copyright (c) LethalAPI Event Team. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Events.EventArgs.Items;

using GameNetcodeStuff;

/// <summary>
///     Represents the event args that are called before an item is dropped.
/// </summary>
/// <param name="player">
///     The player that is dopping the item..
/// </param>
/// <param name="item">
///     The item that is being dropped.
/// </param>
/// <param name="isAllowed">
///     Indicates whether the event is allowed to execute.
/// </param>
public sealed class DeniableDroppingItemEventArgs(PlayerControllerB player, GrabbableObject item, bool isAllowed = true) : IDeniableEvent, IItemEvent, IPlayerEvent
{
    /// <inheritdoc />
    public bool IsAllowed { get; set; } = isAllowed;

    /// <inheritdoc />
    bool IDeniableEvent.HardDenied { get; set; } = false;

    /// <inheritdoc />
    public GrabbableObject Item { get; init; } = item;

    /// <inheritdoc />
    public PlayerControllerB Player { get; init; } = player;
}