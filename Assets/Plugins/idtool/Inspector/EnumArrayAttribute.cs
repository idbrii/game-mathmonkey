using UnityEngine;

namespace idbrii.lib.editor
{
    // http://answers.unity.com/answers/1589393/view.html
    public class EnumNamedArrayAttribute : PropertyAttribute
    {
        public string[] names;
        public EnumNamedArrayAttribute(System.Type names_enum_type)
        {
            this.names = System.Enum.GetNames(names_enum_type);
        }
    }
}
