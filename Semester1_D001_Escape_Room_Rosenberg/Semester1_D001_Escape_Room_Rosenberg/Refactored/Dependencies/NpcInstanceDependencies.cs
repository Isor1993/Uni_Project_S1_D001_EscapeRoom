using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Npc.NpcData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies
{
    /// <summary>
    /// Defines all required dependencies for the <see cref="NpcInstance"/> class.
    /// Provides access to diagnostic management and NPC data containers,
    /// including metadata, dialogue data, and reward configuration.
    /// </summary>
    /// <param name="DiagnosticsManager">
    /// Reference to the <see cref="DiagnosticsManager"/> responsible for logging
    /// checks, warnings, and messages during NPC initialization and state management.
    /// </param>
    /// <param name="Meta">
    /// Reference to the <see cref="NpcMetaData"/> object containing general information
    /// about the NPC, such as ID, name, and type definitions.
    /// </param>
    /// <param name="Dialog">
    /// Reference to the <see cref="NpcDialogData"/> object containing dialogue lines
    /// and message sequences associated with the NPC.
    /// </param>
    /// <param name="Reward">
    /// Reference to the <see cref="NpcRewardData"/> object that defines the rewards
    /// the NPC grants after interaction or quest completion.
    /// </param>
    internal sealed record NpcInstanceDependencies
    ( 
        DiagnosticsManager DiagnosticsManager,
        NpcMetaData Meta,
        NpcDialogData Dialog,
        NpcRewardData Reward
    );
}
