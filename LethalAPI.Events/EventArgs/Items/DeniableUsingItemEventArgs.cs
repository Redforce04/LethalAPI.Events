// -----------------------------------------------------------------------
// <copyright file="DeniableUsingItemEventArgs.cs" company="LethalAPI Event Team">
// Copyright (c) LethalAPI Event Team. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Events.EventArgs.Items;

using GameNetcodeStuff;
using Handlers;
using LethalAPI.Events.Interfaces;

/// <summary>
///     Contains the arguments for the <see cref="Items.DeniableUsingItem"/> event.
/// </summary>
/// <param name="player">
///     The player using the item.
/// </param>
/// <param name="item">
///     The item being used.
/// </param>
/// <param name="isAllowed">
///     Indicates whether the event is allowed to execute.
/// </param>
public class DeniableUsingItemEventArgs(PlayerControllerB player, GrabbableObject item, bool isAllowed = true) : IDeniableEvent, IItemEvent, IPlayerEvent
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