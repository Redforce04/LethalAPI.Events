﻿// -----------------------------------------------------------------------
// <copyright file="UsingItemEventArgs.cs" company="LethalAPI Event Team">
// Copyright (c) LethalAPI Event Team. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Events.EventArgs.Player;

using LethalAPI.Events.Interfaces;

/// <summary>
/// Contains the arguments for the <see cref="Handlers.Player.UsingItem"/> event.
/// </summary>
public class UsingItemEventArgs : IDeniableEvent, IItemEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UsingItemEventArgs"/> class.
    /// </summary>
    /// <param name="grabbableItem">The item being used.</param>
    /// <param name="isAllowed">Indicates whether the event is allowed to execute.</param>
    public UsingItemEventArgs(GrabbableObject grabbableItem, bool isAllowed = true)
    {
        this.IsAllowed = isAllowed;
        this.GrabbableItem = grabbableItem;
    }

    /// <inheritdoc />
    public bool IsAllowed { get; set; }

    /// <inheritdoc />
    public GrabbableObject GrabbableItem { get; }
}