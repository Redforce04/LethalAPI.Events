// -----------------------------------------------------------------------
// <copyright file="Event{T}.cs" company="LethalAPI Event Team">
// Copyright (c) LethalAPI Event Team. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// Taken from EXILED (https://github.com/Exiled-Team/EXILED)
// Licensed under the CC BY SA 3 license. View it here:
// https://github.com/Exiled-Team/EXILED/blob/master/LICENSE.md
// Changes: Namespace adjustments.
// -----------------------------------------------------------------------

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable InconsistentNaming
// ReSharper disable RedundantNameQualifier
namespace LethalAPI.Events.Features;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Attributes;
using LethalAPI.Events;
using LethalAPI.Events.Interfaces;

/// <summary>
/// An implementation of the <see cref="ILethalApiEvent"/> interface that encapsulates an event with arguments.
/// </summary>
/// <typeparam name="T">The specified <see cref="EventArgs"/> that the event will use.</typeparam>
public class Event<T> : ILethalApiEvent
{
    /// <summary>
    /// A list of the registered events.
    /// </summary>
    private static readonly Dictionary<Type, Event<T>> TypeToEvent = new();

    /// <summary>
    /// Where we keep track of custom details for execution.
    /// </summary>
    private readonly Dictionary<CustomEventHandler<T>, TypeHandlerInformation<T>> handlers = new();

    /// <summary>
    /// Where we keep track of custom details for execution.
    /// </summary>
    private readonly Dictionary<CustomEventHandler, GenericHandlerInformation> genericHandlers = new();

    /// <summary>
    /// Indicates whether the event has been patched or not. We can utilize dynamic patching to only patch the events that we need.
    /// </summary>
    private bool patched;

    /// <summary>
    /// Initializes a new instance of the <see cref="Event{T}"/> class.
    /// </summary>
    public Event()
    {
        TypeToEvent.Add(typeof(T), this);
    }

    /// <summary>
    /// Gets a <see cref="IReadOnlyCollection{T}"/> of <see cref="Event{T}"/> which contains all the <see cref="Event{T}"/> instances.
    /// </summary>
    public static IReadOnlyDictionary<Type, Event<T>> Dictionary => TypeToEvent;

    /// <summary>
    /// Subscribes a target <see cref="CustomEventHandler{TEventArgs}"/> to the inner event and checks if patching is possible, if dynamic patching is enabled.
    /// </summary>
    /// <param name="event">The <see cref="Event{T}"/> the <see cref="CustomEventHandler{T}"/> will be subscribed to.</param>
    /// <param name="handler">The <see cref="CustomEventHandler{T}"/> that will be subscribed to the <see cref="Event{T}"/>.</param>
    /// <returns>The <see cref="Event{T}"/> with the handler subscribed to it.</returns>
    public static Event<T> operator +(Event<T> @event, CustomEventHandler<T> handler)
    {
        Log.Debug($"Event {typeof(T).Name} Subscribed to by {handler.Method.Name}", Plugin.Instance.Config.LogEventPatching, "LethalAPI-Events");
        @event.Subscribe(handler);
        return @event;
    }

    /// <summary>
    /// Subscribes a target <see cref="CustomEventHandler{TEventArgs}"/> to the inner event and checks if patching is possible, if dynamic patching is enabled.
    /// </summary>
    /// <param name="event">The <see cref="Event{T}"/> the <see cref="CustomEventHandler{T}"/> will be subscribed to.</param>
    /// <param name="handler">The <see cref="CustomEventHandler{T}"/> that will be subscribed to the <see cref="Event{T}"/>.</param>
    /// <returns>The <see cref="Event{T}"/> with the handler subscribed to it.</returns>
    public static Event<T> operator +(Event<T> @event, TypeHandlerInformation<T> handler)
    {
        Log.Debug($"Event {typeof(T).Name} Subscribed to by {handler.Handler.Method.Name}", Plugin.Instance.Config.LogEventPatching, "LethalAPI-Events");
        @event.Subscribe(handler);
        return @event;
    }

