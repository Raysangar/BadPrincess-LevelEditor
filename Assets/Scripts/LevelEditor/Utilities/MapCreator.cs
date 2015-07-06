using System.Collections;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public static class MapCreator
	{
		public static string createMap(ArrayList entityInfoList)
		{
			string map = "Map = {";
			foreach (EntityInfo entityInfo in entityInfoList)
				map += "\n" + EntityInfoTranslator.toString (entityInfo);
			return (map + "\n}");
		}
	}
}

