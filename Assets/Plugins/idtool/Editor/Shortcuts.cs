
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
            Undo.RegisterCreatedObjectUndo(go, "Group Selected");
            // TODO: Set recttransform to fill parent if applicable
            go.transform.SetParent(Selection.activeTransform.parent, false);
            foreach (var transform in Selection.transforms)
                Undo.SetTransformParent(transform, go.transform, "Group Selected");
            Selection.activeGameObject = go;
        }

    }
}
