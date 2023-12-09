// -----------------------------------------------------------------------
// <copyright file="Enemies.cs" company="LethalAPI Event Team">
// Copyright (c) LethalAPI Event Team. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Events.Handlers;

using EventArgs.Enemies;
using Features;

/// <summary>
/// Contains event handlers for enemy events.
/// </summary>
public static class Enemies
{
    /// <summary>
    ///     Gets the event that is called when an enemy attacks a player.
    /// </summary>
    public static Event<DeniableEnemyAttackingPlayerEventArgs> DeniableEnemyAttackingPlayer { get; } = new();

    /// <summary>
    ///     Gets the event that is called when an enemy detects noise.
    /// </summary>
    public static Event<DeniableEnemyKillingPlayerEventArgs> DeniableEnemyDetectingNoise { get; } = new();

    /// <summary>
    ///     Gets the event that is called when an enemy kills a player.
    /// </summary>
    public static Event<DeniableEnemyKillingPlayerEventArgs> DeniableEnemyKillingPlayer { get; } = new();

    /// <summary>
    ///     Gets the event that is called when an enemy becomes enraged.
    /// </summary>
    public static Event<DeniablePlayerHittingEnemyEventArgs> DeniableEnragingEnemy { get; } = new();

    /// <summary>
    ///     Gets the event that is called when a player hits an enemy.
    /// </summary>
    public static Event<DeniablePlayerHittingEnemyEventArgs> DeniablePlayerHittingEnemy { get; } = new();

    /// <summary>
    ///     Gets the event that is called when a player kills an enemy.
    /// </summary>
    public static Event<DeniablePlayerKillingEnemyEventArgs> DeniablePlayerKillingEnemy { get; } = new();

    /// <summary>
    ///     Gets the event that is called when a player stuns an enemy.
    /// </summary>
    public static Event<DeniableStunningEnemyEventArgs> DeniableStunningEnemy { get; } = new();
}