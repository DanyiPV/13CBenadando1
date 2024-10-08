using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Beadandó_1
{
    internal class Ground : VarosElem
    {
        public void PalyaGen()
        {
            Console.Clear();
            List<(int, int)> Banditak = varosElemek.Where(x => x.GetType() == typeof(Banditak)).Select(x => (Banditak)x).Select(c => c.Hely).ToList();
            List<(int, int)> Whiskeyk = varosElemek.Where(x => x.GetType() == typeof(Whiskey)).Select(x => (Whiskey)x).Select(c => c.Hely).ToList();
            List<(int, int)> AranyRogok = varosElemek.Where(x => x.GetType() == typeof(Aranyrog)).Select(x => (Aranyrog)x).Select(c => c.Hely).ToList();
            Seriff Seriff = varosElemek.Where(x => x.GetType() == typeof(Seriff)).Select(x => (Seriff)x).ToList().First();
            VarosHaza varosHaza = varosElemek.Where(x => x.GetType() == typeof(VarosHaza)).Select(x => (VarosHaza)x).ToList().First();
            for (int i = 0; i < PalyaMeret; i++)
            {
                for (int j = 0; j < PalyaMeret; j++)
                {
                    if (FelfedettTeruletek.Contains((i, j)))
                    {
                        if (Banditak.Contains((i, j))) { Console.BackgroundColor = ConsoleColor.DarkMagenta; Console.Write(" B "); }
                        else if (BarikadHelyek.Contains((i, j))) { Console.BackgroundColor = ConsoleColor.DarkGreen; Console.Write("   "); }
                        else if (Whiskeyk.Contains((i, j))) { Console.BackgroundColor = ConsoleColor.DarkRed; Console.Write(" W "); }
                        else if (AranyRogok.Contains((i, j))) { Console.BackgroundColor = ConsoleColor.Yellow; Console.Write(" A "); }
                        else if (Seriff.Hely == (i, j)) { Console.BackgroundColor = ConsoleColor.DarkYellow; Console.Write(" S "); }
                        else if (varosHaza.Hely == (i, j)) { Console.BackgroundColor = ConsoleColor.Blue; Console.Write(" V "); }
                        else { Console.BackgroundColor = ConsoleColor.White; Console.Write("   "); }
                    }
                    else { Console.BackgroundColor = ConsoleColor.Cyan; Console.Write("   "); }
                }
                Console.WriteLine();
            }
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine(Seriff.ToString());
            List<Banditak> BanditakNev = varosElemek.Where(x => x.GetType() == typeof(Banditak)).Select(x => (Banditak)x).ToList();
            for (int i = 0; i < BanditakNev.Count; i++)
                if(BanditakNev[i].Harcole)
                Console.WriteLine(BanditakNev[i].ToString());
        }
    }
}
