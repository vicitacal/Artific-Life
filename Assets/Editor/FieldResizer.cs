using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FieldResizer : EditorWindow
{
    

    [MenuItem("Game/Resize map")]
    public static void Resize()
    {

        MapCreator creator = FindObjectOfType<MapCreator>();
        creator.InitMap();

    }

}
