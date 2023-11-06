using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {

            X509Certificate2 clientCertificate = LoadClientCertificate(); // Implementirajte funkciju za učitavanje klijentskog sertifikata

            ChannelFactory<ITest> factory = new ChannelFactory<ITest>(new WSHttpBinding());
            factory.Credentials.ClientCertificate.Certificate = clientCertificate;


            ITest proxy = factory.CreateChannel();

            proxy.Test();

            Console.WriteLine("Client. " + factory.Endpoint.Address);

            Console.ReadLine();

        }
    }
}
