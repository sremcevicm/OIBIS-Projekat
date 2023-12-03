using Common;
using Manager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Debugger.Launch();
            string srvCertCN = "Service";
            NetTcpBinding binding = new NetTcpBinding();
            //string address = "net.tcp://localhost:9999/SecurityService";

            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

            X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, srvCertCN);
            EndpointAddress address = new EndpointAddress(new Uri("net.tcp://localhost:4000/Servis"),
                                      new X509CertificateEndpointIdentity(srvCert));
            

            using (ClientProxy proxy = new ClientProxy(binding, address))
            {
                /// 1. Communication test
                //proxy.Test();
                proxy.DodajProjekciju();
                //proxy.IzmeniProjekciju();
                //proxy.IzmeniPopust();
               //proxy.NapraviRezervaciju();
                proxy.PlatiRezervaciju();
                Console.WriteLine("TestCommunication() finished. Press <enter> to continue ...");
                Console.ReadLine();
            }

        }
    }
}
