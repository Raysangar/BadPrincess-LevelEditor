using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class RigidbodyAttributeGetter : ComponentAttributesGetter
	{
		public RigidbodyAttributeGetter ()
		{
		}
		
		public override Dictionary<string, object> getAttributesFrom(Component component)
		{
			Dictionary<string, object> rigidbodyAttributes = new Dictionary<string, object>();
			Rigidbody rigidbody = component as Rigidbody;

            string physicType = component.gameObject.isStatic ? "static" : "dynamic";

            rigidbodyAttributes.Add("physic_type", rigidbody.isKinematic ? "kinematic" : physicType);            
			rigidbodyAttributes.Add ("physic_entity", "rigid");
			rigidbodyAttributes.Add ("physic_mass", rigidbody.mass);

			return rigidbodyAttributes;
		}
	}
}