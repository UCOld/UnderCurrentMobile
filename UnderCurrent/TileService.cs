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
			client.BaseAddress = new Uri("http://197.86.206.188:777/");
			var response = await client.GetAsync("undercurrentcore/core?secretKey=xI0u1F");
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
			}
			return tiles.ToArray();
		}

	}
}

