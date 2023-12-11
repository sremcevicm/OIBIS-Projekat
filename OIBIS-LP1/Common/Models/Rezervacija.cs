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
        string kreirao;

        public Rezervacija(int idProjekcije, DateTime vremeRezervacije, int kolicinaKarata, Status stanjeRezervacije, string kreirao)
        {
            this.IdProjekcije = idProjekcije;
            this.VremeRezervacije = vremeRezervacije;
            this.KolicinaKarata = kolicinaKarata;
            this.StanjeRezervacije = stanjeRezervacije;
            this.Kreirao = kreirao;
        }

        public int Id { get => id; set => id = value; }
        public int IdProjekcije { get => idProjekcije; set => idProjekcije = value; }
        public DateTime VremeRezervacije { get => vremeRezervacije; set => vremeRezervacije = value; }
        public int KolicinaKarata { get => kolicinaKarata; set => kolicinaKarata = value; }
        public Status StanjeRezervacije { get => stanjeRezervacije; set => stanjeRezervacije = value; }
        public string Kreirao { get => kreirao; set => kreirao = value; }
    }
}
