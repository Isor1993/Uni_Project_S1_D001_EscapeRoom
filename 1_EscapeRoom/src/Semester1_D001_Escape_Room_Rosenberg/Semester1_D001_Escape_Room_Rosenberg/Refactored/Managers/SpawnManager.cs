/*****************************************************************************
* Project : Escape Room (K2, S2)
* File    : SpawnManager.cs
* Date    : 09.11.2025
* Author  : Eric Rosenberg
*
* Description :
* Handles the initialization and placement of all dynamic game objects 
* (player, NPCs, keys, and door) within the game board. 
* Manages wall generation, spawn validation, and position allocation logic.
*
* Responsibilities:
* - Collects valid spawn positions (walls and empty tiles)
* - Spawns and registers all major interactive entities
* - Validates free positions using the RulesManager
* - Updates GameObject and GameBoard state after each spawn
* - Handles wall and corner tile construction
*
* History :
* 09.11.2025 ER Created / Documentation fully updated
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
    /// Controls all spawn operations for entities within the game world.
    /// Handles procedural placement of player, door, NPCs, and key fragments,
    /// as well as construction of static walls and corner structures.
    /// </summary>
    internal class SpawnManager

    {
        // === Dependencies ===
        private readonly SpawnManagerDependencies _deps;

        // === Cached Instances ===
        private PlayerInstance? _playerInstance;

        // === Spawn Position Lists ===
        private List<(int y, int x)> _wallPositions = new();
        private List<(int y, int x)> _emptyPositions = new();

        // === Fields ===
        const int MAX_ATTEMPTS = 50;


        /// <summary>
        /// Initializes a new instance of the <see cref="SpawnManager"/> class.
        /// </summary>
        /// <param name="spawnManagerDependencies">Dependency container providing all core systems required for spawning.</param>
        /// <remarks>
        /// Dependencies include managers for diagnostics, randomization, 
        /// symbol handling, wall/door/player/NPC dependencies, and rule validation.
        /// </remarks>
        public SpawnManager(SpawnManagerDependencies spawnManagerDependencies)
        {
            _deps = spawnManagerDependencies;
            _deps.Diagnostic.AddCheck($"{nameof(SpawnManager)}: Initialized successfully.");

        }

        /// <summary>
        /// Returns the currently spawned <see cref="PlayerInstance"/>, if any.
        /// </summary>
        public PlayerInstance? GetPlayer => _playerInstance;

        /// <summary>
        /// Returns all recorded wall tile positions detected on the board.
        /// </summary>
        public List<(int y, int x)> WallPosition => _wallPositions;

        /// <summary>
        /// Returns all currently empty board positions available for spawning objects.
        /// </summary>
        public List<(int y, int x)> EmptyPositions => _emptyPositions;

        /// <summary>
        /// Clears all cached spawn position lists (walls and empty tiles).
        /// </summary>
        public void ClearAll()
        {
            WallPosition.Clear();
            EmptyPositions.Clear();
        }

        /// <summary>
        /// Executes the complete spawn sequence for a new level start.
        /// </summary>
        /// <param name="npcAmount">Number of NPCs to spawn.</param>
        /// <param name="keyAmount">Number of key fragments to spawn.</param>
        /// <remarks>
        /// Sequence: Collect → Walls → Door → Player → NPCs → Keys.
        /// </remarks>
        public void SpawnAll(int npcAmount, int keyAmount)
        {
            CollectSpawnPositions();
            SpawnWalls();
            SpawnDoor();
            SpawnPlayer();
            SpawnNpc(npcAmount);
            SpawnKeyFragment(keyAmount);
        }

        /// <summary>
        /// Performs the spawn sequence when a new level is loaded,
        /// reusing the existing player instance.
        /// </summary>
        /// <param name="npcAmount">Number of NPCs to spawn.</param>
        /// <param name="keyAmount">Number of key fragments to spawn.</param>
        public void SpawnAllNewLvl(int npcAmount, int keyAmount)
        {
            CollectSpawnPositions();
            SpawnWalls();
            SpawnDoor();
            SpawnPlayerNewLVL();
            SpawnNpc(npcAmount);
            SpawnKeyFragment(keyAmount);
        }

        /// <summary>
        /// Attempts to locate a valid free position for object spawning.
        /// </summary>
        /// <param name="positions">List of candidate positions to evaluate.</param>
        /// <returns>
        /// Tuple containing success flag and position coordinates.  
        /// Returns <c>(false, (0,0))</c> if no valid position is found.
        /// </returns>
        /// <remarks>
        /// Performs multiple randomized checks using <see cref="RulesManager.IsPositionFreeForSpawn"/>.
        /// Each attempt is logged through the diagnostics system.
        /// </remarks>
        private (bool success, (int y, int x) position) TryFindFreeSpawnPosition(List<(int y, int x)> positions)
        {

            for (int attempt = 1; attempt <= MAX_ATTEMPTS; attempt++)
            {
                (int y, int x) position = _deps.Random.RandomPositionFromList(positions);

                if (_deps.Rule.IsPositionFreeForSpawn(position))
                {
                    _deps.Diagnostic.AddCheck($"{nameof(SpawnManager)}.{nameof(TryFindFreeSpawnPosition)}: Found free position {position} after {attempt} attempt(s).");
                    return (true, position);
                }

                _deps.Diagnostic.AddWarning($"{nameof(SpawnManager)}.{nameof(TryFindFreeSpawnPosition)}: Attempt {attempt} failed at {position}.");
            }

            _deps.Diagnostic.AddError($"{nameof(SpawnManager)}.{nameof(TryFindFreeSpawnPosition)}: No free position found after {MAX_ATTEMPTS} attempts.");
            return (false, (0, 0));
        }

        /// <summary>
        /// Scans the current game board and records all wall and empty cell positions.
        /// </summary>
        /// <remarks>
        /// Must be called after <see cref="GameBoardManager.DecideArraySize"/> 
        /// to ensure the board exists. Logs total counts of found tiles.
        /// </remarks>
        private void CollectSpawnPositions()
        {
            _wallPositions.Clear();
            _emptyPositions.Clear();

            if (_deps.GameBoard.GameBoardArray == null)
            {
                _deps.Diagnostic.AddError($"{nameof(SpawnManager)}.{nameof(CollectSpawnPositions)}: GameBoardArray is null. Run {nameof(GameBoardManager)}.{nameof(GameBoardManager.DecideArraySize)}() first.");
                return;
            }

            TileType[,] board = _deps.GameBoard.GameBoardArray;

            for (int y = 0; y < board.GetLength(0); y++)
            {
                for (int x = 0; x < board.GetLength(1); x++)
                {
                    TileType tile = board[y, x];
                    if (tile == TileType.WallHorizontal || tile == TileType.WallVertical)
                    {
                        _wallPositions.Add((y, x));
                    }
                    else if (tile == TileType.Empty)
                    {
                        _emptyPositions.Add((y, x));
                    }
                }
            }
            _deps.Diagnostic.AddCheck($"{nameof(SpawnManager)}.{nameof(CollectSpawnPositions)}: Found {_wallPositions.Count} walls and {_emptyPositions.Count} empty positions.");
        }

        /// <summary>
        /// Creates and places the main exit door at a random wall position.
        /// </summary>
        private void SpawnDoor()
        {
            if (_deps.DoorInstanceDeps == null)
            {
                _deps.Diagnostic.AddError($"{nameof(SpawnManager)}.{nameof(SpawnDoor)}: No doorInstance found");
                return;
            }

            DoorInstance doorInstance = new DoorInstance(_deps.DoorInstanceDeps);
            if (_deps.WallInstanceDeps == null)
            {
                _deps.Diagnostic.AddError($"{nameof(SpawnManager)}.{nameof(SpawnWalls)}: No WallInstanceDeps provided.");
                return;
            }

            if (_wallPositions.Count == 0)
            {
                _deps.Diagnostic.AddError($"{nameof(SpawnManager)}.{nameof(SpawnDoor)}: No wall positions available.");
                return;
            }



            doorInstance.Initialize(_deps.Random.RandomPositionFromList(_wallPositions));

            RegisterAndPlaceObject(doorInstance.Position, doorInstance);

            _wallPositions.Remove(doorInstance.Position);


            _deps.Diagnostic.AddCheck($"{nameof(SpawnManager)}.{nameof(SpawnDoor)}: Placed door at {doorInstance.Position}");
        }

        /// <summary>
        /// Creates a new player instance and places it at a random empty position.
        /// </summary>
        private void SpawnPlayer()
        {
            if (_deps.PlayerInstanceDeps == null)
            {
                _deps.Diagnostic.AddError($"{nameof(SpawnManager)}.{nameof(SpawnPlayer)}: No playerInstance found");
                return;
            }

            (bool success, (int y, int x) position) result = TryFindFreeSpawnPosition(_emptyPositions);

            if (!result.success)
            {
                return;
            }
            _playerInstance = new PlayerInstance(_deps.PlayerInstanceDeps, "Joschi");

            _playerInstance.Initialize(result.position);

            RegisterAndPlaceObject(result.position, _playerInstance);

            _emptyPositions.Remove(result.position);

            _deps.Diagnostic.AddCheck($"{nameof(SpawnManager)}.{nameof(SpawnPlayer)}: Player placed successfully at {result.position}.");
        }

        /// <summary>
        /// Repositions the existing player instance at the start of a new level.
        /// </summary>
        private void SpawnPlayerNewLVL()
        {
            if (_deps.PlayerInstanceDeps == null)
            {
                _deps.Diagnostic.AddError($"{nameof(SpawnManager)}.{nameof(SpawnPlayer)}: No playerInstance found");
                return;
            }

            (bool success, (int y, int x) position) result = TryFindFreeSpawnPosition(_emptyPositions);

            if (!result.success)
            {
                return;
            }


            Program.PlayerInstance.Initialize(result.position);

            RegisterAndPlaceObject(result.position, Program.PlayerInstance);

            _emptyPositions.Remove(result.position);

            _deps.Diagnostic.AddCheck($"{nameof(SpawnManager)}.{nameof(SpawnPlayer)}: Player placed successfully at {result.position}.");
        }

        /// <summary>
        /// Spawns multiple key fragments across the board.
        /// </summary>
        /// <param name="amount">Total number of fragments to spawn.</param>
        private void SpawnKeyFragment(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                if (_deps.KeyFragmentInstancedeps == null)
                {
                    _deps.Diagnostic.AddError($"{nameof(SpawnManager)}.{nameof(SpawnKeyFragment)}: No keyFragmentInstance found");
                    return;
                }

                (bool success, (int y, int x) position) result = TryFindFreeSpawnPosition(_emptyPositions);

                if (!result.success)
                {
                    return;
                }
                KeyFragmentInstance keyFragmentInstance = new KeyFragmentInstance(_deps.KeyFragmentInstancedeps);

                keyFragmentInstance.Initialize(result.position);

                RegisterAndPlaceObject(result.position, keyFragmentInstance);

                _emptyPositions.Remove(result.position);

                _deps.Diagnostic.AddCheck($"{nameof(SpawnManager)}.{nameof(SpawnKeyFragment)}: KeyFragment placed successfully at {result.position}.");
            }
        }

        /// <summary>
        /// Spawns a given number of NPCs at randomized positions.
        /// </summary>
        /// <param name="amount">Number of NPCs to spawn.</param>
        private void SpawnNpc(int amount)
        {
            List<NpcInstance> tempNpcList = _deps.Random.GetRandomElements(_deps.Npc.NpcList, amount);

            foreach (NpcInstance npc in tempNpcList)
            {
                if (npc == null)
                {
                    _deps.Diagnostic.AddError($"{nameof(SpawnManager)}.{nameof(SpawnNpc)}: No NpcInstance found");
                    return;
                }

                (bool success, (int y, int x) position) result = TryFindFreeSpawnPosition(_emptyPositions);

                if (!result.success)
                {
                    return;
                }

                npc.AssignPosition(result.position);

                RegisterAndPlaceObject(result.position, npc);

                _emptyPositions.Remove(result.position);

                _deps.Diagnostic.AddCheck($"{nameof(SpawnManager)}.{nameof(SpawnNpc)}: Npc placed successfully at {result.position}.");
            }
        }

        /// <summary>
        /// Generates the wall boundaries around the game board.
        /// </summary>
        private void SpawnWalls()
        {
            // Check if the board exists and recreate before trying to fill it.
            if (_deps.GameBoard.GameBoardArray == null)
            {
                _deps.Diagnostic.AddError($"{nameof(SpawnManager)}.{nameof(SpawnWalls)}: Cannot fill walls. Board has not been initialized.");
                return;
            }
            if (_deps.WallInstanceDeps == null)
            {
                _deps.Diagnostic.AddError($"{nameof(SpawnManager)}.{nameof(SpawnWalls)}: No WallInstanceDeps provided.");
                return;
            }
            // Iterate through all rows.
            for (int y = 0; y < _deps.GameBoard.ArraySizeY; y++)
            {
                // Iterate through all columns.
                for (int x = 0; x < _deps.GameBoard.ArraySizeX; x++)
                {
                    // Assign horizontal walls at the top and bottom.
                    if (y == 0 || y == _deps.GameBoard.ArraySizeY - 1)
                    {

                        WallInstance wall = new WallInstance(_deps.WallInstanceDeps);
                        wall.Initialize(TileType.WallHorizontal, (y, x));
                        _deps.GameObject.RegisterObject((y, x), wall);
                    }
                    // Assign vertical walls on the left and right edges.
                    else if (x == 0 || x == _deps.GameBoard.ArraySizeX - 1)
                    {

                        WallInstance wall = new WallInstance(_deps.WallInstanceDeps);
                        wall.Initialize(TileType.WallVertical, (y, x));
                        _deps.GameObject.RegisterObject((y, x), wall);
                    }
                }
            }
            SetCornersForGameObject();
        }

        /// <summary>
        /// Replaces edge tiles with appropriate corner tile types.
        /// </summary>
        private void SetCornersForGameObject()
        {
            UpdateCorner((0, 0), TileType.WallCornerTopLeft);
            UpdateCorner((0, _deps.GameBoard.ArraySizeX - 1), TileType.WallCornerTopRight);
            UpdateCorner((_deps.GameBoard.ArraySizeY - 1, 0), TileType.WallCornerBottomLeft);
            UpdateCorner((_deps.GameBoard.ArraySizeY - 1, _deps.GameBoard.ArraySizeX - 1), TileType.WallCornerBottomRight);
        }

        /// <summary>
        /// Updates a specific wall tile to a corner tile type on the board.
        /// </summary>
        /// <param name="position">Target position of the corner.</param>
        /// <param name="newType">New tile type to assign (corner variant).</param>
        private void UpdateCorner((int y, int x) position, TileType newType)
        {
            if (_deps.GameBoard.GameBoardArray == null)
            {
                _deps.Diagnostic.AddError($"{nameof(SpawnManager)}.{nameof(UpdateCorner)}: GameBoardArray is null!");
                return;
            }
            if (_deps.GameObject.TryGetObject(position, out object? obj) && obj is WallInstance wall)
            {
                wall.Initialize(newType, position);
                _deps.GameBoard.SetTile(position, newType);

                _deps.Diagnostic.AddCheck($"{nameof(SpawnManager)}: Updated existing wall at {position} to {newType}.");
            }
            else
            {
                _deps.Diagnostic.AddError($"{nameof(SpawnManager)}: No existing wall found at {position} to convert to corner.");
            }
        }

        /// <summary>
        /// Registers a spawned object and updates both the GameObjectManager 
        /// and diagnostic logs accordingly.
        /// </summary>
        /// <param name="position">Position where the object is placed.</param>
        /// <param name="obj">Spawned object instance.</param>
        private void RegisterAndPlaceObject((int y, int x) position, object obj)
        {
            _deps.GameObject.RegisterObject(position, obj);
            _deps.Diagnostic.AddCheck($"{nameof(SpawnManager)}: Registered {obj.GetType().Name} at {position}");
        }
    }
}