﻿// -----------------------------------------------------------------------
// <copyright file="LethalEvent.cs" company="LethalAPI Event Team">
// Copyright (c) LethalAPI Event Team. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Events.Attributes;

using System;

/// <summary>
/// Can be used to subscribe to an event via attributes.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class LethalEvent : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LethalEvent"/> class.
    /// </summary>
    /// <remarks>Automatically determines the event based on the event args.</remarks>
    public LethalEvent()
    {
        this.GameEvent = EventType.None;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LethalEvent"/> class.
    /// </summary>
    /// <param name="gameEvent">The type of event to register.</param>
    public LethalEvent(EventType gameEvent)
    {
        this.GameEvent = gameEvent;
    }

    /// <summary>
    /// Gets the <see cref="EventType"/> to register. If <see cref="EventType.None"/> is used, the event will be determined via the event args.
    /// </summary>
    public EventType GameEvent { get; init; }
}