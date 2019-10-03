using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AukcioProjekt
{
    class Festmeny
    {
        private string cim;
        private string festo;

        public string Festo
        {
            get { return festo; }
        }
        private string stilus;

        public string Stilus
        {
            get { return stilus; }
        }
        private int licitekSzama;

        public int LicitekSzama
        {
            get { return licitekSzama; }
        }

        private int legmagasabbLicit;

        public int LegmagasabbLicit
        {
            get { return legmagasabbLicit; }
            set { legmagasabbLicit = value; }
        }
        private DateTime legutolsoLicitIdeje;

        public DateTime LegutolsoLicitIdeje
        {
            get { return legutolsoLicitIdeje; }
        }


        private bool elkelt;

        public bool Elkelt
        {
            get { return elkelt; }
            set { elkelt = value; }
        }

        public void Licit()
        {
            this.Licit(10);
        }

        public void Licit(int mertek)
        {
            if (mertek<10)
            {
                Console.WriteLine("Túl alacsony licit!");
                return;
            }
            else if (mertek>100)
            {
                Console.WriteLine("Túl magas licit!");
                return;
            }

            if (this.elkelt)
            {
                Console.WriteLine("A festmény már elkelt, nem lehet rá licitálni!");
            }
            else
            {
                if (this.licitekSzama==0)
                {
                    this.legmagasabbLicit = 100;
                }
                string uj = (this.legmagasabbLicit * (1 + mertek / 100.0)).ToString();
                uj = uj.Substring(0, 2) + new string('0', uj.Length - 2);
                this.legmagasabbLicit = int.Parse(uj);
                this.licitekSzama++;
                this.legutolsoLicitIdeje = DateTime.Today;
            }
        }

        public override string ToString()
        {
            string adatok = "\n" + this.festo + ": " + this.cim + " (" + this.stilus + ")\n\t";
            if (elkelt)
            {
                adatok += "Elkelt";
            }
            else if (this.licitekSzama==0)
            {
                adatok += "Nem volt licit.";
            }
            else
            {
                adatok += "licit: " + this.legmagasabbLicit + " - " + this.legutolsoLicitIdeje + " (" + this.licitekSzama + ")\n";
            }

            return adatok;
        }
        public Festmeny(string cim, string festo, string stilus)
        {
            this.cim = cim;
            this.festo = festo;
            this.stilus = stilus;
            this.licitekSzama = 0;
            this.elkelt = false;
            this.legutolsoLicitIdeje = default(DateTime);
        }

    }
    class Program
    {
        static List<Festmeny> festmenyek = new List<Festmeny>();

        static void Main(string[] args)
        {
            //-- 2.a feladat
            festmenyek.Add(new Festmeny("Csendes élet", "Moderson-Becker", "Expresszionista"));
            festmenyek.Add(new Festmeny("Kék ló", "Franz Marc", "Expresszionista"));
            Console.WriteLine("\nHány festményt szeretne megadni?");
            int db = int.Parse(Console.ReadLine());
            for (int i = 0; i < db; i++)
            {
                festmenyek.Add(festmenyFelhasznalotol(i + 1));
            }
            //-- Bálint Ferenc festményinek beolvasása ---------
            Beolvas();
            //-- véletlenszerű licitalas -----------------------
            veletlenLicit();
            //-- felhasznaloi licit ----------------------------
            felhasznaloiLicit();
            //-- festmények adatai -----------------------------
            for (int i = 0; i < festmenyek.Count; i++)
            {
                Console.Write($"{i + 1}. ");
                Console.WriteLine(festmenyek[i].ToString());

            }
            //-- 3 a.) Keresd meg a legdrágábban elkelt festményt, majd az adatait konzolra.
            Console.WriteLine("\nA legdrágábban elkelt festmény:");
            Festmeny max = festmenyek.Find(x => x.LegmagasabbLicit == festmenyek.Max(y => y.LegmagasabbLicit));
            Console.WriteLine(max.ToString());

            //-- 3 b.) Döntsd el, hogy van-e olyan festmény, amelyre 10-nél több alkalommal licitáltak.
            if (festmenyek.Max(x => x.LicitekSzama) > 10)
            {
                Console.WriteLine("\nVolt olyan kép, amire 10-nél többször licitáltak.");
            }
            else
            {
                Console.WriteLine("\nNem volt olyan kép, amire 10-nél többször licitáltak.");
            }

            //-- 3 c.) Számold meg, hogy hány olyan festmény van, amely nem kelt el.
            db = festmenyek.FindAll(x => x.Elkelt == false).Count;
            Console.WriteLine($"{db} festmény nem kelt el.");

            //-- 3 d.) Rendezd át a listát a Legmagasabb Licit szerint csökkenő sorrendben, majd írd ki újra a festményeket.
            festmenyek = festmenyek.OrderByDescending(x => x.LegmagasabbLicit).ToList();
            for (int i = 0; i < festmenyek.Count; i++)
            {
                Console.Write($"{i + 1}. ");
                Console.WriteLine(festmenyek[i].ToString());

            }
            //-- Kiiras fájlba ---------------------------------------------------
            festmenyekFajlba();
            Console.WriteLine("\nProgram vége!");
            Console.ReadKey();
        }

        static void Beolvas()
        {
            Console.WriteLine("\t- Bálint Ferenc adatainak beolvasása...");
            string fajl = @"..\..\festmenyek.csv";
            StreamReader sr = null;
            try
            {
                using (sr = new StreamReader(fajl))
                {
                    while (!sr.EndOfStream)
                    {
                        string[] sor = sr.ReadLine().Split(';');
                        festmenyek.Add(new Festmeny(sor[1], sor[0], sor[2]));
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            finally
            {
                if (sr!=null)
                {
                    sr.Close();
                    sr.Dispose();
                }
            }
        }
        static Festmeny festmenyFelhasznalotol(int ssz)
        {
            Console.Write($"\n\tA {ssz}. festő neve: ");
            string festo = Console.ReadLine();
            Console.Write($"\tA {ssz}. mű címe: ");
            string cim = Console.ReadLine();
            Console.Write($"\tA {ssz}. kép stílusa: ");
            string stilus = Console.ReadLine();
            return new Festmeny(cim, festo, stilus);
        }
        static void veletlenLicit()
        {
            //-- véletlenszerű licitalas
            Console.WriteLine("\n\t- 20 véletlenszerű licitálás...");
            Random rnd = new Random();
            for (int i = 0; i < 20; i++)
            {
                festmenyek[rnd.Next(festmenyek.Count)].Licit(rnd.Next(10, 100));
            }

        }
        static void felhasznaloiLicit()
        {
            Console.WriteLine("\nKérem, licitáljon!");
            while (true)
            {
                //--- A kép sorszámának bekérése ----------------------------------------------------
                Console.WriteLine($"\nKérem a kép sorszámát (1-{festmenyek.Count}): ");
                int db = 0;
                int ssz = 0;
                do
                {
                    if (db > 0)
                    {
                        Console.WriteLine($"Kérem 1 és {festmenyek.Count} közötti értéket adjon meg!");
                    }
                    db++;
                } while (!int.TryParse(Console.ReadLine(), out ssz) || ssz < 0 || ssz > festmenyek.Count);
                if (ssz == 0)
                {
                    return;
                }
                else
                {
                    /*
                     * A sorszám megadása után, ha az adott festményre több mint 2 perce
                     * érkezett utoljára licit akkor állítsa be elkeltre, 
                     * majd hibaüzenetet írjon ki, majd kérjen be új sorszámot
                     */
                    //Console.WriteLine("Time Difference (minutes): " + DateTime.Today.Subtract(festmenyek[ssz].LegutolsoLicitIdeje).Minutes);
                    if (DateTime.Today.Subtract(festmenyek[ssz-1].LegutolsoLicitIdeje).Minutes > 2)
                    {
                        festmenyek[ssz - 1].Elkelt = true;
                        Console.WriteLine("\tElkelt!");
                        continue;
                    }

                }
                //-- A licit értékének a bekérése -----------------------------------
                int licit = 0;
                Console.Write("A licit értéke: ");
                if (int.TryParse(Console.ReadLine(), out licit))
                {
                    if (licit==0)
                    {
                        festmenyek[ssz - 1].Licit();
                    }
                    else
                    {
                        festmenyek[ssz - 1].Licit(licit);
                    }
                }
                else
                {
                    Console.WriteLine("\nÉrvénytelen licit!");
                    return;
                }

            }
        }

        static void festmenyekFajlba()
        {
            Console.WriteLine("\nA rendezett lista tartalmának kiirasa festmenyek_rendezett.csv nevű fájlba.");
            StreamWriter sw = null;
            try
            {
                using (sw = new StreamWriter("festmenyek_rendezett.csv"))
                {
                    for (int i = 0; i < festmenyek.Count; i++)
                    {
                        sw.WriteLine(festmenyek[i].ToString());
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            finally
            {
                if (sw!=null)
                {
                    sw.Close();
                    sw.Dispose();
                }
            }
        }
    }
}
