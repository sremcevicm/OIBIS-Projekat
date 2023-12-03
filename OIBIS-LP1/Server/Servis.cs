using Common;
using Manager;
using System;
using System.Collections.Generic;
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
        public void DodajProjekciju()
        {
            var rawname = Formatter.ParseName(Thread.CurrentPrincipal.Identity.Name);
            Console.WriteLine(rawname);

            string clientname = CertManager.groupName(StoreName.My, StoreLocation.LocalMachine, rawname);

            if (clientname.Contains("admin"))
            {
                //if (Thread.CurrentPrincipal.IsInRole("admin"))
                Console.WriteLine("Admin je dodao projekciju");
            }
        }
        //[PrincipalPermission(SecurityAction.Demand, Role = "Administrate")]
        public void IzmeniPopust()
        {
            //if (Thread.CurrentPrincipal.IsInRole("admin"))
            string name = CertManager.groupName(StoreName.My, StoreLocation.LocalMachine, WindowsIdentity.GetCurrent().Name);
            if (name == "admin")
                Console.WriteLine("Admin je izmenio popust");
        }
        //[PrincipalPermission(SecurityAction.Demand, Role = "Administrate")]
        public void IzmeniProjekciju()
        {
            //if (Thread.CurrentPrincipal.IsInRole("admin"))
            string name = CertManager.groupName(StoreName.My, StoreLocation.LocalMachine, WindowsIdentity.GetCurrent().Name);
            if (name == "admin")
                Console.WriteLine("Admin je izmenio projekciju");
        }
        //[PrincipalPermission(SecurityAction.Demand, Role = "Korisnik")]
        public void NapraviRezervaciju()
        {
            //if (Thread.CurrentPrincipal.IsInRole("korisnik") || Thread.CurrentPrincipal.IsInRole("vip"))
            string name = CertManager.groupName(StoreName.My, StoreLocation.LocalMachine, WindowsIdentity.GetCurrent().Name);
            if (name == "vip" || name == "korisnik")
                Console.WriteLine("Korisnik je napravio rezervaiju");
        }
        //[PrincipalPermission(SecurityAction.Demand, Role = "Korisnik")]
        public void PlatiRezervaciju()
        {
            ServiceSecurityContext securityContext = ServiceSecurityContext.Current;
            string clientName;
            // Provera da li postoji sigurnosni kontekst
            if (securityContext != null)
            {
                // Pristupanje principalu koji sadrži informacije o klijentu
                clientName = securityContext.PrimaryIdentity.Name;
            }
            else
            {
                clientName = null;
            }

            string name = CertManager.groupName(StoreName.My, StoreLocation.LocalMachine, clientName);
            if (name == "OU=vip" || name == "OU=korisnik")
            {
                Console.WriteLine("Korisnik je platio rezervaciju");
            }
        }
    }
}
