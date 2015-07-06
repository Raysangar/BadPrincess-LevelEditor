using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class BoxColliderAttributeGetter : ComponentAttributesGetter
	{
		public BoxColliderAttributeGetter ()
		{
		}
	
		public override Dictionary<string, object> getAttributesFrom(Component component)
		{
			Dictionary<string, object> boxColliderAttributes = new Dictionary<string, object>();
			BoxCollider collider = component as BoxCollider;
			boxColliderAttributes.Add ("physic_shape", "box");
			boxColliderAttributes.Add ("physic_dimensions", Vector3.Scale (collider.size/2, component.gameObject.transform.lossyScale));
			boxColliderAttributes.Add ("physic_trigger", collider.isTrigger);
			boxColliderAttributes.Add ("physic_group", collider.gameObject.layer);
			return boxColliderAttributes;
		}
	}
}

