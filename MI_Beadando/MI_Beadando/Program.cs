using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MI_Beadando
{
    #region abstract osztály
    abstract class AbsztraktAllapot : ICloneable
    {
        public abstract int getNumberOfOps();
        // Megvizsgálja, hogy a belsõ állapot állapot-e.
        // Ha igen, akkor igazat ad vissza, egyébként hamisat.
        public abstract bool allapotE();
        // Megvizsgálja, hogy a belsõ állapot célállapot-e.
        // Ha igen, akkor igazat ad vissza, egyébként hamisat.
        public abstract bool vegAllapotE();
        public abstract bool szuperOp(int i);
        public abstract AbsztraktAllapot clone();
        public abstract bool equals(AbsztraktAllapot a);
    }
    #endregion
    abstract class VakÁllapot : AbsztraktAllapot
    {
        private bool op1()
        {
            if (!preOp1()) return false;
            if (vegAllapotE()) return true;
            return false;
        }
        private bool preOp1()
        {
            return true;
        }
        private bool op2()
        {
            if (!preOp2()) return false;
            if (vegAllapotE()) return true;
            return false;
        }
        private bool preOp2()
        {
            return true;
        }
        private bool op3(int i)
        {
            if (!preOp3(i)) return false;
            if (vegAllapotE()) return true;
            return false;
        }
        private bool preOp3(int i)
        {
            return true;
        }
        public override bool szuperOp(int i)
        {
            switch (i)
            {
                case 0: return op1();
                case 1: return op2();
                case 3: return op3(0);
                case 4: return op3(1);
                case 5: return op3(2);
                default: return false;
            }
        }
        public override int getNumberOfOps()
        {
            return 5;
        }
    }

abstract class FerjekEsFelesegekAllapot: AbsztraktAllapot
    {
        int ferj; //férjek összesen
        int feleseg; //feleség összesen
        int ferjegyikpart;
        int felesegegyikpart;
        char csonak; //hol a csónak egyik vagy másik oldalon
        int ferjmasikpart;
        int felesegmasikpart;

        public FerjekEsFelesegekAllapot(int ferj, int feleseg) //beállítja a kezdő állapotot
        {
            this.ferj = ferj;
            this.feleseg = feleseg;
            ferjegyikpart = ferj;
            felesegegyikpart = feleseg;
            csonak = 'B';
            ferjmasikpart = ferj;
            felesegmasikpart = feleseg;
        }
        public override bool allapotE()
        {
            return ((felesegegyikpart >= ferjegyikpart) || (felesegegyikpart == 0)) &&
                ((felesegegyikpart >= ferjmasikpart) || (felesegmasikpart == 0));
        }
        public override bool vegAllapotE()
        {
            return ferjegyikpart == ferj && felesegegyikpart == feleseg;
        }
        private bool preOp(int feleseg, int ferj)
        {
            if (ferj + feleseg > 2 || ferj + feleseg < 0 || ferj < 0 || feleseg < 0) return false;
            if (ferj == 'B')
                return ferjegyikpart >= ferj && felesegegyikpart >= feleseg;
            else
                return ferjmasikpart >= ferj && felesegmasikpart >= feleseg;
        }
        private bool Atvisz(int ferj, int feleseg)
        {
            if (!preOp(feleseg, ferj)) return false;
            if (csonak == 'J')
            {
                ferjegyikpart += ferj;
                felesegegyikpart += feleseg;
                csonak = 'B';
                ferjmasikpart -= ferj;
                felesegmasikpart -= feleseg;

            }
            else
            {
                ferjegyikpart -= ferj;
                felesegegyikpart -= feleseg;
                csonak = 'J';
                ferjmasikpart += ferj;
                felesegmasikpart += feleseg;
            }
            if (allapotE()) return true;
            return false;
        }
        public override int getNumberOfOps() { return 5; }
        public override bool szuperOp(int i)
        {
           switch (i)
            {
                case 0: return Atvisz(1, 0);
                case 1: return Atvisz(0, 1);
                case 2: return Atvisz(1, 1);
                case 3: return Atvisz(0, 2);
                case 4: return Atvisz(2, 0);
                default: return false;
            }
        }
        public override string ToString()
        {
            return ferjegyikpart + "," + felesegegyikpart + "," + csonak + "," + ferjmasikpart + "," + felesegmasikpart;
        }
        public override bool Equals(Object a)
        {
            FerjekEsFelesegekAllapot aa = (FerjekEsFelesegekAllapot)a;
            return aa.ferjegyikpart == ferjegyikpart && aa.felesegegyikpart == felesegegyikpart && aa.csonak == csonak;
        }
        public override int GetHashCode()
        {
            return ferjegyikpart.GetHashCode() + felesegegyikpart.GetHashCode() + csonak.GetHashCode();
        }
    }
    class Csucs
    {
        AbsztraktAllapot allapot;
        int melyseg;
        Csucs szulo;

        public Csucs(AbsztraktAllapot kezdoAllapot)
        {
            allapot = kezdoAllapot;
            melyseg = 0;
            szulo = null;
        }
        public Csucs(Csucs szulo)
        {
            allapot = (AbsztraktAllapot)szulo.allapot.Clone();
            melyseg = szulo.melyseg + 1;
            this.szulo = szulo;
        }
        public Csucs GetSzulo() { return szulo; }
        public int GetMelyseg() { return melyseg; }
        public bool TerminalisCsucsE() { return allapot.vegAllapotE(); }
        public int getNumberOfOps() { return allapot.getNumberOfOps(); }
        public bool SzuperOperator(int i) { return allapot.szuperOp(i); }
        public override bool Equals(Object obj)
        {
            Csucs cs = (Csucs)obj;
            return allapot.Equals(cs.allapot);
        }
        public override int GetHashCode() { return allapot.GetHashCode(); }
        public override String ToString() { return allapot.ToString(); }
        public List<Csucs>Kiterjesztes()
        {
            List<Csucs> ujCsucsok = new List<Csucs>();
            for (int i = 0; i < getNumberOfOps(); i++)
            {
                Csucs ujCsucs = new Csucs(this);
                if(ujCsucs.SzuperOperator(i))
                {
                    ujCsucsok.Add(ujCsucs);
                }
            }
            return ujCsucsok;
        }
    }
    abstract class Grafkereso
    {
        private Csucs startCsucs;
        public Grafkereso(Csucs startCsucs)
        {
            this.startCsucs = startCsucs;
        }
        protected Csucs GetStartCsucs() { return startCsucs; }
        public abstract Csucs Kereses();
        public void megoldasKiirasa(Csucs egyTermialisCsucs)
        {
            if (egyTermialisCsucs == null)
            {
                Console.WriteLine("Nincs megoldás!");
                return;
            }
            Stack<Csucs> megoldas = new Stack<Csucs>();
            Csucs aktCsucs = egyTermialisCsucs;
            while (aktCsucs != null)
            {
                megoldas.Push(aktCsucs);
                aktCsucs = aktCsucs.GetSzulo();
            }
            foreach (Csucs akt in megoldas) Console.WriteLine(akt);
        }
    }
    class BackTrack : Grafkereso
    {
        int korlat;
        bool emlekezetes;
        public BackTrack(Csucs startCsucs, int korlat,bool emlekezetes)
        :base(startCsucs)
        {
            this.korlat = korlat;
            this.emlekezetes = emlekezetes;
        }
        public BackTrack(Csucs startCsucs) : this(startCsucs, 0, false) { }
        public BackTrack(Csucs startCsucs, int korlat) : this(startCsucs, korlat, false) { }
        public BackTrack(Csucs startCsucs, bool emlekezetes) : this(startCsucs, 0, emlekezetes) { }
        public override Csucs Kereses() { return Kereses(GetStartCsucs()); }
        private Csucs Kereses(Csucs aktCsucs)
        {
            int melyseg = aktCsucs.GetMelyseg();
            if (korlat > 0 && melyseg >= korlat) return null;
            Csucs aktSzulo = null;
            if (emlekezetes) aktSzulo = aktCsucs.GetSzulo();
            while(aktSzulo != null)
            {
                if(aktCsucs.TerminalisCsucsE())
                {
                    return aktCsucs;
                }
                for (int i = 0; i < aktCsucs.getNumberOfOps(); i++)
                {
                    Csucs ujCsucs = new Csucs(aktCsucs);
                    if(ujCsucs.SzuperOperator(i))
                    {
                        Csucs terminalis = Kereses(ujCsucs);
                        if(terminalis != null)
                        {
                            return terminalis;
                        }
                    }
                }  
            }
            return null;
        }

    }
    class MelysegiKereses : Grafkereso
    {
        Stack<Csucs> Nyilt; // Nílt csúcsok halmaza.
        List<Csucs> Zart; // Zárt csúcsok halmaza.
        bool korFigyeles; // Ha hamis, végtelen ciklusba eshet.
        public MelysegiKereses(Csucs startCsucs, bool korFigyeles) :
        base(startCsucs)
    {
            Nyilt = new Stack<Csucs>();
            Nyilt.Push(startCsucs); // kezdetben csak a start csúcs nyílt
            Zart = new List<Csucs>(); // kezdetben a zárt csúcsok halmaza üres
            this.korFigyeles = korFigyeles;
        }
        // A körfigyelés alapértelmezett értéke igaz.
        public MelysegiKereses(Csucs startCsucs) : this(startCsucs, true) { }
        // A megoldás keresés itt indul.
        public override Csucs Kereses()
        {
            // Ha nem kell körfigyelés, akkor sokkal gyorsabb az algoritmus.
            if (korFigyeles) return TerminalisCsucsKereses();
            return TerminalisCsucsKeresesGyorsan();
        }
        private Csucs TerminalisCsucsKereses()
        {
            // Amíg a nyílt csúcsok halmaza nem nem üres.
            while (Nyilt.Count != 0)
            {
                // Ez a legnagyobb mélységû nyílt csúcs.
                Csucs C = Nyilt.Pop();
                // Ezt kiterjesztem.
                List<Csucs> ujCsucsok = C.Kiterjesztes();
                foreach (Csucs D in ujCsucsok)
                {
                    // Ha megtaláltam a terminális csúcsot, akkor kész vagyok.
                    if (D.TerminalisCsucsE()) return D;
                    // Csak azokat az új csúcsokat veszem fel a nyíltak közé,
                    // amik nem szerepeltek még sem a zárt, sem a nyílt csúcsok halmazában.
                    // A Contains a Csúcs osztályban megírt Equals metódust hívja.
                    if (!Zart.Contains(D) && !Nyilt.Contains(D)) Nyilt.Push(D);
                }
                // A kiterjesztett csúcsot átminõsítem zárttá.
                Zart.Add(C);
            }
            return null;
        }
        // Ezt csak akkor szabad használni, ha biztos, hogy az állapottér gráfban nincs kör!
        // Különben valószínûleg végtelen ciklust okoz.
        private Csucs TerminalisCsucsKeresesGyorsan()
        {
            while (Nyilt.Count != 0)
            {
                Csucs C = Nyilt.Pop();
                List<Csucs> ujCsucsok = C.Kiterjesztes();
                foreach (Csucs D in ujCsucsok)
                {
                    if (D.TerminalisCsucsE()) return D;
                    // Ha nincs kör, akkor felesleges megnézni, hogy D volt-e már nyíltak vagy a zártak közt.
                    Nyilt.Push(D);
                }
                // Ha nincs kör, akkor felesleges C-t zárttá minõsíteni.
            }
            return null;
        }
    }
}
class Program
{
    static void Main(string[] args)
    {
    }
}

