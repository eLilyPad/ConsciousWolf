using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Lily
{
  [CustomEditor(typeof(AIManager))]
  public class AIManagerEditor : Editor {

    public override void OnInspectorGUI() 
    {
      base.OnInspectorGUI();

      if (GUILayout.Button("New Location")) 
      {
        ((AIManager)target).NewLocation();
      }
    }

  }
}
