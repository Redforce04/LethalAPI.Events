// -----------------------------------------------------------------------
// <copyright file="PostLeavingTerminalEventArgs.cs" company="LethalAPI Event Team">
// Copyright (c) LethalAPI Event Team. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Events.EventArgs.Terminal;

using GameNetcodeStuff;

/// <summary>
///     Represents the event args that are called after a player leaves the terminal.
/// </summary>
/// <param name="player">
///     The player that left the terminal.
/// </param>
/// <param name="terminal">
///     The terminal node that was accessed.
/// </param>
public sealed class PostLeavingTerminalEventArgs(PlayerControllerB player, TerminalNode terminal) : IPlayerEvent, ITerminalEvent
{
    /// <summary>
    ///     Gets the player that was leaving the terminal.
    /// </summary>
    public PlayerControllerB Player { get; init; } = player;

    /// <inheritdoc />
    public TerminalNode Terminal { get; init; } = terminal;
}