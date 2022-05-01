using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lily
{
  public class GameManager : MonoBehaviour
  {
    [SerializeField] public List<GameObject> totalSpawnedEntities = new List<GameObject>();
    // [SerializeField] protected List<GameObject> activeEntities = new List<GameObject>();

    [SerializeField] public EntityData[] entityDataList;

    List<EntityManager> entityManagers = new List<EntityManager>();
    void Awake()
    {
      foreach(EntityData data in entityDataList) entityManagers.Add(InitializeManager(data));
    }

    void Start()
    {
      StartAllSpawners();
    }
    EntityManager InitializeManager(EntityData data)
    { 
      string managerName = data.name + " Spawner";
      GameObject spawner = new GameObject(managerName);
      
      spawner.AddComponent<EntityManager>().Initialize(data, this);
      spawner.transform.parent = this.transform;
      
      return spawner.GetComponent<EntityManager>();
    }
    public bool TryRegisterDeath(GameObject entity)
    {
      if (totalSpawnedEntities.Contains(entity)) 
      {
        totalSpawnedEntities.Remove(entity);
        entity.GetComponent<EntityManager>().TryRegisterDeath(entity);
        return true;
      }
      return false;
    }

    public void KillAll()
    {
      foreach(GameObject entity in totalSpawnedEntities)
      {
        if (TryRegisterDeath(entity)) Destroy(entity); 
      }
    }
    void StartAllSpawners()
    {
      foreach(EntityManager manager in entityManagers)
      {
        manager.StartSpawner();
      }
    }
  }
}