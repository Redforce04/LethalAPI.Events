// -----------------------------------------------------------------------
// <copyright file="IgnoresAccessChecksToAttribute.cs" company="Redforce04">
// Copyright (c) Redforce04. All rights reserved.
// Licensed under the GPL-3.0 license.
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