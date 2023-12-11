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
        Dictionary<int, Projekcija> sveProjekcije = new Dictionary<int, Projekcija>();
        Dictionary<int, Rezervacija> sveRezervacije = new Dictionary<int, Rezervacija>();

        public void DodajProjekciju(string imeProjekcije, DateTime vremeProjekcije, int sala, double cenaKarte)
        {
            X509Certificate2 clcert = clientCert();
            string rawname = clcert.SubjectName.Name;

            if (rawname.Contains("admin"))
            {
                

                try
                {
                    Audit.AuthorizationSuccess(rawname, OperationContext.Current.IncomingMessageHeaders.Action);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                foreach (Projekcija p in sveProjekcije.Values)
                {
                    if (p.Naziv.Equals(imeProjekcije))
                    {
                        Console.WriteLine("Vec postoji projekcija pod tim imenom");
                    }
                    else
                    {
                        //int id = random.Next(0, 1000000);
                        //sveProjekcije.Add(id, new Projekcija(imeProjekcije, DateTime.Now, sala, cenaKarte));
                        Console.WriteLine($"Admin je dodao projekciju koja se zove: {imeProjekcije}");

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

                DateTime time = DateTime.Now;
                string message = String.Format("Access is denied. User {0} tried to call DodajProjekciju method (time: {1}). " +
                    "For this method user needs to be member of group Admin.", rawname, time.TimeOfDay);
                throw new FaultException<SecurityException>(new SecurityException(message));
            }
        }
        public void IzmeniPopust(int noviPopust)
        {
            X509Certificate2 clcert = clientCert();
            string rawname = clcert.SubjectName.Name;



            if (rawname.Contains("admin"))
            {

                try
                {
                    Audit.AuthorizationSuccess(rawname, OperationContext.Current.IncomingMessageHeaders.Action);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                
                Console.WriteLine("Admin je izmenio popust");
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

                DateTime time = DateTime.Now;
                string message = String.Format("Access is denied. User {0} tried to call IzmeniPopust method (time: {1}). " +
                    "For this method user needs to be member of group Admin.", rawname, time.TimeOfDay);
                throw new FaultException<SecurityException>(new SecurityException(message));
            }
        }
        //[PrincipalPermission(SecurityAction.Demand, Role = "Administrate")]
        public void IzmeniProjekciju(string imeProjekcije, DateTime novoVremeProjekcije, int novaSala, double novaCenaKarte)
        {
            X509Certificate2 clcert = clientCert();
            string rawname = clcert.SubjectName.Name;
            //string clientname = CertManager.GroupName(rawname);

            if (rawname.Contains("admin"))
            {
                try
                {
                    Audit.AuthorizationSuccess(rawname, OperationContext.Current.IncomingMessageHeaders.Action);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                Console.WriteLine("Admin je izmenio projekciju");

                
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

                DateTime time = DateTime.Now;
                string message = String.Format("Access is denied. User {0} tried to call IzmeniProjekciju method (time: {1}). " +
                    "For this method user needs to be member of group Admin.", rawname, time.TimeOfDay);
                throw new FaultException<SecurityException>(new SecurityException(message));
            }
        }
        //[PrincipalPermission(SecurityAction.Demand, Role = "Korisnik")]
        public void NapraviRezervaciju(string imeProjekcije, int brojKarata)
        {
            //if (Thread.CurrentPrincipal.IsInRole("korisnik") || Thread.CurrentPrincipal.IsInRole("vip"))
            X509Certificate2 clcert = clientCert();
            string rawname = clcert.SubjectName.Name;
            //string clientname = CertManager.GroupName(rawname);

            if (rawname.Contains("korisnik") || rawname.Contains("vip"))
            {
                try
                {
                    Audit.AuthorizationSuccess(rawname, OperationContext.Current.IncomingMessageHeaders.Action);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                Console.WriteLine("Korisnik je napravio rezervaiju");

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

                DateTime time = DateTime.Now;
                string message = String.Format("Access is denied. User {0} tried to call NapraviRezervaciju method (time: {1}). " +
                    "For this method user needs to be member of group Korisnik or Vip.", rawname, time.TimeOfDay);
                throw new FaultException<SecurityException>(new SecurityException(message));
            }
        }
        //[PrincipalPermission(SecurityAction.Demand, Role = "Korisnik")]
        public void PlatiRezervaciju(string idRezervacije)
        {
            X509Certificate2 clcert = clientCert();
            string rawname = clcert.SubjectName.Name;
            //string clientname = CertManager.GroupName(rawname);

            if (rawname.Contains("korisnik") || rawname.Contains("vip"))
            {
                try
                {
                    Audit.AuthorizationSuccess(rawname, OperationContext.Current.IncomingMessageHeaders.Action);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                Console.WriteLine("Korisnik je platio rezervaciju");

                
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

                DateTime time = DateTime.Now;
                string message = String.Format("Access is denied. User {0} tried to call PlatiRezervaciju method (time: {1}). " +
                    "For this method user needs to be member of group Korisnik or VIP.", rawname, time.TimeOfDay);
                throw new FaultException<SecurityException>(new SecurityException(message));
            }
        }

        private X509Certificate2 clientCert()
        {
            X509Certificate2 cCert = (X509Certificate2)OperationContext.Current.ServiceSecurityContext.AuthorizationContext.ClaimSets
                .Where(c => c is X509CertificateClaimSet)
                .Select(c => ((X509CertificateClaimSet)c).X509Certificate).FirstOrDefault();
            return cCert;
        }

        
    }
}
