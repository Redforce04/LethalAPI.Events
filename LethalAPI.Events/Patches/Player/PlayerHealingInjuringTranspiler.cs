﻿// -----------------------------------------------------------------------
// <copyright file="PlayerHealingInjuringTranspiler.cs" company="LethalAPI Modding Community">
// Copyright (c) LethalAPI Modding Community. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable InconsistentNaming
// ReSharper disable RedundantArgumentDefaultValue
namespace LethalAPI.Events.Patches.Player;

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

using EventArgs.Player;
using GameNetcodeStuff;
using HarmonyLib;
using HarmonyTools.Injectors;
using LethalAPI.Events.Attributes;
using LethalAPI.Events.Patches.HarmonyTools;
using MonoMod.Utils;

/// <summary>
///     Patches the <see cref="Handlers.Player.Healing"/> and <see cref="Handlers.Player.CriticallyInjure"/> event.
/// </summary>
[HarmonyPatch(typeof(PlayerControllerB), nameof(PlayerControllerB.MakeCriticallyInjured))]
[EventPatch(typeof(Handlers.Player), nameof(Handlers.Player.CriticallyInjure))]
[EventPatch(typeof(Handlers.Player), nameof(Handlers.Player.Healing))]
internal static class PlayerHealingInjuringTranspiler
{
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator, MethodBase original)
    {
        // If enabled, the output info contains more information than just the opcode, index, and label.
        const bool ShowDebugInfo = false;
        List<CodeInstruction> newInstructions = instructions.ToList();

        int index = newInstructions.FindNthInstruction(2, instruction => instruction.opcode == OpCodes.Ret);
        Dictionary<ushort, ushort> indexes = DeniableEventInjector<DeniableHealingEventArgs>.Create(ref newInstructions, ref generator, original).InjectDeniableEvent(index + 1).InjectedInstructionIndexes;
        indexes.AddRange(DeniableEventInjector<DeniableCriticallyInjureEventArgs>.Create(ref newInstructions, ref generator, original).InjectDeniableEvent(2).InjectedInstructionIndexes);

        // EventTranspilerInjector.InjectDeniableEvent<HealingEventArgs>(ref newInstructions, ref generator, ref original, index + 1);
        // EventTranspilerInjector.InjectDeniableEvent<CriticallyInjureEventArgs>(ref newInstructions, ref generator, ref original, 2);
        Log.Debug($"[&3Patching {nameof(PlayerHealingInjuringTranspiler)}&r]", Plugin.Instance.Config.DetailedPatchLogging.Contains(nameof(PlayerHealingInjuringTranspiler)));

        for (int i = 0; i < newInstructions.Count; i++)
            yield return newInstructions[i].Log(i, -1, Plugin.Instance.Config.DetailedPatchLogging.Contains(nameof(PlayerHealingInjuringTranspiler)), ShowDebugInfo, indexes);
    }
}