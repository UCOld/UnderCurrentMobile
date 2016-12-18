using System.Collections.Generic;
using Newtonsoft.Json;

namespace UnderCurrent
{
	public class Block
	{
		[JsonProperty("generalBlockInfo")]
		public GeneralBlockInfo generalBlockInfo { get; set; }

		[JsonProperty("editableFields")]
		public List<EditableField> editableFields { get; set; }

	}
}