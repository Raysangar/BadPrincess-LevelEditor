using UnityEngine;

namespace AssemblyCSharp
{
    public class FloatTranslator : TypeTranslator
    {
        public override string translate(object value)
        {
            return ((float) value).ToString("F6");
        }
    }
}
