using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers
{
    /// <summary>
    /// Central registry for all active objects placed on the game board.
    /// Manages registration, removal, lookup, and diagnostics logging
    /// for every object that exists at a specific (y, x) position.
    /// </summary>
    internal class GameObjectManager
    {
        // === Fields ===
        private readonly Dictionary<(int y, int x), object> _objectOnBoard = new();
        private readonly DiagnosticsManager _diagnostic;
        private readonly GameBoardManager _gameBoard;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameObjectManager"/> class.
        /// Sets up diagnostics and prepares the internal registry dictionary.
        /// </summary>
        /// <param name="diagnosticsManager">
        /// Reference to the <see cref="DiagnosticsManager"/> used for logging
        /// checks, warnings, and error messages during object registration and access.
        /// </param>
        public GameObjectManager(DiagnosticsManager diagnosticsManager, GameBoardManager gameBoardManager)
        {
            _diagnostic = diagnosticsManager;
            _gameBoard = gameBoardManager;
            _diagnostic.AddCheck($"{nameof(GameObjectManager)}: Instance successfully created.");
        }

        /// <summary>
        /// Registers an object on the board at the specified position.
        /// If another object already exists at that position, it is replaced.
        /// </summary>
        /// <param name="position">
        /// The (y, x) coordinates where the object should be placed.
        /// </param>
        /// <param name="boardObject">
        /// The object instance to register on the board.
        public void RegisterObject((int y, int x) position, object boardObject)
        {
            if (boardObject == null)
            {
                _diagnostic.AddError($"{nameof(GameObjectManager)}.{nameof(RegisterObject)}: Tried to register a null object at {position}.");
                return;
            }
            _objectOnBoard[position] = boardObject;
            _diagnostic.AddCheck($"{nameof(GameObjectManager)}: Registered {boardObject.GetType().Name} at {position}.");
        }

        /// <summary>
        /// Removes an object from the board at the specified position.
        /// Logs a warning if no object is found at the given coordinates.
        /// </summary>
        /// <param name="position">
        /// The (y, x) coordinates of the object to remove.
        /// </param>
        public void RemoveObject((int y, int x) position)
        {
            if (_objectOnBoard.Remove(position))
                _diagnostic.AddCheck($"{nameof(GameObjectManager)}: Removed object at {position}.");
            else
                _diagnostic.AddWarning($"{nameof(GameObjectManager)}: Tried to remove object at {position}, but none was found.");
        }

        /// <summary>
        /// Attempts to retrieve any registered object at a given position.
        /// Returns <c>true</c> if an object exists at the specified location; otherwise <c>false</c>.
        /// </summary>
        /// <param name="position">
        /// The (y, x) coordinates to check on the game board.
        /// </param>
        /// <param name="boardObject">
        /// When this method returns, contains the object found at the position,
        /// or <c>null</c> if none was found.
        /// </param>
        /// <returns>
        /// <c>true</c> if an object was successfully retrieved; otherwise <c>false</c>.
        /// </returns>
        public bool TryGetObject((int y, int x) position, out object? boardObject)
        {
            bool success = _objectOnBoard.TryGetValue(position, out boardObject);

            if (!success || boardObject == null)
            {
                _diagnostic.AddWarning($"{nameof(GameObjectManager)}.{nameof(TryGetObject)}: No object found at {position}.");
            }
            else
            {
                _diagnostic.AddCheck($"{nameof(GameObjectManager)}.{nameof(TryGetObject)}: Found {boardObject.GetType().Name} at {position}.");
            }

            return success;
        }

        /// <summary>
        /// Attempts to retrieve an object of a specified type from the game board
        /// at the given position. If the object exists and matches the requested type,
        /// it is returned; otherwise, a warning is logged and <c>null</c> is returned.
        /// </summary>
        /// <typeparam name="T">
        /// The expected reference type of the object to retrieve.
        /// </typeparam>
        /// <param name="position">
        /// The (y, x) coordinates representing the object's position on the game board.
        /// </param>
        /// <returns>
        /// The object cast to the specified type <typeparamref name="T"/> if found and valid;
        /// otherwise <c>null</c>.
        /// </returns>
        public T? GetObject<T>((int y, int x) position) where T : class
        {

            if (_objectOnBoard.TryGetValue(position, out object? boardObject) && boardObject is T instance)
            {
                return instance;
            }
            _diagnostic.AddWarning($"{nameof(GameObjectManager)}.{nameof(GetObject)}: No object found or wrong type at {position}.");
            return null;
        }

        /// <summary>
        /// Clears all registered objects from the board.
        /// Typically used when resetting or reloading a scene.
        /// </summary>
        public void ClearAll()
        {
            _objectOnBoard.Clear();
            _diagnostic.AddCheck($"{nameof(GameObjectManager)}: Cleared all registered objects.");
        }

        /// <summary>
        /// Returns a copy of all currently registered objects.
        /// Intended primarily for debugging and inspection purposes.
        /// </summary>
        /// <returns>
        /// A read-only dictionary containing all registered objects
        /// with their corresponding board positions.
        /// </returns>
        public IReadOnlyDictionary<(int y, int x), object> GetAllObjects()
        {
            return new Dictionary<(int y, int x), object>(_objectOnBoard);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typ"></param>
        /// <param name="position"></param>
        public void UpdateBoard(TileTyp typ,(int y,int x) position)
        {
            _gameBoard.PlaceTileTypOnBoard(typ, position);
        }
    }
}