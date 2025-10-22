using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies
{
    /// <summary>
    /// Defines all required dependencies for constructing and managing the game board.
    /// Provides access to the <see cref="PrintManager"/> for visual output
    /// and the <see cref="DiagnosticsManager"/> for logging and system checks.
    /// </summary>
    /// <param name="PrintManager">
    /// Reference to the <see cref="PrintManager"/> responsible for rendering
    /// or printing the game board to the console.
    /// </param>
    /// <param name="DiagnosticsManager">
    /// Reference to the <see cref="DiagnosticsManager"/> responsible for recording
    /// diagnostic messages, warnings, and debug information during board operations.
    /// </param>
    internal sealed record GameBoardBuilderDependencies
    (
        PrintManager  PrintManager,
        DiagnosticsManager  DiagnosticsManager
    );    
}
