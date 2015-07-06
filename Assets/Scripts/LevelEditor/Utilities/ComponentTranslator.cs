using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
namespace AssemblyCSharp
{
	public static class ComponentTranslator
	{
		static ComponentTranslator ()
		{
			dictionary = new Dictionary<Type, string> ();
			initDictionary ();
		}

		private static void initDictionary()
		{
			dictionary.Add (typeof(BoxCollider), "CPhysicEntity");
			dictionary.Add (typeof(CapsuleCollider), "CPhysicEntity");

		}
	
		private static Dictionary<Type, string> dictionary;

		public static string translate(Type componentType)
		{
			if (componentType.BaseType == typeof(MonoBehaviour))
				return componentType.Name;
			if (dictionary.ContainsKey (componentType))
				return dictionary [componentType];
			return "";
		}
	}
}

