using Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies
{
    /// <summary>
    /// Provides all required dependencies for initializing and operating the <see cref="GameObjectManager"/>.
    /// </summary>
    /// <remarks>
    /// This dependency record bundles together the <see cref="GameBoardManager"/> and 
    /// the <see cref="DiagnosticsManager"/> to ensure synchronized board updates 
    /// and consistent logging for all object-related operations.
    /// </remarks>
    /// <param name="GameBoard">
    /// Reference to the <see cref="GameBoardManager"/> used for tile manipulation 
    /// and maintaining the visual board state.
    /// </param>
    /// <param name="Diagnostic">
    /// Reference to the <see cref="DiagnosticsManager"/> responsible for logging 
    /// checks, warnings, and errors during object registration, movement, and deletion.
    /// </param>
    internal sealed record GameObjectManagerDependencies
    (
        GameBoardManager GameBoard,
        DiagnosticsManager Diagnostic
    );
}
