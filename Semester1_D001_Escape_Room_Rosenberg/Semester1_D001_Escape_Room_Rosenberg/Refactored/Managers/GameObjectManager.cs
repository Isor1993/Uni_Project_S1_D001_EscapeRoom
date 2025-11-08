/*****************************************************************************
* Project : Escape Room (K2, S2)
* File    : GameObjectManager.cs
* Date    : 09.11.2025
* Author  : Eric Rosenberg
*
* Description :
* Manages registration, lookup, and movement of all interactive objects 
* (Player, Door, NPCs, Keys, and Walls) on the game board.
* Synchronizes tile data in the GameBoardManager and provides diagnostic logging.
*
* History :
* 09.11.2025 ER Created / Refactored for SAE Coding Convention compliance
******************************************************************************/

using Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Door;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Key;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Npc;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Player;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Wall;
using System;
using System.Collections.Generic;


namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers
{
    /// <summary>
    /// Handles all interactive objects placed on the game board.
    /// Registers, removes, queries, and moves entities while synchronizing 
    /// their representation in the <see cref="GameBoardManager"/>.
    /// </summary>
    internal class GameObjectManager
    {
        // === Dependencies ===
        private  GameObjectManagerDependencies _deps;

        // === Fields ===
        private readonly Dictionary<(int y, int x), object> _objectOnBoard = new();

        // === Cached single instances ===
        private PlayerInstance? _playerInstance;
        private DoorInstance? _doorInstance;

        /// <summary>
        /// Gets the currently registered <see cref="PlayerInstance"/> if available.
        /// </summary>
        public PlayerInstance? Player => _playerInstance;

        /// <summary>
        /// Gets the currently registered <see cref="DoorInstance"/> if available.
        /// </summary>
        public DoorInstance? Door => _doorInstance;

        /// <summary>
        /// Initializes a new <see cref="GameObjectManager"/> and logs its creation.
        /// </summary>
        /// <param name="gameObjectManagerDependencies">
        /// Record containing required dependencies such as diagnostics and board access.
        /// </param>
        public GameObjectManager(GameObjectManagerDependencies gameObjectManagerDependencies)
        {
            _deps = gameObjectManagerDependencies;
            _deps.Diagnostic.AddCheck($"{nameof(GameObjectManager)}: Initialized successfully.");
        }

        /// <summary>
        /// Registers an object on the board and updates the corresponding tile type.
        /// Supports players, doors, NPCs, key fragments, and walls.
        /// </summary>
        /// <param name="position">The target grid coordinates (y, x).</param>
        /// <param name="boardObject">The object to register.</param>
        public void RegisterObject((int y, int x) position, object boardObject)
        {
            if (boardObject == null)
            {
                _deps.Diagnostic.AddError($"{nameof(GameObjectManager)}.{nameof(RegisterObject)}: Tried to register a null object at {position}.");
                return;
            }

            _objectOnBoard[position] = boardObject;

            TileType type = TileType.None;

            switch (boardObject)
            {
                case PlayerInstance player:
                    type = TileType.Player;
                    _playerInstance = player;
                    break;

                case DoorInstance door:
                    type = TileType.Door;
                    _doorInstance = door;
                    break;

                case NpcInstance :
                    type = TileType.Npc;                    
                    break;

                case KeyFragmentInstance :
                    type = TileType.Key;                   
                    break;

                case WallInstance wall:
                    switch (wall.Type)
                    {
                        case TileType.WallHorizontal:
                            type = TileType.WallHorizontal;
                            break;

                        case TileType.WallVertical:
                            type = TileType.WallVertical;
                            break;

                        case TileType.WallCornerTopLeft:
                            type = TileType.WallCornerTopLeft;
                            break;

                        case TileType.WallCornerTopRight:
                            type = TileType.WallCornerTopRight;
                            break;

                        case TileType.WallCornerBottomLeft:
                            type = TileType.WallCornerBottomLeft;
                            break;

                        case TileType.WallCornerBottomRight:
                            type = TileType.WallCornerBottomRight;
                            break;

                        default:
                            type = TileType.None;
                            _deps.Diagnostic.AddError($"{nameof(GameObjectManager)}.{nameof(RegisterObject)}:No TileType wall found regristerted as None.");
                            break;
                    }
                    break;

                default:
                    type = TileType.None;
                    _deps.Diagnostic.AddError($"{nameof(GameObjectManager)}.{nameof(RegisterObject)}: No TileType found. Registered as None.");
                    break;
            }

            _deps.GameBoard.SetTile(position, type);

            _deps.Diagnostic.AddCheck($"{nameof(GameObjectManager)}.{nameof(RegisterObject)}: Registered {boardObject.GetType().Name} at {position}.");
        }

        /// <summary>
        /// Removes an object from the given board position and clears the tile.
        /// </summary>
        /// <param name="position">The grid position of the object to remove.</param>
        public void RemoveObject((int y, int x) position)
        {
            if (_objectOnBoard.Remove(position))
            {
                _deps.GameBoard.SetTileToEmpty(position);                
                _deps.Diagnostic.AddCheck($"{nameof(GameObjectManager)}.{nameof(RemoveObject)}: Removed object at {position}.");
            }
            else
            {
                _deps.Diagnostic.AddWarning($"{nameof(GameObjectManager)}.{nameof(RemoveObject)}: Tried to remove object at {position}, but none was found.");
            }
        }

        /// <summary>
        /// Attempts to retrieve an object from the specified position.
        /// </summary>
        /// <param name="position">The grid coordinates (y, x).</param>
        /// <param name="boardObject">Outputs the found object, if any.</param>
        /// <returns><c>true</c> if an object exists at the given position; otherwise false.</returns>        
        public bool TryGetObject((int y, int x) position, out object? boardObject)
        {
            bool success = _objectOnBoard.TryGetValue(position, out boardObject);

            if (!success || boardObject == null)
            {
                _deps.Diagnostic.AddWarning($"{nameof(GameObjectManager)}.{nameof(TryGetObject)}: No object found at {position}.");
            }
            else
            {
                _deps.Diagnostic.AddCheck($"{nameof(GameObjectManager)}.{nameof(TryGetObject)}: Found {boardObject.GetType().Name} at {position}.");
            }

            return success;
        }

        /// <summary>
        /// Retrieves an object of a specific type from the given position.
        /// </summary>
        /// <typeparam name="T">Expected object type (e.g. PlayerInstance).</typeparam>
        /// <param name="position">The target grid coordinates (y, x).</param>
        /// <returns>The object if found and matching the type; otherwise null.</returns>
        public T? GetObject<T>((int y, int x) position) where T : class
        {

            if (_objectOnBoard.TryGetValue(position, out object? boardObject) && boardObject is T instance)
            {
                return instance;
            }
            _deps.Diagnostic.AddWarning($"{nameof(GameObjectManager)}.{nameof(GetObject)}: No object found or wrong type at {position}.");
            return null;
        }

        /// <summary>
        /// Removes all registered objects from the internal dictionary.
        /// </summary>
        public void ClearAll()
        {
            _objectOnBoard.Clear();
            _deps.Diagnostic.AddCheck($"{nameof(GameObjectManager)}.{nameof(ClearAll)}: Cleared all registered objects.");
        }

        /// <summary>
        /// Moves a player object from an old position to a new one if possible.
        /// </summary>
        /// <param name="oldPosition">Current grid coordinates of the player.</param>
        /// <param name="newPosition">Target grid coordinates for movement.</param>
        /// <returns><c>true</c> if movement succeeded; otherwise false.</returns>
        public bool MovePlayer((int y, int x) oldPosition, (int y, int x) newPosition)
        {
            if (!_objectOnBoard.TryGetValue(oldPosition, out object? boardObject))
            {
                _deps.Diagnostic.AddError($"{nameof(GameObjectManager)}.{nameof(MovePlayer)}: No Object (player) found at {oldPosition}.");
                return false;
            }

            if (_objectOnBoard.ContainsKey(newPosition))
            {
                
                _deps.Diagnostic.AddWarning($"{nameof(GameObjectManager)}.{nameof(MovePlayer)}: Target position {newPosition} already occupied.");
                return false;
            }

            RemoveObject(oldPosition);            

            RegisterObject(newPosition, boardObject);

            return true;
        }

        /// <summary>
        /// Searches the board for the player instance and returns it with its position.
        /// </summary>
        /// <param name="playerPosition">Outputs the found player position.</param>
        /// <returns>The found <see cref="PlayerInstance"/> or null if not found.</returns>
        private PlayerInstance? GetPlayerInstance(out (int y,int x)playerPosition)
        {
            foreach(KeyValuePair<(int y,int x),object>entry in _objectOnBoard)
            {
                if (entry.Value is PlayerInstance player)
                {
                    playerPosition = entry.Key;
                    return player;
                }
            }
            playerPosition = (1, 1);
            _deps.Diagnostic.AddError($"{nameof(GameObjectManager)}.{nameof(GetPlayerInstance)}: No PlayerInstance found. Default position {playerPosition} assigned.");
            return null;
        }

        /// <summary>
        /// Retrieves the player instance without position output.
        /// </summary>
        private PlayerInstance? GetPlayerInstance()
        {
            return GetPlayerInstance(out _);
        }

        /// <summary>
        /// Searches the board for the door instance and returns it with its position.
        /// </summary>
        /// <param name="doorPosition">Outputs the found door position.</param>
        /// <returns>The found <see cref="DoorInstance"/> or null if not found.</returns>
        private DoorInstance? GetDoorInstance(out (int y, int x) doorPosition)
        {
            foreach (KeyValuePair<(int y, int x), object> entry in _objectOnBoard)
            {
                if (entry.Value is DoorInstance door)
                {
                    doorPosition = entry.Key;
                    return door;
                }
            }
            doorPosition = (1, 1);
            _deps.Diagnostic.AddWarning($"{nameof(GameObjectManager)}.{nameof(GetDoorInstance)}: No DoorInstance found in boardObject. Default door position is {doorPosition}.");
            return null;
        }

        /// <summary>
        /// Retrieves the door instance without position output.
        /// </summary>
        private DoorInstance? GetDoorInstance()
        {
            return GetDoorInstance(out _);
        }
    }
}