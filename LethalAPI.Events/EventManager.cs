// -----------------------------------------------------------------------
// <copyright file="EventManager.cs" company="LethalAPI Event Team">
// Copyright (c) LethalAPI Event Team. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable RedundantNameQualifier
// ReSharper disable UnusedMember.Local
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
namespace LethalAPI.Events;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using EventArgs.Items;
using EventArgs.Map;
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

    private static readonly ReadOnlyDictionary<EventType, Type> EventTypeReferences = new(new Dictionary<EventType, Type>()
    {
        { EventType.Saving, typeof(EventArgs.Server.SavingEventArgs) },
        { EventType.LoadingSave, typeof(EventArgs.Server.LoadingSaveEventArgs) },
        { EventType.ResetSave, typeof(EventArgs.Server.DeniableResettingSaveEventArgs) },
        { EventType.HittingEnemy, typeof(EventArgs.Enemies.DeniablePlayerHittingEnemyEventArgs) },
        { EventType.KillingEnemy, typeof(EventArgs.Enemies.DeniablePlayerKillingEnemyEventArgs) },
        { EventType.StunningEnemy, typeof(EventArgs.Enemies.DeniableStunningEnemyEventArgs) },
        { EventType.EnemyAttackingPlayer, typeof(EventArgs.Enemies.DeniableEnemyAttackingPlayerEventArgs) },
        { EventType.EnemyKillingPlayer, typeof(EventArgs.Enemies.DeniableEnemyKillingPlayerEventArgs) },
        { EventType.Healing, typeof(EventArgs.Player.DeniableHealingEventArgs) },
        { EventType.CriticallyInjure, typeof(EventArgs.Player.DeniableCriticallyInjureEventArgs) },
        { EventType.UsingItem, typeof(DeniableUsingItemEventArgs) },
        { EventType.UsingKey, typeof(DeniableUnlockingDoorEventArgs) },
    });

    private static List<Type> handlerTypes = new(typeof(EventManager).Assembly.DefinedTypes.Where(x => x.FullName?.StartsWith("LethalAPI.Events.Handlers") ?? false));

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
                continue;

            if(!eventInfo.AutoRegisterViaEventManager)
                continue;

            Type? parameterType = method.GetParameters().Length >= 1 ? method.GetParameters()[0].ParameterType : null;
            if (parameterType is not null && parameterType.GetInterface(nameof(ILethalApiEvent)) is null)
            {
                Log.Error($"Could not patch event {method.Name} in type '{method.DeclaringType?.FullName ?? "unknown"}'. The event type requires the corresponding EventArg. The parameter was unknown. Read the docs for an example.");
                continue;
            }

            if (eventInfo.GameEvent != EventType.None)
            {
                bool isStandalone = IsStandaloneEvent(eventInfo.GameEvent);
                if (!isStandalone && parameterType is null)
                {
                    // Log.Error($"Could not patch event {method.Name} in type '{method.DeclaringType?.FullName ?? "unknown"}'. The event type requires the corresponding EventArg. Read the docs for an example.");
                    // continue;
                    parameterType = EventTypeReferences[eventInfo.GameEvent];
                }

                if(isStandalone)
                {
                    SubscribeMethodToEvent(method, eventInfo.GameEvent, eventInfo, instance);
                }
                else
                {
                    SubscribeMethodToEvent(method, parameterType!, eventInfo, instance);
                }

                continue;
            }

            if (parameterType is null)
            {
                Log.Error($"Could not patch event {method.Name} in type '{method.DeclaringType?.FullName ?? "Unknown"}'. Could not find parameterless event type without the event type being specified.");
                continue;
            }

            SubscribeMethodToEvent(method, parameterType, eventInfo, instance);
        }
    }

    private static void FindMethodsToUnregister(Type type, object? instance = null)
    {
        foreach (MethodInfo method in type.GetMethods((instance is null ? BindingFlags.Static : BindingFlags.Instance) | BindingFlags.Public | BindingFlags.NonPublic))
        {
            RegisteredEvent? ev = RegisteredEvent.Get(method, instance);
            if (ev is null)
                continue;
            ev.UnregisterEvent();
        }
    }

    private static bool IsStandaloneEvent(EventType ev) => EventReferences.ContainsKey(ev);

    private static void SubscribeMethodToEvent(MethodInfo method, EventType type, LethalEvent eventInfo, object? instance = null)
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

        new RegisteredEvent(method, EventReferences[type], eventInfo, new CustomEventHandler(action), instance).RegisterEvent();
    }

    private static void SubscribeMethodToEvent(MethodInfo method, Type argsType, LethalEvent eventInfo, object? instance = null) => Method(typeof(EventManager), nameof(SubscribeMethodToEventT)).MakeGenericMethod(argsType).Invoke(null, new[] { method, eventInfo, instance });

    private static void SubscribeMethodToEventT<T>(MethodInfo method, LethalEvent eventInfo, object? instance = null)
        where T : LethalAPI.Events.Interfaces.ILethalApiEvent
    {
        Event<T>? eventInstance = null;
        foreach(Type type in handlerTypes)
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
            Log.Warn($"Could not find the event instance of '{typeof(T).Name}'.");
            return;
        }

        if (method.GetParameters().Length == 0)
        {
            Action genericAction;
            if (instance is null)
                genericAction = (Action)Delegate.CreateDelegate(typeof(Action), method);
            else
                genericAction = (Action)Delegate.CreateDelegate(typeof(Action), target: instance, method.Name);
            new RegisteredEvent<T>(method, eventInstance, eventInfo, new CustomEventHandler(genericAction), instance).RegisterEvent();
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

        new RegisteredEvent<T>(method, eventInstance, eventInfo, new CustomEventHandler<T>(action), instance).RegisterEvent();
    }

    /// <summary>
    /// Used to keep track of registered events for unregistering.
    /// </summary>
    private class RegisteredEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegisteredEvent"/> class.
        /// </summary>
        /// <param name="method">The method that is registered.</param>
        /// <param name="event">The event that is registered.</param>
        /// <param name="info">The Event Info used for custom ordering and execution parameters.</param>
        /// <param name="handler">The event handler for the event.</param>
        /// <param name="instance">The instance of the object to compare.</param>
        internal RegisteredEvent(MethodInfo method, Event @event, LethalEvent info, CustomEventHandler handler, object? instance = null)
        {
            this.Method = method;
            this.Event = @event;
            this.Info = info;
            this.Handler = handler;
            this.Instance = instance;

            if (RegisteredEvents.Any(x => x.Equals(this)))
                return;

            RegisteredEvents.Add(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisteredEvent"/> class.
        /// </summary>
        /// <param name="method">The method that is registered.</param>
        /// <param name="info">The Event Info used for custom ordering and execution parameters.</param>
        /// <param name="instance">The instance of the object to compare.</param>
        protected RegisteredEvent(MethodInfo method, LethalEvent info, object? instance = null)
        {
            this.Method = method;
            this.Info = info;
            this.Instance = instance;
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not the event is registered.
        /// </summary>
        internal bool IsRegistered { get; set; }

        /// <summary>
        /// Gets a list of the registered events.
        /// </summary>
        protected static List<RegisteredEvent> RegisteredEvents { get; } = new();

        /// <summary>
        /// Gets the target method which was registered.
        /// </summary>
        protected MethodInfo Method { get; init; }

        /// <summary>
        /// Gets the instance of the object to compare.
        /// </summary>
        protected object? Instance { get; init; }

        /// <summary>
        /// Gets the event information for the event.
        /// </summary>
        protected LethalEvent Info { get; init; }

        /// <summary>
        /// Gets the handler method which will be executed..
        /// </summary>
        private CustomEventHandler Handler { get; init; } = null!;

        /// <summary>
        /// Gets the event that was registered.
        /// </summary>
        private Event Event { get; init; } = null!;

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            if (obj is not RegisteredEvent ev)
                return false;

            if (ev.Event != this.Event)
                return false;

            if (ev.Method != this.Method)
                return false;

            return ev.Instance == this.Instance;
        }

        /// <summary>
        /// Gets a registered event from a method and it's instance.
        /// </summary>
        /// <param name="method">The method to get the event from.</param>
        /// <param name="instance">The instance of the object to get the event from.</param>
        /// <returns>The registered event instance if found. Null if not found.</returns>
        internal static RegisteredEvent? Get(MethodInfo method, object? instance = null)
        {
            return RegisteredEvents.FirstOrDefault(x => x.Method == method && x.Instance == instance);
        }

        /// <summary>
        /// Register's the event.
        /// </summary>
        internal virtual void RegisterEvent()
        {
            if (RegisteredEvents.Any(x => x.Equals(this) && x.IsRegistered))
                return;

            this.IsRegistered = true;
            RegisteredEvents.Add(this);
            this.Event.Subscribe(new GenericHandlerInformation(this.Handler, this.Info.Priority, this.Info.AutoRegisterViaEventManager));
        }

        /// <summary>
        /// Unregisters the event.
        /// </summary>
        internal virtual void UnregisterEvent()
        {
            this.Event.Unsubscribe(this.Handler);
            RegisteredEvents.Remove(this);
            this.IsRegistered = false;
        }
    }

    /// <summary>
    /// Used to keep track of registered events for unregistering.
    /// </summary>
    /// <typeparam name="T">The type of the event.</typeparam>
    private class RegisteredEvent<T> : RegisteredEvent
        where T : ILethalApiEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegisteredEvent{T}"/> class.
        /// </summary>
        /// <param name="event">The event that is registered.</param>
        /// <param name="method">The method that is registered.</param>
        /// <param name="info">The Event Info used for custom ordering and execution parameters.</param>
        /// <param name="handler">The event handler for the instance.</param>
        /// <param name="instance">The instance of the object to compare.</param>
        internal RegisteredEvent(MethodInfo method, Event<T> @event, LethalEvent info, CustomEventHandler handler, object? instance = null)
            : base(method, info, instance)
        {
            this.Event = @event;
            this.GenericHandler = handler;

            if (RegisteredEvents.Any(x => x.Equals(this)))
                return;

            RegisteredEvents.Add(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisteredEvent{T}"/> class.
        /// </summary>
        /// <param name="event">The event that is registered.</param>
        /// <param name="method">The method that is registered.</param>
        /// <param name="info">The Event Info used for custom ordering and execution parameters.</param>
        /// <param name="handler">The event handler for the instance.</param>
        /// <param name="instance">The instance of the object to compare.</param>
        internal RegisteredEvent(MethodInfo method, Event<T> @event, LethalEvent info, CustomEventHandler<T> handler, object? instance = null)
            : base(method, info, instance)
        {
            this.Event = @event;
            this.Handler = handler;

            if (RegisteredEvents.Any(x => x.Equals(this)))
                return;

            RegisteredEvents.Add(this);
        }

        /// <summary>
        /// Gets the generic handler for the event.
        /// </summary>
        /// <remarks>May be null if <see cref="Handler"/> is being used instead.</remarks>
        private CustomEventHandler? GenericHandler { get; init; }

        /// <summary>
        /// Gets the type specific handler for the event.
        /// </summary>
        /// <remarks>May be null if <see cref="GenericHandler"/> is being used instead.</remarks>
        private CustomEventHandler<T>? Handler { get; init; }

        /// <summary>
        /// Gets the event that was registered.
        /// </summary>
        private Event<T> Event { get; init; }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            if (obj is not RegisteredEvent<T> ev)
                return false;

            if (ev.Event != this.Event)
                return false;

            if (ev.Method != this.Method)
                return false;

            return ev.Instance == this.Instance;
        }

        /// <inheritdoc />
        internal override void RegisterEvent()
        {
            if (RegisteredEvents.Any(x => x.Equals(this) && x.IsRegistered))
                return;

            this.IsRegistered = true;

            if(this.GenericHandler is null)
                this.Event.Subscribe(new TypeHandlerInformation<T>(this.Handler!, this.Info.Priority, this.Info.RunIfDisabled, this.Info.AutoRegisterViaEventManager));
            else
                this.Event.Subscribe(new GenericHandlerInformation(this.GenericHandler!, this.Info.Priority, this.Info.AutoRegisterViaEventManager));
        }

        /// <inheritdoc />
        internal override void UnregisterEvent()
        {
            if(this.GenericHandler is null)
                this.Event.Unsubscribe(this.Handler!);
            else
                this.Event.Unsubscribe(this.GenericHandler!);

            RegisteredEvents.Remove(this);
            this.IsRegistered = false;
        }
    }
}
