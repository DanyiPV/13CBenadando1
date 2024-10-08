using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Beadandó_1
{
    internal class Seriff : VarosElem
    {
        (int, int) hely;
        public (int, int) Hely { get { return hely; } }
        int HP = 100;
        public int GetHP { get { return HP; } }
        int tamadasiEro;
        public int TamadasiEro { get { return tamadasiEro; } }
        int Aranyrogok = 0;
        bool Targetel = false;
        public bool Harcole = false;
        static HashSet<(int,int)> elozoHelyek = new HashSet<(int,int)>();
        static List<Whiskey> megtalaltWhiskeyk = new List<Whiskey>();
        bool WhiskeytKeresE = false;

        public Seriff((int, int) hely, int tamadisEro)
        {
            this.hely = hely;
            this.tamadasiEro = tamadisEro;
        }

        public void LatoMezoFelfed()
        {
            for (int i = 0; i < 8; i++)
            {
                int o_index = Hely.Item1 + szomszedok[i, 0];
                int s_index = Hely.Item2 + szomszedok[i, 1];
                if (o_index > -1 && s_index > -1 && o_index < PalyaMeret && s_index < PalyaMeret && !FelfedettTeruletek.Contains((o_index, s_index)))
                    FelfedettTeruletek.Add((o_index, s_index));
            }
        }

        public void Lepes()
        {
            WhiskeytKeresE = HP <= 49;
            LatoMezoFelfed();
            elozoHelyek.Add(hely);
            List<(int, int)> Whiskeyk = varosElemek.Where(x => x.GetType() == typeof(Whiskey)).Select(x => (Whiskey)x).Select(c => c.Hely).ToList();
            List<(int, int)> AranyRogok = varosElemek.Where(x => x.GetType() == typeof(Aranyrog)).Select(x => (Aranyrog)x).Select(c => c.Hely).ToList();
            List<(int, int)> Banditak = varosElemek.Where(x => x.GetType() == typeof(Banditak)).Select(x => (Banditak)x).Select(c => c.Hely).ToList();
            List<(int, int)> Lephet = new List<(int, int)>();
            List<Banditak> KozeliBanditak = new List<Banditak>();
            (int, int) varosHaza = varosElemek.Where(x => x.GetType() == typeof(VarosHaza)).Select(x => (VarosHaza)x).ToList().First().Hely;
            Random r = new Random();
            (int, int) LegkozelebbiFelfedetlen = LegkozelebbiFelfedetlenKeres(Whiskeyk, Banditak, AranyRogok);
            string MindenTeruletFelfedve = MindenTeruletFelfedveCheck(Banditak);
            for (int i = 0; i < 8; i++)
            {
                int s_index = hely.Item1 + szomszedok[i, 0];
                int o_index = hely.Item2 + szomszedok[i, 1];
                if(MindenTeruletFelfedve == "VaroshazaTarget" && varosHaza == (s_index,o_index))
                {
                    jatekVeg = true;
                }
                if (AranyRogok.Contains((s_index, o_index)) && !jatekVeg)
                {
                    Aranyrog aranyrog = varosElemek.Where(x => x.GetType() == typeof(Aranyrog)).Select(x => (Aranyrog)x).Where(c => c.Hely == (s_index, o_index)).ToList().First();
                    varosElemek.Remove(aranyrog);
                    Aranyrogok++;
                }
                else if (Whiskeyk.Contains((s_index, o_index)) && !jatekVeg)
                {
                    Whiskey whiskey = varosElemek.Where(x => x.GetType() == typeof(Whiskey)).Select(x => (Whiskey)x).Where(c => c.Hely == (s_index, o_index)).ToList().First();
                    if(HP < 50)
                    {
                        HP += 50;
                        varosElemek.Remove(whiskey);
                        if (megtalaltWhiskeyk.Contains(whiskey) && !varosElemek.Contains(whiskey))
                        {
                            megtalaltWhiskeyk.Remove(whiskey);
                        }
                        WhiskyGen();
                    }
                    if (!megtalaltWhiskeyk.Contains(whiskey) && varosElemek.Contains(whiskey)) {
                        megtalaltWhiskeyk.Add(whiskey);
                    }
                }
                else if (Banditak.Contains((s_index,o_index)) && !jatekVeg)
                {
                    Banditak Bandita = varosElemek.Where(x => x.GetType() == typeof(Banditak)).Select(x => (Banditak)x).Where(c => c.Hely == (s_index, o_index)).ToList().First();
                    KozeliBanditak.Add(Bandita);
                }
                if (LepesCheck((s_index, o_index), Whiskeyk, Banditak, AranyRogok) && !elozoHelyek.Contains((s_index, o_index)))
                    Lephet.Add((s_index, o_index));
            }
            if(KozeliBanditak.Count == 0 && !WhiskeytKeresE && MindenTeruletFelfedve == "NincsTarget" && !jatekVeg)
            {
                if (Lephet.Count == 0) { elozoHelyek = new HashSet<(int, int)>(); Lepes(); }
                else 
                {
                    hely = OptimalisLepes(Lephet, LegkozelebbiFelfedetlen);
                    LatoMezoFelfed(); 
                }
            }
            else if (MindenTeruletFelfedve == "NincsTarget" && !jatekVeg && WhiskeytKeresE)
            {
                if (Lephet.Count == 0) { elozoHelyek = new HashSet<(int, int)>(); Lepes(); }
                Harcole = false;
                if(KozeliBanditak.Count > 0)
                {
                    for (int i = 0; i < KozeliBanditak.Count; i++)
                    {
                        if(KozeliBanditak[i].Harcole == true)
                            KozeliBanditak[i].Harcole = false;
                    }
                }
                if (megtalaltWhiskeyk.Count > 0)
                    WhiskeytKeres(Lephet);
                else { if (Lephet.Count > 0) { hely = Lephet[r.Next(Lephet.Count)]; LatoMezoFelfed(); } }
            }
            else if(!jatekVeg)
            {
                if (MindenTeruletFelfedve == "VaroshazaTarget")
                {
                    if (Lephet.Count == 0) { elozoHelyek = new HashSet<(int, int)>(); Lepes(); }
                    else
                    {
                        (int, int) index = Lephet[0];
                        for (int i = 1; i < Lephet.Count - 1; i++)
                        {
                            if (Math.Abs(varosHaza.Item1 - Lephet[i].Item1) < Math.Abs(varosHaza.Item1 - index.Item1) &&
                                Math.Abs(varosHaza.Item2 - Lephet[i].Item2) < Math.Abs(varosHaza.Item2 - index.Item2))
                                index = Lephet[i];
                        }
                        hely = index;
                    }
                }else if(MindenTeruletFelfedve == "BanditaTarget")
                {
                    if (Lephet.Count == 0) { elozoHelyek = new HashSet<(int, int)>(); Lepes(); }
                    {
                        (int, int) index = Lephet[0];
                        (int, int) TargetBandita = Banditak[0];
                        for (int i = 1; i < Lephet.Count - 1; i++)
                        {
                            if (Math.Abs(TargetBandita.Item1 - Lephet[i].Item1) < Math.Abs(TargetBandita.Item1 - index.Item1) &&
                                Math.Abs(TargetBandita.Item2 - Lephet[i].Item2) < Math.Abs(TargetBandita.Item2 - index.Item2))
                                index = Lephet[i];
                        }
                        hely = index;
                    }
                }
            }
            if (KozeliBanditak.Count > 0 && !jatekVeg)
            {
                if (Lephet.Count == 0) { elozoHelyek = new HashSet<(int, int)>(); Lepes(); }
                if (KozeliBanditak.Count == 1 && HP > 35 || KozeliBanditak.Count == 2 && HP > 55)
                {
                    Harcole = true;
                    for (int i = 0; i < KozeliBanditak.Count; i++)
                    {
                        KozeliBanditak[i].Harcole = true;
                    }
                }
                Harcol(KozeliBanditak);
            }
        }

        private string MindenTeruletFelfedveCheck(List<(int, int)> Banditak)
        {
            string ACel = "";
            if (FelfedettTeruletek.Count == 625 && Banditak.Count != 0 && Aranyrogok < 5)
                ACel = "BanditaTarget";
            else if ((FelfedettTeruletek.Count == 625 && Banditak.Count == 0) || Aranyrogok == 5)
                ACel = "VaroshazaTarget";
            else
                ACel = "NincsTarget";
            return ACel;
        }

        private (int, int) OptimalisLepes(List<(int, int)> lephet, (int,int) LegkozelebbiFelfedetlen)
        {
            (int, int) index = lephet[0];
            (int, int) index2 = lephet[0];
            (int, int) ReturnIndex = (0, 0);
            Random r = new Random();
            for (int i = 1; i < lephet.Count; i++)
            {
                if (Math.Abs(LegkozelebbiFelfedetlen.Item1 - lephet[i].Item1) < Math.Abs(LegkozelebbiFelfedetlen.Item1 - index.Item1) &&
                    Math.Abs(LegkozelebbiFelfedetlen.Item2 - lephet[i].Item2) < Math.Abs(LegkozelebbiFelfedetlen.Item2 - index.Item2))
                    index = lephet[i];
                if (LegkozelebbiFelfedetlen.Item1 - lephet[i].Item1 < LegkozelebbiFelfedetlen.Item1 - index2.Item1 &&
                   LegkozelebbiFelfedetlen.Item2 - lephet[i].Item2 <LegkozelebbiFelfedetlen.Item2 - index2.Item2)
                    index2 = lephet[i];
            }
            if(Math.Abs(LegkozelebbiFelfedetlen.Item1 - index.Item1) < Math.Abs(LegkozelebbiFelfedetlen.Item1 - index2.Item1) &&
               Math.Abs(LegkozelebbiFelfedetlen.Item2 - index.Item2) < Math.Abs(LegkozelebbiFelfedetlen.Item2 - index2.Item2))
            {
                ReturnIndex = index;
            }
            else if(Math.Abs(LegkozelebbiFelfedetlen.Item1 - index.Item1) > Math.Abs(LegkozelebbiFelfedetlen.Item1 - index2.Item1) &&
               Math.Abs(LegkozelebbiFelfedetlen.Item2 - index.Item2) > Math.Abs(LegkozelebbiFelfedetlen.Item2 - index2.Item2))
            {
                ReturnIndex = index2;
            }
            else
            {
                ReturnIndex = r.Next(100) < 51 ? index : index2;
            }
            return ReturnIndex;
        }

        private (int, int) LegkozelebbiFelfedetlenKeres(List<(int, int)> Whiskeyk, List<(int, int)> Banditak, List<(int, int)> AranyRogok)
        {
            (int, int) index = ElsoFelfedetlenMegkap(Whiskeyk, Banditak, AranyRogok);
            for (int i = 0; i < PalyaMeret; i++)
            {
                for (int j = 0; j < PalyaMeret; j++)
                {
                    if (LepesCheck((i,j), Whiskeyk, Banditak, AranyRogok) && (!FelfedettTeruletek.Contains((i,j)) && Math.Abs(hely.Item1 - i) < Math.Abs(hely.Item1 - index.Item1) &&
                        !FelfedettTeruletek.Contains((i, j)) && Math.Abs(hely.Item2 - j) < Math.Abs(hely.Item2 - index.Item2)))
                        index = (i,j);
                }
            }
             return index;
        }

        private (int, int) ElsoFelfedetlenMegkap(List<(int, int)> Whiskeyk, List<(int, int)> Banditak, List<(int, int)> AranyRogok)
        {
            (int, int) index = (-1,-1);
            for (int i = 0; i < PalyaMeret; i++)
            {
                for (int j = 0; j < PalyaMeret; j++)
                {
                    if (!FelfedettTeruletek.Contains((i, j)) && LepesCheck((i, j), Whiskeyk, Banditak, AranyRogok) && index == (-1,-1))
                        index = (i, j);
                }
            }
            return index;
        }

        private void WhiskeytKeres(List<(int, int)> lephet)
        {
            (int, int) legkozelebbiWhiskey = megtalaltWhiskeyk.Count == 1? megtalaltWhiskeyk[0].Hely : LegkozelebbiWhiskeyKeres();
            if(lephet.Count != 0)
            {
                (int, int) index = lephet[0];
                for (int i = 1; i < lephet.Count; i++)
                {
                    if (Math.Abs(legkozelebbiWhiskey.Item1 - lephet[i].Item1) < Math.Abs(legkozelebbiWhiskey.Item1 - index.Item1) ||
                       Math.Abs(legkozelebbiWhiskey.Item2 - lephet[i].Item2) < Math.Abs(legkozelebbiWhiskey.Item2 - index.Item2))
                        index = lephet[i];
                }
                hely = index;
                LatoMezoFelfed();
            }
            else
            {
                elozoHelyek = new HashSet<(int, int)>();
                Lepes();
            }
        }

        private (int, int) LegkozelebbiWhiskeyKeres()
        {
            (int, int) index = megtalaltWhiskeyk[0].Hely;
            for (int i = 1; i < megtalaltWhiskeyk.Count; i++)
            {
                if (Math.Abs(hely.Item1 - megtalaltWhiskeyk[i].Hely.Item1) < Math.Abs(hely.Item1 - index.Item1) &&
                    Math.Abs(hely.Item2 - megtalaltWhiskeyk[i].Hely.Item2) < Math.Abs(hely.Item2 - index.Item2))
                    index = megtalaltWhiskeyk[i].Hely;
            }
            return index;
        }

        private void Harcol(List<Banditak> kozeliBanditak)
        {
            Banditak VeleHarcol = kozeliBanditak[0];
            VeleHarcol.HP -= tamadasiEro;
            Random r = new Random();
            for (int i = 0; i < kozeliBanditak.Count; i++)
            {
                if (kozeliBanditak[i].HP > 0)
                    HP -= r.Next(4, 15);
                else
                {
                    Aranyrogok += kozeliBanditak[i].Aranyrogok;
                    varosElemek.Remove(kozeliBanditak[i]);
                }
            }
            if (kozeliBanditak.Count == 0) Harcole = false;
            if(HP <= 0)
            {
                jatekVeg = true;
            }
        }

        public override string ToString()
        {
            return $"Seriff | Életerő: {HP} | Ütési erő: {tamadasiEro} | Aranyrögök: {Aranyrogok}";
        }
    }
}
