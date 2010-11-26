using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smuck.Components
{
    public class HighScoreElement : IComparable
    {
        public string name;
        public int score;
        public bool isLocal;
        // date

        public HighScoreElement(string name, int score, bool isLocal)
        {
            this.name = name;
            this.score = score;
            this.isLocal = isLocal;
        }
        public int CompareTo(object o)
        {
            int result = -1;
            if (o is HighScoreElement)
            {
                if(((HighScoreElement)o).score < this.score)
                {
                    result = -1;
                }
                else if(((HighScoreElement)o).score > this.score)
                {
                    result = 1;
                }
                else//if(((HighScoreElement)o).score == this.score)
                {
                    result = 0;// ((HighScoreElement)o).name.c;
                }
            }
            return result;
        }
    }
}
