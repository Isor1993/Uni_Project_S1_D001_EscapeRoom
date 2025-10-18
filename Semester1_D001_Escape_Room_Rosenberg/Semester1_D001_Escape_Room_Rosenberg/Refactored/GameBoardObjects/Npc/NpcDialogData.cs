using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semester1_D001_Escape_Room_Rosenberg.Refactored.GameBoardObjects.Npc
{
    internal class NpcDialogData
    {
        public NpcDialogData(string question, string correctAnswer, List<(string A, string B, string C)> answerGroups)
        {
            _question = question;
            _correctAnswer = correctAnswer;
            _answerGroups = answerGroups;
        }


        private string _question;
        private List<(string A, string B, string C)> _answerGroups = new();
       
        private string _correctAnswer;

        public string Question => _question;
        public List<(string A, string B, string C)> AnswerGroups => _answerGroups;
    
        public string CorrectAnswer=>_correctAnswer;




    }
}
