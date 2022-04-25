using System.Collections;
using System.Collections.Generic;
using Lily.Ai;
using UnityEngine;

public class AIManager : MonoBehaviour
{
	#region Variables
		[SerializeField] GameObject spawn;
		int spawnCount;
		[SerializeField] int spawnCapacity;
		[SerializeField] float minTimeBetweenSpawn;
		[SerializeField] float maxTimeBetweenSpawn;
		[SerializeField] List<GameObject> spawnLocations = new List<GameObject>();
		private List<GameObject> Spawns = new List<GameObject>();
		private float timeBetweenSpawn;
		int oldSpawnIndex;

	#endregion

	#region Mono Methods
		void Start()
		{
			Spawn();
			Spawns.Add(GameObject.FindGameObjectWithTag(spawn.tag));
		} 
		void Update()
		{
			GetSpawnCount();
		}

		public bool HasSpawn()
		{
			return (Spawns.Count >= 1);
		}

		
	#endregion

	#region AI Checks

	int GetSpawnCount()
	{
		spawnCount = Spawns.Count;
		return spawnCount;
	}

	bool CanSpawn()
	{
		if (GetSpawnCount() <= spawnCapacity) return true;

		return false;
	}
	
	#endregion

	#region Spawn Methods
	void ListSpawn(GameObject spawn)
	{ 
		Spawns.Add(spawn);
	}
	
	void RemoveSpawn(GameObject spawn)
	{ 
		Spawns.Remove(spawn);
	}

	public void Spawn()
	{		
		if (CanSpawn())
		{
			StartCoroutine(SpawnDrop());
		}
	}

	int GetMaxSpawnLocationIndex()
	{
		return spawnLocations.Count;
	}
	int GetRandomSpawnIndex()
	{	
		oldSpawnIndex = spawnLocations.Count + 1;
    int randomSpawnIndex = Random.Range(0, spawnLocations.Count);
		if (randomSpawnIndex == oldSpawnIndex)
    {
      randomSpawnIndex = Random.Range(0, spawnLocations.Count);
    }
		oldSpawnIndex = randomSpawnIndex;
		return randomSpawnIndex;
	}
	IEnumerator SpawnDrop()
	{
		Transform spawnPos = spawnLocations[GetRandomSpawnIndex()].transform;

		timeBetweenSpawn = Random.Range(minTimeBetweenSpawn, maxTimeBetweenSpawn);
		if (spawnCount <= spawnCapacity)
		{
			Instantiate(spawn, spawnPos.position, Quaternion.identity);
			ListSpawn(spawn);
			yield return new WaitForSeconds(timeBetweenSpawn);
			StartCoroutine(SpawnDrop());
		}

	}

	public GameObject GetClosestTarget(Vector3 pos)
	{
		if(Spawns == null) return null;
		float closestDistance = Mathf.Infinity;
		GameObject target = null;

		foreach (GameObject t in Spawns)
		{
			float distanceFromTarget = Vector3.Distance(pos, t.transform.position);
			if (distanceFromTarget < closestDistance)
			{
				target = t;
				closestDistance = distanceFromTarget;
			}
		}
		return target;
	}
	
	public void NewLocation()
	{
		int newSpawnLocationIndex = GetMaxSpawnLocationIndex();
		
		spawnLocations.Add(NewSpawnLocation(newSpawnLocationIndex));
	}

	GameObject NewSpawnLocation(int spawnIndex)
	{	
		GameObject NewSpawn = new GameObject("Spawn Location " + spawnIndex);
		NewSpawn.transform.SetParent(transform);
		return NewSpawn;
	}
  public void Kill(GameObject spawn)
	{
		RemoveSpawn(spawn);
		Destroy(spawn);
	}
	#endregion
	// void CheckIsAlive()
	// {
	// 	foreach(GameObject spawn in Spawns)
	// 	{
	// 		var AI = spawn.GetComponent<BasicAI>();
	// 		if(!AI.IsAlive)
	// 		{
	// 			StartCoroutine(Destroy(spawn));
	// 		}
	// 	}
	// }

	// IEnumerator Destroy(GameObject deadObject)
	// {
	// 	yield return new WaitForSeconds(3);
	// 	Destroy(deadObject);
	// }

}
