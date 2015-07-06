using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace AssemblyCSharp
{
	public class ScriptAttributesGetter : ComponentAttributesGetter
	{
        private int[] integerArray = {};
        private string[] stringArray = {};
        private float[] floatArray = {};
        private Vector3[] vector3Array = {};

		private const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;

		public override Dictionary<string, object> getAttributesFrom(Component component)
		{
			Dictionary<string, object> scriptAttributes = new Dictionary<string, object>();
			addScriptFieldsToDictionary (scriptAttributes, component);
			addScriptPropertiesToDictionary (scriptAttributes, component);
			return scriptAttributes;
		}

		private void addScriptFieldsToDictionary(Dictionary<string, object> scriptAttributes, Component component)
		{

            foreach (FieldInfo fieldInfo in component.GetType().GetFields(flags))
            {
                if (fieldInfo.FieldType == integerArray.GetType())
                {
                    integerArray = (int[]) fieldInfo.GetValue(component);
                    scriptAttributes.Add(fieldInfo.Name + "_count", integerArray.Length);
                    for(int i = 0; i < integerArray.Length; ++i)
                        scriptAttributes.Add(fieldInfo.Name + "_" + i, integerArray[i]);
                }
                else if(fieldInfo.FieldType == stringArray.GetType())
                {
                    stringArray = (string[])fieldInfo.GetValue(component);
                    scriptAttributes.Add(fieldInfo.Name + "_count", stringArray.Length);
                    for (int i = 0; i < stringArray.Length; ++i)
                        scriptAttributes.Add(fieldInfo.Name + "_" + i, stringArray[i]);
                }
                else if (fieldInfo.FieldType == floatArray.GetType())
                {
                    floatArray = (float[])fieldInfo.GetValue(component);
                    scriptAttributes.Add(fieldInfo.Name + "_count", floatArray.Length);
                    for (int i = 0; i < floatArray.Length; ++i)
                        scriptAttributes.Add(fieldInfo.Name + "_" + i, floatArray[i]);
                }
                else if (fieldInfo.FieldType == vector3Array.GetType())
                {
                    vector3Array = (Vector3[])fieldInfo.GetValue(component);
                    scriptAttributes.Add(fieldInfo.Name + "_count", vector3Array.Length);
                    for (int i = 0; i < vector3Array.Length; ++i)
                        scriptAttributes.Add(fieldInfo.Name + "_" + i, vector3Array[i]);
                }
                else
                    scriptAttributes.Add(fieldInfo.Name, fieldInfo.GetValue(component));
            }
		}

		private void addScriptPropertiesToDictionary(Dictionary<string, object> scriptAttributes, Component component)
		{
			foreach (PropertyInfo propertyInfo in component.GetType().GetProperties(flags))
				scriptAttributes.Add(propertyInfo.Name, propertyInfo.GetValue(component, null));
		}
	}
}

