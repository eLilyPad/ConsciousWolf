using System;
using System.Collections;
using System.Collections.Generic;
using Lily.MovementSystem;
using UnityEngine;

namespace Lily
{
  public class Entity : MonoBehaviour, IDamagable, IRevivable
  {
    public static event Action<Entity> OnDeath;
    public EntityManager Manager;
    public int entityID;
    public string Name;
    public Rigidbody rb;

    public FollowPath followPath;

    void Awake()
    {
      followPath = GetComponent<FollowPath>();
      rb = GetComponent<Rigidbody>();
    }
		public int Health= 1;

    public void TakeDamage(int damage)
    {
      Health -= damage;
			if(Health <= 0)Die();
    }
		public virtual void Die()
		{
      // Manager.RegisterDeath(this);
      OnDeath?.Invoke(this);

      this.gameObject.SetActive(false);
		}

    public void Revive(RespawnToken token)
    {
      // if(token != null)
      // {
      //   this.transform.position = token.SpawnLocation;

      //   this.Health = 1;
      //   this.gameObject.SetActive(true);
      // }
      // StartCoroutine(ReviveRoutine(token));
    }

    
  }
}