using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using StudyConfigurationUILibrary.Data;

namespace StudyConfigurationUIibrary.Model
{
    internal class Service
    {
        public static async Task PostStudy(StudyDTO study)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:6735/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    var response = await client.PostAsJsonAsync("api/StudyConfiguration", study);
                    response.EnsureSuccessStatusCode(); // Throw if not a success code.
                }
                catch (HttpRequestException)
                {
                }
            }
        }

        public static async Task UpdateStudy(int id, StudyDTO study)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:6735/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    var response = await client.PutAsJsonAsync("api/StudyConfiguration/" + id, study);
                    response.EnsureSuccessStatusCode(); // Throw if not a success code.
                }
                catch (HttpRequestException)
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
                    var response = await client.GetAsync("api/StudyConfiguration/" + StudyId);
                    response.EnsureSuccessStatusCode(); // Throw if not a success code.
                    return await response.Content.ReadAsAsync<StudyDTO>();
                }
                catch (HttpRequestException)
                {
                    return null;
                }
            }
        }

        public static async Task<bool> RemoveStudy(int StudyId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:6735/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    var response = await client.DeleteAsync("api/StudyConfiguration/" + StudyId);
                    response.EnsureSuccessStatusCode(); // Throw if not a success code.
                    return true;
                }
                catch (HttpRequestException)
                {
                    return false;
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
                    var toReturn = new List<UserDTO>();
                    foreach (var UserId in IDs)
                    {
                        var response = await client.GetAsync("api/User/" + UserId);
                        response.EnsureSuccessStatusCode(); // Throw if not a success code.
                        toReturn.Add(await response.Content.ReadAsAsync<UserDTO>());
                    }
                    return toReturn.ToArray();
                }
                catch (HttpRequestException)
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
                    var response = await client.GetAsync("api/Team/" + TeamId);
                    response.EnsureSuccessStatusCode(); // Throw if not a success code.
                    return await response.Content.ReadAsAsync<TeamDTO>();
                }
                catch (HttpRequestException)
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
                    var response = await client.GetAsync("api/User/" + UserId);
                    response.EnsureSuccessStatusCode(); // Throw if not a success code.
                    return await response.Content.ReadAsAsync<UserDTO>();
                }
                catch (HttpRequestException)
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