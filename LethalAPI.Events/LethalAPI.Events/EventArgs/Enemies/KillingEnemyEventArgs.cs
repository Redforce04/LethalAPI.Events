// -----------------------------------------------------------------------
// <copyright file="KillingEnemyEventArgs.cs" company="Redforce04">
// Copyright (c) Redforce04. All rights reserved.
// Licensed under the GPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Events.EventArgs.Enemies;

using GameNetcodeStuff;
using LethalAPI.Events.Interfaces;

/// <summary>
///     Represents the event args that are called before an enemy is killed by a player.
/// </summary>
public sealed class KillingEnemyEventArgs : IEnemyEvent, IPlayerEvent, IDeniableEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="KillingEnemyEventArgs"/> class.
    /// </summary>
    /// <param name="player">
    ///     The player that is killing the enemy.
    /// </param>
    /// <param name="enemy">
    ///     The enemy that is being killed.
    /// </param>
    /// <param name="isAllowed">
    ///     Indicates whether the event is allowed to occur.
    /// </param>
    public KillingEnemyEventArgs(PlayerControllerB player, EnemyAI enemy, bool isAllowed = true)
    {
        this.Player = player;
        this.Enemy = enemy;
        this.IsAllowed = isAllowed;
    }

    /// <summary>
    ///     Gets the player killing the enemy.
    /// </summary>
    public PlayerControllerB Player { get; init; }

    /// <summary>
    ///     Gets the enemy being killed .
    /// </summary>
    public EnemyAI Enemy { get; init; }

    /// <inheritdoc />
    public bool IsAllowed { get; set; }
}