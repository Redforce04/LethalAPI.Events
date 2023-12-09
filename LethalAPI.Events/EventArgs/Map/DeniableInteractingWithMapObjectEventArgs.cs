// -----------------------------------------------------------------------
// <copyright file="DeniableInteractingWithMapObjectEventArgs.cs" company="LethalAPI Event Team">
// Copyright (c) LethalAPI Event Team. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Events.EventArgs.Map;

using GameNetcodeStuff;

/// <summary>
///     Represents the event args that are called after a map object has been interacted with
/// </summary>
/// <param name="player">
///     The player who is interacting with the map object.
/// </param>
/// <param name="interactable">
///     The item being interacted with.
/// </param>
/// <param name="isAllowed">
///     Indicates whether the event is allowed to execute.
/// </param>
public sealed class DeniableInteractingWithMapObjectEventArgs(PlayerControllerB player, InteractTrigger interactable, bool isAllowed = true) : IDeniableEvent, IPlayerEvent
{
    /// <inheritdoc />
    public bool IsAllowed { get; set; } = isAllowed;

    /// <inheritdoc />
    bool IDeniableEvent.HardDenied { get; set; } = false;

    /// <summary>
    ///     Gets the item that is being interacted with.
    /// </summary>
    public InteractTrigger Interactable { get; init; } = interactable;

    /// <inheritdoc />
    public PlayerControllerB Player { get; init; } = player;
}