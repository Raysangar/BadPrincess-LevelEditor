using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public static class ComponentAttributesGettersDictionary
	{
		static ComponentAttributesGettersDictionary ()
		{
			dictionary = new Dictionary<Type, ComponentAttributesGetter> ();
			initDictionary ();
		}

		private static void initDictionary()
		{
			dictionary.Add (typeof(Transform), new TransformAttributesGetter ());
			dictionary.Add (typeof(MonoBehaviour), new ScriptAttributesGetter ());
			dictionary.Add (typeof(BoxCollider), new BoxColliderAttributeGetter ());
			dictionary.Add (typeof(CapsuleCollider), new CapsuleColliderAttributeGetter ());
			dictionary.Add (typeof(Rigidbody), new RigidbodyAttributeGetter ());
		}

		private static Dictionary<System.Type, ComponentAttributesGetter> dictionary;

		public static ComponentAttributesGetter getAttributesGetterFor(System.Type type)
		{
			if (dictionary.ContainsKey (type))
				return dictionary [type];
			if (dictionary.ContainsKey (type.BaseType))
				return dictionary [type.BaseType];
			return null;
		}
	}
}

