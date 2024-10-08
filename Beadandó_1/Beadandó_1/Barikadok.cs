using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beadandó_1
{
    internal class Barikadok : VarosElem
    {
        (int, int) hely;
        public (int, int) Hely { get { return hely; } }
        int HP = 100;

        public Barikadok((int, int) hely)
        {
            this.hely = hely;
        }

        public override string ToString()
        {
            return $"Barikád helyzet: {hely}";
        }
    }
}
