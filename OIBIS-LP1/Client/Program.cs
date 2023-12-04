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
using System.Threading;
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
            //var rawname = WindowsIdentity.GetCurrent().Name;

            X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, srvCertCN);
            EndpointAddress address = new EndpointAddress(new Uri("net.tcp://localhost:4000/Servis"),
                                      new X509CertificateEndpointIdentity(srvCert));
            

            using (ClientProxy proxy = new ClientProxy(binding, address))
            {
                /// 1. Communication test
                //proxy.Test();

                while (true)
                {
                    Console.WriteLine("Izaberite opciju:");
                    Console.WriteLine("1. Dodaj projekciju");
                    Console.WriteLine("2. Izmeni projekciju");
                    Console.WriteLine("3. Izmeni popust");
                    Console.WriteLine("4. Napravi rezervaciju");
                    Console.WriteLine("5. Plati rezervaciju");
                    Console.WriteLine("0. Izlaz");

                    int choice;
                    if (int.TryParse(Console.ReadLine(), out choice))
                    {
                        switch (choice)
                        {
                            case 1:
                                Console.WriteLine("Unesite ime projekcije:");
                                string imeProjekcije = Console.ReadLine();

                                Console.WriteLine("Unesite vreme rezervacije:");
                                string vremeRezervacije = Console.ReadLine();

                                Console.WriteLine("Unesite broj sale:");
                                if (int.TryParse(Console.ReadLine(), out int sala))
                                {
                                    Console.WriteLine("Unesite cenu karte:");
                                    if (double.TryParse(Console.ReadLine(), out double cenaKarte))
                                    {
                                        proxy.DodajProjekciju(imeProjekcije, vremeRezervacije, sala, cenaKarte);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Nevažeći unos za cenu karte.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Nevažeći unos za broj sale.");
                                }
                                break;

                            case 2:
                                proxy.IzmeniProjekciju();
                                break;

                            case 3:
                                proxy.IzmeniPopust();
                                break;

                            case 4:
                                proxy.NapraviRezervaciju();
                                break;

                            case 5:
                                proxy.PlatiRezervaciju();
                                break;

                            case 0:
                                Console.WriteLine("Izlaz iz programa.");
                                return;

                            default:
                                Console.WriteLine("Nevažeći izbor. Pokušajte ponovo.");
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Nevažeći unos. Unesite broj za izbor opcije.");
                    }

                    Console.WriteLine(); // Prazan red za bolju preglednost
                }
            }

        }
    }
}
