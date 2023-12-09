// -----------------------------------------------------------------------
// <copyright file="DeniablePlayerDyingEventArgs.cs" company="LethalAPI Event Team">
// Copyright (c) LethalAPI Event Team. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Events.EventArgs.Player;

using GameNetcodeStuff;

/// <summary>
///     Represents the event args that are called before a player dies due to any reason.
/// </summary>
/// <param name="player">
///     The player who is dying.
/// </param>
/// <param name="bodyVelocity">
///     The velocity to fling the body at.
/// </param>
/// <param name="spawnBody">
///     Indicates whether or not a ragdoll will be spawned.
/// </param>
/// <param name="deathAnimation">
///     The death animation to play.
/// </param>
/// <param name="causeOfDeath">
///     The cause of the death.
/// </param>
/// <param name="isAllowed">
///     Indicates whether the event is allowed to execute.
/// </param>
public sealed class DeniablePlayerDyingEventArgs(
    PlayerControllerB player,
    Vector3 bodyVelocity,
    bool spawnBody,
    int deathAnimation,
    CauseOfDeath causeOfDeath,
    bool isAllowed = true)
    : IDeniableEvent, IPlayerEvent
{
    /// <inheritdoc />
    public bool IsAllowed { get; set; } = isAllowed;

    /// <inheritdoc />
    bool IDeniableEvent.HardDenied { get; set; } = false;

    /// <inheritdoc />
    public PlayerControllerB Player { get; init; } = player;

    /// <summary>
    ///     Gets or sets the velocity to fling the body.
    /// </summary>
    public Vector3 BodyVelocity { get; set; } = bodyVelocity;

    /// <summary>
    ///     Gets or sets a value indicating whether or not to spawn a ragdoll.
    /// </summary>
    public bool SpawnBody { get; set; } = spawnBody;

    /// <summary>
    ///     Gets or sets the death animation to player after the death.
    /// </summary>
    public int DeathAnimation { get; set; } = deathAnimation;

    /// <summary>
    ///     Gets or sets the death animation to player after the death.
    /// </summary>
    public CauseOfDeath CauseOfDeath { get; set; } = causeOfDeath;
}