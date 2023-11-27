// -----------------------------------------------------------------------
// <copyright file="EventType.cs" company="Redforce04">
// Copyright (c) Redforce04. All rights reserved.
// Licensed under the GPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Events;

/// <summary>
/// A list of all events.
/// </summary>
public enum EventType
{
    /// <summary>
    /// Auto-determines the event to use.
    /// </summary>
    None = 0,

    /// <inheritdoc cref="Handlers.Player.CriticallyInjure"/>
    CriticallyInjure = 1,

    /// <inheritdoc cref="Handlers.Player.Healing"/>
    Healing = 2,

    /// <inheritdoc cref="Handlers.Player.UsingItem"/>
    UsingItem = 3,

    /// <inheritdoc cref="Handlers.Player.UsingKey"/>
    UsingKey = 4,

    /// <inheritdoc cref="Handlers.Server.LoadingSave"/>
    LoadingSave = 5,

    // <inheritdoc cref="Handlers.Server.ResetSave"/>

    /// <inheritdoc cref="LethalAPI.Events.EventArgs.Server.ResetSaveEventArgs"/>
    ResetSave = 6,

    /// <inheritdoc cref="Handlers.Server.Saving"/>
    Saving = 6,

    /// <inheritdoc cref="Handlers.Server.GameOpened"/>
    GameOpened = 7,
}
