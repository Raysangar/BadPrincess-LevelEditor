using UnityEngine;

namespace AssemblyCSharp
{	
	public class Vector3Translator : TypeTranslator
	{
		public override string translate(object value)
		{
			Vector3 vector3 = (Vector3) value;
			return "{" + vector3.x + ", " + vector3.y + ", " + vector3.z + "}";
		}
	}
}

