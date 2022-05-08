using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Lily
{
  public class GameManager : SingletonMB<GameManager>
  {
    
    [SerializeField] public List<Entity> globalEntities = new List<Entity>();
    [SerializeField] protected List<Entity> globalActiveEntities = new List<Entity>();

    [SerializeField] public EntityData[] entityDataList;

    // List<EntityManager> entityManagers = new List<EntityManager>();
    public Dictionary<string, EntityManager> entityManagers = new Dictionary<string, EntityManager>();
    
    public EntityData FoodData, RabbitData;

    protected override void OnAwake()
    {
      // foreach(EntityData data in entityDataList) entityManagers.Add(InitializeManager(data));
      foreach(EntityData data in entityDataList) entityManagers.Add(data.name, InitializeManager(data));
    }

    void Update()
    {
      FoodData = entityDataList[0];
      RabbitData = entityDataList[1];
      GetGlobalEntities();
    }
    
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
    
    public List<Entity> GetGlobalEntities()
    {
      foreach(EntityData data in entityDataList) globalEntities.AddRange(entityManagers[data.name].GetLocalEntities);

      List<Entity> targets = new List<Entity>();

      return targets;
    }

    public List<Entity> SearchTargetsFromPoint(Vector3 position, float radius = 0)
    {

      List<Entity> targets = GetGlobalEntities();

      foreach (var target in targets)
      {
        if (target == null) continue;

        float distance = Vector3.Distance(position, target.gameObject.transform.position);

        if (distance <= radius || radius == 0) targets.Add(target);
      }

      return targets;
    }
  }
}