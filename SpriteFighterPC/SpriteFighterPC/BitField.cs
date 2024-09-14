using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpriteFighter
{
    public class BitField
    {
        private uint m_bitField;

        public BitField() { m_bitField = 0; }

        public void SetField( uint mask )
        {
            m_bitField |= mask;
        }

        public bool isSet( uint mask )
        {
            if ( (m_bitField & mask) != 0 )
                return true;

            return false;
        }

        public void ClearField( uint mask )
        {
            m_bitField &= ~mask;
        }

        public void Clear()
        {
            m_bitField = 0;
        }
    }
}
