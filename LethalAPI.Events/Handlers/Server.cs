// -----------------------------------------------------------------------
// <copyright file="Server.cs" company="LethalAPI Event Team">
// Copyright (c) LethalAPI Event Team. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Events.Handlers;

using LethalAPI.Events.EventArgs.Server;
using LethalAPI.Events.Features;

/// <summary>
/// Contains "Server" event handlers.
/// </summary>
// ReSharper disable MemberCanBePrivate.Global
public static class Server
{
    /// <summary>
    ///     Gets the event that is invoked when a save is initiated.
    /// </summary>
    public static Event<SavingEventArgs> Saving { get; } = new ();

    /// <summary>
    ///     Gets the event that is invoked when a save is initiated.
    /// </summary>
    public static Event<LoadingSaveEventArgs> LoadingSave { get; } = new ();

    /// <summary>
    ///     Gets the event that is invoked when a save is being reset.
    /// </summary>
    public static Event<DeniableResettingSaveEventArgs> DeniableResettingSave { get; } = new ();

    /// <summary>
    ///     Gets the event that is invoked when the game gets to the very first menu.
    /// </summary>
    public static Event GameOpened { get; } = new(nameof(GameOpened));
}