using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CorParques.Presentacion.WinForms
{
    public static class FormBase<Y> where Y:class
    {
        private static HttpClient client = new HttpClient();

        public static  async Task<Y> GetAsync<T>(string path) where T:class
        {
            Initializer();
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<Y>();
                return result;
            }
            return null;
        }

        public static async Task<Y> PostAsync<T>(string path, T element) where T : class
        {
            Initializer();
            HttpResponseMessage response = await client.PostAsJsonAsync(path, element);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<Y>();
                return result;
            }
            return null;
        }

        public static async Task<bool> PutAsync<T>(string path, T element) where T:class
        {
            Initializer();
            HttpResponseMessage response = await client.PutAsJsonAsync(path, element);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<bool>();
            }
            return false;
        }
        public static async Task<System.Net.HttpStatusCode> DeleteAsync(string path)
        {
            Initializer();
            HttpResponseMessage response = await client.DeleteAsync(path);
            return response.StatusCode;
        }

        private static void  Initializer()
        {
            if (client.BaseAddress == null)
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["UrlService"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
        }
    }
}
