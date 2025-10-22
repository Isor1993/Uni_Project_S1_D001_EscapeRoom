using Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Door;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Key;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Npc;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Player;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Wall;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
// RSK Kontrolle ok
namespace Semester1_D001_Escape_Room_Rosenberg
{
    /// <summary>
    /// Handles all spawning operations for the game board, including doors, players,
    /// NPCs, and collectible items such as key fragments.
    /// Collects valid spawn positions and ensures that objects are placed only in available areas.
    /// </summary>
    internal class SpawnManager

    {
        // === Dependencies ===

        // Provides access to managers and builders.
        private readonly SpawnManagerDependencies _scd;
        // Manages door spawning and initialization.
        private readonly DoorInstance _doorInstance;
        // Manages key fragment spawning.
        private readonly KeyFragmentInstance _keyFragmentInstance;
        // Manages NPC spawning.
        private readonly NpcInstance _npcInstance;
        // Manages player spawning.
        private readonly PlayerInstance _playerInstance;
        // Provides wall data and placement logic.
        private readonly WallInstance _wallInstance;

        // === Fields ===
        private List<(int y, int x)> _wallPositions;
        private List<(int y, int x)> _emptyPositions;
        private List<(int y, int x)> _npcPositions;
        private List<(int y, int x)> _keyFragmentsPositions;



        /// <summary>
        /// Initializes a new instance of the <see cref="SpawnManager"/> class.
        /// Sets up all required dependencies for spawning operations.
        /// </summary>
        /// <param name="spawnManagerDependencies">Reference to the <see cref="SpawnManagerDependencies"/> that aggregates core managers and builders.</param>
        /// <param name="doorInstance">The <see cref="DoorInstance"/> used for door placement.</param>
        /// <param name="keyFragmentInstance">The <see cref="KeyFragmentInstance"/> used for item placement.</param>
        /// <param name="npcInstance">The <see cref="NpcInstance"/> used for NPC placement.</param>
        /// <param name="playerInstance">The <see cref="PlayerInstance"/> used for player placement.</param>
        /// <param name="wallInstance">The <see cref="WallInstance"/> used for wall tracking and initialization.</param>
        public SpawnManager(SpawnManagerDependencies spawnManagerDependencies, DoorInstance doorInstance, KeyFragmentInstance keyFragmentInstance, NpcInstance npcInstance, PlayerInstance playerInstance, WallInstance wallInstance)
        {
            this._scd = spawnManagerDependencies;
            this._doorInstance = doorInstance;
            this._keyFragmentInstance = keyFragmentInstance;
            this._npcInstance = npcInstance;
            this._playerInstance = playerInstance;
            this._wallInstance = wallInstance;
        }

        /// <summary>
        /// Gets the list of positions currently occupied by walls.
        /// </summary>
        public List<(int y, int x)> WallPosition => _wallPositions;

        /// <summary>
        /// Gets the list of empty positions available for spawning.
        /// </summary>
        public List<(int y, int x)> EmptyPositions => _emptyPositions;

        /// <summary>
        /// Gets the list of current NPC spawn positions.
        /// </summary>
        public List<(int y, int x)> NpcPossitions => _npcPositions;

        /// <summary>
        /// Gets the list of current key fragment spawn positions.
        /// </summary>
        public List<(int y, int x)> KeyFragmentPositions => _keyFragmentsPositions;

        /// <summary>
        /// Attempts to find a free spawn position from a list of valid candidates.
        /// Makes multiple attempts and returns the first valid position found.
        /// </summary>
        /// <param name="positions">A list of potential spawn coordinates.</param>
        /// <param name="maxAttempts">The maximum number of random attempts to find a free position.</param>
        /// <returns>
        /// A tuple containing a success flag and the resulting position.
        /// If no position is found, <c>success</c> is <c>false</c> and position defaults to (0, 0).
        /// </returns>  
        private (bool success, (int y, int x) position) TryFindFreeSpawnPosition(List<(int y, int x)> positions, int maxAttempts)
        {
            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                (int y, int x) position = _scd.RandomManager.RadomPositionFromList(positions);

                if (_scd.RulesManager.IsPositionFree(position))
                {
                    _scd.DiagnosticsManager.AddCheck($"{nameof(SpawnManager)}.{nameof(TryFindFreeSpawnPosition)}: Found free position {position} after {attempt} attempt(s).");
                    return (true, position);
                }

                _scd.DiagnosticsManager.AddWarning($"{nameof(SpawnManager)}.{nameof(TryFindFreeSpawnPosition)}: Attempt {attempt} failed at {position}.");
            }

