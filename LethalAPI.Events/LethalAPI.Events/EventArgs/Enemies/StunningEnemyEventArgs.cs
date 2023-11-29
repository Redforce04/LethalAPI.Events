// -----------------------------------------------------------------------
// <copyright file="StunningEnemyEventArgs.cs" company="Redforce04">
// Copyright (c) Redforce04. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Events.EventArgs.Enemies;

using GameNetcodeStuff;
using LethalAPI.Events.Interfaces;

/// <summary>
///     Represents the event args that are called before an enemy is killed by a player.
/// </summary>
public sealed class StunningEnemyEventArgs : IEnemyEvent, IPlayerEvent, IDeniableEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StunningEnemyEventArgs"/> class.
    /// </summary>
    /// <param name="player">
    ///     The player who is stunning the enemy.
    /// </param>
    /// <param name="enemy">
    ///     The enemy that is being stunned.
    /// </param>
    /// <param name="isAllowed">
    ///     Indicates whether the event is allowed to occur.
    /// </param>
    public StunningEnemyEventArgs(PlayerControllerB player, EnemyAI enemy, bool isAllowed = true)
    {
        this.Player = player;
        this.Enemy = enemy;
        this.IsAllowed = isAllowed;
    }

    /// <summary>
    ///     Gets the player that is stunning the enemy.
    /// </summary>
    public PlayerControllerB Player { get; init; }

    /// <summary>
    ///     Gets the enemy that is being stunned.
    /// </summary>
    public EnemyAI Enemy { get; init; }

    /// <inheritdoc />
    public bool IsAllowed { get; set; }
}