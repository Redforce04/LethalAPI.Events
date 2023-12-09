// -----------------------------------------------------------------------
// <copyright file="DeniableBuyingItemEventArgs.cs" company="LethalAPI Event Team">
// Copyright (c) LethalAPI Event Team. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Events.EventArgs.Terminal;

/// <summary>
///     Represents the event args that are called before an item is purchased.
/// </summary>
/// <param name="terminal">
///     The terminal node that was accessed.
/// </param>
/// <param name="isAllowed">
///     Indicates whether the event is allowed to execute.
/// </param>
public sealed class DeniableBuyingItemEventArgs(TerminalNode terminal, bool isAllowed = true) : IDeniableEvent, ITerminalEvent
{
    /// <inheritdoc />
    public bool IsAllowed { get; set; } = isAllowed;

    /// <inheritdoc />
    bool IDeniableEvent.HardDenied { get; set; } = false;

    /// <inheritdoc />
    public TerminalNode Terminal { get; init; } = terminal;
}