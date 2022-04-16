using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

namespace Lily
{

    [RequireComponent(typeof(ParticleSystem))]
    public class BakeParticleSystemToMesh : MonoBehaviour {

        public string folderPath = "Meshes";
        public string fileName = "NewBakedParticleSystemMesh";
        public bool keepVertexColors = true;

        public enum NormalType {
            KeepNormals,
            NormalizedVertexPosition,
            ClearNormals
        }

        public NormalType handleNormals;

    #if UNITY_EDITOR
        [ContextMenu("Bake To Mesh Asset")]
        public void SaveAsset() {
            // Bake
            Mesh mesh = new Mesh();
            GetComponent<ParticleSystemRenderer>().BakeMesh(mesh, true);
            if (!keepVertexColors)
                mesh.colors32 = null;
            switch (handleNormals) {
                case NormalType.KeepNormals:
                    break;
                case NormalType.NormalizedVertexPosition:
                    Vector3[] normals = mesh.vertices;
                    int length = normals.Length;
                    for (int i = 0; i < length; i++) {
                        normals[i] = normals[i].normalized;
                    }
                    mesh.normals = normals;
                    break;
                default:
                case NormalType.ClearNormals:
                    mesh.normals = null;
                    break;
            }

            // Setup Path
            string fileName = Path.GetFileNameWithoutExtension(this.fileName) + ".asset";
            Directory.CreateDirectory("Assets/" + folderPath);
            string assetPath = "Assets/" + folderPath + "/" + fileName;

            // Create / Override Asset
            Object existingAsset = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
            if (existingAsset == null) {
                AssetDatabase.CreateAsset(mesh, assetPath);
            } else {
                if (existingAsset is Mesh)
                    (existingAsset as Mesh).Clear();
                EditorUtility.CopySerialized(mesh, existingAsset);
            }
            AssetDatabase.SaveAssets();
        }
    #endif
    }
}
