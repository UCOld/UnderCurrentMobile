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

			var task = client.GetAsync("undercurrentcore/tile?playerName=" + Xamarin.Forms.Application.Current.Properties["username"] + "&secretKey=" + Xamarin.Forms.Application.Current.Properties["password"]);
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
				throw new Exception("Unable to get blocks, " + jsonObject["message"]);
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
			System.Diagnostics.Debug.WriteLine("Response Json: " + json);
			JObject jsonObject = JObject.Parse(json);
			var requestSuccess = (bool)jsonObject.SelectToken("status");
			if (requestSuccess)
			{
				System.Diagnostics.Debug.WriteLine("Request successful " + jsonObject.SelectToken("message"));
				Xamarin.Forms.Application.Current.Properties["username"] = username;
				Xamarin.Forms.Application.Current.Properties["password"] = password;
				return true;
			}
			System.Diagnostics.Debug.WriteLine("Request unsuccessful, " + jsonObject.SelectToken("message"));
			return false;
		}

		public async Task<bool> UpdateBlock(string internalName, string fieldName, string fieldValue)
		{
			var client = new HttpClient();

			var requestMessage = new HttpRequestMessage(HttpMethod.Post, serverUrl + "undercurrentcore/tile");

			JObject postData = new JObject();
			JObject blockObject = new JObject();
			JObject fieldObject = new JObject();
			JArray editedDataArray = new JArray();
			JArray dataArray = new JArray();
			JObject userObject = new JObject();
			userObject.Add("playerName", (string)Xamarin.Forms.Application.Current.Properties["username"]);
			userObject.Add("secretKey", (string)Xamarin.Forms.Application.Current.Properties["password"]);
			blockObject.Add("internalName", internalName);
			fieldObject.Add("fieldName", fieldName);
			fieldObject.Add("fieldValue", fieldValue);
			editedDataArray.Add(fieldObject);
			blockObject.Add("editedData", editedDataArray);
			dataArray.Add(blockObject);      
			postData.Add("user", userObject);
			postData.Add("data", dataArray);

			System.Diagnostics.Debug.WriteLine("Post Json: " + postData);

			requestMessage.Content = new StringContent(postData.ToString(), System.Text.Encoding.UTF8, "application/json");

			var task = client.SendAsync(requestMessage);
			await TimeoutAfter(task, 8000);
			var response = await task;
			var json = response.Content.ReadAsStringAsync().Result;
			System.Diagnostics.Debug.WriteLine("Response Json: " + json);
			JObject jsonObject = JObject.Parse(json);
			var requestSuccess = (bool)jsonObject.SelectToken("status");
			if (requestSuccess)
			{
				System.Diagnostics.Debug.WriteLine("Request successful " + jsonObject.SelectToken("message"));
				return true;
			}
			System.Diagnostics.Debug.WriteLine("Request unsuccessful, " + jsonObject.SelectToken("message"));
			return false;
		}
	}
}

