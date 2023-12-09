// -----------------------------------------------------------------------
// <copyright file="DeniableEnemyDetectingNoiseEventArgs.cs" company="LethalAPI Event Team">
// Copyright (c) LethalAPI Event Team. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Events.EventArgs.Enemies;

/// <summary>
///     Represents the event args that are called before an enemy detects a player's noise.
/// </summary>
/// <param name="isAllowed">
///     Indicates whether the event is allowed to execute.
/// </param>
/// <param name="enemy">
///     The enemy detecting noise.
/// </param>
public sealed class DeniableEnemyDetectingNoiseEventArgs(EnemyAI enemy, bool isAllowed) : IDeniableEvent, IEnemyEvent
{
    /// <inheritdoc />
    public bool IsAllowed { get; set; } = isAllowed;

    /// <inheritdoc />
    bool IDeniableEvent.HardDenied { get; set; } = false;

    /// <inheritdoc />
    public EnemyAI Enemy { get; init; } = enemy;
}