using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Minden állapot osztály õse.
/// </summary>
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
        // Klónoz. Azért van rá szükség, mert némelyik operátor hatását vissza kell vonnunk.
        // A legegyszerûbb, hogy az állapotot leklónozom. Arra hívom az operátort.
        // Ha gond van, akkor visszatérek az eredeti állapothoz.
        // Ha nincs gond, akkor a klón lesz az állapot, amibõl folytatom a keresést.
        // Ez sekély klónozást alkalmaz. Ha elég a sekély klónozás, akkor nem kell felülírni a gyermek osztályban.
        // Ha mély klónozás kell, akkor mindenképp felülírandó.
        public abstract bool szuperOp(int i);
        public virtual object Clone() { return MemberwiseClone(); }
        // Csak akkor kell felülírni, ha emlékezetes backtracket akarunk használni, vagy mélységi keresést.
        // Egyébként maradhat ez az alap implementáció.
        // Programozás technikailag ez egy kampó metódus, amit az OCP megszegése nélkül írhatok felül.
        public override bool Equals(Object a) { return false; }
        // Ha két példány egyenlõ, akkor a hasítókódjuk is egyenlõ.
        // Ezért, ha valaki felülírja az Equals metódust, ezt is illik felülírni.
        public override int GetHashCode() { return base.GetHashCode(); }
    }
    #endregion
    abstract class VakAllapot : AbsztraktAllapot
    {
        // Itt kell megadni azokat a mezõket, amelyek tartalmazzák a belsõ állapotot.
        // Az operátorok belsõ állapot átmenetet hajtanak végre.
        // Elõször az alapoperátorokat kell megírni.
        // Minden operátornak van elõfeltétele.
        // Minden operátor utófeltétele megegyezik az ÁllapotE predikátummal.
        // Az operátor igazat ad vissza, ha alkalmazható, hamisat, ha nem alkalmazható.
        // Egy operátor alkalmazható, ha a belsõ állapotra igaz
        // az elõfeltétele és az állapotátmenet után igaz az utófeltétele.
        // Ez az elsõ alapoperátor.
        private bool op1()
        {
            // Ha az elõfeltétel hamis, akkor az operátor nem alkalmazható.
            if (!preOp1()) return false;
            // állapot átmenet
            //
            // TODO: Itt kell kiegészíteni a kódot!
            //
            // Utófeltétel vizsgálata, ha igaz, akkor alkalmazható az operátor.
            if (vegAllapotE()) return true;
            //
            // TODO: Itt kell kiegészíteni a kódot!
            //
            // és vissza kell adni, hogy nem alkalmazható az operátor.
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
                // Itt kell felsorolnom az összes alapoperátort.
                // Ha egy új operátort veszek fel, akkor ide is fel kell venni.
                case 0: return op1();
                case 1: return op2();
                // A paraméteres operátorok több alap operátornak megfelelnek, ezért itt több sor is tartozik hozzájuk.
                // Hogy hány sor, az feladat függõ.
                case 3: return op3(0);
                case 4: return op3(1);
                case 5: return op3(2);
                default: return false;
            }
        }
        // Visszaadja az alap operátorok számát.
        public override int getNumberOfOps()
        {
            // Az utolsó case számát kell itt visszaadni.
            // Ha bõvítjük az operátorok számát, ezt a számot is növelni kell.
            return 5;
        }
    }
    class FerjekEsFelesegekAllapot : AbsztraktAllapot
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
        {// igaz, ha a szerzetesek biztonságban vannak
            return ((felesegegyikpart >= ferjegyikpart) || (felesegegyikpart == 0)) &&
                ((felesegegyikpart >= ferjmasikpart) || (felesegmasikpart == 0));
        }
        public override bool vegAllapotE()
        {// igaz, ha mindenki átért a jobb oldalra
            return ferjegyikpart == ferj && felesegegyikpart == feleseg;
        }
        private bool preOp(int feleseg, int ferj)
        {        // A csónak 2 személyes, legalább egy ember kell az evezéshez.
                 // Ezt végül is felesleges vizsgálni, mert a szuper operátor csak ennek megfelelõen hívja.
            if (ferj + feleseg > 2 || ferj + feleseg < 0 || ferj < 0 || feleseg < 0) return false;
            if (ferj == 'B')
                return ferjegyikpart >= ferj && felesegegyikpart >= feleseg;
            else
                return ferjmasikpart >= ferj && felesegmasikpart >= feleseg;
        }
        // Átvisz a másik oldalra sz darab szerzetes és k darab kannibált.
        private bool Atvisz(int ferj, int feleseg)
        { //elõfeltétel
            if (!preOp(feleseg, ferj)) return false;
            //állapotváltzozás
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
            //utófeltétel, megj.: az utófeltétel mindig megegyezik az ÁllapotE-vel.
            if (allapotE()) return true;
            return false;
        }
        public override int getNumberOfOps() { return 5; }
        public override bool szuperOp(int i)
        {
            switch (i)
            {
                // Itt kell felsorolnom az összes alapoperátort.
                // Ha egy új operátort veszek fel, akkor ide is fel kell venni.
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
        // Ha két példány egyenlõ, akkor a hasítókódjuk is egyenlõ.
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
        // A szülõkön felfelé haladva a start csúcsig jutok.
        // Konstruktor:
        // A belsõ állapotot beállítja a start csúcsra.
        // A hívó felelõssége, hogy a kezdõ állapottal hívja meg.
        // A start csúcs mélysége 0, szülõje nincs.

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
        // Alkalmazza az összes alkalmazható operátort.
        // Visszaadja az így elõálló új csúcsokat.
        public List<Csucs> Kiterjesztes()
        {
            List<Csucs> ujCsucsok = new List<Csucs>();
            for (int i = 0; i < getNumberOfOps(); i++)
            {
                // Új gyermek csúcsot készítek.
                Csucs ujCsucs = new Csucs(this);
                // Kiprobálom az i.dik alapoperátort. Alkalmazható?
                if (ujCsucs.SzuperOperator(i))
                { // Ha igen, hozzáadom az újakhoz.
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
        /// <summary>
        /// Kiíratja a megoldást egy terminális csúcs alapján.
        /// Feltételezi, hogy a terminális csúcs szülõ referenciáján felfelé haladva eljutunk a start csúcshoz.
        /// A csúcsok sorrendjét megfordítja, hogy helyesen tudja kiírni a megoldást.
        /// Ha a csúcs null, akkor kiírja, hogy nincs megoldás.
        /// </summary>
        /// <param name="egyTerminálisCsúcs">
        /// A megoldást képviselõ terminális csúcs vagy null.
        /// </param>
        public void megoldasKiirasa(Csucs egyTerminalisCsucs)
        {
            if (egyTerminalisCsucs == null)
            {
                Console.WriteLine("Nincs megoldás!");
                return;
            }
            // Meg kell fordítani a csúcsok sorrendjét.
            Stack<Csucs> megoldas = new Stack<Csucs>();
            Csucs aktCsucs = egyTerminalisCsucs;
            while (aktCsucs != null)
            {
                megoldas.Push(aktCsucs);
                aktCsucs = aktCsucs.GetSzulo();
            }
            // Megfordítottuk, lehet kiírni.
            foreach (Csucs akt in megoldas) Console.WriteLine(akt);
        }
    }
    /// <summary>
    /// A backtrack gráfkeresõ algoritmust megvalósító osztály.
    /// A három alap backtrack algoritmust egyben tartalmazza. Ezek
    /// - az alap backtrack
    /// - mélységi korlátos backtrack
    /// - emlékezetes backtrack
    /// Az ág-korlátos backtrack nincs megvalósítva.
    /// </summary>
    class BackTrack : Grafkereso
    {
        int korlat; // Ha nem nulla, akkor mélységi korlátos keresõ.
        bool emlekezetes; // Ha igaz, emlékezetes keresõ.
        public BackTrack(Csucs startCsucs, int korlat, bool emlekezetes)
        : base(startCsucs)
        {
            this.korlat = korlat;
            this.emlekezetes = emlekezetes;
        }
        // nincs mélységi korlát, se emlékezet
        public BackTrack(Csucs startCsucs) : this(startCsucs, 0, false) { }
        // mélységi korlátos keresõ
        public BackTrack(Csucs startCsucs, int korlat) : this(startCsucs, korlat, false) { }
        // emlékezetes keresõ
        public BackTrack(Csucs startCsucs, bool emlekezetes) : this(startCsucs, 0, emlekezetes) { }
        // A keresés a start csúcsból indul.
        // Egy terminális csúcsot ad vissza. A start csúcsból el lehet jutni ebbe a terminális csúcsba.
        // Ha nincs ilyen, akkor null értéket ad vissza.
        public override Csucs Kereses() { return Kereses(GetStartCsucs()); }
        // A keresõ algoritmus rekurzív megvalósítása.
        // Mivel rekurzív, ezért a visszalépésnek a "return null" felel meg.
        private Csucs Kereses(Csucs aktCsucs)
        {
            int melyseg = aktCsucs.GetMelyseg();
            // mélységi korlát vizsgálata
            if (korlat > 0 && melyseg >= korlat) return null;
            // emlékezet használata kör kiszûréséhez
            Csucs aktSzulo = null;
            if (emlekezetes) aktSzulo = aktCsucs.GetSzulo();
            while (aktSzulo != null)
            {
                // Ellenõrzöm, hogy jártam-e ebben az állapotban. Ha igen, akkor visszalépés.
                if (aktCsucs.Equals(aktSzulo)) return null;
                // Visszafelé haladás a szülõi láncon.
                aktSzulo = aktSzulo.GetSzulo();
            }
            if (aktCsucs.TerminalisCsucsE())
            {
                // Megvan a megoldás, vissza kell adni a terminális csúcsot.
                return aktCsucs;
            }
            // Itt hívogatom az alapoperátorokat a szuper operátoron
            // keresztül. Ha valamelyik alkalmazható, akkor új csúcsot
            // készítek, és meghívom önmagamat rekurzívan.
            for (int i = 0; i < aktCsucs.getNumberOfOps(); i++)
            {
                // Elkészítem az új gyermek csúcsot.
                // Ez csak akkor lesz kész, ha alkalmazok rá egy alkalmazható operátort is.
                Csucs ujCsucs = new Csucs(aktCsucs);
                if (ujCsucs.SzuperOperator(i))
                {
                    // Ha igen, rekurzívan meghívni önmagam az új csúcsra.
                    // Ha nem null értéket ad vissza, akkor megvan a megoldás.
                    // Ha null értéket, akkor ki kell próbálni a következõ alapoperátort.
                    Csucs terminalis = Kereses(ujCsucs);
                    if (terminalis != null)
                    // Visszaadom a megoldást képviselõ terminális csúcsot.!= null)
                    {
                        return terminalis;
                    }
                    // Az else ágon kellene visszavonni az operátort.
                    // Erre akkor van szükség, ha az új gyermeket létrehozásában nem lenne klónozást.
                    // Mivel klónoztam, ezért ez a rész üres.
                }
            }
            return null;
        }
    }
    /// <summary>
    /// Mélységi keresést megvalósító gráfkeresõ osztály.
    /// Ez a megvalósítása a mélységi keresésnek felteszi, hogy a start csúcs nem terminális csúcs.
    /// A nyílt csúcsokat veremben tárolja.
    /// </summary>
    class MelysegiKereses : Grafkereso
    {
        // Mélységi keresésnél érdemes a nyílt csúcsokat veremben tárolni,
        // mert így mindig a legnagyobb mélységû csúcs lesz a verem tetején.
        // Így nem kell külön keresni a legnagyobb mélységû nyílt csúcsot, amit ki kell terjeszteni.
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
    class Program
    {
        static void Main(string[] args)
        {
            Csucs startCsucs;
            Grafkereso kereso;
            Grafkereso kereso2;
            Console.WriteLine("A 2.37-es feladat megoldása");
            startCsucs = new Csucs(new FerjekEsFelesegekAllapot(10, 10));
            Console.WriteLine("A kereső egy mélységi korlátos és emlékezetes backtrack");
            kereso = new BackTrack(startCsucs, 10, true);
            kereso.megoldasKiirasa(kereso.Kereses());
            kereso2 = new MelysegiKereses(startCsucs, true);
            kereso2.megoldasKiirasa(kereso2.Kereses());
            Console.ReadLine();
        }
    }
}

