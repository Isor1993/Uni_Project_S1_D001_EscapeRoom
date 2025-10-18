using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Npc
{
    /// <summary>
    /// Represents the reward data assigned to a non-player character (NPC).
    /// Stores information about the number of key fragments and points granted
    /// to the player after interacting with or completing a task for the NPC.
    /// </summary>
    internal class NpcRewardData
    {
        // === Fields ===
        private int _keyFragment;
        private int _points;

        /// <summary>
        /// Initializes a new instance of the <see cref="NpcRewardData"/> class.
        /// </summary>
        /// <param name="keyFragment">The number of key fragments awarded to the player as part of the NPC’s reward.</param>
        /// <param name="points">The number of points awarded to the player as part of the NPC’s reward.</param>
        public NpcRewardData(int keyFragment, int points)
        {
            _keyFragment = keyFragment;
            _points = points;
        }

        /// <summary>
        /// Gets the number of key fragments the player receives from this NPC.
        /// </summary>
        public int KeyFragment=> _keyFragment;

        /// <summary>
        /// Gets the number of points the player receives from this NPC.
        /// </summary>
        public int Points => _points;        
    }
}