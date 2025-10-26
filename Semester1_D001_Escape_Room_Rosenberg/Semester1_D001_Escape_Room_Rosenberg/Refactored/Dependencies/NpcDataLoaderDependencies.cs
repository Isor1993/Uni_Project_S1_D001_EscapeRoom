using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Npc.NpcData;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies
{
    /// <summary>
    /// Provides all required dependencies for the <see cref="NpcDataLoader"/> system.
    /// </summary>
    /// <remarks>
    /// This dependency record bundles the <see cref="DiagnosticsManager"/> and <see cref="SymbolsManager"/> 
    /// into a single container for dependency injection.  
    /// It allows the NPC data loading process to log validation messages and assign 
    /// correct symbols to NPC-related data such as <see cref="NpcMetaData"/> or <see cref="NpcDialogData"/>.
    /// </remarks>
    /// <param name="Diagnostic">
    /// Reference to the <see cref="DiagnosticsManager"/> used for logging checks, warnings, and errors 
    /// during NPC data parsing and initialization.
    /// </param>
    /// <param name="Symbol">
    /// Reference to the <see cref="SymbolsManager"/> that provides visual symbols 
    /// used in NPC metadata and debugging displays.
    /// </param>
    internal sealed record NpcDataLoaderDependencies
    (
        DiagnosticsManager Diagnostic,
        SymbolsManager Symbol
    );
}
