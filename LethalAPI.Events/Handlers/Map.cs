// -----------------------------------------------------------------------
// <copyright file="Map.cs" company="LethalAPI Event Team">
// Copyright (c) LethalAPI Event Team. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Events.Handlers;

using Core.Events.Features;
using EventArgs.Map;

/// <summary>
///     Contains event handlers for map events.
/// </summary>
public static class Map
{
    /// <summary>
    ///     Gets the event that is invoked when a player is interacting with a door.
    /// </summary>
    public static Event<DeniableInteractingWithDoorEventArgs> DeniableInteractingWithDoor { get; } = new();

    /// <summary>
    ///     Gets the event that is invoked when a player is interacting with a map object.
    /// </summary>
    public static Event<DeniableInteractingWithMapObjectEventArgs> DeniableInteractingWithMapObject { get; } = new();

    /// <summary>
    ///     Gets the event that is invoked when a player is unlocking a door.
    /// </summary>
    public static Event<DeniableUnlockingDoorEventArgs> DeniableUnlockingDoor { get; } = new();

    /// <summary>
    ///     Gets the event that is invoked when the map is generating.
    /// </summary>
    public static Event<GeneratingMapEventArgs> GeneratingMap { get; } = new();

    /// <summary>
    ///     Gets the event that is invoked when the map seed is generating.
    /// </summary>
    public static Event<GeneratingMapSeedEventArgs> GeneratingMapSeed { get; } = new();

    /// <summary>
    ///     Gets the event that is invoked after the map is loaded.
    /// </summary>
    public static Event<PostMapLoadedEventArgs> PostMapLoaded { get; } = new();
}
