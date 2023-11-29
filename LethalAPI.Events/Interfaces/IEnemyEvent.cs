// -----------------------------------------------------------------------
// <copyright file="IEnemyEvent.cs" company="LethalAPI Event Team">
// Copyright (c) LethalAPI Event Team. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Events.Interfaces;

/// <summary>
///     Event args for events related to enemies.
/// </summary>
public interface IEnemyEvent : ILethalApiEvent
{
    /// <summary>
    /// Gets the enemy.
    /// </summary>
    public EnemyAI Enemy { get; init; }
}