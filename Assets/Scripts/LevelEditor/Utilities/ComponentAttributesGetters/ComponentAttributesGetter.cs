using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AssemblyCSharp
{
		public abstract class ComponentAttributesGetter
		{
			public abstract Dictionary<string, object> getAttributesFrom(Component component);
		}
}

