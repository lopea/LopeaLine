using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

[CustomEditor(typeof(LopeaLine))]
public class LopeaLineEditor : Editor
{
    public override void OnInspectorGUI()
    {
        LopeaLine tar = (LopeaLine)target;
        
    }
}
