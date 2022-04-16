 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
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
    
	void Update()
	{
		spawnCount = GameObject.FindGameObjectsWithTag(spawn.tag).Length;
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
}
