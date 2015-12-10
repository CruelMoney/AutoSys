using System;
using System.Diagnostics;
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


        public static async Task<Study> GetStudy(int StudyId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:6735/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    HttpResponseMessage response = await client.GetAsync("api/StudyConfiguration/"+StudyId);
                    response.EnsureSuccessStatusCode();    // Throw if not a success code.
                    return await response.Content.ReadAsAsync<Study>();
                }
                catch (HttpRequestException e)
                {
                    return null;
                }
            }
        }
        public static async Task<Team> GetTeam(int TeamId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:6735/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    HttpResponseMessage response = await client.GetAsync("api/StudyConfiguration/Team/"+TeamId);
                    response.EnsureSuccessStatusCode();    // Throw if not a success code.
                    return await response.Content.ReadAsAsync<Team>();
                }
                catch (HttpRequestException e)
                {
                    return null;
                }
            }
        }

        public static async Task PostStudy(Study study)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:6735/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.PostAsJsonAsync("api/StudyConfiguration", study);
                Debug.WriteLine(response.StatusCode.ToString());
            }
        }
    }
}
