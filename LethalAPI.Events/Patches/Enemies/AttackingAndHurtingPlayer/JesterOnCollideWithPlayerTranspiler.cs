﻿// -----------------------------------------------------------------------
// <copyright file="JesterOnCollideWithPlayerTranspiler.cs" company="LethalAPI Event Team">
// Copyright (c) LethalAPI Event Team. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable InconsistentNaming
namespace LethalAPI.Events.Patches.Enemies.AttackingAndHurtingPlayer;

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Handlers;
using HarmonyLib;
using LethalAPI.Events.Attributes;
using LethalAPI.Events.EventArgs.Enemies;
using LethalAPI.Events.Patches.HarmonyTools;

using EventTranspilerInjector = HarmonyTools.EventTranspilerInjector;

/// <summary>
///     Patches the <see cref="Enemies.DeniableEnemyAttackingPlayer"/> and <see cref="Enemies.DeniableEnemyKillingPlayer"/> event.
/// </summary>
// [HarmonyPatch(typeof(JesterAI), nameof(JesterAI.OnCollideWithPlayer))] // kill only
// [EventPatch(typeof(Handlers.Enemies), nameof(Handlers.Enemies.EnemyAttackingPlayer))]
// [EventPatch(typeof(Handlers.Enemies), nameof(Handlers.Enemies.EnemyKillingPlayer))]
internal static class JesterOnCollideWithPlayerTranspiler
{
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator, MethodBase original)
    {
        List<CodeInstruction> newInstructions = instructions.ToList();

        int index = newInstructions.FindNthInstruction(2, instruction => instruction.opcode == OpCodes.Ret);
        EventTranspilerInjector.InjectDeniableEvent<DeniableEnemyAttackingPlayerEventArgs>(ref newInstructions, ref generator, ref original, index + 1);

        for (int i = 0; i < newInstructions.Count; i++)
            yield return newInstructions[i];
    }
}