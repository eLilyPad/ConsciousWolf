using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    #region Variables
        public GameObject spawn;
        public Transform center;
        public int spawnCount;
        public int spawnCapacity;
        public float minTimeBetweenSpawn;
        public float maxTimeBetweenSpawn;
        public GameObject[] respawns;

        [SerializeField]
        private int xPosMin, xPosMax;
        [SerializeField]
        private int yPosMin, yPosMax;
        [SerializeField]
        private int zPosMin, zPosMax;
        private int xPos, yPos, zPos;
        private float timeBetweenSpawn;

    #endregion
    
	void Update()
	{
		spawnCount = GameObject.FindGameObjectsWithTag("Pray").Length;
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
		xPos = Random.Range(xPosMin, xPosMax);
		yPos = Random.Range(yPosMin, yPosMax);
		zPos = Random.Range(zPosMin, zPosMax);
		Vector3 spawnBorder = center.position + new Vector3(xPos, yPos, zPos);
		timeBetweenSpawn = Random.Range(minTimeBetweenSpawn, maxTimeBetweenSpawn);
		if (spawnCount <= spawnCapacity)
		{
			Instantiate(spawn, spawnBorder, Quaternion.identity);

			yield return new WaitForSeconds(timeBetweenSpawn);
			StartCoroutine(SpawnDrop());
		}

	}
}
