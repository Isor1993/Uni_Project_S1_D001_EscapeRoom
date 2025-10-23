using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Npc.NpcData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies
{
    /// <summary>
    /// Defines all required dependencies for the <see cref="NpcDataLoader"/> class.
    /// Provides access to diagnostic and symbol management, as well as all data
    /// structures needed to initialize and configure NPC instances, including
    /// metadata, dialogue, and reward information.
    /// </summary>
    /// <param name="DiagnosticsManager">
    /// Reference to the <see cref="DiagnosticsManager"/> responsible for logging
    /// checks, warnings, and system messages during NPC data loading and validation.
    /// </param>
    /// <param name="SymbolsManager">
    /// Reference to the <see cref="SymbolsManager"/> providing the visual symbols
    /// and identifiers used to represent NPCs in the game environment.
    /// </param>
    /// <param name="Meta">
    /// Reference to the <see cref="NpcMetaData"/> object containing static NPC
    /// information such as identifiers, categories, and behavioral properties.
    /// </param>
    /// <param name="Dialog">
    /// Reference to the <see cref="NpcDialogData"/> object containing dialogue lines,
    /// sequences, and interaction messages associated with NPCs.
    /// </param>
    /// <param name="Reward">
    /// Reference to the <see cref="NpcRewardData"/> object that defines the rewards
    /// granted by NPCs upon interaction or quest completion.
    /// </param>
    /// <param name="NpcInstanceDependencies">
    /// Reference to the <see cref="NpcInstanceDependencies"/> that bundles together
    /// all subordinate dependencies required for NPC instance creation and linkage.
    /// </param>
    internal sealed record NpcDataLoaderDependencies
    (
        DiagnosticsManager DiagnosticsManager,
        SymbolsManager SymbolsManager,
        NpcMetaData Meta,
        NpcDialogData Dialog,
        NpcRewardData Reward,
        NpcInstanceDependencies NpcInstanceDependencies
    );    
}
