using Microsoft.VisualBasic;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Door;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Key;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semester1_D001_Escape_Room_Rosenberg
{
    /// <summary>
    /// 
    /// </summary>
    enum InteractionType
    {
        None = 0,
        Npc,
        Key,
        Door,
        Quit

    }
    /// <summary>
    /// 
    /// </summary>
    internal class InteractionManager
    {
        // === Dependencies ===
        readonly InteractionManagerDependencies _deps;

        // === Fields ===


        public InteractionManager(InteractionManagerDependencies interactionManagerDependencies)
        {
            _deps = interactionManagerDependencies;
            _deps.Diagnostic.AddCheck($"{nameof(InteractionManager)}: Initialized successfully.");
        }

        public void HandleNpc((int y, int x) targetPosition)
        {

        }

        /// <summary>
        /// Handles the pickup of a key fragment object at the specified target position.
        /// </summary>
        /// <remarks>
        /// This method retrieves a <see cref="KeyFragmentInstance"/> from the <see cref="GameObjectManager"/> 
        /// at the given target position.  
        /// If a key fragment is found, it updates the player's inventory and removes the key object 
        /// from the game board.  
        /// Finally, it logs diagnostic messages for debugging and state validation.
        /// </remarks>
        /// <param name="targetPosition">
        /// The grid position (<see cref="int"/> y, <see cref="int"/> x) where the player attempts to collect a key fragment.
        /// </param>
        public void HandleKey((int y, int x) targetPosition)
        {
            // === RETRIEVE KEY OBJECT FROM POSITION ===
            KeyFragmentInstance? key = _deps.GameObject.GetObject<KeyFragmentInstance>(targetPosition);
                   
            if (key == null)
            {
                _deps.Diagnostic.AddError($"{nameof(InteractionManager)}: No key object found at ({targetPosition.y},{targetPosition.x}).");
                return;
            }
           
            _deps.Inventory.AddKeyFragment(key.Amount);
          
            _deps.GameObject.RemoveObject(key.Position);
         
            _deps.Diagnostic.AddCheck($"{nameof(InteractionManager)}.{nameof(HandleKey)}: Picked up key at ({targetPosition.y},{targetPosition.x}).");
        }

        
        public void HandleDoor((int y, int x) targetPosition)
        {
            // === RETRIEVE DOOR OBJECT FROM POSITION ===
            DoorInstance? door = _deps.GameObject.GetObject<DoorInstance>(targetPosition);
          
            if (door == null)
            {
                _deps.Diagnostic.AddError($"{nameof(InteractionManager)}.{nameof(HandleDoor)}: No door object found at ({targetPosition.y},{targetPosition.x}).");
                return;
            }
                        
            int playerKeys = _deps.Inventory.KeyFragment;
            int requiredKeys = _deps.Level.RequiredKeys; 

            if (playerKeys < requiredKeys)
            {
                _deps.UI.FillUpBottomHud("Door", "Required Keys", $"You need more Keys {requiredKeys-_deps.Inventory.KeyFragment}", _deps.Symbol.KeyFragmentSymbol,requiredKeys);
                _deps.Diagnostic.AddWarning($"{nameof(InteractionManager)}.{nameof(HandleDoor)}: Door locked at ({targetPosition.y},{targetPosition.x}). Requires {requiredKeys}, player has {playerKeys}.");
                return;
            }

            _deps.Door.OpenDoor(_deps.GameBoard.ArraySizeY,_deps.GameBoard.ArraySizeX);  
            
            _deps.Level.NewLevel(_deps.Inventory.Score);
        
            _deps.Diagnostic.AddCheck($"{nameof(InteractionManager)}.{nameof(HandleDoor)}: Door opened at ({targetPosition.y},{targetPosition.x}). Used {requiredKeys} key fragments.");
            //TODO new Lvl generating

        }
        public void HandleQuit((int y, int x) targetPosition)
        {

        }

        public void InteractionHandler((int y,int x)targetPosition)
        {
            if (_deps.GameBoard == null)
            {
                _deps.Diagnostic.AddError($"{nameof(PlayerController)}: GameBoardArray reference missing!");
                return;

            }
            else if (_deps.GameBoard.GameBoardArray == null)
            {
                _deps.Diagnostic.AddError($"{nameof(PlayerController)}: GameBoard reference missing!");
                return;

            }

        
            TileTyp tile = _deps.GameBoard.GameBoardArray[targetPosition.y,targetPosition.x];

            switch (tile)
            {
                case TileTyp.Key:
                    HandleKey(targetPosition);
                    break;

                case TileTyp.Npc:
                    HandleNpc(targetPosition);
                    break;

                case TileTyp.Door:
                    HandleDoor(targetPosition);
                    break;

                default:
                    _deps.Diagnostic.AddCheck($"{nameof(InteractionManager)}: No interaction at ({targetPosition.y},{targetPosition.x}) [{tile}].");
                    break;
            }
        }























    }
}