            _scd.DiagnosticsManager.AddError($"{nameof(SpawnManager)}.{nameof(TryFindFreeSpawnPosition)}: No free position found after {maxAttempts} attempts.");
            return (false, (0, 0));
        }

        /// <summary>
        /// Collects all potential wall and empty positions from the current game board array.
        /// This method should be called after the board has been built and initialized.
        /// </summary>
        public void CollectSpawnPositions()
        {
            _wallPositions = new List<(int y, int x)>();
            _emptyPositions = new List<(int y, int x)>();

            if (_scd.GameBoardManager.GameBoardArray == null)
            {
                _scd.DiagnosticsManager.AddError($"{nameof(SpawnManager)}.{nameof(CollectSpawnPositions)}: GameBoardArray is null. Run {nameof(GameBoardManager)}.{nameof(GameBoardManager.DecideArraySize)}() first.");
                return;
            }

            TileTyp[,] board = _scd.GameBoardManager.GameBoardArray;

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
            _scd.DiagnosticsManager.AddCheck($"{nameof(SpawnManager)}.{nameof(CollectSpawnPositions)}: Found {_wallPositions.Count} walls and {_emptyPositions.Count} empty positions.");
        }


        /// <summary>
        /// Spawns a door at a random valid wall position.
        /// </summary>
        public void SpawnDoor()
        {
            if (_doorInstance == null)
            {
                _scd.DiagnosticsManager.AddError($"{nameof(SpawnManager)}.{nameof(SpawnDoor)}: No doorInstance found");
                return;
            }

            DoorInstance doorInstance = new DoorInstance(_scd.DoorInstanceDependencies);
            doorInstance.Initialize(_scd.RandomManager.RadomPositionFromList(_wallPositions));
            _wallPositions.Remove(doorInstance.Position);
            _scd.DiagnosticsManager.AddCheck($"{nameof(SpawnManager)}.{nameof(SpawnDoor)}: Placed door at {doorInstance.Position}");
        }

        /// <summary>
        /// Spawns the player at a randomly selected free position on the board.
        /// </summary>
        public void SpawnPlayer()
        {
            if (_playerInstance == null)
            {
                _scd.DiagnosticsManager.AddError($"{nameof(SpawnManager)}.{nameof(SpawnPlayer)}: No playerInstance found");
                return;
            }

            (bool success ,(int y,int x) position) result = TryFindFreeSpawnPosition(_emptyPositions, 50);

            if(!result.success)
            {
                return;
            }
            PlayerInstance playerInstance = new PlayerInstance(_scd.PlayerInstanceDependencies);
            
            playerInstance.Initialize(result.position);
            _emptyPositions.Remove(result.position);

            _scd.DiagnosticsManager.AddCheck($"{nameof(SpawnManager)}.{nameof(SpawnPlayer)}: Player placed successfully at {result.position}.");
        }

        /// <summary>
        /// Spawns a key fragment at a randomly selected free position on the board.
        /// </summary>
        public void SpawnKeyFragment()
        {
            if (_keyFragmentInstance == null)
            {
                _scd.DiagnosticsManager.AddError($"{nameof(SpawnManager)}.{nameof(SpawnKeyFragment)}: No keyFragmentInstance found");
                return;
            }

            (bool success, (int y, int x) position) result = TryFindFreeSpawnPosition(_emptyPositions, 50);

            if (!result.success)
            {
                return;
            }
            KeyFragmentInstance keyFragmentInstance = new KeyFragmentInstance(_scd.KeyFragmentInstanceDependencies);

            keyFragmentInstance.Initialize(result.position);
            _emptyPositions.Remove(result.position);

            _scd.DiagnosticsManager.AddCheck($"{nameof(SpawnManager)}.{nameof(SpawnPlayer)}: KeyFragment placed successfully at {result.position}.");

        }

        /// <summary>
        /// Spawns an NPC at a randomly selected free position on the board.
        /// </summary>
        public void SpawnNpc()
        {
            if (_keyFragmentInstance == null)
            {
                _scd.DiagnosticsManager.AddError($"{nameof(SpawnManager)}.{nameof(SpawnNpc)}: No NpcInstance found");
                return;
            }

            (bool success, (int y, int x) position) result = TryFindFreeSpawnPosition(_emptyPositions, 50);

            if (!result.success)
            {
                return;
            }
            NpcInstance npcInstance = new NpcInstance(_scd.);

            npcInstance.AssignPosition((result.position));
            _emptyPositions.Remove(result.position);

            _scd.DiagnosticsManager.AddCheck($"{nameof(SpawnManager)}.{nameof(SpawnNpc)}: Npc placed successfully at {result.position}.");
        }
    }  
}

