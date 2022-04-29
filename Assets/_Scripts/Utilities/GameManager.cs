using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lily
{
  public class GameManager : MonoBehaviour
  {
    [SerializeField] protected List<GameObject> spawnedEntities = new List<GameObject>();
    [SerializeField] protected List<GameObject> spawnableEntities = new List<GameObject>();

    [SerializeField] public EntityData[] entityDataList;

    EntityManager entityManager;
    void Start()
    {
      foreach (EntityData data in entityDataList)
      {
        spawnedEntities.AddRange(entityManager.SpawnList(data));
      }
    }
    void Update()
    {
      foreach (EntityData Data in entityDataList)
      {

      }
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
  }
}