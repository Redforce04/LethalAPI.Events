// -----------------------------------------------------------------------
// <copyright file="BaboonBirdOnCollideWithPlayerTranspiler.cs" company="Redforce04">
// Copyright (c) Redforce04. All rights reserved.
// Licensed under the GPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable InconsistentNaming
namespace LethalAPI.Events.Patches.Enemies.AttackingAndHurtingPlayer;

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

using HarmonyLib;
using LethalAPI.Events.Attributes;
using LethalAPI.Events.EventArgs.Enemies;
using LethalAPI.Events.Patches.HarmonyTools;

using EventTranspilerInjector = HarmonyTools.EventTranspilerInjector;

/// <summary>
///     Patches the <see cref="Handlers.Enemies.EnemyAttackingPlayer"/> and <see cref="Handlers.Enemies.EnemyKillingPlayer"/> event.
/// </summary>
[HarmonyPatch(typeof(BaboonBirdAI), nameof(BaboonBirdAI.OnCollideWithPlayer))] // damage + kill animation
[EventPatch(typeof(Handlers.Enemies), nameof(Handlers.Enemies.EnemyAttackingPlayer))]
[EventPatch(typeof(Handlers.Enemies), nameof(Handlers.Enemies.EnemyKillingPlayer))]
internal static class BaboonBirdOnCollideWithPlayerTranspiler
{
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator, MethodBase original)
    {
        List<CodeInstruction> newInstructions = instructions.ToList();

        int constIndex = newInstructions.FindNthInstruction(1, instruction => instruction.opcode == OpCodes.Ldc_I4_S);
        int index = constIndex - 1;
        int originalDamage = (int)newInstructions[constIndex].operand;

        // LocalBuilder attackingPlayerEvent = DeniableEventInjector<EnemyAttackingPlayerEventArgs>.Create(ref newInstructions, ref generator, original).CreateLocalForEventArg(true).Inject().LocalEventArg!;
        EventTranspilerInjector.InjectDeniableEvent<EnemyAttackingPlayerEventArgs>(ref newInstructions, ref generator, ref original, index + 1);
        EventTranspilerInjector.InjectDeniableEvent<EnemyKillingPlayerEventArgs>(ref newInstructions, ref generator, ref original, index + 1);

        for (int i = 0; i < newInstructions.Count; i++)
            yield return newInstructions[i];
    }
}