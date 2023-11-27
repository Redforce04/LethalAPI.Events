// -----------------------------------------------------------------------
// <copyright file="IsExternalInitAttribute.cs" company="Redforce04">
// Copyright (c) Redforce04. All rights reserved.
// Licensed under the GPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

#pragma warning disable
using System.ComponentModel;

// ReSharper disable CheckNamespace
namespace System.Runtime.CompilerServices;

/// <summary>
/// Reserved to be used by the compiler for tracking metadata.
/// This class should not be used by developers in source code.
/// This dummy class is required to compile records when targeting .NET Standard
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
internal static class IsExternalInit;
#pragma warning restore