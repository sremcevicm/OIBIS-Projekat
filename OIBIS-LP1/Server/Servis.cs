using Common;
using Common.Models;
using Manager;
using System;
using System.Collections.Generic;
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
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class Servis : IServis
    {
        //[PrincipalPermission(SecurityAction.Demand, Role = "Administrate")]
        Dictionary<int, Projekcija> sveProjekcije = new Dictionary<int, Projekcija>();
        Dictionary<int, Rezervacija> sveRezervacije = new Dictionary<int, Rezervacija>();
        int vipPopust;
        Random random = new Random();
        public void DodajProjekciju(string imeProjekcije, string vremeRezervacije, int sala, double cenaKarte)
        {
            X509Certificate2 clcert = clientCert();
            Console.WriteLine(clcert.SubjectName.Name);
            string rawname = clcert.SubjectName.Name;

            string clientname = CertManager.GroupName(rawname);

            if (clientname.Contains("admin"))
            {
                foreach(Projekcija p in sveProjekcije.Values)
                {
                    if (p.Naziv.Equals(imeProjekcije))
                    {
                        Console.WriteLine("Vec postoji projekcija pod tim imenom");
                    }
                    else
                    {
                        int id = random.Next(0, 1000000);
                        sveProjekcije.Add(id, new Projekcija(id, imeProjekcije, DateTime.Now, 4, 3.52));
                        Console.WriteLine("Admin je uspesno dodao projekciju");
                    }
                }
            }
            else
            {
                DateTime time = DateTime.Now;
                string message = String.Format("Access is denied. User {0} tried to call Delete method (time: {1}). " +
                    "For this method user needs to be member of group Admin.", rawname, time.TimeOfDay);
                throw new FaultException<SecurityException>(new SecurityException(message));
            }
        }
        //[PrincipalPermission(SecurityAction.Demand, Role = "Administrate")]
        public void IzmeniPopust()
        {
            //if (Thread.CurrentPrincipal.IsInRole("admin"))
            X509Certificate2 clcert = clientCert();
            string rawname = clcert.SubjectName.Name;
            string clientname = CertManager.GroupName(rawname);

            if (clientname.Contains("admin"))
                Console.WriteLine("Admin je izmenio popust");
            else
            {
                DateTime time = DateTime.Now;
                string message = String.Format("Access is denied. User {0} tried to call Delete method (time: {1}). " +
                    "For this method user needs to be member of group Admin.", rawname, time.TimeOfDay);
                throw new FaultException<SecurityException>(new SecurityException(message));
            }
        }
        //[PrincipalPermission(SecurityAction.Demand, Role = "Administrate")]
        public void IzmeniProjekciju()
        {
            X509Certificate2 clcert = clientCert();
            string rawname = clcert.SubjectName.Name;
            string clientname = CertManager.GroupName(rawname);

            if (clientname.Contains("admin"))
                Console.WriteLine("Admin je izmenio projekciju");
            else
            {
                DateTime time = DateTime.Now;
                string message = String.Format("Access is denied. User {0} tried to call Delete method (time: {1}). " +
                    "For this method user needs to be member of group Admin.", rawname, time.TimeOfDay);
                throw new FaultException<SecurityException>(new SecurityException(message));
            }
        }
        //[PrincipalPermission(SecurityAction.Demand, Role = "Korisnik")]
        public void NapraviRezervaciju()
        {
            //if (Thread.CurrentPrincipal.IsInRole("korisnik") || Thread.CurrentPrincipal.IsInRole("vip"))
            X509Certificate2 clcert = clientCert();
            string rawname = clcert.SubjectName.Name;
            string clientname = CertManager.GroupName(rawname);

            if (clientname.Contains("korisnik") || clientname.Contains("vip"))
                Console.WriteLine("Korisnik je napravio rezervaiju");
            else
            {
                DateTime time = DateTime.Now;
                string message = String.Format("Access is denied. User {0} tried to call Delete method (time: {1}). " +
                    "For this method user needs to be member of group Admin.", rawname, time.TimeOfDay);
                throw new FaultException<SecurityException>(new SecurityException(message));
            }
        }
        //[PrincipalPermission(SecurityAction.Demand, Role = "Korisnik")]
        public void PlatiRezervaciju()
        {
            X509Certificate2 clcert = clientCert();
            string rawname = clcert.SubjectName.Name;
            string clientname = CertManager.GroupName(rawname);

            if (clientname.Contains("korisnik") || clientname.Contains("vip"))
            {
                Console.WriteLine("Korisnik je platio rezervaciju");
            }
            else
            {
                DateTime time = DateTime.Now;
                string message = String.Format("Access is denied. User {0} tried to call Delete method (time: {1}). " +
                    "For this method user needs to be member of group Admin.", rawname, time.TimeOfDay);
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
