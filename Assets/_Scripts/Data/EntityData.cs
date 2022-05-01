using UnityEngine;

namespace Lily
{
  [CreateAssetMenu()]
  public class EntityData : UpdatableData
  {

    public string name = "Entity";
    public GameObject prefab;

    public int spawnCapacity;
    public float minTimeBetweenSpawn;
    public float maxTimeBetweenSpawn;
    public Transform[] spawnLocations;

  }
}