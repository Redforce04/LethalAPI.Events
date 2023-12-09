// -----------------------------------------------------------------------
// <copyright file="LethalEvent.cs" company="LethalAPI Event Team">
// Copyright (c) LethalAPI Event Team. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Events.Attributes;

using System;

using Core.Events.Interfaces;
using Enums;

/// <summary>
/// Can be used to subscribe to an event via attributes.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class LethalEvent : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LethalEvent"/> class.
    /// </summary>
    /// <param name="gameEvent">The type of event to register.</param>
    /// <param name="priority">The priority of the event when being run, in comparison to other plugin events.</param>
    /// <param name="runIfDisabled">Indicates whether or not this event should be called if another plugin indicates that the event should not be run.</param>
    /// <param name="autoRegisterViaEventManager">Indicates whether or not the event manager should auto-register and unregister this event when <see cref="EventManager.RegisterEvents"/> is called.</param>
    public LethalEvent(EventType gameEvent = EventType.None, EventPriority priority = EventPriority.Default, bool runIfDisabled = false, bool autoRegisterViaEventManager = true)
    {
        this.GameEvent = gameEvent;
        this.RunIfDisabled = runIfDisabled;
        this.Priority = (int)priority;
        this.AutoRegisterViaEventManager = autoRegisterViaEventManager;
    }

    /// <summary>
    /// Gets the <see cref="EventType"/> to register. If <see cref="EventType.None"/> is used, the event will be determined via the event args.
    /// </summary>
    public EventType GameEvent { get; init; }

    /// <summary>
    /// Gets a value indicating whether or not the event should execute event if it is an <see cref="IDeniableEvent"/> and <see cref="IDeniableEvent.IsAllowed"/> is set to false before triggering the event.
    /// </summary>
    public bool RunIfDisabled { get; init; }

    /// <summary>
    /// Gets the priority of this event in comparison to other events. This allows specifying the run order of events.
    /// </summary>
    public int Priority { get; init; }

    /// <summary>
    /// Gets a value indicating whether or not the event manager should auto-register this event when <see cref="EventManager.RegisterEvents"/> is called.
    /// </summary>
    public bool AutoRegisterViaEventManager { get; init; }
}