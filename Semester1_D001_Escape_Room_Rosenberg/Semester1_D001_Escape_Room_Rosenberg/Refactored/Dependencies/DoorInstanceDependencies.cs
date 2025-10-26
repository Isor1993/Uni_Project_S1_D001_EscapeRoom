using Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies
{
    /// <summary>
    /// <summary>
    /// Provides all dependencies required for initializing and managing a <see cref="DoorInstance"/>.
    /// </summary>
    /// <remarks>
    /// The <see cref="DoorInstanceDependencies"/> record bundles the <see cref="SymbolsManager"/> 
    /// and <see cref="DiagnosticsManager"/> together to ensure consistent symbol assignment 
    /// and diagnostic logging across all door operations.
    /// </remarks>
    /// <param name="Symbol">
    /// Reference to the <see cref="SymbolsManager"/> containing all door-related symbols 
    /// (open/closed, horizontal/vertical).
    /// </param>
    /// <param name="Diagnostic">
    /// Reference to the <see cref="DiagnosticsManager"/> responsible for tracking and logging 
    /// all door-related actions and validations.
    /// </param>
    internal sealed record DoorInstanceDependencies
    (
        SymbolsManager Symbol,        
        DiagnosticsManager Diagnostic
    );
}
