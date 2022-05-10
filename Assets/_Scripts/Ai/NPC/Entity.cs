using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lily
{
  public class Entity : MonoBehaviour, ITakeDamage
  {
    public static event Action<Entity> OnDeath;
    public int entityID;
    public string Name;
    public Rigidbody rb;

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
    
  }
}