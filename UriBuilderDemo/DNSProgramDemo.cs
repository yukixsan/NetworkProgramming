using System;
using System.Net;
using System.Threading;

namespace UriBuilderDemo
{
    class DNSProgramDemo
    {
        static void Main(string[] args)
        {            
            
            var domainEntry = Dns.GetHostEntry("kompas.com");
  
            Console.WriteLine(domainEntry.HostName);
            foreach (var ip in domainEntry.AddressList)
            {
                Console.WriteLine(ip);
            }

            Console.WriteLine(Dns.GetHostName());
            IPAddress ipadd = IPAddress.Parse("142.251.10.95");
            var ipEntry = Dns.GetHostEntry(ipadd);
            foreach (var ip in ipEntry.AddressList)
            {
                Console.WriteLine(ip);
            }

            Thread.Sleep(10000);

        }
    }
}
