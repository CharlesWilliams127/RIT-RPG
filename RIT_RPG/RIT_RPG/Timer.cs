using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RIT_RPG
{
    class Timer
    {
        private int delay;
        private int frame;

        public Timer()
        {
            frame = 0; //give the frame a value
        }

        public int Delay
        {
            get { return delay; }
        }

        public int Frame
        {
            get { return frame; }
        }

        public bool Freeze(int maxTime)
        {
            delay = maxTime;
            if (frame != delay)
            {
                frame++;
            }
            else if(frame == delay)
            {
                frame = 0;
                return true;
            }

            return false;
        }
    }
}
