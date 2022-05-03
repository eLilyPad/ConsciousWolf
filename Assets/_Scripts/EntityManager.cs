using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Lily
{
  public class EntityManager : MonoBehaviour
  {
    #region Events
      // [SerializeField] UnityEvent OnDeath;
      // [SerializeField] UnityEvent OnRevive;

    #endregion
    #region Parameters
      protected Dictionary<Entity, bool> entityList = new Dictionary<Entity, bool>();
      [SerializeField] protected List<Entity> localEntities = new List<Entity>();
      [SerializeField] protected List<Entity> spawnableEntities = new List<Entity>();
      public List<Entity> GetLocalEntities
      {
        get {return localEntities;}
      }

      protected GameObject prefab;
      protected EntityData entityData;

      protected static int _ID = 0;
      public int ID;

      protected string name;

      protected int spawnCount;
      protected int spawnCapacity;
      protected float minTimeBetweenSpawn;
      protected float maxTimeBetweenSpawn;
      protected Transform[] spawnLocations;
      protected float timeBetweenSpawn;

      protected string targetTag; 

    #endregion

    #region Setup

      void OnEnable()
      { 
        
      }

      void LoadEvents()
      {
        foreach (Entity target in localEntities)
        {
          target.OnDeath += RegisterDeath;
        }
      }

      public void Initialize(EntityData data)
      {
        entityData = data;
        LoadData(data);
        StartSpawner();
        StartRespawn();
        // LoadEvents();
      }
      void LoadData(EntityData data)
      {
        name = data.name;
        prefab = data.prefab;
        spawnLocations = data.spawnLocations;
        maxTimeBetweenSpawn = data.maxTimeBetweenSpawn;
        minTimeBetweenSpawn = data.minTimeBetweenSpawn;
        spawnCapacity = data.spawnCapacity;
        targetTag = data.TargetTag;
      }
    #endregion

    #region [blue] Spawn

      public void StartSpawner()
      {
        StartCoroutine(SpawnRoutine());
      }

      IEnumerator SpawnRoutine()
      {
        for(int  i = 0; i < spawnCapacity ; i++) 
        {
          if (!AtSpawnCap()) CreateEntity();
          else Debug.Log("Can't spawn entity, Spawn capacity reached.");
          // Debug.Log("Spawning : " + spawn.name);
          
          yield return new WaitForSeconds(GetSpawnTimeFromRange());
        }
      }
      

      protected Entity CreateEntity()
      {
        // Debug.Log("Spawning");

        ID = _ID++;

        GameObject spawn = (GameObject)Instantiate(prefab, GetSpawnPosition(), Quaternion.identity);
        spawn.transform.parent = this.transform;

        Entity entity = spawn.AddComponent<Entity>();

        entity.entityID = ID;
        entity.Name = name;

        
        entity.OnDeath += RegisterDeath;

        RegisterSpawn(entity);

        return entity;
      }
    #endregion

    #region [green] Respawn Entities
      public void StartRespawn()
      {
        StartCoroutine(RespawnRoutine());
      }

      IEnumerator RespawnRoutine()
      {
        while (true)
        {
          if((spawnableEntities?.Count ?? 0) == 0)
          {
            yield return new WaitForSeconds(GetSpawnTimeFromRange());
            continue;
          }
          
          else 
          {
            yield return new WaitForSeconds(GetSpawnTimeFromRange());
            spawnableEntities[0].gameObject.transform.position = GetSpawnPosition();
            spawnableEntities[0].Revive();
          
            RegisterRespawn(spawnableEntities[0]);
          }
          
        }
      }

    

    #endregion

    #region [teal] Checks
    
    void RegisterRespawn(Entity entity)
    {
      // if (spawnableEntities.Contains(entity)) spawnableEntities.Remove(entity);

      // if (!localEntities.Contains(entity)) localEntities.Add(entity);

      if (entityList.ContainsKey(entity)) entityList[entity] = true;
      
    }
    void RegisterSpawn(Entity entity)
      {
        // GameManager.totalSpawnedEntities.Add(spawn);
        // if (!localEntities.Contains(entity)) localEntities.Add(entity);

        if (!entityList.ContainsKey(entity)) entityList.Add(entity, true);

        spawnCount++;
      }

      void RegisterDeath(Entity entity)
      {
        // if (localEntities.Contains(entity)) localEntities.Remove(entity);

        // if (!spawnableEntities.Contains(entity)) spawnableEntities.Add(entity);

        if (entityList.ContainsKey(entity)) entityList.Add(entity, false);
        
        spawnCount--;
        // else Debug.LogWarning("failed to register death: " + entity.name + " is not registered in localEntities");
      }

      public Vector3 GetSpawnPosition()
      {
        int oldSpawnIndex = spawnLocations.Length;
        int spawnIndex = UnityEngine.Random.Range(0, spawnLocations.Length);
        if (spawnIndex == oldSpawnIndex)
        {
          spawnIndex = UnityEngine.Random.Range(0, spawnLocations.Length);
        }
        // Debug.Log("Spawn at" + spawnLocations[spawnIndex].position);
        return spawnLocations[spawnIndex].position;
      }

      public float GetSpawnTimeFromRange()
      {
        return timeBetweenSpawn = UnityEngine.Random.Range(minTimeBetweenSpawn, maxTimeBetweenSpawn);
      }
      
      public bool AtSpawnCap()
      {
        return entityList.Count >= spawnCapacity;
      }

      public float GetSpawnTime()
      {
        return UnityEngine.Random.Range(minTimeBetweenSpawn, maxTimeBetweenSpawn);
      }
      
      // private GameObject SearchTargetsFromPoint(Vector3 center, float radius)
      // {
      //   GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);
      //   GameObject closestTarget = null;
      //   float closestDistance = Mathf.Infinity;

      //   foreach (GameObject target in targets)
      //   {

      //     float distanceFromTarget = Vector3.Distance(ai.transform.position, target.transform.position);
      //     if (distanceFromTarget < closestDistance)
      //     {
      //       closestTarget = target;
      //       closestDistance = distanceFromTarget;
      //       //ai.oldTarget = TheNearestWithTag().name;
      //     }
      //   }

      //   return closestTarget;
      // }

    #endregion
  }
}