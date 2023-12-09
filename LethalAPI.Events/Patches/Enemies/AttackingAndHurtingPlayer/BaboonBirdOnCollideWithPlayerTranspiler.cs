// -----------------------------------------------------------------------
// <copyright file="BaboonBirdOnCollideWithPlayerTranspiler.cs" company="LethalAPI Event Team">
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

using EventArgs.Enemies;
using Handlers;
using HarmonyLib;
using HarmonyTools.Injectors;
using LethalAPI.Events.Attributes;
using LethalAPI.Events.Patches.HarmonyTools;

using static HarmonyLib.AccessTools;
using AttackInjector = HarmonyTools.Injectors.DeniableEventInjector<EventArgs.Enemies.DeniableEnemyAttackingPlayerEventArgs>;
using EventTranspilerInjector = HarmonyTools.EventTranspilerInjector;

/// <summary>
///     Patches the <see cref="Enemies.DeniableEnemyAttackingPlayer"/> and <see cref="Enemies.DeniableEnemyKillingPlayer"/> event.
/// </summary>
// [HarmonyPatch(typeof(BaboonBirdAI), nameof(BaboonBirdAI.OnCollideWithPlayer))] // damage + kill animation
// [EventPatch(typeof(Handlers.Enemies), nameof(Handlers.Enemies.EnemyAttackingPlayer))]
// [EventPatch(typeof(Handlers.Enemies), nameof(Handlers.Enemies.EnemyKillingPlayer))]
internal static class BaboonBirdOnCollideWithPlayerTranspiler
{
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator, MethodBase original)
    {
        yield break;
        /*
        const bool ShowDebugInfo = true;
        List<CodeInstruction> newInstructions = instructions.ToList();

        int constIndex = newInstructions.FindNthInstruction(1, instruction => instruction.opcode == OpCodes.Stfld);

        int.TryParse(newInstructions[constIndex].operand.ToString(), out int damage);

        LocalBuilder local = generator.DeclareLocal(typeof(AtkArg));
        CodeInstruction[] injectedInstructions =
        {
            new (OpCodes.Ldc_I4_S, damage),
            new (OpCodes.Ldloc_1),
            new (OpCodes.Ldarg_0),
            new (OpCodes.Ldc_I4_1),
            DeniableEventInjector<AtkArg>.Tools.CreateEventArgsObject(),
            DeniableEventInjector<AtkArg>.Tools.CreateEventAction(),
            new (OpCodes.Stloc_S, local),
            new (OpCodes.Ldloc_S, local),
            new (OpCodes.Callvirt, PropertyGetter(typeof(AtkArg), nameof(AtkArg.IsAllowed))),
        };
        AttackInjector attackerInjector = AttackInjector.Create(ref newInstructions, ref generator, original).CreateLocalForEventArg();
        attackerInjector.Goto(index);
        attackerInjector.InjectAt(new CodeInstruction(OpCodes.Ldc_I4_S, damage));
        attackerInjector.InjectAt(new CodeInstruction(OpCodes.Ldloc_1));
        attackerInjector.InjectAt(new CodeInstruction(OpCodes.Ldarg_0));
        attackerInjector.InjectDeniableEvent();

        attackerInjector.Skip(1);
        attackerInjector.Remove();

        attackerInjector.InjectAt(new CodeInstruction(OpCodes.Ldloc_1));
        attackerInjector.InjectAt(new CodeInstruction(OpCodes.Ldloc_S, attackerInjector.LocalEventArg));
        attackerInjector.InjectAt(new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(EnemyAttackingPlayerEventArgs), nameof(EnemyAttackingPlayerEventArgs.Damage))));
        Log.Debug($"[&3Patching {nameof(BaboonBirdOnCollideWithPlayerTranspiler)}&r]", Plugin.Instance.Config.DetailedPatchLogging.Contains(nameof(BaboonBirdOnCollideWithPlayerTranspiler)));
        for (int i = 0; i < newInstructions.Count; i++)
            yield return newInstructions[i].Log(i, -1, Plugin.Instance.Config.DetailedPatchLogging.Contains(nameof(BaboonBirdOnCollideWithPlayerTranspiler)), ShowDebugInfo, attackerInjector.InjectedInstructionIndexes);
    */
    }
}