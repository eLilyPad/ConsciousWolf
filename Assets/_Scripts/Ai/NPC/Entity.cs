using System;
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

    public void Revive()
    {
      this.gameObject.SetActive(true);
      this.Health = 1;
    }
  }
}