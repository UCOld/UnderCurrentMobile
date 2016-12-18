using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace UnderCurrent
{
	public class CoreService
	{

		string serverUrl = "http://192.168.1.6:777/";

		public async Task<Block[]> GetBlocksAsync()
		{
			var client = new HttpClient();
			client.BaseAddress = new Uri(serverUrl);

			var task = client.GetAsync("undercurrentcore/tile?playerName=" + App.Current.Properties["username"] + "&secretKey=" + App.Current.Properties["password"]);
			await TimeoutAfter(task, 8000);
			var response = await task;
			var json = response.Content.ReadAsStringAsync().Result;
			JObject jsonObject = JObject.Parse(json);
			System.Diagnostics.Debug.WriteLine(jsonObject);
			var requestSuccess = (bool)jsonObject.SelectToken("status");
			var blocks = new List<Block>();
			if (requestSuccess)
			{
				JArray jsonArray = jsonObject["data"].Value<JArray>();

				foreach (var x in jsonArray)
				{
					blocks.Add(x.ToObject<Block>());
				}
			}
			else
			{
				throw new Exception("Unable to get blocks, " + jsonObject["message"].ToString());
			}
			return blocks.ToArray();
		}

		private async Task TimeoutAfter(Task task, int millisecondsTimeout)
		{
			if (task == await Task.WhenAny(task, Task.Delay(millisecondsTimeout)))
				await task;
			else
				throw new TimeoutException();
		}

		public async Task<bool> Authenticate(string username, string password)
		{
			System.Diagnostics.Debug.WriteLine("Username is: " + username);
			System.Diagnostics.Debug.WriteLine("Password is: " + password);
			var client = new HttpClient();

			var requestMessage = new HttpRequestMessage(HttpMethod.Post, serverUrl + "undercurrentcore/player");

			JObject postData = new JObject();
			JObject userObject = new JObject();
			userObject.Add("playerName", username);
			userObject.Add("secretKey", password);

			postData.Add("user", userObject);

			requestMessage.Content = new StringContent(postData.ToString(), System.Text.Encoding.UTF8, "application/json");

			var task = client.SendAsync(requestMessage);
			await TimeoutAfter(task, 8000);
			var response = await task;
			var json = response.Content.ReadAsStringAsync().Result;
			System.Diagnostics.Debug.WriteLine("Json: " + json);
			JObject jsonObject = JObject.Parse(json);
			var requestSuccess = (bool)jsonObject.SelectToken("status");
			if (requestSuccess)
			{
				System.Diagnostics.Debug.WriteLine("Request successful " + jsonObject.SelectToken("message"));
				App.Current.Properties["username"] = username;
				App.Current.Properties["password"] = password;
				return true;
			}
			else {
				System.Diagnostics.Debug.WriteLine("Request unsuccessful, " + jsonObject.SelectToken("message"));
				return false;
			}
		}
	}
}

