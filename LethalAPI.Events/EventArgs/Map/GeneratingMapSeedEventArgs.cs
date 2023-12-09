// -----------------------------------------------------------------------
// <copyright file="GeneratingMapSeedEventArgs.cs" company="LethalAPI Event Team">
// Copyright (c) LethalAPI Event Team. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Events.EventArgs.Map;

/// <summary>
///     Represents the event args that are called before a player is killed by an enemy.
/// </summary>
/// <param name="seed">
///     The seed that was chosen.
/// </param>
public sealed class GeneratingMapSeedEventArgs(int seed)
{
    /// <summary>
    /// Gets or sets the map generation seed.
    /// </summary>
    public int Seed { get; set; } = seed;
}