    /// <summary>
    /// Subscribes a target <see cref="CustomEventHandler"/> to the inner event and checks if patching is possible, if dynamic patching is enabled.
    /// </summary>
    /// <param name="event">The <see cref="Event{T}"/> the <see cref="CustomEventHandler{T}"/> will be subscribed to.</param>
    /// <param name="handler">The <see cref="CustomEventHandler{T}"/> that will be subscribed to the <see cref="Event{T}"/>.</param>
    /// <returns>The <see cref="Event{T}"/> with the handler subscribed to it.</returns>
    public static Event<T> operator +(Event<T> @event, CustomEventHandler handler)
    {
        Log.Debug($"Event {typeof(T).Name} (Generic) Subscribed to by {handler.Method.Name}", Plugin.Instance.Config.LogEventPatching, "LethalAPI-Events");
        @event.Subscribe(handler);
        return @event;
    }

    /// <summary>
    /// Subscribes a target <see cref="CustomEventHandler"/> to the inner event and checks if patching is possible, if dynamic patching is enabled.
    /// </summary>
    /// <param name="event">The <see cref="Event{T}"/> the <see cref="CustomEventHandler{T}"/> will be subscribed to.</param>
    /// <param name="handler">The <see cref="CustomEventHandler{T}"/> that will be subscribed to the <see cref="Event{T}"/>.</param>
    /// <returns>The <see cref="Event{T}"/> with the handler subscribed to it.</returns>
    public static Event<T> operator +(Event<T> @event, GenericHandlerInformation handler)
    {
        Log.Debug($"Event {typeof(T).Name} (Generic) Subscribed to by {handler.Handler.Method.Name}", Plugin.Instance.Config.LogEventPatching, "LethalAPI-Events");
        @event.Subscribe(handler);
        return @event;
    }

    /// <summary>
    /// Unsubscribes a target <see cref="CustomEventHandler{TEventArgs}"/> from the inner event and checks if un-patching is possible, if dynamic patching is enabled.
    /// </summary>
    /// <param name="event">The <see cref="Event{T}"/> the <see cref="CustomEventHandler{T}"/> will be unsubscribed from.</param>
    /// <param name="handler">The <see cref="CustomEventHandler{T}"/> that will be unsubscribed from the <see cref="Event{T}"/>.</param>
    /// <returns>The <see cref="Event{T}"/> with the handler unsubscribed from it.</returns>
    public static Event<T> operator -(Event<T> @event, CustomEventHandler<T> handler)
    {
        @event.Unsubscribe(handler);
        return @event;
    }

    /// <summary>
    /// Unsubscribes a target <see cref="CustomEventHandler"/> from the inner event and checks if un-patching is possible, if dynamic patching is enabled.
    /// </summary>
    /// <param name="event">The <see cref="Event{T}"/> the <see cref="CustomEventHandler"/> will be unsubscribed from.</param>
    /// <param name="handler">The <see cref="CustomEventHandler"/> that will be unsubscribed from the <see cref="Event{T}"/>.</param>
    /// <returns>The <see cref="Event{T}"/> with the handler unsubscribed from it.</returns>
    public static Event<T> operator -(Event<T> @event, CustomEventHandler handler)
    {
        @event.Unsubscribe(handler);
        return @event;
    }

    /// <summary>
    /// Subscribes a target <see cref="CustomEventHandler{T}"/> to the inner event if the conditional is true.
    /// </summary>
    /// <param name="handler">The handler to add.</param>
    public void Subscribe(TypeHandlerInformation<T> handler)
    {
        if (Plugin.Instance.Config.UseDynamicPatching && !this.patched)
        {
            Plugin.Instance.Patcher.Patch(this);
            this.patched = true;
        }

        handlers.Add(handler.Handler, handler);
    }

