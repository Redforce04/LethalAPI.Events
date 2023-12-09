// -----------------------------------------------------------------------
// <copyright file="PostSentCommandEventArgs.cs" company="LethalAPI Event Team">
// Copyright (c) LethalAPI Event Team. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Events.EventArgs.Terminal;

/// <summary>
///     Represents the event args that are called after a player leaves the terminal.
/// </summary>
/// <param name="command">
///     The command that was sent.
/// </param>
/// <param name="terminal">
///     The <see cref="TerminalNode"/> instance that was used.
/// </param>
public sealed class PostSentCommandEventArgs(string command, TerminalNode terminal) : ILethalApiEvent, ITerminalEvent
{
    /// <summary>
    ///     Gets the command that was sent.
    /// </summary>
    public string Command { get; } = command;

    /// <inheritdoc />
    public TerminalNode Terminal { get; init; } = terminal;
}