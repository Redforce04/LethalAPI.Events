// -----------------------------------------------------------------------
// <copyright file="EventExtensions.cs" company="LethalAPI Event Team">
// Copyright (c) LethalAPI Event Team. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Events.Extensions;

/// <summary>
/// Contains extensions for modifying event parameters.
/// </summary>
public static class EventExtensions
{
    /// <summary>
    /// Using this will prevent other plugin event's from running after returning. This can break other plugins but can also be necessary for patches of the absolute highest priority. Be careful when using this.
    /// </summary>
    /// <param name="event">The event call to prevent further execution.</param>
    /// <remarks> Warning: Do not use this feature unless you know what you are doing. It can cause issue with other plugins.</remarks>
    public static void HardDenyEvent(this IDeniableEvent @event)
    {
        @event.HardDenied = true;
    }
}