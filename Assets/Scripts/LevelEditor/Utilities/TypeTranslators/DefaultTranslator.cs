using UnityEngine;

namespace AssemblyCSharp
{	
	public class DefaultTranslator : TypeTranslator
	{
		public override string translate(object value)
		{
			return value.ToString ();
		}
	}
}

