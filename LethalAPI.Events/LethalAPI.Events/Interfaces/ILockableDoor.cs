// -----------------------------------------------------------------------
// <copyright file="ILockableDoor.cs" company="Redforce04">
// Copyright (c) Redforce04. All rights reserved.
// Licensed under the GPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable InconsistentNaming
namespace LethalAPI.Events.Interfaces;

/// <summary>
///     Event args used for all door lock related events.
/// </summary>
public interface ILockableDoor : IDoorEvent
{
    /// <summary>
    /// Gets the <see cref="DoorLock"/>.
    /// </summary>
    public DoorLock Lock { get; }
}