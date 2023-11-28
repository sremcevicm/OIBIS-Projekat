using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class Servis : IServis
    {
        [PrincipalPermission(SecurityAction.Demand, Role = "Administrate")]
        public void DodajProjekciju()
        {
            Console.WriteLine("Admin je dodao projekciju");
        }
        [PrincipalPermission(SecurityAction.Demand, Role = "Administrate")]
        public void IzmeniPopust()
        {
            Console.WriteLine("Admin je izmenio popust");
        }
        [PrincipalPermission(SecurityAction.Demand, Role = "Administrate")]
        public void IzmeniProjekciju()
        {
            Console.WriteLine("Admin je izmenio projekciju");
        }
        [PrincipalPermission(SecurityAction.Demand, Role = "Korisnik")]
        public void NapraviRezervaciju()
        {
            Console.WriteLine("Korisnik je napravio rezervaiju");
        }
        [PrincipalPermission(SecurityAction.Demand, Role = "Korisnik")]
        public void PlatiRezervaciju()
        {
            Console.WriteLine("Korisnik je platio rezervaciju");
        }
    }
}
