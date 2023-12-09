// -----------------------------------------------------------------------
// <copyright file="DeniableEnragingEnemyEventArgs.cs" company="LethalAPI Event Team">
// Copyright (c) LethalAPI Event Team. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Events.EventArgs.Enemies;

/// <summary>
///     Represents the event args that are called before an enemy becomes enraged.
/// </summary>
/// <param name="enemy">
///     The enemy that is enraging.
/// </param>
/// <param name="isAllowed">
///     Indicates whether the event is allowed to execute.
/// </param>
public sealed class DeniableEnragingEnemyEventArgs(EnemyAI enemy, bool isAllowed = true) : IDeniableEvent, IEnemyEvent
{
    /// <inheritdoc />
    public bool IsAllowed { get; set; } = isAllowed;

    /// <inheritdoc />
    bool IDeniableEvent.HardDenied { get; set; } = false;

    /// <inheritdoc />
    public EnemyAI Enemy { get; init; } = enemy;
}