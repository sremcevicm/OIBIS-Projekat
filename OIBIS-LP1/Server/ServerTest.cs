using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Server
{
    internal class ServerTest : ITest
    {
        public void Test()
        {
            Console.WriteLine("Klijent se uspesno konektovao na server");
        }
    }
}
