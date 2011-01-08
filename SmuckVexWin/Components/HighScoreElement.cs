using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace V2DRuntime.Components
{
    [XmlRootAttribute(ElementName = "Score", IsNullable = false)]
    public struct HighScoreElement : IComparable
    {
        public string name;
        public int score;
        public int level;
        public string data;
        
        public HighScoreElement(string name, int score) : this(name, score, 0, "")
        {
        }
        public HighScoreElement(string name, int score, int level) : this(name, score, level, "")
        {
        }
        public HighScoreElement(string name, int score, int level, string data)
        {
            this.name = name;
            this.score = score;
            this.level = level;
            this.data = data;
        }
        public int CompareTo(object o)
        {
            int result = -1;
            if (o is HighScoreElement)
            {
                HighScoreElement hs = ((HighScoreElement)o);
                result = (hs.score < this.score) ? -1 : (hs.score > this.score) ? 1 : 0;

                if (result == 0)
                {
                    result = (hs.level < this.level) ? -1 : (hs.level > this.level) ? 1 : 0;
                }
            }
            return result;
        }
    }
}
