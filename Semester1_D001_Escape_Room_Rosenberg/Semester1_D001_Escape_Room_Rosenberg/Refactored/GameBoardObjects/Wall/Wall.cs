namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Wall
{
    /// <summary>
    /// Represents a wall element on the game board.
    /// Provides access to all wall symbols and stores both position and symbol data
    /// for individual wall instances.
    /// </summary>
    internal class Wall
    {
        //TODO *Man könnte ein enum für die Methoden machen.*

        // === Dependencies ===
        // Provides predefined wall symbols.
        private readonly SymbolsManager _symbolsManager;
        // Used for board-related operations.
        private readonly GameBoardBuilder _gameBoardBuilder;

        // === Fields ===
        private char _symbol;
        private (int y, int x) _position;

        /// <summary>
        /// Initializes a new instance of the <see cref="Wall"/> class with the required dependencies.
        /// </summary>
        /// <param name="symbolsManager">Reference to the <see cref="SymbolsManager"/> providing wall symbols.</param>
        /// <param name="gameBoardBuilder">Reference to the <see cref="GameBoardBuilder"/> used for board construction and management.</param>
        public Wall(SymbolsManager symbolsManager,GameBoardBuilder gameBoardBuilder) 
        {
            _symbolsManager = symbolsManager;
            _gameBoardBuilder=gameBoardBuilder;            
        }

        /// <summary>
        /// Gets the character symbol that visually represents this wall element.
        /// </summary>
        public char Symbol => _symbol;

        /// <summary>
        /// Gets the wall's position on the game board.
        /// </summary>
        public (int y, int x) Position =>_position;

        /// <summary>
        /// Assigns the top wall symbol to this wall instance.
        /// </summary>
        /// <returns></returns>
        public void AssignWallSymbolTop()
        {
             _symbol = _symbolsManager.WallTopSymbol;
        }

        /// <summary>
        /// Assigns the side wall symbol to this wall instance.
        /// </summary>
        /// <returns></returns>
        public void AssignWallSymbolSide()
        {
             _symbol= _symbolsManager.WallSideSymbol;
        }

        /// <summary>
        /// Assigns the cross wall symbol (used at intersections) to this wall instance.
        /// </summary>
        /// <returns></returns>
        public void AssignWallSymbolCross()
        {
             _symbol = _symbolsManager.WallCrossSymbol;
        }

        /// <summary>
        /// Assigns the bottom-left corner wall symbol to this wall instance.
        /// </summary>
        /// <returns></returns>
        public void AssignWallSymbolCornerLeftBottom()
        {
             _symbol = _symbolsManager.WallLeftBottomCornerSymbol;
        }

        /// <summary>
        /// Assigns the top-left corner wall symbol to this wall instance.
        /// </summary>
        /// <returns></returns>
        public void AssignWallSymbolCornerLeftTop()
        {
             _symbol = _symbolsManager.WallLeftTopCornerSymbol;
        }

        /// <summary>
        /// Assigns the bottom-right corner wall symbol to this wall instance.
        /// </summary>
        /// <returns></returns>
        public void AssignWallSymbolCornerRightBottom()
        {
             _symbol = _symbolsManager.WallRightBottomCornerSymbol;
        }

        /// <summary>
        /// Assigns the top-right corner wall symbol to this wall instance.
        /// </summary>
        /// <returns></returns>
        public void AssignWallSymbolCornerRightTop()
        {
             _symbol = _symbolsManager.WallRightTopCornerSymbol;
        }

        /// <summary>
        /// Assigns the T-junction wall symbol facing downward.
        /// </summary>
        /// <returns></returns>
        public void AssignWallSymbolTDown()
        {
             _symbol=_symbolsManager.WallTDownSymbol;
        }

        /// <summary>
        /// Assigns the T-junction wall symbol facing left.
        /// </summary>
        /// <returns></returns>
        public void AssignWallSymbolTLeft()
        {
             _symbol=_symbolsManager.WallTLeftSymbol;
        }

        /// <summary>
        /// Assigns the T-junction wall symbol facing right.
        /// </summary>
        /// <returns></returns>
        public void AssignWallSymbolTRight()
        {
             _symbol = _symbolsManager.WallTRightSymbol;
        }

        /// <summary>
        /// Assigns the T-junction wall symbol facing upward.
        /// </summary>
        /// <returns></returns>
        public void AssignWallSymbolUp()
        {
             _symbol = _symbolsManager.WallTUpSymbol;
        }

        /// <summary>
        /// Assigns a position to this wall element on the game board.
        /// </summary>
        /// <param name="y">The Y-coordinate of the wall's position.</param>
        /// <param name="x">The X-coordinate of the wall's position.</param>
        public void AssignPosition(int y, int x)
        {
            _position = (y, x);
        }

        /// <summary>
        /// Initializes the wall with a specific symbol and position on the game board.
        /// </summary>
        /// <param name="symbol">The wall symbol to assign.</param>
        /// <param name="y">The Y-coordinate of the wall's position.</param>
        /// <param name="x">The X-coordinate of the wall's position.</param>
        public void Initialize(char symbol,int y,int x)
        { 
            _symbol = symbol;
            _position = (y, x);
        }    
    }
}
