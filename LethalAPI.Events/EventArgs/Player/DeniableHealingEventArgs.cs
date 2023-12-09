// -----------------------------------------------------------------------
// <copyright file="HealingEventArgs.cs" company="LethalAPI Event Team">
// Copyright (c) LethalAPI Event Team. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Events.EventArgs.Player;

using GameNetcodeStuff;
using LethalAPI.Events.Interfaces;

/// <summary>
///     Contains the arguments for the <see cref="Handlers.Player.Healing"/> event.
/// </summary>
/// <param name="player">
///     The player who is healing.
/// </param>
/// <param name="isAllowed">
///     Indicates whether the event is allowed to execute.
/// </param>
public sealed class DeniableHealingEventArgs(PlayerControllerB player, bool isAllowed = true) : IDeniableEvent, IPlayerEvent
{
    /// <inheritdoc />
    public PlayerControllerB Player { get; init; } = player;

    /// <inheritdoc />
    public bool IsAllowed { get; set; } = isAllowed;

    /// <inheritdoc />
    bool IDeniableEvent.HardDenied { get; set; } = false;
}