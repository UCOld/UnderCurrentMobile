using System;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace UnderCurrent
{
	public class GeneralBlockInfo
	{
		[JsonProperty("internalName")]
		public string internalName { get; set; }

		[JsonProperty("name")]
		public string name { get; set; }

		[JsonProperty("xCoord")]
		public int xCoord { get; set; }

		[JsonProperty("yCoord")]
		public int yCoord { get; set; }

		[JsonProperty("zCoord")]
		public int zCoord { get; set; }

		[JsonProperty("dim")]
		public int dim { get; set; }

		[JsonProperty("dimName")]
		public string dimName { get; set; }
	}
}

