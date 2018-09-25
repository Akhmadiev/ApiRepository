namespace ApiAdditional
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class ApiAdditionalClass
    {
        /// <summary>
        /// Get json data from API
        /// </summary>
        public static async Task<List<T>> GetData<T>(string address) where T : class
        {
            var client = new HttpClient();
            var response = await client.GetAsync(address);

            var data = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<T>>(data);
        }
    }
}
