// -----------------------------------------------------------------------
// <copyright file="PlayerHealingInjuringTranspiler.cs" company="LethalAPI Modding Community">
// Copyright (c) LethalAPI Modding Community. All rights reserved.
// Licensed under the GPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable InconsistentNaming
namespace LethalAPI.Events.Patches.Player;

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

using EventArgs.Player;
using GameNetcodeStuff;
using HarmonyLib;
using LethalAPI.Events.Attributes;
using LethalAPI.Events.Patches.HarmonyTools;

using EventTranspilerInjector = HarmonyTools.EventTranspilerInjector;

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
        List<CodeInstruction> newInstructions = instructions.ToList();

        int index = newInstructions.FindNthInstruction(2, instruction => instruction.opcode == OpCodes.Ret);
        EventTranspilerInjector.InjectDeniableEvent<HealingEventArgs>(ref newInstructions, ref generator, ref original, index + 1);
        EventTranspilerInjector.InjectDeniableEvent<CriticallyInjureEventArgs>(ref newInstructions, ref generator, ref original, 2);

        foreach (CodeInstruction? instruction in newInstructions)
            yield return instruction;
    }
}