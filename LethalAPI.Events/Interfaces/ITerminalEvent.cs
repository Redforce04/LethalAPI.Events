// -----------------------------------------------------------------------
// <copyright file="ITerminalEvent.cs" company="LethalAPI Event Team">
// Copyright (c) LethalAPI Event Team. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Events.Interfaces;

/// <summary>
///     Events args utilizing terminal features.
/// </summary>
public interface ITerminalEvent
{
    /// <summary>
    ///     Gets the <see cref="TerminalNode"/> instance that was accessed.
    /// </summary>
    public TerminalNode Terminal { get; init; }
}