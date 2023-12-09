// -----------------------------------------------------------------------
// <copyright file="EnemyAttackingPlayerEventArgs.cs" company="LethalAPI Event Team">
// Copyright (c) LethalAPI Event Team. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Events.EventArgs.Enemies;

using GameNetcodeStuff;
using LethalAPI.Events.Interfaces;

/// <summary>
///     Represents the event args that are called before a player is attacked by an enemy.
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
///     Indicates whether the event is allowed to execute.
/// </param>
public sealed class DeniableEnemyAttackingPlayerEventArgs(int damage, PlayerControllerB player, EnemyAI enemy, bool isAllowed = true) : IEnemyEvent, IPlayerEvent, IDeniableEvent
{
    /// <summary>
    /// Gets the player that is being attacked.
    /// </summary>
    public PlayerControllerB Player { get; init; } = player;

    /// <summary>
    /// Gets the enemy that is attacking the player.
    /// </summary>
    public EnemyAI Enemy { get; init; } = enemy;

    /// <summary>
    ///     Gets or sets the damage dealt to the player.
    /// </summary>
    /// <remarks>
    ///     Note: Some enemies are one-shot and will always kill the player. In this case the damage will be ignored and will be -1 by default.
    /// </remarks>
    public int Damage { get; set; } = damage;

    /// <inheritdoc />
    public bool IsAllowed { get; set; } = isAllowed;

    /// <inheritdoc />
    bool IDeniableEvent.HardDenied { get; set; } = false;
}