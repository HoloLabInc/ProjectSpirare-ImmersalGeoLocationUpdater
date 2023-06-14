using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ImmersalGeoLocationUpdater
{
    internal class ImmersalClient
    {
        private const string immersalApiUrl = "https://api.immersal.com";
        private const string setMapMetadataEndpoint = "/metadataset";

        private readonly HttpClient httpClient = new HttpClient();

        public async Task<bool> SetMapMetadataAsync(SetMapMetadataRequest request)
        {
            var url = $"{immersalApiUrl}{setMapMetadataEndpoint}";

            var options = new JsonSerializerOptions { IncludeFields = true };
            var json = JsonSerializer.Serialize(request, options);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            Console.WriteLine($"Request: {json}\n");

            var response = await httpClient.PostAsync(url, content);

            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Successfully updated!");
                return true;
            }

            Console.WriteLine($"Update failed");
            Console.WriteLine($"Response: {responseContent}");
            return false;
        }
    }

    internal class SetMapMetadataRequest
    {
        public int id;
        public string token = "";

        public double latitude;
        public double longitude;
        public double altitude;

        public double tx;
        public double ty;
        public double tz;

        public double qx;
        public double qy;
        public double qz;
        public double qw;
    }
}
