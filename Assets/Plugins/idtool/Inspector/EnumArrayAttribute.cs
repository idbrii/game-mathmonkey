using UnityEngine;

namespace idbrii.lib.inspector
{
    // http://answers.unity.com/answers/1589393/view.html
    public class EnumArrayAttribute : PropertyAttribute
    {
        public string[] Labels;
        public EnumArrayAttribute(System.Type names_enum_type)
        {
            Labels = System.Enum.GetNames(names_enum_type);
        }
    }
}
