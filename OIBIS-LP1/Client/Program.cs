﻿using Common;
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
                                Console.WriteLine("Enter imeProjekcije:");
                                string imeProjekcije1 = Console.ReadLine();
                                Console.WriteLine("Enter vremeProjekcije (e.g., '2023-12-11T14:30:00'):");
                                DateTime vremeProjekcije1 = DateTime.Parse(Console.ReadLine());
                                Console.WriteLine("Enter sala:");
                                int sala1 = int.Parse(Console.ReadLine());
                                Console.WriteLine("Enter cenaKarte:");
                                double cenaKarte1 = double.Parse(Console.ReadLine());

                                proxy.DodajProjekciju(imeProjekcije1, vremeProjekcije1, sala1, cenaKarte1);
                                break;

                            case 2:
                                Console.WriteLine("Enter imeProjekcije:");
                                string imeProjekcije2 = Console.ReadLine();
                                Console.WriteLine("Enter novoVremeProjekcije (e.g., '2023-12-11T15:30:00'):");
                                DateTime novoVremeProjekcije2 = DateTime.Parse(Console.ReadLine());
                                Console.WriteLine("Enter novaSala:");
                                int novaSala2 = int.Parse(Console.ReadLine());
                                Console.WriteLine("Enter novaCenaKarte:");
                                double novaCenaKarte2 = double.Parse(Console.ReadLine());

                                proxy.IzmeniProjekciju(imeProjekcije2, novoVremeProjekcije2, novaSala2, novaCenaKarte2);
                                break;

                            case 3:
                                Console.WriteLine("Enter noviPopust:");
                                int noviPopust3 = int.Parse(Console.ReadLine());
                                proxy.IzmeniPopust(noviPopust3);
                                break;

                            case 4:
                                Console.WriteLine("Enter imeProjekcije:");
                                string imeProjekcije4 = Console.ReadLine();
                                Console.WriteLine("Enter brojKarata:");
                                int brojKarata4 = int.Parse(Console.ReadLine());

                                proxy.NapraviRezervaciju(imeProjekcije4, brojKarata4);
                                break;

                            case 5:
                                Console.WriteLine("Enter idRezervacije:");
                                string idRezervacije5 = Console.ReadLine();
                                proxy.PlatiRezervaciju(idRezervacije5);
                                break;

                            default:
                                Console.WriteLine("Invalid choice. Please enter a number between 1 and 5.");
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
