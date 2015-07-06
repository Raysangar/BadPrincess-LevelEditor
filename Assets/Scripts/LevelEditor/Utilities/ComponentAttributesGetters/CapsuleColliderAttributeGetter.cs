using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class CapsuleColliderAttributeGetter : ComponentAttributesGetter
	{
		public CapsuleColliderAttributeGetter ()
		{
		}
		
		public override Dictionary<string, object> getAttributesFrom(Component component)
		{
			Dictionary<string, object> boxColliderAttributes = new Dictionary<string, object>();
			CapsuleCollider collider = component as CapsuleCollider;
			boxColliderAttributes.Add ("physic_shape", "capsule");
			boxColliderAttributes.Add ("physic_type", component.gameObject.isStatic ? "static" : "dynamic");
			boxColliderAttributes.Add ("physic_radius", collider.radius);
			boxColliderAttributes.Add ("physic_height", collider.height);
			boxColliderAttributes.Add ("physic_trigger", collider.isTrigger);
			boxColliderAttributes.Add ("physic_group", collider.gameObject.layer);
			return boxColliderAttributes;
		}
	}
}
