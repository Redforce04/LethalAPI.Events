// -----------------------------------------------------------------------
// <copyright file="EventType.cs" company="LethalAPI Event Team">
// Copyright (c) LethalAPI Event Team. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Events;

using EventArgs.Server;
using Handlers;

/// <summary>
/// A list of all events.
/// </summary>
public enum EventType
{
    /// <summary>
    /// Auto-determines the event to use.
    /// </summary>
    None = 0,

    /// <inheritdoc cref="Handlers.Player.CriticallyInjure"/>
    CriticallyInjure,

    /// <inheritdoc cref="Handlers.Player.Healing"/>
    Healing,

    /// <inheritdoc cref="Items.DeniableUsingItem"/>
    UsingItem,

    /// <inheritdoc cref="Handlers.Player.UsingKey"/>
    UsingKey,

    /// <inheritdoc cref="Handlers.Server.LoadingSave"/>
    LoadingSave,

    /// <inheritdoc cref="DeniableResettingSaveEventArgs"/>
    // <inheritdoc cref="Handlers.Server.ResetSave"/>
    ResetSave,

    /// <inheritdoc cref="Handlers.Server.Saving"/>
    Saving,

    /// <inheritdoc cref="Handlers.Server.GameOpened"/>
    GameOpened,

    /// <inheritdoc cref="Enemies.DeniablePlayerHittingEnemy"/>
    HittingEnemy,

    /// <inheritdoc cref="Enemies.DeniablePlayerKillingEnemy"/>
    KillingEnemy,

    /// <inheritdoc cref="Enemies.DeniableStunningEnemy"/>
    StunningEnemy,

    /// <inheritdoc cref="Enemies.DeniableEnemyAttackingPlayer"/>
    EnemyAttackingPlayer,

    /// <inheritdoc cref="Enemies.DeniableEnemyKillingPlayer"/>
    EnemyKillingPlayer,
}
