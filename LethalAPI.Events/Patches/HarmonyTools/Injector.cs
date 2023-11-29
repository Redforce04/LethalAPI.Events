// -----------------------------------------------------------------------
// <copyright file="Injector.cs" company="LethalAPI Event Team">
// Copyright (c) LethalAPI Event Team. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable MemberCanBePrivate.Global
namespace LethalAPI.Events.Patches.HarmonyTools;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

using Features;
using HarmonyLib;
using Interfaces;

/// <summary>
/// Contains a builder for injecting instructions into transpilers.
/// </summary>
public class Injector : IList<CodeInstruction>, IList, IReadOnlyList<CodeInstruction>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Injector"/> class.
    /// </summary>
    /// <param name="instructions">The pre-existing instructions to patch.</param>
    /// <param name="generator">The generator provided by the transpiler.</param>
    /// <param name="method">The method being patched.</param>
    public Injector(ref IEnumerable<CodeInstruction> instructions, ref ILGenerator generator, MethodBase method)
    {
        this.Instructions = instructions as List<CodeInstruction> ?? instructions.ToList();
        this.Generator = generator;
        this.Method = method;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Injector"/> class.
    /// </summary>
    /// <param name="instructions">The pre-existing instructions to patch.</param>
    /// <param name="generator">The generator provided by the transpiler.</param>
    /// <param name="method">The method being patched.</param>
    public Injector(ref List<CodeInstruction> instructions, ref ILGenerator generator, MethodBase method)
    {
        this.Instructions = instructions;
        this.Generator = generator;
        this.Method = method;
    }

    /// <summary>
    /// Gets a list that stores the type information of the transpiler.
    /// </summary>
    protected static List<TypeInfo> HandlerTypes => typeof(Log).Assembly.DefinedTypes.Where(x => x.FullName?.StartsWith("LethalAPI.Events.Handlers") ?? false).ToList();

    /// <summary>
    /// Gets or sets the index which the next instruction will be injected at.
    /// </summary>
    /// <remarks>This will auto-increment the corresponding amount when any of the builder methods are used. It will always follow the latest instruction by default.</remarks>
    // ReSharper disable once MemberCanBeProtected.Global
    public int IndexToInject { get; protected set; }

    /// <summary>
    /// Gets or sets the amount of instructions which have been injected.
    /// </summary>
    public int AddedInstructionAmount { get; protected set; }

    /// <summary>
    /// Gets or sets the amount of instructions which have been removed.
    /// </summary>
    public int RemovedInstructionAmount { get; protected set; }

    /// <summary>
    /// Gets or sets the instructions to inject into.
    /// </summary>
    protected List<CodeInstruction> Instructions { get; set; }

    /// <summary>
    /// Gets the instance of the ILGenerator to use for generating instructions.
    /// </summary>
    protected ILGenerator Generator { get; }

    /// <summary>
    /// Gets the method being transpiled.
    /// </summary>
    protected MethodBase Method { get; }

    /// <summary>
    /// Moves a label from an index to a new index.
    /// </summary>
    /// <param name="originalInstruction">The index of the original instruction. This should have the label.</param>
    /// <param name="newInstruction">The index of where the instruction where the label will be moved to.</param>
    /// <returns>The current instance of the <see cref="Injector"/>.</returns>
    public virtual Injector MoveLabel(int originalInstruction, int newInstruction)
    {
        Instructions[originalInstruction] = Instructions[originalInstruction].MoveLabelsTo(Instructions[newInstruction]);
        return this;
    }

    /// <summary>
    /// Sets the <see cref="IndexToInject"/>.
    /// </summary>
    /// <param name="index">The new index.</param>
    /// <returns>The current instance of the <see cref="Injector"/>.</returns>
    public virtual Injector SetIndex(int index)
    {
        this.IndexToInject = index;
        return this;
    }

    /// <summary>
    /// Injects instructions at a given index, or the latest index.
    /// </summary>
    /// <param name="instructions">The instuctions to be injected.</param>
    /// <param name="i">The index to inject the instructions.</param>
    /// <returns>The current instance of the <see cref="Injector"/>.</returns>
    public virtual Injector InjectAt(IEnumerable<CodeInstruction> instructions, int i = -1)
    {
        if (i != -1)
        {
            this.IndexToInject = i;
        }

        List<CodeInstruction> codeInstructions = instructions as List<CodeInstruction> ?? instructions.ToList();
        this.Instructions.InsertRange(this.IndexToInject, codeInstructions);
        this.IndexToInject += codeInstructions.Count;
        this.AddedInstructionAmount += codeInstructions.Count;
        return this;
    }

    /// <summary>
    /// Removes instructions at a given index, or the latest index.
    /// </summary>
    /// <param name="amount">The amount of instructions to be removed.</param>
    /// <param name="index">The index to remove the instructions from.</param>
    /// <returns>The current instance of the <see cref="Injector"/>.</returns>
    /// <remarks>If the index is -1, <see cref="IndexToInject"/> will be used instead, and the <see cref="IndexToInject"/> will not be moved.</remarks>
    public virtual Injector Remove(int amount, int index = -1)
    {
        if (index != -1)
        {
            this.IndexToInject = index;
        }

        this.RemovedInstructionAmount += amount;
        this.Instructions.RemoveRange(this.IndexToInject, amount);
        return this;
    }

    /// <summary>
    /// Takes the <see cref="IndexToInject"/> to the index provided.
    /// </summary>
    /// <param name="index">The index to go to.</param>
    /// <returns>The current instance of the <see cref="Injector"/>.</returns>
    public virtual Injector Goto(int index)
    {
        this.IndexToInject = index;
        return this;
    }

    /// <summary>
    /// Jumps the <see cref="IndexToInject"/> by the index provided.
    /// </summary>
    /// <param name="index">The index to go to.</param>
    /// <returns>The current instance of the <see cref="Injector"/>.</returns>
    public virtual Injector Skip(int index)
    {
        this.IndexToInject += index;
        return this;
    }

    /// <summary>
    /// Finds the <see cref="Event{T}"/> property for the given <see cref="ILethalApiEvent"/>.
    /// </summary>
    /// <typeparam name="T">An <see cref="ILethalApiEvent"/> event.</typeparam>
    /// <returns>The <see cref="Event{T}"/> property info for the given <see cref="ILethalApiEvent"/>.</returns>
    protected static PropertyInfo? GetEventPropertyInfo<T>()
        where T : ILethalApiEvent
    {
        return HandlerTypes
            .Select(type => type
                .GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                .FirstOrDefault(pty => pty.PropertyType == typeof(Event<T>)))
            .FirstOrDefault(propertyInfo => propertyInfo is not null);
    }

    /*
     |===========================|
     | List Implementation Stuff |
     |===========================|
     */
    #pragma warning disable SA1201 // property shouldnt follow a method
    #pragma warning disable SA1309 // field should not begin with an underscore.
    /// <inheritdoc />
    public void CopyTo(Array array, int index) => this.Instructions.CopyTo(array.Cast<CodeInstruction>().ToArray(), index);

    /// <inheritdoc cref="ICollection{T}.Count" />
    public int Count => this.Instructions.Count;

    private object? _syncRoot;

    /// <inheritdoc cref="List{T}.InsertRange" />
    public void InsertRange(int index, IEnumerable<CodeInstruction> collection) => Instructions.InsertRange(index, collection);

    /// <inheritdoc cref="List{T}.RemoveRange" />
    public void RemoveRange(int index, int count) => Instructions.RemoveRange(index, count);

    /// <inheritdoc cref="List{T}.RemoveAll" />
    public void RemoveAll(Predicate<CodeInstruction> match) => this.Instructions.RemoveAll(match);

    /// <inheritdoc cref="List{T}.GetRange" />
    public List<CodeInstruction> GetRange(int index, int count) => this.Instructions.GetRange(index, count);

    /// <inheritdoc cref="List{T}.AsReadOnly" />
    public IReadOnlyList<CodeInstruction> AsReadOnly() => this.Instructions.AsReadOnly();

    /// <inheritdoc cref="List{T}.FindLast" />
    public CodeInstruction FindLast(Predicate<CodeInstruction> match) => this.Instructions.FindLast(match);

    /// <inheritdoc cref="List{T}.Find" />
    public CodeInstruction Find(Predicate<CodeInstruction> match) => this.Instructions.Find(match);

    /// <inheritdoc cref="List{T}.FindAll" />
    public List<CodeInstruction> FindAll(Predicate<CodeInstruction> match) => this.Instructions.FindAll(match);

    /// <summary>Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the first occurrence within the entire <see cref="T:System.Collections.Generic.List`1" />.</summary>
    /// <param name="match">The <see cref="T:System.Predicate`1" /> delegate that defines the conditions of the element to search for.</param>
    /// <returns>The zero-based index of the first occurrence of an element that matches the conditions defined by <paramref name="match" />, if found; otherwise, -1.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="match" /> is <see langword="null" />.</exception>
    public int FindIndex(Predicate<CodeInstruction> match) => this.Instructions.FindIndex(match);

    /// <summary>Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the first occurrence within the range of elements in the <see cref="T:System.Collections.Generic.List`1" /> that extends from the specified index to the last element.</summary>
    /// <param name="startIndex">The zero-based starting index of the search.</param>
    /// <param name="match">The <see cref="T:System.Predicate`1" /> delegate that defines the conditions of the element to search for.</param>
    /// <returns>The zero-based index of the first occurrence of an element that matches the conditions defined by <paramref name="match" />, if found; otherwise, -1.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="match" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="startIndex" /> is outside the range of valid indexes for the <see cref="T:System.Collections.Generic.List`1" />.</exception>
    public int FindIndex(int startIndex, Predicate<CodeInstruction> match) => this.Instructions.FindIndex(startIndex, match);

    /// <summary>Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the first occurrence within the range of elements in the <see cref="T:System.Collections.Generic.List`1" /> that starts at the specified index and contains the specified number of elements.</summary>
    /// <param name="startIndex">The zero-based starting index of the search.</param>
    /// <param name="count">The number of elements in the section to search.</param>
    /// <param name="match">The <see cref="T:System.Predicate`1" /> delegate that defines the conditions of the element to search for.</param>
    /// <returns>The zero-based index of the first occurrence of an element that matches the conditions defined by <paramref name="match" />, if found; otherwise, -1.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="match" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///         <paramref name="startIndex" /> is outside the range of valid indexes for the <see cref="T:System.Collections.Generic.List`1" />.
    /// -or-
    /// <paramref name="count" /> is less than 0.
    /// -or-
    /// <paramref name="startIndex" /> and <paramref name="count" /> do not specify a valid section in the <see cref="T:System.Collections.Generic.List`1" />.</exception>
    public int FindIndex(int startIndex, int count, Predicate<CodeInstruction> match) => this.Instructions.FindIndex(startIndex, count, match);

    /// <summary>Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the last occurrence within the entire <see cref="T:System.Collections.Generic.List`1" />.</summary>
    /// <param name="match">The <see cref="T:System.Predicate`1" /> delegate that defines the conditions of the element to search for.</param>
    /// <returns>The zero-based index of the last occurrence of an element that matches the conditions defined by <paramref name="match" />, if found; otherwise, -1.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="match" /> is <see langword="null" />.</exception>
    public int FindLastIndex(Predicate<CodeInstruction> match) => this.Instructions.FindLastIndex(match);

    /// <summary>Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the last occurrence within the range of elements in the <see cref="T:System.Collections.Generic.List`1" /> that extends from the first element to the specified index.</summary>
    /// <param name="startIndex">The zero-based starting index of the backward search.</param>
    /// <param name="match">The <see cref="T:System.Predicate`1" /> delegate that defines the conditions of the element to search for.</param>
    /// <returns>The zero-based index of the last occurrence of an element that matches the conditions defined by <paramref name="match" />, if found; otherwise, -1.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="match" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="startIndex" /> is outside the range of valid indexes for the <see cref="T:System.Collections.Generic.List`1" />.</exception>
    public int FindLastIndex(int startIndex, Predicate<CodeInstruction> match) => this.Instructions.FindLastIndex(startIndex, match);

    /// <summary>Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the last occurrence within the range of elements in the <see cref="T:System.Collections.Generic.List`1" /> that contains the specified number of elements and ends at the specified index.</summary>
    /// <param name="startIndex">The zero-based starting index of the backward search.</param>
    /// <param name="count">The number of elements in the section to search.</param>
    /// <param name="match">The <see cref="T:System.Predicate`1" /> delegate that defines the conditions of the element to search for.</param>
    /// <returns>The zero-based index of the last occurrence of an element that matches the conditions defined by <paramref name="match" />, if found; otherwise, -1.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="match" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///         <paramref name="startIndex" /> is outside the range of valid indexes for the <see cref="T:System.Collections.Generic.List`1" />.
    /// -or-
    /// <paramref name="count" /> is less than 0.
    /// -or-
    /// <paramref name="startIndex" /> and <paramref name="count" /> do not specify a valid section in the <see cref="T:System.Collections.Generic.List`1" />.</exception>
    public int FindLastIndex(int startIndex, int count, Predicate<CodeInstruction> match) => this.Instructions.FindLastIndex(startIndex, count, match);

    /// <inheritdoc />
    public object SyncRoot
    {
        get
        {
            if (this._syncRoot == null)
                System.Threading.Interlocked.CompareExchange<object>(ref this._syncRoot!, new object(), null!);
            return this._syncRoot;
        }
    }

    /// <inheritdoc />
    public bool IsSynchronized => false;

    /// <inheritdoc cref="ICollection{T}.IsReadOnly" />
    public bool IsReadOnly => false;

    /// <inheritdoc />
    public bool IsFixedSize => false;

    /// <inheritdoc cref="IList{T}.this" />
    public CodeInstruction this[int index]
    {
        get => Instructions[index];
        set => Instructions[index] = value;
    }

    /// <inheritdoc />
    public void Add(CodeInstruction item) => Instructions.Add(item);

    /// <inheritdoc />
    public IEnumerator<CodeInstruction> GetEnumerator() => Instructions.GetEnumerator();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => Instructions.GetEnumerator();

    /// <inheritdoc />
    public int Add(object? value)
    {
        if (value is not CodeInstruction instr)
            return -1;

        Instructions.Add(instr);
        return Instructions.Count - 1;
    }

    /// <inheritdoc />
    public bool Contains(object? value)
    {
        if (value is not CodeInstruction instr)
            return false;

        return Instructions.Contains(instr);
    }

    /// <inheritdoc cref="ICollection{T}.Clear" />
    public void Clear() => Instructions.Clear();

    /// <inheritdoc />
    public int IndexOf(object value)
    {
        if (value is not CodeInstruction instr)
            return -1;

        return Instructions.IndexOf(instr);
    }

    /// <inheritdoc />
    public void Insert(int index, object value)
    {
        if (value is not CodeInstruction instr)
            return;

        Instructions.Insert(index, instr);
    }

    /// <inheritdoc />
    public void Remove(object value)
    {
        if (value is not CodeInstruction instr)
            return;

        Instructions.Remove(instr);
    }

    /// <inheritdoc />
    public bool Contains(CodeInstruction item) => Instructions.Contains(item);

    /// <inheritdoc />
    public void CopyTo(CodeInstruction[] array, int arrayIndex) => Instructions.CopyTo(array, arrayIndex);

    /// <inheritdoc />
    public bool Remove(CodeInstruction item) => Instructions.Remove(item);

    /// <inheritdoc />
    public int IndexOf(CodeInstruction item) => this.Instructions.IndexOf(item);

    /// <inheritdoc />
    public void Insert(int index, CodeInstruction item) => Instructions.Insert(index, item);

    /// <inheritdoc cref="IList{T}.RemoveAt" />
    public void RemoveAt(int index) => Instructions.RemoveAt(index);

    /// <inheritdoc/>
    object IList.this[int index]
    {
        get => Instructions[index];
        set
        {
            if (value is not CodeInstruction instr)
                return;

            Instructions[index] = instr;
        }
    }
}