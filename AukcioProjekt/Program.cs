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

        public DateTime dateTime
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
                else
                {
                    string uj = (this.legmagasabbLicit * (1 + mertek / 100)).ToString();
                    uj = uj.Substring(0, 2) + new string('0', uj.Length - 2);
                    this.legmagasabbLicit = int.Parse(uj);
                }
                this.licitekSzama++;
                this.legutolsoLicitIdeje = DateTime.Today;
            }
        }

        public override string ToString()
        {
            string adatok = this.festo + " " + this.cim + " (" + this.stilus + ")\n";
            if (elkelt)
            {
                adatok += "Elkelt";
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
                festmenyek.Add(festmenyFelhasznalotol());
            }
            Beolvas();
            //-- véletlenszerű licitalas
            Random rnd = new Random();
            for (int i = 0; i < 20; i++)
            {
                festmenyek[rnd.Next(festmenyek.Count)].Licit(rnd.Next(10, 100));
            }
            //-- felhasznaloi licit
            Console.WriteLine("\nKérem, licitáljon!");
            felhasznaloiLicit();
            foreach (Festmeny item in festmenyek)
            {
                Console.WriteLine(item.ToString());
            }
            Console.WriteLine("\nProgram vége!");
            Console.ReadKey();
        }

        static void Beolvas()
        {
            Console.WriteLine(" Bálint Ferenc adatainak beolvasása...");
            string fajl = @"..\..\festmenyek.csv";
            StreamReader sr = null;
            try
            {
                using (sr = new StreamReader(fajl))
                {
                    while (!sr.EndOfStream)
                    {
                        string[] sor = sr.ReadLine().Split(';');
                        festmenyek.Add(new Festmeny(sor[0], sor[1], sor[2]));
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
        static Festmeny festmenyFelhasznalotol()
        {
            Console.Write("\nA festő neve: ");
            string festo = Console.ReadLine();
            Console.Write("A mű címe: ");
            string cim = Console.ReadLine();
            Console.Write("Stílus: ");
            string stilus = Console.ReadLine();
            return new Festmeny(cim, festo, stilus);
        }
        static void felhasznaloiLicit()
        {
            while (true)
            {
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
                } while (!int.TryParse(Console.ReadLine(), out ssz) || ssz < 0 || ssz >= festmenyek.Count);
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
                    Console.WriteLine("Time Difference (minutes): " + DateTime.Today.Subtract(festmenyek[ssz].dateTime).Minutes);
                    if (DateTime.Today.Subtract(festmenyek[ssz-1].dateTime).Minutes > 2)
                    {
                        festmenyek[ssz - 1].Elkelt = true;
                        Console.WriteLine("Elkelt!");
                        continue;
                    }

                }
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
    }
}
