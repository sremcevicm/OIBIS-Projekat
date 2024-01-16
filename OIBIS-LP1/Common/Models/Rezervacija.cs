using System;
using System.ComponentModel.DataAnnotations;

namespace Common.Models
{
    public class Rezervacija
    {
        [Key]
        public int RezervacijaId { get; set; }
        public int IdProjekcije { get; set; }
        public DateTime VremeRezervacije { get; set; }
        public int KolicinaKarata { get; set; }
        public Status StanjeRezervacije { get; set; }
        public string Kreirao { get; set; }

        public Rezervacija(int idProjekcije, DateTime vremeRezervacije, int kolicinaKarata, Status stanjeRezervacije, string kreirao)
        {
            this.IdProjekcije = idProjekcije;
            this.VremeRezervacije = vremeRezervacije;
            this.KolicinaKarata = kolicinaKarata;
            this.StanjeRezervacije = stanjeRezervacije;
            this.Kreirao = kreirao;
        }

        public Rezervacija()
        {
            // Pravilo koje zahteva postojanje parametraless konstruktora za Entity Framework
        }
    }
}
