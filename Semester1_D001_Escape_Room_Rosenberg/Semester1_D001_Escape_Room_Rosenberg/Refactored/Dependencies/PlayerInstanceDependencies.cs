using Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies
{
    /// <summary>
    ///Provides all required dependencies for creating and managing a <see cref="PlayerInstance"/>.
    /// </summary>
    /// <remarks>
    /// This dependency record encapsulates the <see cref="DiagnosticsManager"/> and <see cref="SymbolsManager"/>,  
    /// enabling the <see cref="PlayerInstance"/> to log its state and retrieve visual symbols consistently.  
    /// It serves as a lightweight dependency injection container.
    /// </remarks>
    /// <param name="Diagnostic">
    /// Reference to the <see cref="DiagnosticsManager"/> responsible for logging 
    /// player initialization, symbol changes, and status updates.
    /// </param>
    /// <param name="Symbol">
    /// Reference to the <see cref="SymbolsManager"/> that provides the player's visual character representation.
    /// </param>
    internal sealed record PlayerInstanceDependencies
    (
        DiagnosticsManager Diagnostic,
        SymbolsManager Symbol
    );

}
