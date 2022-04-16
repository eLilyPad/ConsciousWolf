using System.Collections;
using System.Collections.Generic;
using Lily.Ai;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    #region Variables
        public GameObject spawn;
        public int spawnCount;
        public int spawnCapacity;
        public float minTimeBetweenSpawn;
        public float maxTimeBetweenSpawn;
        public GameObject[] respawns;

        [SerializeField]
        private Transform[] spawnLocations;
        private float timeBetweenSpawn;

    #endregion


	void Start()
	{
	} 
	void Update()
	{
		
		respawns = GameObject.FindGameObjectsWithTag(spawn.tag);
		spawnCount = respawns.Length;
		if(spawn.tag.Equals("Pray"))
		{
			CheckIsAlive();
		}
		if (spawnCount <= spawnCapacity)
		{
			Spawn();
		}
	}

	public void Spawn()
	{
		StartCoroutine(SpawnDrop());
	}

	IEnumerator SpawnDrop()
	{
    int oldSpawnIndex = spawnLocations.Length + 1;
    int spawnIndex = Random.Range(0, spawnLocations.Length);
    if (spawnIndex == oldSpawnIndex)
    {
      spawnIndex = Random.Range(0, spawnLocations.Length);
    }
		Transform spawnPos = spawnLocations[spawnIndex];

		timeBetweenSpawn = Random.Range(minTimeBetweenSpawn, maxTimeBetweenSpawn);
		if (spawnCount <= spawnCapacity)
		{
			Instantiate(spawn, spawnPos.position, Quaternion.identity);
      oldSpawnIndex = spawnIndex;
			yield return new WaitForSeconds(timeBetweenSpawn);
			StartCoroutine(SpawnDrop());
		}

	}

	void CheckIsAlive()
	{
		foreach(GameObject spawn in respawns)
		{
			var AI = spawn.GetComponent<BasicAI>();
			if(!AI.IsAlive)
			{
				StartCoroutine(Destroy(spawn));
			}
		}
	}

	IEnumerator Destroy(GameObject deadObject)
	{
		yield return new WaitForSeconds(3);
		Destroy(deadObject);
	}
}
