// -----------------------------------------------------------------------
// <copyright file="Player.cs" company="LethalAPI Event Team">
// Copyright (c) LethalAPI Event Team. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Events.Handlers;

using EventArgs.Items;
using EventArgs.Map;
using LethalAPI.Events.EventArgs.Player;
using LethalAPI.Events.Features;

/// <summary>
///     Contains event handlers for player events.
/// </summary>
public static class Player
{
    /// <summary>
    ///     Gets the event that is invoked when a player is changing suits.
    /// </summary>
    public static Event<DeniableChangingSuitEventArgs> ChangingSuit { get; } = new ();

    /// <summary>
    ///     Gets the event that is invoked when a player is critically injured.
    /// </summary>
    public static Event<DeniableCriticallyInjureEventArgs> CriticallyInjure { get; } = new ();

    /// <summary>
    ///     Gets the event that is invoked when a player is being damaged.
    /// </summary>
    public static Event<DeniableDamagingPlayerEventArgs> DamagingPlayer { get; } = new ();

    /// <summary>
    ///     Gets the event that is invoked when a player is healed.
    /// </summary>
    public static Event<DeniableHealingEventArgs> Healing { get; } = new ();

    /// <summary>
    ///     Gets the event that is invoked when a player is dying.
    /// </summary>
    public static Event<PostPlayerDyingEventArgs> DeniablePlayerDying { get; } = new ();

    /// <summary>
    ///     Gets the event that is invoked when a player is teleporting.
    /// </summary>
    public static Event<DeniablePlayerTeleportingEventArgs> DeniablePlayerTeleporting { get; } = new ();

    /// <summary>
    ///     Gets the event that is invoked after a player dies.
    /// </summary>
    public static Event<PostPlayerDyingEventArgs> PostPlayerDying { get; } = new ();
}