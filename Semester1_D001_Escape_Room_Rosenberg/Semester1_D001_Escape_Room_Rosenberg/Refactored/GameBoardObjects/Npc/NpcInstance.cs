

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Npc
{
    /// <summary>
    /// Represents an instantiated NPC within the game.
    /// Holds references to its metadata, dialog, and reward data,
    /// as well as its current interaction state.
    /// </summary>
    internal class NpcInstance
    {
        // === Dependencies ===
        private readonly DiagnosticsManager _diagnosticsManager;

        // === Fields ===
        private NpcMetaData _meta;
        private NpcDialogData _dialog;
        private NpcRewardData _reward;

        private bool _isActive;
        private bool _hasInteracted;
        private TileTyp _typ;

        /// <summary>
        /// Initializes a new instance of the <see cref="NpcInstance"/> class.
        /// Creates and links all NPC-related data components, including metadata, dialog, and reward information.
        /// Also registers the creation event in the diagnostics log.
        /// </summary>
        /// <param name="meta">The metadata object containing general information about the NPC (e.g., name, position, symbol).</param>
        /// <param name="dialog">The dialog data object containing all dialogue lines or messages related to this NPC.</param>
        /// <param name="reward">The reward data object that defines what the NPC grants after interaction or quest completion.</param>
        /// <param name="diagnosticsManager">Reference to the diagnostics manager used for logging and system checks.</param>
        public NpcInstance(NpcMetaData meta, NpcDialogData dialog, NpcRewardData reward, DiagnosticsManager diagnosticsManager)
        {
            this._diagnosticsManager = diagnosticsManager;
            this._meta = meta;
            this._dialog = dialog;
            this._reward = reward;
            _isActive = false;
            _hasInteracted = false;
            _typ = TileTyp.Npc;
            _diagnosticsManager.AddCheck($"{nameof(NpcInstance)}: Instance successfully created.");
        }

        /// <summary>
        /// Gets the tile type assigned to this NPC (typically <see cref="TileTyp.Npc"/>).
        /// </summary>
        public TileTyp Typ => _typ;

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
            _diagnosticsManager.AddCheck($"{nameof(NpcInstance)}: NPC {_meta.Name} activated.");
        }

        /// <summary>
        /// Deactivates the NPC, marking it as inactive in the game world.
        /// </summary>
        public void Deactivate()
        {
            _isActive = false;
            _diagnosticsManager.AddCheck($"{nameof(NpcInstance)}: NPC {_meta.Name} deactivated.");
        }

        /// <summary>
        /// Marks the NPC as interacted with by the player.
        /// </summary>
        public void MarkAsInteracted()
        {
            _hasInteracted = true;
            _diagnosticsManager.AddCheck($"{nameof(NpcInstance)}: NPC {_meta.Name} marked as interacted.");
        }

        /// <summary>
        /// Resets the interaction state, marking the NPC as not yet interacted with.
        /// </summary>
        public void MarkAsNotInteracted()
        {
            _hasInteracted = false;
            _diagnosticsManager.AddCheck($"{nameof(NpcInstance)}: NPC {_meta.Name} marked as not interacted.");
        }
    }
}