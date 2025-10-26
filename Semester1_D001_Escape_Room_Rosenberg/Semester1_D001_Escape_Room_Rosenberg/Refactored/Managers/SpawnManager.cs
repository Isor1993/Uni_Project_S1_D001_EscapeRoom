using Semester1_D001_Escape_Room_Rosenberg.Refactored;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Door;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Key;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Npc;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Player;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Wall;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
// RSK Kontrolle ok
namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers
{
    /// <summary>
    /// Handles the spawning of all interactive game objects onto the board.
    /// </summary>
    /// <remarks>
    /// The <see cref="SpawnManager"/> is responsible for placing all key gameplay elements such as  
    /// <see cref="PlayerInstance"/>, <see cref="DoorInstance"/>, <see cref="NpcInstance"/>, and <see cref="KeyFragmentInstance"/>  
    /// onto the game board in valid positions.  
    /// It performs randomized placement using <see cref="RandomManager"/> while validating all positions via <see cref="RulesManager"/>.
    /// </remarks>
    internal class SpawnManager

    {
        // === Dependencies ===

        private readonly SpawnManagerDependencies _deps;

        // === Fields ===
        private List<(int y, int x)> _wallPositions = new();
        private List<(int y, int x)> _emptyPositions = new();


        /// <summary>
        /// Initializes a new instance of the <see cref="SpawnManager"/> class.
        /// </summary>
        /// <param name="spawnManagerDependencies">The dependency container providing all core systems required for spawning.</param>
        /// <remarks>
        /// Dependencies include managers for rules, diagnostics, symbols, randomization, and references  
        /// to key object dependencies such as walls, doors, NPCs, and the player.
        /// </remarks>
        public SpawnManager(SpawnManagerDependencies spawnManagerDependencies)
        {
            _deps = spawnManagerDependencies;
            _deps.Diagnostic.AddCheck($"{nameof(SpawnManager)}: Initialized successfully.");

        }

        /// <summary>
        /// Gets all current wall positions detected on the board.
        /// </summary>
        public List<(int y, int x)> WallPosition => _wallPositions;

        /// <summary>
        /// Gets all currently empty board positions available for object spawning.
        /// </summary>
        public List<(int y, int x)> EmptyPositions => _emptyPositions;



        /// <summary>
        /// Attempts to find a valid spawn position from a list of candidates.
        /// </summary>
        /// <param name="positions">A list of candidate positions from which a valid one is selected.</param>
        /// <returns>
        /// Returns a tuple containing a success flag and the chosen position.  
        /// If no position is found, <c>success</c> is <c>false</c> and position defaults to (0, 0).
        /// </returns>
        /// <remarks>
        /// The method performs randomized attempts using <see cref="RulesManager.IsPositionFreeForSpawn((int, int))"/>  
        /// to ensure a valid spawn area.  
        /// Each attempt is logged via the <see cref="DiagnosticsManager"/>.
        /// </remarks>
        private (bool success, (int y, int x) position) TryFindFreeSpawnPosition(List<(int y, int x)> positions)
        {
            int maxAttempts = 50;
            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                (int y, int x) position = _deps.Random.RandomPositionFromList(positions);

                if (_deps.Rule.IsPositionFreeForSpawn(position))
                {
                    _deps.Diagnostic.AddCheck($"{nameof(SpawnManager)}.{nameof(TryFindFreeSpawnPosition)}: Found free position {position} after {attempt} attempt(s).");
                    return (true, position);
                }

                _deps.Diagnostic.AddWarning($"{nameof(SpawnManager)}.{nameof(TryFindFreeSpawnPosition)}: Attempt {attempt} failed at {position}.");
            }

            _deps.Diagnostic.AddError($"{nameof(SpawnManager)}.{nameof(TryFindFreeSpawnPosition)}: No free position found after {maxAttempts} attempts.");
            return (false, (0, 0));
        }

        /// <summary>
        /// Scans the current game board and collects all wall and empty cell positions.
        /// </summary>
        /// <remarks>
        /// Must be called after <see cref="GameBoardManager.DecideArraySize"/>  
        /// to ensure a valid board exists before spawning.  
        /// Logs total counts of wall and empty tiles.
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

            TileTyp[,] board = _deps.GameBoard.GameBoardArray;

            for (int y = 0; y < board.GetLength(0); y++)
            {
                for (int x = 0; x < board.GetLength(1); x++)
                {
                    TileTyp tile = board[y, x];
                    if (tile == TileTyp.WallHorizontal || tile == TileTyp.WallVertical)
                    {
                        _wallPositions.Add((y, x));
                    }
                    else if (tile == TileTyp.Empty)
                    {
                        _emptyPositions.Add((y, x));
                    }
                }
            }
            _deps.Diagnostic.AddCheck($"{nameof(SpawnManager)}.{nameof(CollectSpawnPositions)}: Found {_wallPositions.Count} walls and {_emptyPositions.Count} empty positions.");
        }

        /// <summary>
        /// Spawns a single door on a random wall position.
        /// </summary>
        /// <remarks>
        /// Creates a new <see cref="DoorInstance"/> and initializes it at a random wall location.  
        /// Once spawned, the wall position is removed from the available wall list.
        /// </remarks>
        private void SpawnDoor()
        {
            if (_deps.DoorInstance == null)
            {
                _deps.Diagnostic.AddError($"{nameof(SpawnManager)}.{nameof(SpawnDoor)}: No doorInstance found");
                return;
            }

            DoorInstance doorInstance = new DoorInstance(_deps.DoorInstance);

            doorInstance.Initialize(_deps.Random.RandomPositionFromList(_wallPositions), _deps.GameBoard.ArraySizeY, _deps.GameBoard.ArraySizeX);

            RegisterAndPlaceObject(doorInstance.Position, doorInstance);

            _wallPositions.Remove(doorInstance.Position);


            _deps.Diagnostic.AddCheck($"{nameof(SpawnManager)}.{nameof(SpawnDoor)}: Placed door at {doorInstance.Position}");
        }

        /// <summary>
        /// Spawns the player at a random empty position.
        /// </summary>
        /// <remarks>
        /// Uses <see cref="TryFindFreeSpawnPosition(List{(int, int)})"/> to determine  
        /// a valid location and initializes a new <see cref="PlayerInstance"/> at that position.
        /// </remarks>
        private void SpawnPlayer()
        {
            if (_deps.PlayerInstance == null)
            {
                _deps.Diagnostic.AddError($"{nameof(SpawnManager)}.{nameof(SpawnPlayer)}: No playerInstance found");
                return;
            }

            (bool success, (int y, int x) position) result = TryFindFreeSpawnPosition(_emptyPositions);

            if (!result.success)
            {
                return;
            }
            PlayerInstance playerInstance = new PlayerInstance(_deps.PlayerInstance);

            playerInstance.Initialize(result.position);

            RegisterAndPlaceObject(result.position, playerInstance);

            _emptyPositions.Remove(result.position);

            _deps.Diagnostic.AddCheck($"{nameof(SpawnManager)}.{nameof(SpawnPlayer)}: Player placed successfully at {result.position}.");
        }

        /// <summary>
        /// Spawns the specified number of key fragments at random free positions.
        /// </summary>
        /// <param name="amount">The total number of key fragments to spawn.</param>
        private void SpawnKeyFragment(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                if (_deps.KeyFragmentInstance == null)
                {
                    _deps.Diagnostic.AddError($"{nameof(SpawnManager)}.{nameof(SpawnKeyFragment)}: No keyFragmentInstance found");
                    return;
                }

                (bool success, (int y, int x) position) result = TryFindFreeSpawnPosition(_emptyPositions);

                if (!result.success)
                {
                    return;
                }
                KeyFragmentInstance keyFragmentInstance = new KeyFragmentInstance(_deps.KeyFragmentInstance);

                keyFragmentInstance.Initialize(result.position);

                RegisterAndPlaceObject(result.position, keyFragmentInstance);

                _emptyPositions.Remove(result.position);

                _deps.Diagnostic.AddCheck($"{nameof(SpawnManager)}.{nameof(SpawnKeyFragment)}: KeyFragment placed successfully at {result.position}.");
            }
        }

        /// <summary>
        /// Spawns the specified number of NPCs at random free positions.
        /// </summary>
        /// <param name="amount">The number of NPCs to spawn.</param>
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
        /// Spawns all interactive entities: door, player, NPCs, and key fragments.
        /// </summary>
        /// <param name="npcAmount">The number of NPCs to spawn.</param>
        /// <param name="keyAmount">The number of key fragments to spawn.</param>
        /// <remarks>
        /// This method executes the complete spawn sequence in the following order:
        /// 1. Collect positions  
        /// 2. Spawn door  
        /// 3. Spawn player  
        /// 4. Spawn NPCs  
        /// 5. Spawn key fragments  
        /// </remarks>
        public void SpawnAll(int npcAmount, int keyAmount)
        {
            CollectSpawnPositions();
            SpawnDoor();
            SpawnPlayer();
            SpawnNpc(npcAmount);
            SpawnKeyFragment(keyAmount);
        }

        /// <summary>
        /// Registers the spawned object with the <see cref="GameObjectManager"/> and updates the board state.
        /// </summary>
        /// <param name="position">The grid position where the object is placed.</param>
        /// <param name="obj">The object to register and track.</param>
        private void RegisterAndPlaceObject((int y, int x) position, object obj)
        {
            _deps.GameObject.RegisterObject(position, obj);
            _deps.Diagnostic.AddCheck($"{nameof(SpawnManager)}: Registered {obj.GetType().Name} at {position}");
        }
    }
}