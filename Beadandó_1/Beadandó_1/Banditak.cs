using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Beadandó_1
{
    internal class Banditak : VarosElem
    {
        (int, int) hely;
        public (int, int) Hely { get { return hely; } set { hely = value; } }
        public string Nev;
        private static readonly List<string> NevList = new List<string>() { "Fekete Farkas", "Vad Bill", "Sötét Árnyék", "Villámkezű Vince", "Pusztító Pedro", "Árnyéklovas", "Kígyószem Sam", "Vörös Skorpió", "Acél Mark", "Tűzszem" };
        public int Aranyrogok = 0;
        public int HP = 100;
        public bool Targetel = false;
        public bool Harcole = false;
        static List<(int,int)> elozoHelyek = new List<(int,int)>();
            
        public Banditak((int,int) hely)
        {
            this.hely = hely;
            Random r = new Random();
            int random = r.Next(NevList.Count);
            Nev = NevList[random];
            NevList.RemoveAt(random);
        }

        public void Lepes()
        {
            elozoHelyek.Add(hely);
            List<(int, int)> Banditak = varosElemek.Where(x => x.GetType() == typeof(Banditak)).Select(x => (Banditak)x).Select(c => c.Hely).ToList();
            List<(int, int)> Whiskeyk = varosElemek.Where(x => x.GetType() == typeof(Whiskey)).Select(x => (Whiskey)x).Select(c => c.Hely).ToList();
            List<(int, int)> AranyRogok = varosElemek.Where(x => x.GetType() == typeof(Aranyrog)).Select(x => (Aranyrog)x).Select(c => c.Hely).ToList();
            Seriff Seriff = varosElemek.Where(x => x.GetType() == typeof(Seriff)).Select(x => (Seriff)x).ToList().First();
            List<(int, int)> Lephet = new List<(int, int)>();
            for (int i = 0; i < 8; i++)
            {
                int o_index = hely.Item1 + szomszedok[i, 0];
                int s_index = hely.Item2 + szomszedok[i, 1];
                if (AranyRogok.Contains((o_index, s_index)))
                {
                    Aranyrog aranyrog = varosElemek.Where(x => x.GetType() == typeof(Aranyrog)).Select(x => (Aranyrog)x).Where(c => c.Hely == (o_index, s_index)).ToList().First();
                    varosElemek.Remove(aranyrog);
                    Aranyrogok++;
                }
                else if (LepesCheck((o_index,s_index), Whiskeyk, Banditak, AranyRogok) && !elozoHelyek.Contains((o_index, s_index)) && Harcole == false)
                    Lephet.Add((o_index,s_index));
            }
            Random r = new Random();
            if (!Harcole)
            {
                if (Lephet.Count == 0) { elozoHelyek = new List<(int, int)>(); Lepes(); }
                else hely = Lephet[r.Next(Lephet.Count)];
            }
            /*else if(Targetel && Harcole)
            {
                if (Lephet.Count == 0) { elozoHelyek = new List<(int, int)>(); Lepes(); }
                (int, int) index = Lephet[0];
                for (int i = 1; i < Lephet.Count; i++)
                {
                    if (Math.Abs(Seriff.Hely.Item1 - Lephet[i].Item1) < Math.Abs(Seriff.Hely.Item1 - index.Item1) ||
                        Math.Abs(Seriff.Hely.Item2 - Lephet[i].Item2) < Math.Abs(Seriff.Hely.Item2 - index.Item2))
                        index = Lephet[i];
                }
                hely = index;
            }*/
        }

        public override string ToString()
        {
            return $"Bandita | Név: {Nev} | Életerő: {HP} | Aranyrögök: {Aranyrogok}";
        }
    }
}
