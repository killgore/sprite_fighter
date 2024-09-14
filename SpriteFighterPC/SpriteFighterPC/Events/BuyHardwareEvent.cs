using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpriteFighter
{
    public class BuyHardwareEventArgs : EventArgs
    {
        private uint type;
        private int cost;

        public BuyHardwareEventArgs(uint t, int c)
        {
            type = t;
            cost = c;
        }

        public uint _type
        {
            get 
            { 
                return type; 
            }

            set 
            { 
                type = value; 
            }
        }

        public int _cost
        {
            get 
            { 
                return cost; 
            }

            set 
            { 
                cost = value; 
            }
        }
    }

    class BuyHardwareEvent
    {
        public event EventHandler<BuyHardwareEventArgs> BuyHardwareEventHandler;

        public void CreateBuyHardwareEvent(uint type, int cost)
        {
            // Copy to a temporary variable to be thread-safe.
            EventHandler<BuyHardwareEventArgs> temp = BuyHardwareEventHandler;
            if (temp != null)
                temp(this, new BuyHardwareEventArgs(type, cost));
        }
    }
}
