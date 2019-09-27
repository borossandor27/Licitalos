using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            

    }
    class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
