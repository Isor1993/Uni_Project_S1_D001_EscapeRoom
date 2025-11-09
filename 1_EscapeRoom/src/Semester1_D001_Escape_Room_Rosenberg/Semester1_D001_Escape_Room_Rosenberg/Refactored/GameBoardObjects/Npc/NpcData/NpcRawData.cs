/*****************************************************************************
* Project : Escape Room (K2, S2)
* File    : NpcRawData.cs
* Date    : 09.11.2025
* Author  : Eric Rosenberg
*
* Description :
* Defines a unified data record that aggregates all information about a 
* non-player character (NPC) — including metadata, dialogue configuration, 
* and reward definition.  
* Serves as a single, immutable data package for NPC initialization.
*
* Responsibilities:
* - Combine all core NPC components into one immutable structure
* - Provide a clean data interface for loaders and managers
* - Maintain strict separation between raw data and runtime logic
*
* History :
* 09.11.2025 ER Created / Documentation fully updated
******************************************************************************/

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Npc.NpcData
{
    /// <summary>
    /// Represents a complete data container for a non-player character (NPC).
    /// </summary>
    /// <remarks>
    /// The <see cref="NpcRawData"/> record combines three essential NPC components:  
    /// <list type="bullet">
    /// <item><see cref="NpcMetaData"/> — defines the NPC’s name, position, and symbol</item>
    /// <item><see cref="NpcDialogData"/> — stores question and answer configuration</item>
    /// <item><see cref="NpcRewardData"/> — specifies rewards given after interaction</item>
    /// </list>
    /// This record is typically created by <see cref="NpcDataLoader"/> and passed to
    /// <see cref="Managers.NpcManager"/> for NPC registration and instantiation.
    /// </remarks>
    /// <param name="Meta">
    /// The metadata describing the NPC’s identity and board position.
    /// </param>
    /// <param name="Dialog">
    /// The dialogue data defining questions, correct answers, and possible options.
    /// </param>
    /// <param name="Reward">
    /// The reward definition specifying the score and key fragments granted upon interaction.
    /// </param>
    internal sealed record NpcRawData
    (
        NpcMetaData Meta,
        NpcDialogData Dialog,
        NpcRewardData Reward
    );
}