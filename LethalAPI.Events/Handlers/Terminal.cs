// -----------------------------------------------------------------------
// <copyright file="Terminal.cs" company="LethalAPI Event Team">
// Copyright (c) LethalAPI Event Team. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Events.Handlers;

using Core.Events.Features;
using EventArgs.Terminal;

/// <summary>
///     Contains event handlers for terminal events.
/// </summary>
public static class Terminal
{
    /// <summary>
    ///     Gets the event that is invoked when a player is buying an item from the shop.
    /// </summary>
    public static Event<DeniableBuyingItemEventArgs> DeniableBuyingItem { get; } = new();

    /// <summary>
    ///     Gets the event that is invoked when a player is entering the terminal.
    /// </summary>
    public static Event<DeniableEnteringTerminalEventArgs> DeniableEnteringTerminal { get; } = new();

    /// <summary>
    ///     Gets the event that is invoked when a player is sending a command.
    /// </summary>
    public static Event<DeniableSendingCommandEventArgs> DeniableSendingCommand { get; } = new();

    /// <summary>
    ///     Gets the event that is invoked after a player leaves the terminal.
    /// </summary>
    public static Event<PostLeavingTerminalEventArgs> PostLeavingTerminal { get; } = new();

    /// <summary>
    ///     Gets the event that is invoked after a command is sent.
    /// </summary>
    public static Event<PostSentCommandEventArgs> PostSentCommand { get; } = new();
}