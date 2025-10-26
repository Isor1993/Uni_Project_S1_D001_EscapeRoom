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
    /// Provides all required dependencies for initializing and managing an <see cref="NpcInstance"/>.
    /// </summary>
    /// <remarks>
    /// This dependency record bundles the <see cref="DiagnosticsManager"/> and <see cref="SymbolsManager"/>  
    /// for use within the <see cref="NpcInstance"/> class.  
    /// It ensures consistent diagnostic logging and symbol configuration across all NPCs.
    /// </remarks>
    /// <param name="Diagnostic">
    /// Reference to the <see cref="DiagnosticsManager"/> used for logging checks, warnings, and errors 
    /// during NPC creation, updates, and interaction events.
    /// </param>
    /// <param name="Symbol">
    /// Reference to the <see cref="SymbolsManager"/> that provides visual symbols 
    /// associated with NPCs (e.g., quest markers or dialogue icons).
    /// </param>
    internal sealed record NpcInstanceDependencies
    (
        DiagnosticsManager Diagnostic,
        SymbolsManager Symbol
    );
}