    /// <summary>
    /// Subscribes a target <see cref="CustomEventHandler{T}"/> to the inner event if the conditional is true.
    /// </summary>
    /// <param name="handler">The handler to add.</param>
    public void Subscribe(GenericHandlerInformation handler)
    {
        if (Plugin.Instance.Config.UseDynamicPatching && !this.patched)
        {
            Plugin.Instance.Patcher.Patch(this);
            this.patched = true;
        }

        genericHandlers.Add(handler.Handler, handler);
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

        if(handler.Method.GetCustomAttribute<LethalEvent>() is { } ev)
            this.genericHandlers.Add(handler, new GenericHandlerInformation(handler, ev.Priority, ev.AutoRegisterViaEventManager));
        else
            this.genericHandlers.Add(handler, new GenericHandlerInformation(handler));
    }

    /// <summary>
    /// Subscribes a target <see cref="CustomEventHandler"/> to the inner event if the conditional is true.
    /// </summary>
    /// <param name="handler">The handler to add.</param>
    public void Subscribe(CustomEventHandler<T> handler)
    {
        if (Plugin.Instance.Config.UseDynamicPatching && !this.patched)
        {
            Plugin.Instance.Patcher.Patch(this);
            this.patched = true;
        }

        if(handler.Method.GetCustomAttribute<LethalEvent>() is { } ev)
            this.handlers.Add(handler, new TypeHandlerInformation<T>(handler, ev.Priority, ev.AutoRegisterViaEventManager, ev.RunIfDisabled));
        else
            this.handlers.Add(handler, new TypeHandlerInformation<T>(handler));
    }

    /// <summary>
    /// Unsubscribes a target <see cref="CustomEventHandler{T}"/> from the inner event if the conditional is true.
    /// </summary>
    /// <param name="handler">The handler to add.</param>
    public void Unsubscribe(CustomEventHandler<T> handler)
    {
        this.handlers.Remove(handler);
    }

    /// <summary>
    /// Unsubscribes a target <see cref="CustomEventHandler"/> from the inner event if the conditional is true.
    /// </summary>
    /// <param name="handler">The handler to add.</param>
    public void Unsubscribe(CustomEventHandler handler)
    {
        this.genericHandlers.Remove(handler);
    }

    /// <summary>
    /// Executes all <see cref="CustomEventHandler{TEventArgs}"/> listeners safely.
    /// </summary>
    /// <param name="arg">The event argument.</param>
    /// <exception cref="ArgumentNullException">Event or its arg is <see langword="null"/>.</exception>
    public void InvokeSafely(T arg)
    {
        Log.Debug($"Event {typeof(T).Name} Invoked", Plugin.Instance.Config.LogEventExecution, "LethalAPI-Events");
        bool isArgDeniable = arg is IDeniableEvent;

        foreach (TypeHandlerInformation<T> handler in this.handlers.Values.OrderByDescending(x => x.EventPriority))
        {
            try
            {
                if (isArgDeniable && !handler.ExecuteIfDenied && !(arg as IDeniableEvent)!.IsAllowed)
                    continue;

                handler.Handler(arg);

                if (isArgDeniable && (arg as IDeniableEvent)!.HardDenied)
                {
                    Log.Debug($"Method \"{handler.Handler.Method.Name}\" of the class \"{handler.Handler.Method.ReflectedType?.FullName}\" has hard denied the event {typeof(T).Name}.");
                    return;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Method \"{handler.Handler.Method.Name}\" of the class \"{handler.Handler.Method.ReflectedType?.FullName}\" caused an exception when handling the event \"{this.GetType().FullName}\"");
                Log.Exception(ex);
            }
        }

        if (isArgDeniable && !(arg as IDeniableEvent)!.IsAllowed)
        {
            Log.Debug($"Event has been denied.");
            return;
        }

        foreach (GenericHandlerInformation handler in this.genericHandlers.Values.OrderByDescending(x => x.EventPriority))
        {
            try
            {
                handler.Handler();
            }
            catch (Exception ex)
            {
                Log.Error($"Method \"{handler.Handler.Method.Name}\" of the class \"{handler.Handler.Method.ReflectedType?.FullName}\" caused an exception when handling the generic event \"{this.GetType().FullName}\"");
                Log.Exception(ex);
            }
        }
    }
}