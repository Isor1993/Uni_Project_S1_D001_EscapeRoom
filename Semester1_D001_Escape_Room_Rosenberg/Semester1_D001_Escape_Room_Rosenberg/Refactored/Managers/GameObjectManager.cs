using Microsoft.VisualBasic;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Door;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Key;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Npc;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Player;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Wall;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers
{
    /// <summary>
    /// Central registry for all active objects placed on the game board.
    /// </summary>
    /// <remarks>
    /// The <see cref="GameObjectManager"/> acts as a unified storage system for all dynamic game objects,  
    /// including NPCs, keys, doors, walls, and the player.  
    /// It provides methods to register, move, remove, and retrieve objects while ensuring 
    /// that the visual board representation remains synchronized with the internal state.  
    /// All actions are logged via <see cref="DiagnosticsManager"/> for debugging and validation.
    /// </remarks>
    internal class GameObjectManager
    {
        // === Dependencies ===
        private readonly GameObjectManagerDependencies _deps;

        // === Fields ===
        private readonly Dictionary<(int y, int x), object> _objectOnBoard = new();
        // === Cached single instances ===
        private PlayerInstance? _playerInstance;
        private DoorInstance? _doorInstance;

        public PlayerInstance Player => _playerInstance;

        public DoorInstance Door => _doorInstance;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameObjectManager"/> class.
        /// </summary>
        /// <param name="gameObjectManagerDependencies">
        /// Provides the required references for managing the board and diagnostic logging.
        /// </param>
        /// <remarks>
        /// When created, the manager is ready to register and synchronize board objects immediately.  
        /// A diagnostic confirmation message is logged upon instantiation.
        /// </remarks>
        public GameObjectManager(GameObjectManagerDependencies gameObjectManagerDependencies)
        {
            _deps = gameObjectManagerDependencies;
            _deps.Diagnostic.AddCheck($"{nameof(GameObjectManager)}: Initialized successfully.");
        }

        /// <summary>
        /// Registers a new game object at the specified grid position.
        /// </summary>
        /// <remarks>
        /// This method determines the <see cref="TileType"/> of the object based on its type 
        /// (e.g., <see cref="NpcInstance"/>, <see cref="DoorInstance"/>, <see cref="KeyFragmentInstance"/>, etc.)  
        /// and updates both the internal object dictionary and the visual board array accordingly.  
        /// Logs all operations and error cases through the diagnostics system.
        /// </remarks>
        /// <param name="position">
        /// The (y, x) grid coordinates where the object will be placed.
        /// </param>
        /// <param name="boardObject">
        /// The object to register, such as a wall, key, door, or NPC instance.
        /// </param>
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
                    switch (wall.Typ)
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
                            _deps.Diagnostic.AddError($"{nameof(GameObjectManager)}.{nameof(RegisterObject)}:No TileTyp wall found regristerted as None");
                            break;
                    }
                    break;

                default:
                    type = TileType.None;
                    _deps.Diagnostic.AddError($"{nameof(GameObjectManager)}.{nameof(RegisterObject)}: No TileTyp found registerted as None");
                    break;
            }

            _deps.GameBoard.SetTile(position, type);

            _deps.Diagnostic.AddCheck($"{nameof(GameObjectManager)}: Registered {boardObject.GetType().Name} at {position}.");
        }

        /// <summary>
        /// Removes an object from the game board at the specified position.
        /// </summary>
        /// <param name="position">
        /// The (y, x) grid coordinates of the object to remove.
        /// </param>
        public void RemoveObject((int y, int x) position)
        {
            if (_objectOnBoard.Remove(position))
            {
                _deps.GameBoard.SetTileToEmpty(position);                
                _deps.Diagnostic.AddCheck($"{nameof(GameObjectManager)}: Removed object at {position}.");
            }
            else
            {
                _deps.Diagnostic.AddWarning($"{nameof(GameObjectManager)}: Tried to remove object at {position}, but none was found.");
            }
        }

        /// <summary>
        /// Attempts to retrieve a registered object at the given position.
        /// </summary>
        /// <param name="position">The (y, x) position to check on the board.</param>
        /// <param name="boardObject">Outputs the object found, or <c>null</c> if none was present.</param>
        /// <returns><c>true</c> if an object was found; otherwise <c>false</c>.</returns>
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
        /// <typeparam name="T">The expected class type of the object.</typeparam>
        /// <param name="position">The (y, x) position to check on the board.</param>
        /// <returns>
        /// The object cast to the specified type if found; otherwise <c>null</c>.
        /// Logs a warning if the type does not match or the position is empty.
        /// </returns>
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
        /// Removes all currently registered objects from the game board.
        /// </summary>
        /// <remarks>
        /// Typically used during level resets or scene transitions.  
        /// Logs the action in diagnostics for state verification.
        /// </remarks>
        public void ClearAll()
        {
            _objectOnBoard.Clear();
            _deps.Diagnostic.AddCheck($"{nameof(GameObjectManager)}: Cleared all registered objects.");
        }

        /// <summary>
        /// Returns a copy of all currently registered objects on the game board.
        /// </summary>
        /// <remarks>
        /// Intended for debugging, logging, or developer inspection.  
        /// The returned dictionary is a safe copy to prevent external modification.
        /// </remarks>
        /// <returns>
        /// A read-only dictionary of all registered objects and their respective positions.
        /// </returns>
        public IReadOnlyDictionary<(int y, int x), object> GetAllObjects()
        {
            return new Dictionary<(int y, int x), object>(_objectOnBoard);
        }

        /// <summary>
        /// Updates the board tile at the given position to match the provided <see cref="TileType"/>.
        /// </summary>
        /// <param name="position">The (y, x) grid position to update.</param>
        /// <param name="typ">The new <see cref="TileType"/> to assign to this position.</param>
        public void UpdateBoard((int y, int x) position, TileType typ)
        {
            _deps.GameBoard.SetTile(position, typ);            
        }

        /// <summary>
        /// Moves a player object from one position to another on the game board.
        /// </summary>
        /// <remarks>
        /// The method ensures that the new position is not already occupied, 
        /// removes the player from the old position, registers it at the new one, 
        /// and synchronizes both the object registry and board state.  
        /// Diagnostic messages are logged for every operation or failure.
        /// </remarks>
        /// <param name="oldPosition">The player’s current grid position.</param>
        /// <param name="newPosition">The target grid position to move to.</param>
        /// <returns><c>true</c> if the move was successful; otherwise <c>false</c>.</returns>
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
        /// Searches the provided board dictionary for a <see cref="PlayerInstance"/> 
        /// and returns it if found.
        /// </summary>
        /// <param name="boardObject">
        /// The dictionary containing all objects placed on the game board.
        /// The key represents a 2D coordinate tuple (y, x),
        /// and the value represents the stored object 
        /// (e.g., PlayerInstance, DoorInstance, WallInstance, etc.).
        /// </param>
        /// <param name="playerPosition">
        /// Outputs the position of the found <see cref="PlayerInstance"/> as (y, x).
        /// If no player is found, the returned position will be (1, 1).
        /// </param>
        /// <returns>
        /// The located <see cref="PlayerInstance"/>, or <c>null</c> 
        /// if no player object exists within the board dictionary.
        /// </returns>
        public PlayerInstance? GetPlayerInstance(out (int y,int x)playerPosition)
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
            _deps.Diagnostic.AddWarning($"{nameof(GameObjectManager)}.{nameof(GetPlayerInstance)}: No PlayerInstance found in boardObject. Player position deafault is {playerPosition}");
            return null;
        }

        /// <summary>
        /// Searches the internal board dictionary for a <see cref="PlayerInstance"/> 
        /// and returns it if found.
        /// </summary>
        /// <returns>
        /// The located <see cref="PlayerInstance"/>, or <c>null</c> 
        /// if no player object exists within the board.
        /// </returns>
        public PlayerInstance? GetPlayerInstance()
        {
            return GetPlayerInstance(out _);
        }

        public DoorInstance? GetDoorInstance(out (int y, int x) doorPosition)
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
            _deps.Diagnostic.AddWarning($"{nameof(GameObjectManager)}.{nameof(GetDoorInstance)}: No DoorInstance found in boardObject. Door position deafault is {doorPosition}");
            return null;
        }

        public DoorInstance? GetDoorInstance()
        {
            return GetDoorInstance(out _);
        }

    }
}