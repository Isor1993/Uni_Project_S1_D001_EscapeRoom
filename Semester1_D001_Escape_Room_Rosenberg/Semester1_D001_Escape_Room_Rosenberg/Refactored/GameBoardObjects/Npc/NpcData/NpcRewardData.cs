using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Npc.NpcData
{
    /// <summary>
    /// Represents the reward data associated with a non-player character (NPC).
    /// </summary>
    /// <remarks>
    /// Contains information about the number of key fragments and points 
    /// the player receives after interacting with the NPC or completing their task.
    /// </remarks>
    internal class NpcRewardData
    {
        // === Fields ===
        private int _keyFragment;
        private int _points;

        /// <summary>
        /// nitializes a new instance of the <see cref="NpcRewardData"/> class.
        /// </summary>
        /// <param name="keyFragment">The number of key fragments granted to the player as part of the reward.</param>
        /// <param name="points">The number of points granted to the player as part of the reward.</param>
        public NpcRewardData(int keyFragment, int points)
        {
            _keyFragment = keyFragment;
            _points = points;
        }

        /// <summary>
        /// Gets the number of key fragments the player receives from this NPC.
        /// </summary>
        public int KeyFragment => _keyFragment;

        /// <summary>
        /// Gets the number of points the player receives from this NPC.
        /// </summary>
        public int Points => _points;
    }
}