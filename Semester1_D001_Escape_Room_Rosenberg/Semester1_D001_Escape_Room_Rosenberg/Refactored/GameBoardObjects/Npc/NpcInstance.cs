

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Npc
{
    /// <summary>
    /// Represents an instantiated NPC within the game.
    /// Holds references to its metadata, dialog, and reward data,
    /// as well as its current interaction state.
    /// </summary>
    internal class NpcInstance
    {
        // === Fields ===
        private NpcMetaData _meta;
        private NpcDialogData _dialog;
        private NpcRewardData _reward;
        
        private bool _isActive;
        private bool _hasInteracted;

        /// <summary>
        /// Initializes a new instance of the <see cref="NpcInstance"/> class.
        /// </summary>
        /// <param name="meta">The metadata object containing general information about the NPC.</param>
        /// <param name="dialog">The dialog data object containing all dialogue lines or messages related to this NPC.</param>
        /// <param name="reward">The reward data object that defines what the NPC grants after interaction or quest completion.</param>
        public NpcInstance(NpcMetaData meta, NpcDialogData dialog, NpcRewardData reward)
        {
            _meta = meta;
            _dialog = dialog;
            _reward = reward;
            _isActive = false;
            _hasInteracted = false;
        }

        /// <summary>
        /// Gets the metadata information for this NPC.
        /// </summary>
        public NpcMetaData Meta => _meta;
        /// <summary>
        /// Gets the dialog data associated with this NPC.
        /// </summary>
        public NpcDialogData Dialog => _dialog;
        /// <summary>
        /// Gets the reward data associated with this NPC.
        /// </summary>
        public NpcRewardData Reward => _reward;

        /// <summary>
        /// Indicates whether the NPC is currently active in the game.
        /// </summary>
        public bool IsActive => _isActive;
        /// <summary>
        /// Indicates whether the NPC has already been interacted with by the player.
        /// </summary>
        public bool HasInteracted => _hasInteracted;

        /// <summary>
        /// Activates the NPC, marking it as currently active in the game world.
        /// </summary>
        public void Activate()
        {
            _isActive = true;
        }

        /// <summary>
        /// Deactivates the NPC, marking it as inactive in the game world.
        /// </summary>
        public void Deactivate()
        {
            _isActive = false;
        }

        /// <summary>
        /// Marks the NPC as interacted with by the player.
        /// </summary>
        public void MarkAsInteracted()
        {
            _hasInteracted = true;
        }

        /// <summary>
        /// Resets the interaction state, marking the NPC as not yet interacted with.
        /// </summary>
        public void MarkAsNotInteracted()
        {
            _hasInteracted = false;
        }        
    }
}