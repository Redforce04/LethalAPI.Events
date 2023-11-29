// -----------------------------------------------------------------------
// <copyright file="EventManager.cs" company="Redforce04">
// Copyright (c) Redforce04. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable RedundantNameQualifier
// ReSharper disable UnusedMember.Local
namespace LethalAPI.Events;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

using LethalAPI.Events.Attributes;
using LethalAPI.Events.Features;
using LethalAPI.Events.Interfaces;

using static HarmonyLib.AccessTools;

/// <summary>
/// Manages attribute based event registering and unregistering.
/// </summary>
public static class EventManager
{
    private static readonly ReadOnlyDictionary<EventType, Event> EventReferences = new(new Dictionary<EventType, Event>()
    {
        { EventType.GameOpened, Handlers.Server.GameOpened },
    });

    private static Dictionary<Type, List<RegisteredEvent>> registeredEvents = new();

    /// <summary>
    /// Registers events for an object instance.
    /// </summary>
    /// <param name="registeringInstance">The instance being registered.</param>
    public static void RegisterEvents(object registeringInstance) => FindMethodsToRegister(registeringInstance.GetType(), registeringInstance);

    /// <summary>
    /// Unregisters events for an object instance.
    /// </summary>
    /// <param name="unregisteringInstance">The instance being unregistered.</param>
    public static void UnregisterEvents(object unregisteringInstance) => FindMethodsToUnregister(unregisteringInstance.GetType(), unregisteringInstance);

    /// <summary>
    /// Registers static events for a class.
    /// </summary>
    /// <typeparam name="T">The type to register static events for.</typeparam>
    public static void RegisterStaticEvents<T>()
        where T : class => FindMethodsToRegister(typeof(T));

    /// <summary>
    /// Registers static events for a class.
    /// </summary>
    /// <param name="type">The type to register static events for.</param>
    public static void RegisterStaticEvents(Type type)
        => FindMethodsToRegister(type);

    /// <summary>
    /// Unregisters static events for a class.
    /// </summary>
    /// <typeparam name="T">The type to unregister static events for.</typeparam>
    public static void UnregisterStaticEvents<T>()
        where T : class => FindMethodsToUnregister(typeof(T));

    /// <summary>
    /// Unregisters static events for a class.
    /// </summary>
    /// <param name="type">The type to unregister static events for.</param>
    public static void UnregisterStaticEvents(Type type)
        => FindMethodsToUnregister(type);

    private static void FindMethodsToRegister(Type type, object? instance = null)
    {
        foreach (MethodInfo method in type.GetMethods((instance is null ? BindingFlags.Static : BindingFlags.Instance) | BindingFlags.Public | BindingFlags.NonPublic))
        {
            LethalEvent? eventInfo = method.GetCustomAttribute<LethalEvent>();
            if (eventInfo is null)
            {
                continue;
            }

            Type? parameterType = method.GetParameters().Length >= 1 ? method.GetParameters()[0].ParameterType : null;
            if (parameterType?.GetInterface(nameof(ILethalApiEvent)) is not null)
            {
                Log.Error($"Could not patch event {method.Name} in type '{method.DeclaringType?.FullName ?? "unknown"}'. The event type requires the corresponding EventArg. The parameter was unknown. Read the docs for an example.");
                continue;
            }

            if (eventInfo.GameEvent != EventType.None)
            {
                bool isStandalone = IsStandaloneEvent(eventInfo.GameEvent);
                if (!isStandalone && parameterType is null)
                {
                    Log.Error($"Could not patch event {method.Name} in type '{method.DeclaringType?.FullName ?? "unknown"}'. The event type requires the corresponding EventArg. Read the docs for an example.");
                    continue;
                }

                if(isStandalone)
                {
                    SubscribeMethodToEvent(method, eventInfo.GameEvent, instance);
                }
                else
                {
                    SubscribeMethodToEvent(method, parameterType!, instance);
                }

                continue;
            }

            Log.Error($"Could not patch event {method.Name} in type '{method.DeclaringType?.FullName ?? "unknown"}'. The event type was not found.");
        }
    }

    private static void FindMethodsToUnregister(Type type, object? instance = null)
    {
        foreach (MethodInfo method in type.GetMethods((instance is null ? BindingFlags.Static : BindingFlags.Instance) | BindingFlags.Public | BindingFlags.NonPublic))
        {
            LethalEvent? eventInfo = method.GetCustomAttribute<LethalEvent>();
            if (eventInfo is null)
            {
                continue;
            }

            Type? parameterType = method.GetParameters().Length >= 1 ? method.GetParameters()[0].ParameterType : null;
            if (parameterType?.GetInterface(nameof(ILethalApiEvent)) is not null)
            {
                continue;
            }

            if (eventInfo.GameEvent != EventType.None)
            {
                bool isStandalone = IsStandaloneEvent(eventInfo.GameEvent);
                if (!isStandalone && parameterType is null)
                {
                    continue;
                }

                if(isStandalone)
                {
                    SubscribeMethodToEvent(method, eventInfo.GameEvent, instance);
                }
                else
                {
                    SubscribeMethodToEvent(method, parameterType!, instance);
                }

                continue;
            }

            Log.Error($"Could not patch event {method.Name} in type '{method.DeclaringType?.FullName ?? "unknown"}'. The event type was not found.");
        }
    }

