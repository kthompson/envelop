﻿using System;
using System.Collections.Generic;
using System.Reflection;

namespace Envelop
{
    /// <summary>
    /// <c>IKernel</c> interface for exposing basic binding and type resolution functions
    /// </summary>
    public interface IKernel : IBuilder, IResolver, IDisposable
    {
        ///// <summary>
        ///// Loads the modules at the specified file paths.
        ///// </summary>
        ///// <param name="filePaths">The file paths.</param>
        ////TODO: PCL void Load(params string[] filePaths);

        /// <summary>
        /// Loads modules from the specified assemblies.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        void Load(params Assembly[] assemblies);
        /// <summary>
        /// Loads the specified modules.
        /// </summary>
        /// <param name="modules">The modules.</param>
        void Load(params IModule[] modules);

        /// <summary>
        /// Creates a child scope.
        /// </summary>
        /// <returns>The scope.</returns>
        IScope CreateScope();

        ///// <summary>
        ///// Automatically registers all interfaces.
        ///// </summary>
        //TODO: PCL void AutoRegister();

        /// <summary>
        /// Automatically registers all interfaces.
        /// </summary>
        void AutoRegister(IEnumerable<Assembly> assemblies);
    }
}