using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpriteFighter
{
    public class LevelCompleteEventArgs : EventArgs
    {
        private GameLevel gl;
        private long t;

        public LevelCompleteEventArgs(GameLevel finishedLevel, long time)
        {
            gl = finishedLevel;
            t = time;
        }
        public GameLevel CompletedLevel
        {
            get { return gl; }
            set { gl = value; }
        }

        public long CompletionTime
        {
            get { return t; }
            set { t = value; }
        }
    }

    public class LevelCompleteEvent
    {
        // Declare an event of delegate type EventHandler of 
        // LevelCompleteEventArgs.

        public event EventHandler<LevelCompleteEventArgs> LevelCompleteEventHandler;

        public void CreateLevelCompleteEvent(GameLevel val, long time)
        {
            // Copy to a temporary variable to be thread-safe.
            EventHandler<LevelCompleteEventArgs> temp = LevelCompleteEventHandler;
            if (temp != null)
                temp(this, new LevelCompleteEventArgs(val, time));
        }
    }
}
