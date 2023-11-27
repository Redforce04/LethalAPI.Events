﻿// -----------------------------------------------------------------------
// <copyright file="CriticallyInjureEventArgs.cs" company="Redforce04">
// Copyright (c) Redforce04. All rights reserved.
// Licensed under the GPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Events.EventArgs.Player;

using GameNetcodeStuff;
using LethalAPI.Events.Interfaces;

/// <summary>
/// Contains the arguments for the <see cref="Handlers.Player.CriticallyInjure"/> event.
/// </summary>
public class CriticallyInjureEventArgs : IDeniableEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CriticallyInjureEventArgs"/> class.
    /// </summary>
    /// <param name="player">The player who is being critically injured.</param>
    /// <param name="isAllowed">Indicates whether the event is allowed to execute.</param>
    public CriticallyInjureEventArgs(PlayerControllerB player, bool isAllowed = true)
    {
        this.Player = player;
        this.IsAllowed = isAllowed;
    }

    /// <summary>
    /// Gets the <see cref="PlayerControllerB"/> being critically injured.
    /// </summary>
    public PlayerControllerB Player { get; }

    /// <inheritdoc />
    public bool IsAllowed { get; set; }
}