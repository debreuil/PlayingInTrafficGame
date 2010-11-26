using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDW.Display;
using Microsoft.Xna.Framework.Graphics;
using DDW.V2D;
using Smuck.Audio;

namespace Smuck.Components
{
    public class LaneCrossIndicator : Sprite
    {
        public List<Sprite> laneCrossIcon;

        public LaneCrossIndicator(Texture2D texture, V2DInstance inst) : base(texture, inst)
        {
        }
        private int count;
        public int Count
        {
            get { return count; }
            set
            {
                if (count != value)
                {
                    count = value;
                    if (count == targetCount)
                    {
                        stage.audio.PlaySound(Sfx.twoToots);
                    }
                    //else
                    if (count > 0)
                    {
                        stage.audio.PlaySound(Sfx.shortSlide);
                    }
                    SetView();
                }
            }
        }
        private int targetCount;
        public int TargetCount
        {
            get { return targetCount; }
            set
            {
                targetCount = Math.Min(laneCrossIcon.Count, value);
                for (int i = 0; i < laneCrossIcon.Count; i++)
                {
                    laneCrossIcon[i].Visible = i < value;
                }
                SetView();  
            }
        }

        private void SetView()
        {
            for (int i = 0; i < laneCrossIcon.Count; i++)
            {
                if (i < targetCount)
                {
                    if (count < targetCount)
                    {
                        laneCrossIcon[i].GotoAndStop((uint)(i < count ? 1 : 0));
                    }
                    else
                    {
                        laneCrossIcon[i].GotoAndStop(3);
                    }
                }
                else
                {
                    laneCrossIcon[i].Visible = false;
                }
            }
        }
    }
}
