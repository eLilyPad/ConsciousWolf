using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Lily
{
    [CustomEditor(typeof(BakeParticleSystemToMesh))]
    public class BakeParticleSystemToMeshEditor : Editor {

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            if (GUILayout.Button("Bake")) {
                ((BakeParticleSystemToMesh)target).SaveAsset();
            }
        }

    }
}
