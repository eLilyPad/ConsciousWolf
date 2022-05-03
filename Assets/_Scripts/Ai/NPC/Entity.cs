using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lily
{
  public class Entity : MonoBehaviour, IDamagable, IRevivable
  {
    public event Action<Entity> OnDeath;
    public int entityID;
    public string Name;
    public Rigidbody rb;

		public int Health= 1;

    public void TakeDamage(int damage)
    {
      Health -= damage;
			if(Health <= 0)Die();
    }
		public void Die()
		{
      OnDeath?.Invoke(this);

      gameObject.SetActive(false);
		}

    public void Revive(RespawnToken token)
    {
      // EntityManager manager = GameManager.Instance.entityManagers[Name].GetComponent<EntityManager>();
      // StartCoroutine(ReviveRoutine(manager.GetRespawnToken()));
      Debug.Log("try revive");
      StartCoroutine(ReviveRoutine(token));
    }

    IEnumerator ReviveRoutine(RespawnToken token)
    {
      // this.gameObject.transform.position = token.SpawnLocation;
      this.Health = 1;

      yield return new WaitForSeconds(3);
      this.gameObject.SetActive(true);
    }
  }
}