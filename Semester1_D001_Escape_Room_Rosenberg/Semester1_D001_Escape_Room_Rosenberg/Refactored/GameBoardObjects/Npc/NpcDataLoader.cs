using Semester1_D001_Escape_Room_Rosenberg.Refactored.Dependencies;
using Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Npc.NpcData;
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
        private readonly NpcDataLoaderDependencies _deps;


        /// <summary>
        /// Initializes a new instance of the <see cref="NpcDataLoader"/> class.
        /// Sets up all required dependencies for NPC data loading and registers
        /// the loader creation event in the diagnostics log.
        /// </summary>
        /// <param name="npcDataLoaderDependencies">
        /// Reference to the <see cref="NpcDataLoaderDependencies"/> object that provides
        /// the required managers and configuration data for loading NPC information.
        /// </param>
        public NpcDataLoader(NpcDataLoaderDependencies npcDataLoaderDependencies)
        {
            _deps = npcDataLoaderDependencies;
            _deps.Diagnostic.AddCheck($"{nameof(NpcDataLoader)}: Loader instance successfully created.");
        }

        /// <summary>
        /// Loads and parses NPC data from a specified file.
        /// Each line of the file is expected to contain semicolon-separated NPC parameters:
        /// <br/> <c>[Name];[Question];[CorrectAnswer];[OptionB];[OptionC];[KeyFragments];[RewardPoints]</c>
        /// </summary>
        /// <param name="filePath">The full path to the NPC data file.</param>
        /// <returns>
        /// A list of <see cref="NpcRawData"/> objects representing all successfully loaded NPCs.
        /// Returns an empty list if an exception occurs during loading.
        /// </returns>
        public List<NpcRawData> LoadNpcDataFromFile()
        {
            List<NpcRawData> npcRawList = new List<NpcRawData>();
            try
            {
                // Path for txt file in project
                string filePath = "npc_questions.txt";

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
                        _deps.Diagnostic.AddWarning($"{nameof(NpcDataLoader)}.{nameof(LoadNpcDataFromFile)}: Incomplete line {line} skipped ");
                        continue;
                    }

                    // === Create raw NPC data objects ===
                    // The meta includes name, position, and automatically assigns the symbol from the SymbolsManager.
                    NpcMetaData npcMeta = new NpcMetaData(_deps.Symbol, parts[0], (0, 0));
                    // The dialog includes a question, correct answer, and three possible answer options.
                    NpcDialogData npcDialog = new NpcDialogData(parts[1], parts[2], new List<(string A, string B, string C)> { (parts[2], parts[3], parts[4]) });
                    // Parse reward-related values.
                    int keyFragments, rewardPoints;
                    if (!int.TryParse(parts[5], out keyFragments) || !int.TryParse(parts[6], out rewardPoints))
                    {
                        _deps.Diagnostic.AddWarning($"{nameof(NpcDataLoader)}.{nameof(LoadNpcDataFromFile)}: Invalid reward values in line {line} – default values applied.");
                        keyFragments = 0;
                        rewardPoints = 0;
                    }

                    // The Reward includes key fragments and points.
                    NpcRewardData npcReward = new NpcRewardData(keyFragments, rewardPoints);
                    // === Combine into a single NpcRawData record ===
                    NpcRawData npcRaw = new NpcRawData(npcMeta, npcDialog, npcReward);
                    // Add the NPC object to the list.
                    npcRawList.Add(npcRaw);
                }
                _deps.Diagnostic.AddCheck($"{nameof(NpcDataLoader)}.{nameof(LoadNpcDataFromFile)}: Successfully loaded {npcRawList.Count} Npc records.");
                return npcRawList;
            }

            // === Exception Handling ===
            catch (FileNotFoundException ex)
            {
                _deps.Diagnostic.AddException($"{nameof(NpcDataLoader)}.{nameof(LoadNpcDataFromFile)}: File not found ({ex.Message})");
                return new List<NpcRawData>();
            }
            catch (DirectoryNotFoundException ex)
            {
                _deps.Diagnostic.AddException($"{nameof(NpcDataLoader)}.{nameof(LoadNpcDataFromFile)}: Directory not found ({ex.Message})");
            }
            catch (PathTooLongException ex)
            {
                _deps.Diagnostic.AddException($"{nameof(NpcDataLoader)}.{nameof(LoadNpcDataFromFile)}: Path too long ({ex.Message})");
            }
            catch (UnauthorizedAccessException ex)
            {
                _deps.Diagnostic.AddException($"{nameof(NpcDataLoader)}.{nameof(LoadNpcDataFromFile)}: Access denied ({ex.Message})");
            }
            catch (SecurityException ex)
            {
                _deps.Diagnostic.AddException($"{nameof(NpcDataLoader)}.{nameof(LoadNpcDataFromFile)}: Security violation ({ex.Message})");
            }
            catch (IOException ex)
            {
                _deps.Diagnostic.AddException($"{nameof(NpcDataLoader)}.{nameof(LoadNpcDataFromFile)}: General I/O error (e.g., stream already open) ({ex.Message})");
            }
            catch (ArgumentNullException ex)
            {
                _deps.Diagnostic.AddException($"{nameof(NpcDataLoader)}.{nameof(LoadNpcDataFromFile)}: Null argument passed ({ex.Message})");
            }
            catch (ArgumentException ex)
            {
                _deps.Diagnostic.AddException($"{nameof(NpcDataLoader)}.{nameof(LoadNpcDataFromFile)}: Invalid argument format ({ex.Message})");
            }
            catch (FormatException ex)
            {
                _deps.Diagnostic.AddException($"{nameof(NpcDataLoader)}.{nameof(LoadNpcDataFromFile)}: Format error while parsing values ({ex.Message})");
            }
            catch (NotSupportedException ex)
            {
                _deps.Diagnostic.AddException($"{nameof(NpcDataLoader)}.{nameof(LoadNpcDataFromFile)}: Unsupported feature or file format ({ex.Message})");
            }
            catch (OverflowException ex)
            {
                _deps.Diagnostic.AddException($"{nameof(NpcDataLoader)}.{nameof(LoadNpcDataFromFile)}: Numeric overflow error ({ex.Message})");
            }
            catch (Exception ex)
            {
                _deps.Diagnostic.AddException($"{nameof(NpcDataLoader)}.{nameof(LoadNpcDataFromFile)}: Unknown exception occurred ({ex.Message})");
            }
            return new List<NpcRawData>();
        }
    }
}