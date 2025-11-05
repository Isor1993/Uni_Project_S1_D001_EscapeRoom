using Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Door;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Player;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.Managers
{
    /// <summary>
    /// TODO Wegen Zeitproblemen nicht komplet optimiert. gegen fehler sichern von zu langen strings in hud. Fillbuttomhud zu viele parameter
    /// Central UI controller responsible for composing and rendering all Heads-Up Display (HUD) elements.
    /// </summary>
    /// <remarks>
    /// The <see cref="UIManager"/> handles both the top and bottom HUD sections that display  
    /// player stats, system messages, NPC dialogues, and interactive answer options.  
    /// It ensures visual consistency across variable board sizes using proportional symbol layouts  
    /// and offers several helper methods for building text-aligned HUD frames dynamically.
    /// </remarks>
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
        /// Initializes a new instance of the <see cref="UIManager"/> class.
        /// </summary>
        /// <param name="uIManagerDependencies">Provides access to all required managers and configuration symbols.</param>
        public UIManager(UIManagerDependencies uIManagerDependencies)
        {
            _deps = uIManagerDependencies;
            _deps.Diagnostic.AddCheck($"{nameof(UIManager)}: Initialized successfully.");
        }

        /// <summary>
        /// Builds and prints the top HUD section displaying player stats and score.
        /// </summary>
        /// <remarks>
        /// Includes safety validation and renders a separator line for visual structure.
        /// </remarks>
        public void BuildTopHud()
        {
            // === SAFETY CHECK ===
            // Ensures that the game board and its array are initialized before attempting to build the HUD.
            if (_deps.GameBoard == null || _deps.GameBoard.GameBoardArray == null)
            {
                _deps.Diagnostic.AddError($"{nameof(UIManager)}.{nameof(BuildTopHud)}: GameBoard or GameBoardArray is null.");
                return;
            }
            // === INITIALIZATION ===
            // Retrieve the maximum board width for proper HUD alignment.            
            UpdateTopHudLine_1();
            UpdateTopHudLine_2();


            // === SEPARATOR LINE ===
            // Draws a visual divider beneath the HUD.
            Console.SetCursorPosition(Program.CursorPosX, Program.CursorPosYTopHud_3);
            Console.WriteLine(new string('─', _deps.GameBoard.ArraySizeX));
        }

        /// <summary>
        /// Updates and renders the first line of the top HUD (Player name, level, score).
        /// </summary>
        public void UpdateTopHudLine_1()
        {
            if(_deps.GameObject==null)
            {

            }

            PlayerInstance? player = _deps.GameObject?.Player;
            if (player == null)
            {

                _deps.Diagnostic.AddError($"{nameof(UIManager)}.{nameof(UpdateTopHudLine_1)}: Playerinstance is null");
                return;
            }

            // Calculate elapsed time since the start of the game.
            int score = _deps.Inventory.Score;

            // === TIME AND LEVEL DISPLAY ===
            // Format the elapsed time using the configured time symbol.
            string scoreString = $"Score: {score} ";

            // Create a consistent layout with fixed width for balanced alignment.
            string namePart = $"{_deps.Symbol.PlayerSymbol} {player?.Name}";
            string lvlPart = $"Lvl = {_deps.Level.CurrentLvl}";
            string scorePart = scoreString;

            // === FIRST ROW OUTPUT ===
            // Display the player name, current level, and elapsed time.
            Console.SetCursorPosition(Program.CursorPosX, Program.CursorPosYTopHudStart);
            Console.Write(BuildCentredLine(namePart, lvlPart, scorePart, _deps.GameBoard.ArraySizeX));
        }

        /// <summary>
        /// Updates and renders the second line of the top HUD (Lives, Door state, Key fragments).
        /// </summary>
        public void UpdateTopHudLine_2()
        {
            DoorInstance? door = _deps.GameObject?.Door;
            if (door == null)
            {

                _deps.Diagnostic.AddError($"{nameof(UIManager)}.{nameof(UpdateTopHudLine_2)}: DoorInstance is null");
                return;
            }
            PlayerInstance? player = _deps.GameObject?.Player;
            if (player == null)
            {

                _deps.Diagnostic.AddError($"{nameof(UIManager)}.{nameof(UpdateTopHudLine_2)}: PlayerInstance is null");
                return;
            }
            // === SECOND ROW OUTPUT ===
            // Display remaining lives, key fragments, and door state (open/closed).
            string liveString = $"{_deps.Symbol.HearthSymbol}  x{player?.Heart}";
            string keyString = $"{_deps.Symbol.KeyFragmentSymbol}  x{_deps.Inventory.KeyFragment}";
            string doorString = door.IsOpen ? $"{_deps.Symbol.OpenDoorVerticalSymbol} = Open" : $"{_deps.Symbol.ClosedDoorVerticalSymbol} = Closed";

            // === SCOND ROW OUTPUT ===
            // Print the second row to the console.
            Console.SetCursorPosition(Program.CursorPosX, Program.CursorPosYTopHud_2);
            Console.Write(BuildCentredLine(liveString, doorString, keyString, _deps.GameBoard.ArraySizeX));

        }

        /// <summary>
        /// Builds a single horizontally aligned HUD line with three centered text zones.
        /// </summary>
        /// <param name="leftZone">Text to display on the left side.</param>
        /// <param name="midZone">Text centered in the middle.</param>
        /// <param name="rightZone">Text aligned to the right.</param>
        /// <param name="lineMaxLength">Total line width for alignment calculation.</param>
        /// <returns>A fully composed, space-balanced HUD line.</returns>
        public string BuildCentredLine(string leftZone, string midZone, string rightZone, int lineMaxLength)
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
        /// Builds the top frame line of the bottom HUD dialogue box.
        /// </summary>
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
        /// Builds the second line of the bottom HUD that displays the NPC name and info box.
        /// </summary>
        /// <param name="messangerName">The name of the NPC or message sender.</param>
        /// <param name="infoBox">The information box text, such as item or key info.</param>
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
        /// Builds the third bottom HUD line, serving as the horizontal separator.
        /// </summary>
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
        /// Builds one of the NPC message lines for the bottom HUD dialogue box.
        /// </summary>
        /// <param name="npcMessagePart">The message text to render within the dialogue frame.</param>
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
        /// Builds one of the NPC message lines for the bottom HUD dialogue box.
        /// </summary>
        /// <param name="npcMessagePart">The message text to render within the dialogue frame.</param>
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
        /// Builds one of the NPC message lines for the bottom HUD dialogue box.
        /// </summary>
        /// <param name="npcMessagePart">The message text to render within the dialogue frame.</param>
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
        /// Builds the horizontal divider line separating dialogue text from the answer section.
        /// </summary>
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
        /// Maximal 10 Zeichen pro Antwort bei arraygröße.x 45
        /// </summary>
        /// Builds the NPC answer selection line with randomized answer order.
        /// </summary>
        /// <param name="answer_1"></param>
        /// <param name="answer_2"></param>
        /// <param name="answer_3"></param>
        /// <returns></returns>
        private string BuildBottomHudLineAnswer_8(string answer_1, string answer_2, string answer_3)
        {
            // === INITIAL SETUP ===
            const int SYMBOL_OFFSET = 2;
            char SideSymbol = _deps.Symbol.WallVerticalSymbol;
            int lineMaxLength = _deps.GameBoard.ArraySizeX;
            //List<string> answers = new List<string> { answer_1, answer_2, answer_3 };


            //answers = _deps.Random.GetRandomElements(answers, answers.Count);

            // === INITIALIZE LINE ===            

            string coreLine = BuildCentredLine("[1] " + answer_1, "[2] " + answer_2, "[3] " + answer_3 , lineMaxLength-SYMBOL_OFFSET);

            string fullLine = $"{SideSymbol}{coreLine}{SideSymbol}";

            if (fullLine.Length > _deps.GameBoard.ArraySizeX)
            {
                _deps.Diagnostic.AddWarning($"{nameof(UIManager)}.{nameof(BuildBottomHudLineAnswer_8)}: Answers too long ({fullLine.Length}/{_deps.GameBoard.ArraySizeX}). Line was reset.");
                return fullLine = BuildCentredLine(string.Empty, string.Empty, string.Empty, _deps.GameBoard.ArraySizeX);
            }


            return fullLine;
        }

        /// <summary>
        /// Builds the closing bottom frame line for the HUD dialogue box.
        /// </summary>
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
        /// Splits a long NPC message string into multiple lines, ensuring each line fits within a specified width limit.
        /// </summary>
        /// <param name="npcMessage">
        /// The original text message spoken by the NPC.  
        /// It can include spaces, punctuation, and multiple words that need to be formatted for HUD display.
        /// </param>
        /// <param name="maxWidth">
        /// The maximum number of characters allowed per line, typically derived from the HUD or board width.
        /// </param>
        /// <returns>
        /// Returns a <see cref="List{T}"/> of formatted message lines,  
        /// where each string fits within the maximum width constraint without breaking words.
        /// </returns>
        public List<string> CutNpcMessageIntoLines(string npcMessage, int maxWidth)
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

        /// <summary>
        /// Constructs and fills the entire bottom HUD section, including the dialogue box,  
        /// NPC name, info box, message text, and randomized answer options.
        /// </summary>
        /// <param name="messangerName">
        /// The name of the NPC or entity sending the message, displayed in the top-left of the dialogue box.
        /// </param>
        /// <param name="infoBoxstring">
        /// The base text content for the info box (e.g., “Key”, “Item”), combined with the visual symbol and quantity.
        /// </param>
        /// <param name="message">
        /// The dialogue or message text displayed inside the HUD message area.  
        /// This text will automatically wrap into multiple lines depending on width.
        /// </param>
        /// <param name="symbol">
        /// The visual symbol (e.g., key icon, item icon) displayed next to the info box text.
        /// </param>
        /// <param name="count">
        /// The quantity of the symbol item, typically used to display counts like “x3” or “x5”.
        /// </param>
        /// <param name="answer_1">
        /// The first dialogue answer option displayed below the message text.
        /// </param>
        /// <param name="answer_2">
        /// The second dialogue answer option displayed below the message text.
        /// </param>
        /// <param name="answer_3">
        /// The third dialogue answer option displayed below the message text.
        /// </param>
        public void FillUpBottomHud(string messangerName, string infoBoxstring, string message, char symbol, int count, string answer_1, string answer_2, string answer_3)
        {
            if (_deps.GameBoard == null || _deps.GameBoard.GameBoardArray == null)
            {
                _deps.Diagnostic.AddError($"{nameof(UIManager)}.{nameof(FillUpBottomHud)}: Missing GameBoard reference.");
                return;
            }

            // === INITIAL SETUP ===
            // Construct the info box string and define layout parameters.
            // no saftey if count is too high
            string infoBoxSymbol = $"{symbol} x{count}";
            string infoBox = infoBoxstring + infoBoxSymbol;

            const int SYMBOL_OFFSET = 1;

            int innerWidth = _deps.GameBoard.ArraySizeX - SYMBOL_OFFSET * 2;

            // Cut the random NPC message into multiple lines based on the available HUD width.            
            List<string> cutTextLines = CutNpcMessageIntoLines(message, innerWidth);


            // === BUILD HUD STRUCTURE ===
            // Compose the full HUD from top to bottom using dedicated build methods.
            _bottomHudLine_1 = BuildBottomHudLine_1();
            _bottomHudLine_2 = BuildBottomHudLine_2(messangerName, infoBox);
            _bottomHudLine_3 = BuildBottomHudLine_3();
            _bottomHudLine_4 = BuildBottomHudLine_4(cutTextLines.Count > 0 ? cutTextLines[0] : "");
            _bottomHudLine_5 = BuildBottomHudLine_5(cutTextLines.Count > 1 ? cutTextLines[1] : "");
            _bottomHudLine_6 = BuildBottomHudLine_6(cutTextLines.Count > 2 ? cutTextLines[2] : "");
            _bottomHudLine_7 = BuildBottomHudLine_7();
            _bottomHudLine_8 = BuildBottomHudLineAnswer_8(answer_1, answer_2, answer_3);
            _bottomHudLine_9 = BuildBottomHudLine_9();
        }


        public void FillUpBottomHud(string messangerName, string infoBoxstring, string message, char symbol, string answer_1, string answer_2, string answer_3)
        {
            if (_deps.GameBoard == null || _deps.GameBoard.GameBoardArray == null)
            {
                _deps.Diagnostic.AddError($"{nameof(UIManager)}.{nameof(FillUpBottomHud)}: Missing GameBoard reference.");
                return;
            }

            // === INITIAL SETUP ===
            // Construct the info box string and define layout parameters.
            string infoBoxSymbol = $"{symbol}";
            string infoBox = infoBoxstring + infoBoxSymbol;

            const int SYMBOL_OFFSET = 1;

            int innerWidth = _deps.GameBoard.ArraySizeX - SYMBOL_OFFSET * 2;

            // Cut the random NPC message into multiple lines based on the available HUD width.            
            List<string> cutTextLines = CutNpcMessageIntoLines(message, innerWidth);


            // === BUILD HUD STRUCTURE ===
            // Compose the full HUD from top to bottom using dedicated build methods.
            _bottomHudLine_1 = BuildBottomHudLine_1();
            _bottomHudLine_2 = BuildBottomHudLine_2(messangerName, infoBox);
            _bottomHudLine_3 = BuildBottomHudLine_3();
            _bottomHudLine_4 = BuildBottomHudLine_4(cutTextLines.Count > 0 ? cutTextLines[0] : "");
            _bottomHudLine_5 = BuildBottomHudLine_5(cutTextLines.Count > 1 ? cutTextLines[1] : "");
            _bottomHudLine_6 = BuildBottomHudLine_6(cutTextLines.Count > 2 ? cutTextLines[2] : "");
            _bottomHudLine_7 = BuildBottomHudLine_7();
            _bottomHudLine_8 = BuildBottomHudLineAnswer_8(answer_1, answer_2, answer_3);
            _bottomHudLine_9 = BuildBottomHudLine_9();
        }

        /// <summary>
        /// Prints all lines of the bottom HUD to the console output.
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
    }
}