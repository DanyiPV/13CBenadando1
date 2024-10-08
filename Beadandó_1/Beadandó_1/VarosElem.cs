using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Beadandó_1
{
    public abstract class VarosElem
    {
        public static List<VarosElem> varosElemek = new List<VarosElem>();
        public int PalyaMeret = 25;
        public int[,] szomszedok = new int[,] { { -1, -1 }, { -1, 0 }, { -1, 1 }, { 0, -1 }, { 0, 1 }, { 1, -1 }, { 1, 0 }, { 1, 1 } };
        public static List<(int,int)> FelfedettTeruletek = new List<(int,int)>();
        public static List<(int,int)> BarikadHelyek;
        public static bool jatekVeg = false;

        public void BanditakGen()
        {
            List<(int,int)> Banditak = varosElemek.Where(x => x.GetType() == typeof(Banditak)).Select(x => (Banditak)x).Select(c => c.Hely).ToList();
            while (Banditak.Count < 4)
            {
                Random r = new Random();
                (int, int) random = (r.Next(PalyaMeret), r.Next(PalyaMeret));
                if (!Banditak.Contains(random) && BanditakCheck(random, Banditak))
                    varosElemek.Add(new Banditak(random));
                Banditak = varosElemek.Where(x => x.GetType() == typeof(Banditak)).Select(x => (Banditak)x).Select(c => c.Hely).ToList();
            }
            BarikadokGen(Banditak);
        }
        private bool BanditakCheck((int, int) random, List<(int, int)> ListabaKeres)
        {
            bool igaze = true;
            int i = 0;
            while (igaze == true && i < 8)
            {
                if (ListabaKeres.Contains((random.Item1 + szomszedok[i, 0], random.Item2 + szomszedok[i, 1])))
                    igaze = false;
                i++;
            }
            return igaze;
        }
        public void BarikadokGen(List<(int, int)> Banditak)
        {
            List<(int, int)> Barikadok = varosElemek.Where(x => x.GetType() == typeof(Barikadok)).Select(x => (Barikadok)x).Select(c => c.Hely).ToList();
            while (Barikadok.Count < 120)
            {
                Random r = new Random();
                (int, int) random = (r.Next(PalyaMeret), r.Next(PalyaMeret));
                if (!Barikadok.Contains(random) && BarikadokCheck(random.Item1, random.Item2, Banditak))
                    varosElemek.Add(new Barikadok(random));
                Barikadok = varosElemek.Where(x => x.GetType() == typeof(Barikadok)).Select(x => (Barikadok)x).Select(c => c.Hely).ToList();
            }
            BarikadHelyek = Barikadok;
            WhiskyGen();
            AranyRogokGen(Banditak);
        }

        private bool BarikadokCheck(int s_index, int o_index, List<(int, int)> Barikadok)
        {
            int db = 0;
            for (int i = 0; i < 4; i++)
            {
                int s_szomszed = s_index + szomszedok[i, 0];
                int o_szomszed = o_index + szomszedok[i, 1];
                if (o_szomszed > -1 && s_szomszed > -1 && o_szomszed < PalyaMeret && s_szomszed < PalyaMeret)
                {
                    if (Barikadok.Contains((s_szomszed, o_szomszed)))
                        db++;
                }
            }
            return db < 4;
        }
        public void WhiskyGen()
        {
            List<(int, int)> Banditak = varosElemek.Where(x => x.GetType() == typeof(Banditak)).Select(x => (Banditak)x).Select(c => c.Hely).ToList();
            List<(int, int)> Whiskeyk = varosElemek.Where(x => x.GetType() == typeof(Whiskey)).Select(x => (Whiskey)x).Select(c => c.Hely).ToList();
            while (Whiskeyk.Count < 3)
            {
                Random r = new Random();
                (int, int) random = (r.Next(PalyaMeret), r.Next(PalyaMeret));
                if (!BarikadHelyek.Contains(random) && !Banditak.Contains(random) && !Whiskeyk.Contains(random))
                    varosElemek.Add(new Whiskey(random));
                Whiskeyk = varosElemek.Where(x => x.GetType() == typeof(Whiskey)).Select(x => (Whiskey)x).Select(c => c.Hely).ToList();
            }
        }

        public void AranyRogokGen(List<(int, int)> Banditak)
        {
            List<(int, int)> AranyRogok = varosElemek.Where(x => x.GetType() == typeof(Aranyrog)).Select(x => (Aranyrog)x).Select(c => c.Hely).ToList();
            List<(int, int)> Whiskeyk = varosElemek.Where(x => x.GetType() == typeof(Whiskey)).Select(x => (Whiskey)x).Select(c => c.Hely).ToList();
            while (AranyRogok.Count < 5)
            {
                Random r = new Random();
                (int, int) random = (r.Next(PalyaMeret), r.Next(PalyaMeret));
                if (!BarikadHelyek.Contains(random) && !Banditak.Contains(random) && !Whiskeyk.Contains(random) && !AranyRogok.Contains(random))
                    varosElemek.Add(new Aranyrog(random));
                AranyRogok = varosElemek.Where(x => x.GetType() == typeof(Aranyrog)).Select(x => (Aranyrog)x).Select(c => c.Hely).ToList();
            }
            VarosHazGen(Banditak, Whiskeyk, AranyRogok);
        }

        private void VarosHazGen(List<(int, int)> Banditak, List<(int, int)> Whiskeyk, List<(int, int)> AranyRogok)
        {
            (int, int) VarosHazHely = (-1, -1);
            while (VarosHazHely == (-1,-1))
            {
                Random r = new Random();
                (int, int) random = (r.Next(PalyaMeret), r.Next(PalyaMeret));
                if (!BarikadHelyek.Contains(random) && !Banditak.Contains(random) && !Whiskeyk.Contains(random) && !AranyRogok.Contains(random))
                {
                    varosElemek.Add(new VarosHaza(random));
                    VarosHazHely = random;
                }
            }
            SeriffHelyKivalasztas(Banditak, Whiskeyk, AranyRogok, VarosHazHely);
        }

        public void SeriffHelyKivalasztas(List<(int, int)> Banditak, List<(int, int)> Whiskeyk, List<(int, int)> AranyRogok, (int,int) VarosHazHely)
        {
            Random r = new Random();
            (int, int) SeriffHely = (-1, -1);
            while (SeriffHely == (-1, -1))
            {
                (int, int) random = (r.Next(PalyaMeret), r.Next(PalyaMeret));
                if (!Banditak.Contains(random) && !BarikadHelyek.Contains(random) && !Whiskeyk.Contains(random) && !AranyRogok.Contains(random) && BanditakCheck(random, Banditak) && random != VarosHazHely)
                {
                    varosElemek.Add(new Seriff(random, r.Next(20, 35)));
                    SeriffHely = random;
                }
            }
            FelfedettTeruletek.Add(SeriffHely);
            Seriff seriff = varosElemek.Where(x => x.GetType() == typeof(Seriff)).Select(x => (Seriff)x).ToList()[0];
            seriff.LatoMezoFelfed();
            Ground ground = new Ground();
            ground.PalyaGen();
            Thread.Sleep(800);
            LepesekInditas();
        }

        private void LepesekInditas()
        {
            Ground ground = new Ground();
            Seriff seriff = varosElemek.Where(x => x.GetType() == typeof(Seriff)).Select(x => (Seriff)x).ToList()[0];
            while (!jatekVeg && seriff.GetHP > 0)
            {
                seriff = varosElemek.Where(x => x.GetType() == typeof(Seriff)).Select(x => (Seriff)x).ToList()[0];
                seriff.Lepes();
                ground.PalyaGen();
                Thread.Sleep(100);
                List<Banditak> Banditak = varosElemek.Where(x => x.GetType() == typeof(Banditak)).Select(x => (Banditak)x).ToList();
                for (int i = 0; i < Banditak.Count; i++)
                {
                    Banditak[i].Lepes();
                }
                ground.PalyaGen();
                Thread.Sleep(100);
                seriff.Lepes();
                ground.PalyaGen();
                Thread.Sleep(100);
            }
            if (jatekVeg)
            {
                jatekVegKiirat(seriff);
            }
        }

        private void jatekVegKiirat(Seriff seriff)
        {
            List<Banditak> Banditak = varosElemek.Where(x => x.GetType() == typeof(Banditak)).Select(x => (Banditak)x).ToList();
            Console.Clear();
            if (seriff.GetHP > 0)
            {
                Console.WriteLine("       _      _                   \r\n      (_)    | |                  \r\n__   ___  ___| |_ ___  _ __ _   _ \r\n\\ \\ / / |/ __| __/ _ \\| '__| | | |\r\n \\ V /| | (__| || (_) | |  | |_| |\r\n  \\_/ |_|\\___|\\__\\___/|_|   \\__, |\r\n                             __/ |\r\n                            |___/ ");
                Console.WriteLine("٩(^ᗜ^ )و ´-  | A seriff összegyüjtötte az összes aranyrögöt és " + (Banditak.Count == 0 ? "megszabadította a várost a banditáktól!": $"megszabadította a várost {4-Banditak.Count} banditától!"));
            }
            else
            {
                Console.WriteLine("     _       __           _   \r\n    | |     / _|         | |  \r\n  __| | ___| |_ ___  __ _| |_ \r\n / _` |/ _ \\  _/ _ \\/ _` | __|\r\n| (_| |  __/ ||  __/ (_| | |_ \r\n \\__,_|\\___|_| \\___|\\__,_|\\__|\r\n                              \r\n                              ");
                Console.WriteLine($"૮(˶ㅠ︿ㅠ)ა  | A seriff meghalt!");
            }
        }

        public bool LepesCheck((int,int) indexek, List<(int, int)> Banditak, List<(int, int)> Whiskeyk, List<(int, int)> AranyRogok)
        {
            return indexek.Item1 > -1 && indexek.Item2 > -1 && indexek.Item1 < PalyaMeret && indexek.Item2 < PalyaMeret && !Banditak.Contains(indexek) && !BarikadHelyek.Contains(indexek) && !Whiskeyk.Contains(indexek) && !AranyRogok.Contains(indexek) && varosElemek.Where(x => x.GetType() == typeof(VarosHaza)).Select(x => (VarosHaza)x).ToList().First().Hely != indexek && varosElemek.Where(x => x.GetType() == typeof(Seriff)).Select(x => (Seriff)x).ToList().First().Hely != indexek;
        }
    }
}
