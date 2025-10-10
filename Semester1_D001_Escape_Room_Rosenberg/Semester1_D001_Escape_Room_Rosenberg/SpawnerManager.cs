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
    /// Handles spawning of all objects in the game
    /// </summary>
    internal class SpawnerManager

    {
        private readonly PrinterManager _printer;
        private readonly SymbolsManager _symbols;
        private readonly RulesManager _rules;
        private readonly GameBoardBuilder _boardBuilder;
        private readonly Random _rand = new Random();

        public SpawnerManager(PrinterManager printer, SymbolsManager symbols, RulesManager rules, GameBoardBuilder boardBuilder)
        {
            _printer = printer;
            _symbols = symbols;
            _rules = rules;
            _boardBuilder = boardBuilder;

        }

        private (int y, int x) _playerSartPosition;
        public (int y, int x) PlayerStartposition { get => _playerSartPosition; private set => _playerSartPosition = value; }



        //TODO noch richtig einbauen und verknüpfen
        public bool stopGameCriticalError = false;
        /// <summary>
        /// List of quest Npc positions
        /// </summary>
        private readonly List<(int y, int x)> _listQuestNpcPositions = new List<(int y, int x)>();
        /// <summary>
        /// List of key fragment positions
        /// </summary>
        private readonly List<(int y, int x)> _listKeyFragmentPositions = new List<(int y, int x)>();
        /// <summary>
        /// Property for accessing the list of Npc positions
        /// </summary>
        public List<(int y, int x)> ListQuestNpcPositions => _listQuestNpcPositions;
        /// <summary>
        /// Property for accessing the list of key fragment positions
        /// </summary>
        public List<(int y, int x)> ListKeyFragmentPositions => _listKeyFragmentPositions;
        /// <summary>
        /// Constructor for linking builder and symbols
        /// </summary>       
        // Initialize a random object for generating random numbers



        /// <summary>
        /// Method for spawning a door on the game board
        /// </summary>
        public void SpawnDoor()
        {
            // Get the list of wall positions and store it in _listWallPositions
            List<(int y, int x)> _listWallPositions = _boardBuilder.ListWallPositions;
            // Get a random number within the range of wall positions and store it in randomIndex
            int randomIndex = _rand.Next(0, _listWallPositions.Count);
            // Get the wall position using the random index and store it in _doorPosition
            (int y, int x) _doorPosition = _listWallPositions[randomIndex];
            if ((_doorPosition.x == 0 && _doorPosition.y == 0) ||
                    (_doorPosition.x == _boardBuilder.ArraySizeX - 1 && _doorPosition.y == 0) ||
                    (_doorPosition.x == 0 && _doorPosition.y == _boardBuilder.ArraySizeY - 1) ||
                    (_doorPosition.x == _boardBuilder.ArraySizeX - 1 && _doorPosition.y == _boardBuilder.ArraySizeY - 1))
            // If it gets a corner position
            {
                // Call SpawnDoor again
                SpawnDoor();
                // Return to prevent placing a second door after the recursive call
                return;
            }
            // If it’s a top wall coordinate
            else if (_doorPosition.y == 0 || _doorPosition.y == _boardBuilder.ArraySizeY - 1)
            {
                // Place the door symbol in the game board array
                _boardBuilder.GameBoardArray[_doorPosition.y, _doorPosition.x] = _symbols.ClosedDoorTopWallSymbol;
            }
            // All remaining walls are side walls
            else
            {
                // Place the door symbol in the game board array
                _boardBuilder.GameBoardArray[_doorPosition.y, _doorPosition.x] = _symbols.ClosedDoorSideWallSymbol;
            }
        }
        /// <summary>
        /// Method for spawning the player
        /// </summary>
        public void SpawnPlayer()
        {
            List<(int y, int x)> _listEmptyPositions = _boardBuilder.ListEmptyPositions;
            // Define _playerPosition outside the loop so it remains available after exit
            //TODO (int y, int x) _playerPosition;
            // Boolean flag for the while-loop check
            bool isAreaFree = false;
            // Safety guard to prevent an endless loop
            int attempts = 0;
            // Maximum number of allowed attempts
            int maxAttempts = 200;
            do
            {
                // If the list is empty, send a warning message to the printer
                if (_listEmptyPositions.Count == 0)
                {
                    _printer.GetErrorMessage("Warning: SpawnPlayer () = _listEmptyPositions is empty !");
                    // Cancel the method because there’s no space left to spawn
                    return;
                }

                int randomIndex = _rand.Next(0, _listEmptyPositions.Count);
                _playerSartPosition = _listEmptyPositions[randomIndex];
                isAreaFree = _rules.IsPositionFree(_playerSartPosition);
                // Increment attempt counter
                attempts++;
                // Stop if attempts exceed maximum
                if (attempts >= maxAttempts)
                {
                    isAreaFree = false;
                    // Leave while loop
                    break;
                }
            }
            while (!isAreaFree);
            // If the area is not free, print an error message
            if (!isAreaFree)
            {
                _printer.GetErrorMessage("Critical Error: SpawnPlayer(): No free area around player position!");
                //TODO critical error game Stop
                stopGameCriticalError = true;
                // Leave methode
                return;

            }
            _boardBuilder.GameBoardArray[_playerSartPosition.y, _playerSartPosition.x] = _symbols.PlayerSymbol;
            // Remove the player position from the empty list
            _listEmptyPositions.Remove(_playerSartPosition);
            // Store player position in property for use by PlayerMove class later
            //TODO PlayerPosition = _playerPosition;

        }



        /// <summary>
        /// Method for spawning the key fragments
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="symbols"></param>
        /// <param name="rules"></param>
        /// <param name="printer"></param>
        public void SpawnKeyFragment()
        {
            int lvl = Program.CurrentLevel;
            List<(int y, int x)> _listEmptyPositions = _boardBuilder.ListEmptyPositions;
            for (int i = 0; i < lvl; i++)
            {
                (int y, int x) _keyFragmentPosition;
                bool isAreaFree = false;
                int attempts = 0;
                int maxAttempts = 200;
                do
                {
                    if (_listEmptyPositions.Count == 0)
                    {
                        _printer.GetErrorMessage("Warning: SpawnKeyFragment () = _listEmptyPositions is empty !");
                        return;
                    }

                    int randomIndex = _rand.Next(0, _listEmptyPositions.Count);
                    _keyFragmentPosition = _listEmptyPositions[randomIndex];
                    isAreaFree = _rules.IsPositionFree(_keyFragmentPosition);
                    attempts++;
                    if (attempts >= maxAttempts)
                    {
                        isAreaFree = false;
                        break;
                    }
                }
                while (!isAreaFree);
                if (!isAreaFree)
                {
                    _printer.GetErrorMessage("Warning: SpawnKeyFragment(): No free area around key fragment position!");
                    continue;
                }
                _boardBuilder.GameBoardArray[_keyFragmentPosition.y, _keyFragmentPosition.x] = _symbols.KeyFragmentSymbol;
                _listEmptyPositions.Remove(_keyFragmentPosition);
                _listKeyFragmentPositions.Add(_keyFragmentPosition);
            }
        }
        /// <summary>
        /// Method for spawning the QuestNpc
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="symbols"></param>
        /// <param name="rules"></param>
        /// <param name="printer"></param>
        public void SpawnQuestNpc()
        {
            int lvl = Program.CurrentLevel;
            List<(int y, int x)> _listEmptyPositions = _boardBuilder.ListEmptyPositions;
            for (int i = 0; i < lvl; i++)
            {
                (int y, int x) _questNpcPosition;
                bool isAreaFree = false;
                int attempts = 0;
                int maxAttempts = 200;
                do
                {
                    if (_listEmptyPositions.Count == 0)
                    {
                        _printer.GetErrorMessage("Warning: SpawnQuestNpc() = _listEmptyPositions is empty !");
                        return;
                    }
                    int randomIndex = _rand.Next(0, _listEmptyPositions.Count);
                    _questNpcPosition = _listEmptyPositions[randomIndex];
                    isAreaFree = _rules.IsPositionFree(_questNpcPosition);
                    attempts++;
                    if (attempts >= maxAttempts)
                    {
                        isAreaFree = false;
                        break;
                    }
                }
                while (!isAreaFree);
                if (!isAreaFree)
                {
                    _printer.GetErrorMessage("Warning: SpawnQuestNpc(): No free area around quest Npc position!");
                    continue;
                }
                _boardBuilder.GameBoardArray[_questNpcPosition.y, _questNpcPosition.x] = _symbols.QuestSymbol;
                _listEmptyPositions.Remove(_questNpcPosition);
                _listQuestNpcPositions.Add(_questNpcPosition);
            }
        }
        public void SpawnFog()
        {

        }





    }


}
