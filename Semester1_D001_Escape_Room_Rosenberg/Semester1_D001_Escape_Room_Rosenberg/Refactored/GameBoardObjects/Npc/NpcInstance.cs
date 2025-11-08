/*****************************************************************************
* Project : Escape Room (K2, S2)
* File    : NpcInstance.cs
* Date    : 09.11.2025
* Author  : Eric Rosenberg
*
* Description :
* Represents an instantiated non-player character (NPC) within the game world.
* Combines metadata, dialogue, and reward structures, and manages activation,
* interaction, and state changes during gameplay.
*
* Responsibilities:
* - Store all relevant NPC data (meta, dialogue, rewards)
* - Manage activation, interaction, and reset states
* - Provide position assignment and runtime synchronization
* - Log all actions via the DiagnosticsManager
*
* History :
* 09.11.2025 ER Created / Documentation fully updated
******************************************************************************/

using Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Npc.NpcData;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Npc
{
    /// <summary>
    /// Represents an instantiated non-player character (NPC) within the game world.
    /// </summary>
    /// <remarks>
    /// The <see cref="NpcInstance"/> aggregates all key data about an NPC — 
    /// including metadata, dialogue, and rewards — and controls its runtime state 
    /// such as activation and interaction flags.  
    /// Diagnostic logging is integrated for full traceability of NPC behavior.
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
        /// <param name="npcInstanceDependencies">
        /// Injected dependencies required for diagnostics and symbol management.
        /// </param>
        /// <param name="npcMetaData">
        /// Metadata object containing core NPC attributes such as name and position.
        /// </param>
        /// <param name="npcDialogData">
        /// Dialogue object defining the NPC’s questions and possible answers.
        /// </param>
        /// <param name="npcRewardData">
        /// Reward definition that specifies score and key fragment rewards.
        /// </param>
        /// <remarks>
        /// Upon creation, the NPC is set as inactive and un-interacted,
        /// with its tile type defined as <see cref="TileType.Npc"/>.  
        /// A diagnostic entry confirms successful instantiation.
        /// <remarks>
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
        /// Gets the tile type associated with this NPC (always <see cref="TileType.Npc"/>).
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
        /// Activates the NPC, marking it as currently active in the world.
        /// </summary>
        /// <remarks>
        /// Typically called when the player enters interaction range or
        /// initiates a dialogue sequence.
        /// </remarks>
        public void Activate()
        {
            _isActive = true;
            _deps.Diagnostic.AddCheck($"{nameof(NpcInstance)}.{nameof(Activate)}: NPC {_meta.Name} activated.");
        }

        /// <summary>
        /// Deactivates the NPC, marking it as inactive.
        /// </summary>
        /// <remarks>
        /// Typically used when the player leaves range or after dialogue completion.
        /// </remarks>
        public void Deactivate()
        {
            _isActive = false;
            _deps.Diagnostic.AddCheck($"{nameof(NpcInstance)}.{nameof(Deactivate)}: NPC {_meta.Name} deactivated.");
        }

        /// <summary>
        ///Marks the NPC as having been interacted with.
        /// </summary>
        /// <remarks>
        /// Prevents repeated interactions and duplicate rewards after a dialogue event.
        /// </remarks>
        public void MarkAsInteracted()
        {
            _hasInteracted = true;
            _deps.Diagnostic.AddCheck($"{nameof(NpcInstance)}.{nameof(MarkAsInteracted)}: NPC {_meta.Name} marked as interacted.");
        }

        /// <summary>
        /// Resets the NPC’s interaction flag, allowing future interactions.
        /// </summary>
        /// <remarks>
        /// Commonly used during level resets or replay scenarios to restore interaction availability.
        /// </remarks>
        public void MarkAsNotInteracted()
        {
            _hasInteracted = false;
            _deps.Diagnostic.AddCheck($"{nameof(NpcInstance)}.{nameof(MarkAsNotInteracted)}: NPC {_meta.Name} marked as not interacted.");
        }
        /// <summary>
        /// Updates the NPC’s position on the game board.
        /// </summary>
        /// <param name="position">The new position coordinates as (Y, X).</param>
        /// <remarks>
        /// Internally updates the metadata’s position to ensure visual and logical alignment.
        /// </remarks>
        public void AssignPosition((int y, int x) position)
        {
            _meta.AssignPosition(position);
            _deps.Diagnostic.AddCheck($"{nameof(NpcInstance)}.{nameof(AssignPosition)}: NPC {_meta.Name} position updated to {position}.");
        }

        /// <summary>
        ///Resets the NPC’s state to its default configuration.
        /// </summary>
        /// <remarks>
        /// Sets both <see cref="_isActive"/> and <see cref="_hasInteracted"/> to <c>false</c>, 
        /// restoring the NPC to its original inactive state.  
        /// Typically called when restarting or regenerating a level.
        /// </remarks>
        public void ResetState()
        {
            _isActive = false;
            _hasInteracted = false;
            _deps.Diagnostic.AddCheck($"{nameof(NpcInstance)}.{nameof(ResetState)}: NPC {_meta.Name} reset to default state.");
        }
    }
}