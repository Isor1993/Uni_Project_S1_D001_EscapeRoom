

using Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Npc.NpcData;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Npc
{
    /// <summary>
    /// Represents an instantiated non-player character (NPC) within the game.
    /// Holds references to its metadata, dialog, and reward data,
    /// and manages its activation and interaction states during gameplay.
    /// </summary>
    internal class NpcInstance
    {
        // === Dependencies ===
        private readonly NpcInstanceDependencies _instanceDeps;        

        // === Fields ===
        private bool _isActive;
        private bool _hasInteracted;
        private TileTyp _typ;

        /// <summary>
        /// Initializes a new instance of the <see cref="NpcInstance"/> class.
        /// Sets up all required dependencies for NPC initialization,
        /// initializes default state flags, and logs the creation event.
        /// </summary>
        /// <param name="npcInstanceDependencies">
        /// Reference to the <see cref="NpcInstanceDependencies"/> object that provides
        /// the necessary managers and configuration data for NPC setup.
        /// </param>
        public NpcInstance(NpcInstanceDependencies npcInstanceDependencies)
        {
            _instanceDeps=npcInstanceDependencies;            
            _isActive = false;
            _hasInteracted = false;
            _typ = TileTyp.Npc;
            _instanceDeps.DiagnosticsManager.AddCheck($"{nameof(NpcInstance)}: Instance successfully created.");
        }

        /// <summary>
        /// Gets the tile type assigned to this NPC (typically <see cref="TileTyp.Npc"/>).
        /// </summary>
        public TileTyp Typ => _typ;

        /// <summary>
        /// Gets the metadata information for this NPC.
        /// </summary>
        public NpcMetaData Meta => _instanceDeps.Meta;
        /// <summary>
        /// Gets the dialog data associated with this NPC.
        /// </summary>
        public NpcDialogData Dialog => _instanceDeps.Dialog;
        /// <summary>
        /// Gets the reward data associated with this NPC.
        /// </summary>
        public NpcRewardData Reward => _instanceDeps.Reward;

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
            _instanceDeps.DiagnosticsManager.AddCheck($"{nameof(NpcInstance)}: NPC {_instanceDeps.Meta.Name} activated.");
        }

        /// <summary>
        /// Deactivates the NPC, marking it as inactive in the game world.
        /// </summary>
        public void Deactivate()
        {
            _isActive = false;
            _instanceDeps.DiagnosticsManager.AddCheck($"{nameof(NpcInstance)}: NPC {_instanceDeps.Meta.Name} deactivated.");
        }

        /// <summary>
        /// Marks the NPC as interacted with by the player.
        /// </summary>
        public void MarkAsInteracted()
        {
            _hasInteracted = true;
            _instanceDeps.DiagnosticsManager.AddCheck($"{nameof(NpcInstance)}: NPC {_instanceDeps.Meta.Name} marked as interacted.");
        }

        /// <summary>
        /// Resets the interaction state, marking the NPC as not yet interacted with.
        /// </summary>
        public void MarkAsNotInteracted()
        {
            _hasInteracted = false;
            _instanceDeps.DiagnosticsManager.AddCheck($"{nameof(NpcInstance)}: NPC {_instanceDeps.Meta.Name} marked as not interacted.");
        }
        /// <summary>
        /// Updates the NPC’s position on the game board.
        /// </summary>
        /// <param name="position">The new position of the NPC, represented as (y, x) coordinates.</param>
        public void AssignPosition((int y,int x)position)
        {
            _instanceDeps.Meta.AssignPosition(position);
            _instanceDeps.DiagnosticsManager.AddCheck($"{nameof(NpcInstance)}: NPC {_instanceDeps.Meta.Name} position updated to {position}.");
        }
    }
}