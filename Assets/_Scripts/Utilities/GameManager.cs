using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Lily
{
  public class GameManager : MonoBehaviour
  {
    
    [SerializeField] public List<Entity> totalSpawnedEntities = new List<Entity>();
    [SerializeField] protected List<Entity> activeEntities = new List<Entity>();

    [SerializeField] public EntityData[] entityDataList;

    // List<EntityManager> entityManagers = new List<EntityManager>();
    public Dictionary<string, EntityManager> entityManagers = new Dictionary<string, EntityManager>();

    void Awake()
    {
      // foreach(EntityData data in entityDataList) entityManagers.Add(InitializeManager(data));
      foreach(EntityData data in entityDataList) entityManagers.Add(data.name, InitializeManager(data));
    }
    void Start()
    {}

    EntityManager InitializeManager(EntityData data)
    { 
      string managerName = data.name + " Spawner";
      GameObject spawner = new GameObject(managerName);

      spawner.AddComponent<EntityManager>().Initialize(data);

      // spawner.AddComponent<EntityManager>();
      spawner.transform.parent = this.transform;
      
      return spawner.GetComponent<EntityManager>();
    }
    // public void KillAll()
    // {
    //   foreach(GameObject entity in totalSpawnedEntities)
    //   {
    //     if (TryRegisterDeath(entity)) Destroy(entity); 
    //   }
    // }
    
    public List<Entity> SearchTargetsFromPoint(Vector3 position, float radius)
    {
      foreach(EntityData data in entityDataList) activeEntities.AddRange(entityManagers[data.name].GetLocalEntities);

      List<Entity> targets = new List<Entity>();

      foreach (var target in activeEntities)
      {
        if (target == null) continue;

        float distance = Vector3.Distance(position, target.gameObject.transform.position);

        if (distance <= radius) targets.Add(target);
      }

      return targets;
    }
  }
}