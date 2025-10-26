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
    /// Handles all spawning operations for game objects on the board,
    /// including doors, players, NPCs, and key fragments.
    /// Uses rule checks, randomized selection, and dependency-injected logic
    /// to ensure valid placement without collisions or invalid positions.
    /// </summary>
    /// <remarks>
    /// The <see cref="SpawnManager"/> coordinates object creation by combining
    /// rule validation, randomized placement, and registration in the
    /// <see cref="GameObjectManager"/>. It ensures that every spawned entity
    /// adheres to board constraints and maintains consistent diagnostic logging.
    /// </remarks>
    internal class SpawnManager

    {
        // === Dependencies ===
        // Provides access to core managers and builders.
        private readonly SpawnManagerDependencies _deps;
        // Manages door creation.
        private readonly DoorInstance _doorInstance;
        // Handles key fragment creation.
        private readonly KeyFragmentInstance _keyFragmentInstance;
        // Template instance for NPC creation.
        private readonly NpcInstance _npcInstance;
        // Template instance for player creation.
        private readonly PlayerInstance _playerInstance;
        // Provides wall data and placement logic.
        private readonly WallInstance _wallInstance;

        // === Fields ===
        private List<(int y, int x)> _wallPositions = new();
        private List<(int y, int x)> _emptyPositions = new();
        private List<(int y, int x)> _npcPositions = new();
        private List<(int y, int x)> _keyFragmentsPositions = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="SpawnManager"/> class.
        /// Sets up all required dependencies for object spawning operations.
        /// </summary>
        /// <param name="spawnManagerDependencies">
        /// Reference to the <see cref="SpawnManagerDependencies"/> providing access
        /// to all managers and supporting systems required for spawning.
        /// </param>
        /// <param name="doorInstance">Reference to the <see cref="DoorInstance"/> used for door creation.</param>
        /// <param name="keyFragmentInstance">Reference to the <see cref="KeyFragmentInstance"/> used for item spawning.</param>
        /// <param name="npcInstance">Reference to the <see cref="NpcInstance"/> used as an NPC template.</param>
        /// <param name="playerInstance">Reference to the <see cref="PlayerInstance"/> representing the player object.</param>
        /// <param name="wallInstance">Reference to the <see cref="WallInstance"/> used for wall placement logic.</param>
        public SpawnManager(SpawnManagerDependencies spawnManagerDependencies, DoorInstance doorInstance, KeyFragmentInstance keyFragmentInstance, NpcInstance npcInstance, PlayerInstance playerInstance, WallInstance wallInstance)
        {
            _deps = spawnManagerDependencies;
            _doorInstance = doorInstance;
            _keyFragmentInstance = keyFragmentInstance;
            _npcInstance = npcInstance;
            _playerInstance = playerInstance;
            _wallInstance = wallInstance;
        }

        /// <summary>
        /// Gets a list of wall positions on the current board.
        /// </summary>
        public List<(int y, int x)> WallPosition => _wallPositions;

        /// <summary>
        /// Gets a list of all currently empty positions available for spawning.
        /// </summary>
        public List<(int y, int x)> EmptyPositions => _emptyPositions;

        /// <summary>
        /// Gets a list of NPC positions currently assigned during the spawn process.
        /// </summary>
        public List<(int y, int x)> NpcPositions => _npcPositions;

        /// <summary>
        /// Gets a list of positions used for key fragment placement.
        /// </summary>
        public List<(int y, int x)> KeyFragmentPositions => _keyFragmentsPositions;

        /// <summary>
        /// Attempts to find a valid free spawn position from the provided list of positions.
        /// Performs multiple randomized attempts until a valid position is found or the
        /// maximum number of attempts is reached.
        /// </summary>
        /// <param name="positions">
        /// A list of candidate positions to evaluate for potential spawning.
        /// </param>
        /// <returns>
        /// A tuple containing a success flag and the resulting position.
        /// If no valid position is found, <c>success</c> is <c>false</c> and
        /// <c>position</c> defaults to (0, 0).
        /// </returns>
        /// <remarks>
        /// This method uses randomized selection and rule validation via
        /// <see cref="RulesManager.IsPositionFree((int, int))"/> to ensure that
        /// spawned entities are placed only in valid, non-occupied areas.
        /// All attempts and outcomes are logged through the <see cref="DiagnosticsManager"/>.
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
        /// Collects all wall and empty positions from the current game board.
        /// This method must be called after the board has been initialized and sized.
        /// </summary>
        public void CollectSpawnPositions()
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
        /// Spawns a door at a random valid wall position on the board.
        /// </summary>
        public void SpawnDoor()
        {
            if (_doorInstance == null)
            {
                _deps.Diagnostic.AddError($"{nameof(SpawnManager)}.{nameof(SpawnDoor)}: No doorInstance found");
                return;
            }

            DoorInstance doorInstance = new DoorInstance(_deps.DoorInstance);

            doorInstance.Initialize(_deps.Random.RandomPositionFromList(_wallPositions));
            _deps.GameBoard.SetTile(TileTyp.Door, doorInstance.Position);
            _deps.GameObject.RegisterObject(doorInstance.Position, doorInstance);
            _wallPositions.Remove(doorInstance.Position);


            _deps.Diagnostic.AddCheck($"{nameof(SpawnManager)}.{nameof(SpawnDoor)}: Placed door at {doorInstance.Position}");
        }

        /// <summary>
        /// Spawns the player at a random valid free position on the board.
        /// </summary>
        public void SpawnPlayer()
        {
            if (_playerInstance == null)
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
            _deps.GameBoard.SetTile(TileTyp.Player, result.position);
            _deps.GameObject.RegisterObject(result.position, playerInstance);
            _emptyPositions.Remove(result.position);

            _deps.Diagnostic.AddCheck($"{nameof(SpawnManager)}.{nameof(SpawnPlayer)}: Player placed successfully at {result.position}.");
        }

        /// <summary>
        /// Spawns the specified number of key fragments at random valid positions on the game board.
        /// Ensures that each key fragment is placed on an available empty tile and properly registered
        /// in both the game board and object manager systems.
        /// </summary>
        /// <param name="amount">
        /// The number of key fragments to spawn.
        /// </param>
        /// <remarks>
        /// This method performs multiple spawn attempts using randomized valid positions.
        /// If the <see cref="_keyFragmentInstance"/> reference is missing, or no free positions
        /// are available, the method logs the issue through the <see cref="DiagnosticsManager"/>.
        /// Each successfully spawned key fragment is initialized, placed on the board,
        /// and recorded in the <see cref="GameObjectManager"/>.
        /// </remarks>
        public void SpawnKeyFragment(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                if (_keyFragmentInstance == null)
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
                _deps.GameBoard.SetTile(TileTyp.Key, result.position);
                _deps.GameObject.RegisterObject(result.position, keyFragmentInstance);
                _emptyPositions.Remove(result.position);

                _deps.Diagnostic.AddCheck($"{nameof(SpawnManager)}.{nameof(SpawnKeyFragment)}: KeyFragment placed successfully at {result.position}.");
            }
        }

        /// <summary>
        /// Spawns the specified number of NPCs at random valid positions on the board.
        /// </summary>
        /// <param name="amount">The number of NPCs to spawn.</param>
        public void SpawnNpc(int amount)
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
                _deps.GameBoard.SetTile(TileTyp.Npc, result.position);
                _deps.GameObject.RegisterObject(result.position, npc);
                _emptyPositions.Remove(result.position);

                _deps.Diagnostic.AddCheck($"{nameof(SpawnManager)}.{nameof(SpawnNpc)}: Npc placed successfully at {result.position}.");
            }
        }

        /// <summary>
        /// Executes a complete spawning sequence for all core game objects,
        /// including the door, player, NPCs, and key fragments.
        /// </summary>
        /// <param name="npcAmount">
        /// The total number of NPCs to spawn on the game board.
        /// </param>
        /// <param name="keyAmount">
        /// The total number of key fragments to spawn on the game board.
        /// </param>
        /// <remarks>
        /// This method serves as a high-level initialization routine that triggers
        /// all individual spawn methods in sequence. It first collects valid board
        /// positions, then spawns the door, player, NPCs, and key fragments in the
        /// correct order to ensure logical placement and gameplay integrity.
        /// </remarks>
        public void SpawnAll(int npcAmount, int keyAmount)
        {
            CollectSpawnPositions();
            SpawnDoor();
            SpawnPlayer();
            SpawnNpc(npcAmount);
            SpawnKeyFragment(keyAmount);
        }
    }
}

