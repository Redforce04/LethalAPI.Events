// -----------------------------------------------------------------------
// <copyright file="Plugin.cs" company="LethalAPI Event Team">
// Copyright (c) LethalAPI Event Team. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable RedundantNameQualifier
// ReSharper disable RedundantNameQualifier
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedType.Global
#pragma warning disable SA1401 // Field should be made private.
#pragma warning disable SA1402 // File can only contain a single type.
namespace LethalAPI.Events;

using System;
using System.Diagnostics;

using LethalAPI.Events.Features;
using UnityEngine.SceneManagement;

/// <summary>
/// The main plugin class.
/// </summary>
// ReSharper disable ClassNeverInstantiated.Global
public sealed class Plugin : Core.Features.Plugin<Config>
{
    /// <summary>
    /// Gets the main instance of the events api.
    /// </summary>
    public static Plugin Instance { get; private set; } = null!;

    /// <inheritdoc />
    public override string Name => "LethalAPI.Events";

    /// <inheritdoc />
    public override string Description => "Provides a maintainable and extendable event system for LethalAPI";

    /// <inheritdoc />
    public override string Author => "LethalAPI Event Team";

    /// <inheritdoc />
    public override Version Version => new(1, 0, 0);

    /// <summary>
    /// Gets the <see cref="Core.Events.Features.Patcher"/> used to employ all patches.
    /// </summary>
    internal Patcher Patcher { get; private set; } = null!;

    /// <inheritdoc />
    public override void OnEnabled()
    {
        if (!this.Config.IsEnabled)
        {
            return;
        }

        Instance = this;

        Stopwatch watch = Stopwatch.StartNew();
        this.Patch();
        watch.Stop();
        Log.Info($"All patches completed in {watch.Elapsed}");

        SceneManager.sceneUnloaded += Handlers.Internal.SceneUnloaded.OnSceneUnloaded;

        Log.Info($"Started plugin LethalAPI.Events by LethalAPI Event Team.");
    }

    /// <inheritdoc />
    public override void OnDisabled()
    {
        this.Unpatch();
        SceneManager.sceneUnloaded -= Handlers.Internal.SceneUnloaded.OnSceneUnloaded;
        base.OnDisabled();
    }

    /// <summary>
    /// Patches all events.
    /// </summary>
    private void Patch()
    {
        try
        {
            this.Patcher = new Patcher();
            this.Patcher.PatchAll(out int failedPatch, out int totalPatches);

            if (failedPatch == 0)
            {
                Log.Debug($"Events patched successfully! [{totalPatches} total patches]");
            }
            else
            {
                Log.Error($"Patching failed! There are {failedPatch} broken patches [{totalPatches} total patches].");
            }
        }
        catch (Exception exception)
        {
            Log.Error($"Patching failed!\n{exception}");
        }
    }

    /// <summary>
    /// Unpatches all events.
    /// </summary>
    private void Unpatch()
    {
        Log.Debug("Unpatching events...");
        this.Patcher.UnpatchAll();
        Log.Debug("All events have been unpatched complete. Goodbye!");
    }
}
