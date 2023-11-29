// -----------------------------------------------------------------------
// <copyright file="Event.cs" company="LethalAPI Event Team">
// Copyright (c) LethalAPI Event Team. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// Taken from EXILED (https://github.com/Exiled-Team/EXILED)
// Licensed under the CC BY SA 3 license. View it here:
// https://github.com/Exiled-Team/EXILED/blob/master/LICENSE.md
// Changes: Namespace adjustments.
// -----------------------------------------------------------------------

// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable RedundantNameQualifier
#pragma warning disable CS0169 // Field is never used

namespace LethalAPI.Events.Features;

using System;
using System.Collections.Generic;
using System.Linq;

using LethalAPI.Events;
using LethalAPI.Events.Interfaces;

/// <summary>
/// The custom <see cref="EventHandler"/> delegate, with empty parameters.
/// </summary>
public delegate void CustomEventHandler();

/// <summary>
/// An implementation of <see cref="ILethalApiEvent"/> that encapsulates a no-argument event.
/// </summary>
public class Event : ILethalApiEvent
{
    private static readonly List<Event> EventsValue = new();

    /// <summary>
    /// The name of the event.
    /// </summary>
    private readonly string name;

    /// <summary>
    /// Indicates whether the event has been patched or not. We can utilize dynamic patching to only patch the events that we need.
    /// </summary>
    private bool patched;
#pragma warning restore CS0169 // Field is never used

    /// <summary>
    /// Initializes a new instance of the <see cref="Event"/> class.
    /// </summary>
    /// <param name="name">
    ///     The name of the event. Used for logging.
    /// </param>
    public Event(string name)
    {
        EventsValue.Add(this);
        this.name = name;
    }

    private event CustomEventHandler? InnerEvent;

    /// <summary>
    /// Gets a <see cref="IReadOnlyList{T}"/> of <see cref="Event{T}"/> which contains all the <see cref="Event{T}"/> instances.
    /// </summary>
    public static IReadOnlyList<Event> List => EventsValue;

    /// <summary>
    /// Subscribes a <see cref="CustomEventHandler"/> to the inner event, and checks patches if dynamic patching is enabled.
    /// </summary>
    /// <param name="event">The <see cref="Event"/> to subscribe the <see cref="CustomEventHandler"/> to.</param>
    /// <param name="handler">The <see cref="CustomEventHandler"/> to subscribe to the <see cref="Event"/>.</param>
    /// <returns>The <see cref="Event"/> with the handler added to it.</returns>
    public static Event operator +(Event @event, CustomEventHandler handler)
    {
        Log.Debug($"An unknown event has been subscribed to by {handler.Method.Name}", Plugin.Instance.Config.LogEventPatching, "LethalAPI-Events");
        @event.Subscribe(handler);
        return @event;
    }

    /// <summary>
    /// Unsubscribes a target <see cref="CustomEventHandler"/> from the inner event, and checks if un-patching is possible, if dynamic patching is enabled.
    /// </summary>
    /// <param name="event">The <see cref="Event"/> the <see cref="CustomEventHandler"/> will be unsubscribed from.</param>
    /// <param name="handler">The <see cref="CustomEventHandler"/> that will be unsubscribed from the <see cref="Event"/>.</param>
    /// <returns>The <see cref="Event"/> with the handler unsubscribed from it.</returns>
    public static Event operator -(Event @event, CustomEventHandler handler)
    {
        @event.Unsubscribe(handler);
        return @event;
    }

    /// <summary>
    /// Subscribes a target <see cref="CustomEventHandler"/> to the inner event if the conditional is true.
    /// </summary>
    /// <param name="handler">The handler to add.</param>
    public void Subscribe(CustomEventHandler handler)
    {
        if (Plugin.Instance.Config.UseDynamicPatching && !this.patched)
        {
            Plugin.Instance.Patcher.Patch(this);
            this.patched = true;
        }

        this.InnerEvent += handler;
    }

    /// <summary>
    /// Unsubscribes a target <see cref="CustomEventHandler"/> from the inner event if the conditional is true.
    /// </summary>
    /// <param name="handler">The handler to add.</param>
    public void Unsubscribe(CustomEventHandler handler)
    {
        this.InnerEvent -= handler;
    }

    /// <summary>
    /// Executes all <see cref="CustomEventHandler"/> listeners safely.
    /// </summary>
    public void InvokeSafely()
    {
        Log.Debug($"Event {name} Invoked", Plugin.Instance.Config.LogEventPatching, "LethalAPI-Events");
        if (this.InnerEvent is null)
        {
            return;
        }

        foreach (CustomEventHandler handler in this.InnerEvent.GetInvocationList().Cast<CustomEventHandler>())
        {
            try
            {
                handler();
            }
            catch (Exception ex)
            {
                Log.Error($"Method \"{handler.Method.Name}\" of the class \"{handler.Method.ReflectedType?.FullName}\" caused an exception when handling the event \"{this.GetType().FullName}\"\n{ex}");
            }
        }
    }
}