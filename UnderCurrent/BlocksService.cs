using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace UnderCurrent
{
	public class BlocksService
	{
		public BlocksService()
		{
		}

		public async Task<Block[]> GetBlocksAsync()
		{
			var client = new System.Net.Http.HttpClient();
			client.BaseAddress = new Uri("http://localhost:777/");
			var response = await client.GetAsync("undercurrentcore/core?secretKey=AUthmt");
			var blocksJson = response.Content.ReadAsStringAsync().Result;
			BlockObject blockObject = new BlockObject();
			if (blocksJson != null)
			{
				blockObject = JsonConvert.DeserializeObject < BlockObject > (blocksJson);
			}
			return blockObject.blocks;
		}

	}
}

