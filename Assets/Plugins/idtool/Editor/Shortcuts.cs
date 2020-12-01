
using UnityEditor;
using UnityEngine;

namespace idbrii.lib.editor
{
    public static class Shortcuts
    {
        // http://answers.unity.com/answers/1011241/view.html
        [MenuItem("GameObject/Group Selected %g")]
        private static void GroupSelected()
        {
            if (!Selection.activeTransform)
                return;
            var go = new GameObject(Selection.activeTransform.name + " Group");
            if (Selection.activeTransform is RectTransform)
            {
                var rect = go.AddComponent<RectTransform>();
                Maximize(rect);
            }
            Undo.RegisterCreatedObjectUndo(go, "Group Selected");

            go.transform.SetParent(Selection.activeTransform.parent, false);
            foreach (var transform in Selection.transforms)
                Undo.SetTransformParent(transform, go.transform, "Group Selected");
            Selection.activeGameObject = go;
        }

        static void Maximize(RectTransform t)
        {
            t.anchorMin = Vector2.zero;
            t.anchorMax = Vector2.one;
            t.pivot     = Vector2.one * 0.5f;
            t.sizeDelta = Vector2.zero;
        }

    }
}
