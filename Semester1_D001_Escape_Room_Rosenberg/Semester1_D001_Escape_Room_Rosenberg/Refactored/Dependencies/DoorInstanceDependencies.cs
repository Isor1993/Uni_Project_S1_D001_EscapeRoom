using Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies
{
    /// <summary>
    /// Defines all required dependencies for the <see cref="DoorInstance"/> class.
    /// Provides access to the symbol manager for door visuals, the game board
    /// structure for spatial context, and the diagnostics manager for runtime
    /// logging and validation.
    /// </summary>
    /// <param name="Symbol">
    /// Reference to the <see cref="Symbol"/> responsible for providing
    /// door-related visual symbols and graphical representations used in the board layout.
    /// </param>
    /// <param name="GameBoard">
    /// Reference to the <see cref="GameBoardManager"/> that manages the logical
    /// structure and dimensions of the game board where door instances are placed.
    /// </param>
    /// <param name="Diagnostic">
    /// Reference to the <see cref="Diagnostic"/> used to record creation
    /// checks, warnings, and status messages during door initialization.
    /// </param>
    internal sealed record DoorInstanceDependencies
    (
        SymbolsManager Symbol,
        GameBoardManager GameBoard,
        DiagnosticsManager Diagnostic
    );
}
