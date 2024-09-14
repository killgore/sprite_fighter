using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpriteFighter
{
    public class OpenStoreEvent
    {
        public event EventHandler<EventArgs> OpenStoreEventHandler;

        public void CreateOpenStoreEvent()
        {
            // Copy to a temporary variable to be thread-safe.
            EventHandler<EventArgs> temp = OpenStoreEventHandler;
            if (temp != null)
                temp(this, new EventArgs());
        }
    }
}
