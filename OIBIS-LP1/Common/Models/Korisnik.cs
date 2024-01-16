using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class Korisnik
    {
        [Key]
        public int KorisnikId { get; set; }
        public string ImeKorisnika { get; set; }
        public int StanjeNaRacunu { get; set; }

        public Korisnik()
        {
            // Pravilo koje zahteva postojanje parametraless konstruktora za Entity Framework
        }

        public Korisnik(int stanje, string ime)
        {
            this.ImeKorisnika = ime;
            this.StanjeNaRacunu = stanje;
        }
    }
}
