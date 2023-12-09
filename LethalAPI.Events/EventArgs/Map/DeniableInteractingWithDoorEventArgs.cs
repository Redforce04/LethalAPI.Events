// -----------------------------------------------------------------------
// <copyright file="DeniableInteractingWithDoorEventArgs.cs" company="LethalAPI Event Team">
// Copyright (c) LethalAPI Event Team. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Events.EventArgs.Map;

using DunGen;
using GameNetcodeStuff;

/// <summary>
///     Represents the event args that are called before a door is opened.
/// </summary>
/// <param name="player">
///     The player interacting with the door.
/// </param>
/// <param name="door">
///     The door being interacted with.
/// </param>
/// <param name="isAllowed">
///     Indicates whether the event is allowed to execute.
/// </param>
public sealed class DeniableInteractingWithDoorEventArgs(PlayerControllerB player, Door door, bool isAllowed = true) : IDeniableEvent, IDoorEvent, IPlayerEvent
{
    /// <inheritdoc />
    public bool IsAllowed { get; set; } = isAllowed;

    /// <inheritdoc />
    bool IDeniableEvent.HardDenied { get; set; } = false;

    /// <inheritdoc />
    public PlayerControllerB Player { get; init; } = player;

    /// <inheritdoc />
    public Door Door { get; init; } = door;

}