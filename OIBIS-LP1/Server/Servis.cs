using Common;
using Common.Models;
using Manager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Claims;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    //[ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class Servis : IServis
    {

        public void DodajProjekciju(string imeProjekcije, DateTime vremeProjekcije, int sala, double cenaKarte)
        {

            X509Certificate2 clcert = clientCert();
            string rawname = clcert.SubjectName.Name;

            
            Audit.AuthenticationSuccess(rawname);

            if (rawname.Contains("admin"))
            {

                Audit.AuthorizationSuccess(rawname, OperationContext.Current.IncomingMessageHeaders.Action);

                //DbHelper.DodajProjekciju(imeProjekcije, vremeProjekcije, sala, cenaKarte, rawname);
                using (var dbContext = new MyDbContext())
                {
                    var novaProjekcija = new Projekcija { Naziv = imeProjekcije, CenaKarte = cenaKarte, VremeProjekcije = vremeProjekcije, Sala = sala};
                    dbContext.projekcijas.Add(novaProjekcija);
                    if(dbContext.SaveChanges() > 0)
                    {
                        Audit.DataBaseWriteSuccess(rawname);
                    }
                    else
                    {
                        Audit.DataBaseWriteFailed(rawname, "Nije bilo nikakvih izmena");
                    }
                }
                
            }
            else
            {
                try
                {
                    Audit.AuthorizationFailed(rawname, OperationContext.Current.IncomingMessageHeaders.Action, "This method requires Admin permission");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                } 
            }
        }
        public void IzmeniPopust(int noviPopust)
        {
            X509Certificate2 clcert = clientCert();
            string rawname = clcert.SubjectName.Name;

            Audit.AuthenticationSuccess(rawname);

            if (rawname.Contains("admin"))
            {

                Audit.AuthorizationSuccess(rawname, OperationContext.Current.IncomingMessageHeaders.Action);

                using (var dbContext = new MyDbContext()) 
                {
                    try
                    {
                        var existingDiscount = dbContext.popusts.SingleOrDefault();
                        Audit.DataBaseReadSuccess(rawname);
                        if (existingDiscount == null)
                        {
                            // Nema postojećeg popusta, dodajte novi
                            var newDiscount = new Popust { Procenat = noviPopust };
                            dbContext.popusts.Add(newDiscount);
                        }
                        else
                        {
                            // Postoji popust, izmenite ga
                            existingDiscount.Procenat = noviPopust;
                        }

                        if (dbContext.SaveChanges() > 0)
                        {
                            Audit.DataBaseWriteSuccess(rawname);
                        }
                        else
                        {
                            Audit.DataBaseWriteFailed(rawname, "Nije bilo nikakvih izmena");
                        }
                    }
                    catch(Exception ex)
                    {
                        Audit.DataBaseReadFailed(rawname, ex.Message);
                    }
                }
            }
            else
            {
                try
                {
                    Audit.AuthorizationFailed(rawname, OperationContext.Current.IncomingMessageHeaders.Action, "This method requires Admin permission");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        //[PrincipalPermission(SecurityAction.Demand, Role = "Administrate")]
        public void IzmeniProjekciju(string imeProjekcije, DateTime novoVremeProjekcije, int novaSala, double novaCenaKarte)
        {
            X509Certificate2 clcert = clientCert();
            string rawname = clcert.SubjectName.Name;
            //string clientname = CertManager.GroupName(rawname);

            Audit.AuthenticationSuccess(rawname);

            if (rawname.Contains("admin"))
            {
                Audit.AuthorizationSuccess(rawname, OperationContext.Current.IncomingMessageHeaders.Action);

                using (var dbContext = new MyDbContext())
                {
                    try
                    {
                        var projekcijaZaIzmenu = dbContext.projekcijas.FirstOrDefault(p => p.Naziv.Equals(imeProjekcije));
                        Audit.DataBaseReadSuccess(rawname);
                        if (projekcijaZaIzmenu != null)
                        {
                            projekcijaZaIzmenu.VremeProjekcije = novoVremeProjekcije;
                            projekcijaZaIzmenu.Sala = novaSala;
                            projekcijaZaIzmenu.CenaKarte = novaCenaKarte;

                            if (dbContext.SaveChanges() > 0)
                            {
                                Audit.DataBaseWriteSuccess(rawname);
                            }
                            else
                            {
                                //ovde ide audit za database write failed
                                Audit.DataBaseWriteFailed(rawname, "Nije bilo nikakvih izmena");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Nema projekcije sa tim imenom");
                        }
                    }
                    catch(Exception ex)
                    {
                        Audit.DataBaseReadFailed(rawname, ex.Message);
                    }
                }
                
            }
            else
            {
                try
                {
                    Audit.AuthorizationFailed(rawname, OperationContext.Current.IncomingMessageHeaders.Action, "This method requires Admin permission");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        //[PrincipalPermission(SecurityAction.Demand, Role = "Korisnik")]
        public int NapraviRezervaciju(int idProjekcije, int brojKarata)
        {
            X509Certificate2 clcert = clientCert();
            string rawname = clcert.SubjectName.Name;
            int retid = -1;

            Audit.AuthenticationSuccess(rawname);

            if (rawname.Contains("korisnik") || rawname.Contains("vip"))
            {
                ProveriDodajKorisnika(rawname);
                Audit.AuthorizationSuccess(rawname, OperationContext.Current.IncomingMessageHeaders.Action);

                using (var dbContext = new MyDbContext())
                {
                    try
                    {
                        var p = dbContext.projekcijas.FirstOrDefault(pr => pr.ProjekcijaId == idProjekcije);
                        Audit.DataBaseReadSuccess(rawname);
                        if (p != null)
                        {
                            var r = new Rezervacija
                            {
                                IdProjekcije = p.ProjekcijaId,
                                VremeRezervacije = DateTime.Now,
                                KolicinaKarata = brojKarata,
                                StanjeRezervacije = Status.NEPLACENA,
                                Kreirao = rawname
                            };

                            dbContext.rezervacijas.Add(r);
                            //retid = dbContext.rezervacijas.Add(r).RezervacijaId;
                            if (dbContext.SaveChanges() > 0)
                            {
                                Audit.DataBaseWriteSuccess(rawname);

                            }
                            else
                            {
                                //ovde ide audit za database write failed
                                Audit.DataBaseWriteFailed(rawname, "Nije bilo nikakvih izmena");
                            }

                            retid = r.RezervacijaId;
                        }
                        else
                        {
                            Console.WriteLine("Nema projekcije pod tim nazivom");
                            return -1;
                        }
                    }
                    catch (Exception ex)
                    {
                        Audit.DataBaseReadFailed(rawname, ex.Message);
                    }
                        
                    
                    return retid;
                }

            }
            else
            {
                try
                {
                    Audit.AuthorizationFailed(rawname, OperationContext.Current.IncomingMessageHeaders.Action, "This method requires Vip or Korisnik permission");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                

                //DateTime time = DateTime.Now;
                
                return -1;
            }
            
        }
        //[PrincipalPermission(SecurityAction.Demand, Role = "Korisnik")]
        public int PlatiRezervaciju(int idRezervacije)
        {
            X509Certificate2 clcert = clientCert();
            string rawname = clcert.SubjectName.Name;
            //string clientname = CertManager.GroupName(rawname);
            Audit.AuthenticationSuccess(rawname);
            double ukupnaCena = -1;
            int cena = -1;

            if (rawname.Contains("korisnik") || rawname.Contains("vip"))
            {
                Audit.AuthorizationSuccess(rawname, OperationContext.Current.IncomingMessageHeaders.Action);

                //Console.WriteLine("Korisnik je platio rezervaciju");
                using (var dbContext = new MyDbContext())
                {
                    try
                    {
                        var rezervacija = dbContext.rezervacijas.FirstOrDefault(r => r.RezervacijaId == idRezervacije && r.StanjeRezervacije == Status.NEPLACENA);
                        if (rezervacija != null)
                        {
                            var korisnik = dbContext.korisniks.FirstOrDefault(k => k.ImeKorisnika == rawname);
                            var projekcija = dbContext.projekcijas.FirstOrDefault(p => p.ProjekcijaId == rezervacija.IdProjekcije);


                            if (rawname.Contains("vip"))
                            {
                                var popust = dbContext.popusts.SingleOrDefault();
                                ukupnaCena = (projekcija.CenaKarte * rezervacija.KolicinaKarata) * (1 - (double)popust.Procenat / 100);
                                //Console.WriteLine(popust.Procenat.ToString());
                            }
                            else
                            { 
                                ukupnaCena = (projekcija.CenaKarte * rezervacija.KolicinaKarata);
                                //Console.WriteLine(ukupnaCena.ToString());
                            }

                            if (korisnik.StanjeNaRacunu >= ukupnaCena)
                            {
                                korisnik.StanjeNaRacunu -= Convert.ToInt32(ukupnaCena);
                                rezervacija.StanjeRezervacije = Status.PLACENA;

                                if (dbContext.SaveChanges() > 0)
                                {
                                    Audit.DataBaseWriteSuccess(rawname);
                                }
                                else
                                {
                                    //ovde ide audit za database write failed
                                    Audit.DataBaseWriteFailed(rawname, "Nije bilo nikakvih izmena");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Ne postoji rezervacija sa tim id-em");
                        }
                    }
                    catch (Exception ex)
                    {
                        Audit.DataBaseReadFailed(rawname, ex.Message);
                    }
                }
                return cena = Convert.ToInt32(ukupnaCena);
            }
            else
            {
                try
                {
                    Audit.AuthorizationFailed(rawname, OperationContext.Current.IncomingMessageHeaders.Action, "This method requires Vip or Korisnik permission");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                
                return -1;
            }
        }

        private void ProveriDodajKorisnika(string rawname)
        {
            Random random = new Random();
            using (var dbContext = new MyDbContext()) // Zamijenite MyDbContext sa stvarnim imenom vašeg DbContext-a
            {
                // Provera da li korisnik postoji u bazi
                try
                {
                    var korisnik = dbContext.korisniks.FirstOrDefault(k => k.ImeKorisnika == rawname);

                    if (korisnik == null)
                    {
                        // Kreiranje novog korisnika ako ne postoji
                        var noviKorisnik = new Korisnik
                        {
                            ImeKorisnika = rawname,
                            StanjeNaRacunu = random.Next(3000, 15001) // Postavite željeno stanje na računu
                                                                      // Dodajte ostale propertije prema vašim potrebama
                        };

                        // Dodajte novog korisnika u DbSet (tabelu) u DbContext-u
                        dbContext.korisniks.Add(noviKorisnik);

                        // Sačuvajte promene u bazi podataka
                        if (dbContext.SaveChanges() > 0)
                        {
                            Audit.DataBaseWriteSuccess(rawname);
                        }
                        else
                        {
                            //ovde ide audit za database write failed
                            Audit.DataBaseWriteFailed(rawname, "Nije bilo nikakvih izmena");
                        }

                        Console.WriteLine("Novi korisnik je uspešno kreiran.");
                    }
                    else
                    {
                        Console.WriteLine("Korisnik već postoji u bazi.");
                    }
                }
                catch (Exception ex)
                {
                    Audit.DataBaseReadFailed(rawname, ex.Message);
                }
            }
            
        }

        private X509Certificate2 clientCert()
        {
            X509Certificate2 cCert = (X509Certificate2)OperationContext.Current.ServiceSecurityContext.AuthorizationContext.ClaimSets
                .Where(c => c is X509CertificateClaimSet)
                .Select(c => ((X509CertificateClaimSet)c).X509Certificate).FirstOrDefault();
            return cCert;
        }

        public string GetProjekcijeString()
        {
            using (var dbContext = new MyDbContext()) // Zamijenite MyDbContext sa stvarnim imenom vaše DbContext klase
            {
                List<Projekcija> projekcije = dbContext.projekcijas.ToList();

                if (projekcije.Count == 0)
                {
                    return "Nema dostupnih projekcija.";
                }

                // Formiranje stringa za lep prikaz
                string result = "Projekcije:\n";
                foreach (var projekcija in projekcije)
                {
                    result += $"ID: {projekcija.ProjekcijaId}, Naziv: {projekcija.Naziv}, Vreme: {projekcija.VremeProjekcije}, Sala: {projekcija.Sala}, Cena: {projekcija.CenaKarte}\n";
                }

                return result;
            }
        }
    }
}
