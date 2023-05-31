using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameObjectMenu : MonoBehaviour
{
    [MenuItem("GameObject/UI/Drag-Drop Item", false, 999)]
    static void CreateDragDropItem(MenuCommand menuCommand)
    {
        GameObject go = new GameObject("Drag-Drop Item");
        go.AddComponent<DragDropItem>();
        go.AddComponent<RectTransform>();
        go.AddComponent<Image>();
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
    }
}