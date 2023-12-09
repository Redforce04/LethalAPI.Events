// -----------------------------------------------------------------------
// <copyright file="DeniableDamagingPlayerEventArgs.cs" company="LethalAPI Event Team">
// Copyright (c) LethalAPI Event Team. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Events.EventArgs.Player;

using GameNetcodeStuff;
using UnityEngine;

/// <summary>
///     Represents the event args that are called before a player is damaged.
/// </summary>
/// <param name="player">
///     The player taking damage.
/// </param>
/// <param name="damage">
///     The amount of damage the player is taking.
/// </param>
/// <param name="hasDamageSFX">
///     Gets a value indicating whether the damage has a sound effect that will be played.
/// </param>
/// <param name="callRPC">
///     Gets a value indicating whether the damage will result in an rpc being called.
/// </param>
/// <param name="causeOfDeath">
///     Gets or sets the type of damage being applied.
/// </param>
/// <param name="deathAnimation">
///     Gets or sets the death animation to play.
/// </param>
/// <param name="fallDamage">
///     Gets or sets a value indicating whether or not the damage is due to fall damage.
/// </param>
/// <param name="force">
///     Gets or sets the force that will be applied to the damage if they die.
/// </param>
/// <param name="isAllowed">
///     Indicates whether the event is allowed to execute.
/// </param>
public sealed class DeniableDamagingPlayerEventArgs(
    PlayerControllerB player,
    int damage,
    bool hasDamageSFX,
    bool callRPC,
    CauseOfDeath causeOfDeath,
    int deathAnimation,
    bool fallDamage,
    Vector3 force,
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
    ///     Gets a value indicating whether the damage will result in an rpc being called.
    /// </summary>
    public bool CallRPC { get; init; } = callRPC;

    /// <summary>
    ///     Gets a value indicating whether the damage has a sound effect that will be played.
    /// </summary>
    public bool HasDamageSoundEffect { get; init; } = hasDamageSFX;

    /// <summary>
    ///     Gets or sets the amount of damage the player is taking.
    /// </summary>
    public int Damage { get; set; } = damage;

    /// <summary>
    ///     Gets or sets the type of damage being applied.
    /// </summary>
    public CauseOfDeath DamageType { get; set; } = causeOfDeath;

    /// <summary>
    ///     Gets or sets the death animation to play.
    /// </summary>
    public int DeathAnimation { get; set; } = deathAnimation;

    /// <summary>
    ///     Gets or sets the force that will be applied to the damage if they die.
    /// </summary>
    public Vector3 Force { get; set; } = force;

    /// <summary>
    ///     Gets or sets a value indicating whether or not the damage is due to fall damage.
    /// </summary>
    public bool IsFallDamage { get; set; } = fallDamage;
}