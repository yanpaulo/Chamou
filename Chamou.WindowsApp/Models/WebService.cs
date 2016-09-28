using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace Chamou.WindowsApp.Models
{
    public class WebService
    {
        private static readonly string rootUrl = "http://chamou.yanscorp.com/api/Places";


        public static async Task<IEnumerable<Place>> GetPlaces() =>
            JsonConvert.DeserializeObject<IEnumerable<Place>>(await GetStringData(Action("")));

        public static async Task<Place> GetPlaceByCoordinates(double latitude, double longitude) =>
        JsonConvert.DeserializeObject<Place>(
            await GetStringData(Action($"ByCoordinates?Latitude={FormatDouble(latitude)}&Longitude={FormatDouble(longitude)}")));

        public static async Task<string> CallAttendant(int id, string message) =>
            JsonConvert.DeserializeObject<Dictionary<string, string>>
            (await GetStringData($"http://chamou.yanscorp.com/api/Attendants/{id}?message={WebUtility.UrlEncode(message)}"))
            ["Message"];


        #region Private methods
        private static string Action(string name) => $"{rootUrl}/{name}";

        private static string FormatDouble(double d) => d.ToString().Replace(',', '.');

        private static async Task<string> GetStringData(string url)
        {
            //Create an HTTP client object
            HttpClient httpClient = new HttpClient();

            Uri requestUri = new Uri(url);

            //Send the GET request asynchronously and retrieve the response as a string.
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            string httpResponseBody = "";

            try
            {
                //Send the GET request
                httpResponse = await httpClient.GetAsync(requestUri);
                //httpResponse.EnsureSuccessStatusCode();
                httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                httpResponseBody = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
                throw;
            }

            return httpResponseBody;
        }
        #endregion

    }
}
