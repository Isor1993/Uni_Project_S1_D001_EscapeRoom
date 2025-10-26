using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Npc;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies
{
    /// <summary>
    /// Provides all required dependencies for the <see cref="NpcManager"/> system.
    /// </summary>
    /// <remarks>
    /// This dependency record bundles together the <see cref="NpcDataLoader"/>, 
    /// <see cref="DiagnosticsManager"/>, and <see cref="SymbolsManager"/> 
    /// to ensure unified access to data loading, diagnostics logging, 
    /// and symbol management during NPC initialization and runtime.
    /// </remarks>
    /// <param name="NpcDataLoader">
    /// Reference to the <see cref="NpcDataLoader"/> responsible for reading and parsing NPC data files.
    /// </param>
    /// <param name="Diagnostic">
    /// Reference to the <see cref="DiagnosticsManager"/> used for logging system events and validations.
    /// </param>
    /// <param name="Symbol">
    /// Reference to the <see cref="SymbolsManager"/> that manages the visual symbols 
    /// associated with NPCs and their UI representations.
    /// </param>
    internal sealed record NpcManagerDependencies
    (
        NpcDataLoader NpcDataLoader,
        DiagnosticsManager Diagnostic,
        SymbolsManager Symbol
    );
}
