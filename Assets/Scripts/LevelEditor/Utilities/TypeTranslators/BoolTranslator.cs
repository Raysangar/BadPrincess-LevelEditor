using UnityEngine;

namespace AssemblyCSharp
{	
	public class BoolTranslator : TypeTranslator
	{
		public override string translate(object value)
		{
			return value.ToString().ToLower();
		}
	}
}