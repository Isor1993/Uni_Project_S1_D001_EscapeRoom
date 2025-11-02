using Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies
{
    /// <summary>
    /// Contains all dependencies required by the PrintManager
    /// to display the current state of the game board and objects.
    /// </summary>
    internal sealed record PrintManagerDependencies
    (
        GameBoardManager GameBoard,
        GameObjectManager GameObject,
        SymbolsManager Symbol,
        DiagnosticsManager Diagnostic
    );
}
