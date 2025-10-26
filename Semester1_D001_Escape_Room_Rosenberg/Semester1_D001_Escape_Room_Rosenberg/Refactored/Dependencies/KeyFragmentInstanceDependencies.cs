using Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies
{
    /// <summary>
    ///Provides all required dependencies for initializing and managing a <see cref="KeyFragmentInstance"/>.
    /// </summary>
    /// <remarks>
    /// This dependency record bundles together the <see cref="SymbolsManager"/> and the 
    /// <see cref="DiagnosticsManager"/> to allow consistent visual representation and 
    /// diagnostic logging for key fragment objects within the game board.
    /// </remarks>
    /// <param name="Symbol">
    /// Reference to the <see cref="SymbolsManager"/> providing access to all key fragment symbols.
    /// </param>
    /// <param name="Diagnostic">
    /// Reference to the <see cref="DiagnosticsManager"/> used for validation and event logging.
    /// </param>
    internal sealed record KeyFragmentInstanceDependencies
    (
        SymbolsManager Symbol,
        DiagnosticsManager Diagnostic
    );    
}
