using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class TransformAttributesGetter : ComponentAttributesGetter
	{
		public TransformAttributesGetter ()
		{
		}

		public override Dictionary<string, object> getAttributesFrom(Component component)
		{
            Vector3 position = ((UnityEngine.Transform)component).position;
            if (component.gameObject.name == "Castle")
                position.z = -822.0f;

            position.x *= -1;
			Dictionary<string, object> transformAttributes = new Dictionary<string, object>();
			transformAttributes.Add ("position", position );
			transformAttributes.Add ("orientation", ((UnityEngine.Transform)component).rotation.eulerAngles.y);
			return transformAttributes;
		}
	}
}

