using System.Collections.Generic;
using Newtonsoft.Json;

namespace UnderCurrent
{
	public class TileDefinition
	{

		[JsonProperty("collections")]
		public List<Collection> collections { get; set; }
	}
}