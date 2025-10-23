using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies
{
    /// <summary>
    /// Defines all required dependencies for the <see cref="KeyFragmentInstance"/> class.
    /// Provides access to the symbol management and diagnostic systems used for
    /// initializing and monitoring key fragment instances on the game board.
    /// </summary>
    /// <param name="SymbolsManager">
    /// Reference to the <see cref="SymbolsManager"/> that provides the visual symbol
    /// representing the key fragment within the board layout.
    /// </param>
    /// <param name="DiagnosticsManager">
    /// Reference to the <see cref="DiagnosticsManager"/> responsible for logging
    /// initialization checks, warnings, and validation messages related to key fragments.
    /// </param>
    internal sealed record KeyFragmentInstanceDependencies
    (
        SymbolsManager SymbolsManager,
        DiagnosticsManager DiagnosticsManager
    );    
}
