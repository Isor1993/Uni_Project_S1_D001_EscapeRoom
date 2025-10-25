using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies
{
    /// <summary>
    /// Defines all required dependencies for constructing and managing the game board.
    /// Provides access to the <see cref="Print"/> for visual output
    /// and the <see cref="Diagnostic"/> for logging and system checks.
    /// </summary>
    /// <param name="Print">
    /// Reference to the <see cref="Print"/> responsible for rendering
    /// or printing the game board to the console.
    /// </param>
    /// <param name="Diagnostic">
    /// Reference to the <see cref="Diagnostic"/> responsible for recording
    /// diagnostic messages, warnings, and debug information during board operations.
    /// </param>
    internal sealed record GameBoardBuilderDependencies
    (
        PrintManager  Print,
        DiagnosticsManager  Diagnostic
    );    
}
