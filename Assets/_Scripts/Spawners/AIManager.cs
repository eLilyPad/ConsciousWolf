using System.Collections;
using System.Collections.Generic;
using Lily.Ai;
using UnityEngine;

namespace Lily
{
  public class AIManager : MonoBehaviour
  {
    #region Parameters

		[Header("Entity Data")]
    public Dictionary<int, NPC> npcList = new Dictionary<int, NPC>();

    public List<NPC> spawnableNPCs = new List<NPC>();
    public GameObject entity;

    EntityData entityData;

    private static int _ID = 0;
    public int ID;

    string name;

    #region Spawn Settings
    [SerializeField] int spawnCount;
    [SerializeField] int spawnCapacity;
    [SerializeField] float minTimeBetweenSpawn;
    [SerializeField] float maxTimeBetweenSpawn;
    [SerializeField] Transform[] spawnLocations;
    [SerializeField] float timeBetweenSpawn;

    #endregion

    #endregion

    void Start()
    {
      name = entityData.name;
    }
    void Update()
    {
      // respawns = GameObject.FindGameObjectsWithTag(spawn.tag);
      // spawnCount = respawns.Length;
      // if(spawn.tag.Equals("Pray"))
      // {
      // 	CheckIsAlive();
      // }
      // if (spawnCount <= spawnCapacity)
      // {
      // 	Spawn();
      // }
    }

    public void Spawn()
    {
      StartCoroutine(SpawnDrop());
    }

    IEnumerator SpawnDrop()
    {
      if (CheckSpawnCap()) yield break;

      timeBetweenSpawn = Random.Range(minTimeBetweenSpawn, maxTimeBetweenSpawn);

      CreateEntity();

      // Instantiate(entity, GetSpawnPosition(), Quaternion.identity);
      // new GameObject = 

      yield return new WaitForSeconds(timeBetweenSpawn);

      if (!AtSpawnCap()) Spawn();
			if (IsSpawnable()) Respawn();
    }

		public void Respawn()
    {
      StartCoroutine(RespawnEntityRoutine());
    }

    IEnumerator RespawnEntityRoutine()
    {

      timeBetweenSpawn = Random.Range(minTimeBetweenSpawn, maxTimeBetweenSpawn);

      RespawnEntity();

      // Instantiate(entity, GetSpawnPosition(), Quaternion.identity);
      // new GameObject = 

      yield return new WaitForSeconds(timeBetweenSpawn);

			if (IsSpawnable()) Respawn();
    }

		void RespawnEntity()
		{
			int RespawnID = spawnableNPCs[0].entityID;

			npcList[RespawnID].prefab.transform.position = GetSpawnPosition();

			SetActive(RespawnID);
		}
    //creates entity and changes some properties
    void CreateEntity()
    {
      ID = _ID;

      NPC npc = new NPC();

      entity.TryGetComponent<BasicAI>(out BasicAI ai);

      ai.AIManager = this;

      // spawns a game object with entity
      npc.prefab = Instantiate(entity, GetSpawnPosition(), Quaternion.identity);
      npc.prefab.transform.parent = this.transform;
      npc.entityID = ai.GetInstanceID();
      npc.Name = name;

      npcList.Add(npc.entityID, npc);
    }

    public NPC GetEntityFromList(int entityID)
    {
      return npcList[entityID];
    }

    public void SetActive(int entityID, bool active = true)
    {
      GetEntityFromList(entityID).prefab.SetActive(active);
    }

    public void KillEntity(int entityID)
    {
      SetActive(entityID, false);

      spawnableNPCs.Add(GetEntityFromList(entityID));
    }

    Vector3 GetSpawnPosition()
    {
      int oldSpawnIndex = spawnLocations.Length;
      int spawnIndex = Random.Range(0, spawnLocations.Length);
      if (spawnIndex == oldSpawnIndex)
      {
        spawnIndex = Random.Range(0, spawnLocations.Length);
      }
      return spawnLocations[spawnIndex].position;
    }

    bool AtSpawnCap()
    {
      return spawnCount <= spawnCapacity;
    }
    bool IsSpawnable()
    {
      return spawnableNPCs.Count > 0;
    }

  }
  public class NPC
  {
    public int entityID;

    public string Name;

    public GameObject prefab;
  }
}