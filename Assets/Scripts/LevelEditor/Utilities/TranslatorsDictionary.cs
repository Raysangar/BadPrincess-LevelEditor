using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public static class TranslatorsDictionary
	{
		static TranslatorsDictionary ()
		{
			dictionary = new Dictionary<Type, TypeTranslator> ();
			initDictionary ();
		}
		
		private static void initDictionary()
		{
			dictionary.Add (typeof(Vector3), new Vector3Translator ());
			dictionary.Add (typeof(bool), new BoolTranslator ());
			dictionary.Add (typeof(string), new StringTranslator ());
            dictionary.Add(typeof(float), new FloatTranslator());
		}

		private static Dictionary<Type, TypeTranslator> dictionary;


		public static TypeTranslator getTranslatorOf(Type type)
		{
			return (dictionary.ContainsKey (type)) ? 
				dictionary [type] : new DefaultTranslator ();
		}
	}
}

