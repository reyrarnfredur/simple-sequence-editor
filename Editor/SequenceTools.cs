using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SequenceTools
{

    [MenuItem( "GameObject/Sequence/Copy Name", false, 10 )]
    public static void CopyName() {

        GameObject gameObject = Selection.activeGameObject;
        GUIUtility.systemCopyBuffer = gameObject.name;

    }

}
