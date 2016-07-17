using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace UnderCurrent
{
    public class TileService
    {

        private String serverUrl = "http://197.89.14.171:777/";
        private String key = "agtiI0";

        public async Task<Tile[]> GetTilesAsync()
        {
            var client = new System.Net.Http.HttpClient();
            client.BaseAddress = new Uri(serverUrl);
            var response = await client.GetAsync("undercurrentcore/core?secretKey=" + key);
            var json = response.Content.ReadAsStringAsync().Result;
            JObject jsonObject = JObject.Parse(json);
            var requestSuccess = (bool)jsonObject.SelectToken("status");
            JArray jsonArray = jsonObject["data"].Value<JArray>();
            var tiles = new List<Tile>();
            if (requestSuccess && jsonArray != null)
            {
                foreach (var x in jsonArray)
                {
                    tiles.Add(x.ToObject<Tile>());
                }
            } else
            {
                throw new Exception(jsonObject["error_message"].ToString());
            }
            return tiles.ToArray();
        }

        public async Task<bool> Authenticate()
        {
            var client = new System.Net.Http.HttpClient();
            client.BaseAddress = new Uri(serverUrl);
            var response = await client.GetAsync("undercurrentcore/auth?secretKey=" + key);
            var json = response.Content.ReadAsStringAsync().Result;
            JObject jsonObject = JObject.Parse(json);
            var requestSuccess = (bool)jsonObject.SelectToken("status");
            if (requestSuccess) { 
                var authObject = jsonObject["data"].Value<JObject>();
                return (bool)authObject.SelectToken("auth");
            } else {
                return false; 
                    };
        }

    }
}

