// -----------------------------------------------------------------------
// <copyright file="DeniableResettingSaveEventArgs.cs" company="LethalAPI Event Team">
// Copyright (c) LethalAPI Event Team. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Events.EventArgs.Server;

using LethalAPI.Events.Interfaces;

/// <summary>
///     Represents the event args that are called when saving.
/// </summary>
/// <param name="saveSlot">
///     The slot being reset.
/// </param>
/// <param name="isAllowed">
///     Indicates whether the event is allowed to execute.
/// </param>
public sealed class DeniableResettingSaveEventArgs(string saveSlot, bool isAllowed = true) : IDeniableEvent
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

    /// <inheritdoc />
    public bool IsAllowed { get; set; } = isAllowed;

    /// <inheritdoc />
    bool IDeniableEvent.HardDenied { get; set; } = false;
}