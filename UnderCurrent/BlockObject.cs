using System;
using Newtonsoft.Json;

namespace UnderCurrent
{
	public class BlockObject
	{
		[JsonProperty("name")]
		public Block[] blocks { get; set; }
	}
}