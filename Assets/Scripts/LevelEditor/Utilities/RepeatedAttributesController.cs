using System.Collections;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class RepeatedAttributesController
	{
		private ArrayList attributes;

		public RepeatedAttributesController ()
		{
			attributes = new ArrayList ();
		}

		public bool isRepeated(string attribute)
		{
			if (attributes.Contains (attribute))
				return true;
			attributes.Add (attribute);
			return false;
		}
	}
}

