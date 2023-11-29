// -----------------------------------------------------------------------
// <copyright file="IRoomEvent.cs" company="Redforce04">
// Copyright (c) Redforce04. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// Taken from EXILED (https://github.com/Exiled-Team/EXILED)
// Licensed under the CC BY SA 3 license. View it here:
// https://github.com/Exiled-Team/EXILED/blob/master/LICENSE.md
// Changes: Namespace adjustments, and potential removed properties.
// -----------------------------------------------------------------------

namespace LethalAPI.Events.Interfaces;

using UnityEngine;

/// <summary>
///     Event args used for all Room related events.
/// </summary>
public interface IRoomEvent : ILethalApiEvent
{
    /// <summary>
    /// Gets the Player.
    /// </summary>
    public GameObject Room { get; }
}