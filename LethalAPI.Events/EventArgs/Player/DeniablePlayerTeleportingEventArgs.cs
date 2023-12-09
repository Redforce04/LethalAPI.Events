// -----------------------------------------------------------------------
// <copyright file="DeniablePlayerTeleportingEventArgs.cs" company="LethalAPI Event Team">
// Copyright (c) LethalAPI Event Team. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Events.EventArgs.Player;

using GameNetcodeStuff;

/// <summary>
///     Represents the event args that are called before a player is teleported to or from a location.
/// </summary>
/// <param name="player">
///     The player that is being teleported.
/// </param>
/// <param name="position">
///     The position that the player will be teleported to.
/// </param>
/// <param name="shipIsTarget">
///     Indicates whether or not the ship is the target.
/// </param>
/// <param name="isAllowed">
///     Indicates whether the event is allowed to execute.
/// </param>
public sealed class DeniablePlayerTeleportingEventArgs(PlayerControllerB player, Vector3 position, bool shipIsTarget, bool isAllowed = true) : IDeniableEvent, IPlayerEvent
{
    /// <inheritdoc />
    public bool IsAllowed { get; set; } = isAllowed;

    /// <inheritdoc />
    bool IDeniableEvent.HardDenied { get; set; } = false;

    /// <inheritdoc />
    public PlayerControllerB Player { get; init; } = player;

    /// <summary>
    ///     Gets a value indicating whether or not the ship is the target.
    /// </summary>
    public bool ShipIsTarget { get; init; } = shipIsTarget;

    /// <summary>
    ///     Gets or sets the position the player is being teleported too.
    /// </summary>
    public Vector3? TargetPosition { get; set; } = position;
}