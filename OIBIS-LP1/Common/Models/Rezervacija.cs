using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class Rezervacija
    {
        private int id;
        int idProjekcije;
        DateTime vremeRezervacije;
        int kolicinaKarata;
        Status stanjeRezervacije;

        public Rezervacija(int id, int idProjekcije, DateTime vremeRezervacije, int kolicinaKarata, Status stanjeRezervacije)
        {
            this.Id = id;
            this.IdProjekcije = idProjekcije;
            this.VremeRezervacije = vremeRezervacije;
            this.KolicinaKarata = kolicinaKarata;
            this.StanjeRezervacije = stanjeRezervacije;
        }

        public int Id { get => id; set => id = value; }
        public int IdProjekcije { get => idProjekcije; set => idProjekcije = value; }
        public DateTime VremeRezervacije { get => vremeRezervacije; set => vremeRezervacije = value; }
        public int KolicinaKarata { get => kolicinaKarata; set => kolicinaKarata = value; }
        public Status StanjeRezervacije { get => stanjeRezervacije; set => stanjeRezervacije = value; }
    }
}
