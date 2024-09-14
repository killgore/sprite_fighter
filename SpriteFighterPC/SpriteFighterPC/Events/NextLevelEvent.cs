using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpriteFighter
{
    public class NextLevelEvent
    {
        public event EventHandler<EventArgs> NextLevelEventHandler;

        public void CreateNextLevelEvent()
        {
            // Copy to a temporary variable to be thread-safe.
            EventHandler<EventArgs> temp = NextLevelEventHandler;
            if (temp != null)
                temp(this, new EventArgs());
        }
    }
}
