// -----------------------------------------------------------------------
// <copyright file="CriticallyInjureEventArgs.cs" company="LethalAPI Event Team">
// Copyright (c) LethalAPI Event Team. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Events.EventArgs.Player;

using GameNetcodeStuff;
using LethalAPI.Events.Interfaces;

/// <summary>
///     Contains the arguments for the <see cref="Handlers.Player.CriticallyInjure"/> event.
/// </summary>
/// <param name="player">
///     The player who is being critically injured.
/// </param>
/// <param name="isAllowed">
///     Indicates whether the event is allowed to execute.
/// </param>
public class DeniableCriticallyInjureEventArgs(PlayerControllerB player, bool isAllowed = true) : IDeniableEvent, IPlayerEvent
{
    /// <summary>
    /// Gets the <see cref="PlayerControllerB"/> being critically injured.
    /// </summary>
    public PlayerControllerB Player { get; init; } = player;

    /// <inheritdoc />
    public bool IsAllowed { get; set; } = isAllowed;

    /// <inheritdoc />
    bool IDeniableEvent.HardDenied { get; set; } = false;
}