// -----------------------------------------------------------------------
// <copyright file="DeniableChangingSuitEventArgs.cs" company="LethalAPI Event Team">
// Copyright (c) LethalAPI Event Team. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Events.EventArgs.Player;

using GameNetcodeStuff;

/// <summary>
///     Represents the event args that are called before a player change's suits.
/// </summary>
/// <param name="player">
///     The player that is changing suits.
/// </param>
/// <param name="newSuit">
///     The suit that the player is changing to.
/// </param>
/// <param name="isAllowed">
///     Indicates whether the event is allowed to execute.
/// </param>
public sealed class DeniableChangingSuitEventArgs(PlayerControllerB player, UnlockableSuit newSuit, bool isAllowed = true) : IDeniableEvent, IPlayerEvent
{
    /// <inheritdoc />
    public bool IsAllowed { get; set; } = isAllowed;

    /// <inheritdoc />
    bool IDeniableEvent.HardDenied { get; set; } = false;

    /// <inheritdoc />
    public PlayerControllerB Player { get; init; } = player;

    /// <summary>
    /// Gets the original suit that the player was wearing.
    /// </summary>
    public int OldSuitId { get; init; } = player.currentSuitID;

    /// <summary>
    /// Gets or sets the suit that the player is equipping.
    /// </summary>
    public UnlockableSuit NewSuit { get; set; } = newSuit;
}