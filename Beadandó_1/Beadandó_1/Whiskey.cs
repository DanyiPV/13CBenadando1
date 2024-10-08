using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beadandó_1
{
    internal class Whiskey : VarosElem
    {
        (int, int) hely;
        public (int, int) Hely { get { return hely; } set { hely = value; } }

        public Whiskey((int, int) hely)
        {
            this.hely = hely;
        }
    }
}
