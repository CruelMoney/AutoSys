using Microsoft.Owin.Hosting;
using System;
using System.Net.Http;
using WebGrease.Configuration;

namespace OwinSelfhostSample
{
    public class Program
    {
        static void Main()
        {
            string baseAddress = "http://localhost:9000/";

            // Start OWIN host 
            using (WebApp.Start<Startup>(url: baseAddress))
            {

                HttpClient client = new HttpClient();
                var response = client.GetAsync(baseAddress + "api/team").Result;

                if (response != null)
                {
                    Console.WriteLine("Information from service: {0}", response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    Console.WriteLine("ERROR: Impossible to connect to service");
                }

                Console.WriteLine();
                Console.WriteLine("Press ENTER to stop the server and close app...");
                Console.ReadLine();
            }



            Console.ReadLine();
        }
    }
}