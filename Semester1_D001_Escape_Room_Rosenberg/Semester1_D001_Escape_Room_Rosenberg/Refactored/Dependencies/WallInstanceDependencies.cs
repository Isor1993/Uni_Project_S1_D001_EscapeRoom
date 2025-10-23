using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies
{
    /// <summary>
    /// Defines all required dependencies for the <see cref="WallInstance"/> class.
    /// Provides access to the diagnostic and symbol management systems used
    /// for initializing and configuring wall elements on the game board.
    /// </summary>
    /// <param name="DiagnosticsManager">
    /// Reference to the <see cref="DiagnosticsManager"/> responsible for logging
    /// checks, warnings, and system messages during wall setup and validation.
    /// </param>
    /// <param name="SymbolsManager">
    /// Reference to the <see cref="SymbolsManager"/> that provides the visual
    /// and symbolic representations used for wall elements.
    /// </param>
    internal sealed record WallInstanceDependencies
    (
        DiagnosticsManager DiagnosticsManager,
        SymbolsManager SymbolsManager
    );
}