    private static bool IsStandaloneEvent(EventType ev) => EventReferences.ContainsKey(ev);

    private static void SubscribeMethodToEvent(MethodInfo method, EventType type, object? instance = null)
    {
        if (!EventReferences.ContainsKey(type))
        {
            return;
        }

        Action action;
        if (instance is null)
        {
            action = (Action)Delegate.CreateDelegate(typeof(Action), method);
        }
        else
        {
            action = (Action)Delegate.CreateDelegate(typeof(Action), target: instance, method.Name);
        }

        EventReferences[type].Subscribe(new CustomEventHandler(action));
    }

    private static void SubscribeMethodToEvent(MethodInfo method, Type argsType, object? instance = null) => Method(typeof(EventManager), nameof(SubscribeMethodToEventT)).MakeGenericMethod(typeof(Event<>).MakeGenericType(argsType)).Invoke(null, new[] { method, instance });

    private static void SubscribeMethodToEventT<T>(MethodInfo method, object? instance = null)
        where T : LethalAPI.Events.Interfaces.ILethalApiEvent
    {
        Event<T>? eventInstance = null;
        foreach(TypeInfo type in typeof(EventManager).Assembly.DefinedTypes)
        {
            PropertyInfo? property = type.GetProperties(BindingFlags.Static | BindingFlags.Public).FirstOrDefault(x => x.PropertyType == typeof(Event<T>));
            if (property is null)
            {
                continue;
            }

            eventInstance = (Event<T>)property.GetValue(null);
            break;
        }

        if (eventInstance is null)
        {
            Log.Warn($"Could not find the event instance of '{typeof(T).FullName}'.");
            return;
        }

        Action<T> action;
        if (instance is null)
        {
            action = (Action<T>)Delegate.CreateDelegate(typeof(Action<T>), method);
        }
        else
        {
            action = (Action<T>)Delegate.CreateDelegate(typeof(Action<T>), target: instance, method.Name);
        }

        eventInstance.Subscribe(new CustomEventHandler<T>(action));
    }

    /// <summary>
    /// Used to keep track of registered events for unregistering.
    /// </summary>
    internal class RegisteredEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegisteredEvent"/> class.
        /// </summary>
        /// <param name="method">The method that is registered.</param>
        /// <param name="eventHandler">The event handler that is registered.</param>
        /// <param name="event">The event that is registered.</param>
        /// <param name="instance">The instance of the object to compare.</param>
        public RegisteredEvent(MethodInfo method, CustomEventHandler eventHandler, Event @event, object? instance = null)
        {
            this.Method = method;
            this.EventHandler = eventHandler;
            this.Event = @event;
            this.Instance = instance;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisteredEvent"/> class.
        /// </summary>
        /// <param name="method">The method that is registered.</param>
        /// <param name="instance">The instance of the object to compare.</param>
        protected RegisteredEvent(MethodInfo method, object? instance = null)
        {
            this.Method = method;
            this.Instance = instance;
        }

        /// <summary>
        /// Gets the target method which was registered.
        /// </summary>
        public MethodInfo Method { get; init; }

        /// <summary>
        /// Gets the instance of the object to compare.
        /// </summary>
        public object? Instance { get; init; }

        /// <summary>
        /// Gets the event handler that was registered.
        /// </summary>
        public CustomEventHandler? EventHandler { get; init; }

        /// <summary>
        /// Gets the event that was registered.
        /// </summary>
        public Event? Event { get; init; }
    }

    /// <summary>
    /// Used to keep track of registered events for unregistering.
    /// </summary>
    /// <typeparam name="T">The type of the event.</typeparam>
    internal class RegisteredEvent<T> : RegisteredEvent
        where T : ILethalApiEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegisteredEvent{T}"/> class.
        /// </summary>
        /// <param name="eventHandler">The event handler that is registered.</param>
        /// <param name="event">The event that is registered.</param>
        /// <param name="method">The method that is registered.</param>
        /// <param name="instance">The instance of the object to compare.</param>
        public RegisteredEvent(MethodInfo method, CustomEventHandler<T> eventHandler, Event<T> @event, object? instance = null)
            : base(method, instance)
        {
            this.EventHandler = eventHandler;
            this.Event = @event;
        }

        /// <summary>
        /// Gets the event handler that was registered.
        /// </summary>
        public new CustomEventHandler<T> EventHandler { get; init; }

        /// <summary>
        /// Gets the event that was registered.
        /// </summary>
        public new Event<T> Event { get; init; }
    }
}
