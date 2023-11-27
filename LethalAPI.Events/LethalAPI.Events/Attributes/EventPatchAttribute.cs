// -----------------------------------------------------------------------
// <copyright file="EventPatchAttribute.cs" company="Redforce04">
// Copyright (c) Redforce04. All rights reserved.
// Licensed under the GPL-3.0 license.
// </copyright>
// Taken from EXILED (https://github.com/Exiled-Team/EXILED)
// Licensed under the CC BY SA 3 license. View it here:
// https://github.com/Exiled-Team/EXILED/blob/master/LICENSE.md
// Changes: Namespace adjustments.
// -----------------------------------------------------------------------

namespace LethalAPI.Events.Attributes;

using System;

using LethalAPI.Events.Interfaces;

/// <summary>
/// An attribute to contain data about an event patch.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
internal class EventPatchAttribute : Attribute
{
    private readonly Type handlerType;
    private readonly string eventName;

    /// <summary>
    /// Initializes a new instance of the <see cref="EventPatchAttribute"/> class.
    /// </summary>
    /// <param name="handlerType">The <see cref="Type"/> of the handler class that contains the event.</param>
    /// <param name="eventName">The name of the event.</param>
    internal EventPatchAttribute(Type handlerType, string eventName)
    {
        this.handlerType = handlerType;
        this.eventName = eventName;
    }

    /// <summary>
    /// Gets the <see cref="ILethalApiEvent"/> that will be raised by this patch.
    /// </summary>
    internal ILethalApiEvent? Event => (ILethalApiEvent?)this.handlerType.GetProperty(this.eventName)?.GetValue(null);
}