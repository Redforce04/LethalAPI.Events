// -----------------------------------------------------------------------
// <copyright file="DeniableUnlockingDoorEventArgs.cs" company="LethalAPI Event Team">
// Copyright (c) LethalAPI Event Team. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Events.EventArgs.Map;

using DunGen;
using GameNetcodeStuff;
using LethalAPI.Events.Interfaces;

/// <summary>
///     Contains the arguments for the <see cref="Handlers.Player.UsingKey"/> event.
/// </summary>
/// <param name="player">
///     The player unlocking the doo.
/// </param>
/// <param name="door">
///     The target door.
/// </param>
/// <param name="item">
///     The key item being used.
/// </param>
/// <param name="doorLock">
///     The lock being unlocked.
/// </param>
/// <param name="isAllowed">
///     Indicates whether the event is allowed to execute.
/// </param>
public class DeniableUnlockingDoorEventArgs(PlayerControllerB player, GrabbableObject item, Door door, DoorLock doorLock, bool isAllowed = true) : IDeniableEvent, IPlayerEvent, IItemEvent, ILockableDoor
{
    /// <inheritdoc />
    public bool IsAllowed { get; set; } = isAllowed;

    /// <inheritdoc />
    bool IDeniableEvent.HardDenied { get; set; } = false;

    /// <inheritdoc />
    public PlayerControllerB Player { get; init; } = player;

    /// <inheritdoc />
    public Door Door { get; init; } = door;

    /// <inheritdoc />
    public DoorLock Lock { get; init; } = doorLock;

    /// <inheritdoc />
    public GrabbableObject Item { get; init; } = item;

    /// <summary>
    ///     Gets the key unlocking the door.
    /// </summary>
    /// <remarks>
    ///     If the item unlocking the door is a lock pick, this will be null.
    /// </remarks>
    public KeyItem? Key => this.Item as KeyItem;

    /// <summary>
    ///     Gets the lock picker unlocking the door.
    /// </summary>
    /// <remarks>
    ///     If the item unlocking the door is a key, this will be null.
    /// </remarks>
    public LockPicker? LockPicker => this.Item as LockPicker;
}