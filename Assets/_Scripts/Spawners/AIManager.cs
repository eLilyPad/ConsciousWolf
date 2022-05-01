using System.Collections;
using System.Collections.Generic;
using Lily.Ai;
using UnityEngine;

namespace Lily
{
  public class AIManager : EntityManager
  {

    // #region [black] Mono Methods
    // void Start()
    //   {
    //     name = entityData.name;
    //   }
    //   void Update()
    //   {
    //     // respawns = GameObject.FindGameObjectsWithTag(spawn.tag);
    //     // spawnCount = respawns.Length;
    //     // if(spawn.tag.Equals("Pray"))
    //     // {
    //     // 	CheckIsAlive();
    //     // }
    //     if (!AtSpawnCap())
    //     {
    //       Spawn();
    //     }
    //   }
    // #endregion

    // #region [blue] Spawn
    //   protected void Spawn()
    //   {
    //     StartCoroutine(SpawnRoutine());

    //     IEnumerator SpawnRoutine()
    //     {
    //       // Debug.Log("AtSpawnCap: " + AtSpawnCap());
    //       if (AtSpawnCap()) yield break;

    //       timeBetweenSpawn = Random.Range(minTimeBetweenSpawn, maxTimeBetweenSpawn);

    //       GameObject spawn = CreateNPC();

    //       yield return new WaitForSeconds(timeBetweenSpawn);

    //       if (!AtSpawnCap()) Spawn();
    //     }
    //   }
    //   GameObject CreateNPC()
    //   {
    //     ID = _ID++;
        
    //     GameObject spawn = (GameObject)Instantiate(prefab, GetSpawnPosition(), Quaternion.identity);
    //     spawn.transform.parent = this.transform;

    //     prefab.TryGetComponent<BasicAI>(out BasicAI ai);

    //     // ai.AIManager = this;
    //     ai.EntityManager = this;
    //     ai.entityID = ID;
    //     ai.Name = name;

    //     AddToList(ai.entityID, spawn);

    //     return spawn;
    //   }
    // #endregion

    // #region [red] Respawn
    // // public void Respawn()
    // // {
    // //   StartCoroutine(RespawnEntityRoutine());
    // //   IEnumerator RespawnEntityRoutine()
    // //   {
    // //     timeBetweenSpawn = Random.Range(minTimeBetweenSpawn, maxTimeBetweenSpawn);

    // //     RespawnEntity();

    // //     // Instantiate(entity, GetSpawnPosition(), Quaternion.identity);
    // //     // new GameObject = 

    // //     yield return new WaitForSeconds(timeBetweenSpawn);

    // //     if (IsSpawnable()) Respawn();
    // //   }
    // // }
    // // void RespawnEntity()
    // // {
    // //   int RespawnID = GetSpawnableID();

    // //   entityList[RespawnID].transform.position = GetSpawnPosition();

    // //   SetActive(RespawnID);

    // //   spawnableEntities.RemoveAt(0);
    // // }
    // // creates entity and changes some properties

    // #endregion
  }
}