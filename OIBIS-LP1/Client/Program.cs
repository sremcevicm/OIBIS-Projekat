using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            ChannelFactory<ITest> channel = new ChannelFactory<ITest>("Test");

            ITest proxy = channel.CreateChannel();

            proxy.Test();

            Console.WriteLine("Client. " + channel.Endpoint.Address);

            Console.ReadLine();

        }
    }
}
