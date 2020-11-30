using System.Collections.Generic;
using System.Collections;
using UnityEditor;
using UnityEngine;
using idbrii.lib.inspector;

namespace idbrii.lib.editor
{
    // Usage:
    // [EnumArray(typeof(StatsEnum))]
    // public int[] m_Stats = new int[EnumTool.GetCount<StatsEnum>()];
    //
    // Copied from http://answers.unity.com/answers/1589393/view.html
    [CustomPropertyDrawer(typeof(EnumArrayAttribute))]
    public class EnumArray_Drawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var enum_attr = attribute as EnumArrayAttribute;
            // propertyPath returns something like component_hp_max.Array.data[4]
            // so get the index from there
            var start = property.propertyPath.IndexOf("[");
            var index_str = property.propertyPath.Substring(start).Replace("[", "").Replace("]", "");
            int index = System.Convert.ToInt32(index_str);
            if (index < enum_attr.Labels.Length)
            {
                label.text = enum_attr.Labels[index];
            }
            EditorGUI.PropertyField(position, property, label, true);
        }
    }
}
