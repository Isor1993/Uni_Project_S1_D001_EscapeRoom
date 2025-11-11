/*****************************************************************************
* Project : Escape Room (K2, S2)
* File    : NpcRewardData.cs
* Date    : 09.11.2025
* Author  : Eric Rosenberg
*
* Description :
* Defines the reward configuration associated with a non-player character (NPC).
* Stores the number of key fragments and score points the player receives
* after a successful interaction or task completion.
*
* Responsibilities:
* - Store key fragment and score reward values
* - Provide read-only access to NPC reward data
* - Serve as a data container for NpcRawData and NpcInstance
*
* History :
* 09.11.2025 ER Created / Documentation fully updated
******************************************************************************/

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Npc.NpcData
{
    /// <summary>
    /// Represents the reward data associated with a non-player character (NPC).
    /// </summary>
    /// <remarks>
    /// The <see cref="NpcRewardData"/> class contains all reward-related information
    /// for an NPC interaction. It specifies the number of key fragments and
    /// score points awarded to the player.
    /// Used by both <see cref="NpcRawData"/> for data aggregation and
    /// <see cref="GameBoardObjects.Npc.NpcInstance"/> during gameplay.
    /// </remarks>
    internal class NpcRewardData
    {
        // === Fields ===
        private int _keyFragment;

        private int _points;

        /// <summary>
        /// Initializes a new instance of the <see cref="NpcRewardData"/> class.
        /// </summary>
        /// <param name="keyFragment">
        /// The number of key fragments granted to the player as part of the reward.
        /// </param>
        /// <param name="points">
        /// The number of score points granted to the player as part of the reward.
        /// </param>
        public NpcRewardData(int keyFragment, int points)
        {
            _keyFragment = keyFragment;
            _points = points;
        }

        /// <summary>
        /// Gets the number of key fragments awarded by this NPC.
        /// </summary>
        public int KeyFragment => _keyFragment;

        /// <summary>
        /// Gets the number of score points awarded by this NPC.
        /// </summary>
        public int Points => _points;
    }
}