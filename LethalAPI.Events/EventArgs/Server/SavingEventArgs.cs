// -----------------------------------------------------------------------
// <copyright file="SavingEventArgs.cs" company="LethalAPI Event Team">
// Copyright (c) LethalAPI Event Team. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Events.EventArgs.Server;

using System;

using LethalAPI.Events.Interfaces;

#pragma warning disable SA1201

/// <summary>
///     Represents the event args that are called when saving.
/// </summary>
/// <param name="saveSlot">
///     The slot being saved.
/// </param>
/// <param name="saveItem">
///     The item or items being saved.
/// </param>
// ReSharper disable once ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
public sealed class SavingEventArgs(string saveSlot, SaveItem saveItem) : ILethalApiEvent
{
    /// <summary>
    ///     Gets the slot of the save being saved to.
    /// </summary>
    /// <code>
    /// Currently Supports Save Slots:
    ///     LCGeneralSaveData - Global save slot.
    ///     LCSaveFile1 - Save slot 1.
    ///     LCSaveFile2 - Save slot 2.
    ///     LCSaveFile3 - Save slot 3.
    /// </code>
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public string SaveSlot { get; } = saveSlot;

    /// <summary>
    ///     Gets the item or items being saved.
    /// </summary>
    public SaveItem SaveItem { get; } = saveItem;
}

/// <summary>
///     Item or items that are being saved.
/// </summary>
[Flags]
public enum SaveItem
{
    /// <summary>
    ///     Mod information is being saved.
    /// </summary>
    Mods,

    /// <summary>
    ///     Game stats are being saved. Called after GameNetworkManager.SaveGameValues.
    /// </summary>
    GameValues,

    /// <summary>
    ///     Ship items are saved. Called after GameNetworkManager.SaveItemsInShip.
    /// </summary>
    ShipItems,

    /// <summary>
    ///     Unsellable items are saved. Called after GameNetworkManager.ConvertUnsellableItemsToCredits.
    /// </summary>
    UnsellableItems,

    /// <summary>
    ///     Local player values are saved. Called after GameNetworkManager.SaveLocalPlayerValues.
    /// </summary>
    LocalPlayerValues,
}