// -----------------------------------------------------------------------
// <copyright file="Enemies.cs" company="Redforce04">
// Copyright (c) Redforce04. All rights reserved.
// Licensed under the GPL-3.0 license.
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
    ///     Gets or sets the event that is called before an enemy attacks a player.
    /// </summary>
    public static Event<EnemyAttackingPlayerEventArgs> EnemyAttackingPlayer { get; set; } = new();

    /// <summary>
    ///     Gets or sets the event that is called before an enemy kills a player.
    /// </summary>
    public static Event<EnemyKillingPlayerEventArgs> EnemyKillingPlayer { get; set; } = new();

    /// <summary>
    ///     Gets or sets the event that is called before a player hits an enemy.
    /// </summary>
    public static Event<HittingEnemyEventArgs> HittingEnemy { get; set; } = new();

    /// <summary>
    ///     Gets or sets the event that is called before a player kills an enemy.
    /// </summary>
    public static Event<KillingEnemyEventArgs> KillingEnemy { get; set; } = new();

    /// <summary>
    ///     Gets or sets the event that is called before a player stuns an enemy.
    /// </summary>
    public static Event<StunningEnemyEventArgs> StunningEnemy { get; set; } = new();
}