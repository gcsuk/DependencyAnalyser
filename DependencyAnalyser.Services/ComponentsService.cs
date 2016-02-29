using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace DependencyAnalyser.Services
{
    public class ComponentsService : IComponentsService
    {
        private readonly string _apiUrl;

        public ComponentsService(string apiUrl)
        {
            _apiUrl = apiUrl;
        }

        public IEnumerable<Models.Component> GetList()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.GetAsync("api/Components").Result;

                return response.IsSuccessStatusCode
                    ? response.Content.ReadAsAsync<IEnumerable<Models.Component>>().Result
                    : null;
            }
        }

        public Models.Component GetItem(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.GetAsync($"api/Components/{id}").Result;

                return response.IsSuccessStatusCode ? response.Content.ReadAsAsync<Models.Component>().Result : null;
            }
        }

        public Models.Component Add(Models.Component component)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.PutAsync("api/Components", component, new JsonMediaTypeFormatter()).Result;

                return response.IsSuccessStatusCode ? response.Content.ReadAsAsync<Models.Component>().Result : null;
            }
        }

        public Models.Component Update(Models.Component component)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.PostAsync("api/Components", component, new JsonMediaTypeFormatter()).Result;

                return response.IsSuccessStatusCode ? response.Content.ReadAsAsync<Models.Component>().Result : null;
            }
        }

        public bool Delete(Models.Component component)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                return client.DeleteAsync($"api/Components/{component.Id}").Result.IsSuccessStatusCode;
            }
        }
    }
}
