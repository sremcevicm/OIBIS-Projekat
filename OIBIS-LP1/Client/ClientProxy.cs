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

        public void NapraviRezervaciju(string imeProjekcije, int brojKarata)
        {
            try
            {
                factory.NapraviRezervaciju(imeProjekcije, brojKarata);
                Console.WriteLine("Kreiranje rezervacije je odobreno. Broj Vase rezervacije je: ");
            }
            catch (Exception e)
            {
                Console.WriteLine("Greska prilikom kreiranja rezervacije! Error: {0}", e.Message);
            }
        }

        public void PlatiRezervaciju(string idRezervacije)
        {
            try
            {
                factory.PlatiRezervaciju(idRezervacije);
                Console.WriteLine("Placanje rezervacije je odobreno. Platili ste ukupno");
            }
            catch (Exception e)
            {
                Console.WriteLine("Greska prilikom placanja rezervacije! Error: {0}", e.Message);
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
    }
}
