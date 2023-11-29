// -----------------------------------------------------------------------
// <copyright file="Config.cs" company="LethalAPI Event Team">
// Copyright (c) LethalAPI Event Team. All rights reserved.
// Licensed under the LGPL-3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace LethalAPI.Events;

using System.ComponentModel;

using LethalAPI.Core.Interfaces;

/// <summary>
/// The main config class.
/// </summary>
// ReSharper disable ClassNeverInstantiated.Global
public class Config : IConfig
{
    /// <inheritdoc />
    [Description("Indicates whether or not the plugin should run.")]
    public bool IsEnabled { get; set; } = true;

    /// <inheritdoc />
    [Description("Indicates whether debug logs should be output.")]
    public bool Debug { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether or not dynamic patching will be used.
    /// </summary>
    [Description("Gets or sets a value indicating whether or not dynamic patching will be used.")]
    public bool UseDynamicPatching { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether or not event execution will be logged.
    /// </summary>
    public bool LogEventExecution { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether or not patching will be logged..
    /// </summary>
    public bool LogEventPatching { get; set; } = false;
}