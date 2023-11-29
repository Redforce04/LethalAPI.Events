﻿// -----------------------------------------------------------------------
// <copyright file="IgnoresAccessChecksToAttribute.cs" company="LethalAPI Event Team">
// Copyright (c) LethalAPI Event Team. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

// ReSharper disable once CheckNamespace
namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public sealed class IgnoresAccessChecksToAttribute : Attribute
{
    // ReSharper disable once UnusedParameter.Local
    public IgnoresAccessChecksToAttribute(string assemblyName)
    {
    }
}