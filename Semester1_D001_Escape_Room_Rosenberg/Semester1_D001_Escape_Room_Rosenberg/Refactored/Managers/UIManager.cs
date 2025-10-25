using Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies;
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
    /// Central UI controller responsible for composing and rendering all Heads-Up Display (HUD) elements.
    /// </summary>
    /// <remarks>
    /// The <see cref="UIManager"/> handles both the top and bottom HUD sections that provide 
    /// real-time feedback, system messages, and NPC dialogue boxes within the Escape Room game.  
    /// It manages text alignment, symbol-based rendering, and layout consistency 
    /// using the dependency configuration provided by <see cref="UIManagerDependencies"/>.
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
        private DateTime _startTime;


        //TODO Später in richtige klasse machen
        string _playerName = "Joschi";
        int _lives = 3;
        int _keys = 3;
        bool _isDoorOpen = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="UIManager"/> class with dependency injection.
        /// </summary>
        /// <param name="uIManagerDependencies">
        /// Provides references to all external managers and services required by the UI system, 
        /// such as <see cref="GameBoardManager"/>, <see cref="SymbolsManager"/>, 
        /// <see cref="PrintManager"/>, and <see cref="RandomManager"/>.
        /// </param>
        public UIManager(UIManagerDependencies uIManagerDependencies)
        {
            _deps = uIManagerDependencies;
        }

        /// <summary>
        /// Initializes the internal list of NPC messages with predefined sample entries.  
        /// Used for testing and placeholder dialogues until dynamic loading is implemented.
        /// </summary>
        /// <remarks>
        /// Currently adds one sample question for demonstration purposes.  
        /// This will later be replaced by dynamic data from an external source.
        /// </remarks>
        public void InitializeNpcMessages()
        {
            // Eine Test-Quizfrage (genau 72 Zeichen)
            _npcMessages.Add("Wie viele Speicherbits hat ein Byte und welche Werte kann es darstellen?");

            // Optional: weitere vorbefüllte Fragen kannst du hier anhängen
            // _npcMessages.Add("...");
        }

        /// <summary>
        /// Retrieves a random message from the NPC message list.
        /// </summary>
        /// <remarks>
        /// If the list is empty, a fallback message ("Keine Fragen verfügbar.") is returned.  
        /// Uses the injected <see cref="RandomManager"/> for consistent seeded randomness.
        /// </remarks>
        /// <returns>
        /// A randomly selected NPC message string.
        /// </returns>
        public string GetRandomNpcMessage()
        {
            if (_npcMessages.Count == 0)
                return "Keine Fragen verfügbar.";

            // Variante A (reines C# / Console):

            int idx = _deps.Random.Random.Next(0, _npcMessages.Count); // obere Grenze EXKLUSIV → kein Count-1 nötig
            return _npcMessages[idx];

            // Variante B (Unity):
            // int idx = UnityEngine.Random.Range(0, _npcMessages.Count); // int: obere Grenze EXKLUSIV

            // return _npcMessages[idx];
        }

        /// <summary>
        /// Starts or resets the internal gameplay timer for HUD display purposes.
        /// </summary>
        /// <remarks>
        /// Captures the current system time. The elapsed duration is later shown
        /// in the top HUD through <see cref="BuildTopHud"/>.
        /// </remarks>
        public void StartTimer()
        {
            _startTime = DateTime.Now;
        }

        /// <summary>
        /// Builds and displays the top Heads-Up Display (HUD) section of the Escape Room game.
        /// </summary>
        /// <remarks>
        /// The top HUD provides key runtime information such as elapsed time, player name, 
        /// current level, remaining lives, collected keys, and door status. 
        /// It is printed directly to the console using the configured <see cref="PrintManager"/>.
        /// </remarks>
        private void BuildTopHud()
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
            int boardWidth = _deps.GameBoard.GameBoardArray.GetLength(1);
            // Calculate elapsed time since the start of the game.
            TimeSpan elapsed = DateTime.Now - _startTime;

            // === TIME AND LEVEL DISPLAY ===
            // Format the elapsed time using the configured time symbol.
            string timeString = $"{_deps.Symbol.TimeWatchSymbol} {elapsed.Minutes:D2}:{elapsed.Seconds:D2}";

            // Create a consistent layout with fixed width for balanced alignment.
            string namePart = $"{_deps.Symbol.PlayerSymbol} {_playerName}";
            string lvlPart = $"Lvl = {Program.CurrentLevel}";
            string timePart = timeString;

            // === FIRST ROW OUTPUT ===
            // Display the player name, current level, and elapsed time.
            Console.SetCursorPosition(0, 0);
            Console.Write(BuildCentredLine(namePart, lvlPart, timePart, boardWidth));

            // === SECOND ROW OUTPUT ===
            // Display remaining lives, key fragments, and door state (open/closed).
            string liveString = $"{_deps.Symbol.HearthSymbol}  x{_lives}";
            string keyString = $"{_deps.Symbol.KeyFragmentSymbol}  x{_keys}";
            string doorString = _isDoorOpen ? $"{_deps.Symbol.OpenDoorVerticalSymbol} = Open" : $"{_deps.Symbol.ClosedDoorVerticalSymbol} = Closed";

            // === SCOND ROW OUTPUT ===
            // Print the second row to the console.
            Console.SetCursorPosition(0, 1);
            Console.Write(BuildCentredLine(liveString, doorString, keyString, boardWidth));

            // === SEPARATOR LINE ===
            // Draws a visual divider beneath the HUD.
            Console.SetCursorPosition(0, 2);
            Console.WriteLine(new string('─', boardWidth));
        }

        /// <summary>
        /// Builds a fully centered HUD line by aligning a left, middle, and right zone
        /// within a defined maximum line width.
        /// </summary>
        /// <remarks>
        /// This method dynamically positions the middle section (<paramref name="midZone"/>)  
        /// so that it appears horizontally centered between the left and right zones 
        /// based on the specified total line width.  
        /// It automatically calculates and inserts appropriate spacing on both sides 
        /// to maintain symmetrical alignment across the entire line.  
        /// This function is particularly useful for layouting HUD headers, dividers, 
        /// or informational text rows that require central alignment between static elements.
        /// </remarks>
        /// <param name="leftZone">
        /// The text or symbol block displayed on the left side of the line.
        /// </param>
        /// <param name="midZone">
        /// The central text or content block to be horizontally centered.
        /// </param>
        /// <param name="rightZone">
        /// The text or symbol block displayed on the right side of the line.
        /// </param>
        /// <param name="lineMaxLength">
        /// The total width (in characters) available for the line construction.
        /// </param>
        /// <returns>
        /// A formatted string combining all three zones with automatically balanced spacing.
        /// </returns>
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
        /// Builds the first connecting line of the bottom Heads-Up Display (HUD),
        /// forming the upper frame border for the bottom HUD section.
        /// </summary>
        /// <remarks>
        /// This method generates the top connection line of the bottom HUD frame using 
        /// predefined wall and corner symbols from the <see cref="_deps.Symbol"/> configuration.  
        /// It combines left and right corner symbols, a horizontal wall segment, and a middle divider 
        /// to ensure that the HUD frame remains visually symmetrical and aligned with the current game board width.
        /// </remarks>
        /// <returns>
        /// A formatted string representing the top boundary line of the bottom HUD frame.
        /// </returns>
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
        /// Builds the second line of the bottom Heads-Up Display (HUD),
        /// displaying the NPC name on the left and the NPC dialogue message on the right.
        /// </summary>
        /// <remarks>
        /// This method constructs the second bottom HUD line used for NPC interactions.  
        /// It displays the NPC name on the left and the dialogue message on the right,
        /// separated by a vertical divider symbol.  
        /// The function includes validation for null references and string overflow, ensuring that
        /// layout symmetry and console alignment remain intact during rendering.
        /// </remarks>
        /// <param name="npcName">
        /// The left-aligned text section, representing the NPC's display name.
        /// </param>
        /// <param name="infoBox">
        /// The right-aligned text section, containing the infobox dialogue or message content.
        /// </param>
        /// <returns>
        /// A formatted string representing the complete second line of the bottom HUD,
        /// properly spaced and aligned within the frame.
        /// </returns>
        private string BuildBottomHudLine_2(string npcName, string infoBox)
        {
            // === INITIAL SETUP ===
            int lineMaxLength = _deps.GameBoard.ArraySizeX;
            char verticalHudSymbol = _deps.Symbol.WallVerticalSymbol;

            string leftZone = npcName;
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
        /// Builds the third connecting line of the bottom Heads-Up Display (HUD),
        /// representing the lower divider between the HUD frame and the NPC dialogue area.
        /// </summary>
        /// <remarks>
        /// This method constructs the bottom connecting line of the HUD frame using predefined wall and corner symbols 
        /// from the <see cref="_deps.Symbol"/> configuration.  
        /// It combines left and right connection corners, a horizontal wall segment, and a middle connector symbol 
        /// to maintain a consistent visual alignment with the top HUD borders.
        /// </remarks>
        /// <returns>
        /// A formatted string representing the bottom connecting divider line of the HUD frame.
        /// </returns>
        private string BuildBottomHudLine_3()
        {
            // === INITIAL SETUP ===            
            char leftConnectSymbol = _deps.Symbol.WallCornerBottomLeftSymbol;
            char rightConnectSymbol = _deps.Symbol.WallCornerBottomRightSymbol;
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
        /// Builds the fourth line of the bottom Heads-Up Display (HUD),
        /// displaying the first row of NPC dialogue text within the message frame.
        /// </summary>
        /// <remarks>
        /// This method constructs a single HUD text line that displays an NPC message 
        /// inside a bordered frame.  
        /// It aligns the text between two vertical wall symbols and automatically adjusts 
        /// spacing based on the message length and HUD width.  
        /// Used as the first visible dialogue row in the NPC message box.
        /// </remarks>
        /// <param name="npcMessagePart">
        /// The text part content to be displayed in the first line of the NPC dialogue section.
        /// </param>
        /// <returns>
        /// A formatted string representing the first dialogue line framed with vertical HUD borders.
        /// </returns>
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
        /// Builds the fifth line of the bottom Heads-Up Display (HUD),
        /// displaying the second row of NPC dialogue text within the message frame.
        /// </summary>
        /// <remarks>
        /// This method constructs the second visible dialogue line for NPC messages inside the HUD box.  
        /// It mirrors the layout of <see cref="BuildBottomHudLine_4"/> to maintain consistent text alignment 
        /// and spacing across multi-line dialogues.  
        /// The message is framed between two vertical wall symbols and padded dynamically 
        /// according to the total HUD width and text length.
        /// </remarks>
        /// <param name="npcMessagePart">
        /// The text content to be displayed in the second line of the NPC dialogue section.
        /// </param>
        /// <returns>
        /// A formatted string representing the second dialogue line framed with vertical HUD borders.
        /// </returns>
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
        /// Builds the sixth line of the bottom Heads-Up Display (HUD),
        /// displaying the third row of NPC dialogue text within the message frame.
        /// </summary>
        /// <remarks>
        /// This method constructs the third visible dialogue line for NPC messages inside the HUD box.  
        /// It mirrors the structure of <see cref="BuildBottomHudLine_4"/> and <see cref="BuildBottomHudLine_5"/>,
        /// ensuring that multi-line dialogue text remains visually consistent and properly aligned.  
        /// The message is centered between two vertical wall symbols and dynamically spaced 
        /// according to the total HUD width and text length.
        /// </remarks>
        /// <param name="npcMessagePart">
        /// The text content to be displayed in the third line of the NPC dialogue section.
        /// </param>
        /// <returns>
        /// A formatted string representing the third dialogue line framed with vertical HUD borders.
        /// </returns>
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
        /// Builds the seventh and final line of the bottom Heads-Up Display (HUD),
        /// closing the dialogue frame with a lower border divider.
        /// </summary>
        /// <remarks>
        /// This method constructs the closing divider line at the bottom of the HUD dialogue frame.  
        /// It combines left and right connector symbols with a continuous horizontal wall segment, 
        /// visually completing the HUD’s bordered layout.  
        /// The structure ensures alignment with the upper HUD frame lines and maintains consistent width 
        /// across the entire dialogue box.
        /// </remarks>
        /// <returns>
        /// A formatted string representing the closing divider line of the bottom HUD frame.
        /// </returns>
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

        //TODO noch bauen richtig aktuell nur rahmen hier kommen 3 antworten rein
        /// <summary>
        /// Builds the eighth line of the bottom Heads-Up Display (HUD),
        /// currently serving as a placeholder row for future player input or NPC responses.
        /// </summary>
        /// <remarks>
        /// This version only renders an empty framed line.  
        /// In future iterations, this will host selectable answer options or dialogue input fields.
        /// </remarks>
        /// <returns>
        /// A framed but currently empty line string within the HUD structure.
        /// </returns>
        private string BuildBottomHudLine_8()
        {
            // === INITIAL SETUP ===
            char SideSymbol = _deps.Symbol.WallVerticalSymbol;
            int lineMaxLength = _deps.GameBoard.ArraySizeX;
            const int SYMBOL_OFFSET = 1;

            // === INITIALIZE LINE ===
            string line = string.Empty;

            line += SideSymbol
                + new string(' ', lineMaxLength - SYMBOL_OFFSET * 2)
                + SideSymbol;


            return line;
        }

        /// <summary>
        /// Builds the ninth and final structural line of the bottom Heads-Up Display (HUD),
        /// forming the absolute bottom border of the dialogue frame.
        /// </summary>
        /// <remarks>
        /// This method constructs the closing bottom boundary of the HUD frame using predefined wall corner 
        /// and horizontal symbols from the <see cref="_deps.Symbol"/> configuration.  
        /// It connects the left and right bottom corners with a continuous horizontal wall segment, 
        /// visually completing the HUD enclosure and ensuring perfect alignment with the game board width.  
        /// This line marks the final structural element in the bottom HUD rendering sequence.
        /// </remarks>
        /// <returns>
        /// A formatted string representing the complete bottom boundary line of the HUD frame.
        /// </returns>
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


        public List<string> CutNpcMessageIntoLines(string npcMessage, int maxWidth)
        {
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
            }

            // === FINALIZE ===
            // Add the last line if any content remains.
            if (currentLine.Length > 0)
            {
                lines.Add(currentLine);
            }

            // Return the list of formatted lines.
            return lines;
        }

        /// <summary>
        /// Fills and prepares all bottom Heads-Up Display (HUD) lines with NPC and message data,
        /// dynamically composing the entire dialogue box content.
        /// </summary>
        /// <remarks>
        /// This method initializes and assembles all nine bottom HUD lines to form the complete NPC dialogue frame.  
        /// It generates header, divider, and text sections by invoking the respective `BuildBottomHudLine_X()` methods 
        /// and injects NPC name, message text, and additional info symbols into their corresponding HUD segments.  
        /// The text is automatically split into multiple rows using <see cref="CutNpcMessageIntoLines"/> 
        /// to ensure optimal fit within the available HUD width.  
        /// This function handles data binding and sequence composition but does not render the HUD itself 
        /// (see <see cref="PrintHud"/> for the output process).
        /// </remarks>
        /// <param name="messangerName">
        /// The NPC name displayed in the upper-left section of the HUD.
        /// </param>
        /// <param name="infoBoxstring">
        /// The descriptive info string combined with symbol data (e.g. “Keys: x2” or “Hearts: x3”).
        /// </param>
        /// <param name="symbol">
        /// The symbol character representing the item or info type displayed in the info box.
        /// </param>
        /// <param name="count">
        /// The numerical value displayed next to the symbol (e.g. collected keys or items).
        /// </param>
        public void FillUpBottomHud(string messangerName, string infoBoxstring,string message, char symbol, int count)
        {
            if (_deps.GameBoard == null || _deps.GameBoard.GameBoardArray == null)
            {
                _deps.Diagnostic.AddError($"{nameof(UIManager)}.{nameof(FillUpBottomHud)}: Missing GameBoard reference.");
                return;
            }

            // === INITIAL SETUP ===
            // Construct the info box string and define layout parameters.
            string infoBoxSymbol = $"{symbol} x{count}";
            string infoBox = infoBoxstring + infoBoxSymbol;

            const int SYMBOL_OFFSET = 1;

            int innerWidth = _deps.GameBoard.ArraySizeX - SYMBOL_OFFSET * 2;
            // === TEST MESSAGE SETUP ===
            //TODO ersetzen später npc messages
            // Initialize message data for temporary visualization.
            InitializeNpcMessages();

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
            _bottomHudLine_8 = BuildBottomHudLine_8();
            _bottomHudLine_9 = BuildBottomHudLine_9();
        }

        /// <summary>
        /// Renders the complete bottom Heads-Up Display (HUD) in the console output,
        /// printing all pre-built HUD lines sequentially to form the full dialogue frame.
        /// </summary>
        /// <remarks>
        /// This method outputs each preconstructed line of the bottom HUD to the console in the correct order,  
        /// visually forming the full multi-line dialogue box used for NPC interactions.  
        /// The printed structure includes upper connectors, dialogue rows, divider lines, and the final boundary line.  
        /// Each line is assumed to be previously built and stored in its corresponding `_bottomHudLine_X` variable.  
        /// This method does not perform any layout calculations; it only handles the final rendering sequence.
        /// </remarks>
        public void PrintHud()
        {
            Console.WriteLine(_bottomHudLine_1);
            Console.WriteLine(_bottomHudLine_2);
            Console.WriteLine(_bottomHudLine_3);
            Console.WriteLine(_bottomHudLine_4);
            Console.WriteLine(_bottomHudLine_5);
            Console.WriteLine(_bottomHudLine_6);
            Console.WriteLine(_bottomHudLine_7);
            Console.WriteLine(_bottomHudLine_8);
            Console.WriteLine(_bottomHudLine_9);
        }
    }
}
