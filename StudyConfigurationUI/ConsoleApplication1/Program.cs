using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ConsoleApplication1.Data;

namespace ConsoleApplication1
{
    class Service
    {

        static void Main()
        {

            var study = new Study()
            {
                Name = "This Study",
                IsFinished = false,
                CurrentStage = 1
            };
            PostStudy(study);
        }

        static async Task RunAsync()
        {
            using (var client = new HttpClient())
            {
                // TODO - Send HTTP requests
            }
        }

        static async Task GetStudy(int StudyId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:6735/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
        }

        static async Task PostStudy(Study study)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:6735/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.PostAsJsonAsync("api/StudyConfiguration", study);
                Console.WriteLine(response.IsSuccessStatusCode);
            }
        }
    }
}
