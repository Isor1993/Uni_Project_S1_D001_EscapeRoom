/*****************************************************************************
* Project : Escape Room (K2, S2)
* File    : DiagnosticsManager.cs
* Date    : 09.11.2025
* Author  : Eric Rosenberg
*
* Description :
* Handles collection, organization, and output of diagnostic logs for the
* console-based Escape Room. Supports categorized messages (EXCEPTION, ERROR,
* WARNING, CHECK), counting utilities, ordered printing, and chronological
* recombination based on embedded timestamps.
*
* History :
* 09.11.2025 ER Created / Refactored for SAE Coding Convention compliance
******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;

namespace Semester1_D001_Escape_Room_Rosenberg
{
    /// <summary>
    /// Central diagnostics hub that captures and prints categorized log messages.
    /// <para>
    /// Categories:
    /// <list type="bullet">
    /// <item><description><c>EXCEPTION</c> – unrecoverable faults thrown or caught.</description></item>
    /// <item><description><c>ERROR</c> – critical states that prevent correct execution.</description></item>
    /// <item><description><c>WARNING</c> – non-critical issues that may degrade behavior.</description></item>
    /// <item><description><c>CHECK</c> – positive/neutral sanity checks for traceability.</description></item>
    /// </list>
    /// </para>
    /// Each log entry is timestamped in the format <c>[HH:mm:ss.fff]</c> to support
    /// chronological reconstruction of execution flow.
    /// </summary>
    internal class DiagnosticsManager
    {

        // === Fields ===

        /// <summary>
        /// Contains all logged critical error messages.
        /// These indicate severe issues that may prevent the program
        /// from continuing or functioning correctly.
        /// </summary>
        private readonly List<string> _errors = new();

        /// <summary>
        /// Contains all logged non-critical warnings.
        /// Warnings represent potential problems or recoverable situations
        /// that should be reviewed but do not interrupt execution.
        /// </summary>
        private readonly List<string> _warnings = new();

        /// <summary>
        /// Contains all logged system checks or informational entries.
        /// These are used to verify correct operation or record internal checkpoints.
        /// </summary>
        private readonly List<string> _checks = new();

        /// <summary>
        ///  Contains all logged exception messages.
        /// These capture thrown or caught exceptions with full timestamp context.
        /// </summary>
        private readonly List<string> _exceptions = new();

        /// <summary>
        /// Gets the total number of recorded exception messages
        /// currently stored in the diagnostics buffer.
        /// <para>
        /// Use this value to quickly assess how many runtime faults
        /// (thrown or caught) occurred during the last execution segment.
        /// </para>
        /// </summary>
        public int ExceptionCount => _exceptions.Count;

        /// <summary>
        /// Gets the total number of recorded error messages.
        /// <para>
        /// Errors indicate critical states that prevented correct execution
        /// but were not thrown as exceptions (e.g., invalid configuration).
        /// </para>
        /// </summary>
        public int ErrorCount => _errors.Count;

        /// <summary>
        /// Gets the total number of recorded warning messages.
        /// <para>
        /// Warnings represent recoverable or minor issues that may have
        /// degraded behavior without stopping the program.
        /// </para>
        /// </summary>
        public int WarningCount => _warnings.Count;

        /// <summary>
        /// Gets the total number of recorded check entries.
        /// <para>
        /// Checks are neutral/positive trace points used to verify control
        /// flow and successful milestones during execution.
        /// </para>
        /// </summary>
        public int CheckCount => _checks.Count;

        // === Message Recording ===

        /// <summary>
        /// Adds a new exception message to the exception log with a precise timestamp.
        /// <para>
        /// This should be used for unexpected runtime faults that were caught by the system.
        /// Each entry is formatted as:
        /// <code>
        /// [HH:mm:ss.fff] [EXCEPTION] &lt;message&gt;
        /// </code>
        /// </para>
        /// </summary>
        /// <param name="message">A short and clear description of the exception event.</param> 
        public void AddException(string message)
        {
            string timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
            _exceptions.Add($"[{timestamp}] [EXCEPTION] {message}");
        }

        /// <summary>
        /// Adds a new error message to the error log with a precise timestamp.
        /// <para>
        /// This is used for critical situations that are not thrown as exceptions
        /// but still prevent correct execution — for example, invalid data,
        /// missing dependencies, or broken configurations.
        /// </para>
        /// </summary>
        /// <param name="message">A short and clear description of the error condition.</param>
        public void AddError(string message)
        {
            string timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
            _errors.Add($"[{timestamp}] [ERROR] {message}");
        }

        /// <summary>
        /// Adds a new warning message to the warning log with a precise timestamp.
        /// <para>
        /// Use this for recoverable or minor issues that do not stop execution
        /// but may cause degraded behavior. Typical examples are missing optional
        /// resources or fallback logic triggers.
        /// </para>
        /// </summary>
        /// <param name="message">A short and clear description of the warning.</param>
        public void AddWarning(string message)
        {
            string timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
            _warnings.Add($"[{timestamp}] [WARNING] {message}");
        }

        /// <summary>
        /// Adds a new system check entry to the check log with a precise timestamp.
        /// <para>
        /// Use this method to confirm that a specific part of the code executed
        /// correctly. These entries are neutral or positive and useful for
        /// verifying control flow and program stability.
        /// </para>
        /// </summary>
        /// <param name="message">A short and clear description of the verification point.</param>
        public void AddCheck(string message)
        {
            string timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
            _checks.Add($"[{timestamp}] [CHECK] {message}");
        }
                
        // === Output and Reporting ===
     
        /// <summary>
        /// Prints all collected logs (exceptions, errors, warnings, and checks)
        /// to the console using the specified <see cref="PrintManager"/>.
        /// <para>
        /// The output order follows the severity hierarchy:
        /// <list type="number">
        /// <item><description>Exceptions</description></item>
        /// <item><description>Errors</description></item>
        /// <item><description>Warnings</description></item>
        /// <item><description>Checks</description></item>
        /// </list>
        /// Each entry preserves its original timestamp and prefix, allowing
        /// chronological tracing across categories.
        /// </para>
        /// </summary>
        /// <param name="printManager">
        /// Reference to the <see cref="PrintManager"/> responsible for writing
        /// formatted text output to the console.
        public void PrintAll(PrintManager printer)
        {
            printer.PrintLine("--- Diagnostics Report ---");


            foreach (string msg in _errors) printer.PrintLine(msg);

            foreach (string msg in _exceptions) printer.PrintLine(msg);

            foreach (string msg in _warnings) printer.PrintLine(msg);

            foreach (string msg in _checks) printer.PrintLine(msg);
            printer.PrintLine("--- END OF DIAGNOSTICS ---");
        }

        /// <summary>
        /// Prints a summary count of all diagnostic categories
        /// (exceptions, errors, warnings, and checks).
        /// <para>
        /// This method is typically called at the end of a game session or
        /// test run to display a compact overview of logged issues.
        /// Example output:
        /// <code>
        /// Exceptions: 2 | Errors: 1 | Warnings: 3 | Checks: 14
        /// </code>
        /// </para>
        /// </summary>
        /// <param name="printManager">
        /// Reference to the <see cref="PrintManager"/> responsible for writing
        /// formatted text output to the console.
        /// </param>
        public void PrintAllCount(PrintManager printer)
        {
            printer.PrintLine(
                $"Exceptions: {_exceptions.Count} | " +
                $"Errors: {_errors.Count} | " +
                $"Warnings: {_warnings.Count} | " +
                $"Checks: {_checks.Count}"
                );            
        }

        // === Chronological Output ===

        /// <summary>
        /// Prints all diagnostic messages from every category
        /// (exceptions, errors, warnings, and checks) in chronological order
        /// based on their timestamp prefixes.
        /// <para>
        /// Each message is expected to start with a timestamp in the format
        /// <c>[HH:mm:ss.fff]</c>. The method merges all categories into a single
        /// list, extracts the timestamps, and sorts all entries by actual time.
        /// </para>
        /// <para>
        /// If a message does not contain a valid timestamp, it is placed at
        /// the end of the sorted list to preserve stability of the output.
        /// </para>
        /// </summary>
        public void PrintChronologicalLogs()
        {
           
            List<string> combined = new ();
            combined.AddRange(_exceptions);
            combined.AddRange(_errors);
            combined.AddRange(_warnings);
            combined.AddRange(_checks);

            // Sort logs by their timestamp substring — assumes format "[HH:mm:ss.fff]"
            var sorted = combined
                .OrderBy(log =>
                {
                    try
                    {
                        string timePart = log.Substring(1, 12); // "HH:mm:ss.fff"
                        return DateTime.ParseExact(timePart, "HH:mm:ss.fff", null);
                    }
                    catch
                    {
                        // Fallback for malformed logs
                        return DateTime.MinValue;
                    }
                })
                .ToList();

            // Print the sorted result
            Console.WriteLine("=== Chronological Log Output ===");
            foreach (var log in sorted)
                Console.WriteLine(log);
            Console.WriteLine("=== End of Chronological Output ===");
        }

        /// <summary>
        /// Clears all stored diagnostic data across every category,
        /// including exceptions, errors, warnings, and checks.
        /// <para>
        /// Use this method to reset the diagnostics state between test runs
        /// or level reloads to ensure logs only reflect current execution data.
        /// </para>
        /// </summary>
        public void ClearAll()
        {
            _errors.Clear();
            _warnings.Clear();
            _checks.Clear();
            _exceptions.Clear();
        }
    }
}