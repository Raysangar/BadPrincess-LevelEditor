using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyCSharp
{
    public static class EntityInfoTranslator
    {
        private static string entity;

        public static string toString(EntityInfo entityInfo)
        {
            entity = entityInfo.getName + " = {\n";
            addGenericAttributes(entityInfo);
            addComponentsAttributes(entityInfo);
            return entity + "\n},";
        }

        private static void addGenericAttributes(EntityInfo entityInfo)
        {
            entity += "\ttype = " + translateAttribute(entityInfo.getType) + ",";
            if (entityInfo.getIsPlayer)
                entity += "\n\tisPlayer = " + translateAttribute(entityInfo.getIsPlayer) + ",";
            if (entityInfo.getIsEnemy)
                entity += "\n\tisEnemy = " + translateAttribute(entityInfo.getIsEnemy) + ",";
            if (entityInfo.getIsGenerator)
                entity += "\n\tisGenerator = " + translateAttribute(entityInfo.getIsGenerator) + ",";
        }

        private static void addComponentsAttributes(EntityInfo entityInfo)
        {
            RepeatedAttributesController repeatedAttributesController = new RepeatedAttributesController();
            foreach (Dictionary<string, object> componentAttributes in entityInfo.getComponentsInfo)
            {
                foreach (KeyValuePair<string, object> attribute in componentAttributes)
                {
                    if (!repeatedAttributesController.isRepeated(attribute.Key))
                        entity += "\n\t" + attribute.Key + " = " + TranslatorsDictionary.getTranslatorOf(attribute.Value.GetType()).translate(attribute.Value) + ",";
                    else
                    {
                        resolveConflicts(attribute);
                        Debug.Log("Atributo " + attribute.Key + " esta repetido en el game object " + entityInfo.getName);
                    }
                }
            }
        }

        private static string translateAttribute(object attribute)
        {
            return TranslatorsDictionary.getTranslatorOf(attribute.GetType()).translate(attribute);
        }

        private static void resolveConflicts(KeyValuePair<string, object> attribute)
        {
            if (attribute.Key.Equals("physic_entity") && attribute.Value.Equals("rigid"))
            {
                entity.Replace("physic_entity = \"dynamic\"", "physic_entity = \"rigid\"");
            }
        }
    }
}

