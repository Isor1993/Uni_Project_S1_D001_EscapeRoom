/*****************************************************************************
* Project : Escape Room (K2, S2)
* File    : UIManager.cs
* Date    : 09.11.2025
* Author  : Eric Rosenberg
*
* Description :
* Handles all visual console output related to the game HUD (Heads-Up Display).
* Responsible for drawing and updating the top and bottom UI sections,
* including player stats, NPC messages, symbols, and interaction responses.
* Integrates data from multiple managers (GameBoard, GameObject, Inventory, etc.)
* and provides visual feedback through structured console rendering.
*
* ---------------------------------------------------------------------------
* 🧱 HUD Content Rules (Dialogformat & Limits)
* ---------------------------------------------------------------------------
* Each NPC dialogue line follows the format:
*   [Name];[Question];[CorrectAnswer];[OptionB];[OptionC];[KeyFragments];[RewardPoints]
*
* ┌──────────────┬──────────────────────────────┬──────────────┬────────────────────────────┐
* │ Field        │ Description                  │ Max Length   │ Value Rules                │
* ├──────────────┼──────────────────────────────┼──────────────┼────────────────────────────┤
* │ Name         │ NPC name (no spaces)         │ ≤ 12 chars   │ Use underscores (_) only   │
* │ Question     │ Full question ending with ?  │ ≤ 117 chars  │ Full sentence required     │
* │ CorrectAnswer│ Correct answer (keyword)     │ ≤ 10 chars   │ One word or short phrase   │
* │ OptionB      │ Wrong answer 1               │ ≤ 10 chars   │ Short keyword or phrase    │
* │ OptionC      │ Wrong answer 2               │ ≤ 10 chars   │ Short keyword or phrase    │
* │ KeyFragments │ Reward fragment indicator    │ Always 1     │ Always “1”                 │
* │ RewardPoints │ Points awarded               │ 50 – 100     │ Fixed or random range      │
* └──────────────┴──────────────────────────────┴──────────────┴────────────────────────────┘
*
* ⚙ Usage:
* - All dialogue lines must comply with these formatting and length limits.
* - `CutNpcMessageIntoLines()` handles wrapping for up to 117 characters.
* - `BuildBottomHudLineAnswer_8()` builds centered answer options ([1][2][3])
*   and should be checked against max 10 chars per option.
*
* History :
* 09.11.2025 ER Created / Refactored for SAE Coding Convention compliance
******************************************************************************/

using Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Door;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Player;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers
{
    /// <summary>
    ///  Manages all visual console output for the player HUD.
    /// Draws and updates the top section (stats, score, symbols)
    /// and bottom section (NPC dialogues, messages, and answer prompts).
    /// </summary>
    internal class UIManager
    {
        // === Dependencies ===
        private readonly UIManagerDependencies _deps;

        // === Fields ===
        private string _bottomHudLine_1 = string.Empty;

        private string _bottomHudLine_2 = string.Empty;
        private string _bottomHudLine_3 = string.Empty;
        private string _bottomHudLine_4 = string.Empty;
        private string _bottomHudLine_5 = string.Empty;
        private string _bottomHudLine_6 = string.Empty;
        private string _bottomHudLine_7 = string.Empty;
        private string _bottomHudLine_8 = string.Empty;
        private string _bottomHudLine_9 = string.Empty;

        private readonly List<string> _npcMessages = new List<string>();

        /// <summary>
        /// Initializes a new <see cref="UIManager"/> instance with all required dependencies.
        /// </summary>
        /// <param name="uIManagerDependencies">Dependency record linking GameBoard, GameObject, Inventory, and Diagnostics.</param>
        public UIManager(UIManagerDependencies uIManagerDependencies)
        {
            _deps = uIManagerDependencies;
            _deps.Diagnostic.AddCheck($"{nameof(UIManager)}: Initialized successfully.");
        }

        /// <summary>
        /// Builds and prints the top HUD section containing player info and door state.
        /// </summary>
        public void BuildTopHud()
        {
            if (_deps.GameBoard == null || _deps.GameBoard.GameBoardArray == null)
            {
                _deps.Diagnostic.AddError($"{nameof(UIManager)}.{nameof(BuildTopHud)}: GameBoard or GameBoardArray is null.");
                return;
            }

            UpdateTopHudLine_1_GameInfos();
            UpdateTopHudLine_2_GameInfos_2();

            Console.SetCursorPosition(Program.CursorPosX, Program.CursorPosYTopHud_3);
            Console.WriteLine(new string('─', _deps.GameBoard.ArraySizeX));

            _deps.Diagnostic.AddCheck($"{nameof(UIManager)}.{nameof(BuildTopHud)}: BuildTopHud successfully.");
        }

        /// <summary>
        /// Builds the bottom HUD section with NPC dialogue and item count box.
        /// </summary>
        /// <param name="messangerName">Displayed NPC name (≤12 chars, underscores only).</param>
        /// <param name="infoBoxString">Label for the info box (e.g. "Inventory: ").</param>
        /// <param name="message">NPC question text (≤117 chars, must end with '?').</param>
        /// <param name="symbol">Display symbol (e.g. for key or heart).</param>
        /// <param name="count">Item count displayed next to the symbol (always positive).</param>
        /// <param name="answer_1">Answer option A (≤10 chars).</param>
        /// <param name="answer_2">Answer option B (≤10 chars).</param>
        /// <param name="answer_3">Answer option C (≤10 chars).</param>
        /// <remarks>See HUD Content Rules in class header.</remarks>
        public void FillUpBottomHud(string messangerName, string infoBoxString, string message, char symbol, int count, string answer_1, string answer_2, string answer_3)
        {
            if (_deps.GameBoard == null || _deps.GameBoard.GameBoardArray == null)
            {
                _deps.Diagnostic.AddError($"{nameof(UIManager)}.{nameof(FillUpBottomHud)}: Missing GameBoard reference.");
                return;
            }

            // no saftey if count is too high
            string infoBoxSymbol = $"{symbol} x{count}";
            string infoBox = infoBoxString + infoBoxSymbol;

            const int SYMBOL_OFFSET = 1;

            int innerWidth = _deps.GameBoard.ArraySizeX - SYMBOL_OFFSET * 2;

            List<string> cutTextLines = CutNpcMessageIntoLines(message, innerWidth);

            // === BUILD HUD STRUCTURE ===
            _bottomHudLine_1 = BuildBottomHudLine_1();
            _bottomHudLine_2 = BuildBottomHudLine_2(messangerName, infoBox);
            _bottomHudLine_3 = BuildBottomHudLine_3();
            _bottomHudLine_4 = BuildBottomHudLine_4(cutTextLines.Count > 0 ? cutTextLines[0] : "");
            _bottomHudLine_5 = BuildBottomHudLine_5(cutTextLines.Count > 1 ? cutTextLines[1] : "");
            _bottomHudLine_6 = BuildBottomHudLine_6(cutTextLines.Count > 2 ? cutTextLines[2] : "");
            _bottomHudLine_7 = BuildBottomHudLine_7();
            _bottomHudLine_8 = BuildBottomHudLineAnswer_8(answer_1, answer_2, answer_3);
            _bottomHudLine_9 = BuildBottomHudLine_9();
            _deps.Diagnostic.AddCheck($"{nameof(UIManager)}.{nameof(FillUpBottomHud)}: FillUpBottomHud successfully.");
        }

        /// <summary>
        /// Builds a version of the bottom HUD without an item count field.
        /// </summary>
        /// <param name="messangerName">Displayed NPC name (≤12 chars).</param>
        /// <param name="infoBoxstring">Label for the info box.</param>
        /// <param name="message">NPC dialogue text (≤117 chars).</param>
        /// <param name="symbol">Symbol representing NPC or event.</param>
        /// <param name="answer_1">Answer option 1 (≤10 chars).</param>
        /// <param name="answer_2">Answer option 2 (≤10 chars).</param>
        /// <param name="answer_3">Answer option 3 (≤10 chars).</param>
        /// <remarks>Respects HUD text rules defined in the class header.</remarks>
        public void FillUpBottomHud(string messangerName, string infoBoxstring, string message, char symbol, string answer_1, string answer_2, string answer_3)
        {
            if (_deps.GameBoard == null || _deps.GameBoard.GameBoardArray == null)
            {
                _deps.Diagnostic.AddError($"{nameof(UIManager)}.{nameof(FillUpBottomHud)}: Missing GameBoard reference.");
                return;
            }

            string infoBoxSymbol = $"{symbol}";
            string infoBox = infoBoxstring + infoBoxSymbol;

            const int SYMBOL_OFFSET = 1;

            int innerWidth = _deps.GameBoard.ArraySizeX - SYMBOL_OFFSET * 2;

            List<string> cutTextLines = CutNpcMessageIntoLines(message, innerWidth);

            // === BUILD HUD STRUCTURE ===

            _bottomHudLine_1 = BuildBottomHudLine_1();
            _bottomHudLine_2 = BuildBottomHudLine_2(messangerName, infoBox);
            _bottomHudLine_3 = BuildBottomHudLine_3();
            _bottomHudLine_4 = BuildBottomHudLine_4(cutTextLines.Count > 0 ? cutTextLines[0] : "");
            _bottomHudLine_5 = BuildBottomHudLine_5(cutTextLines.Count > 1 ? cutTextLines[1] : "");
            _bottomHudLine_6 = BuildBottomHudLine_6(cutTextLines.Count > 2 ? cutTextLines[2] : "");
            _bottomHudLine_7 = BuildBottomHudLine_7();
            _bottomHudLine_8 = BuildBottomHudLineAnswer_8(answer_1, answer_2, answer_3);
            _bottomHudLine_9 = BuildBottomHudLine_9();

            _deps.Diagnostic.AddCheck($"{nameof(UIManager)}.{nameof(FillUpBottomHud)}: FillUpBottomHud successfully.");
        }

        /// <summary>
        /// Prints the composed bottom HUD onto the console.
        /// </summary>
        public void PrintBottomHud()
        {
            int hudY = Program.CursorPosYBottomHudStart;
            Console.SetCursorPosition(Program.CursorPosX, hudY);
            Console.WriteLine(_bottomHudLine_1);
            Console.WriteLine(_bottomHudLine_2);
            Console.WriteLine(_bottomHudLine_3);
            Console.WriteLine(_bottomHudLine_4);
            Console.WriteLine(_bottomHudLine_5);
            Console.WriteLine(_bottomHudLine_6);
            Console.WriteLine(_bottomHudLine_7);
            Console.WriteLine(_bottomHudLine_8);
            Console.WriteLine(_bottomHudLine_9);

            _deps.Diagnostic.AddCheck($"{nameof(UIManager)}.{nameof(PrintBottomHud)}: Bottom HUD drawn at Y={Program.CursorPosYBottomHudStart}.");
        }

        /// <summary>
        /// Builds an empty placeholder bottom HUD with no NPC dialogue or answers.
        /// </summary>
        public void BuildEmptyBottomHud()
        {
            if (_deps.GameBoard == null || _deps.GameBoard.GameBoardArray == null)
            {
                _deps.Diagnostic.AddError($"{nameof(UIManager)}.{nameof(BuildEmptyBottomHud)}: Missing GameBoard reference.");
                return;
            }

            int width = _deps.GameBoard.ArraySizeX;

            _bottomHudLine_1 = BuildBottomHudLine_1();
            _bottomHudLine_2 = BuildBottomHudLine_2(string.Empty, string.Empty);
            _bottomHudLine_3 = BuildBottomHudLine_3();
            _bottomHudLine_4 = BuildBottomHudLine_4(string.Empty);
            _bottomHudLine_5 = BuildBottomHudLine_5(string.Empty);
            _bottomHudLine_6 = BuildBottomHudLine_6(string.Empty);
            _bottomHudLine_7 = BuildBottomHudLine_7();

            // Antworten-Zeile bewusst LEER (keine [1][2][3])
            _bottomHudLine_8 = BuildBottomHudLineAnswer_8(string.Empty, string.Empty, string.Empty);

            _bottomHudLine_9 = BuildBottomHudLine_9();

            _deps.Diagnostic.AddCheck($"{nameof(UIManager)}.{nameof(BuildEmptyBottomHud)}: Empty bottom HUD built.");
        }

        /// <summary>
        /// Updates the second top HUD line (Lives, keys, door state).
        /// </summary>
        private void UpdateTopHudLine_1_GameInfos()
        {
            if (_deps.GameObject == null)
            {
                _deps.Diagnostic.AddError($"{nameof(UIManager)}.{nameof(UpdateTopHudLine_1_GameInfos)}: GameObject is null.");
            }

            PlayerInstance? player = _deps.GameObject?.Player;
            if (player == null)
            {
                _deps.Diagnostic.AddError($"{nameof(UIManager)}.{nameof(UpdateTopHudLine_1_GameInfos)}: Playerinstance is null");
                return;
            }

            int score = _deps.Inventory.Score;

            string scoreString = $"Score: {score} ";

            string namePart = $"{_deps.Symbol.PlayerSymbol} {player?.Name}";
            string lvlPart = $"Lvl = {_deps.Level.CurrentLvl}";
            string scorePart = scoreString;

            Console.SetCursorPosition(Program.CursorPosX, Program.CursorPosYTopHudStart);
            Console.Write(BuildCentredLine(namePart, lvlPart, scorePart, _deps.GameBoard.ArraySizeX));
            _deps.Diagnostic.AddCheck($"{nameof(UIManager)}.{nameof(UpdateTopHudLine_1_GameInfos)}: UpdateTopHudLine_1_GameInfos successfully.");
        }

        /// <summary>
        /// Updates the second top HUD line (Lives, keys, door state).
        /// </summary>
        private void UpdateTopHudLine_2_GameInfos_2()
        {
            DoorInstance? door = _deps.GameObject?.Door;
            if (door == null)
            {
                _deps.Diagnostic.AddError($"{nameof(UIManager)}.{nameof(UpdateTopHudLine_2_GameInfos_2)}: DoorInstance is null");
                return;
            }
            PlayerInstance? player = _deps.GameObject?.Player;
            if (player == null)
            {
                _deps.Diagnostic.AddError($"{nameof(UIManager)}.{nameof(UpdateTopHudLine_2_GameInfos_2)}: PlayerInstance is null");
                return;
            }

            string liveString = $"{_deps.Symbol.HearthSymbol}  x{player?.Lives}";
            string keyString = $"{_deps.Symbol.KeyFragmentSymbol}  x{_deps.Inventory.KeyFragment}";
            string doorString = door.IsOpen ? $"{_deps.Symbol.OpenDoorVerticalSymbol} = Open" : $"{_deps.Symbol.ClosedDoorVerticalSymbol} = Closed";

            Console.SetCursorPosition(Program.CursorPosX, Program.CursorPosYTopHud_2);
            Console.Write(BuildCentredLine(liveString, doorString, keyString, _deps.GameBoard.ArraySizeX));

            _deps.Diagnostic.AddCheck($"{nameof(UIManager)}.{nameof(UpdateTopHudLine_2_GameInfos_2)}: UpdateTopHudLine_2_GameInfos_2 successfully.");
        }

        /// <summary>
        /// Builds a centered console HUD line by combining three text zones:
        /// left, middle, and right. Ensures balanced spacing within the given line width.
        /// </summary>
        /// <param name="leftZone">Text aligned to the left section of the HUD line.</param>
        /// <param name="midZone">Text placed in the centered area of the HUD line.</param>
        /// <param name="rightZone">Text aligned to the right section of the HUD line.</param>
        /// <param name="lineMaxLength">Maximum allowed line length in console characters.</param>
        /// <returns>Returns a formatted string containing all zones properly centered.</returns>
        /// <returns></returns>
        private string BuildCentredLine(string leftZone, string midZone, string rightZone, int lineMaxLength)
        {
            // === CALCULATE MIDZONE START POSITION ===
            // Determines where the mid-zone text should begin based on the maximum line width.
            int midZoneStartPosition = lineMaxLength / 2 - midZone.Length / 2;

            // === INITIALIZE LINE WITH LEFT ZONE ===
            string line = leftZone;

            // === CALCULATE AND ADD SPACES BETWEEN LEFT AND MID ZONE ===
            int leftZoneSpace = Math.Max(1, midZoneStartPosition - leftZone.Length);
            line += new string(' ', leftZoneSpace);

            // === ADD MID ZONE ===
            line += midZone;

            // === CALCULATE AND ADD SPACES BETWEEN MID AND RIGHT ZONE ===
            int rightZoneSpace = Math.Max(1, lineMaxLength - line.Length - rightZone.Length);
            line += new string(' ', rightZoneSpace);

            // === ADD RIGHT ZONE ===
            line += rightZone;

            // === RETURN FINAL LINE ===
            return line;
        }

        /// <summary>
        /// Builds the top border line of the bottom HUD frame.
        /// Combines corner, horizontal, and connector symbols for visual separation.
        /// </summary>
        /// <returns>Formatted HUD frame line used as the top boundary of the bottom section.</returns>
        private string BuildBottomHudLine_1()
        {
            // === INITIAL SETUP ===
            int lineMaxLength = _deps.GameBoard.ArraySizeX;

            char midTDownSymbol = _deps.Symbol.WallTDownSymbol;
            char TopSymbol = _deps.Symbol.WallHorizontalSymbol;
            char rightCornerTopSymbol = _deps.Symbol.WallCornerTopRightSymbol;
            char leftCornerTopSymbol = _deps.Symbol.WallCornerTopLeftSymbol;

            const int LEFT_SEGMENT_LENGTH = 14;
            const int RIGHT_SEGMENT_OFFSET = -17;

            // === INITIALIZE LINE ===
            string line = string.Empty;

            // === BUILD STRUCTURE ===
            // Left corner + left segment + middle divider + right segment + right corner.
            line += leftCornerTopSymbol
                + new string(TopSymbol, LEFT_SEGMENT_LENGTH)
                + midTDownSymbol
                + new string(TopSymbol, RIGHT_SEGMENT_OFFSET + lineMaxLength)
                + rightCornerTopSymbol;

            // === RETURN FINAL LINE ===
            return line;
        }

        /// <summary>
        /// Builds the second HUD line containing NPC name and info box.
        /// </summary>
        /// <param name="messangerName">Displayed NPC name.</param>
        /// <param name="infoBox">Formatted info string for the right side.</param>
        /// <returns>HUD line with aligned NPC name and info text.</returns>
        private string BuildBottomHudLine_2(string messangerName, string infoBox)
        {
            // === INITIAL SETUP ===
            int lineMaxLength = _deps.GameBoard.ArraySizeX;
            char verticalHudSymbol = _deps.Symbol.WallVerticalSymbol;

            string leftZone = messangerName;
            string rightZone = infoBox;

            const int SYMBOL_OFFSET = 1;
            const int LEFT_SEGMENT_LENGTH = 14;

            // === SAFETY CHECK ===
            // Ensure the GameBoard and its array are initialized before HUD generation.
            if (_deps.GameBoard == null || _deps.GameBoard.GameBoardArray == null)
            {
                _deps.Diagnostic.AddError($"{nameof(UIManager)}.{nameof(BuildBottomHudLine_2)}: GameBoard or GameBoardArray is null.");
                return string.Empty;
            }

            // === INITIALIZATION ===
            lineMaxLength = _deps.GameBoard.GameBoardArray.GetLength(1);
            int rightZoneMax = lineMaxLength - (LEFT_SEGMENT_LENGTH + SYMBOL_OFFSET);

            string line = string.Empty;

            // === VALIDATE LEFT ZONE LENGTH ===
            // If the NPC name exceeds the allowed width, clear it and log a developer warning.
            if (leftZone.Length > LEFT_SEGMENT_LENGTH)
            {
                leftZone = string.Empty;
                _deps.Diagnostic.AddError($"{nameof(UIManager)}.{nameof(BuildBottomHudLine_2)}: NPC name too long.");
            }
            else
            {
                // Initialize line with the left border and left content.
                line = verticalHudSymbol + leftZone;
            }

            // === LEFT SPACING ===
            // Add spacing between the left content and the middle divider for balanced layout.
            int leftZoneSpace = Math.Max(1, LEFT_SEGMENT_LENGTH + SYMBOL_OFFSET - leftZone.Length - SYMBOL_OFFSET);
            line += new string(' ', leftZoneSpace);

            // === MIDDLE DIVIDER + RIGHT SECTION ===
            // Insert the middle divider and append the right-side message text.
            line += verticalHudSymbol + rightZone;

            // === RIGHT SPACING ===
            // Calculate spacing before the final right border to reach the total HUD width.
            int rightZoneSpace = Math.Max(1, rightZoneMax - rightZone.Length - SYMBOL_OFFSET - SYMBOL_OFFSET);
            line += new string(' ', rightZoneSpace);
            line += verticalHudSymbol;

            // === RETURN FINAL LINE ===
            return line;
        }

        /// <summary>
        /// Builds a connecting divider line between the NPC header section and the message box.
        /// </summary>
        /// <returns>Formatted HUD line dividing the name/info area from the dialogue text area.</returns>
        private string BuildBottomHudLine_3()
        {
            // === INITIAL SETUP ===
            char leftConnectSymbol = _deps.Symbol.WallTRightSymbol;
            char rightConnectSymbol = _deps.Symbol.WallTLeftSymbol;
            char TopSymbol = _deps.Symbol.WallHorizontalSymbol;
            char midTUpSymbol = _deps.Symbol.WallTUpSymbol;

            int lineMaxLength = _deps.GameBoard.ArraySizeX;

            const int SYMBOL_OFFSET = 1;
            const int LEFT_SEGMENT_LENGTH = 15;
            const int RIGHT_SEGMENT_OFFSET = -17;

            // === INITIALIZE LINE ===
            string line = string.Empty;

            // === BUILD STRUCTURE ===
            // Combines the frame components in the following order:
            // Left connector + top segment + middle connector + remaining segment + right connector.
            line += leftConnectSymbol
                + new string(TopSymbol, LEFT_SEGMENT_LENGTH - SYMBOL_OFFSET)
                + midTUpSymbol
                + new string(TopSymbol, RIGHT_SEGMENT_OFFSET + lineMaxLength)
                + rightConnectSymbol;

            // === RETURN FINAL LINE ===
            return line;
        }

        /// <summary>
        /// Builds the fourth HUD line containing the first visible segment of the NPC dialogue text.
        /// </summary>
        /// <param name="npcMessagePart">Substring of the full NPC message to display on this line.</param>
        /// <returns>Formatted string for display inside the bottom HUD message box.</returns>
        private string BuildBottomHudLine_4(string npcMessagePart)
        {
            // === INITIAL SETUP ===
            char SideSymbol = _deps.Symbol.WallVerticalSymbol;

            int lineMaxLength = _deps.GameBoard.ArraySizeX;

            const int SYMBOL_OFFSET = 1;

            string text = npcMessagePart;

            // === INITIALIZE LINE ===
            string line = string.Empty;

            // === BUILD MESSAGE FRAME ===
            // Compose the line with side borders, message text, and balanced spacing.
            line += SideSymbol
                + text
                + new string(' ', lineMaxLength - SYMBOL_OFFSET * 2 - text.Length)
                + SideSymbol;

            // === RETURN FINAL LINE ===
            return line;
        }

        /// <summary>
        /// Builds the fifth HUD line containing the second segment of the NPC dialogue text.
        /// </summary>
        /// <param name="npcMessagePart">Substring of the full NPC message to display on this line.</param>
        /// <returns>Formatted string representing the second line of the NPC message area.</returns>
        private string BuildBottomHudLine_5(string npcMessagePart)
        {
            // === INITIAL SETUP ===
            int lineMaxLength = _deps.GameBoard.ArraySizeX;

            char SideSymbol = _deps.Symbol.WallVerticalSymbol;

            const int SYMBOL_OFFSET = 1;

            string text = npcMessagePart;

            // === INITIALIZE LINE ===
            string line = string.Empty;

            // === BUILD MESSAGE FRAME ===
            // Compose the line with left and right borders, text, and calculated spacing.
            line += SideSymbol
                + text
                + new string(' ', lineMaxLength - SYMBOL_OFFSET * 2 - text.Length)
                + SideSymbol;

            // === RETURN FINAL LINE ===
            return line;
        }

        /// <summary>
        /// Builds the sixth HUD line containing the third segment of the NPC dialogue text.
        /// </summary>
        /// <param name="npcMessagePart">Substring of the full NPC message to display on this line.</param>
        /// <returns>Formatted string representing the third line of the NPC message box.</returns>
        private string BuildBottomHudLine_6(string npcMessagePart)
        {
            // === INITIAL SETUP ===
            char SideSymbol = _deps.Symbol.WallVerticalSymbol;

            int lineMaxLength = _deps.GameBoard.ArraySizeX;

            string text = npcMessagePart;

            const int SYMBOL_OFFSET = 1;

            // === INITIALIZE LINE ===
            string line = string.Empty;

            // === BUILD MESSAGE FRAME ===
            // Construct the line with side borders, text, and balanced spacing.
            line += SideSymbol
                + text + new string(' ', lineMaxLength - SYMBOL_OFFSET * 2 - text.Length)
                + SideSymbol;

            // === RETURN FINAL LINE ===
            return line;
        }

        /// <summary>
        /// Builds a horizontal divider line separating the NPC dialogue section from the answer options.
        /// </summary>
        /// <returns>Formatted divider line visually closing the message box area.</returns>
        private string BuildBottomHudLine_7()
        {
            // === INITIAL SETUP ===
            char leftConnectSymbol = _deps.Symbol.WallTRightSymbol;
            char rightConnectSymbol = _deps.Symbol.WallTLeftSymbol;
            char TopSymbol = _deps.Symbol.WallHorizontalSymbol;

            int lineMaxLength = _deps.GameBoard.ArraySizeX;

            const int SYMBOL_OFFSET = 1;

            // === INITIALIZE LINE ===
            string line = string.Empty;

            // === BUILD DIVIDER LINE ===
            // Combine the left connector, the main top symbols, and the right connector to close the HUD frame.
            line += leftConnectSymbol
                + new string(TopSymbol, lineMaxLength - SYMBOL_OFFSET * 2)
                + rightConnectSymbol;

            // === RETURN FINAL LINE ===
            return line;
        }

        /// <summary>
        /// Builds the centered answer options ([1][2][3]) line for NPC dialogues.
        /// Ensures that each answer respects the ≤10 character rule.
        /// </summary>
        /// <param name="answer_1">Option 1 text (≤10 chars).</param>
        /// <param name="answer_2">Option 2 text (≤10 chars).</param>
        /// <param name="answer_3">Option 3 text (≤10 chars).</param>
        /// <returns>Formatted line containing all three options centered in the HUD.</returns>
        /// <remarks>Validates length against HUD Content Rules in the class header.</remarks>
        private string BuildBottomHudLineAnswer_8(string answer_1, string answer_2, string answer_3)
        {
            // === INITIAL SETUP ===
            const int SYMBOL_OFFSET = 2;
            char SideSymbol = _deps.Symbol.WallVerticalSymbol;
            int lineMaxLength = _deps.GameBoard.ArraySizeX;
            //List<string> answers = new List<string> { answer_1, answer_2, answer_3 };

            //answers = _deps.Random.GetRandomElements(answers, answers.Count);

            // === INITIALIZE LINE ===

            string coreLine = BuildCentredLine("[1] " + answer_1, "[2] " + answer_2, "[3] " + answer_3, lineMaxLength - SYMBOL_OFFSET);

            string fullLine = $"{SideSymbol}{coreLine}{SideSymbol}";

            if (fullLine.Length > _deps.GameBoard.ArraySizeX)
            {
                _deps.Diagnostic.AddWarning($"{nameof(UIManager)}.{nameof(BuildBottomHudLineAnswer_8)}: Answers too long ({fullLine.Length}/{_deps.GameBoard.ArraySizeX}). Line was reset.");
                return fullLine = BuildCentredLine(string.Empty, string.Empty, string.Empty, _deps.GameBoard.ArraySizeX);
            }

            return fullLine;
        }

        /// <summary>
        /// Builds the bottom boundary line of the HUD frame.
        /// Closes the visual structure with corner and horizontal symbols.
        /// </summary>
        /// <returns>Formatted line representing the lower edge of the HUD box.</returns>
        private string BuildBottomHudLine_9()
        {
            // === INITIAL SETUP ===
            char leftCornerBottomSymbol = _deps.Symbol.WallCornerBottomLeftSymbol;
            char rightCornerBottomSymbol = _deps.Symbol.WallCornerBottomRightSymbol;
            char TopSymbol = _deps.Symbol.WallHorizontalSymbol;
            int lineMaxLength = _deps.GameBoard.ArraySizeX;
            const int SYMBOL_OFFSET = 1;

            // === INITIALIZE LINE ===
            string line = string.Empty;

            // === BUILD BOTTOM FRAME LINE ===
            // Combine left and right corners with the central horizontal wall segment.
            line += leftCornerBottomSymbol
                + new string(TopSymbol, lineMaxLength - SYMBOL_OFFSET * 2)
                + rightCornerBottomSymbol;

            // === RETURN FINAL LINE ===
            return line;
        }

        /// <summary>
        /// Splits a long NPC dialogue message into multiple lines based on the HUD width.
        /// </summary>
        /// <param name="npcMessage">The NPC message (≤117 characters recommended).</param>
        /// <param name="maxWidth">Maximum character width per HUD line.</param>
        /// <returns>List of properly formatted dialogue lines for display.</returns>
        /// <remarks>Automatically truncates and adds ellipsis (…) if text exceeds HUD limits.</remarks>
        private List<string> CutNpcMessageIntoLines(string npcMessage, int maxWidth)
        {
            const int SYMBOL_OFFSET = 2;
            maxWidth -= SYMBOL_OFFSET;
            // === INITIALIZE STORAGE ===
            // Create a list to store the formatted message lines.
            List<string> lines = new List<string>();

            // Holds the current line being built.
            string currentLine = string.Empty;

            // === SPLIT MESSAGE INTO WORDS ===
            // Separate the NPC message by spaces to preserve whole words.
            foreach (string word in npcMessage.Split(' '))
            {
                // === CHECK LINE WIDTH ===
                // If adding the next word exceeds the maximum width, save the current line.
                if ((currentLine + word).Length > maxWidth)
                {
                    lines.Add(currentLine);
                    currentLine = string.Empty;
                }

                // === ADD SPACE BETWEEN WORDS ===
                // If currentLine already has text, add a space before appending the next word.
                if (currentLine.Length > 0)
                {
                    currentLine += " ";
                }

                // === APPEND WORD TO LINE ===
                currentLine += word;

                if (currentLine.Length > maxWidth)
                {
                    string truncated = currentLine.Substring(0, maxWidth - 4) + "…";
                    lines.Add(truncated);
                    currentLine = string.Empty;
                }
            }

            // === FINALIZE ===
            // Add the last line if any content remains.
            if (currentLine.Length > 0)
            {
                if (currentLine.Length > maxWidth)
                {
                    currentLine = currentLine.Substring(0, maxWidth - 4) + "…";
                }
                lines.Add(currentLine);
            }

            // Return the list of formatted lines.
            return lines;
        }
    }
}