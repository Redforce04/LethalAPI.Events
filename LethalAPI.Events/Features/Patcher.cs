// -----------------------------------------------------------------------
// <copyright file="Patcher.cs" company="LethalAPI Event Team">
// Copyright (c) LethalAPI Event Team. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// Taken from EXILED (https://github.com/Exiled-Team/EXILED)
// Licensed under the CC BY SA 3 license. View it here:
// https://github.com/Exiled-Team/EXILED/blob/master/LICENSE.md
// Changes: Namespace adjustments.
// -----------------------------------------------------------------------

// ReSharper disable RedundantNameQualifier
// ReSharper disable MemberCanBePrivate.Global
namespace LethalAPI.Events.Features;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using HarmonyLib;
using LethalAPI.Events;
using LethalAPI.Events.Attributes;
using LethalAPI.Events.Interfaces;

/// <summary>
/// A tool for patching.
/// </summary>
internal class Patcher
{
    /// <summary>
    /// The below variable is used to increment the name of the harmony instance, otherwise harmony will not work upon a plugin reload.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    private static int patchesCounter;

    /// <summary>
    /// Initializes a new instance of the <see cref="Patcher"/> class.
    /// </summary>
    internal Patcher()
    {
        this.Harmony = new($"lethalapi.events.{++patchesCounter}");
    }

    /// <summary>
    /// Gets or sets a <see cref="HashSet{T}"/> that contains all patch types that haven't been patched.
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global
    private static HashSet<Type> UnpatchedTypes { get; set; } = Plugin.Instance.Config.UseDynamicPatching ? GetNonEventPatchTypes() : GetAllPatchTypes();

    /// <summary>
    /// Gets a <see cref="HashSet{T}"/> that contains all patch types that have been patched.
    /// </summary>
    private static HashSet<Type> PatchedTypes { get; } = new();

    /// <summary>
    /// Gets a set of types and methods for which LethalAPI patches should not be run.
    /// </summary>
    // ReSharper disable once CollectionNeverUpdated.Global
    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once CollectionNeverUpdated.Local
    private static HashSet<MethodBase> DisabledPatchesHashSet { get; } = new();

    /// <summary>
    /// Gets the <see cref="HarmonyLib.Harmony"/> instance.
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global
    private HarmonyLib.Harmony Harmony { get; }

    /// <summary>
    /// Patches all events.
    /// </summary>
    /// <param name="failedPatch">the number of failed patch returned.</param>
    /// <param name="totalPatches">the number of total patches attempted.</param>
    internal void PatchAll(out int failedPatch, out int totalPatches)
    {
        totalPatches = 0;
        failedPatch = 0;

        try
        {
            List<Type> toPatch = new (UnpatchedTypes);
            foreach (Type patch in toPatch)
            {
                totalPatches++;
                try
                {
                    PatchedTypes.Add(patch);
                    this.Harmony.CreateClassProcessor(patch).Patch();
                    UnpatchedTypes.Remove(patch);
                    Log.Debug($"Patching type '{patch.FullName}'", Plugin.Instance.Config.LogEventPatching, "LethalAPI-Patcher");
                }
                catch (HarmonyException exception)
                {
                    Log.Error($"Could not patch type '{patch.Name}' due to an error.");
                    if(Plugin.Instance.Config.Debug)
                        Log.Exception(exception);
                    failedPatch++;
                }
            }

            Log.Debug("Events patched by attributes successfully!");
        }
        catch (Exception exception)
        {
            Log.Error($"Patching by attributes failed!\n{exception}");
        }

        if (Plugin.Instance.Config.DetailedPatchLogging.Contains("Pause"))
        {
            Log.Raw(" [&2WARNING&r] &5Thread is going to be paused. You can disable this by removing 'Pause' from the Detailed Patch Logging config option.");
            System.Threading.Thread.Sleep(int.MaxValue);
        }
    }

    /// <summary>
    /// Unpatches all events.
    /// </summary>
    internal void UnpatchAll()
    {
        Log.Debug("Un-patching events...");
        HarmonyLib.Harmony.UnpatchID(this.Harmony.Id);
        UnpatchedTypes = GetAllPatchTypes();
        PatchedTypes.Clear();

        Log.Debug("All events have been unpatched. Goodbye!");
    }

    /// <summary>
    /// Patches all events that target a specific <see cref="ILethalApiEvent"/>.
    /// </summary>
    /// <param name="event">The <see cref="ILethalApiEvent"/> all matching patches should target.</param>
    internal void Patch(ILethalApiEvent @event)
    {
        try
        {
            List<Type> types = new (GetAllPatchTypes().Where(x => x.GetCustomAttributes<EventPatchAttribute>().Any((epa) => epa.Event == @event)));

            Log.Debug($"Patching event for {types.Count} types.", Plugin.Instance.Config.LogEventPatching, "LethalAPI-Patcher");
            foreach (Type type in types)
            {
                if (PatchedTypes.Contains(type))
                {
                    Log.Debug($"Type {type.FullName} has already been patched.", Plugin.Instance.Config.LogEventPatching, "LethalAPI-Patcher");
                    continue;
                }

                PatchedTypes.Add(type);
                List<MethodInfo> methodInfos = new PatchClassProcessor(this.Harmony, type).Patch();
                if (DisabledPatchesHashSet.Any(x => methodInfos.Contains(x)))
                {
                    this.ReloadDisabledPatches();
                }

                UnpatchedTypes.Remove(type);
            }
        }
        catch (Exception ex)
        {
            Log.Error($"Could not patch event '{@event.GetType().Name}' due to an error. [Dynamic]");
            if(Plugin.Instance.Config.Debug)
                Log.Exception(ex);
        }
    }

    /// <summary>
    /// Gets all types that have a <see cref="HarmonyPatch"/> attributed to them.
    /// </summary>
    /// <returns>A <see cref="HashSet{T}"/> of all patch types.</returns>
    private static HashSet<Type> GetAllPatchTypes()
    {
        HashSet<Type> types = new ();
        foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
        {
            try
            {
                if (type.GetCustomAttribute<HarmonyPatch>() is null)
                {
                    continue;
                }

                types.Add(type);
            }
            catch (TypeLoadException)
            {
            }
        }

        return types;
    }

    /// <summary>
    /// Gets all types that have a <see cref="HarmonyPatch"/> attributed to them, but don't have an <see cref="EventPatchAttribute"/> attribute.
    /// </summary>
    /// <returns>A <see cref="HashSet{T}"/> of all patch types.</returns>
    private static HashSet<Type> GetNonEventPatchTypes()
    {
        HashSet<Type> types = new ();
        foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
        {
            try
            {
                if (type.GetCustomAttribute<HarmonyPatch>() is null)
                {
                    continue;
                }

                if (type.GetCustomAttributes<EventPatchAttribute>().Any())
                {
                    continue;
                }

                if (type.GetCustomAttributes<EventPatchAttribute>().Any())
                {
                    continue;
                }

                types.Add(type);
            }
            catch (TypeLoadException)
            {
            }
        }

        return types;
    }

    /// <summary>
    /// Checks the <see cref="DisabledPatchesHashSet"/> list and un-patches any methods that have been defined there. Once un-patching has been done, they can be patched by plugins, but will not be re-patchable by LethalAPI until a server reboot.
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global
    private void ReloadDisabledPatches()
    {
        foreach (MethodBase method in DisabledPatchesHashSet)
        {
            this.Harmony.Unpatch(method, HarmonyPatchType.All, this.Harmony.Id);

            Log.Info($"Unpatched {method.Name}");
        }
    }
}
