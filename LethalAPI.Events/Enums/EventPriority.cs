// -----------------------------------------------------------------------
// <copyright file="EventPriority.cs" company="LethalAPI Event Team">
// Copyright (c) LethalAPI Event Team. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Events.Enums;

/// <summary>
/// Represents the execution order that events are called in. <see cref="Highest"/> events are called first, and <see cref="Lowest"/> events are called last. <see cref="Default"/> is in the middle.
/// </summary>
public enum EventPriority : ushort
{
    /// <summary>
    /// Indicates that the event should run last. Runs 6th. Runs after <see cref="Low"/> events.
    /// </summary>
    Lowest = 100,

    /// <summary>
    /// Indicates that the event is a low priority event. Runs 5th. Runs after <see cref="Default"/> events.
    /// </summary>
    Low = 300,

    /// <summary>
    /// Indicates that the event is a normal event. Runs 4th. Runs after <see cref="Important"/> events.
    /// </summary>
    Default = 500,

    /// <summary>
    /// Indicates that the event is more imporant than <see cref="Default"/> events. Runs 3rd. Runs after <see cref="High"/> events.
    /// </summary>
    Important = 600,

    /// <summary>
    /// Indicates that the event should run early. Runs 2nd. Runs after <see cref="Highest"/> events.
    /// </summary>
    High = 800,

    /// <summary>
    /// Indicates that the event should run first. Runs 1st. Runs before other events.
    /// </summary>
    Highest = 1000,
}