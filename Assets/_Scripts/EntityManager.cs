using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lily
{
  public class EntityManager : MonoBehaviour
  {
    #region Parameters
      [SerializeField] protected Dictionary<int, GameObject> entityList = new Dictionary<int, GameObject>();
      [SerializeField] protected List<GameObject> spawnableEntities = new List<GameObject>();

      [SerializeField] public GameObject prefab;
      [SerializeField] public EntityData entityData;

      protected static int _ID = 0;
      public int ID;

      protected string name;

      [SerializeField] protected int spawnCount;
      [SerializeField] protected int spawnCapacity;
      [SerializeField] protected float minTimeBetweenSpawn;
      [SerializeField] protected float maxTimeBetweenSpawn;
      [SerializeField] protected Transform[] spawnLocations;
      protected float timeBetweenSpawn;

    #endregion

    #region [black] Mono Methods
      void Start()
      {
        name = entityData.name;
      }
      void Update()
      {
        if (!AtSpawnCap())
        {
          Spawn();
        }
      }
    #endregion

    #region [blue] Spawn
      protected void Spawn()
      {
        StartCoroutine(SpawnRoutine());

        IEnumerator SpawnRoutine()
        {
          if (AtSpawnCap()) yield break;

          timeBetweenSpawn = Random.Range(minTimeBetweenSpawn, maxTimeBetweenSpawn);

          GameObject spawn = CreateEntity();
          // Debug.Log("Spawning : " + spawn.name);

          yield return new WaitForSeconds(timeBetweenSpawn);

          if (!AtSpawnCap()) Spawn();
          if (IsSpawnable()) Respawn();
        }
      }
      protected GameObject CreateEntity()
      {
        // Debug.Log("Spawning");

        ID = _ID++;

        GameObject spawn = (GameObject)Instantiate(prefab, GetSpawnPosition(), Quaternion.identity);
        spawn.transform.parent = this.transform;

        prefab.TryGetComponent<Entity>(out Entity entity);

        entity.EntityManager = this;
        entity.entityID = ID;
        entity.Name = name;

        AddToList(ID, spawn);

        return spawn;
      }
    #endregion

    #region [red] Respawn
      protected void Respawn()
    {
      StartCoroutine(RespawnEntityRoutine());
      IEnumerator RespawnEntityRoutine()
      {
        timeBetweenSpawn = Random.Range(minTimeBetweenSpawn, maxTimeBetweenSpawn);

        RespawnEntity();

        // Instantiate(entity, GetSpawnPosition(), Quaternion.identity);
        // new GameObject = 

        yield return new WaitForSeconds(timeBetweenSpawn);

        if (IsSpawnable()) Respawn();
      }
    }
      protected void RespawnEntity()
    {
      int RespawnID = GetSpawnableID();

      entityList[RespawnID].transform.position = GetSpawnPosition();

      SetActive(RespawnID);

      spawnableEntities.RemoveAt(0);
    }
      //creates entity and changes some properties

    #endregion
    
    #region [teal] Checks
      public void AddToList(int ID, GameObject spawn)
      {
        // entityList.Add(ID, spawn);
        if (!entityList.TryAdd(ID, spawn)) Debug.Log("Entity not added to list: " + ID);;
      }
      public GameObject GetEntityFromList(int entityID)
      {
        
        GameObject entity = entityList[entityID];
        if (!entityList.TryGetValue(entityID, out GameObject e)) Debug.Log("Entity not found: " + entityID);
        entity = e;
        return entity;
      }

      public int GetSpawnableID()
      {
        spawnableEntities[0].TryGetComponent<Entity>(out Entity entity);

        return entity.entityID;
      }
      public void InstaKill(int entityID)
      {
        SetActive(entityID, false);

        spawnableEntities.Add(GetEntityFromList(entityID));
      }
      public void SetActive(int entityID, bool active = true)
      {
        var entity = GetEntityFromList(entityID);
        
        entity.SetActive(active);
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
        spawnCount = entityList.Count;
        return spawnCount >= spawnCapacity;
      }
      public bool IsSpawnable()
      {
        return spawnableEntities.Count > 0;
      }
    #endregion
  }
}