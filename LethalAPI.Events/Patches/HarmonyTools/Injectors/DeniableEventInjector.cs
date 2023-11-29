// -----------------------------------------------------------------------
// <copyright file="DeniableEventInjector.cs" company="LethalAPI Event Team">
// Copyright (c) LethalAPI Event Team. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable MemberCanBePrivate.Global
namespace LethalAPI.Events.Patches.HarmonyTools.Injectors;

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

using Features;
using HarmonyLib;
using Interfaces;

using static HarmonyLib.AccessTools;

#pragma warning disable SA1648 // prevent inherit-doc for inheriting class.
/// <summary>
/// A code injector for deniable events.
/// </summary>
/// <typeparam name="T">The type of deniable event to inject.</typeparam>
public class DeniableEventInjector<T> : Injector
    where T : IDeniableEvent
{
    /// <summary>
    /// Indicates whether or not constructor parameters should be auto inserted.
    /// </summary>
    private bool autoInsertConstructorParameters = true;

    /// <summary>
    /// Indicates whether or not to create local of the event arg for later reference.
    /// </summary>
    private bool createLocalEventArg;

    /// <inheritdoc />
    public DeniableEventInjector(ref IEnumerable<CodeInstruction> instructions, ref ILGenerator generator, MethodBase method)
        : base(ref instructions, ref generator, method)
    {
    }

    /// <inheritdoc />
    public DeniableEventInjector(ref List<CodeInstruction> instructions, ref ILGenerator generator, MethodBase method)
        : base(ref instructions, ref generator, method)
    {
    }

    /// <summary>
    /// Gets the local event arg that is made for the event.
    /// </summary>
    /// <remarks>You must call <see cref="CreateLocalForEventArg"/> before injecting, or else this will be null.</remarks>
    /// <returns>The instance of the local that is made.</returns>
    public LocalBuilder? LocalEventArg { get; private set; }

    /// <summary>
    /// Creates a new instance of <see cref="DeniableEventInjector{T}"/>.
    /// </summary>
    /// <param name="instructions">The instructions to use.</param>
    /// <param name="generator">The generator that was provided.</param>
    /// <param name="method">The method that was provided.</param>
    /// <returns>The instance of the <see cref="DeniableEventInjector{T}"/> that is created.</returns>
    public static DeniableEventInjector<T> Create(ref List<CodeInstruction> instructions, ref ILGenerator generator, MethodBase method) => new(ref instructions, ref generator, method);

    /// <summary>
    /// Creates a new instance of <see cref="DeniableEventInjector{T}"/>.
    /// </summary>
    /// <param name="instructions">The instructions to use.</param>
    /// <param name="generator">The generator that was provided.</param>
    /// <param name="method">The method that was provided.</param>
    /// <returns>The instance of the <see cref="DeniableEventInjector{T}"/> that is created.</returns>
    public static DeniableEventInjector<T> Create(ref IEnumerable<CodeInstruction> instructions, ref ILGenerator generator, MethodBase method) => new(ref instructions, ref generator, method);

    /// <summary>
    /// Injects the deniable event.
    /// </summary>
    /// <returns>The current instance of the <see cref="Injector"/>.</returns>
    public DeniableEventInjector<T> Inject()
    {
        PropertyInfo? propertyInfo = GetEventPropertyInfo<T>();

        if (propertyInfo is null)
        {
            Log.Warn($"Could not find an acceptable event handler for event {typeof(T).FullName}! No injection will occur for this event.");
            return this;
        }

        CodeInstruction originalInstruction = this.Instructions[this.IndexToInject];

        List<CodeInstruction> parameterStack = this.autoInsertConstructorParameters
            ? Tools.CreateEventParameters(this.Method)
            : new List<CodeInstruction>();

        Label rtn = this.Generator.DefineLabel();

        List<CodeInstruction> opcodes = new()
        {
            parameterStack,
            Tools.CreateEventArgsObject(),
            new(OpCodes.Dup), /* if(createLocalEventArg)
            new(OpCodes.Dup),
            new(OpCodes.Stloc_S, this.LocalEventArg), */
            Tools.CreateEventAction(),
            Tools.CreateEventDenyReturn(rtn),
        };

        if (createLocalEventArg)
        {
            this.LocalEventArg = this.Generator.DeclareLocal(typeof(T));
            opcodes.InsertRange(3, new CodeInstruction[]
            {
                new (OpCodes.Dup),
                new (OpCodes.Stloc_S, this.LocalEventArg),
            });
        }

        this.Instructions.InsertRange(this.IndexToInject, opcodes);
        this.Instructions[this.Instructions.Count - 1].WithLabels(rtn);

        if (originalInstruction.labels.Count > 0)
        {
            this.Instructions[this.IndexToInject].labels.AddRange(originalInstruction.labels);
            originalInstruction.labels.Clear();
        }

        return this;
    }

    /// <summary>
    /// Indicates whether or not constructor parameters should be auto inserted.
    /// </summary>
    /// <param name="shouldAutoInsert">Indicates whether or not to auto insert constructor parameters.</param>
    /// <returns>The current instance of the <see cref="Injector"/>.</returns>
    public DeniableEventInjector<T> AutoInsertConstructorParameters(bool shouldAutoInsert)
    {
        this.autoInsertConstructorParameters = shouldAutoInsert;
        return this;
    }

    /// <summary>
    /// Indicates whether or not to create local of the event arg for later reference.
    /// </summary>
    /// <param name="shouldCreateLocal">Indicates whether or not to create local of the event arg.</param>
    /// <returns>The current instance of the <see cref="Injector"/>.</returns>
    public DeniableEventInjector<T> CreateLocalForEventArg(bool shouldCreateLocal)
    {
        this.createLocalEventArg = shouldCreateLocal;
        return this;
    }

    /// <summary>
    /// Contains a set of tools for injecting events.
    /// </summary>
    public static class Tools
    {
        /// <summary>
        /// Create instructions which return if the given <see cref="IDeniableEvent"/> is not allowed.
        /// </summary>
        /// <param name="ret">The return label to branch to.</param>
        /// <returns>A list of <see cref="CodeInstruction"/>s that return if the event is not allowed.</returns>
        public static IEnumerable<CodeInstruction> CreateEventDenyReturn(Label ret)
        {
            return new List<CodeInstruction>
            {
                new(OpCodes.Callvirt, PropertyGetter(typeof(T), nameof(IDeniableEvent.IsAllowed))), new(OpCodes.Brfalse, ret),
            };
        }

        /// <summary>
        /// Create instructions to construct the given <see cref="ILethalApiEvent"/>.
        /// </summary>
        /// <returns>A <see cref="CodeInstruction"/> that constructs the event.</returns>
        public static CodeInstruction CreateEventArgsObject()
        {
            return new CodeInstruction(OpCodes.Newobj, GetDeclaredConstructors(typeof(T))[0]);
        }

        /// <summary>
        /// Create instruction to call the delegate for the given <see cref="ILethalApiEvent"/>.
        /// </summary>
        /// <returns>A <see cref="CodeInstruction"/> that calls the event delegate.</returns>
        /// <exception cref="Exception">Thrown if the event delegate could not be found.</exception>
        public static CodeInstruction CreateEventAction()
        {
            PropertyInfo? propertyInfo = GetEventPropertyInfo<T>();
            if (propertyInfo?.GetValue(null) is not Event<T> @event)
            {
                throw new Exception($"Failed to get event {typeof(T).Name}!");
            }

            return Transpilers.EmitDelegate((Action<T>)Action);

            void Action(T eventArgs) => @event.InvokeSafely(eventArgs);
        }

        /// <summary>
        /// Creates instructions to load event parameters for the given <see cref="ILethalApiEvent"/>.
        /// </summary>
        /// <param name="originalMethod">The original method being transpiled.</param>
        /// <returns>A list of <see cref="CodeInstruction"/>s that load the event parameters.</returns>
        public static List<CodeInstruction> CreateEventParameters(MethodBase originalMethod)
        {
            ParameterInfo[] originalMethodParameters = originalMethod.GetParameters();
            ParameterInfo[] eventConstructorParameters = GetDeclaredConstructors(typeof(T))[0].GetParameters();

            List<CodeInstruction> parameterStack = new();
            for (int i = 0; i < eventConstructorParameters.Length; i++)
            {
                ParameterInfo parameter = eventConstructorParameters[i];

                if (i == 0 && parameter.ParameterType == originalMethod.DeclaringType && !originalMethod.IsStatic)
                {
                    parameterStack.Insert(parameterStack.Count, new CodeInstruction(OpCodes.Ldarg_0));
                    continue;
                }

                if (i == eventConstructorParameters.Length - 1 && parameter.ParameterType == typeof(bool))
                {
                    parameterStack.Insert(parameterStack.Count, new CodeInstruction(OpCodes.Ldc_I4_1));
                    continue;
                }

                for (int j = 0; j < originalMethodParameters.Length; j++)
                {
                    ParameterInfo originalMethodParameter = originalMethodParameters[j];
                    if (originalMethodParameter.ParameterType != parameter.ParameterType ||
                        originalMethodParameter.Name != parameter.Name)
                    {
                        continue;
                    }

                    CodeInstruction instruction = (j + (originalMethod.IsStatic ? 0 : 1)) switch
                    {
                        0 => new CodeInstruction(OpCodes.Ldarg_0),
                        1 => new CodeInstruction(OpCodes.Ldarg_1),
                        2 => new CodeInstruction(OpCodes.Ldarg_2),
                        3 => new CodeInstruction(OpCodes.Ldarg_3),
                        var n => new CodeInstruction(OpCodes.Ldarg_S, n),
                    };

                    parameterStack.Insert(parameterStack.Count, instruction);
                }
            }

            return parameterStack;
        }
    }
}