using Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies
{
    /// <summary>
    /// Represents the collection of core dependencies required by the <see cref="UIManager"/>.
    /// </summary>
    /// <remarks>
    /// This record is used to bundle together all manager references that the <see cref="UIManager"/> 
    /// needs to operate. It ensures clean dependency injection and clear separation of concerns 
    /// between system components.
    /// </remarks>
    /// <param name="GameBoard">
    /// Provides access to the current game board structure and manages tile or object placement.
    /// </param>
    /// <param name="Symbol">
    /// Handles all symbol-related logic, including symbol rendering and symbol-to-object mapping.
    /// </param>
    /// <param name="Print">
    /// Responsible for formatted text output and debugging information within the UI.
    /// </param>
    /// <param name="Random">
    /// Supplies controlled randomization logic for UI-related features, such as randomized hints or messages.
    /// </param>
    internal sealed record UIManagerDependencies
    (
        GameBoardManager GameBoard,
        DiagnosticsManager Diagnostic,
        SymbolsManager Symbol,
        PrintManager Print,
        RandomManager Random        
    );   
}
