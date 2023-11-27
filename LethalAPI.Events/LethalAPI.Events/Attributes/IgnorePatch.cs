// -----------------------------------------------------------------------
// <copyright file="IgnorePatch.cs" company="Redforce04">
// Copyright (c) Redforce04. All rights reserved.
// Licensed under the GPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Events.Attributes;

using System;

/// <summary>
/// Indicates to the <see cref="Features.Patcher"/> to not patch a class.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
internal sealed class IgnorePatch : Attribute;