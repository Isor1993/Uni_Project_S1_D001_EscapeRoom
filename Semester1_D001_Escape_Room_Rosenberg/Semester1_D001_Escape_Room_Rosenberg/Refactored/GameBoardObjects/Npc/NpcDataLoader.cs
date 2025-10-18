using System;
using System.Collections.Generic;
using System.IO;
using System.Security;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Npc
{
    /// <summary>
    /// Handles loading and parsing of NPC data from external text files.
    /// Creates and returns a list of <see cref="NpcInstance"/> objects based on file contents.
    /// Includes extensive exception handling and diagnostic logging for reliability.
    /// </summary>
    internal class NpcDataLoader
    {
        // === Dependencies ===
        private readonly DiagnosticsManager _diagnostics;
        private readonly SymbolsManager _symbolsManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="NpcDataLoader"/> class.
        /// </summary>
        /// <param name="diagnostics">Reference to the diagnostics manager used for logging messages and exceptions.</param>
        /// <param name="symbolsManager">Reference to the symbols manager providing NPC-related symbols.</param>
        public NpcDataLoader(DiagnosticsManager diagnostics, SymbolsManager symbolsManager)
        {
            _diagnostics = diagnostics;
            _symbolsManager = symbolsManager;
        }

        /// <summary>
        /// Loads and parses NPC data from a specified file.
        /// Each line of the file is expected to contain semicolon-separated NPC parameters:
        /// <br/> <c>[Name];[Question];[CorrectAnswer];[OptionB];[OptionC];[KeyFragments];[RewardPoints]</c>
        /// </summary>
        /// <param name="filePath">The full path to the NPC data file.</param>
        /// <returns>
        /// A list of <see cref="NpcInstance"/> objects representing all successfully loaded NPCs.
        /// Returns an empty list if an exception occurs during loading.
        /// </returns>
        public List<NpcInstance> LoadNpcDataFromFile(string filePath)
        {
            List<NpcInstance> tempNpcList = new List<NpcInstance>();
            try
            {
                foreach (string line in File.ReadAllLines(filePath))
                {
                    // Skip empty or whitespace-only lines.
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    // Split the line into parts separated by ';'.
                    // Removes all leading and trailing whitespace characters from each string part.
                    string[] parts = line.Split(';', StringSplitOptions.TrimEntries);

                    // Validate that the line contains all 7 required parts.
                    if (parts.Length < 7)
                    {
                        _diagnostics.AddWarning($"{nameof(NpcDataLoader)}: Incomplete line skipped: '{line}'");
                        continue;
                    }

                    // === Create NPC components ===
                    // The meta includes name, position, and automatically assigns the symbol from the SymbolsManager.
                    NpcMetaData npcMeta = new NpcMetaData(parts[0], (0, 0), _symbolsManager);
                    // The dialog includes a question, correct answer, and three possible answer options.
                    NpcDialogData npcDialog = new NpcDialogData(parts[1], parts[2], new List<(string A, string B, string C)> { (parts[2], parts[3], parts[4]) });
                    // Parse reward-related values.
                    int keyFragments, rewardPoints;
                    if (!int.TryParse(parts[5], out keyFragments) || !int.TryParse(parts[6], out rewardPoints))
                    {
                        _diagnostics.AddWarning($"{nameof(NpcDataLoader)}: Invalid reward values in line '{line}' – default values applied.");
                        keyFragments = 0;
                        rewardPoints = 0;
                    }

                    //The Reward includes key fragments and points.
                    NpcRewardData npcReward = new NpcRewardData(keyFragments, rewardPoints);
                    // Combine all NPC data into a single instance and add it to the list.
                    NpcInstance npcInstance = new NpcInstance(npcMeta, npcDialog, npcReward);
                    // Add the NPC object to the list.
                    tempNpcList.Add(npcInstance);
                }
                _diagnostics.AddCheck($"{nameof(NpcDataLoader)}: Successfully loaded {tempNpcList.Count} Npc records.");
                return tempNpcList;
            }

            // === Exception Handling ===
            catch (FileNotFoundException ex)
            {
                _diagnostics.AddException($"{nameof(NpcDataLoader)}: File not found ({ex.Message})");
                return new List<NpcInstance>();
            }
            catch (DirectoryNotFoundException ex)
            {
                _diagnostics.AddException($"{nameof(NpcDataLoader)}: Directory not found ({ex.Message})");
            }
            catch (PathTooLongException ex)
            {
                _diagnostics.AddException($"{nameof(NpcDataLoader)}: Path too long ({ex.Message})");
            }
            catch (UnauthorizedAccessException ex)
            {
                _diagnostics.AddException($"{nameof(NpcDataLoader)}: Access denied ({ex.Message})");
            }
            catch (SecurityException ex)
            {
                _diagnostics.AddException($"{nameof(NpcDataLoader)}: Security violation ({ex.Message})");
            }
            catch (IOException ex)
            {
                _diagnostics.AddException($"{nameof(NpcDataLoader)}: General I/O error (e.g., stream already open) ({ex.Message})");
            }
            catch (ArgumentNullException ex)
            {
                _diagnostics.AddException($"{nameof(NpcDataLoader)}: Null argument passed ({ex.Message})");
            }
            catch (ArgumentException ex)
            {
                _diagnostics.AddException($"{nameof(NpcDataLoader)}: Invalid argument format ({ex.Message})");
            }
            catch (FormatException ex)
            {
                _diagnostics.AddException($"{nameof(NpcDataLoader)}: Format error while parsing values ({ex.Message})");
            }
            catch (NotSupportedException ex)
            {
                _diagnostics.AddException($"{nameof(NpcDataLoader)}: Unsupported feature or file format ({ex.Message})");
            }
            catch (OverflowException ex)
            {
                _diagnostics.AddException($"{nameof(NpcDataLoader)}: Numeric overflow error ({ex.Message})");
            }
            catch (Exception ex)
            {
                _diagnostics.AddException($"{nameof(NpcDataLoader)}: Unknown exception occurred ({ex.Message})");
            }
            return new List<NpcInstance>();
        }
    }
}