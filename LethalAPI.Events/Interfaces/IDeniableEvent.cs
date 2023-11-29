// -----------------------------------------------------------------------
// <copyright file="IDeniableEvent.cs" company="Redforce04">
// Copyright (c) Redforce04. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// Taken from EXILED (https://github.com/Exiled-Team/EXILED)
// Licensed under the CC BY SA 3 license. View it here:
// https://github.com/Exiled-Team/EXILED/blob/master/LICENSE.md
// Changes: Namespace adjustments, and potential removed properties.
// -----------------------------------------------------------------------

namespace LethalAPI.Events.Interfaces;

/// <summary>
///     Event args for events that can be allowed or denied.
/// </summary>
public interface IDeniableEvent : ILethalApiEvent
{
    /// <summary>
    ///     Gets or sets a value indicating whether or not the event is allowed to continue.
    /// </summary>
    public bool IsAllowed { get; set; }
}