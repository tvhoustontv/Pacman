using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacman.GameStates.World
{
    class Score
    {
        private int score = 0;

        public int SCORE
        {
            get
            {
                return score;
            }
            set
            {
                score = value;
            }
        }     
    }
}
