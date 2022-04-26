using UnityEngine;

[CreateAssetMenu()]
public class AISettings : UpdatableData
{
  public string name = "";
  public float baseHealthPoint = 1f;
  public GameObject prefab;
}
