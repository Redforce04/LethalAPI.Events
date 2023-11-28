// -----------------------------------------------------------------------
// <copyright file="EnemyAttackingPlayerEventArgs.cs" company="Redforce04">
// Copyright (c) Redforce04. All rights reserved.
// Licensed under the GPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Events.EventArgs.Enemies;

using GameNetcodeStuff;
using LethalAPI.Events.Interfaces;

/// <summary>
///     Represents the event args that are called before a player is attacked by an enemy.
/// </summary>
public sealed class EnemyAttackingPlayerEventArgs : IEnemyEvent, IPlayerEvent, IDeniableEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EnemyAttackingPlayerEventArgs"/> class.
    /// </summary>
    /// <param name="damage">
    ///     The damage being dealt. For enemies that do a one-shot kill, this should be set to zero.
    /// </param>
    /// <param name="player">
    ///     The player that is being attacked.
    /// </param>
    /// <param name="enemy">
    ///     The enemy that is attacking the player.
    /// </param>
    /// <param name="isAllowed">
    ///     Indicates whether the event is allowed to occur.
    /// </param>
    public EnemyAttackingPlayerEventArgs(int damage, PlayerControllerB player, EnemyAI enemy, bool isAllowed = true)
    {
        this.Damage = damage;
        this.Player = player;
        this.Enemy = enemy;
        this.IsAllowed = isAllowed;
    }

    /// <summary>
    /// Gets the player that is being attacked.
    /// </summary>
    public PlayerControllerB Player { get; init; }

    /// <summary>
    /// Gets the enemy that is attacking the player.
    /// </summary>
    public EnemyAI Enemy { get; init; }

    /// <summary>
    ///     Gets or sets the damage dealt to the player.
    /// </summary>
    /// <remarks>
    ///     Note: Some enemies are one-shot and will always kill the player. In this case the damage will be ignored and will be -1 by default.
    /// </remarks>
    public int Damage { get; set; }

    /// <inheritdoc />
    public bool IsAllowed { get; set; }
}