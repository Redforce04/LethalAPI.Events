// -----------------------------------------------------------------------
// <copyright file="EnemyKillingPlayerEventArgs.cs" company="LethalAPI Event Team">
// Copyright (c) LethalAPI Event Team. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Events.EventArgs.Enemies;

using GameNetcodeStuff;
using LethalAPI.Events.Interfaces;

/// <summary>
///     Represents the event args that are called before a player is killed by an enemy.
/// </summary>
/// <param name="player">
///     The player that is being killed.
/// </param>
/// <param name="enemy">
///     The enemy that is killing the player.
/// </param>
/// <param name="isAllowed">
///     Indicates whether the event is allowed to execute.
/// </param>
public sealed class DeniableEnemyKillingPlayerEventArgs(PlayerControllerB player, EnemyAI enemy, bool isAllowed = true) : IEnemyEvent, IPlayerEvent, IDeniableEvent
{
    /// <summary>
    /// Gets the player that is being killed.
    /// </summary>
    public PlayerControllerB Player { get; init; } = player;

    /// <summary>
    /// Gets the enemy that is killing the player.
    /// </summary>
    public EnemyAI Enemy { get; init; } = enemy;

    /// <inheritdoc />
    public bool IsAllowed { get; set; } = isAllowed;

    /// <inheritdoc />
    bool IDeniableEvent.HardDenied { get; set; } = false;
}