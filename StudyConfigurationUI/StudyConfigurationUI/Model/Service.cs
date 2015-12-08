using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;
using StudyConfigurationUI.Data;


namespace StudyConfigurationUI.Model
{
    class Service
    {
        
        

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
                HttpResponseMessage response = await client.PostAsJsonAsync("api/products", study);
                if (response.IsSuccessStatusCode)
                {
                }
            }
        }
    }
}
