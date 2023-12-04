using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class Projekcija
    {
        private int id;
        private string naziv;
        private DateTime vremeProjekcije;
        private int sala;
        private double cenaKarte;

        public Projekcija(int id, string naziv, DateTime vremeProjekcije, int sala, double cenaKarte)
        {
            this.Id = id;
            this.Naziv = naziv;
            this.VremeProjekcije = vremeProjekcije;
            this.Sala = sala;
            this.CenaKarte = cenaKarte;
        }

        public int Id { get => id; set => id = value; }
        public string Naziv { get => naziv; set => naziv = value; }
        public DateTime VremeProjekcije { get => vremeProjekcije; set => vremeProjekcije = value; }
        public int Sala { get => sala; set => sala = value; }
        public double CenaKarte { get => cenaKarte; set => cenaKarte = value; }
    }
}
