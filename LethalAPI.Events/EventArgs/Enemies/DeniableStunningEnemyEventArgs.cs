// -----------------------------------------------------------------------
// <copyright file="StunningEnemyEventArgs.cs" company="LethalAPI Event Team">
// Copyright (c) LethalAPI Event Team. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Events.EventArgs.Enemies;

using GameNetcodeStuff;
using LethalAPI.Events.Interfaces;

/// <summary>
///     Represents the event args that are called before an enemy is killed by a player.
/// </summary>
/// <param name="player">
///     The player who is stunning the enemy.
/// </param>
/// <param name="enemy">
///     The enemy that is being stunned.
/// </param>
/// <param name="isAllowed">
///     Indicates whether the event is allowed to execute.
/// </param>
public sealed class DeniableStunningEnemyEventArgs(PlayerControllerB player, EnemyAI enemy, bool isAllowed = true) : IEnemyEvent, IPlayerEvent, IDeniableEvent
{
    /// <summary>
    ///     Gets the player that is stunning the enemy.
    /// </summary>
    public PlayerControllerB Player { get; init; } = player;

    /// <summary>
    ///     Gets the enemy that is being stunned.
    /// </summary>
    public EnemyAI Enemy { get; init; } = enemy;

    /// <inheritdoc />
    public bool IsAllowed { get; set; } = isAllowed;

    /// <inheritdoc />
    bool IDeniableEvent.HardDenied { get; set; } = false;
}