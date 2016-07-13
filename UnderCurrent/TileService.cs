using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace UnderCurrent
{
	public class TileService
	{

		public async Task<Tile[]> GetTilesAsync()
		{
			var client = new System.Net.Http.HttpClient();
			client.BaseAddress = new Uri("http://197.89.14.171:777/");
			var response = await client.GetAsync("undercurrentcore/core?secretKey=XnEHlj");
			var json = response.Content.ReadAsStringAsync().Result;
			JObject jsonObject = JObject.Parse(json);
			var success = (bool)jsonObject.SelectToken("status");
			JArray jsonArray = jsonObject["data"].Value<JArray>();
			var tiles = new List<Tile>();
			if (success && jsonArray != null)
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
            client.BaseAddress = new Uri("http://197.89.14.171:777/");
            var response = await client.GetAsync("undercurrentcore/auth?secretKey=XnEHlj");
			var json = response.Content.ReadAsStringAsync().Result;
            JObject jsonObject = JObject.Parse(json);
			return (bool)jsonObject.SelectToken("status");
        }

    }
}

