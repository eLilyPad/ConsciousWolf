using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lily
{
  public class EntityManager : MonoBehaviour
  {
    #region Parameters
      // protected Dictionary<int, GameObject> entityList = new Dictionary<int, GameObject>();
      // [SerializeField] protected List<GameObject> spawnedEntities = new List<GameObject>();
      // [SerializeField] protected List<GameObject> spawnableEntities = new List<GameObject>();

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

      GameManager GameManager;

    #endregion

    #region Setup

      public void Initialize(EntityData data, GameManager gameManager)
      {
        GameManager = gameManager;
        entityData = data;
        LoadData(data);
      }
      void LoadData(EntityData data)
      {
        name = data.name;
        prefab = data.prefab;
        spawnLocations = data.spawnLocations;
        maxTimeBetweenSpawn = data.maxTimeBetweenSpawn;
        minTimeBetweenSpawn = data.minTimeBetweenSpawn;
        spawnCapacity = data.spawnCapacity;
      }
    #endregion

    #region [blue] Spawn
      public GameObject SpawnSingle(GameManager gameManager)
      {
        return CreateEntity(gameManager);
      }

      public void StartSpawner()
      {
        StartCoroutine(SpawnRoutine());
      }

      IEnumerator SpawnRoutine()
      {
        while (true)
        {
          if (!AtSpawnCap()) CreateEntity(GameManager);
          else Debug.Log("Can't spawn entity, Spawn capacity reached.");
          // Debug.Log("Spawning : " + spawn.name);
          
          timeBetweenSpawn = Random.Range(minTimeBetweenSpawn, maxTimeBetweenSpawn);
          yield return new WaitForSeconds(timeBetweenSpawn);
        }
      }
      

      protected GameObject CreateEntity(GameManager gM)
      {
        // Debug.Log("Spawning");

        ID = _ID++;

        GameObject spawn = (GameObject)Instantiate(prefab, GetSpawnPosition(), Quaternion.identity);
        spawn.transform.parent = this.transform;

        prefab.TryGetComponent<Entity>(out Entity entity);

        // entity.transform.parent = this.transform;

        entity.EntityManager = this;
        entity.GameManager = gM;
        entity.entityID = ID;
        entity.Name = name;

        RegisterSpawn(spawn);
        

        return spawn;
      }
    #endregion
    
    #region [teal] Checks
      public void RegisterSpawn(GameObject spawn)
      {
        GameManager.totalSpawnedEntities.Add(spawn);
        spawnCount++;
      }

      public bool TryRegisterDeath(GameObject entity)
      {
        if (GameManager.totalSpawnedEntities.Contains(entity)) 
        {
          GameManager.totalSpawnedEntities.Remove(entity);
          spawnCount--;
          return true;
        }
        return false;
      }

      public Vector3 GetSpawnPosition()
      {
        int oldSpawnIndex = spawnLocations.Length;
        int spawnIndex = Random.Range(0, spawnLocations.Length);
        if (spawnIndex == oldSpawnIndex)
        {
          spawnIndex = Random.Range(0, spawnLocations.Length);
        }
        return spawnLocations[spawnIndex].position;
      }
      public bool AtSpawnCap()
      {
        return spawnCount >= spawnCapacity;
      }

      public float GetSpawnTime()
      {
        return Random.Range(minTimeBetweenSpawn, maxTimeBetweenSpawn);
      }
    #endregion
  }
}