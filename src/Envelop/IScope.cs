using System;
using System.Collections;
using System.Collections.Generic;

namespace Envelop
{
    /// <summary>
    /// This interface defines the scope of the lifetime of any associated activations.
    /// 
    /// Disposing the scope will deactivate any associated activations
    /// </summary>
    public interface IScope : IResolver,  IDisposable
    {
        /// <summary>
        /// Gets the parent scope or null for the root scope.
        /// </summary>
        /// <value>The parent.</value>
        IScope Parent { get; }
        
        /// <summary>
        /// Adds an activation to the scope.
        /// </summary>
        /// <param name="activation">The activation.</param>
        void AddActivation(IActivation activation);

        /// <summary>
        /// Creates a child scope.
        /// </summary>
        /// <returns>The scope.</returns>
        IScope CreateScope ();
    }
}