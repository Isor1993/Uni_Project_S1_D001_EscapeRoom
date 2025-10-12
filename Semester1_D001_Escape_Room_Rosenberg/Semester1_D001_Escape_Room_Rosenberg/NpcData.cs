using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semester1_D001_Escape_Room_Rosenberg
{
    /// <summary>
    /// DTO for NpcData
    /// </summary>
    internal class NpcData
    {
        // Properties for Npc
        public string Name { get; }
        public string Question { get; }
        public string CorrectAnswer { get; }
        public int RewardKeyFragment { get; }

        public int RewardPoints { get; }

        // Konstruktor for the Properties
        public NpcData(string name, string question, string correctAnswer, int rewardKeyFragment,int rewardPoints)
        {
            Name = name;
            Question = question;
            CorrectAnswer = correctAnswer;
            RewardKeyFragment = rewardKeyFragment;
            RewardPoints = rewardPoints;
        }
    }
}

