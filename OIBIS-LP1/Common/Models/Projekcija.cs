using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class Projekcija
    {
        [Key]
        public int ProjekcijaId { get; set; }
        public string Naziv { get; set; }
        public DateTime VremeProjekcije { get; set; }
        public int Sala { get; set; }
        public double CenaKarte { get; set; }

        public Projekcija(string naziv, DateTime vremeProjekcije, int sala, double cenaKarte)
        {
            this.Naziv = naziv;
            this.VremeProjekcije = vremeProjekcije;
            this.Sala = sala;
            this.CenaKarte = cenaKarte;
        }

        public Projekcija()
        {
            // Pravilo koje zahteva postojanje parametraless konstruktora za Entity Framework
        }
    }
}
