using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            WSHttpBinding binding = new WSHttpBinding(SecurityMode.Message);
            binding.Security.Message.ClientCredentialType = MessageCredentialType.Certificate;
            binding.Security.Message.NegotiateServiceCredential = false;
            binding.Security.Message.EstablishSecurityContext = false;
            

            using (ServiceHost host = new ServiceHost(typeof(ServerTest)))
            {
                X509Certificate2 serviceCertificate = GetServerCertificate(); // Funkcija za dobijanje sertifikata
                host.Credentials.ServiceCertificate.Certificate = serviceCertificate;

                host.Open();

                Console.WriteLine("Server started successfully");

                Console.Read();
            }
        }
    }
}
