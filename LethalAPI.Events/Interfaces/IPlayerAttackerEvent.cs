// -----------------------------------------------------------------------
// <copyright file="IPlayerAttackerEvent.cs" company="Redforce04">
// Copyright (c) Redforce04. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Events.Interfaces;

using GameNetcodeStuff;

/// <summary>
///     Event args for when a player is taking damage from another player.
/// </summary>
public interface IPlayerAttackerEvent : IPlayerEvent
{
    /// <summary>
    /// Gets the attacking player.
    /// </summary>
    public PlayerControllerB Attacker { get; init; }
}