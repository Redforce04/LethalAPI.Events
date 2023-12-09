// -----------------------------------------------------------------------
// <copyright file="PostPlayerDyingEventArgs.cs" company="LethalAPI Event Team">
// Copyright (c) LethalAPI Event Team. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Events.EventArgs.Player;

/// <summary>
///     Represents the event args that are called after a player has finished dying completely.
/// </summary>
/// <param name="player">
///     The player that is being teleported.
/// </param>
public sealed class PostPlayerDyingEventArgs(PlayerControllerB player) : IPlayerEvent
{
    /// <inheritdoc />
    public PlayerControllerB Player { get; init; } = player;
}