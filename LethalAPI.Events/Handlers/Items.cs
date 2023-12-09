// -----------------------------------------------------------------------
// <copyright file="Items.cs" company="LethalAPI Event Team">
// Copyright (c) LethalAPI Event Team. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Events.Handlers;

using Core.Events.EventArgs.Player;
using Core.Events.Features;
using EventArgs.Items;

/// <summary>
///     Contains event handlers for item events.
/// </summary>
public static class Items
{
    /// <summary>
    ///     Gets the event that is invoked when a player is dropping an item.
    /// </summary>
    public static Event<DeniableDroppingItemEventArgs> DeniableDroppingItem { get; } = new();

    /// <summary>
    ///     Gets the event that is invoked when a player is equipping an item.
    /// </summary>
    public static Event<DeniableEquippingItemEventArgs> DeniableEquippingItem { get; } = new();

    /// <summary>
    ///     Gets the event that is invoked when a player is picking up an item.
    /// </summary>
    public static Event<DeniablePickingUpItemEventArgs> DeniablePickingUpItem { get; } = new();

    /// <summary>
    ///     Gets the event that is invoked when a player is using an item.
    /// </summary>
    public static Event<DeniableUsingItemEventArgs> DeniableUsingItem { get; } = new();
}