using UnityEngine;

namespace AssemblyCSharp
{	
	public class StringTranslator : TypeTranslator
	{
		public override string translate(object value)
		{
			return "\"" + value + "\"";
		}
	}
}

