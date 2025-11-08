/*****************************************************************************
* Project : Escape Room (K2, S2)
* File    : NpcDialogData.cs
* Date    : 09.11.2025
* Author  : Eric Rosenberg
*
* Description :
* Defines the dialogue configuration of a non-player character (NPC).
* Stores the question, the correct answer, and grouped answer options (A, B, C)
* used during player interactions or quiz-style encounters.
*
* Responsibilities:
* - Maintain all dialogue-related information of an NPC
* - Provide access to the question, correct answer, and multiple-choice options
* - Allow controlled modification of the correct answer when needed
*
* History :
* 09.11.2025 ER Created / Documentation fully updated
******************************************************************************/

using System.Collections.Generic;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Npc.NpcData
{
    /// <summary>
    /// Represents the dialogue configuration of a non-player character (NPC).
    /// </summary>
    /// <remarks>
    /// The <see cref="NpcDialogData"/> class encapsulates all data related to
    /// NPC dialogue interactions. It contains the question text, the correct answer,
    /// and a set of possible answer options grouped as A, B, and C.
    /// Each NPC can have one main question and multiple sets of potential answers
    /// for interactive quiz-based gameplay.
    /// </remarks>
    internal class NpcDialogData
    {
        // === Fields ===
        private readonly string _question;
        private string _correctAnswer;
        private readonly List<(string A, string B, string C)> _answerGroups = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="NpcDialogData"/> class.
        /// </summary>
        /// <param name="question">The question posed by the NPC.</param>
        /// <param name="correctAnswer">The correct answer associated with the question.</param>
        /// <param name="answerGroups">A list of grouped answer options (A, B, C).</param>
        /// <remarks>
        /// Each answer group typically contains three options from which
        /// the player must choose. This structure allows flexible extension
        /// for randomized or level-specific dialogue variations.
        /// </remarks>
        public NpcDialogData(string question, string correctAnswer, List<(string A, string B, string C)> answerGroups)
        {
            _question = question;
            _correctAnswer = correctAnswer;
            _answerGroups = answerGroups;
        }

        /// <summary>
        ///Gets the question text that the NPC presents to the player.
        /// </summary>
        public string Question => _question;

        /// <summary>
        /// Gets all available answer groups for the NPC’s question.
        /// </summary>
        /// <remarks>
        /// Each tuple represents one set of multiple-choice options (A, B, C).  
        /// These can be expanded for randomized quiz sequences or adaptive dialogue trees.
        /// </remarks>
        public List<(string A, string B, string C)> AnswerGroups => _answerGroups;

        /// <summary>
        /// Gets or sets the correct answer to the NPC’s question.
        /// </summary>
        /// <remarks>
        /// The setter allows runtime correction or randomization of answers
        /// for dynamic interaction behavior.
        /// </remarks>
        public string CorrectAnswer { get => _correctAnswer; set => _correctAnswer = value; }
    }
}