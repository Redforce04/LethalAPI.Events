// -----------------------------------------------------------------------
// <copyright file="Records.cs" company="LethalAPI Event Team">
// Copyright (c) LethalAPI Event Team. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

#pragma warning disable SA1649 // file name should match type.
#pragma warning disable SA1402 // file may only contain a single type.
namespace LethalAPI.Events.Features;

using System;

/// <summary>
/// The custom <see cref="EventHandler{TEventArgs}"/> delegate, with empty parameters.
/// </summary>
public delegate void CustomEventHandler();

/// <summary>
/// The custom <see cref="EventHandler"/> delegate.
/// </summary>
/// <typeparam name="TEventArgs">The <see cref="EventHandler{TEventArgs}"/> type.</typeparam>
/// <param name="ev">The <see cref="EventHandler{TEventArgs}"/> instance.</param>
public delegate void CustomEventHandler<in TEventArgs>(TEventArgs ev);

/// <summary>
/// Contains information pertaining to specific events and important execution information.
/// </summary>
/// <param name="EventPriority">Indicates the priority the event should have in consideration to other plugin events during execution.</param>
/// <param name="AutoRegisterViaAttribute">Indicates whether or not events should be registered and unregistered when <see cref="EventManager.RegisterEvents"/> is called.</param>
public record EventHandlerInformation(int EventPriority = 500, bool AutoRegisterViaAttribute = true);

/// <summary>
/// Contains information pertaining to specific events and important execution information.
/// </summary>
/// <param name="Handler">The event handler that will be invoked for the event.</param>
/// <param name="EventPriority">Indicates the priority the event should have in consideration to other plugin events during execution.</param>
/// <param name="AutoRegisterViaAttribute">Indicates whether or not events should be registered and unregistered when <see cref="EventManager.RegisterEvents"/> is called.</param>
public record GenericHandlerInformation(CustomEventHandler Handler, int EventPriority = 500, bool AutoRegisterViaAttribute = true) : EventHandlerInformation(EventPriority, AutoRegisterViaAttribute);

/// <summary>
/// Contains information pertaining to specific events and important execution information.
/// </summary>
/// <param name="Handler">The event handler that will be invoked for the event.</param>
/// <param name="EventPriority">Indicates the priority the event should have in consideration to other plugin events during execution.</param>
/// <param name="AutoRegisterViaAttribute">Indicates whether or not events should be registered and unregistered when <see cref="EventManager.RegisterEvents"/> is called.</param>
/// <param name="ExecuteIfDenied">Indicates whether or not the event should still be called even if another event denies it from occuring.</param>
/// <typeparam name="T">The type args of the event.</typeparam>
public record TypeHandlerInformation<T>(CustomEventHandler<T> Handler, int EventPriority = 500, bool ExecuteIfDenied = false, bool AutoRegisterViaAttribute = true) : EventHandlerInformation(EventPriority, AutoRegisterViaAttribute);