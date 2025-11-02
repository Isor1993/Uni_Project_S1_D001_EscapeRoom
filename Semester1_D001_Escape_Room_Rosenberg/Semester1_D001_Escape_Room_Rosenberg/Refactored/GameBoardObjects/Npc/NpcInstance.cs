

using Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Npc.NpcData;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Npc
{
    /// <summary>
    ///  Represents an instantiated non-player character (NPC) within the game world.
    /// </summary>
    /// <remarks>
    /// The <see cref="NpcInstance"/> class combines all relevant NPC data — metadata, dialogue, and rewards — 
    /// and manages the NPC’s activation, interaction, and position states during gameplay.  
    /// It interacts closely with the <see cref="NpcInstanceDependencies"/> for diagnostics and configuration.
    /// </remarks>
    internal class NpcInstance
    {
        // === Dependencies ===
        private readonly NpcInstanceDependencies _deps;
        private readonly NpcMetaData _meta;
        private readonly NpcDialogData _dialog;
        private readonly NpcRewardData _reward;

        // === Fields ===
        private bool _isActive;
        private bool _hasInteracted;
        private TileType _typ;

        /// <summary>
        /// Initializes a new instance of the <see cref="NpcInstance"/> class.
        /// </summary>
        /// <param name="npcInstanceDependencies">Injected dependencies required for initialization and diagnostics.</param>
        /// <param name="npcMetaData">The <see cref="NpcMetaData"/> object containing basic information like name and position.</param>
        /// <param name="npcDialogData">The <see cref="NpcDialogData"/> object containing the NPC’s dialogue and answer sets.</param>
        /// <param name="npcRewardData">The <see cref="NpcRewardData"/> object defining the rewards associated with this NPC.</param>
        /// <remarks>
        /// Upon creation, the NPC is initialized as inactive and un-interacted, with its tile type set to <see cref="TileType.Npc"/>.  
        /// A diagnostic entry is automatically logged confirming successful creation.
        /// </remarks>
        public NpcInstance(NpcInstanceDependencies npcInstanceDependencies, NpcMetaData npcMetaData, NpcDialogData npcDialogData, NpcRewardData npcRewardData)
        {
            _deps = npcInstanceDependencies;
            _meta = npcMetaData;
            _dialog = npcDialogData;
            _reward = npcRewardData;
            _isActive = false;
            _hasInteracted = false;
            
            _typ = TileType.Npc;
            _deps.Diagnostic.AddCheck($"{nameof(NpcInstance)}: Instance successfully created.");
        }

        /// <summary>
        /// Gets the tile type assigned to this NPC (typically <see cref="TileType.Npc"/>).
        /// </summary>
        public TileType Typ => _typ;

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
        /// Activates the NPC, marking it as currently active within the game world.
        /// </summary>
        /// <remarks>
        /// Typically called when the player enters the NPC’s interaction range 
        /// or when dialogue becomes available.
        public void Activate()
        {
            _isActive = true;
            _deps.Diagnostic.AddCheck($"{nameof(NpcInstance)}: NPC {_meta.Name} activated.");
        }

        /// <summary>
        /// Deactivates the NPC, marking it as inactive in the game world.
        /// </summary>
        /// <remarks>
        /// Often used when the player moves away or the dialogue sequence ends.
        /// </remarks>
        public void Deactivate()
        {
            _isActive = false;
            _deps.Diagnostic.AddCheck($"{nameof(NpcInstance)}: NPC {_meta.Name} deactivated.");
        }

        /// <summary>
        /// Marks the NPC as having been interacted with by the player.
        /// </summary>
        /// <remarks>
        /// Used to prevent duplicate interactions or reward duplication after a dialogue event.
        /// </remarks>
        public void MarkAsInteracted()
        {
            _hasInteracted = true;
            _deps.Diagnostic.AddCheck($"{nameof(NpcInstance)}: NPC {_meta.Name} marked as interacted.");
        }

        /// <summary>
        /// Resets the interaction flag, marking the NPC as not yet interacted with.
        /// </summary>
        /// <remarks>
        /// This can be used during level resets or when replaying a scene 
        /// to allow renewed interactions.
        /// </remarks>
        public void MarkAsNotInteracted()
        {
            _hasInteracted = false;
            _deps.Diagnostic.AddCheck($"{nameof(NpcInstance)}: NPC {_meta.Name} marked as not interacted.");
        }
        /// <summary>
        /// Updates the NPC’s position on the game board.
        /// </summary>
        /// <param name="position">The new position of the NPC, represented as (Y, X) coordinates.</param>
        /// <remarks>
        /// Internally calls <see cref="NpcMetaData.AssignPosition((int, int))"/> to update the metadata.  
        /// This ensures the visual and logical position stay synchronized.
        /// </remarks>
        public void AssignPosition((int y, int x) position)
        {
            _meta.AssignPosition(position);
            _deps.Diagnostic.AddCheck($"{nameof(NpcInstance)}: NPC {_meta.Name} position updated to {position}.");
        }

        /// <summary>
        /// Resets the NPC’s runtime state to its default configuration.
        /// </summary>
        /// <remarks>
        /// Sets <see cref="_isActive"/> and <see cref="_hasInteracted"/> to <c>false</c>,  
        /// effectively restoring the NPC to an untouched, inactive state.  
        /// This is typically called when restarting a level or rebuilding the game board.
        /// </remarks>
        public void ResetState()
        {
            _isActive = false;
            _hasInteracted = false;
            _deps.Diagnostic.AddCheck($"{nameof(NpcInstance)}: NPC {_meta.Name} reset to default state.");
        }
    }
}