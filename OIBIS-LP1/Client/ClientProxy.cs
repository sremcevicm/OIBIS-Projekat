using Common;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class ClientProxy : ChannelFactory<IServis>, IServis, IDisposable
    {
        IServis factory;
        //string group;

        public ClientProxy(NetTcpBinding binding, EndpointAddress address) : base(binding, address)
        {
            string cltCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

            this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.ChainTrust;
            this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            /// Set appropriate client's certificate on the channel. Use CertManager class to obtain the certificate based on the "cltCertCN"
            this.Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, cltCertCN);
            //group = groupName(this.Credentials.ClientCertificate.Certificate);

            factory = this.CreateChannel();
            //this.State = CommunicationState.Opened;
        }

       

        public void DodajProjekciju(string imeProjekcije, DateTime vremeRezervacije, int sala, double cenaKarte)
        {
            try
            {
                factory.DodajProjekciju(imeProjekcije, vremeRezervacije, sala, cenaKarte);
                Console.WriteLine($"Dodana projekcija: {imeProjekcije}, Vreme: {vremeRezervacije}, Sala: {sala}, Cena karte: {cenaKarte}");
            }
            catch (Exception e)
            {
                Console.WriteLine("Greska prilikom dodavanja projekcije! Error: {0}", e.Message);
                if (e.InnerException != null)
                {
                    Console.WriteLine("Inner Exception: " + e.InnerException.Message);
                }
            }
        }

        public void IzmeniPopust(int noviPopust)
        {
            try
            {
                factory.IzmeniPopust(noviPopust);
                Console.WriteLine($"Izmena popusta je odobrena. On sada iznosi {noviPopust}");
            }
            catch (Exception e)
            {
                Console.WriteLine("Greska prilikom izmene popusta! Error: {0}", e.Message);
            }
        }

        public void IzmeniProjekciju(string imeProjekcije, DateTime novoVremeProjekcije, int novaSala, double novaCenaKarte)
        {
            try
            {
                factory.IzmeniProjekciju(imeProjekcije, novoVremeProjekcije, novaSala, novaCenaKarte);
                Console.WriteLine($"Izmenjena projekcija: {imeProjekcije}! Izmene ---> Vreme: {novoVremeProjekcije}, Sala: {novaSala}, Cena karte: {novaCenaKarte}");
            }
            catch (Exception e)
            {
                Console.WriteLine("Greska prilikom izmene projekcije! Error: {0}", e.Message);
            }
        }

        public int NapraviRezervaciju(int idProjekcije, int brojKarata)
        {
            try
            {
                int broj = factory.NapraviRezervaciju(idProjekcije, brojKarata);
                Console.WriteLine("Kreiranje rezervacije je odobreno. ID Vase rezervacije je: " + broj.ToString());
                return broj;
            }
            catch (Exception e)
            {
                Console.WriteLine("Greska prilikom kreiranja rezervacije! Error: {0}", e.Message);
                return -1;
            }
        }

        public int PlatiRezervaciju(int idRezervacije)
        {
            try
            {
                int broj = factory.PlatiRezervaciju(idRezervacije);
                Console.WriteLine("Placanje rezervacije je odobreno. Platili ste ukupno " + broj.ToString() + "rsd");
                return broj;
            }
            catch (Exception e)
            {
                Console.WriteLine("Greska prilikom placanja rezervacije! Error: {0}", e.Message);
                return -1;
            }
        }

        

        public void Dispose()
        {
            if (factory != null)
            {
                factory = null;
            }

            this.Close();
        }

        public string GetProjekcijeString()
        {
            string result = "";
            try
            {
                result = factory.GetProjekcijeString();
                //Console.WriteLine("Placanje rezervacije je odobreno. Platili ste ukupno");
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine("Greska priliom iscitavanja projekcija! Error: {0}", e.Message);
             
            }
            return result;
        }
    }
}
