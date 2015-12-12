using System;
using System.Collections.Generic;
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

        public static async Task PostStudy(StudyDTO study)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:6735/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    HttpResponseMessage response = await client.PostAsJsonAsync("api/StudyConfiguration", study);
                    response.EnsureSuccessStatusCode();    // Throw if not a success code.
                    return;
                }
                catch (HttpRequestException e)
                {
                    
                }
            }
        }

        public static async Task UpdateStudy(StudyDTO study)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:6735/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    HttpResponseMessage response = await client.PutAsJsonAsync("api/StudyConfiguration", study);
                    response.EnsureSuccessStatusCode();    // Throw if not a success code.
                    return;
                }
                catch (HttpRequestException e)
                {

                }
            }
        }
        public static async Task<StudyDTO> GetStudy(int StudyId)
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
                    return await response.Content.ReadAsAsync<StudyDTO>();
                }
                catch (HttpRequestException e)
                {
                    return null;
                }
            }
        }

        public static async Task<UserDTO[]> GetUsers(int[] IDs)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:6735/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    List<UserDTO> toReturn = new List<UserDTO>();
                    foreach (int UserId in IDs)
                    {
                        HttpResponseMessage response = await client.GetAsync("api/User/" + UserId);
                        response.EnsureSuccessStatusCode();    // Throw if not a success code.
                        toReturn.Add(await response.Content.ReadAsAsync<UserDTO>()); 
                    }
                    return toReturn.ToArray();

                }
                catch (HttpRequestException e)
                {
                    return null;
                }
            }
        }

        public static async Task<TeamDTO> GetTeam(int TeamId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:6735/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    HttpResponseMessage response = await client.GetAsync("api/Team/"+TeamId);
                    response.EnsureSuccessStatusCode();    // Throw if not a success code.
                    return await response.Content.ReadAsAsync<TeamDTO>();
                }
                catch (HttpRequestException e)
                {
                    return null;
                }
            }
        }

        public static async Task<UserDTO> GetUser(int UserId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:6735/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    HttpResponseMessage response = await client.GetAsync("api/User/" + UserId);
                    response.EnsureSuccessStatusCode();    // Throw if not a success code.
                    return await response.Content.ReadAsAsync<UserDTO>();
                }
                catch (HttpRequestException e)
                {
                    return null;
                }
            }
        }

        /*public static async Task PostStudy(Study study)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:6735/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.PostAsJsonAsync("api/StudyConfiguration", study);
                Debug.WriteLine(response.StatusCode.ToString());
            }
        }*/
    }
}
