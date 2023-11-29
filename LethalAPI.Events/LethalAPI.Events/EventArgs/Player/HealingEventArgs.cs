// -----------------------------------------------------------------------
// <copyright file="HealingEventArgs.cs" company="Redforce04">
// Copyright (c) Redforce04. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Events.EventArgs.Player;

using GameNetcodeStuff;
using LethalAPI.Events.Interfaces;

/// <summary>
/// Contains the arguments for the <see cref="Handlers.Player.Healing"/> event.
/// </summary>
public sealed class HealingEventArgs : IDeniableEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HealingEventArgs"/> class.
    /// </summary>
    /// <param name="player">The player who is healing.</param>
    /// <param name="isAllowed">Indicates whether the event is allowed to execute.</param>
    public HealingEventArgs(PlayerControllerB player, bool isAllowed = true)
    {
        this.Player = player;
        this.IsAllowed = isAllowed;
    }

    /// <summary>
    /// Gets the player who is being healed.
    /// </summary>
    public PlayerControllerB Player { get; }

    /// <inheritdoc />
    public bool IsAllowed { get; set; }
}